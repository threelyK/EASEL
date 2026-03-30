using System;
using UnityEngine;

public static class RadioActions
{
    public static Action<bool> OnRadioReady;
    
    public static Action<AudioClip> OnClipGenerated;
    public static Action<string> ResponseGenerated;
}