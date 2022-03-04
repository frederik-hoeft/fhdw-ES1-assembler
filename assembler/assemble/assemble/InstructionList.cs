using ES1Assembler.Instructions;

namespace ES1Assembler;

internal class InstructionList
{
    private readonly long _startPosition;
    private InstructionListNode? _first = null;
    private InstructionListNode? _last = null;
    private int _totalLength = 0;

    public InstructionList(long startPosition) => _startPosition = startPosition;

    public Dictionary<string, Label> Labels { get; } = new();

    public static bool TryCreate(FileStream stream, AssemblyContext context)
    {
        if (StreamParser.TrySkipLine(stream) is false)
        {
            throw new InvalidDataException("encountered enexpected end of stream while parsing .text region!");
        }
        InstructionList list = new(stream.Position);
        bool didNotReachEnd;
        do
        {
            int c = stream.ReadByte();
            IInstruction? instruction = null;
            didNotReachEnd = c switch
            {
                ';' => StreamParser.TrySkipLine(stream),
                >= 'a' and <= 'z' => --stream.Position as long? is not null && StreamParser.TryParseString(stream, out string? s) && s switch
                {
                    "and" => SimpleOp.TryParse<AND>(stream, out instruction),
                    "or" => SimpleOp.TryParse<OR>(stream, out instruction),
                    "not" => SimpleOp.TryParse<NOT>(stream, out instruction),
                    "add" => SimpleOp.TryParse<ADD>(stream, out instruction),
                    "rst" => SimpleOp.TryParse<RST>(stream, out instruction),
                    "mul" => SimpleOp.TryParse<MUL>(stream, out instruction),
                    "swp" => SimpleOp.TryParse<SWP>(stream, out instruction),
                    "ret" => SimpleOp.TryParse<RET>(stream, out instruction),
                    "movxo" => MOVXO.TryParse(stream, out instruction),
                    "movxi" => MOVXI.TryParse(stream, out instruction),
                    "jmp" => JMP.TryParse(stream, out instruction),
                    "jz" => JZ.TryParse(stream, out instruction),
                    "jo" => JO.TryParse(stream, out instruction),
                    "jc" => JC.TryParse(stream, out instruction),
                    "label" => Label.TryParse(stream, list.Labels, out instruction),
                    _ => throw new InvalidDataException($"encountered unknown instruciton '{s}' on position {stream.Position - s?.Length}")
                },
                _ when char.IsWhiteSpace((char)c) => true,
                _ => throw new InvalidDataException($"Encountered unknown character '{(char)c}' at position {stream.Position}!")
            };
            if (instruction is not null)
            {
                InstructionListNode node = new(instruction);
                if (list._first is null || list._last is null)
                {
                    list._first = node;
                    list._last = node;
                }
                else
                {
                    list._last.Next = node;
                    node.Previous = list._last;
                    list._last = node;
                }
                list._totalLength += instruction.Length;
            }
        } while (didNotReachEnd);
        context.Data.CalculateOffsets(list._totalLength);
        list.CalculateOffsets();
        context.Instructions = list;
        return true;
    }

    private void CalculateOffsets()
    {
        int offset = 0;
        for (InstructionListNode? node = _first; node is not null; node = node.Next)
        {
            node.Instruction.Address = (byte)offset;
            offset += node.Instruction.Length;
        }
    }

    public void Accept(AssemblyStream stream, AssemblyContext context)
    {
        for (InstructionListNode? node = _first; node is not null; node = node.Next)
        {
            node.Instruction.Accept(stream, context);
        }
    }

    private class InstructionListNode
    {
        public InstructionListNode? Previous { get; set; } = null;
        public InstructionListNode? Next { get; set; } = null;
        public IInstruction Instruction { get; }

        public InstructionListNode(IInstruction instruction) => Instruction = instruction;
    }
}