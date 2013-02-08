using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ClientLocate.Models
{
    public class ClientDocumentRepository : IClientDocumentRepository
    {
        ClientLocateDbContext context = new ClientLocateDbContext();

        public ClientDocument Find(int id)
        {
            return context.ClientDocuments.Find(id);
        }
    }

    public interface IClientDocumentRepository
    {
        ClientDocument Find(int id);
    }
}