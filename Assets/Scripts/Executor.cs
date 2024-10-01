
public class Executor
{
    private string _name;
    private int _score = 0;

    public string Name => _name;
    public int Score => _score;

    public void SetName(string name)
    {
        _name = name;
    }

    public void AddPoints(int pointsValue)
    {
        _score += pointsValue;
    }
}
