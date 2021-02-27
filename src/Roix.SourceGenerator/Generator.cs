using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Roix.SourceGenerator
{
    public class Generator
    {
        public CompilationUnitSyntax GeneratePartialDeclaration(INamedTypeSymbol container, StructDeclarationSyntax structDecl)
        {
            var recordStructDecl = (StructDeclarationSyntax)structDecl.ChildNodes().First(x => x is StructDeclarationSyntax);

            var def = new RecordDefinition(recordStructDecl);
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

            return CompilationUnit().AddUsings(WithComponentModel(root.Usings))
                .AddMembers(topDecl)
                .WithTrailingTrivia(CarriageReturnLineFeed)
                .NormalizeWhitespace();
        }

        private UsingDirectiveSyntax[] WithComponentModel(IEnumerable<UsingDirectiveSyntax> usings) => usings.ToArray();

        private IEnumerable<MemberDeclarationSyntax> GetGeneratedNodes(RecordDefinition def)
        {
            yield return CSharpSyntaxTree.ParseText(
                $@"        private readonly {Consts.SourceValuesStruct} _value;
")
                .GetRoot().ChildNodes()
                .OfType<MemberDeclarationSyntax>()
                .First()
                .WithTrailingTrivia(CarriageReturnLineFeed, CarriageReturnLineFeed);

            foreach (var p in def.Properties)
                foreach (var s in WithTrivia(GetGeneratedMember(p), p.LeadingTrivia, p.TrailingTrivia))
                    yield return s;

            //foreach (var p in def.DependentProperties)
            //    foreach (var s in WithTrivia(GetGeneratedMember(p), p.LeadingTrivia, p.TrailingTrivia))
            //        yield return s;
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

        private IEnumerable<MemberDeclarationSyntax> GetGeneratedMember(SimpleProperty p)
        {
            var source = string.Format(@"        public {1} {0} => _value.{0};",
                p.Name, p.Type.WithoutTrivia().GetText().ToString());

            var generatedNodes = CSharpSyntaxTree.ParseText(source)
                .GetRoot().ChildNodes()
                .OfType<MemberDeclarationSyntax>();

            return generatedNodes;
        }

    }
}
