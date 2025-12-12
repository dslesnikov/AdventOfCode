namespace AdventOfCode.Day8Playground;

public class UnionFind
{
    private readonly int[] _rank;
    private readonly int[] _root;
    
    public int ComponentsCount { get; private set; }

    public UnionFind(int size)
    {
        _rank = new int[size];
        _root = new int[size];
        for (var i = 0; i < size; i++)
        {
            _root[i] = i;
        }
        ComponentsCount = size;
    }

    public int Find(int value)
    {
        if (_root[value] != value)
        {
            _root[value] = Find(_root[value]);
        }
        return _root[value];
    }
    
    public void Union(int a, int b)
    {
        var rootA = Find(a);
        var rootB = Find(b);
        if (rootA == rootB)
        {
            return;
        }
        
        ComponentsCount--;
        if (_rank[rootA] < _rank[rootB])
        {
            _root[rootA] = rootB;
        }
        else if (_rank[rootA] > _rank[rootB])
        {
            _root[rootB] = rootA;
        }
        else
        {
            _root[rootB] = rootA;
            _rank[rootA]++;
        }
    }
}