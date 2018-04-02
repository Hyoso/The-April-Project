using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

abstract public class GameState
{
    string stateName = "default";

    public GameState(string stateName)
    {
        this.stateName = stateName;
    }

    public virtual void Enter()
    {
        throw new NotImplementedException();
    }

    public virtual void Exit()
    {
        throw new NotImplementedException();
    }

    public string GetName()
    {
        return stateName;
    }
}