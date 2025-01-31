using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolManager : MonoBehaviour
{
    public enum Mode
    {
        Drawing,
        Token
    }

    public Mode currentMode = Mode.Token; // Default mode

    private void Update()
    {
        // Example: Switch between modes using the Tab key
        //if (Input.GetKeyDown(KeyCode.Tab))
        //{
        //    ToggleMode();
        //}
    }

    public void ToggleMode()
    {
        // Toggle between Drawing and Token modes
        currentMode = currentMode == Mode.Drawing ? Mode.Token : Mode.Drawing;
        Debug.Log("Switched to " + currentMode + " mode.");
    }
    public void SelectMode(int value)
    {
        Mode mode;
        if(value == 0) mode = Mode.Drawing;
        else mode = Mode.Token;//if(value == 1)

        currentMode = mode;
        Debug.Log("Switched to " + currentMode + " mode.");
    }
}
