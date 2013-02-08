using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ClientLocate.Models
{
    public class PersonRepository : IPersonRepository
    {
        ClientLocateDbContext context = new ClientLocateDbContext();

        public IQueryable<Person> All
        {
            get { return context.People; }
        }


        public IQueryable<Person> AllIncluding(params Expression<Func<Person, object>>[] includeProperties)
        {
            IQueryable<Person> query = context.People;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public List<Person> AllPeople()
        {
            return context.People.Include(c => c.Addresses).Include(d => d.Phones).ToList();
        }
        public Person Find(int id)
        {
            return context.People.Include(c => c.Addresses).Include(d => d.Phones).Where(s => s.Id == id).First();
        }

        public void InsertOrUpdate(Person person)
        {
            if (person.Id == default(int))
            {
                // New entity
                context.People.Add(person);
            }
            else
            {
                // Existing entity
                context.Entry(person).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var person = context.People.Find(id);
            context.People.Remove(person);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }

    public interface IPersonRepository
    {
        IQueryable<Person> All { get; }
        List<Person> AllPeople();

        IQueryable<Person> AllIncluding(params Expression<Func<Person, object>>[] includeProperties);
        Person Find(int id);
        void InsertOrUpdate(Person person);
        void Delete(int id);
        void Save();
    }
}