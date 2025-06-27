using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueScriptableObject", menuName = "Scriptable Objects/DialogueScriptableObject")]
public class DialogueScriptableObject : ScriptableObject
{
    public ExpressionPortraitSetScriptableObject GirlExpressionPortraitSet, SpiritExpressionPortraitSet;
    [SerializeField] public DialogueEntry[] dialogueEntries;
}
