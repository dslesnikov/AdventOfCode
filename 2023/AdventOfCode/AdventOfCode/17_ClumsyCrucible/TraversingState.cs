namespace AdventOfCode._17_ClumsyCrucible;

public record TraversingState(
    int Row,
    int Col,
    int HeatLoss,
    Direction Direction,
    int Steps);