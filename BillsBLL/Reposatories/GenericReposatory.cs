using BillsDAL.Context;
using BillsDAL.Reposatories;
using BillsEntity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.BLL.Specifications;

namespace BillsBLL.Reposatories
{
    public class GenericReposatory<T> : IgenericReposatory<T> where T : class
    {
        private readonly BillsDbContext _context;

        public GenericReposatory(BillsDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
             _context.SaveChanges();
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<T>> GetAllWithSpecAsync(ISpecification<T> Spec)
        {
           return await GetQuery(Spec).ToListAsync();

        }

        public async Task<T> GetByIdWithSpecAsync( ISpecification<T> Spec)
        {
            return await GetQuery(Spec).FirstOrDefaultAsync();
        }

        private IQueryable<T> GetQuery(ISpecification<T> Spec)
        {
             var result =  SpecificationEvaluator<T>.GetQuery(_context.Set<T>(), Spec);
            return result;
        }
    }
}
