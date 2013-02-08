using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace ClientLocate.Models
{
    [DataContract]
    public class Phone
    {
        [DataMember]
        public int Id { get; set; }

        [MaxLength(3)]
        [DataMember]
        public string AreaCode { get; set; }

        [MaxLength(7)]
        [DataMember]
        public string LocalNumber { get; set; }

        [MaxLength(10)]
        [DataMember]
        public string Extension { get; set; }

        [DataMember]
        public string PhoneType { get; set; }

        [DataMember]
        public int PersonId { get; set; }
      
        [XmlIgnore]
        public Person Person { get; set; }
    }
}