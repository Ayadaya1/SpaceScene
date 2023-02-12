using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDialogue : MonoBehaviour
{
    public DialogueBase dialogue;
    private void OnTriggerEnter(Collider other)
    {
        DialogueManager.instance.EnqueueDialogue(dialogue);
    }
}
