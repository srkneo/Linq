using LinqEfPractice;
using LinqEfPractice.ConsoleApp;
using LinqEfPractice.Data;
using LinqEfPractice.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Program start ?");

// EF InMemory setup
var options = new DbContextOptionsBuilder<PracticeDbContext>()
    .UseInMemoryDatabase("PracticeDb")
    .Options;

using var db = new PracticeDbContext(options);

// >>> Probe JSON directly
var root = JsonDataLoader.LoadJson<RootObject>("sampleData.json");


// … repeat for other tables
db.SaveChanges();

// Seed EF from JSON
db.Database.EnsureDeleted();
db.Database.EnsureCreated();
db.EnsureSeededFromJson();

db.EnsureSeededFromJson();

var answers = new Answers(db);
answers.Scenario30();

// --- SQLite context for RAW SQL scenarios ---
var sqlitePath = Path.Combine(AppContext.BaseDirectory, "PracticeSql.db");
var sqliteConnString = new SqliteConnectionStringBuilder { DataSource = sqlitePath }.ToString();

var sqliteOptions = new DbContextOptionsBuilder<PracticeDbContext>()
    .UseSqlite(sqliteConnString)
    .Options;

//using var sqlDb = new PracticeDbContext(sqliteOptions);
//sqlDb.Database.EnsureCreated();
//sqlDb.EnsureSeededFromJson();

// IMPORTANT: pass sqlDb (not db) to SQLQA
//var sqlQa = new SQLQA(sqlDb);

// try one call
//var s1 = sqlQa.Scenario1();
//Console.WriteLine("SQL Scenario1 count: " + s1.Count);

Console.WriteLine("\nDone. Press any key to exit...");
Console.ReadKey();
