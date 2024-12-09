namespace AdventOfCode.Day09DiskFragmenter;

public class Solution : IFromText<Solution>
{
    public static int Day => 9;

    private readonly IReadOnlyList<int> _input;

    private Solution(IReadOnlyList<int> input)
    {
        _input = input;
    }

    public string SolvePartOne()
    {
        var defragmented = new List<(int Id, int Length)>
        {
            (0, _input[0])
        };
        var lastIndex = (_input.Count - 1) % 2 == 0
            ? _input.Count - 1
            : _input.Count - 2;
        var freeSpaceIndex = 1;
        var freeSpaceLength = _input[1];
        for (var i = lastIndex; i > freeSpaceIndex; i -= 2)
        {
            var fragmentLength = _input[i];
            while (fragmentLength > 0)
            {
                if (freeSpaceLength == 0)
                {
                    var nextFragment = freeSpaceIndex + 1;
                    defragmented.Add((nextFragment / 2, _input[nextFragment]));
                    freeSpaceIndex += 2;
                    freeSpaceLength = _input[freeSpaceIndex];
                    if (freeSpaceIndex >= i)
                    {
                        break;
                    }
                }

                if (freeSpaceIndex + 1 == i)
                {
                    defragmented.Add((i / 2, fragmentLength));
                    break;
                }
                var canMove = Math.Min(freeSpaceLength, fragmentLength);
                defragmented.Add((i / 2, canMove));
                fragmentLength -= canMove;
                freeSpaceLength -= canMove;
            }
        }

        var result = 0L;
        var length = 0L;
        foreach (var fragment in defragmented)
        {
            result += fragment.Id * (fragment.Length * (length + fragment.Length + length - 1) / 2);
            length += fragment.Length;
        }

        return result.ToString();
    }

    public string SolvePartTwo()
    {
        var defragmented = new LinkedList<(int Id, int Index, int Length)>(_input
            .Select((item, index) => index % 2 == 0
                ? (index / 2, index, item)
                : (-1, index, item)));
        var currentToMove = defragmented.Last;
        while (currentToMove != null)
        {
            if (currentToMove.Value.Id == -1)
            {
                currentToMove = currentToMove.Previous;
                continue;
            }
            var fragmentLength = currentToMove.Value.Length;
            var currentFree = defragmented.First;
            while (currentFree!.Value.Index < currentToMove.Value.Index)
            {
                if (currentFree.Value.Id != -1 ||
                    currentFree.Value.Length < fragmentLength)
                {
                    currentFree = currentFree.Next;
                    continue;
                }

                var spareLength = currentFree.Value.Length - fragmentLength;
                currentFree.Value = (currentToMove.Value.Id, currentFree.Value.Index, fragmentLength);
                if (spareLength > 0)
                {
                    defragmented.AddAfter(currentFree, (-1, currentFree.Value.Index, spareLength));
                }

                var totalSpaceAround = fragmentLength;
                if (currentToMove.Previous?.Value.Id == -1)
                {
                    totalSpaceAround += currentToMove.Previous.Value.Length;
                    defragmented.Remove(currentToMove.Previous);
                }
                if (currentToMove.Next?.Value.Id == -1)
                {
                    totalSpaceAround += currentToMove.Next.Value.Length;
                    defragmented.Remove(currentToMove.Next);
                }
                currentToMove.Value = (-1, currentToMove.Value.Index, totalSpaceAround);
                break;
            }
            currentToMove = currentToMove.Previous;
        }

        var result = 0L;
        var length = 0L;
        foreach (var fragment in defragmented)
        {
            if (fragment.Id != -1)
            {
                result += fragment.Id * (fragment.Length * (length + fragment.Length + length - 1) / 2);
            }
            length += fragment.Length;
        }

        return result.ToString();
    }

    public static Solution FromText(string text)
    {
        return new Solution(text.Select(x => int.Parse([x])).ToArray());
    }
}