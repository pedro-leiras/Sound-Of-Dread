using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public string notePrefix = "Note"; // Assuming your notes have a naming convention like "Note1", "Note2", etc.

    public void ShowNotes(int sequence)
    {
        
        for (int i = 0; i < sequence; i++)
        {
            Transform note = transform.Find(notePrefix + sequence);

            if (note != null)
            {
                note.gameObject.SetActive(true);
            }
        }
    }

}
