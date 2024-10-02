using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Note : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _context;
    [SerializeField] private TextMeshProUGUI _taskText;
    [SerializeField] private TextMeshProUGUI _scoreUI;
    
    private int _dificult;
    private bool _isFilled = false;
    private bool _isDone = false;
    private int _score;

    private Toggle[] toggles = new Toggle[5];

    public bool IsFilled => _isFilled;
    public TextMeshProUGUI ScoreUI => _scoreUI;
    public int Score => _score;

    public bool IsDone => _isDone;
    public TextMeshProUGUI Context => _context;
    public TextMeshProUGUI TaskText => _taskText;
    public int Dificult => _dificult;

    private void Awake()
    {
        _score = 0;
        _context = _context.GetComponent<TextMeshProUGUI>();
        _taskText = _taskText.GetComponent<TextMeshProUGUI>();
        _scoreUI = _scoreUI.GetComponent<TextMeshProUGUI>();

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

    public void StartDoingTask()
    {

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
        Debug.Log($"{_taskText.text}");
        if (_context.text == "" || _taskText.text == "") _isFilled = false;
        else _isFilled = true;
    }

    public void SetPoints(int value)
    {
        _score = value;
        _scoreUI.text = $"Score: {value}";
    }

    public void SetNoteDone()
    {
        _isDone = true;
    }


    public void DeleteNote()
    {
        Destroy(this.gameObject);
    }
}
