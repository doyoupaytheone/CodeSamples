//Created by Justin Simmons

using UnityEngine;

//Stores the dialogue data of a conversation (character name, image, and sentences they speak)
[System.Serializable]
public class DialogueData
{
    public Sprite[] speakerSprites;
    public string[] speakerNames;
    public AudioClip[] speakerSounds;
    public int[] currentSentenceSpeaker;
    [TextArea(2,5)] public string[] sentences;
}
