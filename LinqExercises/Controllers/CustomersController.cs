using LinqExercises.Infrastructure;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace LinqExercises.Controllers
{
    public class CustomersController : ApiController
    {
        private NORTHWNDEntities _db;

        public CustomersController()
        {
            _db = new NORTHWNDEntities();
        }

        // GET: api/customers/city/London
        [HttpGet, Route("api/customers/city/{city}"), ResponseType(typeof(IQueryable<Customer>))]
        public IHttpActionResult GetAll(string city)
        {
            IQueryable<Customer> customer = _db.Customers.Where(c => c.City == city);
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        // GET: api/customers/mexicoSwedenGermany
        [HttpGet, Route("api/customers/mexicoSwedenGermany"), ResponseType(typeof(IQueryable<Customer>))]
        public IHttpActionResult GetAllFromMexicoSwedenGermany()
        {
            string[] countries = { "Mexico", "Sweden", "Germany" };
            IQueryable<Customer> customer = _db.Customers.Where(u => countries.Contains(u.Country));
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        // GET: api/customers/shippedUsing/Speedy Express
        [HttpGet, Route("api/customers/shippedUsing/{shipperName}"), ResponseType(typeof(IQueryable<Customer>))]
        public IHttpActionResult GetCustomersThatShipWith(string shipperName)
        {
            //throw new NotImplementedException("Write a query to return all customers with orders that shipped 
            //using the given shipperName.");
            var Customername = from cust in _db.Customers
                              join o in _db.Orders on cust.CustomerID equals o.CustomerID
                              join s in _db.Shippers on o.ShipVia equals s.ShipperID
                              where s.CompanyName == shipperName
                              select cust;
            var distinctCutomers = Customername.Distinct();
            return Ok(distinctCutomers);
        }

        // GET: api/customers/withoutOrders
        [HttpGet, Route("api/customers/withoutOrders"), ResponseType(typeof(IQueryable<Customer>))]
        public IHttpActionResult GetCustomersWithoutOrders()
        {
            //throw new NotImplementedException("Write a query to return all customers with no orders in the Orders table.");
            //    SELECT C.CompanyName FROM Customers C LEFT JOIN Orders O
            //    ON c.CustomerID = O.CustomerID WHERE O.OrderID IS NULL
            var customers = from cust in _db.Customers
                            join ord in _db.Orders on cust.CustomerID equals ord.CustomerID into nullList
                            from nulls in nullList.DefaultIfEmpty()
                            where nulls == null
                            select cust;
     
            return Ok(customers);
    }
 

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }
    }
}
