namespace AdventOfCode.Day11Reactor;

public class Solution : ISolution, IFromLines<Solution, Connection>
{
    public static int Day => 11;

    private readonly IReadOnlyDictionary<string, Connection> _connections;

    private Solution(IReadOnlyDictionary<string, Connection> connections)
    {
        _connections = connections;
    }

    public string SolvePartOne()
    {
        var paths = CountPaths("you", "out");
        return paths.ToString();
    }

    public string SolvePartTwo()
    {
        var svrToFft = CountPaths("svr", "fft");
        var fftToDac = CountPaths("fft", "dac");
        var dacToOut = CountPaths("dac", "out");
        
        var svrToDac = CountPaths("svr", "dac");
        var dacToFft = CountPaths("dac", "fft");
        var fftToOut = CountPaths("fft", "out");
        
        var totalPaths = svrToFft * fftToDac * dacToOut +
                         svrToDac * dacToFft * fftToOut;
        return totalPaths.ToString();
    }

    private long CountPaths(string start, string end)
    {
        var memo = new Dictionary<string, long>();
        return Dfs(start);

        long Dfs(string node)
        {
            if (node == end)
            {
                return 1;
            }

            if (memo.TryGetValue(node, out var count))
            {
                return count;
            }

            var totalPaths = 0L;
            if (_connections.TryGetValue(node, out var connection))
            {
                totalPaths += connection.To.Sum(Dfs);
            }

            memo[node] = totalPaths;
            return totalPaths;
        }
    }

    public static Connection ParseLine(string line)
    {
        return Connection.Parse(line);
    }

    public static Solution FromParsed(IReadOnlyList<Connection> entries)
    {
        var connections = entries.ToDictionary(c => c.From);
        return new Solution(connections);
    }
}