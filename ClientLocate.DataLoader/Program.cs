using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Threading;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Xml.Serialization;

namespace ClientLocate.DataLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            const int FibonacciCalculations = 5;
            int groupSize = 100000 / FibonacciCalculations;
            int batchSize = 2000;

            var connectionString = "Data Source=.;Integrated Security=True;Pooling=False;Initial Catalog=ClientSearch";

            // One event is used for each Fibonacci object.
            ManualResetEvent[] doneEvents = new ManualResetEvent[FibonacciCalculations];
            DataMover[] fibArray = new DataMover[FibonacciCalculations];
            Random r = new Random();

            // Configure and start threads using ThreadPool.
            Console.WriteLine("launching {0} tasks...", FibonacciCalculations);
            for (int i = 0; i < FibonacciCalculations; i++)
            {
                doneEvents[i] = new ManualResetEvent(false);
                DataMover f = new DataMover(connectionString, i, groupSize, batchSize, doneEvents[i]);
                fibArray[i] = f;
                ThreadPool.QueueUserWorkItem(f.ThreadPoolCallback, i);
            }

            // Wait for all threads in pool to calculate.
            WaitHandle.WaitAll(doneEvents);
            Console.WriteLine("All calculations are complete.");


            //DataRow row = dt.NewRow();
            //row.ItemArray = data;
            //dt.Rows.Add(row);


        }

        public class DataMover
        {
            private ManualResetEvent _doneEvent;
            private string _connectionString;
            private int _seedValue;
            private int _blockSize;
            private int _batchSize;
            // Constructor.
            public DataMover(string connectionString, int seedValue, int blockSize, int batchSize, ManualResetEvent doneEvent)
            {
                _doneEvent = doneEvent;
                _connectionString = connectionString;
                _seedValue = seedValue;
                _blockSize = blockSize;
                _batchSize = batchSize;

            }

            // Wrapper method for use with thread pool.
            public void ThreadPoolCallback(Object threadContext)
            {
                int threadIndex = (int)threadContext;
                Console.WriteLine("thread {0} started...", threadIndex);
                Calculate();
                Console.WriteLine("thread {0} result calculated...", threadIndex);
                _doneEvent.Set();
            }

            // Recursive method that calculates the Nth Fibonacci number.
            public void Calculate()
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Id", typeof(Int32));
                dt.Columns.Add("PayloadXml", typeof(string));
                dt.Columns.Add("PayloadJson", typeof(string));
                dt.Columns.Add("Name", typeof(string));
                dt.Columns.Add("Address", typeof(string));
                dt.Columns.Add("Phone", typeof(string));
                dt.Columns.Add("Policy", typeof(string));


                var start = _seedValue * _blockSize;
                var end = start + _blockSize;

                for (int i = start; i < end; i = i + _batchSize)
                {
                    dt.Rows.Clear();

                    var people = GetPeople(_connectionString, i, _batchSize);

                    foreach (var person in people)
                    {
                        dt.Rows.Add(new object[] { person.Id, ToXml(person), person.SerializeToJSON(), person.ToString(), ToXml(person.Addresses), ToXml(person.Phones), ToXml(person.PolicyKeys) });
                    }

                    BulkCopy(_connectionString, dt);
                    Console.WriteLine(i + _batchSize);
                    System.Threading.Thread.Sleep(1000);
                }
            }
            public string ToXml(object o)
            {
                var xs = new XmlSerializer(o.GetType());
                var stream = new StringWriter();
                xs.Serialize(stream, o);
                stream.Close();
                return stream.ToString();
            }

            private static List<Person> GetPeople(string connectionString, int index, int rowNumber)
            {
                List<Person> r = new List<Person>();
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    cn.Open();

                    string query = String.Format("Select top {2} [Id],[ClientId],[FirstName],[LastName],[MiddleName],[NamePrefix],[NameSuffix],[Gender] ,[DateOfBirth],[TaxId]from People where Id >= {0} and Id < {1}", index, index + rowNumber, rowNumber);

                    SqlCommand cmd = new SqlCommand(query, cn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var t = new Person();
                        t.Id = reader.GetInt32(0);
                        t.ClientId = reader.SafeGetString(1);
                        t.FirstName = reader.SafeGetString(2);
                        t.LastName = reader.SafeGetString(3);
                        t.MiddleName = string.Empty;
                        t.NamePrefix = reader.SafeGetString(5);
                        t.NameSuffix = reader.SafeGetString(6);
                        t.Gender = reader.SafeGetString(7);
                        t.DateOfBirth = reader.GetDateTime(8) as DateTime? ?? default(DateTime);
                        t.TaxId = reader.SafeGetString(9);
                        r.Add(t);
                    }

                    reader.Close();

                    query = String.Format("Select [Id],[AreaCode],[LocalNumber],[Extension],[PhoneType],[PersonId] FROM Phones where PersonId in ({0})", String.Join(",", r.Select(c => c.Id).ToArray()));
                    cmd = new SqlCommand(query, cn);
                    reader = cmd.ExecuteReader();


                    while (reader.Read())
                    {
                        var phone = new Phone();
                        phone.AreaCode = reader.SafeGetString(1);
                        phone.LocalNumber = reader.SafeGetString(2);
                        phone.Extension = reader.SafeGetString(3);
                        phone.PhoneType = reader.SafeGetString(4);

                        var person = r.Where(q => q.Id == reader.GetInt32(5)).First();
                        person.Phones.Add(phone);
                    }

                    reader.Close();

                    query = String.Format("Select [AddressLine1],[AddressLine2],[UnitType],[UnitNumber],[City],[State],[Zip],[Country],[AddressType],[PersonId] FROM Addresses where PersonId in ({0})", String.Join(",", r.Select(c => c.Id).ToArray()));
                    cmd = new SqlCommand(query, cn);
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var a = new Address();
                        a.AddressLine1 = reader.SafeGetString(0);
                        a.AddressLine2 = reader.SafeGetString(1);
                        a.UnitType = reader.SafeGetString(2);
                        a.UnitNumber = reader.SafeGetString(3);
                        a.City = reader.SafeGetString(4);
                        a.State = reader.SafeGetString(5);
                        a.Zip = reader.SafeGetString(6);
                        a.Country = reader.SafeGetString(7);
                        a.AddressType = reader.SafeGetString(8);

                        var person = r.Where(q => q.Id == reader.GetInt32(9)).First();
                        person.Addresses.Add(a);
                    }

                    reader.Close();
                    query = String.Format("Select [policytype],[policyIdentifier],[PersonId] FROM PolicyKeys where PersonId in ({0})", String.Join(",", r.Select(c => c.Id).ToArray()));
                    cmd = new SqlCommand(query, cn);
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var a = new PolicyKey();
                        a.PolicyType = reader.SafeGetString(0);
                        a.PolicyIdentifier = reader.SafeGetString(1);
                        var person = r.Where(q => q.Id == reader.GetInt32(2)).First();
                        person.PolicyKeys.Add(a);
                    }
                }

                return r;
            }
            private static void BulkCopy(string connectionString, DataTable dt)
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    cn.Open();
                    using (SqlBulkCopy copy = new SqlBulkCopy(cn))
                    {
                        copy.ColumnMappings.Add(0, 0);
                        copy.ColumnMappings.Add(1, 1);
                        copy.ColumnMappings.Add(2, 2);
                        copy.ColumnMappings.Add(3, 3);
                        copy.ColumnMappings.Add(4, 4);
                        copy.ColumnMappings.Add(5, 5);
                        copy.ColumnMappings.Add(6, 6);
                        copy.DestinationTableName = "ClientDocuments";
                        copy.WriteToServer(dt);
                    }
                }
            }
        }
    }
    public static class Extensions
    {
        public static string SafeGetString(this SqlDataReader reader, int colIndex)
        {
            if (!reader.IsDBNull(colIndex))
                return reader.GetString(colIndex);
            else
                return string.Empty;
        }
    }

}

