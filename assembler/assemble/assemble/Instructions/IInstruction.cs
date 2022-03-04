namespace ES1Assembler.Instructions;

internal interface IInstruction : IAssemblyStreamVisitor
{
    int Length { get; }

    byte Address { get; set; }
}