
using System;
using UnityEngine;

public class MasterManager : Singleton<MasterManager>
{
    public string SessionID;
    public int visualScore;
    public int auralScore;
    public int rwScore;
    public int kinesScore;
    
    private void Start()
    {
        if (SessionID != null)
        {
            Debug.Log("SessionID: " + SessionID);
            if (SessionID == "TEST") Debug.LogWarning("Using TEST SessionID");
            return;
        }
        Debug.LogError("SessionID not set");
        throw new Exception("SessionID not set");
    }
}
