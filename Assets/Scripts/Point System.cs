
public class PointSystem : Singleton<PointSystem>
{
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
            // sufficient funds
        }
    }

    public void GainPoints(int amount)
    {
        _points += amount;
    }
    
}
