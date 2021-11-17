//Created by Justin Simmons

[System.Serializable]
public class SaveData
{
    //Name for the save slot
    private string _saveName;
    public string SaveName
    {
        get { return _saveName; }
        set { _saveName = value; }
    }
    //Time in minutes that this save slot has on file
    private float _playTime;
    public float PlayTime
    {
        get { return _playTime; }
        set { _playTime = value; }
    }
    //Index of furthest level reached
    private int _furthestLevelIndex;
    public int FurthestLevelIndex
    {
        get { return _furthestLevelIndex; }
        set { _furthestLevelIndex = value; }
    }
    //Index of the spells acquired in the save
    private int _totalSpellsAcquired = 0;
    public int TotalSpellsAquired
    {
        get { return _totalSpellsAcquired; }
        set { _totalSpellsAcquired = value; }
    }

    //Constructor to create a new SaveData class
    public SaveData(string name, float playTime, int furthestLevel, int totalSpellsAcquired)
    {
        this._saveName = name;
        this._playTime = playTime;
        this._furthestLevelIndex = furthestLevel;
        this._totalSpellsAcquired = totalSpellsAcquired;
    }
}
