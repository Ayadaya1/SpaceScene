using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
public class DialogueBase : ScriptableObject
{
    [System.Serializable]
    public class Info
    {
        public CharacterProfile character;
        public CharacterProfile Character
        {
            get { return character; }
            set { character = value; }
        }
        [TextArea(4, 8)]
        public string Text;

        public EmotionType characterEmotion;

        public void ChangeEmotion()
        {
            character.Emotion = characterEmotion;
        }
        public void SetText(string str)
        {
            Text = str;
        }
    }
    [Header("Insert Dialogue Information Below")]
    public List<Info> dialogueInfo;
    public void AddDialogues(DialogueBase extra)
    {
        foreach(Info i in extra.dialogueInfo)
        {
            dialogueInfo.Add(i);
        }
    }
}