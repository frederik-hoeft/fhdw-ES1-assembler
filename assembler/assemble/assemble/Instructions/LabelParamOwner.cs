using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES1Assembler.Instructions;

internal abstract class LabelParamOwner : AddressParamOwner
{
    protected LabelParamOwner(string label) : base(label)
    {
    }

    public override void Accept(AssemblyStream stream, AssemblyContext context)
    {
        WriteOpCode(stream);
        if (context.Instructions.Labels.TryGetValue(VariableOrLabel, out Label? label))
        {
            stream.WriteByte(label.Address);
        }
    }
}
