using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIFMe.DBConnector
{
    public class datatag
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ID { get; set; }
        public string tag { get; set; }
        public string conjunction_tags { get; set; }
        public datatag() { ID = "---"; }
        public datatag(string _id, string _tag, string _conjunction_tags)
        {
            this.ID = _id;
            this.tag = _tag;
            this.conjunction_tags = _conjunction_tags;
        }
    }
}
