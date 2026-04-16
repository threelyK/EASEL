using System;

public class ProjectorColour
{
    public static Action<ProjectorColourEnum> OnPColourChange;
        
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
        
        OnPColourChange?.Invoke(colour);
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
        
        OnPColourChange?.Invoke(colour);
    }
}

public enum ProjectorColourEnum
{
    Red,
    Green,
    Blue,
    Yellow
}