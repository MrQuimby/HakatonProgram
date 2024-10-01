using UnityEngine;
using UnityEngine.EventSystems;

public class UISlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private Kanban _kanban;
    [SerializeField] private bool _isDoneContainer;
    [SerializeField] private bool _isDoingContainer;
    private bool _isContainNote;

    public bool IsContainNote => _isContainNote;

    private void Awake()
    {
        _kanban = _kanban.GetComponent<Kanban>();
        _isContainNote = SlotIsContainNote();
    }

    public void OnDrop(PointerEventData eventData)
    {
        var otherItem = eventData.pointerDrag.transform;

        if (_isDoingContainer && otherItem.GetComponent<Note>().IsFilled)
        {
            SetSlot(otherItem, transform);
            _kanban.TaskIsDoing(otherItem.GetComponent<Note>());

        }
        else if(_isDoneContainer && otherItem.GetComponent<Note>().IsFilled)
        {
            Note note = otherItem.GetComponent<Note>();
            SetSlot(otherItem, transform);
            
            if(!note.IsDone) _kanban.TaskIsDone(note);
            note.SetNoteDone();
        }
        else if(!_isDoingContainer && !_isDoneContainer)
        {
            SetSlot(otherItem, transform);
            
        }
    }

    private void SetSlot(Transform item, Transform parent)
    {
        item.SetParent(parent);
        item.localPosition = Vector3.zero;
    }

    public bool SlotIsContainNote()
    {
        if (GetComponentInChildren<Note>() != null) return true;
        return false;
    }

    public void SetContainEmpty()
    {
        _isContainNote = false;
    }

}
