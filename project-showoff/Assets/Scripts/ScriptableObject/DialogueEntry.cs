using UnityEngine;

[System.Serializable]
public class DialogueEntry
{
    public enum Speaker { Human, Spirit };
    public Speaker lineSpeaker;
    public string dialogueLine;
    public enum Portrait { a, b, c };
    public Portrait linePortrait;
}
