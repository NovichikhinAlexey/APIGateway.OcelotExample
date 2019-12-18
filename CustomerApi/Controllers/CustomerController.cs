using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CustomerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private static int _indexer;
        private static ConcurrentDictionary<string, CustomerModel> _data = new ConcurrentDictionary<string, CustomerModel>();

        // POST api/customer
        [HttpPost]
        public ActionResult<string> AddCustomer([FromBody]CustomerModel customer)
        {
            if (string.IsNullOrEmpty(customer.Id))
                customer.Id = Interlocked.Increment(ref _indexer).ToString();

            if (_data.ContainsKey(customer.Id))
                return BadRequest(new {Message = $"Customer with Id={customer.Id} already exist"});

            if (!_data.TryAdd(customer.Id, customer))
            {
                return BadRequest(new { Message = $"Customer with Id={customer.Id} already exist" });
            }

            return Ok(customer.Id);
        }

        // PUT api/customer
        [HttpPut]
        public ActionResult<string> UpdateCustomer([FromBody]CustomerModel customer)
        {
            if (!_data.ContainsKey(customer.Id))
                return BadRequest(new { Message = $"Customer with Id={customer.Id} not found" });
            
            _data.AddOrUpdate(customer.Id, customer, (s, model) => customer);

            return Ok(customer.Id);
        }

        // GET api/customer
        [HttpGet]
        public ActionResult<CustomerModel> GetCustomerById([FromQuery] string id)
        {
            if (_data.TryGetValue(id, out var customer))
            {
                return Ok(customer);
            }

            return NotFound(new {Message="Customer not found"});
        }

        // api/customer/half
        [HttpGet("half")]
        public ActionResult<string> Half()
        {
            return Ok($"Count users: {_data.Count}");
        }
    }

    public class CustomerModel
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
