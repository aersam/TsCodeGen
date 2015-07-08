using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsCodeGen.TsItems
{
    interface IOutputItem
    {
        string FullName
        { get; }

        string Write(int indent);//
    }
}
