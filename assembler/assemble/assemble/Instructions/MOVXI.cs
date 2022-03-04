using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES1Assembler.Instructions;

internal class MOVXI : VariableParamOwner
{
    protected MOVXI(string variable) : base(variable)
    {
    }

    protected override void WriteOpCode(AssemblyStream stream) => stream.WriteOpCode(OpCode.MOVXI);

    public static bool TryParse(FileStream stream, out IInstruction? instruction)
    {
        instruction = new MOVXI(ParseParameter(stream));
        return true;
    }
}
