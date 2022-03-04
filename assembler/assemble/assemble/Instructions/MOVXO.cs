using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES1Assembler.Instructions;

internal class MOVXO : VariableParamOwner
{
    protected MOVXO(string variable) : base(variable)
    {
    }

    protected override void WriteOpCode(AssemblyStream stream) => stream.WriteOpCode(OpCode.MOVXO);

    public static bool TryParse(FileStream stream, out IInstruction? instruction)
    {
        instruction = new MOVXO(ParseParameter(stream));
        return true;
    }
}
