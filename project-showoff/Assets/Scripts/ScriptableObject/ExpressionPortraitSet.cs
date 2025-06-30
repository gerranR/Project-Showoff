using UnityEngine;

[System.Serializable]
public class ExpressionPortraitSet
{
    public enum Expression { Smile, Surprised, Pout, Confused, Cheerful, Neutral, Frown, Sleepy }
    public Expression expression;
    public Sprite sprite;
}
