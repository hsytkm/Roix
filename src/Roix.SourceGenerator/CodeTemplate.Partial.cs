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

        private RoixStructGeneratorOptions GetOptionsFromName(string structName)
        {
            var isRoix = structName.Contains("Roix");
            var isInt = structName.Contains("Int");
            var isBorder = structName.Contains("Border");

            var isPoint = structName.Contains("Point");
            var isRect = structName.Contains("Rect");
            var isSize = structName.Contains("Size");
            var isVector = structName.Contains("Vector");

            var option = RoixStructGeneratorOptions.None;
            if (!isRoix) return option;

            if (isInt) option |= RoixStructGeneratorOptions.TypeInt;
            if (isBorder) option |= RoixStructGeneratorOptions.WithBorder;
            if (!isBorder)
            {
                if (isPoint || isSize || isVector) option |= RoixStructGeneratorOptions.XYPair;
                if (isRect) option |= RoixStructGeneratorOptions.Rect;
                if (isPoint || isRect || isSize || isVector) option |= RoixStructGeneratorOptions.HasParent;
            }
            return option;
        }

        private string JoinItemsWithFormat(IEnumerable<string> items, string format = "")
        {
            if (string.IsNullOrWhiteSpace(format))
                return string.Join(", ", items);

            return string.Join(", ", items.Select(x => string.Format(format, x)));
        }

        internal string GetNames(string format = "") => JoinItemsWithFormat(Properties.Select(p => p.Name), format);

        internal string GetLowerNames(string format = "") => JoinItemsWithFormat(Properties.Select(p => p.Name.ToLower()), format);

        internal string GetToString() => string.Join(", ", JoinItemsWithFormat(Properties.Select(p => p.Name), "{0} = {{{0}}}"));

        internal string GetToStringWithFormat()
            => string.Join(", ", JoinItemsWithFormat(Properties.Select(p => p.Name), "{0} = {{{0}.ToString(format, formatProvider)}}"));

        internal string GetOperate2Value(ArithmeticOperators ope, string name1, string name2)
            => string.Join(", ", JoinItemsWithFormat(Properties.Select(p => p.Name), $"{name1}.{{0}} {GetOperatorString(ope)} {name2}.{{0}}"));

        internal string GetOperate1Value(ArithmeticOperators ope, string name1, string value2)
            => string.Join(", ", JoinItemsWithFormat(Properties.Select(p => p.Name), $"{name1}.{{0}} {GetOperatorString(ope)} {value2}"));

        private string GetInTypeIfNecessary(TypeSyntax typeSyntax)
        {
            // built-in types
            if (typeSyntax is PredefinedTypeSyntax) return typeSyntax.ToString();

            // my structs
            if (typeSyntax is IdentifierNameSyntax) return $"in {typeSyntax}";

            throw new NotImplementedException();
        }

        internal string GetTypeAndLowerNames(string format)
            => string.Join(", ", Properties.Select(p => string.Format(format, p.Type, p.Name.ToLower())));

        internal string GetTypeAndLowerNamesForArgs(string format)
            => string.Join(", ", Properties.Select(p => string.Format(format, GetInTypeIfNecessary(p.Type), p.Name.ToLower())));

        internal string GetRoixSizeStructName() => HasFlag(RoixStructGeneratorOptions.TypeInt) ? "RoixIntSize" : "RoixSize";

        internal string GetRoixPointStructName() => HasFlag(RoixStructGeneratorOptions.TypeInt) ? "RoixIntPoint" : "RoixPoint";

        internal string GetRoixDefaultBuiltInType() => HasFlag(RoixStructGeneratorOptions.TypeInt) ? "int" : "double";

        internal string GetRoixNameWithoutInt() => HasFlag(RoixStructGeneratorOptions.TypeInt) ? Name.Replace("Int", "") : Name;

        internal string GetRoixBorderName() => HasFlag(RoixStructGeneratorOptions.HasParent) ? Name.Replace("Roix", "RoixBorder") : Name;

        private string GetOperatorString(ArithmeticOperators ope) => ope switch
        {
            ArithmeticOperators.Add => "+",
            ArithmeticOperators.Subtract => "-",
            ArithmeticOperators.Multiply => "*",
            ArithmeticOperators.Divide => "/",
            _ => throw new NotImplementedException(),
        };

        internal string GetOperatorRoixAndRatio(ArithmeticOperators ope, string name, string ratio)
        {
            static string GetRatioXYPropertyName(int index) => ((index & 1) == 0) ? "X" : "Y";

            var list = new List<string>();
            for (int i = 0; i < Properties.Count; ++i)
            {
                list.Add(name + "." + Properties[i].Name + " " + GetOperatorString(ope) + " " + ratio + "." + GetRatioXYPropertyName(i));
            }
            return string.Join(", ", list);
        }

    }
}
