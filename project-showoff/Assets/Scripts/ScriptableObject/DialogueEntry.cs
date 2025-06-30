using UnityEngine;

[System.Serializable]
public class DialogueEntry
{
    public enum Speaker { Human, Spirit };
    public Speaker lineSpeaker;
    public string dialogueLine;
    public enum Portrait { Smile, Surprised, Pout, Confused, Cheerful, Neutral, Frown, Sleepy };
    public Portrait linePortrait;
}
