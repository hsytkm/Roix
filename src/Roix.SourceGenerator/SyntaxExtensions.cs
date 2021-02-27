using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Roix.SourceGenerator
{
    static class SyntaxExtensions
    {
        private static readonly SyntaxToken PartialToken = Token(SyntaxKind.PartialKeyword);

        public static ClassDeclarationSyntax AddPartialModifier(this ClassDeclarationSyntax typeDecl)
        {
            if (typeDecl.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword))) return typeDecl;
            return typeDecl.AddModifiers(new[] { PartialToken });
        }

        public static StructDeclarationSyntax GetPartialTypeDelaration(this StructDeclarationSyntax typeDecl)
            => CSharpSyntaxTree.ParseText($@"
readonly partial struct {GetGenericTypeName(typeDecl)}
{{
}}
").GetRoot().ChildNodes().OfType<StructDeclarationSyntax>().First();

        private static string GetGenericTypeName(TypeDeclarationSyntax typeDecl)
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
