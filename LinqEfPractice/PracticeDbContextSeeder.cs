using LinqEfPractice.Data;
using LinqEfPractice.Models;

namespace LinqEfPractice
{
    public static class PracticeDbContextSeeder
    {
        public static void EnsureSeededFromJson(this PracticeDbContext db, string fileName = "sampleData.json")
        {
            if (db.Products.Any()) {  return; }



            var root = JsonDataLoader.LoadJson<RootObject>(fileName);

            // Build a totals map from OrderItems
            var totalsByOrder = root.OrderItems
                .GroupBy(oi => oi.OrderId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(oi => oi.UnitPrice * oi.Quantity)
                );

            // Fill TotalBill on each Order (if not already present in JSON)
            foreach (var o in root.Orders)
            {
                if (totalsByOrder.TryGetValue(o.OrderId, out var total))
                    o.TotalBill = total;
            }

            db.AddRange(root.Departments);
            db.AddRange(root.Employees);
            db.AddRange(root.Categories);
            db.AddRange(root.Products);
            db.AddRange(root.Customers);
            db.AddRange(root.Orders);
            db.AddRange(root.OrderItems);
            db.SaveChanges();

        }
    }
}
