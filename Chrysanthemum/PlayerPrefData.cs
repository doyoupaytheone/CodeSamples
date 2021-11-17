//Created by Justin Simmons

using UnityEngine;

public static class PlayerPrefData
{
    #region VolumePrefs

    private static float _mainVolume;
    public static float MainVolume
    {
        get { return _mainVolume; }
        set { _mainVolume = value; }
    }

    private static float _musicVolume;
    public static float MusicVolume
    {
        get { return _musicVolume; }
        set { _musicVolume = value; }
    }

    private static float _sfxVolume;
    public static float SfxVolume
    {
        get { return _sfxVolume; }
        set { _sfxVolume = value; }
    }

    private static float _uiVolume;
    public static float UIVolume
    {
        get { return _uiVolume; }
        set { _uiVolume = value; }
    }

    #endregion

    #region VideoPrefs

    private static int _resolutionIndex;
    public static int ResolutionIndex
    {
        get { return _resolutionIndex; }
        set { _resolutionIndex = value; }
    }

    private static int _videoQualityIndex;
    public static int VideoQualityIndex
    {
        get { return _videoQualityIndex; }
        set { _videoQualityIndex = value; }
    }

    private static bool _isFullscreen = false;
    public static bool IsFullscreen
    {
        get { return _isFullscreen; }
        set { _isFullscreen = value; }
    }

    #endregion

    #region CustomInputPrefs

    private static KeyCode _left;
    public static KeyCode Left 
    { 
        get { return _left; } 
        set { _left = value; }
    }

    private static KeyCode _right;
    public static KeyCode Right
    {
        get { return _right; }
        set { _right = value; }
    }

    private static KeyCode _lookUp;
    public static KeyCode LookUp
    {
        get { return _lookUp; }
        set { _lookUp = value; }
    }

    private static KeyCode _lookDown;
    public static KeyCode LookDown
    {
        get { return _lookDown; }
        set { _lookDown = value; }
    }

    private static KeyCode _interact;
    public static KeyCode Interact
    {
        get { return _interact; }
        set { _interact = value; }
    }

    private static KeyCode _jump;
    public static KeyCode Jump
    {
        get { return _jump; }
        set { _jump = value; }
    }

    private static KeyCode _sprint;
    public static KeyCode Sprint
    {
        get { return _sprint; }
        set { _sprint = value; }
    }

    private static KeyCode _primaryAttack;
    public static KeyCode PrimaryAttack
    {
        get { return _primaryAttack; }
        set { _primaryAttack = value; }
    }

    private static KeyCode _secondaryAttack;
    public static KeyCode SecondaryAttack
    {
        get { return _secondaryAttack; }
        set { _secondaryAttack = value; }
    }

    #endregion
}
