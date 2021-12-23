using CustomerApi.Data;
using CustomerApi.Models;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Services
{
    public interface ICustomerRepository
    {
        Task<List<Customer>> GetCustomers(string name);
        Task<int> AddCustomer(Customer customer);

        Task<int> DeleteCustomer(int customerId);

        Task<int> UpdateCustomer(Customer customer);
    }
    public class CustomerRepository : ICustomerRepository
    {
        private readonly EntityContext _entityContext;
        public CustomerRepository(EntityContext entityContext)
        {
            _entityContext = entityContext;
        }

        public async Task<int> AddCustomer(Customer customer)
        {
            await _entityContext.Customers.AddAsync(customer);
            await _entityContext.SaveChangesAsync();

            return customer.CustomerId;
        }

        public async Task<int> DeleteCustomer(int customerId)
        {
            int result = 0;
            var customer = await _entityContext.Customers.FirstOrDefaultAsync(x => x.CustomerId == customerId);

            if (customer != null)
            {
                //Delete
                _entityContext.Customers.Remove(customer);

                result = await _entityContext.SaveChangesAsync();
            }
            return result;
        }

        public async Task<List<Customer>> GetCustomers(string name)
        {
            return await _entityContext.Customers.Where(item => String.IsNullOrEmpty(name) || item.FirstName.Contains(name) || item.LastName.Contains(name))
            .ToListAsync();
        }

        public async Task<int> UpdateCustomer(Customer customer)
        {
            int result = 0;
            var oldCustomer = await _entityContext.Customers.AsNoTracking<Customer>().FirstOrDefaultAsync(x => x.CustomerId == customer.CustomerId);

            if (oldCustomer != null)
            {
                _entityContext.Update(customer);
                result = await _entityContext.SaveChangesAsync();
            }
            return result;
        }
    }
}
