using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqEfPractice.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string Name { get; set; } = "";
        public ICollection<Employee>? Employees { get; set; }
    }

    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; } = "";
        public int DepartmentId { get; set; }
        public int? ManagerId { get; set; }
        public DateTime JoinDate { get; set; }
        public bool IsActive { get; set; }
        public decimal Salary { get; set; }

        public Department? Department { get; set; }
        public Employee? Manager { get; set; }
        public ICollection<Employee>? DirectReports { get; set; }
    }

    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = "";
        public ICollection<Product>? Products { get; set; }
    }

    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = "";
        public int CategoryId { get; set; }
        public decimal Price { get; set; }

        public Category? Category { get; set; }
        public ICollection<OrderItem>? OrderItems { get; set; }
    }

    public class Customer
    {
        public int CustomerId { get; set; }
        public string Name { get; set; } = "";
        public string Country { get; set; } = "";
        public ICollection<Order>? Orders { get; set; }
    }

    public class Order
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = "";

        public Customer? Customer { get; set; }
        public ICollection<OrderItem>? Items { get; set; }
    }

    public class OrderItem
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public Order? Order { get; set; }
        public Product? Product { get; set; }
    }

    // Container for JSON deserialization
    public class RootObject
    {
        public List<Department> Departments { get; set; } = new();
        public List<Employee> Employees { get; set; } = new();
        public List<Category> Categories { get; set; } = new();
        public List<Product> Products { get; set; } = new();
        public List<Customer> Customers { get; set; } = new();
        public List<Order> Orders { get; set; } = new();
        public List<OrderItem> OrderItems { get; set; } = new();
    }
}
