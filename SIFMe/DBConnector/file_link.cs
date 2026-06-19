using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Windows.Interop;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIFMe.DBConnector
{
    public class filelink
    {
        [Key]
        public int ID { get; set; }
        string link_ = "";
        public string link { get => link_; set {
                if (link_ != value)
                {
                    link_ = value;
                    try
                    {
                        name = Path.GetFileName(link_);
                        Icon icon = Icon.ExtractAssociatedIcon(link_);
                        if (icon != null)
                        {
                            img = Imaging.CreateBitmapSourceFromHIcon(
                                icon.Handle,
                                Int32Rect.Empty,
                                BitmapSizeOptions.FromEmptyOptions());
                        }
                    } catch { name = "Файл не найден"; img = null; }
                }
            } 
        }
        public string name { get; set; }
        public string tags { get; set; }
        [NotMapped]
        public ImageSource img { get; set; }
        public filelink() { ID = 0; }
        public filelink(int _id, string _link, string _tags) 
        {
            this.ID = _id; this.link = _link; this.tags = _tags;
        }
    }
}
