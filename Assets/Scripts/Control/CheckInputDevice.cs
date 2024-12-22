using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class CheckInputDevice : MonoBehaviour
{
    public enum eInputState
    {
        MouseKeyboard,
        Controller
    };

    private eInputState m_State = eInputState.MouseKeyboard;
    
    [SerializeField] private TextMeshProUGUI[] logs;

    void Update()
    {
        eInputState newState = GetCurrentInputState();
        if (newState != m_State)
        {
            m_State = newState;
        }
    }

    public eInputState GetInputState()
    {
        return m_State;
    }

    private eInputState GetCurrentInputState()
    {
        var keyboard = Keyboard.current;
        var mouse = Mouse.current;
        var gamepad = Gamepad.current;

        // logs[0].text = (gamepad != null).ToString();
        // logs[1].text = (gamepad != null && Gamepad.current.allControls.Any(x => x is ButtonControl button && !x.synthetic)).ToString();

        if (gamepad != null && gamepad.allControls.Any(control => control is ButtonControl button && button.isPressed))
        {
            Cursor.visible = false;
            return eInputState.Controller;
        }

        if (keyboard != null && keyboard.anyKey.isPressed || mouse != null && (mouse.delta.ReadValue() != Vector2.zero))
        {
            Cursor.visible = true;
            return eInputState.MouseKeyboard;
        }

        return m_State;
    }
}