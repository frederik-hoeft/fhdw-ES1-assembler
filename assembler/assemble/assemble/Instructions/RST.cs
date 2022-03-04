using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES1Assembler.Instructions;

internal class RST : SimpleOp
{
    protected override OpCode OpCode => OpCode.RST;
}
