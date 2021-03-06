﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Roix.SourceGenerator
{
    class RecordDefinition
    {
        public StructDeclarationSyntax ParentSyntax { get; }
        public IReadOnlyList<SimpleProperty> Properties { get; }
        public bool IsConstructorDeclared { get; }

        public RecordDefinition(StructDeclarationSyntax parentDecl, StructDeclarationSyntax recordDecl)
        {
            ParentSyntax = parentDecl;
            Properties = SimpleProperty.New(recordDecl).ToArray();
            IsConstructorDeclared = GetIsConstructorDeclared(ParentSyntax, Properties);
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
