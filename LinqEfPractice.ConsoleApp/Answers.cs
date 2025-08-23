using LinqEfPractice;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Linq;

namespace LinqEfPractice.ConsoleApp
{
    public class Answers
    {
        private readonly PracticeDbContext _db;

        // Constructor to inject db context
        public Answers(PracticeDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Scenario 1:
        /// Return ProductId, Name, Price for all products 
        /// in the Electronics category with Price < 6000,
        /// sorted by Price ascending, then Name.
        /// </summary>
        public void Scenario1()
        {

            var result = _db.Products
                .Join(_db.Categories,
                      p => p.CategoryId,
                      c => c.CategoryId,
                      (p, c) => new { p, c })
                .Where(x => x.c.Name == "Electronics" && x.p.Price < 6000)
                .OrderBy(x => x.p.Price)
                .ThenBy(x => x.p.Name)
                .Select(x => new { x.p.ProductId, x.p.Name, x.p.Price })
                .ToList();

            Console.WriteLine("Scenario 1 Results:");
            foreach (var item in result)
            {
                Console.WriteLine($"{item.ProductId} - {item.Name} - {item.Price}");
            }
        }

        public void Scenario1_Optimize()
        {
            var result = _db.Products
                .AsNoTracking()
                .Where(p => p.Price < 6000 && p.Category!.Name == "Electronics")
                .OrderBy(p => p.Price)
                .ThenBy(p => p.Name)
                .Select(p => new { p.ProductId, p.Name, p.Price })
                .ToList();

            Console.WriteLine("Scenario 1 Results:");
            foreach (var item in result)
                Console.WriteLine($"{item.ProductId} - {item.Name} - {item.Price}");
        }


        /// <summary>
        /// Scenario 2:
        /// List all active employees (IsActive == true) 
        /// who joined after Jan 1, 2023, 
        /// sorted by JoinDate ascending.
        /// Return EmployeeId, FullName, JoinDate.
        /// </summary>
        public void Scenario2()
        {
            var result = _db.Employees
                .Where(e => e.IsActive && e.JoinDate > new DateTime(2023, 1, 1))
                .OrderBy(e => e.JoinDate)
                .Select(e => new { e.EmployeeId, e.FullName, e.JoinDate })
                .ToList();

            Console.WriteLine("\nScenario 2 Results:");
            foreach (var emp in result)
            {
                Console.WriteLine($"{emp.EmployeeId} - {emp.FullName} - {emp.JoinDate:yyyy-MM-dd}");
            }
        }


        /// <summary>
        /// Scenario 3:
        /// For each department, list:
        /// - Department Name
        /// - Total Employees
        /// - Average Salary
        /// - Highest Salary
        /// Sort by Department Name.
        /// </summary>
        public void Scenario3()
        {
            // TODO: Write your LINQ query here

           var result = _db.Employees.AsNoTracking()
                .GroupBy(e => e.DepartmentId)
                .Select(g => new
                {
                    DepartmentName = _db.Departments
                    .Where(d => d.DepartmentId == g.Key)
                    .Select(d => d.Name).FirstOrDefault(),

                    TotalEmployees = g.Count(),
                    AverageSalary = g.Average(e => e.Salary),
                    MaxSalary = g.Max(e => e.Salary)
                })
                .OrderBy(r => r.DepartmentName)
                .ToList();

            Console.WriteLine("\nScenario 3 Results:");
            foreach (var emp in result)
            {
                Console.WriteLine($"{emp.DepartmentName} - {emp.TotalEmployees} - {emp.AverageSalary:0} - {emp.MaxSalary:0}");

            }

        }

        /// <summary>
        /// Scenario 4:
        /// List all products with Price greater than 1000.
        /// Return ProductId, Name, and Price.
        /// Sort by Price descending.
        /// </summary>
        public void Scenario4()
        {
            // TODO: Write your LINQ query here
          var result =  _db.Products
                .AsNoTracking()
                .Where(p => p.Price > 1000)
                .Select(p => new
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Price = p.Price
                })
                .OrderByDescending(p => p.Price)
                .ToList();


            Console.WriteLine("\nScenario 4 Results:");
            foreach (var prod in result)
            {
                Console.WriteLine($"{prod.ProductId} - {prod.Name} - {prod.Price}");

            }
        }


        /// <summary>
        /// Scenario 5:
        /// List all customers who are from "India".
        /// Return CustomerId, Name, and Country.
        /// Sort by Name ascending.
        /// </summary>
        public void Scenario5()
        {
            // TODO: Write your LINQ query here
           var result = _db.Customers
                .AsNoTracking()
                .Where(c => c.Country.Equals("India", StringComparison.OrdinalIgnoreCase))
                .OrderBy(c => c.Name)
                .Select(c => new
                {
                    CustomerId = c.CustomerId,
                    Name = c.Name,
                    Country = c.Country
                })
                .ToList();

            Console.WriteLine("\nScenario 5 Results:");
            foreach (var cust in result)
            {
                Console.WriteLine($"{cust.CustomerId} - {cust.Name}  -  {cust.Country}");

            }
        }


        /// <summary>
        /// Scenario 6:
        /// List all employees who are NOT active (IsActive == false).
        /// Return EmployeeId, FullName, and IsActive.
        /// Sort by FullName ascending.
        /// </summary>
        public void Scenario6()
        {
            // TODO: Write your LINQ query here
           var result =  _db.Employees
                .AsNoTracking()
                .Where(e=> e.IsActive == false)
                .OrderBy( e => e.FullName)
                .Select(e => new {
                    EmployeeID= e.EmployeeId,
                    FullName = e.FullName,
                    IsActive = e.IsActive
                })
                .ToList();

            Console.WriteLine("\nScenario 6 Results:");
            foreach (var res in result)
            {
                Console.WriteLine($"{res.EmployeeID} - {res.FullName}  -  {res.IsActive}");

            }
        }


        /// <summary>
        /// Scenario 7:
        /// List all products whose Name starts with "M".
        /// Return ProductId, Name, and Price.
        /// Sort by Name ascending.
        /// </summary>
        public void Scenario7()
        {
            // TODO: Write your LINQ query here
            var result = _db.Products.AsNoTracking()
                            .Where(p => p.Name.StartsWith("M", StringComparison.OrdinalIgnoreCase))
                            .OrderBy(p => p.Name)
                            .Select(p => new
                            {
                                ProductId = p.ProductId,
                                Name = p.Name,
                                Price = p.Price
                            }).ToList();

            Console.WriteLine("\nScenario 7 Results:");
            foreach (var res in result)
            {
                Console.WriteLine($"{res.ProductId} - {res.Name}  -  {res.Price:0.00}");

            }


        }


        /// <summary>
        /// Scenario 8:
        /// List all orders placed after January 1, 2025.
        /// Return OrderId, CustomerId, and OrderDate.
        /// Sort by OrderDate ascending.
        /// </summary>
        public void Scenario8()
        {
            // TODO: Write your LINQ query here
          var result =  _db.Orders.AsNoTracking()
                .Where(o => o.OrderDate > new DateTime(2025, 1, 1))
                .OrderBy(o => o.OrderDate)
                .Select(o => new
                {
                    OrderId = o.OrderId,
                    CustomerId = o.CustomerId,
                    OrderDate = o.OrderDate
                }).ToList();

            Console.WriteLine("\nScenario 8 Results:");
            foreach (var res in result)
            {
                Console.WriteLine($"{res.OrderId} - {res.CustomerId}  -  {res.OrderDate:yyyy-MM-dd}");

            }
        }

        /// <summary>
        /// Scenario 9:
        /// Using only the Orders table:
        /// For each Status, return:
        /// - Status
        /// - TotalOrders (count)
        /// - EarliestOrderDate (min)
        /// - LatestOrderDate (max)
        /// Sort by TotalOrders descending, then Status ascending.
        /// </summary>
        public void Scenario9()
        {
            // TODO: Write your LINQ query here
            var result = _db.Orders.AsNoTracking()
                .GroupBy(o => o.Status)
                .Select(g => new
                {
                    Status = g.Key,
                    TotalOrders = g.Count(),
                    EarliestOrderDate = g.Min(o => o.OrderDate),
                    LatestOrderDate = g.Max(o => o.OrderDate)
                })
                .OrderByDescending(x => x.TotalOrders)
                .ThenBy(x => x.Status)
                .ToList();


            Console.WriteLine("\nScenario 9 Results:");
            foreach (var res in result)
            {
                Console.WriteLine($"{res.TotalOrders} - {res.EarliestOrderDate:yyyy-MM-dd}  -  {res.LatestOrderDate:yyyy-MM-dd}");

            }

        }


        /// <summary>
        /// Scenario 10:
        /// Using only the Orders table:
        /// 1) Consider only orders with OrderDate >= 2025-01-01 (WHERE).
        /// 2) Group by CustomerId.
        /// 3) For each group, return:
        ///    - CustomerId
        ///    - TotalOrders (count)
        ///    - FirstOrderDate (min)
        ///    - LastOrderDate (max)
        /// 4) Keep only groups where TotalOrders >= 2 (HAVING).
        /// 5) Sort by LastOrderDate descending, then CustomerId ascending.
        /// </summary>
        public void Scenario10()
        {
            // TODO: Write your LINQ query here
            var result = _db.Orders
                .AsNoTracking()
                .Where(o => o.OrderDate >= new DateTime(2025, 01, 01))
                .GroupBy(o => o.CustomerId)
                .Select(g => new
                {

                    CustomerId = g.Key,
                    TotalOrders = g.Count(),
                    FirstOrderDate = g.Min(o => o.OrderDate),
                    LastOrderDate = g.Max(o => o.OrderDate)
                })
                .Where(g => g.TotalOrders >= 2)
                .OrderByDescending(o => o.LastOrderDate)
                .ThenBy(c => c.CustomerId)
                .ToList();

            Console.WriteLine("\nScenario 10 Results:");
            foreach (var res in result)
            {
                Console.WriteLine($"{res.TotalOrders} - {res.FirstOrderDate:yyyy-MM-dd}  -  {res.LastOrderDate:yyyy-MM-dd}");

            }
        }

        /// <summary>
        /// Scenario 11:
        /// Using only the Employees table:
        /// 1) Consider employees with JoinDate >= 2022-01-01.
        /// 2) Group by the Join Year (e.JoinDate.Year).
        /// 3) For each year, return:
        ///    - Year
        ///    - TotalEmployees     
        ///    - MaxSalary
        /// 4) Keep only years where TotalEmployees >= 2.
        /// 5) Sort by Year ascending.
        /// </summary>
        public void Scenario11()
        {
            // TODO: Write your LINQ query here
            
                         
        }



    }
}
