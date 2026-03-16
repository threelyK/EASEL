
using System;
using UnityEngine;

public class MasterManager : Singleton<MasterManager>
{
    public string SessionID;

    private void Start()
    {
        if (SessionID != null)
        {
            Debug.Log("SessionID: " + SessionID);
            if (SessionID == "Test") Debug.LogWarning("Using Test SessionID");
            return;
        }
        Debug.LogError("SessionID not set");
        throw new Exception("SessionID not set");
    }
}
