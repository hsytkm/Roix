#nullable disable
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roix.SourceGenerator
{
#if false
    [Generator]
    public sealed class RoixStructGenerator1 : ISourceGenerator
    {
        private const string _generatorName = nameof(RoixStructGenerator1);
        private const string _attributeName = _generatorName + "Attribute";
        private static readonly string _attributeFullName = $"{Consts.Namespace}.{_attributeName}";
        private readonly static string _attributeSource = $@"
using System;
namespace {Consts.Namespace}
{{
    [AttributeUsage(AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    public sealed class {_attributeName} : Attribute
    {{
    }}
}}";

        public void Initialize(GeneratorInitializationContext context)
        {
#if DEBUG
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                //System.Diagnostics.Debugger.Launch();
            }
#endif
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            context.AddSource($"{_generatorName}.cs", SourceText.From(_attributeSource, Encoding.UTF8));

            try
            {
                if (context.SyntaxReceiver is not SyntaxReceiver receiver) return;

                var options = (context.Compilation as CSharpCompilation).SyntaxTrees[0].Options as CSharpParseOptions;
                var compilation = context.Compilation.AddSyntaxTrees(
                    CSharpSyntaxTree.ParseText(SourceText.From(_attributeSource, Encoding.UTF8), options));
                var attributeSymbol = compilation.GetTypeByMetadataName(_attributeFullName);

                foreach (var candidate in receiver.CandidateStructs)
                {
                    var model = compilation.GetSemanticModel(candidate.SyntaxTree);
                    var typeSymbol = ModelExtensions.GetDeclaredSymbol(model, candidate);
                    var attribute = typeSymbol.GetAttributes().FirstOrDefault(ad =>
                        ad.AttributeClass.Equals(attributeSymbol, SymbolEqualityComparer.Default));
                    if (attribute is null) continue;
                    var isReadOnlyStruct = (typeSymbol as ITypeSymbol)?.IsReadOnly ?? false;
                    if (!isReadOnlyStruct) continue;
                    var namespaceName = typeSymbol.ContainingNamespace.ToDisplayString();
                    var structName = typeSymbol.Name;
                    //var properties = candidate.Members
                    //    .OfType<PropertyDeclarationSyntax>()
                    //    .Where(prop => prop.AccessorList?.Accessors.Any(acc => acc.Kind() == SyntaxKind.InitAccessorDeclaration) ?? false)
                    //    .Select(prop => new { Type = prop.Type.ToString(), Name = prop.Identifier.Text });

                    var fields = candidate.Members
                        .OfType<FieldDeclarationSyntax>()
                        .Select(field => (field, model: compilation.GetSemanticModel(field.SyntaxTree)))
                        .SelectMany(x => x.field.Declaration.Variables.Select(variable => x.model.GetDeclaredSymbol(variable) as IFieldSymbol))
                        .Select(x => (Type: x.Type.ToString(), Name: x.Name))
                        .ToArray();
                    var properties = fields.Select(f => ToPropertyName(f.Name)).ToArray();

                    var isConstructorDeclared = candidate.Members.Any(mem => mem.Kind() == SyntaxKind.ConstructorDeclaration);
                    var parameters = string.Join(", ", fields.Select(f => f.Type + " " + ToLocalName(f.Name)));
                    var outParameters = string.Join(", ", fields.Select(f => "out " + f.Type + " " + ToLocalName(f.Name)));
                    var fieldNames = string.Join(", ", fields.Select(f => f.Name));
                    var propNames = string.Join(", ", properties);
                    var argNames = string.Join(", ", fields.Select(f => ToLocalName(f.Name)));
                    //var otherArgNams = string.Join(", ", properties.Select(x => "other." + x));
                    var leftArgNames = string.Join(", ", properties.Select(x => "left." + x));
                    var rightArgNames = string.Join(", ", properties.Select(x => "right." + x));
                    //var sourceArgNames = string.Join(", ", fields.Select(p => "source." + p.Name));
                    var toStringParam = string.Join(", ", properties.Select(x => $"{{nameof({x})}} = {{{x}}}"));
                    var toStringParam2 = string.Join(", ", properties.Select(x => $"{{nameof({x})}} = {{{x}.ToString(format, formatProvider)}}"));

                    var sb = new StringBuilder();
                    sb.AppendLine($"#nullable enable");
                    sb.AppendLine($"using System;");
                    if (!typeSymbol.ContainingNamespace.IsGlobalNamespace)
                    {
                        sb.AppendLine($"namespace {namespaceName}");
                        sb.AppendLine($"{{");
                    }
                    sb.AppendLine($"    readonly partial struct {structName} : IEquatable<{structName}>, IFormattable");
                    sb.AppendLine($"    {{");
                    sb.AppendLine($"        public static {structName} Zero {{ get; }} = default;");
                    foreach (var f in fields)
                    {
                        sb.AppendLine($"        public {f.Type} {ToPropertyName(f.Name)} => {f.Name};");
                    }
                    if (!isConstructorDeclared)
                    {
                        sb.AppendLine($"        public {structName}({parameters}) => ({fieldNames}) = ({argNames});");
                    }
                    sb.AppendLine($"        public void Deconstruct({outParameters}) => ({argNames}) = ({propNames});");
                    sb.AppendLine($"        public bool Equals({structName} other) => (this == other);");
                    sb.AppendLine($"        public override bool Equals(object? obj) => (obj is {structName} other) && Equals(other);");
                    sb.AppendLine($"        public override int GetHashCode() => HashCode.Combine({propNames});");
                    sb.AppendLine($"        public static bool operator ==(in {structName} left, in {structName} right) => ({leftArgNames}) == ({rightArgNames});");
                    sb.AppendLine($"        public static bool operator !=(in {structName} left, in {structName} right) => !(left == right);");
                    sb.AppendLine($"        public override string ToString() => $\"{{nameof({structName})}} {{{{ {toStringParam} }}}}\";");
                    sb.AppendLine($"        public string ToString(string? format, IFormatProvider? formatProvider) => $\"{{nameof({structName})}} {{{{ {toStringParam2} }}}}\";");
                    sb.AppendLine($"        public bool IsZero => this == Zero;");
                    sb.AppendLine($"    }}");
                    if (!typeSymbol.ContainingNamespace.IsGlobalNamespace)
                    {
                        sb.AppendLine($"}}");
                    }
                    context.AddSource($"{structName}.Generated.cs", SourceText.From(sb.ToString(), Encoding.UTF8));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());
            }

            static string ToLocalName(string fieldName) => fieldName[0] == '_' ? fieldName.Substring(1) : fieldName;
            static string ToPropertyName(string fieldName) => fieldName[0] == '_' ? new string(new char[] { fieldName[1] }).ToUpper() + fieldName.Substring(2) : fieldName;
        }

        private sealed class SyntaxReceiver : ISyntaxReceiver
        {
            internal List<StructDeclarationSyntax> CandidateStructs { get; } = new();

            public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
            {
                static bool hasMyGeneratorAttribute(SyntaxList<AttributeListSyntax> attrs)
                    => attrs.SelectMany(x => x.Attributes).Any(x => x.Name.ToString().Contains(_generatorName));

                static bool isCandidate(StructDeclarationSyntax s)
                {
                    //var fields = s.Members.OfType<FieldDeclarationSyntax>()
                    //    .Where(x => hasMyGeneratorAttribute(x.AttributeLists));

                    var isReadOnly = s.Modifiers.IndexOf(SyntaxKind.ReadOnlyKeyword) != -1;
                    var hasAttribute = hasMyGeneratorAttribute(s.AttributeLists);
                    return isReadOnly && hasAttribute;
                }

                if (syntaxNode is StructDeclarationSyntax structDeclaration && isCandidate(structDeclaration))
                {
                    CandidateStructs.Add(structDeclaration);
                }
            }
        }
    }
#endif
}
