﻿using LinqExercises.Infrastructure;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace LinqExercises.Controllers
{
    public class OrdersController : ApiController
    {
        private NORTHWNDEntities _db;

        public OrdersController()
        {
            _db = new NORTHWNDEntities();
        }

        //GET: api/orders/between/01.01.1997/12.31.1997
        [HttpGet, Route("api/orders/between/{startDate}/{endDate}"), ResponseType(typeof(IQueryable<Order>))]
        public IHttpActionResult GetOrdersBetween(DateTime startDate, DateTime endDate)
        {
            //throw new NotImplementedException("Write a query to return all orders with required dates between the given start date and the given end date with freight under 100 units.");
            IQueryable<Order> orders = _db.Orders.Where(o => o.OrderDate >= startDate.Date && o.OrderDate <= endDate && o.Freight < 100.0m);
            if (orders == null)
            {
                return NotFound();
            }

            return Ok(orders);
        }

        //GET: api/orders/reports/purchase
        [HttpGet, Route("api/orders/reports/purchase"), ResponseType(typeof(IQueryable<object>))]
        public IHttpActionResult PurchaseReport()
        {
            // See this blog post for more information about projecting to anonymous objects. https://blogs.msdn.microsoft.com/swiss_dpe_team/2008/01/25/using-your-own-defined-type-in-a-linq-query-expression/
            //throw new NotImplementedException(@"
            //    Write a query to return an array of anonymous objects that have two properties. 

            //    1. A Product property containing that particular product
            //    2. A QuantityPurchased property containing the number of times that product was purchased.

            //    This array should be ordered by QuantityPurchased in descending order.
            //");

            var query = from orderDetail in _db.Order_Details
                        group orderDetail by orderDetail.ProductID into orderGroup
                        select new
                        {
                            ProdctID = orderGroup.Key,
                            Quantity = orderGroup.Sum(x => x.Quantity)
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
