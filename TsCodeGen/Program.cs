using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsCodeGen
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().DoWork();
        }

        void DoWork()
        {
            var workspace = MSBuildWorkspace.Create();
            var project = workspace.OpenProjectAsync(@"../../../ATestProject/ATestProject.csproj").Result;
            


            var compUnit = project.GetCompilationAsync().Result;
            var context = new Input.TsContext();
            
            var output = new TsItems.TsOutputer();
            foreach (var st in compUnit.SyntaxTrees)
            {
                
                var semanticModel = compUnit.GetSemanticModel(st);
                var root = st.GetRoot();
                var tsw = new TSWalker(output, context, semanticModel);
                tsw.Visit(root);
                Console.WriteLine(st.FilePath);
            }


            System.IO.File.WriteAllText("output.ts", output.GetContent(), Encoding.UTF8);
        }
    }
}
