using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;

namespace TsCodeGen
{
    class TSWalker : CSharpSyntaxWalker
    {
        Compilation compilation;
        SemanticModel model;
        StringBuilder output;

        public TSWalker(Compilation compilation, SemanticModel model, StringBuilder output)
        {
            this.compilation = compilation;
            this.model = model;
            this.output = output;
        }

        public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            base.VisitPropertyDeclaration(node);
            var name = node.Identifier.ToString();
            var symbol = model.GetDeclaredSymbol(node);
            if (symbol.GetMethod != null && symbol.SetMethod != null)
            {
                var typeName = symbol.Type.Name.ToString();
                var newTypeName = TsItems.TsOutputer.GetT4Type(symbol.Type);
                
                output.AppendLine("" + name + ": " + newTypeName +";");
            }
        }

        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {

            var name = node.Identifier.ToString();
            output.AppendLine("export interface " + name);
            output.AppendLine("{");
            var symbol = model.GetDeclaredSymbol(node);
            base.VisitClassDeclaration(node);
            output.AppendLine("}");
        }

        public override void VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
        {
            var name = node.Name.ToString();
            output.AppendLine("//From Path: " + node.SyntaxTree.FilePath);
            output.AppendLine("declare module " + name + "{");

            base.VisitNamespaceDeclaration(node);
            output.AppendLine("}");
        }
    }
}
