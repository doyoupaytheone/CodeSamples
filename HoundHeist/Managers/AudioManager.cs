//Created by Justin Simmons

using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    [SerializeField] private AudioMixer _worldAudio;
    [SerializeField] private AudioSource _uiAudio;
    [SerializeField] private AudioSource _musicAudio;
    [SerializeField] private AudioClip _defaultUIClip;
    [SerializeField] private AudioClip[] _musicTracks;
    [SerializeField] private AudioMixerSnapshot volumeOffSnapshot;
    [SerializeField] private AudioMixerSnapshot volumeOnSnapshot;

    private int FADETIME = 2;

    private void Start()
    {
        //Assigns the default UI audio clip
        _uiAudio.clip = _defaultUIClip;

        LoadAudioPrefs();
        FadeAudioIn();
    }

    //Loads all audio preferences from player prefs if there are audio prefs saved
    public void LoadAudioPrefs()
    {
        if (PlayerPrefs.HasKey("MainVolume"))
        {
            PlayerPrefData.MainVolume = PlayerPrefs.GetFloat("MainVolume");
            PlayerPrefData.MusicVolume = PlayerPrefs.GetFloat("MusicVolume");
            PlayerPrefData.SfxVolume = PlayerPrefs.GetFloat("SfxVolume");
            PlayerPrefData.UIVolume = PlayerPrefs.GetFloat("UiVolume");
        }
        else
            Debug.Log("No player preferences for audio were loaded.");
    }

    //Saves all audio preferences as player prefs
    public void SaveAudioPrefs()
    {
        PlayerPrefs.SetFloat("MainVolume", PlayerPrefData.MainVolume);
        PlayerPrefs.SetFloat("MusicVolume", PlayerPrefData.MusicVolume);
        PlayerPrefs.SetFloat("SfxVolume", PlayerPrefData.SfxVolume);
        PlayerPrefs.SetFloat("UiVolume", PlayerPrefData.UIVolume);
    }

    //Sets the audio for a group to a specific level
    public void SetGroupVolume(float volume, string groupName)
    {
        PlayerPrefs.SetFloat(groupName, volume);
        _worldAudio.SetFloat(groupName, MapSliderVolumeToDecible(volume));
    }

    //Changes the audio to a new track
    public void ChangeMusicTrack(int trackIndex)
    {
        _musicAudio.Stop();
        _musicAudio.clip = _musicTracks[trackIndex];
        _musicAudio.Play();
    }

    //Fades the audio in over a specified time
    public void FadeAudioIn() => volumeOnSnapshot.TransitionTo(FADETIME);

    //Fades the audio out over a specified time
    public void FadeAudioOut() => volumeOffSnapshot.TransitionTo(FADETIME);

    //Creates a sound when selecting UI
    public void UISound(AudioClip clip)
    {
        if (clip == null) _uiAudio.clip = _defaultUIClip; //if the parameter is null, assign the default audio clip
        else _uiAudio.clip = clip; //If not, assign the new audio clip

        _uiAudio.Play(); //Play the audio
    }

    //Ends the sound of the UI audio
    public void StopUISound() => _uiAudio.Stop();

    //Adjusts the volume value on the mixer proportional to the slider amount returned by the UI
    private float MapSliderVolumeToDecible(float value)
    {
        return Mathf.RoundToInt(Mathf.Log10(value) * 20);
    }
}
