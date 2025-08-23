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

        // Scenario 11: Employees joined >= 2022-01-01, group by year, having >=2
        public List<JoinYearAggRow> Scenario11()
        {
            const string sql = @"
SELECT CAST(STRFTIME('%Y', e.JoinDate) AS INT) AS [Year],
       COUNT(*) AS TotalEmployees,
       MAX(e.Salary) AS MaxSalary
FROM Employees e
WHERE e.JoinDate >= @from
GROUP BY CAST(STRFTIME('%Y', e.JoinDate) AS INT)
HAVING COUNT(*) >= 2
ORDER BY [Year] ASC;";
            // Note: STRFTIME works on SQLite. For SQL Server, use YEAR(e.JoinDate).
            return ExecQuery(sql,
                cmd => { var p = cmd.CreateParameter(); p.ParameterName = "@from"; p.Value = new DateTime(2022, 1, 1); cmd.Parameters.Add(p); },
                r => new JoinYearAggRow
                {
                    Year = r.GetInt32(0),
                    TotalEmployees = r.GetInt32(1),
                    MaxSalary = Convert.ToDecimal(r.GetValue(2))
                });
        }
    }

    // Simple POCO rows to map SQL results
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
}
