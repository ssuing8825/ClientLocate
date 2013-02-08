using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ClientLocate.Models
{
    public class ClientLocateDbContext : DbContext, IDisposable   
    {

        public ClientLocateDbContext()
            : base("ClientSearch")
        {
            System.Data.Entity.Database.SetInitializer(new InitDb());
        }
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, add the following
        // code to the Application_Start method in your Global.asax file.
        // Note: this will destroy and re-create your database with every model change.
        // 
        public DbSet<ClientLocate.Models.Address> Addresses { get; set; }

        public DbSet<ClientLocate.Models.Person> People { get; set; }

        public DbSet<ClientLocate.Models.Phone> Phones { get; set; }

        public DbSet<ClientLocate.Models.ClientDocument> ClientDocuments { get; set; }

    }
    public class InitDb : System.Data.Entity.DropCreateDatabaseIfModelChanges<ClientLocateDbContext>
    {
        protected override void Seed(ClientLocateDbContext context)
        {
            var person1 = new Person { FirstName = "Robert", LastName = "Smith", Gender = "Male", TaxId = "1234567897", ClientId = "AKS9AN23J4J34J3N8", DateOfBirth = new DateTime(1974, 3, 21) };
            var person2 = new Person { FirstName = "Steve", LastName = "Parker", Gender = "Male", TaxId = "2847858953", ClientId = "ASKDAL32LKJ23LK2J", DateOfBirth = new DateTime(1980, 3, 12) };
            var person3 = new Person { FirstName = "Barbera", LastName = "Bush", Gender = "Female", TaxId = "3032547895", ClientId = "23LKF2FF498FNFNFN", DateOfBirth = new DateTime(1985, 2, 13) };
            var person4 = new Person { FirstName = "Jill", LastName = "O'Riely", Gender = "Female", TaxId = "8789586589", ClientId = "3489F43JKLN34I8FF", DateOfBirth = new DateTime(1978, 5, 11) };
            var person5 = new Person { FirstName = "Jack", LastName = "Smith", Gender = "Male", TaxId = "2154875896", ClientId = "34UIO34N34N3434N4", DateOfBirth = new DateTime(1964, 6, 6) };
            var person6 = new Person { FirstName = "Tony", LastName = "DeMarco", Gender = "Male", TaxId = "1234569857", ClientId = "34IO34NF34IF34NF3", DateOfBirth = new DateTime(1988, 7, 9) };
            var person7 = new Person { FirstName = "Brian", LastName = "Trivisanno", Gender = "Male", TaxId = "1254878528", ClientId = "34KL34LKFR8TTRPOR", DateOfBirth = new DateTime(1966, 8, 24) };

            context.People.Add(person1);
            context.People.Add(person2);
            context.People.Add(person3);
            context.People.Add(person4);
            context.People.Add(person5);
            context.People.Add(person6);
            context.People.Add(person7);

            var address1Garage = new Address { AddressLine1 = "134 Any Street", City = "Cleveland", State = "OH", AddressType = "Garage", Person = person1 };
            var address1Home = new Address { AddressLine1 = "134 Any Street", City = "Cleveland", State = "OH", AddressType = "Home", Person = person1 };
            var address2Home = new Address { AddressLine1 = "13009 Harvey Rd", City = "Columbia", State = "MD", AddressType = "Home", Person = person2 };
            var address3Home = new Address { AddressLine1 = "1584 Crest Rd", City = "Silver Spring", State = "VA", AddressType = "Home", Person = person3 };
            var address4Home = new Address { AddressLine1 = "505 16th St N.W.", City = "Takoma Park", State = "MD", AddressType = "Home", Person = person4 };
            var address5Garage = new Address { AddressLine1 = "3020 Park Drive", City = "Baltimore", State = "MD", AddressType = "Home", Person = person5 };
            var address5Home = new Address { AddressLine1 = "8083 Commerce Dr", UnitType = "Suite", UnitNumber = "600", City = "Reston", State = "VA", AddressType = "Garage", Person = person6 };
            var address6Home = new Address { IsPoBox = true, PoBox = "2508", City = "Baltimore", State = "MD", AddressType = "Home", Person = person6 };

            context.Addresses.Add(address1Garage);
            context.Addresses.Add(address1Home);
            context.Addresses.Add(address2Home);
            context.Addresses.Add(address3Home);
            context.Addresses.Add(address4Home);
            context.Addresses.Add(address5Garage);
            context.Addresses.Add(address5Home);
            context.Addresses.Add(address6Home);

            var phone1 = new Phone { AreaCode = "919", LocalNumber = "1234567", PhoneType = "Home", Person = person1 };
            var phone2 = new Phone { AreaCode = "919", LocalNumber = "7845478", PhoneType = "Work", Person = person1 };
            var phone3 = new Phone { AreaCode = "919", LocalNumber = "4485723", PhoneType = "Cell", Person = person1 };

            var phone4 = new Phone { AreaCode = "240", LocalNumber = "7678677", PhoneType = "Home", Person = person2 };
            var phone5 = new Phone { AreaCode = "440", LocalNumber = "8887868", PhoneType = "Work", Person = person2 };
            var phone6 = new Phone { AreaCode = "216", LocalNumber = "7852527", PhoneType = "Home", Person = person3 };
            var phone7 = new Phone { AreaCode = "540", LocalNumber = "7828777", PhoneType = "Home", Person = person4 };
            var phone8 = new Phone { AreaCode = "603", LocalNumber = "7827878", PhoneType = "Home", Person = person5 };

            context.Phones.Add(phone1);
            context.Phones.Add(phone2);
            context.Phones.Add(phone3);
            context.Phones.Add(phone4);
            context.Phones.Add(phone5);
            context.Phones.Add(phone6);
            context.Phones.Add(phone7);

            context.Phones.Add(phone8);
            context.Phones.Add(phone1);

            context.SaveChanges();

            var people = context.People.Include(c => c.Addresses).Include(c => c.Phones);
            foreach (var item in people)
            {
                context.ClientDocuments.Add(new ClientDocument
                {
                    Id = item.Id,
                    PayloadXml = item.SerializeToXML(),
                    PayloadJson = item.SerializeToJSON()
                });
            }
            context.SaveChanges();

            base.Seed(context);
        }
    }
}