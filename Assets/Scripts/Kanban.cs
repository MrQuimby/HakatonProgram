using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Kanban : MonoBehaviour
{
    [SerializeField] private GameObject _loginPanel;

    private string _name;

    private NoteMaker _noteMaker;
    private Executor _executor;
    private UISlot _emptySlot;

    private List<Note> _notes;

    private void Start()
    {
        _noteMaker = GetComponent<NoteMaker>();//
        _emptySlot = _noteMaker.GetSlotToNote();
    }

    public void EnterName(string name)
    {
        _name = name;
    }
}
