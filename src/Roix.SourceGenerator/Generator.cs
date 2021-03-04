using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Roix.SourceGenerator.Extensions;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Roix.SourceGenerator
{
    // https://github.com/ufcpp/ValueChangedGenerator/
    class Generator
    {
        public CompilationUnitSyntax GeneratePartialDeclaration(INamedTypeSymbol container, StructDeclarationSyntax structDecl)
        {
            var recordDecl = (StructDeclarationSyntax)structDecl.ChildNodes().First(x => x is StructDeclarationSyntax);

            var def = new RecordDefinition(structDecl, recordDecl);
            var generatedNodes = GetGeneratedNodes(def).ToArray();

            var newStructDecl = container.GetContainingTypesAndThis()
                .Select((type, i) => i == 0
                    ? StructDeclaration(type.Name).GetPartialTypeDelaration().AddMembers(generatedNodes)
                    : StructDeclaration(type.Name).GetPartialTypeDelaration())
                .Aggregate((a, b) => b.AddMembers(a));

            var ns = container.ContainingNamespace.FullName().NullIfEmpty();

            MemberDeclarationSyntax topDecl = ns is not null
                ? NamespaceDeclaration(IdentifierName(ns)).AddMembers(newStructDecl)
                : newStructDecl;

            var root = (CompilationUnitSyntax)structDecl.SyntaxTree.GetRoot();

            return CompilationUnit()
                .AddUsings(WithComponentModel(root.Usings))
                .AddMembers(topDecl)
                .WithLeadingTrivia(CreateNullableTrivia())
                .WithTrailingTrivia(CarriageReturnLineFeed)
                .NormalizeWhitespace();
        }

        // https://github.com/dotnet/roslyn/blob/master/src/Features/CSharp/Portable/MetadataAsSource/CSharpMetadataAsSourceService.cs#L137
        private static SyntaxTrivia[] CreateNullableTrivia()
            => new[] { Trivia(NullableDirectiveTrivia(Token(SyntaxKind.EnableKeyword), isActive: true)), CarriageReturnLineFeed };

        private UsingDirectiveSyntax[] WithComponentModel(IEnumerable<UsingDirectiveSyntax> usings) => usings.ToArray();

        private IEnumerable<MemberDeclarationSyntax> GetGeneratedNodes(RecordDefinition def)
        {
            yield return CSharpSyntaxTree.ParseText(
                $@"        private readonly {Consts.SourceValuesStruct} _values;
")
                .GetRoot().ChildNodes()
                .OfType<MemberDeclarationSyntax>()
                .First()
                .WithTrailingTrivia(CarriageReturnLineFeed, CarriageReturnLineFeed);

            // Properties
            foreach (var p in def.Properties)
                foreach (var s in WithTrivia(GetGeneratedMember(p), p.LeadingTrivia, p.TrailingTrivia))
                    yield return s;

            if (!def.IsConstructorDeclared)
            {
                foreach (var s in GetGeneratedCtor(def)) yield return s;
            }

            foreach (var s in GetGeneratedInterfaces(def)) yield return s;

            foreach (var s in GetGeneratedRoixMethods(def)) yield return s;
        }

        private IEnumerable<MemberDeclarationSyntax> WithTrivia(IEnumerable<MemberDeclarationSyntax> members, SyntaxTriviaList leadingTrivia, SyntaxTriviaList trailingTrivia)
        {
            var array = members.ToArray();

            if (array.Length == 0) yield break;

            if (array.Length == 1)
            {
                yield return array[0]
                    .WithLeadingTrivia(leadingTrivia)
                    .WithTrailingTrivia(trailingTrivia);

                yield break;
            }

            yield return array[0].WithLeadingTrivia(leadingTrivia);

            for (int i = 1; i < array.Length - 1; i++)
                yield return array[i];

            yield return array[array.Length - 1].WithTrailingTrivia(trailingTrivia);
        }

        private static MemberDeclarationSyntax ToMemberDeclarationSyntax(string source)
            => CSharpSyntaxTree.ParseText(source).GetRoot().ChildNodes().OfType<MemberDeclarationSyntax>().First();

        private IEnumerable<MemberDeclarationSyntax> GetGeneratedMember(SimpleProperty p)
        {
            var source = string.Format(@"        public {1} {0} => _values.{0};",
                p.Name, p.Type.WithoutTrivia().GetText().ToString());

            yield return ToMemberDeclarationSyntax(source);
        }

        private IEnumerable<MemberDeclarationSyntax> GetGeneratedCtor(RecordDefinition def)
        {
            static string GetTypeString(TypeSyntax type)
                => type.IsKind(SyntaxKind.StructDeclaration) && type.IsKind(SyntaxKind.ReadOnlyKeyword)
                    ? "in " + type.ToString()
                    : type.ToString();

            var structName = def.ParentSyntax.GetGenericTypeName();
            var source = string.Format(@"        public {0}({1}) => _values = new({2});", structName,
                string.Join(", ", def.Properties.Select(p => GetTypeString(p.Type) + " " + p.Name.ToLower())),
                string.Join(", ", def.Properties.Select(p => p.Name.ToLower())));

            yield return ToMemberDeclarationSyntax(source);
        }

        private IEnumerable<MemberDeclarationSyntax> GetGeneratedInterfaces(RecordDefinition def)
        {
            var structName = def.ParentSyntax.GetGenericTypeName();
            var sources = new[]
            {
                string.Format(@"        public void Deconstruct({0}) => ({1}) = ({2});",
                    string.Join(", ", def.Properties.Select(p => $"out {p.Type} {p.Name.ToLower()}")),
                    string.Join(", ", def.Properties.Select(p => p.Name.ToLower())),
                    string.Join(", ", def.Properties.Select(p => p.Name))),

                //string.Format(@"        public bool Equals({0} other) => ({1}) == ({2});", structName,
                //    string.Join(", ", def.Properties.Select(p => p.Name)),
                //    string.Join(", ", def.Properties.Select(p => "other." + p.Name))),
                string.Format(@"        public bool Equals({0} other) => this == other;", structName),

                string.Format(@"        public override bool Equals(object? obj) => (obj is {0} other) && Equals(other);", structName),

                string.Format(@"        public override int GetHashCode() => HashCode.Combine({0});",
                    string.Join(", ", def.Properties.Select(p => p.Name))),

                string.Format(@"        public static bool operator ==(in {0} left, in {0} right) => ({1}) == ({2});", structName,
                    string.Join(", ", def.Properties.Select(p => "left." + p.Name)),
                    string.Join(", ", def.Properties.Select(p => "right." + p.Name))),

                string.Format(@"        public static bool operator !=(in {0} left, in {0} right) => !(left == right);", structName),

                string.Format(@"        public override string ToString() => $""{0} {{{{ {1} }}}}"";", structName,
                    string.Join(", ", def.Properties.Select(p => p.Name + " = {" + p.Name + "}"))),

                string.Format(@"        public string ToString(string? format, IFormatProvider? formatProvider) => $""{0} {{{{ {1} }}}}"";", structName,
                    string.Join(", ", def.Properties.Select(p => p.Name + " = {" + p.Name + ".ToString(format, formatProvider)}"))),
            };

            foreach (var source in sources)
                yield return ToMemberDeclarationSyntax(source);
        }

        private IEnumerable<MemberDeclarationSyntax> GetGeneratedRoixMethods(RecordDefinition def)
        {
            var structName = def.ParentSyntax.GetGenericTypeName();
            var sources = new[]
            {
                string.Format(@"        public static {0} Zero {{ get; }} = default;", structName),
                string.Format(@"        public bool IsZero => this == Zero;"),
                string.Format(@"        public bool IsNotZero => !IsZero;"),
            };

            foreach (var source in sources)
                yield return ToMemberDeclarationSyntax(source);
        }

    }
}
