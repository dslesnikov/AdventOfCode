namespace AdventOfCode._14_ParabolicReflectorDish;

public class Solution(Field field) : ISolution<Solution>
{
    public static int Day => 14;

    public static Solution Parse(string s)
    {
        var lines = s.Split('\n');
        var tiles = new TileType[lines.Length][];
        for (var row = 0; row < lines.Length; row++)
        {
            tiles[row] = new TileType[lines[row].Length];
            for (var col = 0; col < lines[row].Length; col++)
            {
                tiles[row][col] = lines[row][col] switch
                {
                    '.' => TileType.Empty,
                    '#' => TileType.Solid,
                    'O' => TileType.Ball,
                    _ => tiles[row][col]
                };
            }
        }
        return new Solution(new Field(tiles));
    }

    public string Part1()
    {
        var copy = new Field(field);
        copy.TiltNorth();
        var result = copy.GetNorthBeamLoad();
        return result.ToString();
    }

    public string Part2()
    {
        var copy = new Field(field);
        var initialState = copy.ExtractState();
        var states = new Dictionary<FieldState, int>
        {
            { initialState, 0 }
        };
        var statesList = new List<FieldState> { initialState };
        var index = 1;
        while (true)
        {
            var state = CycleOnce(copy);
            if (states.TryGetValue(state, out var headLength))
            {
                var periodLength = index - headLength;
                var targetIndex = (1_000_000_000 - headLength) % periodLength + headLength;
                var targetState = statesList[targetIndex];
                return targetState.GetNorthBeamLoad(field.Height).ToString();
            }
            states[state] = index;
            statesList.Add(state);
            index++;
        }
    }

    private static FieldState CycleOnce(Field field)
    {
        field.TiltNorth();
        field.TiltWest();
        field.TiltSouth();
        field.TiltEast();
        return field.ExtractState();
    }
}