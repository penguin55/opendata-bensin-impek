using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    public KeyCode moveUp { get; set; }
    public KeyCode moveLeft { get; set; }
    public KeyCode moveDown { get; set; }
    public KeyCode moveRight { get; set; }
    public KeyCode dash { get; set; }
    public KeyCode activateItem { get; set; }

    void Awake()
    {
        instance = this;

        /*Assign each keycode when the game starts.
		 * Loads data from PlayerPrefs so if a user quits the game, 
		 * their bindings are loaded next time. Default values
		 * are assigned to each Keycode via the second parameter
		 * of the GetString() function
		 */
        moveUp = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("upKey", "W"));
        moveLeft = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("leftKey", "A"));
        moveDown = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("downKey", "S"));
        moveRight = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("rightKey", "D"));
        dash = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("dashKey", "Space"));
        activateItem = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("activateKey", "E"));
        Debug.Log((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("activateKey", "E")));
    }
}
