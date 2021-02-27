using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Roix.SourceGenerator
{
    class RecordDefinition
    {
        public StructDeclarationSyntax ParentSyntax { get; }
        public IReadOnlyList<SimpleProperty> Properties { get; }

        public RecordDefinition(StructDeclarationSyntax parentDecl, StructDeclarationSyntax recordDecl)
        {
            ParentSyntax = parentDecl;
            Properties = SimpleProperty.New(recordDecl).ToArray();
        }
    }

    class SimpleProperty
    {
        public TypeSyntax Type { get; }
        public string Name { get; }
        public SyntaxTriviaList LeadingTrivia { get; }
        public SyntaxTriviaList TrailingTrivia { get; }

        public SimpleProperty(FieldDeclarationSyntax d)
        {
            Type = d.Declaration.Type;
            Name = d.Declaration.Variables[0].Identifier.Text;
            LeadingTrivia = d.GetLeadingTrivia();
            TrailingTrivia = d.GetTrailingTrivia();
        }

        public static IEnumerable<SimpleProperty> New(StructDeclarationSyntax decl)
            => decl.Members.OfType<FieldDeclarationSyntax>().Select(d => new SimpleProperty(d));
    }
}
