using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES1Assembler;

internal class AssemblyStream
{
    private readonly Stream _stream;

    public AssemblyStream(Stream stream) => _stream = stream;

    public void WriteOpCode(OpCode op) => WriteNibble((byte)op);

    public void WriteNibble(byte n)
    {
        if (n > 16)
        {
            throw new ArgumentOutOfRangeException(nameof(n));
        }
        if (n < 0xA)
        {
            _stream.WriteByte((byte)('0' + n));
        }
        else
        {
            _stream.WriteByte((byte)('A' + n - 0xA));
        }
    }

    public void WriteByte(byte b)
    {
        WriteNibble((byte)(b >> 4));
        WriteNibble((byte)(b & 0xF));
    }
}
