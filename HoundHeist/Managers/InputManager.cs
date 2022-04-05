using UnityEngine;
using System;
using System.Collections;

public class InputManager : MonoBehaviour
{
    private MenuManager _menuManager;
    private KeyCode _newKey;
    private bool _waitingForInput;

    private void Awake() => _menuManager = FindObjectOfType<MenuManager>();

    private void Start() => LoadAllCustomInputs();

    //Saves a single input change rather than all of them
    private void SaveCustomInput(string selectedPlayerAction, KeyCode newInput) => PlayerPrefs.SetString(selectedPlayerAction, newInput.ToString());

    //Saves all of the input data to the player prefs
    public void SaveAllCustomInputs()
    {
        PlayerPrefs.SetString("left", PlayerPrefData.Left.ToString());
        PlayerPrefs.SetString("right", PlayerPrefData.Right.ToString());
        PlayerPrefs.SetString("up", PlayerPrefData.Up.ToString());
        PlayerPrefs.SetString("down", PlayerPrefData.Down.ToString());
        PlayerPrefs.SetString("interact", PlayerPrefData.Interact.ToString());
        PlayerPrefs.SetString("companion", PlayerPrefData.Companion.ToString());
        PlayerPrefs.SetString("sprint", PlayerPrefData.Sprint.ToString());
        PlayerPrefs.SetString("primaryAttack", PlayerPrefData.PrimaryAttack.ToString());
        PlayerPrefs.SetString("secondaryAttack", PlayerPrefData.SecondaryAttack.ToString());
    }

    //Loads all of the input data from the player prefs
    public void LoadAllCustomInputs()
    {
        PlayerPrefData.Left = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("left", KeyCode.A.ToString()));
        PlayerPrefData.Right = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("right", KeyCode.D.ToString()));
        PlayerPrefData.Up = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("up", KeyCode.W.ToString()));
        PlayerPrefData.Down = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("down", KeyCode.S.ToString()));
        PlayerPrefData.Interact = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("interact", KeyCode.E.ToString()));
        PlayerPrefData.Companion = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("companion", KeyCode.Space.ToString()));
        PlayerPrefData.Sprint = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("sprint", KeyCode.LeftShift.ToString()));
        PlayerPrefData.PrimaryAttack = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("primaryAttack", KeyCode.Mouse0.ToString()));
        PlayerPrefData.SecondaryAttack = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("secondaryAttack", KeyCode.Mouse1.ToString()));
    }

    //Changes the inputs in the PlayerPrefData script
    private void ChangeInput(string selectedPlayerAction, KeyCode newInput)
    {
        switch (selectedPlayerAction)
        {
            case ("left"):
                PlayerPrefData.Left = newInput;
                break;
            case ("right"):
                PlayerPrefData.Right = newInput;
                break;
            case ("up"):
                PlayerPrefData.Up = newInput;
                break;
            case ("down"):
                PlayerPrefData.Down = newInput;
                break;
            case ("interact"):
                PlayerPrefData.Interact = newInput;
                break;
            case ("companion"):
                PlayerPrefData.Companion = newInput;
                break;
            case ("sprint"):
                PlayerPrefData.Sprint = newInput;
                break;
            case ("primaryAttack"):
                PlayerPrefData.PrimaryAttack = newInput;
                break;
            case ("secondaryAttack"):
                PlayerPrefData.SecondaryAttack = newInput;
                break;
            default:
                Debug.LogError("The selected key input does not work as intended in the " + this.name + " script.");
                return;
        }

        SaveCustomInput(selectedPlayerAction, newInput);
    }

    //Waits for the player to press a key to assign the input
    private IEnumerator WaitForInput()
    {
        while (_waitingForInput)
        {
            Debug.Log("Checking for input.");
            
            //Loop over all the keycodes
            foreach (KeyCode tempKey in Enum.GetValues(typeof(KeyCode)))
            {
                //Captures any key that has been pressed
                if (Input.GetKeyDown(tempKey))
                {
                    Debug.Log(tempKey.ToString() + " has been pressed.");
                    _newKey = tempKey;
                    _waitingForInput = false;
                    break;
                }
            }

            yield return null;
        }
    }

    //Lets the player choose a key/mouse input to change
    public void AssignNewInput(string selectedControl) => StartCoroutine(AssignInput(selectedControl));
    private IEnumerator AssignInput(string selectedControl)
    {
        _waitingForInput = true;
        
        yield return WaitForInput();

        ChangeInput(selectedControl, _newKey);
        _menuManager.ResetCustomControls();
    }
}
