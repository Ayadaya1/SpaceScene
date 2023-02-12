using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Character Profile")]
public class CharacterProfile : ScriptableObject
{
    public string Name;
    private Sprite portrait;
    public AudioClip Voice;
    public Font Font;
    public Sprite Portrait
    {
        get 
        {
            SetEmotionType(Emotion);
            return portrait; 
        }
    }
    [System.Serializable]
    public class EmotionPortraits
    {
        public Sprite standart;
        public Sprite happy;
        public Sprite angry;
    }

    public EmotionPortraits emotionPortraits;

    public EmotionType Emotion { get; set; }

    public void SetEmotionType(EmotionType emotion)
    {
        Emotion = emotion;
        switch (emotion)
        {
            case EmotionType.Standart:
                portrait = emotionPortraits.standart;
                break;
            case EmotionType.Happy:
                portrait = emotionPortraits.happy;
                break;
            case EmotionType.Angry:
                portrait = emotionPortraits.angry;
                break;
        }
    }
}

public enum EmotionType
{
    Standart,
    Happy,
    Angry
}