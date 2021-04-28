using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Roix.SourceGenerator.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Roix.SourceGenerator
{
    [Generator]
    public sealed class RoixStructGenerator : ISourceGenerator
    {
        private const string _attributeName = nameof(RoixStructGenerator) + "Attribute";

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
            if (context.Compilation is not CSharpCompilation) return;

            var attrCode = new RoixStructGeneratorAttributeTemplate().TransformText();
            context.AddSource(_attributeName + ".cs", attrCode);

            try
            {
                if (context.SyntaxReceiver is not SyntaxReceiver receiver) return;

                foreach (var (parent, record, options) in receiver.Targets)
                {
                    var model = context.Compilation.GetSemanticModel(parent.SyntaxTree);
                    if (model.GetDeclaredSymbol(parent) is INamedTypeSymbol typeSymbol)
                    {
                        var template = new CodeTemplate(parent, record, options)
                        {
                            Namespace = typeSymbol.ContainingNamespace.ToDisplayString(),
                        };

                        if (context.CancellationToken.IsCancellationRequested) return;

                        var text = template.TransformText();
                        context.AddSource(typeSymbol.GenerateHintName(), text);
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
            internal List<(StructDeclarationSyntax parent, StructDeclarationSyntax record, RoixStructGeneratorOptions options)> Targets { get; } = new();

            public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
            {
                static bool IsReadOnlyStruct(StructDeclarationSyntax structDeclarationSyntax)
                    => structDeclarationSyntax.Modifiers.Any(m => m.IsKind(SyntaxKind.ReadOnlyKeyword));

                // Self除いてるけど Ancestor だけ探すメソッドないの？
                static StructDeclarationSyntax? GetParentStructDeclarationSyntax(StructDeclarationSyntax structDeclarationSyntax)
                    => structDeclarationSyntax.FirstAncestorOrSelf<StructDeclarationSyntax>(x => x != structDeclarationSyntax);

                if (syntaxNode is not StructDeclarationSyntax record) return;

                if (record.Identifier.Text != Consts.SourceValuesStruct) return;
                if (!IsReadOnlyStruct(record)) return;

                var parent = GetParentStructDeclarationSyntax(record);
                if (parent is null) return;

                if (!parent.AttributeLists.SelectMany(x => x.Attributes).Any(x => x.Name.ToString() is nameof(RoixStructGenerator) or _attributeName))
                    return;

                if (!IsReadOnlyStruct(parent)) return;
                if (!parent.ChildNodes().Any(n => n == record)) return;

                var options = GetOptionsFromAttribute(parent);

                Targets.Add((parent, record, options));
            }

            private static RoixStructGeneratorOptions GetOptionsFromAttribute(StructDeclarationSyntax structDeclarationSyntax)
            {
                var attr = structDeclarationSyntax.AttributeLists.SelectMany(x => x.Attributes)
                    .FirstOrDefault(x => x.Name.ToString() is nameof(RoixStructGenerator) or _attributeName);

                var argSyntax = attr?.ArgumentList?.Arguments.FirstOrDefault();

                if (argSyntax is null) return RoixStructGeneratorOptions.None;

                // e.g. Options.Flag0 | Options.Flag1 => Flag0 , Flag1
                var parsed = Enum.Parse(typeof(RoixStructGeneratorOptions),
                    argSyntax.Expression.ToString().Replace(nameof(RoixStructGeneratorOptions) + ".", "").Replace("|", ","));

                return (RoixStructGeneratorOptions)parsed;
            }
        }
    }
}
