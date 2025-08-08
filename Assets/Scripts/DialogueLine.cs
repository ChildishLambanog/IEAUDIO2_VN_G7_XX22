using UnityEngine;


[System.Serializable]
public class DialogueLine
{
    public string speaker;
    public string text;
    public AudioClip voiceClip;   // Optional
    public AudioClip musicClip;   // Optional
    public AudioClip sfxClip;

    public Sprite backgroundImage;   // for background
    public GameObject characterObject;   // for character

    public bool isChoice; // Mark if this is a choice
    public bool isEndOfBranch;
    public string[] choices; // The choices shown to player
    public int[] nextLineIndices; // Where to go for each choice

    public string goToSceneName;
}
