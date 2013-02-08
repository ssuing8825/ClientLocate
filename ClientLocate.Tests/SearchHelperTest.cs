using ClientLocate.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using System.Collections.Generic;
using ClientLocate.Models;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;

namespace ClientLocate.Tests
{
    
    
    /// <summary>
    ///This is a test class for SearchHelperTest and is intended
    ///to contain all SearchHelperTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SearchHelperTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        ///// <summary>
        /////A test for LocateClient
        /////</summary>
        //// TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        //// http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        //// whether you are testing a page, web service, or a WCF service.
        //[TestMethod()]
        //[HostType("ASP.NET")]
        //[UrlToTest("http://localhost:51324")]
        //public void LocateClientTest()
        //{
        //    SearchHelper target = new SearchHelper(); // TODO: Initialize to an appropriate value
        //    string query = string.Empty; // TODO: Initialize to an appropriate value
        //    string searchType = string.Empty; // TODO: Initialize to an appropriate value
        //    List<string> expected = null; // TODO: Initialize to an appropriate value
        //    List<string> actual;
        //    actual = target.LocateClient(query, searchType);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        /// <summary>
        ///A test for LocateClient
        ///</summary>
        // TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        // http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        // whether you are testing a page, web service, or a WCF service.
        [TestMethod()]
        //[HostType("ASP.NET")]
     //   [UrlToTest("http://localhost:51324")]
        public void LocateClientTest1()
        {
            SearchHelper target = new SearchHelper(); // TODO: Initialize to an appropriate value
            string name = "Steve"; // TODO: Initialize to an appropriate value
            string phone = "123"; // TODO: Initialize to an appropriate value
            string address = string.Empty; // TODO: Initialize to an appropriate value
            string policy = string.Empty; // TODO: Initialize to an appropriate value
            List<string> expected = null; // TODO: Initialize to an appropriate value
            List<string> actual;
            actual = target.LocateClient(name, phone, address, policy);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        [TestMethod()]
         public void LocateClientT()
        {
            SearchHelper target = new SearchHelper(); // TODO: Initialize to an appropriate value
        

            string s = SerializeToJSON(new List<string> { "Steve","BIll","asdfas"});

            Console.Write(s);

        }
        public string SerializeToJSON(object o)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(o.GetType());
            MemoryStream ms = new MemoryStream();
            serializer.WriteObject(ms, o);
            string json = Encoding.Default.GetString(ms.ToArray());
            return json;
        }

        
        ///// <summary>
        /////A test for CreatePredicate
        /////</summary>
        //// TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        //// http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        //// whether you are testing a page, web service, or a WCF service.
        //[TestMethod()]
        //[HostType("ASP.NET")]
        //[UrlToTest("http://localhost:51324")]
        //[DeploymentItem("ClientLocate.dll")]
        //public void CreatePredicateTest()
        //{
        //    SearchHelper_Accessor target = new SearchHelper_Accessor(); // TODO: Initialize to an appropriate value
        //    string query = string.Empty; // TODO: Initialize to an appropriate value
        //    string expected = string.Empty; // TODO: Initialize to an appropriate value
        //    string actual;
        //    actual = target.CreatePredicate(query);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for CreatePredicate
        /////</summary>
        //// TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        //// http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        //// whether you are testing a page, web service, or a WCF service.
        //[TestMethod()]
        //[HostType("ASP.NET")]
        //[UrlToTest("http://localhost:51324")]
        //[DeploymentItem("ClientLocate.dll")]
        //public void CreatePredicateTest1()
        //{
        //    SearchHelper_Accessor target = new SearchHelper_Accessor(); // TODO: Initialize to an appropriate value
        //    string column = string.Empty; // TODO: Initialize to an appropriate value
        //    string query = string.Empty; // TODO: Initialize to an appropriate value
        //    string expected = string.Empty; // TODO: Initialize to an appropriate value
        //    string actual;
        //    actual = target.CreatePredicate(column, query);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for CountClient
        /////</summary>
        //// TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        //// http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        //// whether you are testing a page, web service, or a WCF service.
        //[TestMethod()]
        //[HostType("ASP.NET")]
        //[UrlToTest("http://localhost:51324")]
        //public void CountClientTest()
        //{
        //    SearchHelper target = new SearchHelper(); // TODO: Initialize to an appropriate value
        //    string query = string.Empty; // TODO: Initialize to an appropriate value
        //    string searchType = string.Empty; // TODO: Initialize to an appropriate value
        //    int expected = 0; // TODO: Initialize to an appropriate value
        //    int actual;
        //    actual = target.CountClient(query, searchType);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for Columns
        /////</summary>
        //// TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        //// http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        //// whether you are testing a page, web service, or a WCF service.
        //[TestMethod()]
        //[HostType("ASP.NET")]
        //[UrlToTest("http://localhost:51324")]
        //[DeploymentItem("ClientLocate.dll")]
        //public void ColumnsTest()
        //{
        //    SearchHelper_Accessor target = new SearchHelper_Accessor(); // TODO: Initialize to an appropriate value
        //    string searchType = string.Empty; // TODO: Initialize to an appropriate value
        //    string expected = string.Empty; // TODO: Initialize to an appropriate value
        //    string actual;
        //    actual = target.Columns(searchType);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for CheckParseErrors
        /////</summary>
        //// TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        //// http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        //// whether you are testing a page, web service, or a WCF service.
        //[TestMethod()]
        //[HostType("ASP.NET")]
        //[UrlToTest("http://localhost:51324")]
        //[DeploymentItem("ClientLocate.dll")]
        //public void CheckParseErrorsTest()
        //{
        //    SearchHelper_Accessor target = new SearchHelper_Accessor(); // TODO: Initialize to an appropriate value
        //    bool expected = false; // TODO: Initialize to an appropriate value
        //    bool actual;
        //    actual = target.CheckParseErrors();
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for BuildPredicate
        /////</summary>
        //// TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        //// http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        //// whether you are testing a page, web service, or a WCF service.
        //[TestMethod()]
        //[HostType("ASP.NET")]
        //[UrlToTest("http://localhost:51324")]
        //[DeploymentItem("ClientLocate.dll")]
        //public void BuildPredicateTest()
        //{
        //    SearchHelper_Accessor target = new SearchHelper_Accessor(); // TODO: Initialize to an appropriate value
        //    string name = string.Empty; // TODO: Initialize to an appropriate value
        //    string phone = string.Empty; // TODO: Initialize to an appropriate value
        //    string address = string.Empty; // TODO: Initialize to an appropriate value
        //    string policy = string.Empty; // TODO: Initialize to an appropriate value
        //    string expected = string.Empty; // TODO: Initialize to an appropriate value
        //    string actual;
        //    actual = target.BuildPredicate(name, phone, address, policy);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for AddDateToQuery
        /////</summary>
        //// TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        //// http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        //// whether you are testing a page, web service, or a WCF service.
        //[TestMethod()]
        //[HostType("ASP.NET")]
        //[UrlToTest("http://localhost:51324")]
        //[DeploymentItem("ClientLocate.dll")]
        //public void AddDateToQueryTest()
        //{
        //    SearchHelper_Accessor target = new SearchHelper_Accessor(); // TODO: Initialize to an appropriate value
        //    string query = string.Empty; // TODO: Initialize to an appropriate value
        //    string expected = string.Empty; // TODO: Initialize to an appropriate value
        //    string actual;
        //    actual = target.AddDateToQuery(query);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for SearchHelper Constructor
        /////</summary>
        //// TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        //// http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        //// whether you are testing a page, web service, or a WCF service.
        //[TestMethod()]
        //[HostType("ASP.NET")]
        //[UrlToTest("http://localhost:51324")]
        //public void SearchHelperConstructorTest()
        //{
        //    SearchHelper target = new SearchHelper();
        //    Assert.Inconclusive("TODO: Implement code to verify target");
        //}

        ///// <summary>
        /////A test for SearchHelper Constructor
        /////</summary>
        //// TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        //// http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        //// whether you are testing a page, web service, or a WCF service.
        //[TestMethod()]
        //[HostType("ASP.NET")]
        //[UrlToTest("http://localhost:51324")]
        //public void SearchHelperConstructorTest1()
        //{
        //    IClientDocumentRepository clientDocumentRepository = null; // TODO: Initialize to an appropriate value
        //    SearchHelper target = new SearchHelper(clientDocumentRepository);
        //    Assert.Inconclusive("TODO: Implement code to verify target");
        //}
    }
}
