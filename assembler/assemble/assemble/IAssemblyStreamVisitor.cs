using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES1Assembler;

internal interface IAssemblyStreamVisitor
{
    void Accept(AssemblyStream stream, AssemblyContext context);
}
