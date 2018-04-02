using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStateFactory 
{
    public static GameState Create(string state)
    {
        /// EXAMPLE
        //switch(state)
        //{
            //case "GameRoll":
            //    return new GameRollState(state);
            //case "HiLo":
            //    return new HiLoState(state);
            //case "JackpotRepeat":
            //    return new JackpotRepeatState(state);
            //case "default":
            //    break;
        //}

        Debug.Log("no default state found");
        return null;
    }

}
