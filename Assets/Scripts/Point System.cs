
using System;

public class PointSystem : Singleton<PointSystem>
{
    public event Action onPointChange;
    public int _points;
    
    public int GetPoints()
    {
        return _points;
    }

    public bool EnoughPoints(int amount)
    {
        return _points >= amount;
    }
    
    public void SpendPoints(int amount) 
    {
        if (amount > _points) 
        {
            // insufficient funds
        } else
        {
            _points -= amount;
            onPointChange?.Invoke();
            // sufficient funds
        }
    }

    public void GainPoints(int amount)
    {
        _points += amount;
        onPointChange?.Invoke();
    }
    
}
