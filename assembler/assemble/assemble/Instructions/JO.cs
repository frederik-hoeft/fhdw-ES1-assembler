using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES1Assembler.Instructions;

internal class JO : LabelParamOwner
{
    private JO(string label) : base(label)
    {
    }

    protected override void WriteOpCode(AssemblyStream stream) => stream.WriteOpCode(OpCode.JO);

    public static bool TryParse(FileStream stream, out IInstruction? instruction)
    {
        instruction = new JO(ParseParameter(stream));
        return true;
    }
}
