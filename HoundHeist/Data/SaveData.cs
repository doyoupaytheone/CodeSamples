[System.Serializable]
public class SaveData
{
    //Time in minutes that this save slot has on file
    private float _playTime;
    public float PlayTime
    {
        get { return _playTime; }
        set { _playTime = value; }
    }

    //Constructor to create a new SaveData class
    public SaveData(string name, float playTime, int furthestLevel, int totalSpellsAcquired)
    {
        this._playTime = playTime;
    }
}
