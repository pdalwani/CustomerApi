using CustomerApi.Controllers;
using CustomerApi.Models;
using CustomerApi.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CustomerApiTests
{
    public class CustomerControllerTests
    {
        private readonly Mock<ICustomerRepository> _mockRepo;
        private readonly CustomerController _controller;
        public CustomerControllerTests()
        {
            _mockRepo = new Mock<ICustomerRepository>();
            _controller = new CustomerController(_mockRepo.Object);
        }
        [Fact]
        public async void GetAll_Return()
        {
            var result = await _controller.GetAll();
            Assert.IsType<OkObjectResult>(result as OkObjectResult);

        }

        [Theory]
        [InlineData(12)]
        [InlineData(0)]
        public async void Add_Execute_ReturnType(int customerId)
        {
            var customer = new Customer();
            customer.CustomerId = customerId;

            _mockRepo.Setup(repo => repo.AddCustomer(customer))
        .Returns(Task.FromResult(customerId));

            var result = await _controller.Add(customer);
            if (customerId > 0)
            {
                Assert.IsType<OkObjectResult>(result as OkObjectResult);
            }
            else
            {
                Assert.IsType<BadRequestResult>(result as BadRequestResult);
            }

        }

        [Theory]
        [InlineData(12)]
        [InlineData(0)]
        public async void update_Execute_ReturnType(int customerId)
        {
            var customer = new Customer();
            customer.CustomerId = customerId;

            _mockRepo.Setup(repo => repo.UpdateCustomer(customer))
        .Returns(Task.FromResult(customerId));

            var result = await _controller.Update(customer);
            if (customerId > 0)
            {
                Assert.IsType<OkObjectResult>(result as OkObjectResult);
            }
            else
            {
                Assert.IsType<NotFoundResult>(result as NotFoundResult);
            }


        }

        [Theory]
        [InlineData(12)]
        [InlineData(0)]
        public async void Delete_Execute_ReturnType(int customerId)
        {
            _mockRepo.Setup(repo => repo.DeleteCustomer(customerId))
        .Returns(Task.FromResult(customerId));

            var result = await _controller.Delete(customerId);

            if (customerId > 0)
            {
                Assert.IsType<OkObjectResult>(result as OkObjectResult);
            }
            else
            {
                Assert.IsType<NotFoundResult>(result as NotFoundResult);
            }

        }
    }
}
