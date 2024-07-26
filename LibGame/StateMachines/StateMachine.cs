namespace LibGame.StateMachines;

public interface IStateMachineState
{
    public void Tick();
}

public sealed class EmptyStateMachineState : IStateMachineState
{
    public void Tick() { }
}

public sealed class StateMachine
{
    public StateMachine()
    {
        this.Current = new EmptyStateMachineState();
    }

    public IStateMachineState Current { get; private set; }

    public void Transition(IStateMachineState newState)
    {
        this.Current = newState;
    }

    public void Tick()
    {
        this.Current.Tick();
    }
}
