using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameManager : MonoSingleton<GameManager>
{
    private MenuManager _menuManager;
    private AudioManager _audioManager;
    private VideoManager _videoManager;
    private InputManager _inputManager;
    private GUIManager _guiManager;

    [HideInInspector] public CanvasGroup fadeCanvas;

    [HideInInspector] public GameState currentGameState;
    public enum GameState
    {
        MainMenu,
        Paused,
        Playing,
    }

    private bool _isFrozen = false;
    public bool IsFrozen
    {
        get { return _isFrozen; }
    }

    private SaveData[] _saveSlots = new SaveData[3];
    public SaveData[] SaveSlots
    {
        get { return _saveSlots; }
        set { _saveSlots = value; }
    }

    private int _currentSaveSlot;
    public int CurrentSaveSlot
    {
        get { return _currentSaveSlot; }
    }

    private static string _baseDirectory;
    private static string[] _filepaths = new string[3];
    private static float _sessionStartTime;

    private void Awake()
    {
        //Makes sure there is only one game manager
        if (_instance)
        {
            //If there is one and this is not it, destroy this object
            if (_instance != this)
                Destroy(this.gameObject);
        }
        //Make this the primary instance and don't destroy it when changing scenes
        else
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }

        //Finds the fade canvas group
        fadeCanvas = GetComponentInChildren<CanvasGroup>();
    }

    private void Start()
    {
        FindScriptsInScene();
        SaveDataSetup();
    }

    #region UtilityMethods

    //When the game is shut down, save the game
    private void OnApplicationQuit()
    {
        SavePrefs();
        SaveGame();
    }

    private void FindScriptsInScene()
    {
        _menuManager = FindObjectOfType<MenuManager>();
        _audioManager = FindObjectOfType<AudioManager>();
        _videoManager = FindObjectOfType<VideoManager>();
        _inputManager = FindObjectOfType<InputManager>();
    }

    //Logs a time that allows the manager to calculate the total game time played
    public void LogNewSessionStartTime() => _sessionStartTime = Time.time;

    //Closes the game
    public void ExitGame() => Application.Quit();

    //Freezes the player's inputs
    public void ToggleFreezePlayer() => _isFrozen = !IsFrozen;

    //Pauses the game
    public void Pause(bool paused)
    {
        //Makes sure you can't pause the game in the main menu
        if (currentGameState != GameState.MainMenu)
        {
            //"Stops" the game
            if (paused)
            {
                //Freezes movement in game
                Time.timeScale = 0f;
                //Freezes player input
                _isFrozen = true;
                //Changes the game state
                currentGameState = GameState.Paused;
            }
            //"Starts" the game
            else
            {
                Time.timeScale = 1f;
                _isFrozen = false;
                currentGameState = GameState.Playing;
            }
        }
    }

    public void PlayerHasDied()
    {
        //Finds the gui manager and asks it to open the death menu
        _guiManager = FindObjectOfType<GUIManager>();
        _guiManager.OpenPlayerDiedMenu();
    }

    #endregion

    #region SceneManagement

    //When a new scene finishes loading
    private void OnLevelWasLoaded(int level)
    {
        FindScriptsInScene();

        //If going back to the main menu, reload the appropriate player prefs to be adjusted
        if (level == 0)
        {
            currentGameState = GameState.MainMenu;
            SaveDataSetup();
        }
        //Otherwise, log the starting time of the new session (to record time in game)
        else
        {
            currentGameState = GameState.Playing;
            LogNewSessionStartTime();
        }

        //Audio fades in
        _audioManager.FadeAudioIn();

        //Fade screen in
        StartCoroutine(ToggleFadeCanvas());
    }

    //Changes the scene to a particular index
    public void ChangeScene(int sceneIndex)
    {
        SavePrefs();
        SaveGame();

        //Begins the transition to the next scene
        StartCoroutine(TransitionToNextScene(sceneIndex));
    }

    //Changes the scene to a particular index
    public void ToNextScene()
    {
        //Gets the current build index
        int current = SceneManager.GetActiveScene().buildIndex;

        //Checks if there is another scene
        //If not, go to the main menu
        if (current + 1 > SceneManager.sceneCountInBuildSettings)
            StartCoroutine(TransitionToNextScene(0));
        else
        {
            SavePrefs();
            SaveGame();

            //Begins the transition to the next scene
            StartCoroutine(TransitionToNextScene(current + 1));
        }
    }

    //Refreshes the current scene
    public void RefreshScene() => ChangeScene(SceneManager.GetActiveScene().buildIndex);

    IEnumerator TransitionToNextScene(int sceneIndex)
    {
        //Audio fades out
        _audioManager.FadeAudioOut();
        //Fade in waiting screen
        StartCoroutine(ToggleFadeCanvas());

        yield return new WaitForSeconds(2f);

        //Loads the next scene
        SceneManager.LoadSceneAsync(sceneIndex);
    }

    IEnumerator ToggleFadeCanvas()
    {
        if (fadeCanvas.alpha > 0.8)
        {
            while (fadeCanvas.alpha > 0)
            {
                fadeCanvas.alpha -= 0.01f;
                yield return new WaitForSeconds(0.01f);
            }
        }
        else
        {
            while (fadeCanvas.alpha < 1)
            {
                fadeCanvas.alpha += 0.01f;
                yield return new WaitForSeconds(0.01f);
            }
        }
    }

    #endregion

    #region DataAndPreferences

    //Saves all player preference data
    private void SavePrefs()
    {
        if (_inputManager != null)
            _inputManager.SaveAllCustomInputs();

        if (_audioManager != null)
            _audioManager.SaveAudioPrefs();

        if (_videoManager != null)
            _videoManager.SaveVideoPrefs();

        PlayerPrefs.Save();
    }

    //Loads all player preference data
    private void LoadPrefs()
    {
        if (_inputManager != null)
            _inputManager.LoadAllCustomInputs();

        if (_audioManager != null)
            _audioManager.LoadAudioPrefs();

        if (_videoManager != null)
            _videoManager.LoadVideoPrefs();
    }

    private void SaveDataSetup()
    {
        //Creates save data slot and initializes it if it hasn't already been created
        InitializeSaveSlots();

        //Assigns the directory for save slots
        VerifyDirectory();

        //Creates names for all filepaths
        for (int i = 0; i < _filepaths.Length; i++)
            _filepaths[i] = _baseDirectory + "/SaveSlot" + (i + 1) + ".dat";

        //Verifies if each file exists and if not, creates one
        for (int i = 0; i < _filepaths.Length; i++)
            VerifyFile(i);

        LoadAllSaveData();
    }

    //Creates new SaveData classes for each save slot and initializes them with default values.
    private void InitializeSaveSlots()
    {
        for (int i = 0; i < _saveSlots.Length; i++)
        {
            if (_saveSlots[i] == null)
                _saveSlots[i] = new SaveData("", 0, 0, 0);
        }
    }

    //Checks to make sure the desired directory exists and if not, it will make one
    private static void VerifyDirectory()
    {
        _baseDirectory = Application.persistentDataPath + "/SaveData";

        if (!Directory.Exists(_baseDirectory))
            Directory.CreateDirectory(_baseDirectory);
    }

    //Checks to make sure the desired file exists and if not, it will make one
    private void VerifyFile(int slotIndex)
    {
        string file = _filepaths[slotIndex];

        if (!File.Exists(file))
        {
            //Creates a new file at the desired filepath
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fileStream = File.Create(file);
            bf.Serialize(fileStream, _saveSlots[slotIndex]);
            fileStream.Close();
            Debug.Log("File was created at " + file);
        }
        else
            Debug.Log("File already exists at " + file);
    }

    private void LoadAllSaveData()
    {
        //Loads any saved player preferences
        LoadPrefs();

        //For each save slot, it loads any data available
        for (int i = 0; i < _saveSlots.Length; i++)
            _saveSlots[i] = LoadGame(_filepaths[i]);

        //Update all UI elements based on current loaded data
        CollectAndUpdateData();
    }

    public SaveData LoadGame(string filepath)
    {
        //Checks to make sure file still exists
        if (!File.Exists(filepath))
            return default;
        else
        {
            //Checks to be sure the file isn't empty
            if (filepath.Length == 0)
                return default;
            else
            {
                //Opens the filepath, writes the new data in i, and closes the file
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = new FileStream(filepath, FileMode.Open);
                SaveData saveSlot = (SaveData)bf.Deserialize(file);
                file.Close();

                return saveSlot;
            }
        }
    }

    //Changes the currently selected save file and returns true if the slot is empty
    public bool SelectSaveSlot(int index)
    {
        _currentSaveSlot = index;

        if (_saveSlots[index].PlayTime == 0)
            return true;
        else
            return false;
    }

    public void SaveGame()
    {
        //Calculates the amount of time that has passed since the gaming session began
        _saveSlots[_currentSaveSlot].PlayTime += (float) ((Time.time - _sessionStartTime) / 60);

        //Opens the filepath, writes the new data in i, and closes the file
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = new FileStream(_filepaths[_currentSaveSlot], FileMode.Open);
        bf.Serialize(file, _saveSlots[_currentSaveSlot]);
        file.Close();
    }

    public void DeleteSaveData(int slotIndex)
    {
        //Resets SaveData class
        _saveSlots[slotIndex].PlayTime = 0;

        //Resaves the new, empty save slot
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = new FileStream(_filepaths[slotIndex], FileMode.Open);
        bf.Serialize(file, _saveSlots[slotIndex]);
        file.Close();

        //Reloads the new save data and updates the UI
        CollectAndUpdateData();
    }

    public void CollectAndUpdateData()
    {
        //Collects total time data
        float[] times = new float[_saveSlots.Length];
        for (int i = 0; i < _saveSlots.Length; i++)
            times[i] = _saveSlots[i].PlayTime;

        if (currentGameState == GameState.MainMenu)
        {
            //Updates UI with all save data collected from prefs and slots
            _menuManager.LoadMenuSettings(times);
        }
    }

    #endregion
}
