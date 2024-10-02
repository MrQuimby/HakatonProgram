using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Kanban : MonoBehaviour
{
    [SerializeField] private string apiKey = "Вставте свой API ключ";
                                            

    [SerializeField] private TextMeshProUGUI _nameOnScreen;
    [SerializeField] private TextMeshProUGUI _pointsOnScreen;
    [SerializeField] private GameObject _loginPanel;

    [SerializeField] private TopMaker _topMaker;

    private string _name;
    private List<Executor> _executors;

    private NoteMaker _noteMaker;
    private Executor _executor;
    private UISlot _emptySlot;

    private Judge _judge;

    private List<Note> _notes;

    private void Start()
    {
        LoadList();
        foreach (var item in _executors)
        {
            Debug.Log(item.ToString());
        }
        if (_executors == null)
        {
            _executors = new List<Executor>();
        }

        _topMaker = GetComponent<TopMaker>();
        _nameOnScreen = _nameOnScreen.GetComponent<TextMeshProUGUI>();
        _pointsOnScreen = _pointsOnScreen.GetComponent<TextMeshProUGUI>();
        _noteMaker = GetComponent<NoteMaker>();

        _emptySlot = _noteMaker.GetSlotToNote();


    }

    public void EnterName(TextMeshProUGUI name)
    {
        _name = name.text;
        CheckExecutor(name.text);
        ShowNameAndPoints();
    }

    private void ShowNameAndPoints()
    {
        _nameOnScreen.text = _executor.Name;
        _pointsOnScreen.text = Convert.ToString(_executor.Score);
    }
    
    public void CloseLoginPanel()
    {
        _loginPanel.SetActive(false);
    }

    private void CheckExecutor(string name)
    {
        bool isContain = false;
        foreach (var item in _executors)
        {
            if (name == item.Name)
            {
                isContain = true;
                _executor = item;
            }
        }
        if (!isContain)
        {
            Executor newEx = new Executor();
            newEx.SetName(name);
            _executors.Add(newEx);//
            _executor = newEx;
        }

        SaveList();
    }

    public Executor GetExecutor()
    {
        return _executor;
    }

    public void TaskIsDoing(Note note)
    {

    }

    public async void TaskIsDone(Note note)
    {

        _judge = new Judge(apiKey);
        int a = await _judge.GetScoreAsync(note.Context.text, note.Dificult, note.TaskText.text);

        _executor.AddPoints(a); // функция добавляет очки пользователю
        SaveList();             // Функция сохраняет данные о баллах пользователя
        ShowNameAndPoints();    // Обновляет данные о баллах на экран

    }

    public void SaveList()
    {
        string allUsers = "";

        foreach (var item in _executors)
        {
            allUsers += $"{item.Name}/{item.Score}/";
        }
        PlayerPrefs.SetString("Executors", allUsers);
        PlayerPrefs.Save();
    }

    private void LoadList()
    {
        _executors = new List<Executor>();

        if (PlayerPrefs.HasKey("Executors"))
        {
            string allUsers = PlayerPrefs.GetString("Executors");
            var usersBuf = allUsers.Split('/');
            

            for (int i = 0; i < usersBuf.Length - 1; i+=2)
            {
                Executor ex = new Executor();
                ex.SetName(usersBuf[i]);
                ex.AddPoints(int.Parse(usersBuf[i+1]));
                _executors.Add(ex);
            }
        } 
    }

    public void ShowTopList()
    {
        _topMaker.ShowAllExecutors(_executors);
    }
}
