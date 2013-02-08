using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Irony.Compiler;
using ClientLocate.Models;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;

namespace ClientLocate.Resources
{

    //http://www.sqlservercentral.com/articles/Full-Text+Search+(2008)/64248/

    public class SearchHelper
    {
        private IClientDocumentRepository clientDocumentRepository;
        private SearchGrammar _grammar;
        private LanguageCompiler _compiler;
        private string error;

        public SearchHelper()
            : this(new ClientDocumentRepository())
        {

        }
        public SearchHelper(IClientDocumentRepository clientDocumentRepository)
        {
            this.clientDocumentRepository = clientDocumentRepository;

            _grammar = new SearchGrammar();
            _compiler = new LanguageCompiler(_grammar);
            Irony.StringSet errors = _compiler.Parser.GetErrors();
            if (errors.Count > 0)
            {
                throw new ApplicationException("SearchGrammar contains errors. Investigate using GrammarExplorer." + errors.ToString());
            }
        }

        public int CountClient(string query, string searchType)
        {
            //this function should just be a scalor.

            var queryPredicate = CreatePredicate(query);
            StringBuilder sb = new StringBuilder(200);
            sb.Append("SELECT Id, '' as PayloadXML, '' as PayloadJSON ");
            sb.Append("FROM ClientDocuments ");
            sb.AppendFormat("WHERE CONTAINS({0}, '{1}' );", Columns(searchType), queryPredicate);

            List<int> list = null;
            using (var context = new ClientLocateDbContext())
            {
                list = context.ClientDocuments.SqlQuery(sb.ToString()).Select(c => c.Id).Distinct().ToList();
            }
            return list.Count;

        }

        private string CreatePredicate(string query)
        {
            var queryWithDate = AddDateToQuery(query);

            AstNode root = _compiler.Parse(queryWithDate);
            if (!CheckParseErrors())
            {
                throw new ApplicationException(error);
            }

            var queryPredicate = SearchGrammar.ConvertQuery(root, SearchGrammar.TermType.Inflectional);
            return queryPredicate;
        }
        public int CountClient(string name, string phone, string address, string policy)
        {
            StringBuilder sb = new StringBuilder(200);
            sb.Append("SELECT Id, '' as PayloadXML, '' as PayloadJSON ");
            sb.Append("FROM ClientDocuments ");
            sb.AppendFormat("WHERE {0}", BuildPredicate(name, phone, address, policy));

            List<int> list = null;
            using (var context = new ClientLocateDbContext())
            {
                list = context.ClientDocuments.SqlQuery(sb.ToString()).Select(c => c.Id).Distinct().ToList();
            }
            return list.Count;
        }
        public List<string> LocateClient(string name, string phone, string address, string policy)
        {

            StringBuilder sb = new StringBuilder(200);
            sb.Append("SELECT top 50 Id, '' as PayloadXML, PayloadJSON ");
            sb.Append("FROM ClientDocuments ");
            sb.AppendFormat("WHERE {0}", BuildPredicate(name, phone, address, policy));

            List<string> list = null;
            using (var context = new ClientLocateDbContext())
            {
                list = context.ClientDocuments.SqlQuery(sb.ToString()).Select(c => c.PayloadJson).Distinct().ToList();
            }
            return list;

        }
        private string BuildPredicate(string name, string phone, string address, string policy)
        {
            List<string> predicate = new List<string>();
            if (name != string.Empty)
                predicate.Add(CreatePredicate("Name", name));
            if (phone != string.Empty)
                predicate.Add(CreatePredicate("Phone", phone));
            if (address != string.Empty)
                predicate.Add(CreatePredicate("Address", address));
            if (policy != string.Empty)
                predicate.Add(CreatePredicate("Policy", policy));

            return String.Join(" or ", predicate.ToArray());
        }
        private string CreatePredicate(string column, string query)
        {
            AstNode root = _compiler.Parse(query);

            if (!CheckParseErrors())
            {
                throw new ApplicationException(error);
            }
            var queryPredicate = SearchGrammar.ConvertQuery(root, SearchGrammar.TermType.Inflectional);
            return string.Format("CONTAINS([{0}], '{1}' )", column, queryPredicate);

        }
        public List<string> LocateClient(string query, string searchType)
        {
            query = AddDateToQuery(query);

            AstNode root = _compiler.Parse(query);
            if (!CheckParseErrors())
            {
                throw new ApplicationException(error);
            }

            var queryPredicate = SearchGrammar.ConvertQuery(root, SearchGrammar.TermType.Inflectional);
            StringBuilder sb = new StringBuilder(200);
            sb.Append("SELECT top 50 Id, '' as PayloadXML, PayloadJSON ");
            sb.Append("FROM ClientDocuments ");
            sb.AppendFormat("WHERE CONTAINS({0}, '{1}' );", Columns(searchType), queryPredicate);

            List<string> list = null;
            using (var context = new ClientLocateDbContext())
            {
                list = context.ClientDocuments.SqlQuery(sb.ToString()).Select(c => c.PayloadJson).Distinct().ToList();
            }
            return list;

        }

        private string Columns(string searchType)
        {
            if (searchType.Contains("All")) return "PayloadXml";

            List<string> ftscolumns = new List<string>();
            foreach (var column in searchType.Split(','))
            {
                switch (column)
                {
                    case "Name":
                        ftscolumns.Add("Name");
                        break;
                    case "Address":
                        ftscolumns.Add("Address");
                        break;
                    case "Phone":
                        ftscolumns.Add("Phone");
                        break;
                    case "Policy":
                        ftscolumns.Add("Policy");
                        break;
                    default:
                        ftscolumns.Add("PayloadXml");
                        break;
                }
            }
            return String.Format("({0})", String.Join(",", ftscolumns.ToArray()));
        }
        private string AddDateToQuery(string query)
        {
            DateTime testDate;
            var peices = query.Split(' ');
            string newQuery = string.Empty;

            foreach (var item in peices.ToList())
            {
                if (DateTime.TryParse(item, out testDate))
                {
                    newQuery += "\"" + testDate.ToString("yyyy-MM-ddTHH:mm:ss") + "\"";
                }
                else
                    newQuery += item + " ";
            }
            return newQuery;
        }
        private bool CheckParseErrors()
        {
            if (_compiler.Context.Errors.Count == 0) return true;
            error = "Errors: \r\n";
            foreach (SyntaxError err in _compiler.Context.Errors)
                error += err.ToString() + "\r\n";
            return false;
        }

        internal List<string> GetWordList(string name)
        {
            return Thesaurus(name); ;
        }
        public List<string> GetWordList(string query, string searchType)
        {
            DateTime testDate;
            var querySplit = query.Split(' ').ToList();
            querySplit.Remove("or");
            querySplit.Remove("and");
            var result = new List<string>();

            foreach (var item in querySplit)
            {
                if (item.Contains('~'))
                {
                    result.AddRange(Thesaurus(item.Replace("~", "").Replace("*", "")));
                }
                else if (item.Contains('*'))
                {
                    result.Add(item.Replace("*", ""));
                }
                else if (DateTime.TryParse(item, out testDate))
                {
                    result.Add(testDate.ToString("MM/dd/yyyy"));
                }
                else
                    result.Add(item);
            }
            return result;
        }

        private List<string> Thesaurus(string word)
        {
            if (word.Contains('~'))
            {
                word = word.Replace("~", "");
            }

            var t = new List<string>();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ClientSearch"].ConnectionString))
            {
                var command = new SqlCommand("SELECT display_term FROM sys.dm_fts_parser('FORMSOF (THESAURUS, " + word + ")', 1033, 0, 0)", connection);
                connection.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    t.Add(reader.GetString(0));
                }

                reader.Close();
            }
            return t;
        }
    }
}