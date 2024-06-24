namespace AdventOfCode._20_PulsePropagation;

public class FlipFlopModule(string name, IReadOnlyList<string> inputs, IReadOnlyList<string> destinations) : Module(name, inputs, destinations)
{
    private bool IsOn { get; set; }

    public override IEnumerable<Signal> ReceiveSignal(Signal signal)
    {
        if (signal.Type == SignalType.High)
        {
            yield break;
        }
        IsOn = !IsOn;
        foreach (var destination in Destinations)
        {
            yield return new Signal(Name, destination, IsOn ? SignalType.High : SignalType.Low);
        }
    }

    public override void Reset()
    {
        IsOn = false;
    }

    public override int GetCurrentState()
    {
        return IsOn ? 1 : 0;
    }
}