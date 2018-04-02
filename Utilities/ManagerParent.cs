using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerParent : Singleton<ManagerParent>
{
    private void Start()
    {
        dontDestroyOnLoad = true;
        base.Start();
    }
}
