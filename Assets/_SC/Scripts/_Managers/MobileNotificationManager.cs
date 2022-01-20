using UnityEngine;
using System;
using System.Collections.Generic;

/*#if UNITY_ANDROID
using Unity.Notifications.Android;
#endif*/

public class MobileNotificationManager : Singleton<MobileNotificationManager>
{
/*#if UNITY_ANDROID
    private AndroidNotificationChannel defaultNotificationChannel;

    private void Start()
    {
        defaultNotificationChannel = new AndroidNotificationChannel()
        {
            Id = "default_channel",
            Name = "Default Channel",
            Description = "For Generic notifications",
            Importance = Importance.Default,
        };

        AndroidNotificationCenter.RegisterNotificationChannel(defaultNotificationChannel);

        Notification1_Every10Hour();
        Notification1_Every24Hour();

        /*
        AndroidNotificationCenter.NotificationReceivedCallback receivedNotificationHandler = delegate (AndroidNotificationIntentData data)
        {
            var msg = "Notification received : " + data.Id + "\n";
            msg += "\n Notification received: ";
            msg += "\n .Title: " + data.Notification.Title;
            msg += "\n .Body: " + data.Notification.Text;
            msg += "\n .Channel: " + data.Channel;
            Debug.Log(msg);
        };

        AndroidNotificationCenter.OnNotificationReceived += receivedNotificationHandler;

        var notificationIntentData = AndroidNotificationCenter.GetLastNotificationIntent();

        if (notificationIntentData != null)
        {
            Debug.Log("App was opened with notification!");
        }
        
    }


    private void Notification1_Every10Hour()
    {
        AndroidNotification notification = new AndroidNotification()
        {
            Title = "Only 1% Can Reach Level 200 😱",
            Text = "Do you have the skill???🙄",
            //FireTime = DateTime.Now.AddHours(10),
            //RepeatInterval = new TimeSpan(10, 0, 0),

            FireTime = DateTime.Now.AddMinutes(5),
            RepeatInterval = new TimeSpan(0, 5, 0),
        };

        var identifier = AndroidNotificationCenter.SendNotification(notification, "default_channel");
    }

    private void Notification1_Every24Hour()
    {
        AndroidNotification notification = new AndroidNotification()
        {
            Title = "Don't be late!🙄",
            Text = "You have an appointment with the daily puzzle.🧡💙💜",
            //FireTime = DateTime.Now.AddHours(24),
            //RepeatInterval = new TimeSpan(24, 0, 0),

            FireTime = DateTime.Now.AddMinutes(10),
            RepeatInterval = new TimeSpan(0, 12, 0),
        };

        var identifier = AndroidNotificationCenter.SendNotification(notification, "default_channel");
    }

#endif*/
}