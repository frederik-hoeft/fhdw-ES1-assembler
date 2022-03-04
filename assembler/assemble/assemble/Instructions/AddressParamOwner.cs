namespace ES1Assembler.Instructions;

internal abstract class AddressParamOwner : IInstruction
{
    public int Length => 3;

    protected abstract void WriteOpCode(AssemblyStream stream);

    public abstract void Accept(AssemblyStream stream, AssemblyContext context);

    public string VariableOrLabel { get; }

    public byte Address { get; set; }

    protected AddressParamOwner(string variableOrLabel) => VariableOrLabel = variableOrLabel;

    protected static string ParseParameter(FileStream stream)
    {
        if (StreamParser.TrySkipWhiteSpace(stream) is false)
        {
            throw new InvalidDataException("Encountered end of stream while parsing instruction!");
        }
        if (!StreamParser.TryParseAlphanumericString(stream, out string? parameter))
        {
            throw new InvalidDataException("Encountered end of stream while parsing instruction!");
        }
        return parameter!;
    }
}