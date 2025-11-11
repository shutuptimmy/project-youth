using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inputEvents
{
    public inputEventContext inputEventContext { get; private set; } = inputEventContext.DEFAULT;

    public void ChangeInputEventContext(inputEventContext newContext)
    {
        this.inputEventContext = newContext;
    }

    public event Action<Vector2> onMovePressed;
    public void MovePressed(Vector2 moveDir)
    {
        onMovePressed?.Invoke(moveDir);
    }

    public event Action<inputEventContext> onSubmitPressed;
    public void SubmitPressed()
    {
        onSubmitPressed?.Invoke(this.inputEventContext);
    }

    public event Action<inputEventContext> onInteractPressed;
    public void InteractPressed()
    {
        onInteractPressed?.Invoke(this.inputEventContext);
    }

    public event Action onQuestLogTogglePressed;
    public void QuestLogTogglePressed()
    {
        if (onQuestLogTogglePressed != null)
        {
            onQuestLogTogglePressed();
        }
    }

    public event Action<inputEventContext> onPausePressed;
    public void PausePressed()
    {
        onPausePressed?.Invoke(this.inputEventContext);
    }
}
