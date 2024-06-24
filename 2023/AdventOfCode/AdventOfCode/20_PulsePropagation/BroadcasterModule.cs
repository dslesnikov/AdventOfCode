namespace AdventOfCode._20_PulsePropagation;

public class BroadcasterModule(IReadOnlyList<string> destinations)
    : Module("broadcaster", new[] { "button" }, destinations)
{
    public override IEnumerable<Signal> ReceiveSignal(Signal signal)
    {
        return Destinations.Select(destination => new Signal(Name, destination, signal.Type));
    }

    public override void Reset(){}

    public override int GetCurrentState()
    {
        return 0;
    }
}