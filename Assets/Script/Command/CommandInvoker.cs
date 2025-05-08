using System.Collections.Generic;
using UnityEngine;

public class CommandInvoker
{
    private readonly List<ICommand> _currentCommands = new List<ICommand>();

    public bool IsEmpty => _currentCommands.Count == 0;

    public void AddCommand(ICommand command)
    {
        if (command == null)
        {
#if UNITY_EDITOR
            Debug.LogWarning("Command is null.");
#endif
            return;
        }

        if (command is ActiveSkillCommand)
        {
            foreach (var cmd in _currentCommands)
            {
                if (cmd is ActiveSkillCommand)
                {
                    _currentCommands.Remove(cmd);
                    break;
                }
            }
        }
        
        _currentCommands.Add(command);
    }
    
    public void Execute()
    {
        if (_currentCommands == null)
        {
            return;
        }

        for (int i = _currentCommands.Count - 1; i >= 0; i--)
        {
            var command = _currentCommands[i];

            if (command.Execute())
            {
                command.OnComplete();
                _currentCommands.RemoveAt(i);
            }
        }
    }

    public void Undo()
    {
        if (_currentCommands == null)
        {
            Debug.LogWarning("Command is null.");
            return;
        }

        foreach (var command in _currentCommands)
        {
            command.Undo();
        }
        _currentCommands.Clear();
    }

    public void Reset()
    {
        foreach (var command in _currentCommands)
        {
            command.Undo();
        }
        _currentCommands.Clear();
    }
}