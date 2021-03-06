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
        internal RoixStructGeneratorOptions Options { get; }
        public string? ToStringFormat { get; set; }

        internal string Name { get; }
        internal StructDeclarationSyntax ParentSyntax { get; }
        internal IReadOnlyList<SimpleProperty> Properties { get; }
        internal bool IsConstructorDeclared { get; }

        internal CodeTemplate(StructDeclarationSyntax parentDecl, StructDeclarationSyntax recordDecl, RoixStructGeneratorOptions options)
        {
            ParentSyntax = parentDecl;
            Name = parentDecl.GetGenericTypeName();
            Options = options | GetOptionsFromName(Name);
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

        internal RoixStructGeneratorOptions GetOptionsFromName(string structName)
        {
            var isRoix = structName.Contains("Roix");
            var isInt = structName.Contains("Int");
            var isBorder = structName.Contains("Border");
            var isPoint = structName.Contains("Point");
            var isVector = structName.Contains("Vector");
            var isSize = structName.Contains("Size");
            var isRect = structName.Contains("Rect");

            var option = RoixStructGeneratorOptions.None;
            if (!isRoix) return option;

            if (isInt) option |= RoixStructGeneratorOptions.TypeInt;
            if (isBorder) option |= RoixStructGeneratorOptions.WithBorder;
            if (!isBorder)
            {
                if (isPoint || isVector || isSize) option |= RoixStructGeneratorOptions.XYPair;
                if (isRect) option |= RoixStructGeneratorOptions.Rect;
            }
            return option;
        }
    }
}
