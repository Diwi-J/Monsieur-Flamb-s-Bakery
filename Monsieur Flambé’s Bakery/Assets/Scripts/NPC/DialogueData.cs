using UnityEngine;

[System.Serializable]
public class Dialogue
{
    //NPC Data.
    public string npcName;
    [TextArea(3, 10)]
    public string[] sentences;
}

