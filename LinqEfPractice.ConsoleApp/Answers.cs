﻿using LinqEfPractice;
using LinqEfPractice.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
            var result = _db.Products
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
            var result = _db.Employees
                 .AsNoTracking()
                 .Where(e => e.IsActive == false)
                 .OrderBy(e => e.FullName)
                 .Select(e => new
                 {
                     EmployeeID = e.EmployeeId,
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
            var result = _db.Orders.AsNoTracking()
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
            var result = _db.Employees.AsNoTracking()
                            .Where(e => e.JoinDate >= new DateTime(2022, 1, 1))
                            .GroupBy(g => g.JoinDate.Year)

                            .Select(g => new
                            {

                                Year = g.Key,
                                TotalEmployees = g.Count(),
                                MaxSalary = g.Max(e => e.Salary)
                            })
                            .Where(g => g.TotalEmployees >= 2)
                            .OrderBy(g => g.Year)
                            .ToList();

            Console.WriteLine("\nScenario 11 Results:");
            foreach (var res in result)
            {
                Console.WriteLine($"{res.Year} - {res.TotalEmployees}  -  {res.MaxSalary}");

            }
        }

        /// <summary>
        /// Scenario 12 (Employees — conditional aggregates):
        /// For each JoinYear (e.JoinDate.Year), return:
        /// - Year
        /// - ActiveCount (employees with IsActive == true)
        /// - InactiveCount (employees with IsActive == false)
        //  - AvgSalaryActive (average salary of active employees)
        //  - MaxSalaryInactive (max salary among inactive employees; null if none)
        /// Keep only years where BOTH ActiveCount >= 1 AND InactiveCount >= 1.
        /// Sort by Year ascending.
        /// </summary>
        public void Scenario12()
        {
            // TODO: Write your LINQ query here

            var result = _db.Employees.AsNoTracking()
                            .GroupBy(g => g.JoinDate.Year)
                            .Select(g => new
                            {
                                Year = g.Key,
                                ActiveCount = g.Count(x => x.IsActive),
                                InactiveCount = g.Count(x => !x.IsActive),
                                AvgSalaryActive = g.Where(x => x.IsActive).Any() ? g.Where(x => x.IsActive).Average(x => x.Salary) : (decimal?)null,
                                MaxSalaryInactive = g.Where(x => !x.IsActive).Any() ?
                                                       g.Where(x => !x.IsActive).Max(x => x.Salary) : (decimal?)null,
                            })
                            .Where(g => g.ActiveCount >= 1 && g.InactiveCount >= 1)
                            .OrderBy(g => g.Year)
                            .ToList();

            Console.WriteLine("\nScenario 12 Results:");
            foreach (var res in result)
            {
                Console.WriteLine($"{res.Year} - {res.ActiveCount}  -  {res.InactiveCount} - {res.AvgSalaryActive}  -  {res.MaxSalaryInactive}");

            }
        }

        /// <summary>
        /// Scenario 13 (Products — price analytics):
        /// Using only the Products table:
        /// 1) Group products by CategoryId.
        /// 2) For each group, return:
        ///    - CategoryId
        ///    - ProductCount
        ///    - MinPrice
        ///    - MaxPrice
        ///    - AvgPrice
        /// 3) Keep only categories where ProductCount >= 2.
        /// 4) Sort by AvgPrice descending.
        /// </summary>
        public void Scenario13()
        {
            // TODO: Write your LINQ query here
            var result = _db.Products.AsNoTracking()
                            .GroupBy(g => g.CategoryId)
                            .Select(g => new
                            {
                                CategoryId = g.Key,
                                ProductCount = g.Count(),
                                MinPrice = g.Min(x => x.Price),
                                MaxPrice = g.Max(x => x.Price),
                                AvgPrice = g.Average(x => x.Price)
                            })
                            .Where(x => x.ProductCount >= 2)
                            .OrderByDescending(g => g.AvgPrice)
                            .ToList();

            Console.WriteLine("\nScenario 13 Results:");
            foreach (var res in result)
            {
                Console.WriteLine($"{res.CategoryId} - {res.ProductCount}  -  {res.MinPrice} - {res.MaxPrice}  -  {res.AvgPrice}");

            }
        }

        /// <summary>
        /// Scenario 14 (Products — Top N per group, single table):
        /// From the Products table only:
        /// For each CategoryId, return the TOP 2 most expensive products in that category.
        /// Each result row should include:
        /// - CategoryId
        /// - ProductId
        /// - Name
        /// - Price
        /// Sort products within each category by Price descending, then Name ascending.
        /// Finally, sort the overall output by CategoryId ascending, then Price descending.
        /// </summary>
        public void Scenario14()
        {
            // TODO: Write your LINQ query here
            var result = _db.Products
                 .AsNoTracking()
                 .ToList() // materialize -> LINQ to Objects from here
                 .GroupBy(p => p.CategoryId)
                 .SelectMany(g => g
                     .OrderByDescending(p => p.Price)
                     .ThenBy(p => p.Name)
                     .Take(2)
                     .Select(p => new
                     {
                         CategoryId = g.Key,
                         p.ProductId,
                         p.Name,
                         p.Price
                     }))
                 .OrderBy(x => x.CategoryId)
                 .ThenByDescending(x => x.Price)
                 .ThenBy(x => x.Name)
                 .ToList();


            Console.WriteLine("\nScenario 14 Results:");
            Console.WriteLine("CategoryId | ProductId | Name                  | Price");
            Console.WriteLine("-----------|-----------|-----------------------|-------");

            foreach (var res in result)
            {
                Console.WriteLine($"{res.CategoryId,-10} | {res.ProductId,-9} | {res.Name,-21} | {res.Price,6}");
            }
        }


        /// <summary>
        /// Scenario 15 (Orders — recent per customer):
        /// From the Orders table:
        /// For each CustomerId, return the MOST RECENT order (latest OrderDate).
        /// Each result row should include:
        /// - CustomerId
        /// - OrderId
        /// - OrderDate
        /// - Status
        /// Sort final results by CustomerId ascending.
        /// </summary>
        public void Scenario15()
        {
            // TODO: Write your LINQ query here
            var result = _db.Orders.AsNoTracking()
                            .ToList()
                            .GroupBy(g => g.CustomerId)
                            .SelectMany(g => g
                                .OrderByDescending(g => g.OrderDate)
                                .Take(1)
                                .Select(o => new
                                {
                                    CustomerId = g.Key,
                                    OrderId = o.OrderId,
                                    OrderDate = o.OrderDate,
                                    Status = o.Status
                                }))
                                .OrderBy(x => x.CustomerId)
                                .ToList();

            Console.WriteLine("\nScenario 15 Results:");
            Console.WriteLine("CustomerId | OrderId | OrderDate   | Status");
            Console.WriteLine("-----------|---------|-------------|---------");

            foreach (var res in result)
            {
                Console.WriteLine($"{res.CustomerId,-10} | {res.OrderId,-7} | {res.OrderDate:yyyy-MM-dd} | {res.Status}");
            }

        }


        /// <summary>
        /// Scenario 16 (Orders — earliest per customer):
        /// From the Orders table:
        /// For each CustomerId, return the EARLIEST order (minimum OrderDate).
        /// Each result row should include:
        /// - CustomerId
        /// - OrderId
        /// - OrderDate
        /// - Status
        /// Sort final results by CustomerId ascending.
        /// </summary>
        public void Scenario16()
        {
            // TODO: Write your LINQ query here

            var result = _db.Orders.AsNoTracking()
                            .ToList()
                            .GroupBy(g => g.CustomerId)
                            .SelectMany(g => g
                                .OrderBy(g => g.OrderDate)
                                .ThenBy(o => o.OrderId)
                                .Take(1)
                                .Select(o => new
                                {
                                    CustomerId = g.Key,
                                    OrderId = o.OrderId,
                                    OrderDate = o.OrderDate,
                                    Status = o.Status
                                }))
                            .OrderBy(x => x.CustomerId)
                            .ToList();

            Console.WriteLine("\nScenario 16 Results:");
            Console.WriteLine("CustomerId | OrderId | OrderDate   | Status");
            Console.WriteLine("-----------|---------|-------------|---------");
            foreach (var r in result)
            {
                Console.WriteLine($"{r.CustomerId,-10} | {r.OrderId,-7} | {r.OrderDate:yyyy-MM-dd} | {r.Status}");
            }
        }

        /// <summary>
        /// Scenario 17 (Orders — highest bill per customer):
        /// From the Orders table:
        /// For each CustomerId, return the order with the HIGHEST TotalBill.
        /// Each result row should include:
        /// - CustomerId
        /// - OrderId
        /// - OrderDate
        /// - TotalBill
        /// Sort final results by TotalBill descending, then CustomerId ascending.
        /// </summary>
        public void Scenario17()
        {
            // TODO: Write your LINQ query here
            var results = _db.Orders.AsNoTracking()
                              .ToList()
                              .GroupBy(g => g.CustomerId)
                              .SelectMany(g => g
                                   .OrderByDescending(o => o.TotalBill)
                                   .ThenByDescending(o => o.OrderDate)
                                   .Take(1)
                                   .Select(o => new
                                   {
                                       CustomerId = o.CustomerId,
                                       OrderId = o.OrderId,
                                       OrderDate = o.OrderDate,
                                       TotalBill = o.TotalBill
                                   }))
                              .OrderByDescending(x => x.TotalBill)
                              .ThenBy(x => x.CustomerId)
                              .ToList();

            Console.WriteLine("\nScenario 17 Results:");
            Console.WriteLine("CustomerId | OrderId | OrderDate   | TotalBill");
            Console.WriteLine("-----------|---------|-------------|----------");
            foreach (var r in results)
            {
                Console.WriteLine($"{r.CustomerId,-10} | {r.OrderId,-7} | {r.OrderDate:yyyy-MM-dd} | {r.TotalBill,9:0.00}");
            }


        }

        /// <summary>
        /// Scenario 18 (Orders — first order per day):
        /// From the Orders table:
        /// For each calendar day (OrderDate.Date), return the EARLIEST order of that day.
        /// Each row should include:
        /// - Date
        /// - OrderId
        /// - CustomerId
        /// - OrderDate
        /// - Status
        /// Sort final results by Date ascending, then OrderId ascending.
        /// </summary>
        public void Scenario18()
        {
            // TODO: Write your LINQ query here
            var result = _db.Orders.AsNoTracking()
                            .ToList()
                            .GroupBy(o => o.OrderDate.Date)
                            .SelectMany(g => g
                            .OrderBy(o => o.OrderDate)
                            .Take(1)
                            .Select(o => new
                            {
                                Date = o.OrderDate.Date,
                                OrderId = o.OrderId,
                                CustomerId = o.CustomerId,
                                OrderDate = o.OrderDate,
                                Status = o.Status
                            }))
                            .OrderBy(o => o.Date)
                            .ThenBy(o => o.OrderId)
                            .ToList();

            Console.WriteLine("\nScenario 18 Results:");
            Console.WriteLine("Date        | OrderId | CustomerId | OrderDate   | Status");
            Console.WriteLine("------------|---------|------------|-------------|---------");

            foreach (var r in result)
            {
                Console.WriteLine($"{r.Date:yyyy-MM-dd} | {r.OrderId,-7} | {r.CustomerId,-10} | {r.OrderDate:yyyy-MM-dd HH:mm} | {r.Status}");
            }


        }

        /// <summary>
        /// Scenario 19 (Orders — latest per status):
        /// From the Orders table:
        /// For each Status, return the MOST RECENT order (max OrderDate).
        /// Each row should include:
        /// - Status
        /// - OrderId
        /// - CustomerId
        /// - OrderDate
        /// Sort final results by Status ascending.
        /// </summary>
        public void Scenario19()
        {
            // TODO: Write your LINQ query here

            var result = _db.Orders.AsNoTracking()
                            .ToList()
                            .GroupBy(o => o.Status)
                            .SelectMany(g => g
                                .OrderByDescending(o => o.OrderDate)
                                .ThenByDescending(o => o.OrderId)
                                .Take(1)
                                .Select(o => new
                                {
                                    Status = o.Status,
                                    OrderId = o.OrderId,
                                    CustomerId = o.CustomerId,
                                    OrderDate = o.OrderDate
                                }))
                            .OrderBy(o => o.Status)
                            .ToList();

            Console.WriteLine("\nScenario 19 Results:");
            Console.WriteLine("Status     | OrderId | CustomerId | OrderDate");
            Console.WriteLine("-----------|---------|------------|-------------");
            foreach (var r in result)
            {
                Console.WriteLine($"{r.Status,-10} | {r.OrderId,-7} | {r.CustomerId,-10} | {r.OrderDate:yyyy-MM-dd HH:mm}");
            }

        }

        /// <summary>
        /// Scenario 20 (Orders — customer cohort summary + pick representative order):
        /// Using only the Orders table:
        /// 1) Consider orders on/after 2024-01-01.
        /// 2) Group by CustomerId and compute, per customer:
        ///    - OrderCount
        ///    - FirstOrderDate (min)
        ///    - LastOrderDate  (max)
        ///    - AvgBill        (average of TotalBill)
        ///    - MaxBill        (maximum TotalBill)
        ///    - ActiveDays     ((LastOrderDate - FirstOrderDate).TotalDays)
        /// 3) Keep only customers where:
        ///    - OrderCount >= 2
        ///    - ActiveDays  >= 30
        /// 4) For each remaining customer, return the SINGLE order that has the MaxBill
        ///    (if there are ties, pick the most recent OrderDate).
        /// 5) Final output columns:
        ///    - CustomerId, OrderId, OrderDate, TotalBill, OrderCount, FirstOrderDate, LastOrderDate, AvgBill, MaxBill, ActiveDays
        /// 6) Sort by TotalBill descending, then CustomerId ascending.
        /// </summary>
        public void Scenario20()
        {
            // TODO: Write your LINQ query here

            var start = new DateTime(2024, 1, 1);

            // 1–2) Filter + group + aggregates (keep CustomerId)
            var grouped = _db.Orders
                .AsNoTracking()
                .Where(o => o.OrderDate >= start)
                .GroupBy(o => o.CustomerId)
                .Select(g => new
                {
                    CustomerId = g.Key,
                    OrderCount = g.Count(),
                    FirstOrderDate = g.Min(x => x.OrderDate),
                    LastOrderDate = g.Max(x => x.OrderDate),
                    AvgBill = g.Average(x => x.TotalBill),
                    MaxBill = g.Max(x => x.TotalBill),
                    Orders = g
                })
                .ToList();


            // 3) Add ActiveDays and apply HAVING

            var filtered = grouped
                            .Select(x => new
                            {

                                x.CustomerId,
                                x.OrderCount,
                                x.FirstOrderDate,
                                x.LastOrderDate,
                                x.AvgBill,
                                x.MaxBill,
                                ActiveDays = (x.LastOrderDate - x.FirstOrderDate).TotalDays,
                                x.Orders
                            })
                            .Where(x => x.OrderCount >= 2 && x.ActiveDays >= 30)
                            .ToList();

            // 4) For each remaining customer, pick the SINGLE order with MaxBill (tie-break: latest OrderDate)

            var result = filtered.
                Select(x =>
                {

                    var top = x.Orders
                               .OrderByDescending(o => o.TotalBill)
                               .ThenByDescending(o => o.OrderDate)
                               .First();


                    return new
                    {

                        x.CustomerId,
                        OrderId = top.OrderId,
                        OrderDate = top.OrderDate,
                        TotalBill = top.TotalBill,
                        x.OrderCount,
                        x.FirstOrderDate,
                        x.LastOrderDate,
                        x.AvgBill,
                        x.MaxBill,
                        x.ActiveDays

                    };
                })
                // 6) Final sort
                .OrderByDescending(r => r.TotalBill)
                .ThenBy(r => r.CustomerId)
                .ToList();

            // Pretty print
            Console.WriteLine("\nScenario 20 Results:");
            Console.WriteLine("CustomerId | OrderId | OrderDate   | TotalBill | Orders | FirstOrder  | LastOrder   | AvgBill  | MaxBill  | ActiveDays");
            Console.WriteLine("-----------|---------|-------------|-----------|--------|-------------|-------------|----------|----------|-----------");
            foreach (var r in result)
            {
                Console.WriteLine($"{r.CustomerId,-10} | {r.OrderId,-7} | {r.OrderDate:yyyy-MM-dd} | {r.TotalBill,9:0.00} | " +
                                  $"{r.OrderCount,6} | {r.FirstOrderDate:yyyy-MM-dd} | {r.LastOrderDate:yyyy-MM-dd} | " +
                                  $"{r.AvgBill,8:0.00} | {r.MaxBill,8:0.00} | {r.ActiveDays,10:0}");
            }


        }


        /// <summary>
        /// Scenario 21 (Orders — highest bill per day):
        /// From the Orders table:
        /// For each calendar day (OrderDate.Date), return the SINGLE order
        /// with the highest TotalBill (tie-breaker: latest OrderDate).
        /// Return: Date, OrderId, CustomerId, TotalBill, OrderDate.
        /// Sort by Date ascending, then TotalBill descending.
        /// </summary>
        public void Scenario21()
        {
            // TODO: Write your LINQ query here
            var result = _db.Orders.AsNoTracking()
                            .ToList()
                            .GroupBy(o => o.OrderDate.Date)
                            .SelectMany(g => g

                                .OrderByDescending(o => o.TotalBill)
                                .ThenByDescending(o => o.OrderDate)
                                .Take(1)
                                .Select(
                                    s => new
                                    {
                                        Date = g.Key,
                                        OrderId = s.OrderId,
                                        CustomerId = s.CustomerId,
                                        TotalBill = s.TotalBill,
                                        OrderDate = s.OrderDate,
                                    }
                                ))
                            .OrderBy(r => r.Date)
                            .ThenByDescending(r => r.TotalBill)
                            .ToList();

            // Pretty console output
            Console.WriteLine("\nScenario 21 Results:");
            Console.WriteLine("Date        | OrderId | CustomerId | TotalBill | OrderDate");
            Console.WriteLine("------------|---------|------------|-----------|----------------");
            foreach (var r in result)
            {
                Console.WriteLine($"{r.Date:yyyy-MM-dd} | {r.OrderId,-7} | {r.CustomerId,-10} | {r.TotalBill,9:0.00} | {r.OrderDate:yyyy-MM-dd HH:mm}");
            }

        }

        /// <summary>
        /// Scenario 22 (Employees — top salary per department):
        /// From the Employees table:
        /// For each DepartmentId, return the SINGLE employee with the HIGHEST Salary
        /// (tie‑breaker: latest JoinDate, then EmployeeId).
        /// Return: DepartmentId, EmployeeId, FullName, Salary, JoinDate.
        /// Sort final results by DepartmentId ascending, then Salary descending.
        /// </summary>
        public void Scenario22()
        {
            // TODO: Write your LINQ query here

            var result = _db.Employees.AsNoTracking()
                            .ToList()
                            .GroupBy(e => e.DepartmentId)
                            .SelectMany(g => g
                                        .OrderByDescending(e => e.Salary)
                                        .ThenByDescending(e => e.JoinDate)
                                        .ThenBy(e => e.EmployeeId)
                                        .Take(1)
                                        .Select(e => new
                                        {

                                            DepartmentId = g.Key,
                                            EmployeeId = e.EmployeeId,
                                            FullName = e.FullName,
                                            Salary = e.Salary,
                                            JoinDate = e.JoinDate,
                                        }))
                            .OrderBy(r => r.DepartmentId)
                            .ThenByDescending(r => r.Salary)
                            .ToList();

            Console.WriteLine("\nScenario 22 Results:");
            Console.WriteLine("DeptId | EmployeeId | FullName               | Salary    | JoinDate");
            Console.WriteLine("-------|------------|------------------------|-----------|----------");
            foreach (var r in result)
            {
                Console.WriteLine($"{r.DepartmentId,6} | {r.EmployeeId,10} | {r.FullName,-22} | {r.Salary,9:0.00} | {r.JoinDate:yyyy-MM-dd}");
            }

        }

        /// <summary>
        /// Scenario 23 (Orders — 2025 filter + latest per status with having):
        /// Using only the Orders table:
        /// 1) WHERE: Consider orders with OrderDate >= 2025-01-01.
        /// 2) GROUP BY: Status.
        /// 3) HAVING: Keep only statuses where Count >= 2.
        /// 4) From each remaining status, return the SINGLE most recent order
        ///    (tie-breaker: highest OrderId).
        /// Return: Status, OrderId, CustomerId, OrderDate.
        /// Sort final results by Status ascending.
        /// </summary>
        public void Scenario23()
        {
            // TODO: Write your LINQ query here
            var result = _db.Orders
                    .AsNoTracking()
                    .Where(o => o.OrderDate >= new DateTime(2025, 1, 1))
                    .ToList()
                    .GroupBy(o => o.Status)
                    .Where(g => g.Count() >= 2) // HAVING
                    .SelectMany(g => g
                        .OrderByDescending(o => o.OrderDate)   // most recent first
                        .ThenByDescending(o => o.OrderId)      // tie-breaker
                        .Take(1)                               // just 1 order per status
                        .Select(o => new
                        {
                            Status = o.Status,
                            o.OrderId,
                            o.CustomerId,
                            o.OrderDate
                        }))
                    .OrderBy(r => r.Status) // final sort
                    .ToList();


            Console.WriteLine("\nScenario 23 Results:");
            Console.WriteLine("Status     | OrderId | CustomerId | OrderDate");
            Console.WriteLine("-----------|---------|------------|-------------");
            foreach (var r in result)
            {
                Console.WriteLine($"{r.Status,-10} | {r.OrderId,-7} | {r.CustomerId,-10} | {r.OrderDate:yyyy-MM-dd HH:mm}");
            }

        }

        /// <summary>
        /// Scenario 24 (Products — filtered + having + top‑1 per group):
        /// Using only the Products table:
        /// 1) WHERE: Consider products with Price >= 500.
        /// 2) GROUP BY: CategoryId.
        /// 3) HAVING: Keep only categories where ProductCount >= 2.
        /// 4) From each remaining category, return the SINGLE most expensive product
        ///    (tie‑breaker: Name ascending).
        /// Return: CategoryId, ProductId, Name, Price.
        /// Sort final results by CategoryId ascending.
        /// </summary>
        public void Scenario24()
        {
            // TODO: Write your LINQ query here

            var result = _db.Products.AsNoTracking()
                            .Where(p => p.Price >= 500)
                            .ToList()
                            .GroupBy(p => p.CategoryId)
                            .Where(p => p.Count() > 2)
                            .SelectMany(g => g
                                .OrderByDescending(p => p.Price)
                                .ThenBy(p => p.Name)
                                .Take(1)
                                .Select(p => new
                                {

                                    CategoryId = p.CategoryId,
                                    ProductId = p.ProductId,
                                    Name = p.Name,
                                    Price = p.Price,
                                }))
                            .OrderBy(p => p.CategoryId)
                            .ToList();

            Console.WriteLine("\nScenario 24 Results:");
            Console.WriteLine("CategoryId | ProductId | Name                 | Price");
            Console.WriteLine("-----------|-----------|----------------------|-----------");
            foreach (var r in result)
            {
                Console.WriteLine($"{r.CategoryId,-10} | {r.ProductId,-9} | {r.Name,-20} | {r.Price,9:0.00}");
            }
        }

        /// <summary>
        /// Scenario 25 (Products + Categories — basic join):
        /// List all products with their Category Name.
        /// Return: ProductId, ProductName, CategoryName, Price.
        /// Sort by CategoryName ascending, then ProductName ascending.
        /// </summary>
        public void Scenario25()
        {
            // TODO: Write your LINQ query here (EF Core method syntax, join Products ↔ Categories on CategoryId)

            //Option 1 (explicit join)
            var result = _db.Products
                            .AsNoTracking()
                            .Join(_db.Categories,
                                    p => p.CategoryId,
                                    c => c.CategoryId,
                                    (p, c) => new
                                    {
                                        p.ProductId,
                                        ProductName = p.Name,
                                        CategoryName = c.Name,
                                        p.Price
                                    })
                            .OrderBy(x => x.CategoryName)
                            .ThenBy(x => x.ProductName)
                            .ToList();

            //Option 2(navigation property)

            var result2 = _db.Products
                            .AsNoTracking()
                            .Select(p => new
                            {

                                p.ProductId,
                                ProductName = p.Name,
                                CategoryName = p.Category!.Name,
                                p.Price
                            })
                            .OrderBy(x => x.CategoryName)
                            .ThenBy(x => x.ProductName)
                            .ToList();

            Console.WriteLine("\nScenario 25 Results:");
            Console.WriteLine("ProductId | ProductName            | CategoryName        | Price");
            Console.WriteLine("----------|------------------------|---------------------|----------");
            foreach (var r in result) // or result2
            {
                Console.WriteLine($"{r.ProductId,9} | {r.ProductName,-22} | {r.CategoryName,-19} | {r.Price,8:0.00}");
            }


        }


        /// <summary>
        /// Scenario 26 (Products in a given category):
        /// List all products that belong to the "Electronics" category.
        /// Return: ProductId, ProductName, CategoryName, Price.
        /// Sort by Price descending, then ProductName ascending.
        /// (Provide both: Option 1 = explicit Join, Option 2 = navigation property)
        /// </summary>
        public void Scenario26()
        {
            // TODO: Write your LINQ query here (both options)
            //Option 1 (explicit join)

            var result = _db.Products
                         .AsNoTracking()
                         .Join(_db.Categories,
                                p => p.CategoryId,
                                c => c.CategoryId,
                                (p, c) => new
                                {
                                    p.ProductId,
                                    ProductName = p.Name,
                                    CategoryName = c.Name,
                                    Price = p.Price
                                })
                         .Where(c => c.CategoryName == "Electronics")
                         .OrderByDescending(p => p.Price)
                         .ThenBy(p => p.ProductName)
                         .ToList();

            //Option 2(navigation property)
            var result2 = _db.Products
                                .AsNoTracking()
                                .Where(c => c.Category!.Name == "Electronics")
                                .Select(p => new
                                {
                                    p.ProductId,
                                    ProductName = p.Name,
                                    CategoryName = p.Category!.Name,
                                    Price = p.Price
                                })
                                .OrderByDescending(p => p.Price)
                                .ThenBy(p => p.ProductName)
                                .ToList();

            Console.WriteLine("\nScenario 26 Results:");
            Console.WriteLine("ProductId | ProductName            | CategoryName | Price");
            Console.WriteLine("----------|------------------------|--------------|----------");
            foreach (var r in result) // or result2
            {
                Console.WriteLine($"{r.ProductId,9} | {r.ProductName,-22} | {r.CategoryName,-12} | {r.Price,8:0.00}");
            }

        }

        /// <summary>
        /// Scenario 27 (Orders + Customers — 2025 orders with customer name):
        /// List all orders placed in the year 2025, along with the customer's name.
        /// Return: OrderId, CustomerName, OrderDate, Status.
        /// Sort by OrderDate ascending, then CustomerName ascending.
        /// Provide BOTH:
        ///   Option 1 = explicit Join (Orders ↔ Customers on CustomerId)
        ///   Option 2 = navigation property (o.Customer.Name)
        /// </summary>
        public void Scenario27()
        {
            // TODO: Write your LINQ query here (both options)


            //Option 1 = explicit Join(Orders ↔ Customers on CustomerId)
            var result = _db.Orders.AsNoTracking()
                            .Where(o => o.OrderDate.Year == 2025)
                            .Join(_db.Customers,
                            o => o.CustomerId,
                            c => c.CustomerId,
                            (o, c) => new
                            {

                                o.OrderId,
                                CustomerName = c.Name,
                                OrderDate = o.OrderDate,
                                Status = o.Status,
                            })
                            .OrderBy(o => o.OrderDate)
                            .ThenBy(o => o.CustomerName)
                            .ToList();

            //Option 2 = navigation property (o.Customer.Name)

            var result2 = _db.Orders.AsNoTracking()
                            .Where(o => o.OrderDate.Year == 2025)
                            .Select(o => new
                            {
                                o.OrderId,
                                CustomerName = o.Customer!.Name,
                                OrderDate = o.OrderDate,
                                Status = o.Status,

                            })
                            .OrderBy(o => o.OrderDate)
                            .ThenBy(o => o.CustomerName)
                            .ToList();

            Console.WriteLine("\nScenario 27 Results:");
            Console.WriteLine("OrderId | CustomerName         | OrderDate   | Status");
            Console.WriteLine("--------|----------------------|-------------|---------");
            foreach (var r in result) // or result2
            {
                Console.WriteLine($"{r.OrderId,7} | {r.CustomerName,-20} | {r.OrderDate:yyyy-MM-dd} | {r.Status}");
            }




        }


        /// <summary>
        /// Scenario 28 (Employees + Departments — active employees):
        /// List all employees who are Active (IsActive == true),
        /// along with their Department Name.
        /// Return: EmployeeId, FullName, DepartmentName, JoinDate.
        /// Sort by DepartmentName ascending, then FullName ascending.
        /// Provide BOTH:
        ///   Option 1 = explicit Join (Employees ↔ Departments on DepartmentId)
        ///   Option 2 = navigation property (e.Department.Name)
        /// </summary>
        public void Scenario28()
        {
            // TODO: Write your LINQ query here (both options)

            // Option 1 = explicit Join (Employees ↔ Departments on DepartmentId)
            var result = _db.Employees.AsNoTracking()
                            .Where(e => e.IsActive)
                            .Join(_db.Departments,
                                e => e.DepartmentId,
                                d => d.DepartmentId,
                                (e, d) => new
                                {
                                    e.EmployeeId,
                                    e.FullName,
                                    DepartmentName = d.Name,
                                    e.JoinDate
                                })
                            .OrderBy(d => d.DepartmentName)
                            .ThenBy(e => e.FullName)
                            .ToList();

            //   Option 2 = navigation property (e.Department.Name)

            var result2 = _db.Employees.AsNoTracking()
                            .Where(e => e.IsActive)
                            .Select(e => new
                            {

                                e.EmployeeId,
                                e.FullName,
                                DepartmentName = e.Department!.Name,
                                e.JoinDate
                            })
                            .OrderBy(d => d.DepartmentName)
                            .ThenBy(e => e.FullName)
                            .ToList();


            Console.WriteLine("\nScenario 28 Results:");
            Console.WriteLine("EmployeeId | FullName               | DepartmentName      | JoinDate");
            Console.WriteLine("-----------|------------------------|---------------------|----------");
            foreach (var r in result) // or result2
            {
                Console.WriteLine($"{r.EmployeeId,10} | {r.FullName,-22} | {r.DepartmentName,-19} | {r.JoinDate:yyyy-MM-dd}");
            }
        }


        /// <summary>
        /// Scenario 29 (Customers + Orders — Indian customers with 2025 orders):
        /// List all orders placed in 2025 by customers whose Country = "India".
        /// Return: OrderId, CustomerName, Country, OrderDate, Status.
        /// Sort by CustomerName ascending, then OrderDate ascending.
        /// Provide BOTH:
        ///   Option 1 = explicit Join (Customers ↔ Orders on CustomerId)
        ///   Option 2 = navigation property (o.Customer.Name, o.Customer.Country)
        /// </summary>
        public void Scenario29()
        {
            // TODO: Write your LINQ query here (both options)

            // Option 1 = explicit Join (Customers ↔ Orders on CustomerId)

            var start = new DateTime(2025, 1, 1);
            var end = new DateTime(2026, 1, 1);


            var result = _db.Orders.AsNoTracking()
                            .Where(o => o.OrderDate >= start && o.OrderDate < end)
                            .Join(_db.Customers,
                                o => o.CustomerId,
                                c => c.CustomerId,
                                (o, c) => new { o, c })
                            .Where(x => x.c.Country == "India")
                            .Select
                                (x => new
                                {
                                    x.o.OrderId,
                                    CustomerName = x.c.Name,
                                    x.c.Country,
                                    x.o.OrderDate,
                                    x.o.Status
                                })

                            .OrderBy(c => c.CustomerName)
                            .ThenBy(o => o.OrderDate)
                            .ToList();


            // Option 2 = navigation property (o.Customer.Name, o.Customer.Country)

            var result2 = _db.Orders.AsNoTracking()
                            .Where(o => o.OrderDate >= start && o.OrderDate < end && o.Customer!.Country == "India")
                            .Select(o => new
                            {

                                o.OrderId,
                                CustomerName = o.Customer!.Name,
                                Country = o.Customer!.Country,
                                o.OrderDate,
                                o.Status

                            })
                            .OrderBy(c => c.CustomerName)
                            .ThenBy(o => o.OrderDate)
                            .ToList();

            Console.WriteLine("\nScenario 29 Results:");
            Console.WriteLine("OrderId | CustomerName         | Country | OrderDate   | Status");
            Console.WriteLine("--------|----------------------|---------|-------------|---------");
            foreach (var r in result) // or result2
            {
                Console.WriteLine($"{r.OrderId,7} | {r.CustomerName,-20} | {r.Country,-7} | {r.OrderDate:yyyy-MM-dd} | {r.Status}");
            }
        }

        /// <summary>
        /// Scenario 30 (OrderItems + Products — 2025 items with product info):
        /// List all OrderItems from orders placed in 2025,
        /// along with the Product Name and UnitPrice.
        /// Return: OrderId, ProductId, ProductName, Quantity, UnitPrice, TotalPrice.
        /// Sort by OrderId ascending, then ProductName ascending.
        /// Provide BOTH:
        ///   Option 1 = explicit Join (OrderItems ↔ Products on ProductId)
        ///   Option 2 = navigation property (oi.Product.Name, oi.Product.Price)
        /// </summary>
        public void Scenario30()
        {
            // TODO: Write your LINQ query here (both options)

            //Option 1 = explicit Join (OrderItems ↔ Products on ProductId)

            var start = new DateTime(2025, 1, 1);
            var end = new DateTime(2026, 1, 1);


            var result = _db.OrderItems.AsNoTracking()
                            .Where(oi => oi.Order!.OrderDate >= start && oi.Order!.OrderDate < end)
                            .Join(_db.Products,
                                ot => ot.ProductId,
                                p => p.ProductId,
                                (ot, p) => new { ot, p })
                            .Select(x => new
                            {

                                x.ot.OrderId,
                                x.p.ProductId,
                                ProductName = x.p.Name,
                                x.ot.Quantity,
                                x.ot.UnitPrice,
                                TotalPrice = x.ot.UnitPrice * x.ot.Quantity
                            })
                            .OrderBy(ot => ot.OrderId)
                            .ThenBy(p => p.ProductName)
                            .ToList();

            //Option 2 = navigation property (oi.Product.Name, oi.Product.Price)

            var result2 = _db.OrderItems.AsNoTracking()
                            .Where(oi => oi.Order!.OrderDate >= start && oi.Order!.OrderDate < end)
                            .Select(x => new
                            {

                                x.OrderId,
                                x.Product!.ProductId,
                                ProductName = x.Product!.Name,
                                x.Quantity,
                                x.UnitPrice,
                                TotalPrice = x.UnitPrice * x.Quantity
                            })
                            .OrderBy(ot => ot.OrderId)
                            .ThenBy(p => p.ProductName)
                            .ToList();

            Console.WriteLine("\nScenario 30 Results:");
            Console.WriteLine("OrderId | ProductId | ProductName           | Qty | UnitPrice | TotalPrice");
            Console.WriteLine("--------|-----------|-----------------------|-----|-----------|-----------");
            foreach (var r in result) // or result2
            {
                Console.WriteLine($"{r.OrderId,7} | {r.ProductId,9} | {r.ProductName,-21} | {r.Quantity,3} | {r.UnitPrice,9:0.00} | {r.TotalPrice,9:0.00}");
            }



        }

        /// <summary>
        /// Scenario 31 (Orders + Customers — status filter):
        /// List all "Delivered" orders with the customer's name.
        /// Return: OrderId, CustomerName, OrderDate, Status.
        /// Sort by CustomerName asc, then OrderDate desc.
        /// Provide BOTH: explicit Join and navigation property.
        /// </summary>
        public void Scenario31()
        {
            // TODO: Write your LINQ query here (both options)

            // Option 1 = explicit Join 

            var result = _db.Orders.AsNoTracking()
                            .Where(o => o.Status == "Delivered")
                            .Join(_db.Customers,
                                   o => o.CustomerId,
                                   c => c.CustomerId,
                                   (o, c) => new { o, c })
                            .Select(x => new
                            {

                                x.o.OrderId,
                                CustomerName = x.c.Name,
                                x.o.OrderDate,
                                x.o.Status
                            })
                            .OrderBy(c => c.CustomerName)
                            .ThenByDescending(c => c.OrderDate)
                            .ToList();


            // Option 2 = navigation property

            var result2 = _db.Orders.AsNoTracking()
                            .Where(o => o.Status == "Delivered")
                            .Select(o => new
                            {

                                o.OrderId,
                                CustomerName = o.Customer!.Name,
                                o.OrderDate,
                                o.Status
                            })
                            .OrderBy(c => c.CustomerName)
                            .ThenByDescending(o => o.OrderDate)
                            .ToList();


            Console.WriteLine("\nScenario 31 Results:");
            Console.WriteLine("OrderId | CustomerName         | OrderDate           | Status");
            Console.WriteLine("--------|----------------------|---------------------|--------");
            foreach (var r in result) // or result2
            {
                Console.WriteLine($"{r.OrderId,7} | {r.CustomerName,-20} | {r.OrderDate:yyyy-MM-dd HH:mm} | {r.Status}");
            }

        }


        /// <summary>
        /// Scenario 32 (OrderItems + Products — 2025 totals per product, with grouping):
        /// Using OrderItems and Products:
        /// 1) Consider only items whose parent Order was placed in 2025.
        /// 2) Group by ProductId.
        /// 3) For each product, return:
        ///    - ProductId
        ///    - ProductName
        ///    - TotalQtySold (sum of Quantity)
        ///    - TotalRevenue (sum of UnitPrice * Quantity)
        /// 4) Sort by TotalQtySold descending, then ProductName ascending.
        /// Provide BOTH:
        ///   Option 1 = explicit Join (OrderItems ↔ Products on ProductId, and filter by oi.Order.OrderDate)
        ///   Option 2 = navigation property (oi.Product.Name, oi.Order.OrderDate)
        /// </summary>
        public void Scenario32()
        {
            // TODO: Write BOTH Option 1 (Join) and Option 2 (Navigation) queries here
            // Option 1 = explicit Join 

            var start = new DateTime(2025, 1, 1);
            var end = new DateTime(2026, 1, 1);

            var result = _db.OrderItems.AsNoTracking()
                            .Where(oi => oi.Order!.OrderDate >= start && oi.Order!.OrderDate < end)
                            .Join(_db.Products,
                                 oi => oi.ProductId,
                                 p => p.ProductId,
                                 (oi, p) => new { oi, p })
                            .GroupBy(x => new { x.p.ProductId, x.p.Name })
                            .Select(g => new
                            {

                                g.Key.ProductId,
                                ProductName = g.Key.Name,
                                TotalQtySold = g.Sum(x => x.oi.Quantity),
                                TotalRevenue = g.Sum(x => x.oi.UnitPrice * x.oi.Quantity)
                            })
                            .OrderByDescending(x => x.TotalQtySold)
                            .ThenBy(p => p.ProductName)
                            .ToList();


            // Option 2 = navigation property

            var result2 = _db.OrderItems.AsNoTracking()
                            .Where(oi => oi.Order!.OrderDate >= start && oi.Order!.OrderDate < end)
                            .GroupBy(x => new { x.Product!.ProductId, x.Product!.Name })
                            .Select(g => new
                            {

                                g.Key.ProductId,
                                ProductName = g.Key.Name,
                                TotalQtySold = g.Sum(x => x.Quantity),
                                TotalRevenue = g.Sum(x => x.UnitPrice * x.Quantity)
                            })
                            .OrderByDescending(x => x.TotalQtySold)
                            .ThenBy(p => p.ProductName)
                            .ToList();

            Console.WriteLine("\nScenario 32 Results:");
            Console.WriteLine("ProductId | ProductName           | TotalQty | TotalRevenue");
            Console.WriteLine("----------|-----------------------|---------:|------------:");
            foreach (var r in result) // or result2
            {
                Console.WriteLine($"{r.ProductId,9} | {r.ProductName,-21} | {r.TotalQtySold,8} | {r.TotalRevenue,12:0.00}");
            }

        }

        /// <summary>
        /// Scenario 33 (Employees + Departments — active headcount per department):
        /// Using Employees and Departments:
        /// 1) Consider only employees where IsActive == true.
        /// 2) Group by DepartmentId (and include Department Name).
        /// 3) For each group, return:
        ///    - DepartmentId
        ///    - DepartmentName
        ///    - ActiveCount (number of active employees)
        /// 4) Sort by ActiveCount descending, then DepartmentName ascending.
        /// Provide BOTH:
        ///   Option 1 = explicit Join (Employees ↔ Departments on DepartmentId)
        ///   Option 2 = navigation property (e.Department.Name)
        /// </summary>
        public void Scenario33()
        {
            // TODO: Write BOTH Option 1 (Join) and Option 2 (Navigation) queries here

            // Option 1 = explicit Join 

            var result = _db.Employees.AsNoTracking()
                            .Where(e => e.IsActive)
                            .Join(_db.Departments,
                                    e => e.DepartmentId,
                                    d => d.DepartmentId,
                                    (e, d) => new { e, d })
                            .GroupBy(x => new { x.d.DepartmentId, x.d.Name })
                            .Select(g => new
                            {

                                g.Key.DepartmentId,
                                DepartmentName = g.Key.Name,
                                ActiveCount = g.Count()
                            })
                            .OrderByDescending(x => x.ActiveCount)
                            .ThenBy(d => d.DepartmentName)
                            .ToList();




            // Option 2 = navigation property

            var result2 = _db.Employees.AsNoTracking()
                            .Where(e => e.IsActive)
                            .GroupBy(x => new { x.Department!.DepartmentId, x.Department!.Name })
                            .Select(g => new
                            {

                                g.Key.DepartmentId,
                                DepartmentName = g.Key.Name,
                                ActiveCount = g.Count()
                            })
                            .OrderByDescending(x => x.ActiveCount)
                            .ThenBy(d => d.DepartmentName)
                            .ToList();

            Console.WriteLine("\nScenario 33 Results:");
            Console.WriteLine("DepartmentId | DepartmentName        | ActiveCount");
            Console.WriteLine("-------------|-----------------------|------------");
            foreach (var r in result) // or result2
            {
                Console.WriteLine($"{r.DepartmentId,12} | {r.DepartmentName,-21} | {r.ActiveCount,11}");
            }


        }

        /// <summary>
        /// Scenario 34 (Orders + Customers — 2025 orders per country):
        /// Using Orders and Customers:
        /// 1) Consider orders placed in 2025.
        /// 2) Group by Country (from Customers).
        /// 3) For each group, return:
        ///    - Country
        ///    - OrderCount (number of orders in 2025)
        /// 4) Sort by OrderCount descending, then Country ascending.
        /// Provide BOTH:
        ///   Option 1 = explicit Join (Orders ↔ Customers on CustomerId)
        ///   Option 2 = navigation property (o.Customer.Country)
        /// </summary>
        public void Scenario34()
        {
            // TODO: Write BOTH Option 1 (Join) and Option 2 (Navigation) queries here

            //Option 1 (Join)
            var start = new DateTime(2025, 1, 1);
            var end = new DateTime(2026, 1, 1);

            var result = _db.Orders.AsNoTracking()
                            .Where(o => o.OrderDate >= start && o.OrderDate < end)
                            .Join(_db.Customers,
                                o => o.CustomerId,
                                c => c.CustomerId,
                                (o, c) => new { o, c }
                            )
                            .GroupBy(x => x.c.Country)
                            .Select(g => new
                            {

                                Country = g.Key,
                                OrderCount = g.Count()
                            })
                            .OrderByDescending(o => o.OrderCount)
                            .ThenBy(c => c.Country)
                            .ToList();

            // Option 2 = navigation property

            var result2 = _db.Orders.AsNoTracking()
                           .Where(o => o.OrderDate >= start && o.OrderDate < end)
                           .GroupBy(x => x.Customer!.Country)
                           .Select(g => new
                           {

                               Country = g.Key,
                               OrderCount = g.Count()
                           })
                            .OrderByDescending(o => o.OrderCount)
                            .ThenBy(c => c.Country)
                            .ToList();

            Console.WriteLine("\nScenario 34 Results:");
            Console.WriteLine("Country        | OrderCount");
            Console.WriteLine("----------------|-----------");
            foreach (var r in result) // or result2
            {
                Console.WriteLine($"{r.Country,-15} | {r.OrderCount,10}");
            }


        }


        /// <summary>
        /// Scenario 35 (Products + Categories — average price per category):
        /// Using Products and Categories:
        /// 1) Group products by Category.
        /// 2) For each group, return:
        ///    - CategoryId
        ///    - CategoryName
        ///    - ProductCount (number of products in the category)
        ///    - AvgPrice (average product price)
        /// 3) Only include categories where ProductCount >= 2 (HAVING condition).
        /// 4) Sort by AvgPrice descending, then CategoryName ascending.
        /// Provide BOTH:
        ///   Option 1 = explicit Join (Products ↔ Categories on CategoryId)
        ///   Option 2 = navigation property (p.Category.Name)
        /// </summary>
        public void Scenario35()
        {
            // TODO: Write BOTH Option 1 (Join) and Option 2 (Navigation) queries here


            // Option 1 = explicit Join (Products ↔ Categories on CategoryId)
            var result = _db.Products.AsNoTracking()
                            .Join(_db.Categories,
                                p => p.CategoryId,
                                c => c.CategoryId,
                                (p, c) => new { p, c })
                            .GroupBy(x => new { x.c.CategoryId, x.c.Name })
                            .Select(g => new
                            {

                                CategoryId = g.Key.CategoryId,
                                CategoryName = g.Key.Name,
                                ProductCount = g.Count(),
                                AvgPrice = g.Average(x => x.p.Price)
                            })
                            .Where(p => p.ProductCount >= 2)
                            .OrderByDescending(p => p.AvgPrice)
                            .ThenBy(c => c.CategoryName)
                            .ToList();


            //Option 2 = navigation property (p.Category.Name)

            var result2 = _db.Products.AsNoTracking()
                           .GroupBy(x => new { x.Category!.CategoryId, x.Category!.Name })
                           .Select(g => new
                           {
                               CategoryId = g.Key.CategoryId,
                               CategoryName = g.Key.Name,
                               ProductCount = g.Count(),
                               AvgPrice = g.Average(x => x.Price)
                           })
                            .Where(p => p.ProductCount >= 2)
                            .OrderByDescending(p => p.AvgPrice)
                            .ThenBy(c => c.CategoryName)
                            .ToList();

            Console.WriteLine("\nScenario 35 Results:");
            Console.WriteLine("CategoryId | CategoryName          | ProductCount | AvgPrice");
            Console.WriteLine("-----------|-----------------------|-------------:|---------:");
            foreach (var r in result) // or result2
            {
                Console.WriteLine($"{r.CategoryId,10} | {r.CategoryName,-21} | {r.ProductCount,12} | {r.AvgPrice,8:0.00}");
            }





        }


        /// <summary>
        /// Scenario 36 (Orders + Customers — 2025 order stats per customer):
        /// Using Orders and Customers:
        /// 1) Consider orders placed in 2025.
        /// 2) Group by CustomerId (and include Customer Name).
        /// 3) For each group, return:
        ///    - CustomerId
        ///    - CustomerName
        ///    - OrderCount (number of orders)
        ///    - TotalBill (sum of TotalBill)
        ///    - AvgBill (average TotalBill)
        /// 4) Only include customers where OrderCount >= 2 (HAVING).
        /// 5) Sort by TotalBill descending, then CustomerName ascending.
        /// Provide BOTH:
        ///   Option 1 = explicit Join (Orders ↔ Customers on CustomerId)
        ///   Option 2 = navigation property (o.Customer.Name)
        /// </summary>
        public void Scenario36()
        {
            // TODO: Write BOTH Option 1 (Join) and Option 2 (Navigation) queries here

            //Option 1 (Join)
            var start = new DateTime(2025, 1, 1);
            var end = new DateTime(2026, 1, 1);


            var result = _db.Orders.AsNoTracking()
                            .Where(o => o.OrderDate >= start && o.OrderDate < end)
                            .Join(_db.Customers,
                                o => o.CustomerId,
                                c => c.CustomerId,
                                (o, c) => new { o, c }
                            )
                            .GroupBy(x => new { x.c.CustomerId, x.c.Name })
                            .Select(g => new {

                                g.Key.CustomerId,
                                CustomerName = g.Key.Name,
                                OrderCount = g.Count(),
                                TotalBill = g.Sum(x => x.o.TotalBill),
                                AvgBill = g.Average(x => x.o.TotalBill)
                            })
                            .Where(c => c.OrderCount >= 2)
                            .OrderByDescending(o => o.TotalBill)
                            .ThenBy(c => c.CustomerName)
                            .ToList();

            //Option 2 = navigation property

            var result2 = _db.Orders.AsNoTracking()
                            .Where(o => o.OrderDate >= start && o.OrderDate < end)
                            .GroupBy(x => new { x.CustomerId, x.Customer!.Name })
                            .Select(g => new {

                                g.Key.CustomerId,
                                CustomerName = g.Key.Name,
                                OrderCount = g.Count(),
                                TotalBill = g.Sum(x => x.TotalBill),
                                AvgBill = g.Average(x => x.TotalBill)
                            })
                            .Where(c => c.OrderCount >= 2)
                            .OrderByDescending(o => o.TotalBill)
                            .ThenBy(c => c.CustomerName)
                            .ToList();

            Console.WriteLine("\nScenario 36 Results:");
            Console.WriteLine("CustomerId | CustomerName         | OrderCount | TotalBill  | AvgBill");
            Console.WriteLine("-----------|----------------------|-----------:|-----------:|--------:");
            foreach (var r in result) // or result2
            {
                Console.WriteLine($"{r.CustomerId,10} | {r.CustomerName,-20} | {r.OrderCount,10} | {r.TotalBill,10:0.00} | {r.AvgBill,7:0.00}");
            }



        }

        /// <summary>
        /// Scenario 37 (Orders + Customers — 2025 orders per customer + latest date):
        /// Using Orders and Customers:
        /// 1) Consider orders placed in 2025.
        /// 2) Group by Customer (CustomerId + Name).
        /// 3) For each group, return:
        ///    - CustomerId
        ///    - CustomerName
        ///    - OrderCount
        ///    - LastOrderDate (max OrderDate)
        /// 4) Keep only customers where OrderCount >= 2 (HAVING).
        /// 5) Sort by OrderCount descending, then CustomerName ascending.
        /// Provide BOTH:
        ///   Option 1 = explicit Join (Orders ↔ Customers on CustomerId)
        ///   Option 2 = navigation property (o.Customer.Name)
        /// </summary>
        public void Scenario37()
        {
            // TODO: Write BOTH Option 1 (Join) and Option 2 (Navigation) queries here

            
            var start = new DateTime(2025, 1, 1);
            var end = new DateTime(2026, 1, 1);


            //Option 1 (Join)
            var result = _db.Orders.AsNoTracking()
                            .Where(o => o.OrderDate >= start && o.OrderDate < end)
                            .Join(_db.Customers,
                                o => o.CustomerId,
                                c => c.CustomerId,
                                (o, c) => new { o, c })
                            .GroupBy(x => new { x.c.CustomerId, x.c.Name })
                            .Where(g => g.Count() >= 2)
                            .Select(g => new
                            {

                                g.Key.CustomerId,
                                CustomerName = g.Key.Name,
                                OrderCount = g.Count(),
                                LastOrderDate = g.Max(x => x.o.OrderDate)
                            })                            
                            .OrderByDescending(x => x.OrderCount)
                            .ThenBy(c => c.CustomerName)
                            .ToList();

            //Option 2 = navigation property

            var result2 = _db.Orders.AsNoTracking()
                            .Where(o => o.OrderDate >= start && o.OrderDate < end)
                            .GroupBy(x => new { x.CustomerId, x.Customer!.Name })
                            .Where(g => g.Count() >= 2)
                            .Select(g => new
                            {

                                g.Key.CustomerId,
                                CustomerName = g.Key.Name,
                                OrderCount = g.Count(),
                                LastOrderDate = g.Max(x => x.OrderDate)
                            })
                            
                            .OrderByDescending(x => x.OrderCount)
                            .ThenBy(c => c.CustomerName)
                            .ToList();

            Console.WriteLine("\nScenario 37 Results:");
            Console.WriteLine("CustomerId | CustomerName         | OrderCount | LastOrderDate");
            Console.WriteLine("-----------|----------------------|-----------:|---------------");
            foreach (var r in result) // or result2
            {
                Console.WriteLine($"{r.CustomerId,10} | {r.CustomerName,-20} | {r.OrderCount,10} | {r.LastOrderDate:yyyy-MM-dd}");
            }



        }

        /// <summary>
        /// Scenario 38 (Employees + Departments — average salary per department):
        /// Using Employees and Departments:
        /// 1) Consider only employees where IsActive == true.
        /// 2) Group by Department (DepartmentId + Name).
        /// 3) For each group, return:
        ///    - DepartmentId
        ///    - DepartmentName
        ///    - ActiveCount  (number of active employees)
        ///    - AvgSalary    (average Salary of active employees)
        /// 4) Keep only departments where ActiveCount >= 2 (HAVING).
        /// 5) Sort by AvgSalary descending, then DepartmentName ascending.
        /// Provide BOTH:
        ///   Option 1 = explicit Join (Employees ↔ Departments on DepartmentId)
        ///   Option 2 = navigation property (e.Department.Name)
        /// </summary>
        public void Scenario38()
        {
            // TODO: Write BOTH Option 1 (Join) and Option 2 (Navigation) queries here

            //Option 1 (Join)

            var result = _db.Employees.AsNoTracking()
                            .Where(e => e.IsActive)
                            .Join(_db.Departments,
                                    e => e.DepartmentId,
                                    d => d.DepartmentId,
                                    (e, d) => new { e, d })
                            .GroupBy(g => new { g.d.DepartmentId, g.d.Name })
                            .Where(g => g.Count() >= 2)
                            .Select(g => new
                            {

                                g.Key.DepartmentId,
                                DepartmentName = g.Key.Name,
                                ActiveCount = g.Count(),
                                AvgSalary = g.Average(x => x.e.Salary)
                            })
                            
                            .OrderByDescending(x => x.AvgSalary)
                            .ThenBy(x => x.DepartmentName)
                            .ToList();

            //Option 2 (Navigation)
            var result2 = _db.Employees.AsNoTracking()
                            .Where(e => e.IsActive)
                            .GroupBy(g => new { g.DepartmentId, g.Department!.Name })
                            .Where(g => g.Count() >= 2)
                            .Select(g => new
                            {

                                g.Key.DepartmentId,
                                DepartmentName = g.Key.Name,
                                ActiveCount = g.Count(),
                                AvgSalary = g.Average(x => x.Salary)
                            })
                            .OrderByDescending(x => x.AvgSalary)
                            .ThenBy(x => x.DepartmentName)
                            .ToList();

            Console.WriteLine("\nScenario 38 Results:");
            Console.WriteLine("DepartmentId | DepartmentName        | ActiveCount | AvgSalary");
            Console.WriteLine("-------------|-----------------------|------------:|---------:");
            foreach (var r in result) // or result2
            {
                Console.WriteLine($"{r.DepartmentId,12} | {r.DepartmentName,-21} | {r.ActiveCount,12} | {r.AvgSalary,8:0.00}");
            }



        }

        /// <summary>
        /// Scenario 39 (Employees + Departments — top salary per department with WHERE):
        /// Using Employees and Departments:
        /// 1) WHERE: Only consider employees where IsActive == true AND JoinDate >= 2024-01-01.
        /// 2) Group by Department (DepartmentId + Name).
        /// 3) From each group, return the SINGLE employee with the HIGHEST Salary
        ///    (tie-breakers: latest JoinDate, then lowest EmployeeId).
        /// 4) Return: DepartmentId, DepartmentName, EmployeeId, FullName, Salary, JoinDate.
        /// 5) Sort final results by DepartmentName ascending, then Salary descending.
        /// Provide BOTH:
        ///   Option 1 = explicit Join (Employees ↔ Departments on DepartmentId)
        ///   Option 2 = navigation property (e.Department.Name)
        /// </summary>
        public void Scenario39()
        {
            // TODO: Write BOTH Option 1 (Join) and Option 2 (Navigation) queries here

            // Option 1 (Join)
            var result = _db.Employees.AsNoTracking()
                            .Where(x => x.IsActive && x.JoinDate >= new DateTime(2024, 1, 1))
                           
                            .Join(_db.Departments,
                                    e => e.DepartmentId,
                                    d => d.DepartmentId,
                                    (e, d) => new { e, d })
                            .GroupBy(g => new { g.d.DepartmentId, g.d.Name })
                            .ToList()
                            .SelectMany(g => g
                               .OrderByDescending(g => g.e.Salary)
                               .ThenByDescending(g => g.e.JoinDate)
                               .ThenBy(g => g.e.EmployeeId)
                               .Take(1)
                               .Select(x => new
                               {

                                   x.d.DepartmentId,
                                   DepartmentName = x.d.Name,
                                   x.e.EmployeeId,
                                   x.e.FullName,
                                   x.e.Salary,
                                   x.e.JoinDate
                               }))
                            .OrderBy(x => x.DepartmentName)
                            .ThenByDescending(x => x.Salary)
                            .ToList();

            //Option 2 (Navigation)
            var result2 = _db.Employees.AsNoTracking()
                .Where(x => x.IsActive && x.JoinDate >= new DateTime(2024, 1, 1))                
                .GroupBy(g => new { g.DepartmentId, g.Department!.Name })
                .ToList()
                            .SelectMany(g => g
                               .OrderByDescending(g => g.Salary)
                               .ThenByDescending(g => g.JoinDate)
                               .ThenBy(g => g.EmployeeId)
                               .Take(1)
                               .Select(x => new
                               {

                                   x.DepartmentId,
                                   DepartmentName = x.Department!.Name,
                                   x.EmployeeId,
                                   x.FullName,
                                   x.Salary,
                                   x.JoinDate
                               }))
                            .OrderBy(x => x.DepartmentName)
                            .ThenByDescending(x => x.Salary)
                            .ToList();


            Console.WriteLine("\nScenario 39 Results:");
            Console.WriteLine("DeptId | DepartmentName        | EmployeeId | FullName              | Salary    | JoinDate");
            Console.WriteLine("-------|-----------------------|------------|-----------------------|-----------|----------");
            foreach (var r in result) // or result2
            {
                Console.WriteLine($"{r.DepartmentId,6} | {r.DepartmentName,-21} | {r.EmployeeId,10} | {r.FullName,-21} | {r.Salary,9:0.00} | {r.JoinDate:yyyy-MM-dd}");
            }

        }


        /// <summary>
        /// Scenario 40 (Orders + Customers — 2025 orders per country with revenue):
        /// Using Orders and Customers:
        /// 1) Consider only orders placed in 2025.
        /// 2) Group by Country (from Customers).
        /// 3) For each group, return:
        ///    - Country
        ///    - OrderCount (number of orders)
        ///    - TotalRevenue (sum of TotalBill)
        ///    - AvgRevenue   (average TotalBill)
        /// 4) Keep only countries where OrderCount >= 2 (HAVING).
        /// 5) Sort by TotalRevenue descending, then Country ascending.
        /// Provide BOTH:
        ///   Option 1 = explicit Join (Orders ↔ Customers on CustomerId)
        ///   Option 2 = navigation property (o.Customer.Country)
        /// </summary>
        public void Scenario40()
        {
            // TODO: Write BOTH Option 1 (Join) and Option 2 (Navigation) queries here


            var start = new DateTime(2025, 1, 1);
            var end = new DateTime(2026, 1, 1);

            //Option 1 (Join)
            var result = _db.Orders.AsNoTracking()
                            .Where(o => o.OrderDate >= start && o.OrderDate < end)
                            .Join(_db.Customers,
                                   o => o.CustomerId,
                                   c => c.CustomerId,
                                   (o, c) => new { o, c })
                            .GroupBy(g => g.c.Country)
                            .Where(g => g.Count() >= 2)
                            .Select(g => new
                            {
                                Country = g.Key,
                                OrderCount = g.Count(),
                                TotalRevenue = g.Sum(x => x.o.TotalBill),
                                AvgRevenue = g.Average(x => x.o.TotalBill)
                            })
                            .OrderByDescending(x => x.TotalRevenue)
                            .ThenBy(x => x.Country)
                            .ToList();

            //Option 2 (Navigation)

            var result2 = _db.Orders.AsNoTracking()
                            .Where(o => o.OrderDate >= start && o.OrderDate < end)
                            .GroupBy(g => g.Customer!.Country)
                            .Where(g => g.Count() >= 2)
                            .Select(g => new
                            {
                                Country = g.Key,
                                OrderCount = g.Count(),
                                TotalRevenue = g.Sum(x => x.TotalBill),
                                AvgRevenue = g.Average(x => x.TotalBill)
                            })
                            .OrderByDescending(x => x.TotalRevenue)
                            .ThenBy(x => x.Country)
                            .ToList();

            Console.WriteLine("\nScenario 40 Results:");
            Console.WriteLine("Country         | OrderCount | TotalRevenue | AvgRevenue");
            Console.WriteLine("----------------|-----------:|-------------:|----------:");
            foreach (var r in result) // or result2
            {
                Console.WriteLine($"{r.Country,-15} | {r.OrderCount,10} | {r.TotalRevenue,12:0.00} | {r.AvgRevenue,9:0.00}");
            }



        }

        /// <summary>
        /// Scenario 41 (Products + Categories — 500+ price products per category):
        /// Using Products and Categories:
        /// 1) WHERE: Consider only products with Price >= 500.
        /// 2) GROUP BY: Category (CategoryId + Name).
        /// 3) For each group, return:
        ///    - CategoryId
        ///    - CategoryName
        ///    - ProductCount (items meeting the filter)
        ///    - AvgPrice     (average Price of those items)
        /// 4) HAVING: Keep only categories where ProductCount >= 2.
        /// 5) ORDER BY: ProductCount desc, then CategoryName asc.
        /// Provide BOTH:
        ///   Option 1 = explicit Join (Products ↔ Categories)
        ///   Option 2 = navigation property (p.Category.Name)
        /// </summary>
        public void Scenario41()
        {
            // TODO: Write BOTH Option 1 (Join) and Option 2 (Navigation) queries here

            // Option 1(Join)

            var result = _db.Products.AsNoTracking()
                            .Where(p => p.Price >= 500)
                            .Join(_db.Categories,
                                   p => p.CategoryId,
                                   c => c.CategoryId,
                                   (p, c) => new { p, c })
                            .GroupBy(g => new { g.c.CategoryId, g.c.Name })
                            .Where(g => g.Count() >= 2)
                            .Select(g => new
                            {

                                g.Key.CategoryId,
                                CategoryName = g.Key.Name,
                                ProductCount = g.Count(),
                                AvgPrice = g.Average(x => x.p.Price)
                            })
                            .OrderByDescending(p => p.ProductCount)
                            .ThenBy(c => c.CategoryName)
                            .ToList();


            //Option 2 (Navigation)


            var result2 = _db.Products.AsNoTracking()
                            .Where(p => p.Price >= 500)
                            .GroupBy(g => new { g.Category!.CategoryId, g.Category!.Name })
                            .Where(g => g.Count() >= 2)
                            .Select(g => new
                            {

                                g.Key.CategoryId,
                                CategoryName = g.Key.Name,
                                ProductCount = g.Count(),
                                AvgPrice = g.Average(x => x.Price)
                            })
                            .OrderByDescending(p => p.ProductCount)
                            .ThenBy(c => c.CategoryName)
                            .ToList();

            Console.WriteLine("\nScenario 41 Results:");
            Console.WriteLine("CategoryId | CategoryName          | ProductCount | AvgPrice");
            Console.WriteLine("-----------|-----------------------|-------------:|---------:");
            foreach (var r in result) // or result2
            {
                Console.WriteLine($"{r.CategoryId,10} | {r.CategoryName,-21} | {r.ProductCount,12} | {r.AvgPrice,8:0.00}");
            }

        }

        /// <summary>
        /// Scenario 42 (Orders + Customers — 2025 stats per customer):
        /// Using Orders and Customers:
        /// 1) WHERE: Only consider orders placed in 2025.
        /// 2) GROUP BY: CustomerId + CustomerName.
        /// 3) SELECT: CustomerId, CustomerName, OrderCount, TotalRevenue (Sum TotalBill), AvgRevenue.
        /// 4) HAVING: Only customers where OrderCount >= 2.
        /// 5) ORDER BY: TotalRevenue DESC, then CustomerName ASC.
        /// Provide BOTH:
        ///   Option 1 = explicit Join (Orders ↔ Customers on CustomerId)
        ///   Option 2 = navigation property (o.Customer.Name)
        /// </summary>
        public void Scenario42()
        {
            // TODO: Write BOTH Option 1 (Join) and Option 2 (Navigation) queries here
            
            var start = new DateTime(2025, 1, 1);
            var end = new DateTime(2026, 1, 1);

            //Option 1 (Join)

            var result = _db.Orders.AsNoTracking()
                            .Where(o => o.OrderDate >= start && o.OrderDate < end)
                            .Join(_db.Customers,
                                   o => o.CustomerId,
                                   c => c.CustomerId,
                                   (o, c) => new { o, c })
                            .GroupBy(g => new { g.c.CustomerId, g.c.Name })
                            .Where(g => g.Count() >= 2)
                            .Select(g => new
                            {

                                g.Key.CustomerId,
                                CustomerName = g.Key.Name,
                                OrderCount = g.Count(),
                                TotalRevenue = g.Sum(x => x.o.TotalBill),
                                AvgRevenue = g.Average(x => x.o.TotalBill)
                            })
                            .OrderByDescending(x => x.TotalRevenue)
                            .ThenBy(c => c.CustomerName)
                            .ToList();


            //Option 2 (Navigation)

            var result2 = _db.Orders.AsNoTracking()
                            .Where(o => o.OrderDate >= start && o.OrderDate < end)
                            .GroupBy(g => new { g.Customer!.CustomerId, g.Customer!.Name })
                            .Where(g => g.Count() >= 2)
                            .Select(g => new
                            {

                                g.Key.CustomerId,
                                CustomerName = g.Key.Name,
                                OrderCount = g.Count(),
                                TotalRevenue = g.Sum(x => x.TotalBill),
                                AvgRevenue = g.Average(x => x.TotalBill)
                            })
                            .OrderByDescending(x => x.TotalRevenue)
                            .ThenBy(c => c.CustomerName)
                            .ToList();


        }


        /// <summary>
        /// Scenario 43 (Employees + Departments — post-2023 active salary stats per department):
        /// Using Employees and Departments:
        /// 1) WHERE: Only employees with IsActive == true AND JoinDate >= 2023-01-01.
        /// 2) GROUP BY: Department (DepartmentId + Name).
        /// 3) SELECT per group:
        ///    - DepartmentId
        ///    - DepartmentName
        ///    - ActiveCount   (number of matching employees)
        ///    - AvgSalary     (average Salary)
        ///    - MaxSalary     (maximum Salary)
        /// 4) HAVING: Keep only departments where ActiveCount >= 2.
        /// 5) ORDER BY: MaxSalary DESC, then DepartmentName ASC.
        /// Provide BOTH inside this method:
        ///   Option 1 = explicit Join (Employees ↔ Departments on DepartmentId)
        ///   Option 2 = navigation property (e.Department.Name)
        /// </summary>
        public void Scenario43()
        {
            // TODO: Write BOTH Option 1 (Join) and Option 2 (Navigation) queries here
            // - Remember: AsNoTracking() for read-only
            // - Filter BEFORE grouping
            // - Apply HAVING with .Where(g => g.Count() >= 2) after GroupBy
            // - Sort by MaxSalary desc, then DepartmentName asc

            // Option 1 (Join)

            var result = _db.Employees.AsNoTracking()
                            .Where(e => e.IsActive && e.JoinDate >= new DateTime(2023, 1, 1))
                            .Join(_db.Departments,
                                  e => e.DepartmentId,
                                  d => d.DepartmentId,
                                  (e, d) => new { e, d })
                            .GroupBy(g => new { g.d.DepartmentId, g.d.Name })
                            .Where(g => g.Count() >= 2)
                            .Select(g => new
                            {
                                g.Key.DepartmentId,
                                DepartmentName = g.Key.Name,
                                ActiveCount = g.Count(),
                                AvgSalary = g.Average(x => x.e.Salary),
                                MaxSalary = g.Max(x => x.e.Salary)
                            })
                            .OrderByDescending(x => x.MaxSalary)
                            .ThenBy(x => x.DepartmentName)
                            .ToList();

            //Option 2 (Navigation)
            var result2 = _db.Employees.AsNoTracking()
                            .Where(e => e.IsActive && e.JoinDate >= new DateTime(2023, 1, 1))
                            .GroupBy(g => new { g.Department!.DepartmentId, g.Department!.Name })
                            .Where(g => g.Count() >= 2)
                            .Select(g => new
                            {
                                g.Key.DepartmentId,
                                DepartmentName = g.Key.Name,
                                ActiveCount = g.Count(),
                                AvgSalary = g.Average(x => x.Salary),
                                MaxSalary = g.Max(x => x.Salary)
                            })
                            .OrderByDescending(x => x.MaxSalary)
                            .ThenBy(x => x.DepartmentName)
                            .ToList();

            Console.WriteLine("\nScenario 43 Results:");
            Console.WriteLine("DepartmentId | DepartmentName        | ActiveCount | AvgSalary | MaxSalary");
            Console.WriteLine("-------------|-----------------------|------------:|----------:|---------:");
            foreach (var r in result) // or result2
            {
                Console.WriteLine($"{r.DepartmentId,12} | {r.DepartmentName,-21} | {r.ActiveCount,12} | {r.AvgSalary,9:0.00} | {r.MaxSalary,9:0.00}");
            }

        }

        /// <summary>
        /// Scenario 44 (Orders + Customers — 2025 revenue stats per country):
        /// Using Orders and Customers:
        /// 1) WHERE: Orders in 2025.
        /// 2) GROUP BY: Country (from Customers).
        /// 3) SELECT:
        ///    - Country
        ///    - OrderCount
        ///    - TotalRevenue (Sum TotalBill)
        ///    - MaxOrderValue (Max TotalBill)
        /// 4) HAVING: OrderCount >= 3.
        /// 5) ORDER BY: TotalRevenue DESC, then Country ASC.
        /// Provide BOTH:
        ///   Option 1 = explicit Join (Orders ↔ Customers)
        ///   Option 2 = navigation property (o.Customer.Country)
        /// </summary>
        public void Scenario44()
        {
            // TODO: Write BOTH Option 1 (Join) and Option 2 (Navigation) queries here

            var start = new DateTime(2025, 1, 1);
            var end = new DateTime(2026, 1, 1);

            //Option 1 (Join)

            var result = _db.Orders.AsNoTracking()
                            .Where(o => o.OrderDate >= start && o.OrderDate < end)
                            .Join(_db.Customers,
                                    o => o.CustomerId,
                                    c => c.CustomerId,
                                    (o, c) => new { o, c })
                            .GroupBy(g => g.c.Country)
                            .Where(g => g.Count() >= 3)
                            .Select(g => new
                            {

                                Country = g.Key,
                                OrderCount = g.Count(),
                                TotalRevenue = g.Sum(x => x.o.TotalBill),
                                MaxOrderValue = g.Max(x => x.o.TotalBill)
                            })
                            .OrderByDescending(x => x.TotalRevenue)
                            .ThenBy(x => x.Country)
                            .ToList();

            //Option 2 (Navigation)

            var result2 = _db.Orders.AsNoTracking()
                           .Where(o => o.OrderDate >= start && o.OrderDate < end)
                           .GroupBy(g => g.Customer!.Country)
                            .Where(g => g.Count() >= 3)
                            .Select(g => new
                            {

                                Country = g.Key,
                                OrderCount = g.Count(),
                                TotalRevenue = g.Sum(x => x.TotalBill),
                                MaxOrderValue = g.Max(x => x.TotalBill)
                            })
                            .OrderByDescending(x => x.TotalRevenue)
                            .ThenBy(x => x.Country)
                            .ToList();

            Console.WriteLine("\nScenario 44 Results:");
            Console.WriteLine("Country         | OrderCount | TotalRevenue | MaxOrderValue");
            Console.WriteLine("----------------|-----------:|-------------:|--------------:");
            foreach (var r in result) // or result2
            {
                Console.WriteLine($"{r.Country,-15} | {r.OrderCount,10} | {r.TotalRevenue,12:0.00} | {r.MaxOrderValue,13:0.00}");
            }



        }

        /// <summary>
        /// Scenario 45 (OrderItems + Products — 2025 sales per category):
        /// Using OrderItems and Products:
        /// 1) WHERE: Only items whose parent Order was placed in 2025 (via oi.Order.OrderDate).
        /// 2) JOIN: OrderItems ↔ Products on ProductId.
        /// 3) GROUP BY: Product.CategoryId + CategoryName (from Products).
        /// 4) SELECT:
        ///    - CategoryId
        ///    - CategoryName
        ///    - TotalQtySold   (Sum oi.Quantity)
        ///    - TotalRevenue   (Sum oi.UnitPrice * oi.Quantity)
        /// 5) HAVING: TotalQtySold >= 5.
        /// 6) ORDER BY: TotalRevenue DESC, then CategoryName ASC.
        /// Provide BOTH:
        ///   Option 1 = explicit Join (OrderItems ↔ Products)
        ///   Option 2 = navigation property (oi.Product.CategoryId / oi.Product.Name).
        /// </summary>
        public void Scenario45()
        {
            // TODO: Write BOTH Option 1 (Join) and Option 2 (Navigation) queries here

            var start = new DateTime(2025, 1, 1);
            var end = new DateTime(2026, 1, 1);
            //Option 1 (Join)

            var result = _db.OrderItems.AsNoTracking()
                            .Where(oi => oi.Order!.OrderDate >= start && oi.Order.OrderDate < end)
                            .Join(_db.Products,
                                oi => oi.ProductId,
                                p => p.ProductId,
                                (oi, p) => new { oi, p })
                            .GroupBy(g => new { g.p.CategoryId, g.p.Category!.Name })
                            .Select(g => new
                            {

                                g.Key.CategoryId,
                                CategoryName = g.Key.Name,
                                TotalQtySold = g.Sum(x => x.oi.Quantity),
                                TotalRevenue = g.Sum(x => x.oi.UnitPrice * x.oi.Quantity)

                            })
                            .Where(x => x.TotalQtySold >= 5)
                            .OrderByDescending(x => x.TotalRevenue)
                            .ThenBy(x => x.CategoryName)
                            .ToList();

            //Option 2 (Navigation)

            var result2 = _db.OrderItems.AsNoTracking()
                            .Where(oi => oi.Order!.OrderDate <= start && oi.Order.OrderDate < end)
                            .GroupBy(g => new { g.Product!.CategoryId, g.Product!.Category!.Name })
                            .Select(g => new
                            {

                                g.Key.CategoryId,
                                CategoryName = g.Key.Name,
                                TotalQtySold = g.Sum(x => x.Quantity),
                                TotalRevenue = g.Sum(x => x.UnitPrice * x.Quantity)

                            })
                            .Where(x => x.TotalQtySold >= 5)
                            .OrderByDescending(x => x.TotalRevenue)
                            .ThenBy(x => x.CategoryName)
                            .ToList();

            Console.WriteLine("\nScenario 45 Results:");
            Console.WriteLine("CategoryId | CategoryName          | TotalQty | TotalRevenue");
            Console.WriteLine("-----------|-----------------------|---------:|------------:");
            foreach (var r in result) // or result2
            {
                Console.WriteLine($"{r.CategoryId,10} | {r.CategoryName,-21} | {r.TotalQtySold,8} | {r.TotalRevenue,12:0.00}");
            }

        }

        /// <summary>
        /// Scenario 46 (Orders + Customers — high-value orders per customer in 2025):
        /// Using Orders and Customers:
        /// 1) WHERE: Only consider orders placed in 2025.
        /// 2) JOIN: Orders ↔ Customers on CustomerId.
        /// 3) GROUP BY: CustomerId + CustomerName.
        /// 4) SELECT per group:
        ///    - CustomerId
        ///    - CustomerName
        ///    - OrderCount
        ///    - MaxOrderValue   (maximum TotalBill)
        ///    - AvgOrderValue   (average TotalBill)
        /// 5) HAVING: Keep only customers where MaxOrderValue >= 5000.
        /// 6) ORDER BY: MaxOrderValue DESC, then CustomerName ASC.
        /// Provide BOTH:
        ///   Option 1 = explicit Join (Orders ↔ Customers)
        ///   Option 2 = navigation property (o.Customer.Name)
        /// </summary>
        public void Scenario46()
        {
            // TODO: Write BOTH Option 1 (Join) and Option 2 (Navigation) queries here

            var start = new DateTime(2025, 1, 1);
            var end = new DateTime(2026, 1, 1);

            //Option 1 (Join)

            var result = _db.Orders.AsNoTracking()
                            .Where(o => o.OrderDate >= start && o.OrderDate < end)
                            .Join(_db.Customers,
                                   o => o.CustomerId,
                                   c => c.CustomerId,
                                   (o, c) => new { o, c })
                            .GroupBy(g => new { g.c.CustomerId, g.c.Name })
                            .Select(g => new {

                                g.Key.CustomerId,
                                CustomerName = g.Key.Name,
                                OrderCount = g.Count(),
                                MaxOrderValue = g.Max(x => x.o.TotalBill),
                                AvgOrderValue = g.Average(x => x.o.TotalBill)
                            })
                            .Where(x => x.MaxOrderValue >= 5000)
                            .OrderByDescending(x => x.MaxOrderValue)
                            .ThenBy(x => x.CustomerName)
                            .ToList();

            //Option 2 (Navigation)

            var result2 = _db.Orders.AsNoTracking()
                            .Where(o => o.OrderDate >= start && o.OrderDate < end)
                            .GroupBy(g => new { g.Customer!.CustomerId, g.Customer!.Name })
                            .Select(g => new {

                                g.Key.CustomerId,
                                CustomerName = g.Key.Name,
                                OrderCount = g.Count(),
                                MaxOrderValue = g.Max(x => x.TotalBill),
                                AvgOrderValue = g.Average(x => x.TotalBill)
                            })
                            .Where(x => x.MaxOrderValue >= 5000)
                            .OrderByDescending(x => x.MaxOrderValue)
                            .ThenBy(x => x.CustomerName)
                            .ToList();

            Console.WriteLine("\nScenario 46 Results:");
            Console.WriteLine("CustomerId | CustomerName         | OrderCount | MaxOrderValue | AvgOrderValue");
            Console.WriteLine("-----------|----------------------|-----------:|--------------:|--------------:");
            foreach (var r in result) // or result2
            {
                Console.WriteLine($"{r.CustomerId,10} | {r.CustomerName,-20} | {r.OrderCount,10} | {r.MaxOrderValue,13:0.00} | {r.AvgOrderValue,13:0.00}");
            }


        }


        /// <summary>
        /// Scenario 47 (Orders + Customers — average spenders per country in 2025):
        /// Using Orders and Customers:
        /// 1) WHERE: Only consider orders placed in 2025.
        /// 2) JOIN: Orders ↔ Customers on CustomerId.
        /// 3) GROUP BY: Country (from Customers).
        /// 4) SELECT per group:
        ///    - Country
        ///    - OrderCount
        ///    - CustomerCount (distinct customers in that country)
        ///    - TotalRevenue (sum TotalBill)
        ///    - AvgOrderValue (average TotalBill)
        /// 5) HAVING: Keep only countries with CustomerCount >= 2.
        /// 6) ORDER BY: AvgOrderValue DESC, then Country ASC.
        /// Provide BOTH:
        ///   Option 1 = explicit Join (Orders ↔ Customers)
        ///   Option 2 = navigation property (o.Customer.Country)
        /// </summary>
        public void Scenario47()
        {
            // TODO: Write BOTH Option 1 (Join) and Option 2 (Navigation) queries here


            var start = new DateTime(2025, 1, 1);
            var end = new DateTime(2026, 1, 1);


            //Option 1 (Join)

            var result = _db.Orders.AsNoTracking()
                            .Where(o => o.OrderDate >= start && o.OrderDate < end)
                            .Join(_db.Customers,
                                  o => o.CustomerId,
                                  c => c.CustomerId,
                                  (o, c) => new { o, c })
                            .GroupBy(g => g.c.Country)
                            .Select(g => new
                            {

                                Country = g.Key,
                                OrderCount = g.Count(),
                                CustomerCount = g.Select(x => x.c.CustomerId).Distinct().Count(),
                                TotalRevenue = g.Sum(x => x.o.TotalBill),
                                AvgOrderValue = g.Average(x => x.o.TotalBill)
                            })
                            .Where(x => x.CustomerCount >= 2)
                            .OrderByDescending(x => x.AvgOrderValue)
                            .ThenBy(x => x.Country)
                            .ToList();

            //Option 2 (Navigation)

            var result2 = _db.Orders.AsNoTracking()
                            .Where(o => o.OrderDate >= start && o.OrderDate < end)
                            .GroupBy(g => g.Customer!.Country)
                            .Select(g => new
                            {

                                Country = g.Key,
                                OrderCount = g.Count(),
                                CustomerCount = g.Select(x => x.Customer!.CustomerId).Distinct().Count(),
                                TotalRevenue = g.Sum(x => x.TotalBill),
                                AvgOrderValue = g.Average(x => x.TotalBill)
                            })
                            .Where(x => x.CustomerCount >= 2)
                            .OrderByDescending(x => x.AvgOrderValue)
                            .ThenBy(x => x.Country)
                            .ToList();


            Console.WriteLine("\nScenario 47 Results:");
            Console.WriteLine("Country         | Orders | Customers | TotalRevenue | AvgOrderValue");
            Console.WriteLine("----------------|-------:|----------:|-------------:|--------------:");
            foreach (var r in result) // or result2
            {
                Console.WriteLine($"{r.Country,-15} | {r.OrderCount,6} | {r.CustomerCount,9} | {r.TotalRevenue,12:0.00} | {r.AvgOrderValue,13:0.00}");
            }



        }

        /// <summary>
        /// Scenario 48 (Employees + Departments — post-2022 salary distribution per department):
        /// Using Employees and Departments:
        /// 1) WHERE: Employees with JoinDate >= 2023-01-01.
        /// 2) JOIN: Employees ↔ Departments on DepartmentId.
        /// 3) GROUP BY: DepartmentId + DepartmentName.
        /// 4) SELECT per group:
        ///    - DepartmentId
        ///    - DepartmentName
        ///    - EmployeeCount
        ///    - MinSalary
        ///    - MaxSalary
        ///    - AvgSalary
        /// 5) HAVING: EmployeeCount >= 3.
        /// 6) ORDER BY: AvgSalary DESC, then DepartmentName ASC.
        /// Provide BOTH:
        ///   Option 1 = explicit Join (Employees ↔ Departments)
        ///   Option 2 = navigation property (e.Department.Name)
        /// </summary>
        public void Scenario48()
        {
            // TODO: Write BOTH Option 1 (Join) and Option 2 (Navigation) queries here

            //Option 1 (Join)

            var result = _db.Employees.AsNoTracking()
                            .Where(e => e.JoinDate >= new DateTime(2023, 1, 1))
                            .Join(_db.Departments,
                                e => e.DepartmentId,
                                d => d.DepartmentId,
                                (e, d) => new { e, d })
                            .GroupBy(g => new { g.d.DepartmentId, g.d.Name })
                            .Select(g => new
                            {

                                g.Key.DepartmentId,
                                DepartmentName = g.Key.Name,
                                EmployeeCount = g.Count(),
                                MinSalary = g.Min(x => x.e.Salary),
                                MaxSalary = g.Max(x => x.e.Salary),
                                AvgSalary = g.Average(x => x.e.Salary)
                            })
                            .Where(x => x.EmployeeCount >= 3)
                            .OrderByDescending(x => x.AvgSalary)
                            .ThenBy(x => x.DepartmentName)
                            .ToList();


            //Option 2 (Navigation)

            var result2 = _db.Employees.AsNoTracking()
                            .Where(e => e.JoinDate >= new DateTime(2023, 1, 1))
                            .GroupBy(g => new { g.Department!.DepartmentId, g.Department!.Name })
                            .Select(g => new
                            {

                                g.Key.DepartmentId,
                                DepartmentName = g.Key.Name,
                                EmployeeCount = g.Count(),
                                MinSalary = g.Min(x => x.Salary),
                                MaxSalary = g.Max(x => x.Salary),
                                AvgSalary = g.Average(x => x.Salary)
                            })
                            .Where(x => x.EmployeeCount >= 3)
                            .OrderByDescending(x => x.AvgSalary)
                            .ThenBy(x => x.DepartmentName)
                            .ToList();

            Console.WriteLine("\nScenario 48 Results:");
            Console.WriteLine("DeptId | DepartmentName        | Employees | MinSalary | MaxSalary | AvgSalary");
            Console.WriteLine("-------|-----------------------|----------:|----------:|----------:|----------:");
            foreach (var r in result) // or result2
            {
                Console.WriteLine($"{r.DepartmentId,6} | {r.DepartmentName,-21} | {r.EmployeeCount,9} | {r.MinSalary,9:0.00} | {r.MaxSalary,9:0.00} | {r.AvgSalary,9:0.00}");
            }
        }

        /// <summary>
        /// Scenario 49 (Orders + Customers — active order range per customer in 2025):
        /// Using Orders and Customers:
        /// 1) WHERE: Only consider orders placed in 2025.
        /// 2) JOIN: Orders ↔ Customers on CustomerId.
        /// 3) GROUP BY: CustomerId + CustomerName.
        /// 4) SELECT per group:
        ///    - CustomerId
        ///    - CustomerName
        ///    - OrderCount
        ///    - FirstOrderDate (MIN OrderDate)
        ///    - LastOrderDate  (MAX OrderDate)
        ///    - TotalRevenue   (SUM TotalBill)
        /// 5) HAVING: Only include customers with OrderCount >= 2.
        /// 6) ORDER BY: LastOrderDate DESC, then CustomerName ASC.
        /// Provide BOTH:
        ///   Option 1 = explicit Join (Orders ↔ Customers)
        ///   Option 2 = navigation property (o.Customer.Name).
        /// </summary>
        public void Scenario49()
        {
            // TODO: Write BOTH Option 1 (Join) and Option 2 (Navigation) queries here


            var start = new DateTime(2025, 1, 1);
            var end = new DateTime(2026, 1, 1);

            //  Option 1 (Join)

            var result = _db.Orders.AsNoTracking()
                            .Where(o => o.OrderDate >= start && o.OrderDate < end)
                            .Join(_db.Customers,
                                   o => o.CustomerId,
                                   c => c.CustomerId,
                                   (o, c) => new { o, c })
                            .GroupBy(g => new { g.c.CustomerId, g.c.Name })
                            .Select(g => new
                            {

                                g.Key.CustomerId,
                                CustomerName = g.Key.Name,
                                OrderCount = g.Count(),
                                FirstOrderDate = g.Min(x => x.o.OrderDate),
                                LastOrderDate = g.Max(x => x.o.OrderDate),
                                TotalRevenue = g.Sum(x => x.o.TotalBill)
                            })
                            .Where(x => x.OrderCount >= 2)
                            .OrderByDescending(x => x.LastOrderDate)
                            .ThenBy(x => x.CustomerName)
                            .ToList();

            //Option 2 (Navigation)


            var result2 = _db.Orders.AsNoTracking()
                           .Where(o => o.OrderDate >= start && o.OrderDate < end)                           
                           .GroupBy(g => new { g.Customer!.CustomerId, g.Customer!.Name })
                           .Select(g => new
                           {

                               g.Key.CustomerId,
                               CustomerName = g.Key.Name,
                               OrderCount = g.Count(),
                               FirstOrderDate = g.Min(x => x.OrderDate),
                               LastOrderDate = g.Max(x => x.OrderDate),
                               TotalRevenue = g.Sum(x => x.TotalBill)
                           })
                           .Where(x => x.OrderCount >= 2)
                           .OrderByDescending(x => x.LastOrderDate)
                           .ThenBy(x => x.CustomerName)
                           .ToList();



        }

    }
}


