using System;

public class ProjectorColour
{
    public ProjectorColourEnum colour = ProjectorColourEnum.Red;
    
    public void NextColour()
    {
        colour = colour switch
        {
            ProjectorColourEnum.Red => ProjectorColourEnum.Green,
            ProjectorColourEnum.Green => ProjectorColourEnum.Blue,
            ProjectorColourEnum.Blue => ProjectorColourEnum.Yellow,
            ProjectorColourEnum.Yellow => ProjectorColourEnum.Red,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    public void PrevColour()
    {
        colour = colour switch
        {
            ProjectorColourEnum.Red => ProjectorColourEnum.Yellow,
            ProjectorColourEnum.Green => ProjectorColourEnum.Red,
            ProjectorColourEnum.Blue => ProjectorColourEnum.Green,
            ProjectorColourEnum.Yellow => ProjectorColourEnum.Blue,
            _ => throw new ArgumentOutOfRangeException()
        };
        
    }
}

public enum ProjectorColourEnum
{
    Red,
    Green,
    Blue,
    Yellow
}