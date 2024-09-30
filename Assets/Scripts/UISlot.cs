using UnityEngine;
using UnityEngine.EventSystems;

public class UISlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private bool _isDoneContainer;
    private bool _isContainNote;

    public bool IsContainNote => _isContainNote;

    private void Awake()
    {
        _isContainNote = SlotIsContainNote();
    }

    public void OnDrop(PointerEventData eventData)
    {
        var otherItem = eventData.pointerDrag.transform;

        if (!_isDoneContainer)
        {
            otherItem.SetParent(transform);
            otherItem.localPosition = Vector3.zero;
        }

        if(_isDoneContainer && otherItem.GetComponent<Note>().IsFilled)
        {
            otherItem.SetParent(transform);
            otherItem.localPosition = Vector3.zero;
        }
    }

    public bool SlotIsContainNote()
    {
        if (GetComponentInChildren<Note>() != null) return true;//
        return false;
    }

    public void SetContainEmpty()
    {
        _isContainNote = false;
    }
}
