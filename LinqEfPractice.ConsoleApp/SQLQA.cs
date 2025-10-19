using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace LinqEfPractice.ConsoleApp
{
    public class SQLQA
    {
        private readonly DbConnection _conn;

        public SQLQA(PracticeDbContext db)
        {
            _conn = db.Database.GetDbConnection();
        }

        private void EnsureOpen()
        {
            if (_conn.State != ConnectionState.Open)
                _conn.Open();
        }

        private List<T> ExecQuery<T>(string sql, Action<DbCommand>? paramBinder, Func<DbDataReader, T> map)
        {
            EnsureOpen();
            using var cmd = _conn.CreateCommand();
            cmd.CommandText = sql;
            paramBinder?.Invoke(cmd);
            var list = new List<T>();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
                list.Add(map(reader));
            return list;
        }

        // Scenario 1: products in Electronics with price < 6000, ordered by price, name
        public List<ProductRow> Scenario1()
        {
            const string sql = @"
                SELECT p.ProductId, p.Name, p.Price
                FROM Products p
                JOIN Categories c ON c.CategoryId = p.CategoryId
                WHERE c.Name = @cat AND p.Price < @price
                ORDER BY p.Price ASC, p.Name ASC;";

            var results = ExecQuery(sql,
                cmd =>
                {
                    var p1 = cmd.CreateParameter();
                    p1.ParameterName = "@cat";
                    p1.Value = "Electronics";
                    cmd.Parameters.Add(p1);

                    var p2 = cmd.CreateParameter();
                    p2.ParameterName = "@price";
                    p2.Value = 6000m;
                    cmd.Parameters.Add(p2);
                },
                r => new ProductRow
                {
                    ProductId = r.GetInt32(0),
                    Name = r.GetString(1),
                    Price = r.GetDecimal(2)
                });

            // Print to console
            Console.WriteLine("\nSQL Scenario 1 Results:");
            foreach (var item in results)
            {
                Console.WriteLine($"{item.ProductId} - {item.Name} - {item.Price:0.00}");
            }

            return results;
        }


        // Scenario 3: per department aggregates
        public List<DeptAggRow> Scenario3()
        {
                        const string sql = @"
            SELECT d.Name AS DepartmentName,
                   COUNT(e.EmployeeId) AS TotalEmployees,
                   AVG(e.Salary) AS AverageSalary,
                   MAX(e.Salary) AS MaxSalary
            FROM Departments d
            LEFT JOIN Employees e ON e.DepartmentId = d.DepartmentId
            GROUP BY d.Name
            ORDER BY d.Name;";
            return ExecQuery(sql, null, r => new DeptAggRow
            {
                DepartmentName = r.IsDBNull(0) ? "" : r.GetString(0),
                TotalEmployees = r.GetInt32(1),
                AverageSalary = r.IsDBNull(2) ? 0m : Convert.ToDecimal(r.GetValue(2)),
                MaxSalary = r.IsDBNull(3) ? 0m : Convert.ToDecimal(r.GetValue(3))
            });
        }

        // Scenario 9: Orders grouped by status
        public List<OrderStatusAggRow> Scenario9()
        {
                        const string sql = @"
            SELECT o.Status,
                   COUNT(*) AS TotalOrders,
                   MIN(o.OrderDate) AS EarliestOrderDate,
                   MAX(o.OrderDate) AS LatestOrderDate
            FROM Orders o
            GROUP BY o.Status
            ORDER BY TotalOrders DESC, o.Status ASC;";
            return ExecQuery(sql, null, r => new OrderStatusAggRow
            {
                Status = r.GetString(0),
                TotalOrders = r.GetInt32(1),
                EarliestOrderDate = r.GetDateTime(2),
                LatestOrderDate = r.GetDateTime(3)
            });
        }

        // Scenario 10: Orders since 2025-01-01 grouped by CustomerId, having count >= 2, ordered by last date desc then CustomerId
        public List<CustomerOrdersAggRow> Scenario10()
        {
            const string sql = @"
            SELECT o.CustomerId,
                   COUNT(*) AS TotalOrders,
                   MIN(o.OrderDate) AS FirstOrderDate,
                   MAX(o.OrderDate) AS LastOrderDate
            FROM Orders o
            WHERE o.OrderDate >= @fromDate
            GROUP BY o.CustomerId
            HAVING COUNT(*) >= 2
            ORDER BY LastOrderDate DESC, o.CustomerId ASC;";
            return ExecQuery(sql,
                cmd => {
                    var p = cmd.CreateParameter(); p.ParameterName = "@fromDate"; p.Value = new DateTime(2025, 1, 1); cmd.Parameters.Add(p);
                },
                r => new CustomerOrdersAggRow
                {
                    CustomerId = r.GetInt32(0),
                    TotalOrders = r.GetInt32(1),
                    FirstOrderDate = r.GetDateTime(2),
                    LastOrderDate = r.GetDateTime(3)
                });
        }

        
        public List<EmployeeRow> SQLScenario1()
        {
            const string sql = @"SELECT EmployeeId,FullName,IsActive FROM Employees WHERE IsActive = 1 ORDER BY FullName ASC";

            var results = ExecQuery(sql,
                paramBinder: null,
                map: r => new EmployeeRow
                { 
                    EmployeeId = r.GetInt32(0),
                    FullName = r.GetString(1),
                    IsActive = Convert.ToBoolean(r.GetValue(2))
                });

            // Print to console with column headers
            Console.WriteLine("\nSQL Scenario1 Results:");
            Console.WriteLine("EmployeeId | FullName | IsActive");
            Console.WriteLine("---------------------------------");
            foreach (var e in results)
            {
                Console.WriteLine($"{e.EmployeeId} | {e.FullName} | {e.IsActive}");
            }

            return results;
        }

        /// <summary>
        /// SQL Scenario2:
        /// Table: Products
        /// Columns: ProductId (int), Name (string), Price (decimal), CategoryId (int)
        ///
        /// Task:
        /// List all products with a Price greater than 1000.
        /// Return ProductId, Name, Price.
        /// Sort results by Price descending.
        /// </summary>
        public List<ProductRow> SQLScenario2()
        {
            const string sql = @"SELECT ProductId, Name, Price
                                    FROM Products
                                    WHERE Price > 1000
                                    ORDER BY Price DESC";

            var results = ExecQuery(sql,
                paramBinder: null,
                map: r => new ProductRow
                {
                    ProductId = r.GetInt32(0),
                    Name = r.GetString(1),
                    Price = r.GetDecimal(2)
                });

            // Print to console with column headers
            Console.WriteLine("\nSQL Scenario2 Results:");
            Console.WriteLine("ProductId | Name | Price");
            Console.WriteLine("-------------------------");
            foreach (var p in results)
            {
                Console.WriteLine($"{p.ProductId} | {p.Name} | {p.Price:0.00}");
            }

            return results;
        }


        /// <summary>
        /// SQL Scenario3:
        /// Table: Customers
        /// Columns: CustomerId (int), Name (string), Country (string)
        ///
        /// Task:
        /// List all customers who are from "India".
        /// Return CustomerId, Name, Country.
        /// Sort results by Name ascending.
        /// </summary>
        public List<CustomerRow> SQLScenario3()
        {
            const string sql = @"Select CustomerId,Name,Country from Customers where Country = 'India' ORDER BY Name ASC";

            var results = ExecQuery(sql,
                paramBinder: null,
                map: r => new CustomerRow
                {
                    CustomerId = r.GetInt32(0),
                    Name = r.GetString(1),
                    Country = r.GetString(2)
                });

            // Print to console with column headers
            Console.WriteLine("\nSQL Scenario3 Results:");
            Console.WriteLine("CustomerId | Name | Country");
            Console.WriteLine("---------------------------");
            foreach (var c in results)
            {
                Console.WriteLine($"{c.CustomerId} | {c.Name} | {c.Country}");
            }

            return results;
        }

        /// <summary>
        /// SQL Scenario4:
        /// Table: Employees
        /// Columns: EmployeeId (int), FullName (string), IsActive (bool), JoinDate (datetime), DepartmentId (int), Salary (decimal)
        ///
        /// Task:
        /// List all employees who are NOT active (IsActive = 0).
        /// Return EmployeeId, FullName, IsActive.
        /// Sort results by FullName ascending.
        /// </summary>
        public List<EmployeeRow> SQLScenario4()
        {
            const string sql = @"SELECT EmployeeId,FullName, IsActive FROM Employees WHERE IsActive = 0 ORDER BY FullName";

            var results = ExecQuery(sql,
                paramBinder: null,
                map: r => new EmployeeRow
                {
                    EmployeeId = r.GetInt32(0),
                    FullName = r.GetString(1),
                    IsActive = Convert.ToBoolean(r.GetValue(2))
                });

            // Print to console with column headers
            Console.WriteLine("\nSQL Scenario4 Results:");
            Console.WriteLine("EmployeeId | FullName | IsActive");
            Console.WriteLine("---------------------------------");
            foreach (var e in results)
            {
                Console.WriteLine($"{e.EmployeeId} | {e.FullName} | {e.IsActive}");
            }

            return results;
        }

        /// <summary>
        /// SQL Scenario5:
        /// Table: Products
        /// Columns: ProductId (int), Name (string), Price (decimal), CategoryId (int)
        ///
        /// Task:
        /// List all products whose Name starts with the letter 'M'.
        /// Return ProductId, Name, Price.
        /// Sort results by Name ascending.
        /// </summary>
        public List<ProductRow> SQLScenario5()
        {
            const string sql = @"Select ProductId, Name, Price From Products Where Name like 'M%' Order by Name ASC";

            var results = ExecQuery(sql,
                paramBinder: null,
                map: r => new ProductRow
                {
                    ProductId = r.GetInt32(0),
                    Name = r.GetString(1),
                    Price = r.GetDecimal(2)
                });

            // Print to console with column headers
            Console.WriteLine("\nSQL Scenario5 Results:");
            Console.WriteLine("ProductId | Name | Price");
            Console.WriteLine("-------------------------");
            foreach (var p in results)
            {
                Console.WriteLine($"{p.ProductId} | {p.Name} | {p.Price:0.00}");
            }

            return results;
        }


        /// <summary>
        /// SQL Scenario6:
        /// Table: Orders
        /// Columns: OrderId (int), CustomerId (int), OrderDate (datetime), Status (string)
        ///
        /// Task:
        /// List all orders placed after January 1, 2025.
        /// Return OrderId, CustomerId, OrderDate.
        /// Sort results by OrderDate ascending.
        /// </summary>
        public List<OrderRow> SQLScenario6()
        {
            const string sql = @"Select OrderId, CustomerId, OrderDate from Orders where OrderDate > '2025-01-01' ORDER BY OrderDate ASC";

            var results = ExecQuery(sql,
                paramBinder: null,
                map: r => new OrderRow
                {
                    OrderId = r.GetInt32(0),
                    CustomerId = r.GetInt32(1),
                    OrderDate = r.GetDateTime(2)
                });

            // Print to console with column headers
            Console.WriteLine("\nSQL Scenario6 Results:");
            Console.WriteLine("OrderId | CustomerId | OrderDate");
            Console.WriteLine("--------------------------------");
            foreach (var o in results)
            {
                Console.WriteLine($"{o.OrderId} | {o.CustomerId} | {o.OrderDate:yyyy-MM-dd}");
            }

            return results;
        }


        /// <summary>
        /// SQL Scenario7:
        /// Tables: Products (ProductId, Name, Price, CategoryId)
        ///          Categories (CategoryId, Name)
        ///
        /// Task:
        /// List all products along with their category name.
        /// Return ProductId, ProductName, CategoryName, and Price.
        /// Sort results by CategoryName ascending, then ProductName ascending.
        /// </summary>
        public List<ProductCategoryRow> SQLScenario7()
        {
            const string sql = @"Select p.ProductId, p.Name as ProductName,c.Name as CatagoryName,p.Price

                                From Product p Inner Join Catagories c ON p.ProductId = c.ProductId
                                Order By c.Name ASC,P.Name ASC";

            var results = ExecQuery(sql,
                paramBinder: null,
                map: r => new ProductCategoryRow
                {
                    ProductId = r.GetInt32(0),
                    ProductName = r.GetString(1),
                    CategoryName = r.GetString(2),
                    Price = r.GetDecimal(3)
                });

            // Print to console with column headers
            Console.WriteLine("\nSQL Scenario7 Results:");
            Console.WriteLine("ProductId | ProductName | CategoryName | Price");
            Console.WriteLine("------------------------------------------------");
            foreach (var row in results)
            {
                Console.WriteLine($"{row.ProductId} | {row.ProductName} | {row.CategoryName} | {row.Price:0.00}");
            }

            return results;
        }

        /// <summary>
        /// SQL Scenario8:
        /// Tables: Employees (EmployeeId, FullName, IsActive, JoinDate, DepartmentId, Salary)
        ///          Departments (DepartmentId, Name)
        ///
        /// Task:
        /// List all employees along with their department name.
        /// Return EmployeeId, FullName, DepartmentName, and Salary.
        /// Sort results by DepartmentName ascending, then FullName ascending.
        /// </summary>
        public List<EmployeeDepartmentRow> SQLScenario8()
        {
            const string sql = @"SELECT e.EmployeeId, e.FullName, d.Name DepartmentName, e.Salary FROM Employees e INNER JOIN Departments d ON e.DepartmentId = d.DepartmentId ORDER BY d.Name ASC, e.FullName ASC";

            var results = ExecQuery(sql,
                paramBinder: null,
                map: r => new EmployeeDepartmentRow
                {
                    EmployeeId = r.GetInt32(0),
                    FullName = r.GetString(1),
                    DepartmentName = r.GetString(2),
                    Salary = r.GetDecimal(3)
                });

            // Print to console with column headers
            Console.WriteLine("\nSQL Scenario8 Results:");
            Console.WriteLine("EmployeeId | FullName | DepartmentName | Salary");
            Console.WriteLine("------------------------------------------------");
            foreach (var row in results)
            {
                Console.WriteLine($"{row.EmployeeId} | {row.FullName} | {row.DepartmentName} | {row.Salary:0.00}");
            }

            return results;
        }


        /// <summary>
        /// SQL Scenario9:
        /// Tables: Orders (OrderId, CustomerId, OrderDate, Status)
        ///          Customers (CustomerId, Name, Country)
        ///
        /// Task:
        /// List all orders along with the customer name and country.
        /// Return OrderId, CustomerName, Country, and OrderDate.
        /// Sort results by CustomerName ascending, then OrderDate ascending.
        /// </summary>
        public List<OrderCustomerRow> SQLScenario9()
        {
            const string sql = @"SELECT o.OrderId, c.Name AS CustomerName, c.Country, o.OrderDate From Orders o INNER JOIN Customers c ON o.CustomerId = c.CustomerId ORDER BY c.Name ASC, o.OrderDate ASC";

            var results = ExecQuery(sql,
                paramBinder: null,
                map: r => new OrderCustomerRow
                {
                    OrderId = r.GetInt32(0),
                    CustomerName = r.GetString(1),
                    Country = r.GetString(2),
                    OrderDate = r.GetDateTime(3)
                });

            // Print to console with column headers
            Console.WriteLine("\nSQL Scenario9 Results:");
            Console.WriteLine("OrderId | CustomerName | Country | OrderDate");
            Console.WriteLine("---------------------------------------------");
            foreach (var row in results)
            {
                Console.WriteLine($"{row.OrderId} | {row.CustomerName} | {row.Country} | {row.OrderDate:yyyy-MM-dd}");
            }

            return results;
        }


        /// <summary>
        /// SQL Scenario10:
        /// Tables: OrderItems (OrderItemId, OrderId, ProductId, Quantity, UnitPrice)
        ///          Products (ProductId, Name, Price, CategoryId)
        ///
        /// Task:
        /// List all order items along with their product name and unit price
        /// for items where Quantity > 2.
        /// Return OrderItemId, ProductName, Quantity, and UnitPrice.
        /// Sort results by ProductName ascending.
        /// </summary>
        public List<OrderItemProductRow> SQLScenario10()
        {
            const string sql = @"SELECT oi.OrderItemid,p.Name as ProductName, oi.Quantity,oi.UnitPrice FROM OrderItems oi INNER JOIN  Products ON p  oi.ProductId = p.ProductId where oi.Quantity > 2 
                                 ORDER BY p.Name ASC";

            var results = ExecQuery(sql,
                paramBinder: null,
                map: r => new OrderItemProductRow
                {
                    OrderItemId = r.GetInt32(0),
                    ProductName = r.GetString(1),
                    Quantity = r.GetInt32(2),
                    UnitPrice = r.GetDecimal(3)
                });

            // Print to console with column headers
            Console.WriteLine("\nSQL Scenario10 Results:");
            Console.WriteLine("OrderItemId | ProductName | Quantity | UnitPrice");
            Console.WriteLine("------------------------------------------------");
            foreach (var row in results)
            {
                Console.WriteLine($"{row.OrderItemId} | {row.ProductName} | {row.Quantity} | {row.UnitPrice:0.00}");
            }

            return results;
        }


        /// <summary>
        /// SQL Scenario11:
        /// Tables: Orders (OrderId, CustomerId, OrderDate, Status)
        ///          Customers (CustomerId, Name, Country)
        ///
        /// Task:
        /// List all orders placed after January 1, 2025,
        /// along with the customer name and country.
        /// Return OrderId, CustomerName, Country, and OrderDate.
        /// Sort results by OrderDate ascending.
        /// </summary>
        public List<OrderCustomerRow> SQLScenario11()
        {
            const string sql = @"SELECT o.OrderId, c.Name as CustomerName, c.Country, o.OrderDate
                                 FROM Orders o INNER JOIN Customers c ON o.CustomerId = c.CustomerId
                                 WHERE o.OrderDate > '2025-01-01'
                                 ORDER BY OrderDate ASC";

            var results = ExecQuery(sql,
                paramBinder: null,
                map: r => new OrderCustomerRow
                {
                    OrderId = r.GetInt32(0),
                    CustomerName = r.GetString(1),
                    Country = r.GetString(2),
                    OrderDate = r.GetDateTime(3)
                });

            // Print to console with column headers
            Console.WriteLine("\nSQL Scenario11 Results:");
            Console.WriteLine("OrderId | CustomerName | Country | OrderDate");
            Console.WriteLine("---------------------------------------------");
            foreach (var row in results)
            {
                Console.WriteLine($"{row.OrderId} | {row.CustomerName} | {row.Country} | {row.OrderDate:yyyy-MM-dd}");
            }

            return results;
        }




    }

    // Simple POCO rows to map SQL results

    public class OrderItemProductRow
    {
        public int OrderItemId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }


    public class OrderCustomerRow
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
    }


    public class EmployeeDepartmentRow
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public decimal Salary { get; set; }
    }


    public class ProductCategoryRow
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }


    public class CustomerRow
    {
        public int CustomerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
    }


    public class EmployeeRow
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    public class ProductRow
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
    }

    public class DeptAggRow
    {
        public string DepartmentName { get; set; } = "";
        public int TotalEmployees { get; set; }
        public decimal AverageSalary { get; set; }
        public decimal MaxSalary { get; set; }
    }

    public class OrderStatusAggRow
    {
        public string Status { get; set; } = "";
        public int TotalOrders { get; set; }
        public DateTime EarliestOrderDate { get; set; }
        public DateTime LatestOrderDate { get; set; }
    }

    public class CustomerOrdersAggRow
    {
        public int CustomerId { get; set; }
        public int TotalOrders { get; set; }
        public DateTime FirstOrderDate { get; set; }
        public DateTime LastOrderDate { get; set; }
    }

    public class JoinYearAggRow
    {
        public int Year { get; set; }
        public int TotalEmployees { get; set; }
        public decimal MaxSalary { get; set; }
    }

    public class OrderRow
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
    }

}
