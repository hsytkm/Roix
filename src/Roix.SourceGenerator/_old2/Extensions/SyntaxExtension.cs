using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Roix.SourceGenerator.Extensions
{
    static class SyntaxExtension
    {
        private static readonly SyntaxToken PartialToken = Token(SyntaxKind.PartialKeyword);

        public static ClassDeclarationSyntax AddPartialModifier(this ClassDeclarationSyntax typeDecl)
        {
            if (typeDecl.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword))) return typeDecl;
            return typeDecl.AddModifiers(new[] { PartialToken });
        }

        public static StructDeclarationSyntax GetPartialTypeDelaration(this StructDeclarationSyntax typeDecl)
        {
            var structName = GetGenericTypeName(typeDecl);
            return CSharpSyntaxTree.ParseText($@"
readonly partial struct {structName} : IEquatable<{structName}>, IFormattable
{{
}}
").GetRoot().ChildNodes().OfType<StructDeclarationSyntax>().First();
        }

        public static string GetGenericTypeName(this TypeDeclarationSyntax typeDecl)
        {
            if (typeDecl.TypeParameterList == null)
            {
                return typeDecl.Identifier.Text;
            }

            return typeDecl.Identifier.Text + "<" +
                string.Join(", ", typeDecl.TypeParameterList.Parameters.Select(p => p.Identifier.Text)) +
                ">";
        }
    }
}
