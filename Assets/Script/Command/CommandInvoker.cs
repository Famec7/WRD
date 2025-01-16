using System.Collections.Generic;
using UnityEngine;

public class CommandInvoker
{
    private readonly Queue<ICommand> _commandQueue = new Queue<ICommand>();
    private ICommand _currentCommand;

    public bool IsEmpty => _commandQueue.Count == 0 && _currentCommand == null;

    public void AddCommand(ICommand command)
    {
        if (command == null)
        {
#if UNITY_EDITOR
            Debug.LogWarning("Command is null.");
#endif
            return;
        }

        _commandQueue.Enqueue(command);
    }
    
    public void Execute()
    {
        if (_currentCommand == null && _commandQueue.Count > 0)
        {
            _currentCommand = _commandQueue.Dequeue();
        }

        if (_currentCommand == null)
        {
            return;
        }

        if (_currentCommand.Execute())
        {
            _currentCommand.OnComplete();
            _currentCommand = null;
        }
    }

    public void Undo()
    {
        if (_currentCommand == null)
        {
            Debug.LogWarning("Command is null.");
            return;
        }

        _currentCommand.Undo();
        _currentCommand = null;
    }

    public void Reset()
    {
        if (_currentCommand != null)
        {
            _currentCommand.Undo();
            _currentCommand = null;
        }

        while (_commandQueue.Count > 0)
        {
            var command = _commandQueue.Dequeue();
            command.Undo();
        }
    }
}