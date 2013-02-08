using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ClientLocate.Models
{ 
    public class PhoneRepository : IPhoneRepository
    {
        ClientLocateDbContext context = new ClientLocateDbContext();

        public IQueryable<Phone> All
        {
            get { return context.Phones; }
        }

        public IQueryable<Phone> AllIncluding(params Expression<Func<Phone, object>>[] includeProperties)
        {
            IQueryable<Phone> query = context.Phones;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public Phone Find(int id)
        {
            return context.Phones.Find(id);
        }

        public void InsertOrUpdate(Phone phone)
        {
            if (phone.Id == default(int)) {
                // New entity
                context.Phones.Add(phone);
            } else {
                // Existing entity
                context.Entry(phone).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var phone = context.Phones.Find(id);
            context.Phones.Remove(phone);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }

    public interface IPhoneRepository
    {
        IQueryable<Phone> All { get; }
        IQueryable<Phone> AllIncluding(params Expression<Func<Phone, object>>[] includeProperties);
        Phone Find(int id);
        void InsertOrUpdate(Phone phone);
        void Delete(int id);
        void Save();
    }
}