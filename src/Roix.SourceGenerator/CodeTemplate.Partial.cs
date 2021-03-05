using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Roix.SourceGenerator.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Roix.SourceGenerator
{
    public partial class CodeTemplate
    {
        internal enum ArithmeticOperators
        {
            Add, Subtract, Multiply, Divide
        }

        internal string Namespace { get; set; } = "";
        internal string? Type { get; set; }
        internal RoixStructGeneratorOptions Options { get; set; }
        public string? ToStringFormat { get; set; }

        internal string Name { get; }
        internal StructDeclarationSyntax ParentSyntax { get; }
        internal IReadOnlyList<SimpleProperty> Properties { get; }
        internal bool IsConstructorDeclared { get; }

        public CodeTemplate(StructDeclarationSyntax parentDecl, StructDeclarationSyntax recordDecl)
        {
            Name = parentDecl.GetGenericTypeName();
            ParentSyntax = parentDecl;
            Properties = SimpleProperty.New(recordDecl).ToArray();
            IsConstructorDeclared = GetIsConstructorDeclared(parentDecl, Properties);
        }

        private bool GetIsConstructorDeclared(StructDeclarationSyntax structDecl, IReadOnlyList<SimpleProperty> properties)
        {
            var ctorDeclarationSyntaxs = structDecl.Members.Where(mem => mem.Kind() == SyntaxKind.ConstructorDeclaration)
                .OfType<ConstructorDeclarationSyntax>();

            foreach (var syntax in ctorDeclarationSyntaxs)
            {
                var typeNames = syntax.ParameterList.Parameters.Select(x => x.Type?.ToString() ?? "");
                var props = properties.Select(p => p.Type.ToString());
                if (typeNames?.SequenceEqual(props) ?? false)
                    return true;
            }
            return false;
        }

        internal bool HasFlag(RoixStructGeneratorOptions options) => Options.HasFlag(options);

    }
}
