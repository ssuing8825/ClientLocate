using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClientLocate.Models;
using System.Data.Entity;
using ClientLocate.Tests.Model;


namespace ClientLocate.Tests
{
    [TestClass]
    public class IntegrationTests
    {
        //[TestMethod]
        //public void QueryDb()
        //{
        //    using (var context = new ClientLocateDbContext())
        //    {
        //        var results = context.ClientDocuments.SqlQuery(SearchQuery()).ToList();
        //        Assert.IsTrue(results.Count == 4);
        //        //   String.Join(",", list.ToArray());
        //    }

        //}

        //private string SearchQuery()
        //{
        //    StringBuilder sb = new StringBuilder(200);
        //    sb.Append("SELECT Id, '' as PayloadXML, PayloadJSON ");
        //    sb.Append("FROM ClientDocuments ");
        //    sb.Append("WHERE CONTAINS(PayloadXML, 'MD' ) ");
        //    return sb.ToString();

        //}

        //private string SearchQuery2(string query)
        //{
        //    StringBuilder sb = new StringBuilder(200);
        //    sb.Append("SELECT Id, '' as PayloadXML, PayloadJSON ");
        //    sb.Append("FROM ClientDocuments ");
        //    sb.AppendFormat("WHERE FREETEXT (PayloadXML, '{0}' ) ", query);
        //    sb.AppendFormat("or CONTAINS (PayloadXML, '\"{0}*\"' ) ", query);

        //    return sb.ToString();

        //}

        //[TestMethod]
        //public void DateFormat()
        //{
        //    string date = "3/21/1974";
        //    var dateParsed = DateTime.Parse(date);
        //    Assert.AreEqual(dateParsed.ToString("yyyy-MM-ddTHH:mm:ss"), "1974-03-21T00:00:00");
        //}
        [TestMethod]
        public void FillStuff()
        {
            //FillTables();

            FillPayload();

        }
        private static void FillTables()
        {
            using (var contextSource = new ClientDBContext())
            {
                using (var contextDestination = new ClientLocateDbContext())
                {
                    var existing = contextDestination.People.Select(c => c.ClientId).ToList();

                    var people = (from ci in contextSource.ClientInfoes
                                  where ci.PrimarySubtypeCode == null
                                  select (new Person
                                  {
                                      FirstName = ci.FirstName,
                                      LastName = ci.LastName,
                                      ClientId = ci.OasisClientId,
                                      DateOfBirth = ci.IndividualDateOfBirth.Value,
                                      MiddleName = ci.MiddleName,
                                      NamePrefix = ci.NamePrefixCode,
                                      NameSuffix = ci.NameSuffixCode
                                  }
                                      )).ToList();
                    

                    StringBuilder sb = new StringBuilder();
                    sb.Append("Select LIne1,line2,cityname,statecode,postalcode,countrycode, rat.Description, ci.OasisClientId ");
                    sb.Append("From ClientAddress ca  ");
                    sb.Append("inner join Reference.AddressType rat on rat.AddressTypeCode = ca.AddressTypeCode ");
                    sb.Append("inner join ClientInfo ci on ca.ClientId = ci.ClientId, ");
                    sb.Append("(Select MAX(AddressSeqNum) AddressSeqNum, ClientId, AddressTypeCode ");
                    sb.Append("from ClientAddress ");
                    sb.Append("Group by ClientId, AddressTypeCode)  maxAdd ");
                    sb.Append("Where ca.ClientId = maxAdd.ClientId and ca.AddressTypeCode = maxAdd.AddressTypeCode and ca.AddressSeqNum = maxAdd.AddressSeqNum and PrimarySubtypecode is null ");
                    var dd = new ClientLocate.Tests.Model.CDbContext();

                    var clientAddresses = dd.ExecuteStoreQuery<ClientAddressModel>(sb.ToString()).ToList();
                    foreach (var person in people)
                    {
                        if (existing.Contains(person.ClientId)) continue;

                        contextDestination.People.Add(person);

                        List<ClientLocate.Models.Phone> phones = contextSource.ClientInfoes.Single(c => c.OasisClientId == person.ClientId).Phones.Select(pho => new ClientLocate.Models.Phone { Person = person, PhoneType = pho.PhoneTypeReference.Value.Description, LocalNumber = pho.PhoneNumber, AreaCode = pho.AreaCode, Extension = pho.Extension }).ToList();
                        foreach (var phone in phones)
                        {
                            contextDestination.Phones.Add(phone);
                        }

                        List<ClientLocate.Models.Address> addresses = clientAddresses.Where(f => f.OasisClientId == person.ClientId).Select(a => new ClientLocate.Models.Address { Person = person, AddressLine1 = a.Line1, AddressLine2 = a.line2, AddressType = a.Description, City = a.cityname, Country = a.countrycode, State = a.statecode, Zip = a.postalcode }).ToList();
                        foreach (var address in addresses)
                        {
                            contextDestination.Addresses.Add(address);
                        }

                        contextDestination.SaveChanges();
                    }

                }
            }
        }

        private static void FillPayload()
        {
            using (var contextDestination = new ClientLocateDbContext())
            {
                var existing = contextDestination.People.Select(c => c.Id).ToList();

                foreach (var item in existing)
                {
                    try
                    {
                        var p = contextDestination.People.Where(g => g.Id == item).Include(d => d.Addresses).Include(c => c.Phones).Select(d => d).First();

                        contextDestination.ClientDocuments.Add(new ClientDocument
                        {
                            Id = p.Id,
                            PayloadXml = p.SerializeToXML(),
                            PayloadJson = p.SerializeToJSON()
                        });

                        contextDestination.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
        }

    }
}
