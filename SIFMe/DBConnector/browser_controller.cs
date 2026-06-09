using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SIFMe.DBConnector
{
    public class browser_controller
    {
        const string techdata_name = "tech_data";
        char[] id_parts = new char[] { '-', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'};
        int last_id = 1;
        int last_link_id = 1;
        public List<datatag> tag_list;
        public List<file_link> link_list;
        public browser_controller() 
        {
            tag_list = new List<datatag>();
            link_list = new List<file_link>();
            string current_directory = AppDomain.CurrentDomain.BaseDirectory;
            string techdata_folder = current_directory + techdata_name;
            string techdata_link = techdata_folder + "\\" + techdata_name + ".txt";
            if (!Directory.Exists(techdata_folder)) Directory.CreateDirectory(techdata_folder);
            if (!File.Exists(techdata_link)) File.WriteAllText(techdata_link, last_id.ToString());
            last_id = int.Parse(File.ReadAllText(techdata_link));
        }
        public string new_tag(string tag)
        {
            if (tag_list.Exists(t => t.tag == tag)) return "Такой тег уже существует!";
            datatag dt = new datatag();
            dt.ID = intID_toStringID(last_id);
            dt.tag = tag;
            tag_list.Add(dt);
            last_id++;
            return "";
        }
        public void save_link(file_link link)
        {
            if (link.ID == 0)
            {
                link.ID = last_link_id;
                last_link_id++;
                link_list.Add(link);
            }
        }
        public void del_link(file_link link)
        {
            if (link_list.Contains(link)) link_list.Remove(link);
        }
        string intID_toStringID(int id)
        {
            string result = "";
            int sub_id = 0;
            int iterator = 0;
            while (id > 0)
            {
                if (id > 26) { if (id % 26 == 0) { last_id += (int)Math.Pow(26, iterator); id += 1; } sub_id = id % 26; id = (id-sub_id) / 26; iterator++; }
                else { sub_id = id; id = 0; }
                result += id_parts[sub_id];
            }
            return result;
        }
    }
}
