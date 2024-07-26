namespace VoiceCommand.Input;

internal struct InputSequence(InputAction[] actions)
{
    public bool HasFinished { get; private set; }
    public readonly InputAction CurrentAction => _actions[_currentActionIndex];

    private readonly InputAction[] _actions = actions;
    private int _currentActionIndex = 0;

    public InputAction GetNextAction()
    {
        if (_currentActionIndex >= _actions.Length)
        {
            HasFinished = true;
            Reset();
        }

        _currentActionIndex++;

        return CurrentAction;
    }

    public void Reset()
    {
        _currentActionIndex = 0;
    }
}