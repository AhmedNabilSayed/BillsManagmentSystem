using BillsDAL.Reposatories;
using BillsEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillsBLL.IReposatories
{
    public interface ICustomerRepository : IgenericReposatory<Customer>
    {
        IQueryable<Customer> GetBySearch(string search);
    }
}
