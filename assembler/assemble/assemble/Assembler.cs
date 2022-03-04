using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES1Assembler;

internal static class Assembler
{
    public static void Assemble(string filename)
    {
        AssemblyContext context;
        using (FileStream input = File.OpenRead(filename))
        {
            context = AssemblyContext.ParseFrom(input);
        }
        string outname = filename + ".hex";
        File.Delete(outname);
        using FileStream output = File.OpenWrite(outname);
        AssemblyStream assemblyStream = new(output);
        context.Accept(assemblyStream);
        output.Flush();
    }
}
