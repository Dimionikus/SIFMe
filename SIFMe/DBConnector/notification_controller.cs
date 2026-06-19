using SIFMe.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace SIFMe.DBConnector
{
    public class notification_controller
    {
        const string techdata_name = "tech_data";
        bool isLoaded = false;
        int last_id = 1;
        public List<notification> not_list;
        public notification_controller()
        {
            not_list = connector.get_context().notifications.ToList();
            techdata_update();
            NotificationsTimersUpdate();
        }
        private void techdata_update()
        {
            string current_directory = AppDomain.CurrentDomain.BaseDirectory;
            string techdata_folder = current_directory + techdata_name;
            string techdata_link = techdata_folder + "\\" + techdata_name + "3.txt";
            if (!Directory.Exists(techdata_folder)) Directory.CreateDirectory(techdata_folder);
            if (File.Exists(techdata_link) && !isLoaded) last_id = int.Parse(File.ReadAllText(techdata_link));
            File.WriteAllText(techdata_link, last_id.ToString());
            isLoaded = true;
        }
        public void save_not(notification noti)
        {
            if (noti.ID == 0)
            {
                noti.ID = last_id;
                last_id++;
                techdata_update();
                connector.get_context().notifications.Add(noti);
            }
            else
            {
                var upd_link = connector.get_context().notifications.First(fl => fl.ID == noti.ID);
                upd_link.title = noti.title;
                upd_link.description = noti.description;
                upd_link.play_type = noti.play_type;
                upd_link.interval = noti.interval;
            }
            connector.get_context().SaveChanges();
            not_list = connector.get_context().notifications.ToList();
            NotificationsTimersUpdate();
        }
        public void del_not(notification noti)
        {
            if (connector.get_context().notifications.ToList().Exists(notif => notif.ID == noti.ID)) {
                notification notific = connector.get_context().notifications.ToList().First(notif => notif.ID == noti.ID);
                connector.get_context().notifications.Remove(notific); 
            }
            connector.get_context().SaveChanges();
            not_list = connector.get_context().notifications.ToList();
            NotificationsTimersUpdate();
        }
        List<notification> notifications = new List<notification>();
        bool firstLoad = false;
        private void NotificationsTimersUpdate()
        {
            foreach (notification noti in notifications)
            {
                if (!not_list.Exists(notif => notif.ID == noti.ID))
                {
                    notifications.Remove(noti);
                    noti.timer = null;
                }
            }
            foreach (notification noti in not_list)
            {
                notification notific;
                bool timerUpdated = false;
                if (firstLoad == false) timerUpdated = true;
                if (notifications.Exists(notif => notif.ID == noti.ID))
                {
                    notific = notifications.First(notif => notif.ID == noti.ID);
                    if (notific.title != noti.title) notific.title = noti.title;
                    if (notific.description != noti.description) notific.description = noti.description;
                    if (notific.play_type != noti.play_type) { notific.play_type = noti.play_type; timerUpdated = true; }
                    if (notific.interval != noti.interval) { notific.interval = noti.interval; timerUpdated = true; }
                } else
                {
                    notific = new notification();
                    notific.ID = noti.ID; notific.title = noti.title; notific.description = noti.description; notific.play_type = noti.play_type; notific.interval = noti.interval;
                    timerUpdated = true;
                }
                if (notific.play_type == 0 && timerUpdated == true) {
                    notific.timer = null;
                    notific.timer = new DispatcherTimer();
                    notific.timer.Interval = notific.duration;
                    notific.timer.Tick += (s,e) => { TimerBigTick(notific); };
                    notific.timer.Start();
                }
                if (notific.play_type == 1 && timerUpdated == true) {
                    notific.timer = null;
                    notific.timer = new DispatcherTimer();
                    notific.timer.Interval = new TimeSpan(0,1,0);
                    notific.timer.Tick += (s, e) => { TimerTick(notific); };
                    notific.timer.Start();
                }
            }
            firstLoad = true;
        }
        public notification_list nl;
        List<notification> nl_list = new List<notification>();
        private void TimerBigTick(notification noti)
        {
            nl_list = nl.notifications;
            nl_list.Add(noti);
            nl.notifications = nl_list;
        }
        private void TimerTick(notification noti)
        {
            if (noti.time.Hour != DateTime.Now.Hour || noti.time.Minute != DateTime.Now.Minute) return;
            nl_list = nl.notifications;
            nl_list.Add(noti);
            nl.notifications = nl_list;
        }
    }
}
