namespace AdventOfCode._20_PulsePropagation;

public abstract class Module(string name, IReadOnlyList<string> inputs, IReadOnlyList<string> destinations)
{
    public string Name { get; } = name;

    public IReadOnlyList<string> Inputs { get; } = inputs;

    protected IReadOnlyList<string> Destinations { get; } = destinations;

    public abstract IEnumerable<Signal> ReceiveSignal(Signal signal);

    public abstract void Reset();

    public abstract int GetCurrentState();
}