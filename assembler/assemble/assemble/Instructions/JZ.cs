using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES1Assembler.Instructions;

internal class JZ : LabelParamOwner
{
    private JZ(string label) : base(label)
    {
    }

    protected override void WriteOpCode(AssemblyStream stream) => stream.WriteOpCode(OpCode.JZ);

    public static bool TryParse(FileStream stream, out IInstruction? instruction)
    {
        instruction = new JZ(ParseParameter(stream));
        return true;
    }
}
