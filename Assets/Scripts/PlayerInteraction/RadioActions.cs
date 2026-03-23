using System;

public static class RadioActions
{
    public static Action<bool> OnRadioReady;


    public static Action<float> OnClipGenerated;
    public static Action<string> ResponseGenerated;
}