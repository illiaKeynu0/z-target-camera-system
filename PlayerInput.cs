using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public static Vector2 MoveInput;
    public static Vector2 MouseInput;
    
    public void OnMove(InputValue value)
    {
        MoveInput = value.Get<Vector2>();
    }
    
    public void OnLook(InputValue value)
    {
        MouseInput = value.Get<Vector2>();

        if (TargetFinder.OnTarget)
        {
            if (MouseInput.x > .5)
            {
                TargetFinder.TargetSelect(1);
            }
            if (MouseInput.x <= -.5)
            {
                TargetFinder.TargetSelect(-1);
            }
        }
    }
    
    public void OnTargetLock(InputValue button)
    {
        if (button.isPressed && !TargetFinder.OnTarget)
        {
            TargetFinder.TargetLockOn();
        }
        else if (button.isPressed && TargetFinder.OnTarget)
        {
            TargetFinder.TargetLockOff();
        }
    }
}