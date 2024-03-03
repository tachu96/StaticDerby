using UnityEngine;
using UnityEngine.InputSystem;

public class InputDeviceSelector : MonoBehaviour
{
    public GameObject keyboardInstructions;
    public GameObject controllerInstructions;

    void Start()
    {
        // Subscribe to the onDeviceChange event
        InputSystem.onDeviceChange += OnDeviceChange;

        // Initially show the instructions based on the detected input method
        UpdateInstructions();
    }

    void OnDestroy()
    {
        // Unsubscribe from the onDeviceChange event when the script is destroyed
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        // When a device changes (connects/disconnects), update the instructions
        UpdateInstructions();
    }

    void UpdateInstructions()
    {
        // Check if any joystick (controller) is connected
        if (Gamepad.current != null)
        {
            // Joystick connected, show controller instructions
            ShowControllerInstructions();
        }
        else
        {
            // No joystick connected, show keyboard instructions
            ShowKeyboardInstructions();
        }
    }

    void ShowKeyboardInstructions()
    {
        keyboardInstructions.SetActive(true);
        controllerInstructions.SetActive(false);
    }

    void ShowControllerInstructions()
    {
        keyboardInstructions.SetActive(false);
        controllerInstructions.SetActive(true);
    }
}