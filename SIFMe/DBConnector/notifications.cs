using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.Globalization;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Threading;

namespace SIFMe.DBConnector
{
    public class notification
    {
        [Key]
        public int ID { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        int pt = -1;
        public int play_type { get => pt; set {
                if (pt != value) { pt = value;
                    interval = interval;
                    if (pt == 0) pt_show = "Каждые:";
                    if (pt == 1) pt_show = "Будет в:";
                }
            } 
        }
        [NotMapped]
        public string pt_show {  get; set; }
        [NotMapped]
        public DateTime time = DateTime.MinValue;
        [NotMapped]
        public TimeSpan duration = TimeSpan.Zero;
        public string interval { get {
                string result = "";
                if (play_type == 0)
                {
                    if (duration.Hours > 0) { if (duration.Hours < 10) result += "0"; result += duration.Hours.ToString(); } else result += "00";
                    result += ":";
                    if (duration.Minutes > 0) { if (duration.Minutes < 10) result += "0"; result += duration.Minutes.ToString(); } else result += "00";
                }
                if (play_type == 1)
                {
                    if (time.Hour > 0) { if (time.Hour < 10) result += "0"; result += time.Hour.ToString(); } else result += "00";
                    result += ":";
                    if (time.Minute > 0) { if (time.Minute < 10) result += "0"; result += time.Minute.ToString(); } else result += "00";
                }
                return result;
            } set {
                if (value == null) value = "00:00";
                if (Regex.IsMatch(value, @"^\d{2}:\d{2}$"))
                {
                    var t = TimeSpan.ParseExact(value, "hh\\:mm", CultureInfo.InvariantCulture);
                    int hours = t.Hours;
                    int minutes = t.Minutes;
                    if (play_type == 0)
                    {
                        TimeSpan timeSpan = new TimeSpan(hours, minutes, 0);
                        if (duration != timeSpan) duration = timeSpan;
                    }
                    if (play_type == 1)
                    {
                        DateTime dateTime = DateTime.MinValue;
                        dateTime += t;
                        if (time != dateTime) time = dateTime;
                    }
                }
            } 
        }
        public notification() {
            ID = 0;
            play_type = 0;
        }
        public notification(int id, string _title, string _description, int _play_type, string _interval)
        {
            ID = id;
            this.title = _title;
            this.description = _description;
            this.play_type = _play_type;
            this.interval = _interval;
        }
        [NotMapped]
        public DispatcherTimer timer { get; set; }
    }
}
