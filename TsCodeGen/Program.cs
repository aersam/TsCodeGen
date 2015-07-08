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
            Task.WaitAll(new Program().DoWork());
        }

        async Task DoWork()
        {
            var workspace = MSBuildWorkspace.Create();
            var project = await workspace.OpenProjectAsync(@"../../../ATestProject/ATestProject.csproj");
            StringBuilder allOutput = new StringBuilder();


            var compUnit = await project.GetCompilationAsync();


            foreach (var st in compUnit.SyntaxTrees)
            {
                var semanticModel = compUnit.GetSemanticModel(st);
                var root = st.GetRoot();
                var tsw = new TSWalker(compUnit, semanticModel, allOutput);
                tsw.Visit(root);
                Console.WriteLine(st.FilePath);
            }


            System.IO.File.WriteAllText("output.d.ts", allOutput.ToString(), Encoding.UTF8);
        }
    }
}
