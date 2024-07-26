namespace VoiceCommand.Input;

internal struct InputSequence
{
    public InputSequence(InputAction[] actions)
    {
        _actions = actions;
        _currentActionIndex = 0;
    }

    public readonly bool HasFinished => _currentActionIndex >= _actions.Length;

    public readonly InputAction CurrentAction => _actions[_currentActionIndex];

    public InputAction GetNextAction()
    {
        if (_currentActionIndex >= _actions.Length)
        {

            Reset();
        }

        InputAction current = CurrentAction;

        _currentActionIndex++;

        return current;
    }

    public void Reset()
    {
        _currentActionIndex = 0;
    }

    private readonly InputAction[] _actions;
    private int _currentActionIndex;
}