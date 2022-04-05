using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _pause;
    [SerializeField] private GameObject _death;
    [SerializeField] private GameObject _aux;

    [Header("Pause Menu Elements")]
    [SerializeField] private GameObject _resumeButton;
    [SerializeField] private GameObject _mainMenuButton;
    [SerializeField] private GameObject _exitGameButton;
    [SerializeField] private GameObject _confirmExitGameButton;
    [SerializeField] private GameObject _confirmExitLevelButton;
    [SerializeField] private GameObject _backButton;
    [SerializeField] private GameObject _confirmationText;

    [Header("Death Menu Elements")]
    [SerializeField] private GameObject _deathRestartButton;
    [SerializeField] private GameObject _deathMainMenuButton;
    [SerializeField] private GameObject _deathExitGameButton;
    [SerializeField] private GameObject _deathConfirmExitGameButton;
    [SerializeField] private GameObject _deathConfirmExitLevelButton;
    [SerializeField] private GameObject _deathBackButton;
    [SerializeField] private GameObject _deathConfirmationText;

    [Header("Player HUD Elements")]
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private Slider _manaSlider;
    [SerializeField] private Image _currentSpellIcon;
    [SerializeField] private ParticleSystem _currentSpellEffect;

    public float _currentMana = 100;
    public float _maxMana = 100;
    public float _currentHealth = 100;
    public float _maxHealth = 100;

    public void Clicked() => Debug.Log("The button was clicked.");

    private void Start()
    {
        //Makes sure UI sliders are full at the beginning of the scene
        SetHealthSliderMax(_maxHealth);
        RestoreHealthSliderToMax();
        SetManaSliderMax(_maxMana);
        RestoreManaSliderToMax();
    }

    private void Update()
    {
        //Pauses the game if the escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
            PauseGame();
    }

    #region InGameMenus

    //Makes a game object visible
    public void ToggleOnDisplay(GameObject objectToToggle) => objectToToggle.SetActive(true);

    //Makes a game object invisible
    public void ToggleOffDisplay(GameObject objectToToggle) => objectToToggle.SetActive(false);

    //Tells the GameManager to exit the game entirely
    public void ExitGame() => GameManager.Instance.ExitGame();

    //Resumes the game and tells the GameManager to change scenes to the main menu
    public void ExitToMain()
    {
        PauseGame();
        GameManager.Instance.ChangeScene(0);
    }

    //Opens the death menu
    public void OpenPlayerDiedMenu()
    {
        PauseGame();
        _death.SetActive(true);
    }

    //Restarts the current scene
    public void RestartLevel()
    {
        PauseGame();
        GameManager.Instance.RefreshScene();
    }

    public void PauseGame()
    {
        //Checks to see if the game is currently paused
        bool isGamePaused = GameManager.Instance.currentGameState != GameManager.GameState.Paused;

        //Pause or resume the game depending on the current state
        GameManager.Instance.Pause(isGamePaused);

        //Opens or closes the pause menu and makes sure it is reset for use
        if (isGamePaused)
        {
            _pause.SetActive(true);
            ResetPauseMenu();
        }
        else
            _pause.SetActive(false);
    }

    //Makes the appropriate game objects visible/invisible to reset the menu
    public void ResetDeathMenu()
    {
        ToggleOffDisplay(_deathConfirmationText);
        ToggleOffDisplay(_deathConfirmExitGameButton);
        ToggleOffDisplay(_deathConfirmExitLevelButton);
        ToggleOffDisplay(_deathBackButton);
        ToggleOnDisplay(_deathRestartButton);
        ToggleOnDisplay(_deathMainMenuButton);
        ToggleOnDisplay(_deathExitGameButton);
    }

    //Makes the appropriate game objects visible/invisible to reset the menu
    public void ResetPauseMenu()
    {
        ToggleOffDisplay(_confirmationText);
        ToggleOffDisplay(_confirmExitGameButton);
        ToggleOffDisplay(_confirmExitLevelButton);
        ToggleOffDisplay(_backButton);
        ToggleOnDisplay(_resumeButton);
        ToggleOnDisplay(_mainMenuButton);
        ToggleOnDisplay(_exitGameButton);
    }

    #endregion

    #region PlayerHUD

    public void ChangeCurrentSpellIcon(Sprite newIcon)
    {
        _currentSpellIcon.sprite = newIcon;
        _currentSpellEffect.Play();
    }

    public void SetHealthSliderMax(float newMaxValue)
    {
        _maxHealth = newMaxValue;
        _healthSlider.maxValue = newMaxValue;
        _currentHealth = _maxHealth;
        _healthSlider.value = _currentHealth;
    }

    //Restores the slider amount to the max value
    public void RestoreHealthSliderToMax() => _healthSlider.value = _maxHealth;

    //Changes the slider amount by some value
    public void ChangeHealthSliderValue(float changeInValue)
    {
        if (_currentHealth + changeInValue < 0)
            _currentHealth = 0;
        else if (_currentHealth + changeInValue > _maxHealth)
            _currentHealth = _maxHealth;
        else
            _currentHealth += changeInValue;

        _healthSlider.value = _currentHealth;
    }

    //Sets the max slider amount to a value
    public void SetManaSliderMax(float newMaxValue)
    {
        _maxMana = newMaxValue;
        _manaSlider.maxValue = newMaxValue;
    }

    //Restores the slider amount to the max value
    public void RestoreManaSliderToMax() => _manaSlider.value = _maxMana;

    //Changes the slider amount by some value
    public void ChangeManaSliderValue(float changeInValue)
    {
        if (_currentMana + changeInValue < 0)
            _currentMana = 0;
        else if (_currentMana + changeInValue > _maxMana)
            _currentMana = _maxMana;
        else
            _currentMana += changeInValue;

            _manaSlider.value = _currentMana;
    }
    #endregion
}
