using System.Collections.Immutable;

namespace AdventOfCode._20_PulsePropagation;

public class ConjunctionModule : Module
{
    private int _state;
    private readonly ImmutableDictionary<string, int> _inputs;
    private int _highCount;

    public ConjunctionModule(
        string name,
        IReadOnlyList<string> inputs,
        IReadOnlyList<string> destinations) : base(name, inputs, destinations)
    {
        var inputDictionary = new Dictionary<string, int>();
        for (var i = 0; i < inputs.Count; i++)
        {
            inputDictionary[inputs[i]] = i;
        }
        _inputs = inputDictionary.ToImmutableDictionary();
    }

    public override IEnumerable<Signal> ReceiveSignal(Signal signal)
    {
        var signalValue = (int)signal.Type;
        var bit = _inputs[signal.From];
        var currentValue = (_state >> bit) & 1;
        if (currentValue != signalValue)
        {
            _state ^= 1 << bit;
            _highCount += signalValue == 1
                ? 1
                : -1;
        }
        var signalType = _highCount == _inputs.Count
            ? SignalType.Low
            : SignalType.High;
        foreach (var destination in Destinations)
        {
            yield return new Signal(Name, destination, signalType);
        }
    }

    public override void Reset()
    {
        _highCount = 0;
        _state = 0;
    }

    public override int GetCurrentState()
    {
        return _state;
    }
}