using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Executor : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameOnScreen;
    [SerializeField] private TextMeshProUGUI _pointsOnScreen;

    private string _name;
    private int _score;

    public string Name => _name;
    public int Score => _score;

    private void Awake()
    {
        _nameOnScreen = _nameOnScreen.GetComponent<TextMeshProUGUI>();
        _pointsOnScreen = _pointsOnScreen.GetComponent<TextMeshProUGUI>();
    }

    public void SetName(string name)
    {
        _name = name;
        _nameOnScreen.text = name;
    }

    public void AddPoints(int pointsValue)
    {
        _score += pointsValue;
        _pointsOnScreen.text = Convert.ToString(pointsValue);
    }
}
