using UnityEngine;

public class VideoManager : MonoBehaviour
{
    //Loads all video preferences from player prefs if there are audio prefs saved
    public void LoadVideoPrefs()
    {
        if (PlayerPrefs.HasKey("ResolutionIndex"))
        {
            PlayerPrefData.ResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex");
            PlayerPrefData.VideoQualityIndex = PlayerPrefs.GetInt("QualityIndex");

            int isFullscreen = PlayerPrefs.GetInt("IsFullscreen");
            if (isFullscreen == 1)
                PlayerPrefData.IsFullscreen = true;
            else
                PlayerPrefData.IsFullscreen = false;
        }
        else
            Debug.Log("No player preferences for video were loaded.");
    }

    //Saves all video preferences as player prefs
    public void SaveVideoPrefs()
    {
        PlayerPrefs.SetInt("ResolutionIndex", PlayerPrefData.ResolutionIndex);
        PlayerPrefs.SetInt("QualityIndex", PlayerPrefData.VideoQualityIndex);

        if (PlayerPrefData.IsFullscreen)
            PlayerPrefs.SetInt("IsFullscreen", 1);
        else
            PlayerPrefs.SetInt("IsFullscreen", 0);
    } 
}
