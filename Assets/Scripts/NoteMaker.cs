using System.Collections.Generic;
using UnityEngine;

public class NoteMaker : MonoBehaviour
{
    [SerializeField] private UISlot[] _uiSlots;
    [SerializeField] private Note _notePrefab;

    private List<Note> _notes;

    private void Awake()
    {
        _notes = new List<Note>();
    }

    public void GetNewNote()
    {
        Note newNote = Instantiate(_notePrefab, GetSlotToNote().transform);
        _notes.Add(newNote);
    }

    public UISlot GetSlotToNote()
    {
        UISlot slot = null;
        foreach (var item in _uiSlots)
        {
            if (item.SlotIsContainNote()) continue;
            else if (!item.IsContainNote)
                return item;
        }
        return slot;
    }

}
