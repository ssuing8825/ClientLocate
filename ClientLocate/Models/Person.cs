using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Text;
using System.Collections.ObjectModel;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace ClientLocate.Models
{
    [DataContract]
    public class Person
    {
        [Key]
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
    }
}