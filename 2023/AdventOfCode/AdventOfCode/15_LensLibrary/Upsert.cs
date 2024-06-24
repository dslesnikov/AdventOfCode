namespace AdventOfCode._15_LensLibrary;

public sealed record Upsert(string Label, int FocalLength) : Operation(Label)
{
    public override string Instruction => $"{Label}={FocalLength}";
}