using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES1Assembler.Instructions;

internal class JMP : LabelParamOwner
{
    private JMP(string label) : base(label)
    {
    }

    protected override void WriteOpCode(AssemblyStream stream) => stream.WriteOpCode(OpCode.JMP);

    public static bool TryParse(FileStream stream, out IInstruction? instruction)
    {
        instruction = new JMP(ParseParameter(stream));
        return true;
    }
}
