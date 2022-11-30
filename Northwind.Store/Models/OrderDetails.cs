namespace Northwind.Store.Models
{
    using System;

    public class OrderDetails
    {
        public int? OrderId { get; set; }
        public string CustomerId { get; set; }
        public int? EmployeeId { get; set; }
        public DateTime OrderDate { get; set; }
        public int? ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal? Discount { get; set; }
        public int? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
    }
}