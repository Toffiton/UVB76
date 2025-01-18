using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DebugContoller : MonoBehaviour
{
    private Controls _controls;

    private void Awake()
    {
        _controls = new Controls();
    }

    private void OnEnable()
    {
        _controls.Main.Enable();
        _controls.Main.LButton.performed += ClearAllPlayerPrefs;
    }

    private void OnDisable()
    {
        _controls.Main.Disable();
        _controls.Main.LButton.performed -= ClearAllPlayerPrefs;
    }
    
    private void ClearAllPlayerPrefs(InputAction.CallbackContext obj)
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(0);
    }
}
