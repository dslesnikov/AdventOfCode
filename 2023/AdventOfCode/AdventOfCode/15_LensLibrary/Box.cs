using System.Collections;

namespace AdventOfCode._15_LensLibrary;

public class Box : IEnumerable<int>
{
    private readonly LinkedList<int> _lenses = [];
    private readonly Dictionary<string, LinkedListNode<int>> _labels = new();

    public void Execute(Operation operation)
    {
        switch (operation)
        {
            case Remove remove:
                if (_labels.TryGetValue(remove.Label, out var removeNode))
                {
                    _lenses.Remove(removeNode);
                    _labels.Remove(remove.Label);
                }
                return;
            case Upsert upsert:
                if (_labels.TryGetValue(upsert.Label, out var upsertNode))
                {
                    upsertNode.Value = upsert.FocalLength;
                }
                else
                {
                    _labels[upsert.Label] = _lenses.AddLast(upsert.FocalLength);
                }
                return;
            default:
                throw new ArgumentOutOfRangeException(nameof(operation));
        }
    }

    public IEnumerator<int> GetEnumerator()
    {
        return _lenses.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}