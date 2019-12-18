using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace OrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private static ConcurrentDictionary<string, InvoiceModel> _data = new ConcurrentDictionary<string, InvoiceModel>();

        // POST api/invoice
        [HttpPost]
        public ActionResult<InvoiceModel> Create([FromBody]InvoiceModel invoice)
        {
            invoice.Id = Guid.NewGuid().ToString("N");
            _data[invoice.Id] = invoice;
            return Ok(invoice);
        }

        // GET api/invoice/5
        [HttpGet("{id}")]
        public ActionResult<InvoiceModel> GetById(string id)
        {
            if (_data.TryGetValue(id, out var invoice))
            {
                return Ok(invoice);
            }

            return NotFound(new {Message = "Invoice not found"});
        }

        // GET api/invoice
        [HttpGet]
        public ActionResult<List<InvoiceModel>> GetAll()
        {
            return Ok(_data.Values.ToList());
        }
    }

    public class InvoiceModel
    {
        public string Id { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }
    }
}
