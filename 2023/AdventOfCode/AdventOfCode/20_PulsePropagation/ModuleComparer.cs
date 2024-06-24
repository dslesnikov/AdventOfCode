namespace AdventOfCode._20_PulsePropagation;

public class ModuleComparer : IEqualityComparer<Module>
{
    public bool Equals(Module? x, Module? y)
    {
        if (ReferenceEquals(x, y))
        {
            return true;
        }
        if (ReferenceEquals(x, null))
        {
            return false;
        }
        if (ReferenceEquals(y, null))
        {
            return false;
        }
        return string.Equals(x.Name, y.Name, StringComparison.OrdinalIgnoreCase);
    }

    public int GetHashCode(Module obj)
    {
        return StringComparer.OrdinalIgnoreCase.GetHashCode(obj.Name);
    }
}