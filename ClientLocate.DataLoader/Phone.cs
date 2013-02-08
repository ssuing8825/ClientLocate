using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace ClientLocate.DataLoader
{
    [DataContract]
    public class Phone
    {
        [DataMember]
        public string AreaCode { get; set; }
        [DataMember]
        public string LocalNumber { get; set; }
        [DataMember]
        public string Extension { get; set; }
        [DataMember]
        public string PhoneType { get; set; }
     
    }
}