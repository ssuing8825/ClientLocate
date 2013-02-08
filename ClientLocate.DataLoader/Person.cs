using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Text;
using System.Collections.ObjectModel;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace ClientLocate.DataLoader
{
    [DataContract]
    public class Person
    {
        public Person()
        {
            Addresses = new Collection<Address>();
            Phones = new Collection<Phone>();
            PolicyKeys = new Collection<PolicyKey>();
        }
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string ClientId { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string MiddleName { get; set; }
        [DataMember]
        public string NamePrefix { get; set; }
        [DataMember]
        public string NameSuffix { get; set; }
        [DataMember]
        public string Gender { get; set; }
        [DataMember]
        public DateTime DateOfBirth { get; set; }
        [DataMember]
        public string TaxId { get; set; }
        [DataMember]
        public Collection<Address> Addresses { get; set; }
        [DataMember]
        public Collection<Phone> Phones { get; set; }
        [DataMember]
        public Collection<PolicyKey> PolicyKeys { get; set; }

        public string SerializeToXML()
        {
            var xs = new XmlSerializer(this.GetType());
            var stream = new StringWriter();
            xs.Serialize(stream, this);
            stream.Close();
            return stream.ToString();
        }

        public string SerializeToJSON()
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(this.GetType());
            MemoryStream ms = new MemoryStream();
            serializer.WriteObject(ms, this);
            string json = Encoding.Default.GetString(ms.ToArray());
            return json;
        }
        public override string ToString()
        {
            return LastName + ", " + FirstName + " " + MiddleName;
        }
   
    }
}