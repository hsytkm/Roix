//#nullable disable

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
    [Generator]
    public sealed class RoixStructGenerator2 : ISourceGenerator
    {
        private const string _generatorName = nameof(RoixStructGenerator2);
        private const string _attributeName = _generatorName + "Attribute";
        //private static readonly string _attributeFullName = $"{Consts.Namespace}.{_attributeName}";
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

                foreach (var (self, parent) in receiver.CandidateStructs)
                {
                    var model = context.Compilation.GetSemanticModel(parent.SyntaxTree);
                    if (model.GetDeclaredSymbol(parent) is INamedTypeSymbol type)
                    {
                        var source = new Generator().GeneratePartialDeclaration(type, parent).ToFullString();
                        context.AddSource(type.GenerateHintName(), source);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());
            }
        }

        private sealed class SyntaxReceiver : ISyntaxReceiver
        {
            internal List<(StructDeclarationSyntax self, StructDeclarationSyntax parent)> CandidateStructs { get; } = new();

            public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
            {
                static bool IsReadOnlyStruct(StructDeclarationSyntax structDeclarationSyntax)
                    => structDeclarationSyntax.Modifiers.Any(m => m.IsKind(SyntaxKind.ReadOnlyKeyword));

                // Self除いてるけど Ancestor だけ探すメソッドないの？
                static StructDeclarationSyntax? GetParentStructDeclarationSyntax(StructDeclarationSyntax structDeclarationSyntax)
                    => structDeclarationSyntax.FirstAncestorOrSelf<StructDeclarationSyntax>(x => x != structDeclarationSyntax);

                if (syntaxNode is not StructDeclarationSyntax structDeclarationSyntax) return;

                if (structDeclarationSyntax.Identifier.Text != Consts.SourceValuesStruct) return;
                if (!IsReadOnlyStruct(structDeclarationSyntax)) return;

                var parent = GetParentStructDeclarationSyntax(structDeclarationSyntax);
                if (parent is null) return;

                if (!IsReadOnlyStruct(parent)) return;
                if (!parent.ChildNodes().Any(n => n == structDeclarationSyntax)) return;

                CandidateStructs.Add((structDeclarationSyntax, parent));
            }
        }
    }
}
