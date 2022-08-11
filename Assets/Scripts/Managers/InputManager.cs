using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    public static bool GetIfMouseHasMoved()
    {
        return Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0;
    }

    public static bool GetLeftClickDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return false;
        }

        return Input.GetButtonDown("Fire1");
    }

    public static bool GetRightClickDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return false;
        }

        return Input.GetButtonDown("Fire2");
    }
}
