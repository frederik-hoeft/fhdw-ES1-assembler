using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ES1Assembler.Instructions;

internal class Label : IInstruction
{
    public int Length => 0;

    public string Name { get; }
    public byte Address { get; set; }

    private Label(string name) => Name = name;

    public static bool TryParse(FileStream stream, Dictionary<string, Label> labels, out IInstruction? instruction)
    {
        if (StreamParser.TrySkipWhiteSpace(stream) is false)
        {
            throw new InvalidDataException("Encountered end of stream while parsing instruction!");
        }
        if (!StreamParser.TryParseString(stream, out string? name))
        {
            throw new InvalidDataException("Encountered end of stream while parsing instruction!");
        }
        int b = stream.ReadByte();
        _ = b switch
        {
            -1 => throw new InvalidDataException("Encountered end of stream while parsing instruction!"),
            not ':' => throw new InvalidDataException("Missing ':' after label!"),
            _ => true
        };
        Label label = new(name!);
        labels.Add(name!, label);
        instruction = label;
        return true;
    }

    public void Accept(AssemblyStream stream, AssemblyContext context) { }
}
