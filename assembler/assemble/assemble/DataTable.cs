using ES1Assembler.Instructions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES1Assembler;

internal class DataTable : IAssemblyStreamVisitor
{
    public IReadOnlyDictionary<string, Variable> Variables { get; }

    public List<Variable> OrderedVariables { get; }

    private DataTable(Dictionary<string, Variable> variables)
    {
        Variables = variables;
        OrderedVariables = variables.Values.ToList();
    }

    public static bool TryCreate(FileStream stream, AssemblyContext context)
    {
        if (StreamParser.TrySkipLine(stream) is false)
        {
            throw new InvalidDataException("encountered enexpected end of stream while parsing .data region!");
        }
        Dictionary<string, Variable> variables = new();
        bool didNotReachEnd;
        do
        {
            int c = stream.ReadByte();
            Variable? var = null;
            didNotReachEnd = c switch
            {
                ';' => StreamParser.TrySkipLine(stream),
                >= 'a' and <= 'z' or >= 'A' and <= 'Z' or '_' => --stream.Position as long? is not null && Variable.TryParse(stream, out var),
                _ when char.IsWhiteSpace((char)c) => true,
                '.' => --stream.Position as long? is null, // decrement position and return false (could be start of .text section)
                _ => throw new InvalidDataException($"Encountered unknown character '{c}' at position {stream.Position}!")
            };
            if (var is not null)
            {
                variables.Add(var.Name, var);
            }
        } while (didNotReachEnd);
        context.Data = new DataTable(variables);
        return true;
    }

    public void CalculateOffsets(int globalOffset)
    {
        int offset = 0;
        foreach (Variable variable in OrderedVariables)
        {
            variable.Address = (byte)(globalOffset + offset);
            offset += variable.Length;
        }
    }

    public void Accept(AssemblyStream stream, AssemblyContext context)
    {
        foreach (Variable variable in OrderedVariables)
        {
            variable.Accept(stream, context);
        }
    }
}
