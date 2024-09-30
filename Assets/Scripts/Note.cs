using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Note : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _lable;
    [SerializeField] private TextMeshProUGUI _taskText;
    private int _dificult;
    [SerializeField] private TextMeshProUGUI _score;

    private bool _isFilled = false;

    private Toggle[] toggles = new Toggle[5];

    public bool IsFilled => _isFilled;

    private void Awake()
    {
        _lable = _lable.GetComponent<TextMeshProUGUI>();
        _taskText = _taskText.GetComponent<TextMeshProUGUI>();
        _score = _score.GetComponent<TextMeshProUGUI>();//
        _dificult = 1;
        toggles = GetComponentsInChildren<Toggle>();
        

    }

    void Start()
    {
        for (int i = 0; i < toggles.Length; i++)
        {
            int index = i; 
            toggles[i].onValueChanged.AddListener((value) => OnToggleChanged(index, value));
        }
    }

    void OnToggleChanged(int index, bool value)
    {
        if (value)
        {
            for (int i = 0; i <= index; i++)
            {
                toggles[i].isOn = true;
            }
        }
        else
        {
            for (int i = index + 1; i < toggles.Length; i++)
            {
                toggles[i].isOn = false;
            }
        }

        ChectDificult();
    }


    private void ChectDificult()
    {
        _dificult = 0;
        foreach (var item in toggles)
        {
            if (item.isOn) _dificult++;
        }
    }

    public void CheckFilling()
    {
        if (_lable.text == "" || _taskText.text == "" || _score.text == "") _isFilled = false;
        else _isFilled = true;
    }

}
