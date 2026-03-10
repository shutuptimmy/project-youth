using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class inputManager : MonoBehaviour
{
    public void MovePressed(InputAction.CallbackContext context)
    {
        if (context.performed || context.canceled)
        {
            gameEventsManager.instance.inputEvents.MovePressed(context.ReadValue<Vector2>());
        }
    }

    public void SubmitPressed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            gameEventsManager.instance.inputEvents.SubmitPressed();
        }
        else if (context.canceled)
        {
            gameEventsManager.instance.inputEvents.SubmitReleased();
        }
    }

    public void InteractPressed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            gameEventsManager.instance.inputEvents.InteractPressed();
        }
    }

    public void GetExp(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            gameEventsManager.instance.playerEvents.ExperienceGained(30);
        }
    }

    public void QuestLogTogglePressed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            gameEventsManager.instance.inputEvents.QuestLogTogglePressed();
        }
    }

    public void PausePressed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            gameEventsManager.instance.inputEvents.PausePressed();
        }
    }

    public void DragPressed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            gameEventsManager.instance.inputEvents.DragPressed();
        }
        else if (context.canceled)
        {
            gameEventsManager.instance.inputEvents.DragReleased();
        }
    }
}
