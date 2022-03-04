using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES1Assembler.Instructions;

internal abstract class SimpleOp : IInstruction
{
    protected abstract OpCode OpCode { get; }

    public int Length => 1;

    public byte Address { get; set; }

    public static bool TryParse<T>(FileStream stream, out IInstruction? instruction) where T : SimpleOp, new()
    {
        instruction = new T();
        return true;
    }

    public void Accept(AssemblyStream stream, AssemblyContext context) => stream.WriteOpCode(OpCode);
}
