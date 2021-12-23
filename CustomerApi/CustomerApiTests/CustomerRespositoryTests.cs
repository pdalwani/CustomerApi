using CustomerApi.Controllers;
using CustomerApi.Data;
using CustomerApi.Models;
using CustomerApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CustomerApiTests
{
    public class CustomerRepositoryTests
    {
        private readonly Mock<ICustomerRepository> _mockRepo;
        private readonly CustomerController _controller;
        private EntityContext _context;
        private CustomerRepository _repo;

        public CustomerRepositoryTests()
        {
            this.GetContext();
            _repo = new CustomerRepository(_context);
        }

        private async void GetContext()
        {
            var options = new DbContextOptionsBuilder<EntityContext>()
           .UseInMemoryDatabase(databaseName: "CustomerDataBase")
          .Options;

            _context = new EntityContext(options);
            var all = await _context.Customers.ToListAsync();
            _context.Customers.RemoveRange(all);
            _context.SaveChanges();

            var testCustomer1 = new Customer
            {
                CustomerId = 123,
                FirstName = "Luke",
                LastName = "Skywalker",
                DateOfBirth = new DateTime(2015, 12, 31)
            };

            _context.Customers.Add(testCustomer1);

            var testCustomer2 = new Customer
            {
                CustomerId = 456,
                FirstName = "Mark",
                LastName = "Robinson",
                DateOfBirth = new DateTime(2011, 11, 29)
            };

            _context.Customers.Add(testCustomer2);
            _context.SaveChanges();

        }

        [Theory]
        [InlineData("", 2, true)]
        [InlineData("Luke", 1, true)]
        [InlineData("Robinson", 1, false)]
        [InlineData("Jerry", 0, true)]
        public async void GetAll_Return(string name, int resultCount, bool isFirstName)
        {
            var result = await _repo.GetCustomers(name);
            Assert.Equal(result.Count, resultCount);
            if (resultCount == 1)
            {
                if (isFirstName)
                    Assert.Equal(result[0].FirstName, name);
                else
                    Assert.Equal(result[0].LastName, name);
            }
        }

        [Fact]
        public async void Add_Execute_ReturnType()
        {
            var customer = new Customer();
            customer.CustomerId = 1;
            customer.FirstName = "Mary";

            var result = await _repo.AddCustomer(customer);
            Assert.Equal(1, result);
            var records = await _context.Customers.ToListAsync();
            var recordsCount = records.Count;
            Assert.Equal(3, recordsCount);

        }

        [Theory]
        [InlineData(123, 1)]
        [InlineData(1, 0)]
        public async void Update_Execute_ReturnType(int customerId, int returnId)
        {
            var options = new DbContextOptionsBuilder<EntityContext>()
           .UseInMemoryDatabase(databaseName: "CustomerDataBase")
           .Options;

            var _disconnectedCont = new EntityContext(options);
            _repo = new CustomerRepository(_disconnectedCont);

            var customer = new Customer();
            customer.CustomerId = customerId;
            customer.FirstName = "Marco";

            var result = await _repo.UpdateCustomer(customer);
            Assert.Equal(result, returnId);
        }

        [Theory]
        [InlineData(123, 1)]
        [InlineData(1, 0)]
        public async void Delete_Execute_ReturnType(int customerId, int returnId)
        {
            var result = await _repo.DeleteCustomer(customerId);
            Assert.Equal(result, returnId);
        }


    }
}
