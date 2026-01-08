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



    // private Vector2 movement;
    // private bool interactPressed = false;
    // private bool submitPressed = false;

    // private static inputManager instance;

    // public static inputManager GetInstance()
    // {
    //     return instance;
    // }

    // private void Awake()
    // {
    //     if (instance != null)
    //     {
    //         Debug.Log("Found more than one Input Manager in the scene. Removing Duplicate..");
    //         Destroy(gameObject);
    //         return;
    //     }
    //     instance = this;
    // }

    // public void MovePressed(InputAction.CallbackContext context)
    // {
    //     if (context.performed)
    //     {
    //         movement = context.ReadValue<Vector2>();
    //     }
    //     else if (context.canceled)
    //     {
    //         movement = context.ReadValue<Vector2>();
    //     }
    // }

    // public void InteractButtonPressed(InputAction.CallbackContext context)
    // {
    //     if (context.performed)
    //     {
    //         interactPressed = true;
    //     }
    //     else if (context.canceled)
    //     {
    //         interactPressed = false;
    //     }
    // }

    // public void SubmitPressed(InputAction.CallbackContext context)
    // {
    //     if (context.performed)
    //     {
    //         submitPressed = true;
    //     }
    //     else if (context.canceled)
    //     {
    //         submitPressed = false;
    //     }
    // }

    // public Vector2 GetMoveDirection()
    // {
    //     return movement;
    // }

    // // for any of the below 'Get' methods, if we're getting it then we're also using it,
    // // which means we should set it to false so that it can't be used again until actually
    // // pressed again.

    // public bool GetInteractPressed()
    // {
    //     bool result = interactPressed;
    //     interactPressed = false;
    //     return result;
    // }

    // public bool GetSubmitPressed()
    // {
    //     bool result = submitPressed;
    //     submitPressed = false;
    //     return result;
    // }

    // public void RegisterSubmitPressed()
    // {
    //     submitPressed = false;
    // }
}
