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
