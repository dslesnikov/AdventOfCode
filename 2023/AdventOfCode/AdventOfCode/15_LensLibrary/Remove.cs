namespace AdventOfCode._15_LensLibrary;

public sealed record Remove(string Label) : Operation(Label)
{
    public override string Instruction => $"{Label}-";
}