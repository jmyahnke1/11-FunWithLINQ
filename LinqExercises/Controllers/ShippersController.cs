using LinqExercises.Infrastructure;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using System.Linq.Dynamic;

namespace LinqExercises.Controllers
{
    public class ShippersController : ApiController
    {
        private NORTHWNDEntities _db;

        public ShippersController()
        {
            _db = new NORTHWNDEntities();
        }

        //GET: api/shippers/reports/freight
        [HttpGet, Route("/api/shippers/reports/freight"), ResponseType(typeof(IQueryable<dynamic>))]
        public IHttpActionResult GetFreightReport()
        {
            // See this blog post for more information about projecting to anonymous objects. https://blogs.msdn.microsoft.com/swiss_dpe_team/2008/01/25/using-your-own-defined-type-in-a-linq-query-expression/
            //throw new NotImplementedException(@"
            //    Write a query to return an array of anonymous objects that have two properties. 

            //    1. A Shipper property containing that particular shipper.
            //    2. A FreightTotals property containing the freight totals for that shipper

            //    Return the rows ordered by FreightTotals
            //");

            var query = from sup in _db.Suppliers
                        join ord in _db.Orders on sup.SupplierID equals ord.ShipVia
                        group ord by sup.SupplierID into supplierGroup
                        select new
                        {
                            Shipper = supplierGroup.Key,
                            FreightTotals = supplierGroup.Sum(fr => fr.Freight)
                        };

            if (query == null)
                return NotFound();
            else
                return Ok(query);

        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }
    }
}
