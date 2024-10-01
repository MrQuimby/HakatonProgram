using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class TopMaker : MonoBehaviour
{
    [SerializeField] private Line _prefabLine;
    [SerializeField] private RectTransform _parentLines;

    private void GetNewLine(Executor ex)
    {
        Line newLine = Instantiate(_prefabLine, _parentLines);
        newLine.SetName(ex.Name);
        newLine.SetPoints(ex.Score);
    }

    public void ShowAllExecutors(List<Executor> list)
    {
        List<Executor> topList = new List<Executor>();

        Executor topMen = list[0];
        for (int i = 0; i < list.Count; i++)
        {
            for (int j = i + 1; j < list.Count; j++)
            {
                if (list[i].Score < list[j].Score)
                {
                    topMen = list[i];
                    list[i] = list[j];
                    list[j] = topMen;
                }
            }
        }

        for (int i = 0; i < list.Count; i++)
        {
            GetNewLine(list[i]);
        }
    }

    public void ClearTop()
    {
        Line[] _lines = _parentLines.GetComponentsInChildren<Line>();
        
        foreach (var item in _lines)
        {
            Destroy(item.gameObject);
        }
    }
}
