using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Objects;

namespace ClientLocate.Tests.Model
{
    public class ClientAddressModel
    {
        public string Line1 { get; set; }
        public string line2 { get; set; }
        public string cityname { get; set; }
        public string statecode { get; set; }
        public string postalcode { get; set; }
        public string countrycode { get; set; }
        public string Description { get; set; }
        public string OasisClientId { get; set; }

    }

    public class CDbContext : ObjectContext
    {
        public CDbContext()
            : base("name=ClientDBContext", "ClientDBContext")
        {
        }
        public ObjectSet<ClientAddressModel> ClientAddresses
        {
            get
            {
                if ((_ClientAddresses == null))
                {
                    _ClientAddresses = base.CreateObjectSet<ClientAddressModel>("ClientAddresses");
                }
                return _ClientAddresses;
            }
        }
        private ObjectSet<ClientAddressModel> _ClientAddresses;

    }
}
