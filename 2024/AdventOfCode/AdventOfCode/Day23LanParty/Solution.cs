namespace AdventOfCode.Day23LanParty;

public readonly record struct Connection(string From, string To)
{
    public static Connection FromLine(ReadOnlySpan<char> line)
    {
        var from = line[..2].ToString();
        var to = line[3..].ToString();
        return new Connection(from, to);
    }

    public Connection Reverse() => new(To, From);
}

public class Solution : IFromLines<Solution, Connection>
{
    public static int Day => 23;

    private readonly IReadOnlyList<Connection> _connections;
    private readonly IReadOnlyDictionary<string, IReadOnlySet<string>> _graph;

    private Solution(IReadOnlyList<Connection> connections)
    {
        _connections = connections;
        var graph = new Dictionary<string, HashSet<string>>();
        foreach (var connection in _connections)
        {
            if (graph.TryGetValue(connection.From, out var neighbors))
            {
                neighbors.Add(connection.To);
            }
            else
            {
                graph[connection.From] = [connection.To];
            }
            if (graph.TryGetValue(connection.To, out neighbors))
            {
                neighbors.Add(connection.From);
            }
            else
            {
                graph[connection.To] = [connection.From];
            }
        }

        _graph = graph.ToDictionary(x => x.Key, IReadOnlySet<string> (x) => x.Value);
    }

    public string SolvePartOne()
    {
        var sets = new HashSet<(string, string, string)>();
        var setBuffer = new string[3];
        foreach (var node in _graph.Keys.Where(x => x[0] == 't'))
        {
            var firstDegree = _graph[node];
            foreach (var another in firstDegree)
            {
                var secondDegree = _graph[another];
                foreach (var last in secondDegree)
                {
                    if (_graph[last].Contains(node))
                    {
                        setBuffer[0] = node;
                        setBuffer[1] = another;
                        setBuffer[2] = last;
                        Array.Sort(setBuffer);
                        sets.Add((setBuffer[0], setBuffer[1], setBuffer[2]));
                    }
                }
            }
        }

        return sets.Count.ToString();
    }

    public string SolvePartTwo()
    {
        var maxClique = GetMaxClique();
        return string.Join(',', maxClique.Order(StringComparer.Ordinal));
    }

    private HashSet<string> GetMaxClique()
    {
        var maximalClique = new HashSet<string>();
        foreach (var node in _graph.Keys)
        {
            var clique = GetMaximalClique(node);
            if (clique.Count > maximalClique.Count)
            {
                maximalClique = clique;
            }
        }

        return maximalClique;
    }

    private HashSet<string> GetMaximalClique(string node)
    {
        var currentClique = new HashSet<string> { node };
        foreach (var another in _graph.Keys)
        {
            if (currentClique.All(x => _graph[x].Contains(another)))
            {
                currentClique.Add(another);
            }
        }
        return currentClique;
    }

    public static Connection ParseLine(ReadOnlySpan<char> line)
    {
        return Connection.FromLine(line);
    }

    public static Solution FromParsed(IReadOnlyList<Connection> entries)
    {
        return new Solution(entries);
    }
}