using BillsBLL.IReposatories;
using BillsDAL.Context;
using BillsEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillsBLL.Reposatories
{
    public class CustomerRepository : GenericReposatory<Customer>, ICustomerRepository
    {
        private readonly BillsDbContext _context;

        public CustomerRepository(BillsDbContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<Customer> GetBySearch(string search)
        {
            return _context.Customers.Where(i => i.CustomerName.Trim().ToLower().Contains(search.Trim().ToLower()));
        }
    }
}
