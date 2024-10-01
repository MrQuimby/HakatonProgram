using TMPro;
using UnityEngine;

public class Line : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _points;

    private void Awake()
    {
        _name = _name.GetComponent<TextMeshProUGUI>();
        _points = _points.GetComponent<TextMeshProUGUI>();  
    }

    public void SetName(string name)
    {
        _name.text = name;
    }

    public void SetPoints(int value)
    {
        _points.text = value.ToString();
    }
}
