using UnityEngine;
using System;
using System.Collections.Generic;

/*#if UNITY_IOS
using Unity.Notifications.iOS;
#endif*/

public class MobileNotificationManageriOS : Singleton<MobileNotificationManageriOS>
{
/*#if UNITY_IOS
    private string notificationId_every10Hour = "everyday10";
    private string notificationId_every24Hour = "everyday24";

    private void Start()
    {
        Notification1_Every10Hour();
        Notification1_Every24Hour();

        /*
        iOSNotificationCenter.OnRemoteNotificationReceived += recievedNotification =>
        {
            Debug.Log("Recieved notification " + notification.Identifier + "!");
        };

        iOSNotification notificationIntentData = iOSNotificationCenter.GetLastRespondedNotification();

        if (notificationIntentData != null)
        {
            Debug.Log("App was opened with notification!");
        }
        
    }

    private void Notification1_Every10Hour()
    {
        iOSNotificationTimeIntervalTrigger timeTrigger = new iOSNotificationTimeIntervalTrigger()
        {
            TimeInterval = new TimeSpan(10, 0, 0),
            Repeats = true,
        };

        iOSNotification notification = new iOSNotification()
        {
            Identifier = notificationId_every10Hour,
            Title = "Only 1% Can Reach Level 200 😱",
            Subtitle = "",
            Body = "Do you have the skill???🙄",
            ShowInForeground = true,
            ForegroundPresentationOption = (PresentationOption.Alert),
            CategoryIdentifier = "category_everyday",
            ThreadIdentifier = "comeBack10",
            Trigger = timeTrigger,
        };

        iOSNotificationCenter.ScheduleNotification(notification);
    }

    private void Notification1_Every24Hour()
    {
        iOSNotificationTimeIntervalTrigger timeTrigger = new iOSNotificationTimeIntervalTrigger()
        {
            TimeInterval = new TimeSpan(24, 0, 0),
            Repeats = true,
        };

        iOSNotification notification = new iOSNotification()
        {
            Identifier = notificationId_every24Hour,
            Title = "Don't be late!🙄",
            Subtitle = "",
            Body = "You have an appointment with the daily puzzle.🧡💙💜",
            ShowInForeground = true,
            ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
            CategoryIdentifier = "category_everyday",
            ThreadIdentifier = "comeBack24",
            Trigger = timeTrigger,
        };

        iOSNotificationCenter.ScheduleNotification(notification);
    }

#endif*/
}