using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    #region SerializeField Variables

    [Header("Menu Groups")]
    [SerializeField] private CanvasFadeEffect _mainMenuGroup;
    [SerializeField] private CanvasFadeEffect _fileSelectGroup;
    [SerializeField] private CanvasFadeEffect _dataManagementGroup;
    [SerializeField] private CanvasFadeEffect _audioSettingsGroup;
    [SerializeField] private CanvasFadeEffect _videoSettingsGroup;
    [SerializeField] private CanvasFadeEffect _customControlsGroup;
    [SerializeField] private CanvasFadeEffect _cosmeticsGroup;
    [SerializeField] private CanvasFadeEffect _attributionGroup;

    [Header("Save Slot Objects")]
    [SerializeField] private TextMeshProUGUI[] _playTimesText;
    [SerializeField] private TextMeshProUGUI[] _playTimesManageText;

    [Header("Video Selections")]
    [SerializeField] private TMP_Dropdown _resolutionDropdown;
    [SerializeField] private TMP_Dropdown _qualityDropdown;

    [Header("Audio Selections")]
    [SerializeField] private Slider _mainVolumeSlider;
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _sfxVolumeSlider;
    [SerializeField] private Slider _uiVolumeSlider;

    [Header("Player Input Customization Objects")]
    [SerializeField] private TextMeshProUGUI _leftButton;
    [SerializeField] private TextMeshProUGUI _rightButton;
    [SerializeField] private TextMeshProUGUI _upButton;
    [SerializeField] private TextMeshProUGUI _downButton;
    [SerializeField] private TextMeshProUGUI _interactButton;
    [SerializeField] private TextMeshProUGUI _companionButton;
    [SerializeField] private TextMeshProUGUI _sprintButton;
    [SerializeField] private TextMeshProUGUI _primaryAttackButton;
    [SerializeField] private TextMeshProUGUI _secondaryAttackButton;

    #endregion

    private AudioManager _audioManager;
    private Resolution[] _resolutions;
    private int _selectedSaveSlot = 0;

    private void Awake() => _audioManager = FindObjectOfType<AudioManager>();

    private void Start() => ResetResolutionOptions();

    //Tells the game manager to begin the game
    public void StartGame(int slotIndex) => GameManager.Instance.ToNextScene();

    //Tells the game manager to quit the game
    public void ExitGame() => GameManager.Instance.ExitGame();

    #region AccessSaveData

    //Tells the game manager to delete the selected save slot
    public void DeleteSaveSlot(int slotIndex) => GameManager.Instance.DeleteSaveData(slotIndex);

    //Tells the game manager to select a specific save slot to manipulate
    public void SelectCurrentSaveSlot(int slotIndex)
    {
        //Select the save slot in GM
        _selectedSaveSlot = slotIndex;
        bool isSlotEmpty = GameManager.Instance.SelectSaveSlot(slotIndex);

        //Closes the main menu to see house view
        ToggleMenu(_mainMenuGroup);
    }

    public void LoadMenuSettings(float[] playTimes)
    {
        for (int i = 0; i < playTimes.Length; i++)
        {
            if (playTimes[i] < 1)
            {
                _playTimesText[i].text = "0:00";
                _playTimesManageText[i].text = "0:00";
            }
            else
            {
                string minutes = String.Format("{0:00}", playTimes[i] % 60);
                _playTimesText[i].text = (int)(playTimes[i] / 60) + ":" + minutes;
                _playTimesManageText[i].text = (int)(playTimes[i] / 60) + ":" + minutes;
            }
        }

        //Sets the values of the sliders for settings
        ResetVolumeValues();
        //Sets the text on the custom input buttons
        ResetCustomControls();
    }

    public void SavePreferences()
    {
        //Saves the pref values as player pref data
        PlayerPrefData.MainVolume = _mainVolumeSlider.value;
        PlayerPrefData.MusicVolume = _musicVolumeSlider.value;
        PlayerPrefData.SfxVolume = _sfxVolumeSlider.value;
        PlayerPrefData.UIVolume = _uiVolumeSlider.value;
        PlayerPrefData.ResolutionIndex = _resolutionDropdown.value;
        PlayerPrefData.VideoQualityIndex = _qualityDropdown.value;
        PlayerPrefData.IsFullscreen = Screen.fullScreen;
    }

    #endregion

    #region ChangeValues

    public void SetMainVolume(float value) => _audioManager.SetGroupVolume(value, "MainVolume");

    public void SetMusicVolume(float value) => _audioManager.SetGroupVolume(value, "MusicVolume");

    public void SetSFXVolume(float value) => _audioManager.SetGroupVolume(value, "SfxVolume");

    public void SetUIVolume(float value) => _audioManager.SetGroupVolume(value, "UiVolume");
    
    public void SetVideoQuality(int value) => QualitySettings.SetQualityLevel(value);

    public void SetFullscreen(bool isFullscreen) => Screen.fullScreen = isFullscreen;

    public void SetResolution(int value)
    {
        Resolution resolution = _resolutions[value];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    #endregion

    #region ResetDisplays

    private void ResetResolutionOptions()
    {
        //Gets access to all possible screen resolutions of the current computer
        _resolutions = Screen.resolutions;
        //Clears the default resolutions options
        _resolutionDropdown.ClearOptions();

        //Creates an array of strings for each resolution option
        List<string> options = new List<string>();
        for (int i = 0; i < _resolutions.Length; i++)
        {
            string option = _resolutions[i].width + " x " + _resolutions[i].height;
            options.Add(option);

            //Checks to see if the available resolution is what the player is currently using and if so, set it to that
            if (_resolutions[i].width == Screen.currentResolution.width && _resolutions[i].height == Screen.currentResolution.height)
                PlayerPrefData.ResolutionIndex = i;
        }

        //Adds the array of resolution option strings to the dropdown
        _resolutionDropdown.AddOptions(options);

        //Sets the displayed value to the one currently being used
        _resolutionDropdown.value = PlayerPrefData.ResolutionIndex;
        _resolutionDropdown.RefreshShownValue();
    }

    //Makes a game object visible
    public void ToggleOnDisplay(GameObject objectToToggle) => objectToToggle.SetActive(true);

    //Makes a game object invisible
    public void ToggleOffDisplay(GameObject objectToToggle) => objectToToggle.SetActive(false);

    //Makes the appropriate game objects visible/invisible to reset the menu
    public void ToggleDataMenu()
    {
        var fileFadeTime = 0;
        var dataFadeTime = 0;

        if (!_fileSelectGroup.isVisible) fileFadeTime = 1;
        if (!_dataManagementGroup.isVisible) dataFadeTime = 1;
        
        _fileSelectGroup.ToggleFade(fileFadeTime);
        _dataManagementGroup.ToggleFade(dataFadeTime);

        //Refreshes the settings data
        GameManager.Instance.CollectAndUpdateData();
    }

    public void ToggleMenu(CanvasFadeEffect canvasToToggle) => canvasToToggle.ToggleFade(0);

    //Resets the current values for the custom player inputs
    public void ResetCustomControls()
    {
        _leftButton.text = PlayerPrefData.Left.ToString();
        _rightButton.text = PlayerPrefData.Right.ToString();
        _upButton.text = PlayerPrefData.Up.ToString();
        _downButton.text = PlayerPrefData.Down.ToString();
        _interactButton.text = PlayerPrefData.Interact.ToString();
        _companionButton.text = PlayerPrefData.Companion.ToString();
        _sprintButton.text = PlayerPrefData.Sprint.ToString();
        _primaryAttackButton.text = PlayerPrefData.PrimaryAttack.ToString();
        _secondaryAttackButton.text = PlayerPrefData.SecondaryAttack.ToString();
    }

    public void ResetVolumeValues()
    {
        //Updates the position of the sliders based on pref values
        _mainVolumeSlider.value = PlayerPrefData.MainVolume;
        _musicVolumeSlider.value = PlayerPrefData.MusicVolume;
        _sfxVolumeSlider.value = PlayerPrefData.SfxVolume;
        _uiVolumeSlider.value = PlayerPrefData.UIVolume;

        //Sets the pref values in the audio manager
        _audioManager.SetGroupVolume(_mainVolumeSlider.value, "MainVolume");
        _audioManager.SetGroupVolume(_musicVolumeSlider.value, "MusicVolume");
        _audioManager.SetGroupVolume(_sfxVolumeSlider.value, "SfxVolume");
        _audioManager.SetGroupVolume(_uiVolumeSlider.value, "UiVolume");
    }

    #endregion
}
