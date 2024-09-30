using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
{
    [SerializeField] private CanvasGroup _canvasGroupItemObgects;
    
    private CanvasGroup _canvasGroupToDrag;
    private Canvas _mainCanvas;
    private RectTransform _rectTransform;

    private Transform _parentSlot;
    private bool _isSelected;


    private void Start()
    {
        _canvasGroupItemObgects = _canvasGroupItemObgects.GetComponent<CanvasGroup>();
        _canvasGroupToDrag = GetComponent<CanvasGroup>(); 
        _mainCanvas = GetComponentInParent<Canvas>();        
        _rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        ShowUp();
        _canvasGroupToDrag.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _mainCanvas.scaleFactor;
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        ShowUp();
        
        transform.localPosition = Vector3.zero;
        _canvasGroupToDrag.blocksRaycasts = true;

        _rectTransform.parent.GetComponent<UISlot>().SetContainEmpty();
    }

    private void ShowUp()
    {
        var slotTransform = _rectTransform.parent;
        slotTransform.SetAsLastSibling();
        var blockTransform = slotTransform.parent;
        blockTransform.SetAsLastSibling();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_isSelected)
        {
            ReturnNote();
            _canvasGroupItemObgects.blocksRaycasts = false;
        }
        else
        {
            ShowUp();

            _canvasGroupItemObgects.blocksRaycasts = true;
            _parentSlot = _rectTransform.parent;
            transform.SetParent(_mainCanvas.transform);
            _rectTransform.localScale = Vector3.one * 3; 
            transform.localPosition = Vector3.zero;

            _isSelected = true;
        }
    }

    public void ReturnNote()
    {
        transform.SetParent(_parentSlot);
        _rectTransform.localScale = Vector3.one;
        transform.localPosition = Vector3.zero;
        _isSelected = false;
        _canvasGroupItemObgects.blocksRaycasts = false;
    }
}
