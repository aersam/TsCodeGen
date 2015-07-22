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
        

        
        SemanticModel model;
        TsItems.TsOutputer output;
        Input.TsContext context;

        public TSWalker( TsItems.TsOutputer output, Input.TsContext context, SemanticModel model)
        {
            this.model = model;
            this.context = context;
            this.output = output;
            output.ResetIndent();
        }

        public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            base.VisitPropertyDeclaration(node);
            var name = node.Identifier.ToString();
            var symbol = model.GetDeclaredSymbol(node);
            if (symbol.GetMethod != null && symbol.SetMethod != null)
            {
                var typeName = symbol.Type.Name.ToString();
                var newTypeName = context.GetTsType(symbol.Type);

                output.AppendLine(name + ": " + newTypeName + ";");
            }
        }
        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {

            var name = node.Identifier.ToString();
            string typeArgs = "";
            var firstChild = node.ChildNodes().FirstOrDefault();
            if (firstChild is TypeParameterListSyntax)
            {
                typeArgs = ((TypeParameterListSyntax)firstChild).ToString();
            }
            var symbol = model.GetDeclaredSymbol(node);

            output.AppendLine( "export interface " + name + typeArgs + "{");
            output.IncreaseIndent();
            base.VisitClassDeclaration(node);
            output.DecreaseIndent();
            output.AppendLine("}");

        }

        public override void VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
        {
            var name = node.Name.ToString();
            output.AppendLine("//From Path: " + node.SyntaxTree.FilePath);
            output.AppendLine("declare module " + name + "{");
            output.IncreaseIndent();
            base.VisitNamespaceDeclaration(node);
            output.DecreaseIndent();
            output.AppendLine("}");
        }

        public override void VisitEnumDeclaration(EnumDeclarationSyntax node)
        {

            var name = node.Identifier.ToString();
            if (node.Parent is ClassDeclarationSyntax)
            {
                base.VisitEnumDeclaration(node);
                return;
            }
            output.AppendLine("enum " + name + "{");
            output.IncreaseIndent();
            base.VisitEnumDeclaration(node);
            output.DecreaseIndent();
            output.AppendLine("}");
        }

        public override void VisitEnumMemberDeclaration(EnumMemberDeclarationSyntax node)
        {
            if (node.Parent.Parent is ClassDeclarationSyntax)
                return;
            base.VisitEnumMemberDeclaration(node);
            output.AppendLine(node.ToString()+",");//To simple?
        }
        
    }
}
