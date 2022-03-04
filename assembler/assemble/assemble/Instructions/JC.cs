using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES1Assembler.Instructions;

internal class JC : LabelParamOwner
{
    private JC(string label) : base(label)
    {
    }

    protected override void WriteOpCode(AssemblyStream stream) => stream.WriteOpCode(OpCode.JC);

    public static bool TryParse(FileStream stream, out IInstruction? instruction)
    {
        instruction = new JC(ParseParameter(stream));
        return true;
    }
}
