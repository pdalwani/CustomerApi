using CustomerApi.Data;
using CustomerApi.Models;
using CustomerApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CustomerApi.Controllers
{
    [Route("api/customer")]
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly EntityContext _context;
        private readonly ICustomerRepository _customerRepository;

        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        /// <summary>
        /// Gets the list of all Customers or serach by First Name or Last Name.
        /// </summary>
        /// <returns>The list of Customers.</returns>
        // GET: api/Customer
        [HttpGet]
        [Route("Search")]
        public async Task<IActionResult> GetAll(string name = "")
        {
            var customers = await _customerRepository.GetCustomers(name);

            return Ok(customers);
        }


        /// <summary>
        /// Add new customer.
        /// </summary>
        /// <returns>Customer ID</returns>
        // POST: api/Customer
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add(Customer customer)
        {
            var customerId = await _customerRepository.AddCustomer(customer);
            if (customerId > 0)
                return Ok(customerId);
            return BadRequest();
        }

        /// <summary>
        /// Delete customer.
        /// </summary>
        /// <returns>Customer ID</returns>
        // DELETE: api/Customer
        [HttpDelete]
        public async Task<IActionResult> Delete(int customerId)
        {
            var response = await _customerRepository.DeleteCustomer(customerId);
            if (response > 0)

                return Ok(response);
            else
                return NotFound();
        }

        /// <summary>
        /// Update Existing customer.
        /// </summary>
        /// <returns>Customer ID</returns>
        // PUT: api/Customer
        [HttpPut]
        public async Task<IActionResult> Update(Customer customer)
        {
            var response = await _customerRepository.UpdateCustomer(customer);
            if (response > 0)

                return Ok(response);
            else
                return NotFound();
        }
    }

}

