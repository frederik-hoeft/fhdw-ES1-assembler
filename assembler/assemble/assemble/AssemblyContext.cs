using System.Buffers.Binary;

namespace ES1Assembler;

internal class AssemblyContext
{
    private DataTable _data = null!;
    private InstructionList _instructions = null!;
    
    public DataTable Data
    {
        get => _data;
        set
        {
            if (_data is not null)
            {
                throw new InvalidOperationException($"{nameof(Data)} was already initialized!");
            }
            _data = value;
        }
    }

    public InstructionList Instructions
    {
        get => _instructions;
        set
        {
            if (_instructions is not null)
            {
                throw new InvalidOperationException($"{nameof(Instructions)} was already initialized!");
            }
            _instructions = value;
        }
    }

    private AssemblyContext()
    {
    }

    public static unsafe AssemblyContext ParseFrom(FileStream stream)
    {
        byte* pBuffer = stackalloc byte[16];
        Span<byte> buffer = new(pBuffer, 16);
        AssemblyContext context = new();
        bool didNotReachEnd;
        do
        {
            int c = stream.ReadByte();
            didNotReachEnd = c switch
            {
                ';' => StreamParser.TrySkipLine(stream),
                '.' => (c = stream.ReadByte()) switch
                {
                    't' when stream.Read(buffer[..4]) != -1 && BinaryPrimitives.ReadUInt32BigEndian(buffer) == 0x6578743Au => InstructionList.TryCreate(stream, context), // text:
                    'c' when stream.Read(buffer[..4]) != -1 && BinaryPrimitives.ReadUInt32BigEndian(buffer) == 0x6F64653Au => InstructionList.TryCreate(stream, context), // code:
                    'd' when stream.Read(buffer[..4]) != -1 && BinaryPrimitives.ReadUInt32BigEndian(buffer) == 0x6174613Au => DataTable.TryCreate(stream, context), // data:
                    _ => true
                },
                -1 => false,
                _ => true
            };
        } while (didNotReachEnd);
        _ = context._data ?? throw new InvalidDataException(".data region is missing!");
        _ = context._instructions ?? throw new InvalidDataException(".text region is missing!");
        return context;
    }

    public void Accept(AssemblyStream stream)
    {
        Instructions.Accept(stream, this);
        Data.Accept(stream, this);
    }
}