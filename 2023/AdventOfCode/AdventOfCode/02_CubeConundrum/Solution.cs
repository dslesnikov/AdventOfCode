namespace AdventOfCode._02_CubeConundrum;

public class Solution(IReadOnlyList<Game> games) : ISolution<Solution>
{
    public static int Day => 2;

    public static Solution Parse(string s)
    {
        var games = s.Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(Game.Parse)
            .ToArray();
        return new Solution(games);
    }

    public string Part1()
    {
        const int maxRed = 12;
        const int maxGreen = 13;
        const int maxBlue = 14;
        var validGames = games
            .Where(game => game.Sets.All(set => set is { Red: <= maxRed, Green: <= maxGreen, Blue: <= maxBlue }));
        var result = validGames.Sum(x => x.Id);
        return result.ToString();
    }

    public string Part2()
    {
        var powers = games
            .Select(game =>
            {
                var (minRed, minGreen, minBlue) = (0, 0, 0);
                foreach (var cubeSet in game.Sets)
                {
                    minRed = Math.Max(minRed, cubeSet.Red);
                    minGreen = Math.Max(minGreen, cubeSet.Green);
                    minBlue = Math.Max(minBlue, cubeSet.Blue);
                }

                return minRed * minGreen * minBlue;
            });
        return powers.Sum().ToString();
    }
}