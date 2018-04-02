using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventsManager : Singleton<GameEventsManager>
{
#region EVENTS
    /// <summary>
    /// Add all custom events here.
    /// These events must be called in the correct locations to ensure the game behaves as intended.
    /// </summary>
    public UnityEvent endCardDealEvents = new UnityEvent();
    public UnityEvent gameLossEvents = new UnityEvent();
    public UnityEvent gameWinEvents = new UnityEvent();
    public UnityEvent gameFinishedEvents = new UnityEvent();
    public UnityEvent enterHiLoEvents = new UnityEvent();
    public UnityEvent enterFTLEvents = new UnityEvent();
    public UnityEvent hiloTableEvents = new UnityEvent();
    public UnityEvent collectWinEvents = new UnityEvent(); // todo
    public UnityEvent timerTickEvents = new UnityEvent();
    public UnityEvent hiloGambleEvents = new UnityEvent(); // todo
    public UnityEvent gambleHiAndWinEvents = new UnityEvent();
    public UnityEvent gambleLoAndWinEvents = new UnityEvent();
#endregion // EVENTS

    public override void Start()
    {
        base.Start();
    }
}
