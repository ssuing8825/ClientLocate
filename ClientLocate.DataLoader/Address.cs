using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace ClientLocate.DataLoader
{
    [DataContract]
    public class Address
    {
        [DataMember]
        public string AddressLine1 { get; set; }
        [DataMember]
        public string AddressLine2 { get; set; }
        [DataMember]
        public string PoBox { get; set; }
        [DataMember]
        public string UnitType { get; set; }
        [DataMember]
        public string UnitNumber { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string Zip { get; set; }
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public string AddressType { get; set; }

    }
}