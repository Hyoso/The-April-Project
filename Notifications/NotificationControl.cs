using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.SimpleAndroidNotifications;
using System;

public class NotificationControl : MonoBehaviour
{
    public void Rate()
    {
        Application.OpenURL("http://www.google.com/");
    }

    public void ScheduleNotification(TimeSpan timeToShow, string message = "Your daily bonus is available!")
    {
        var notificationParams = new NotificationParams
        {
            Id = UnityEngine.Random.Range(0, int.MaxValue),
            Delay = timeToShow,
            Title = "Jacks or Better",
            Message = message,
            Ticker = "Collect your bonus!",
            Sound = true,
            Vibrate = true,
            Light = false,
            SmallIcon = NotificationIcon.Bell,
            SmallIconColor = new Color(0, 0.5f, 0),
            LargeIcon = "app_icon"
        };

        Debug.Log("Started notification at: " + (DateTime.Now + timeToShow).ToString());

        NotificationManager.SendCustom(notificationParams);
    }

    public void CancelAll()
    {
        NotificationManager.CancelAll();
    }
}
