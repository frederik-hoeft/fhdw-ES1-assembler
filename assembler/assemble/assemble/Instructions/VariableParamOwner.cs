using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES1Assembler.Instructions;

internal abstract class VariableParamOwner : AddressParamOwner
{
    protected VariableParamOwner(string variable) : base(variable)
    {
    }

    public override void Accept(AssemblyStream stream, AssemblyContext context)
    {
        WriteOpCode(stream);
        if (context.Data.Variables.TryGetValue(VariableOrLabel, out Variable? variable))
        {
            stream.WriteByte(variable.Address);
        }
    }
}
