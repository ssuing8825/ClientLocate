using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace ClientLocate.DataLoader
{
    [DataContract]
    public class PolicyKey
    {
        [DataMember]
        public string PolicyType { get; set; }
        [DataMember]
        public string PolicyIdentifier { get; set; }
    }
}