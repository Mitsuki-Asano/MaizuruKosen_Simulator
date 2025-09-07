using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueNode
{
    public enum NodeType { Message, Choice }

    public NodeType nodeType;
    [TextArea] public string message;
    public List<string> choices;
    public List<int> nextNodeIndex;

    public int rootID = 0; // どの選択肢ルートに属するか
}
