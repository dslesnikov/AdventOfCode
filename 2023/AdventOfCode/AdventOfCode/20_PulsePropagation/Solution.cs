namespace AdventOfCode._20_PulsePropagation;

public class Solution(Dictionary<string, Module> modules) : ISolution<Solution>
{
    public static int Day => 20;

    public static Solution Parse(string s)
    {
        var rawModules = s.Split('\n')
            .Select(line =>
            {
                var parts = line.Split(" -> ");
                var type = line[0];
                var input = type == 'b'
                    ? parts[0]
                    : parts[0][1..];
                var targets = parts[1].Split(", ");
                return new
                {
                    Type = type,
                    Input = input,
                    Targets = targets
                };
            })
            .ToArray();
        var inputs = new Dictionary<string, List<string>>();
        foreach (var module in rawModules)
        {
            foreach (var target in module.Targets)
            {
                if (!inputs.TryGetValue(target, out var value))
                {
                    value = [];
                    inputs[target] = value;
                }
                value.Add(module.Input);
            }
        }
        var modules = new Dictionary<string, Module>();
        foreach (var module in rawModules)
        {
            var outgoing = module.Targets;
            var incoming = inputs.GetValueOrDefault(module.Input, []);
            switch (module.Type)
            {
                case '%':
                    modules[module.Input] = new FlipFlopModule(module.Input, incoming, outgoing);
                    break;
                case '&':
                {
                    modules[module.Input] = new ConjunctionModule(module.Input, incoming, outgoing);
                    break;
                }
                case 'b':
                    modules[module.Input] = new BroadcasterModule(outgoing);
                    break;
            }
        }
        return new Solution(modules);
    }

    public string Part1()
    {
        var lowCount = 0L;
        var highCount = 0L;
        var queue = new Queue<Signal>();
        for (var i = 0; i < 1000; i++)
        {
            queue.Clear();
            queue.Enqueue(new Signal("button", "broadcaster", SignalType.Low));
            while (queue.TryDequeue(out var signal))
            {
                switch (signal.Type)
                {
                    case SignalType.Low:
                        lowCount++;
                        break;
                    case SignalType.High:
                        highCount++;
                        break;
                }
                if (!modules.ContainsKey(signal.To))
                {
                    continue;
                }
                var module = modules[signal.To];
                foreach (var outgoing in module.ReceiveSignal(signal))
                {
                    queue.Enqueue(outgoing);
                }
            }
        }
        return (lowCount * highCount).ToString();
    }

    public string Part2()
    {
        foreach (var module in modules.Values)
        {
            module.Reset();
        }
        var queue = new Queue<Signal>();
        var states = modules
            .Values
            .ToDictionary(x => x, _ => new List<int>(), new ModuleComparer());
        for (var i = 0; i < 10000; i++)
        {
            queue.Clear();
            queue.Enqueue(new Signal("button", "broadcaster", SignalType.Low));
            while (queue.TryDequeue(out var signal))
            {
                if (!modules.ContainsKey(signal.To))
                {
                    continue;
                }
                var module = modules[signal.To];
                foreach (var outgoing in module.ReceiveSignal(signal))
                {
                    queue.Enqueue(outgoing);
                }
            }
            foreach (var (module, items) in states)
            {
                var state = module.GetCurrentState();
                items.Add(state);
            }
        }
        var periodLength = states
            .Where(x => x.Key is ConjunctionModule)
            .Select(x => GetPeriod(x.Value));
        var result = periodLength.Aggregate(1L, (acc, item) => acc * item);
        return result.ToString();
    }

    private static int GetPeriod(IReadOnlyList<int> items)
    {
        for (var length = 1; length < items.Count; length++)
        {
            var period = true;
            for (var index = 0; index < length; index++)
            {
                for (var offset = index; offset < items.Count; offset += length)
                {
                    if (items[index] != items[offset])
                    {
                        period = false;
                        break;
                    }
                }
                if (!period)
                {
                    break;
                }
            }
            if (period)
            {
                return length;
            }
        }
        return -1;
    }
}