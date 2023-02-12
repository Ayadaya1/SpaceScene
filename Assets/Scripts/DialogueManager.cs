using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private readonly List<char> punctuationChars = new List<char>
    {
        '.',
        ',',
        '!',
        '?',
        ' '
    };
    public static DialogueManager instance;
    private void Awake()
    {
        if(instance!=null)
        {
            Debug.LogWarning("Fix this" + gameObject.name);
        }
        else
        {
            instance = this;
        }
    }

    public GameObject dialogueBox;

    public Text dialogueName;
    public Text dialogueText;
    public Image dialoguePortrait;
    public float delay = 0.001f;

    public bool dialogueRunning;

    private bool isCurrentlyTyping;
    private string completeText;
    public Queue<DialogueBase.Info> dialogueInfo = new Queue<DialogueBase.Info>();

    public void EnqueueDialogue(DialogueBase db)
    {
        dialogueBox.SetActive(true);
        dialogueRunning = true;
        dialogueInfo.Clear();
        foreach(DialogueBase.Info info in db.dialogueInfo)
        {
            dialogueInfo.Enqueue(info);
        }
        DequeueDialogue();
    }

    public void DequeueDialogue()
    {
        if (isCurrentlyTyping)
        {
            CompleteText();
            StopAllCoroutines();
            isCurrentlyTyping = false;
            return;
        }
        if (dialogueInfo.Count == 0)
        {
            EndOfDialogue();
            return;
        }


        DialogueBase.Info info = dialogueInfo.Dequeue();
        completeText = info.Text;
        Debug.Log(info.character.Name);
        Debug.Log(info.Text);
        Debug.Log(info.character.Font.name);
        dialogueName.text = info.character.Name;
        dialogueText.text = info.Text;
        dialogueText.font = info.character.Font;
        info.ChangeEmotion();
        dialoguePortrait.sprite = info.character.Portrait;  
        dialogueText.text = "";

        StartCoroutine(TypeText(info));
    }
    private void CompleteText()
    {
        dialogueText.text = completeText;
    }
    IEnumerator TypeText(DialogueBase.Info info)
    {
        isCurrentlyTyping = true;
        foreach(char c in info.Text.ToCharArray())
        {
            yield return new WaitForSeconds(delay);
            dialogueText.text += c;
            if (!punctuationChars.Contains(c))
            {
                AudioManager.instance.PlayClip(info.character.Voice);   
            }
            else if (c!=' ')
            {
                yield return new WaitForSeconds(0.25f);
            }
        }
        isCurrentlyTyping = false;
    }
    public void EndOfDialogue()
    {
        dialogueBox.SetActive(false);
        dialogueRunning = false;
    }
    
}
