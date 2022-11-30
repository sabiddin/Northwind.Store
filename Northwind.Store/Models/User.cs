namespace Northwind.Store.Models
{
    public class User
    {
        public int? UserId { get; set; }
        public string CustomerId { get; set; }
        public int? EmployeeId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string LoginId { get; set; }
        public string Password { get; set; }
        public bool? Admin { get; set; }
        public string Department { get; set; }
    }
}