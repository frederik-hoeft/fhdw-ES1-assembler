using System.Text;

namespace ES1Assembler.Instructions;

internal class Variable : IInstruction
{
    public int Length => 1;

    public string Name { get; }

    public byte InitialValue { get; }

    public byte Address { get; set; }

    private Variable(string name, byte initialValue)
    {
        Name = name;
        InitialValue = ((sbyte)initialValue) < 0 ? (byte)(initialValue >> 4) : initialValue;
    }

    public static bool TryParse(FileStream stream, out Variable variable)
    {
        _ = StreamParser.TryParseAlphanumericString(stream, out string? name);
        if (StreamParser.TrySkipWhiteSpace(stream) is false)
        {
            throw new EndOfStreamException("Ecountered end of stream while parsing variable!");
        }
        if (stream.ReadByte() != 'D' || stream.ReadByte() != 'W')
        {
            throw new InvalidDataException($"Invalid syntax on position {stream.Position} in variable declaration! Expected 'DW' but got somethign else.");
        }
        if (StreamParser.TrySkipWhiteSpace(stream) is false || StreamParser.TrySkipNumber(stream, out int skippedBytes) is false)
        {
            throw new EndOfStreamException("Ecountered end of stream while parsing variable!");
        }
        byte[] buffer = new byte[skippedBytes];
        stream.Position -= skippedBytes;
        stream.Read(buffer, 0, skippedBytes);
        string sValue = Encoding.ASCII.GetString(buffer);
        int value = int.Parse(sValue);
        variable = new Variable(name!, (byte)value);
        return true;
    }

    public void Accept(AssemblyStream stream, AssemblyContext context) => stream.WriteNibble(InitialValue);
}