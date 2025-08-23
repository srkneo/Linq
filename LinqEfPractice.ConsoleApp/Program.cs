using LinqEfPractice;
using Microsoft.EntityFrameworkCore;
using LinqEfPractice.ConsoleApp;
using LinqEfPractice.Data;
using LinqEfPractice.Models;

Console.WriteLine("Program start ?");

// EF InMemory setup
var options = new DbContextOptionsBuilder<PracticeDbContext>()
    .UseInMemoryDatabase("PracticeDb")
    .Options;

using var db = new PracticeDbContext(options);

// >>> Probe JSON directly
var root = JsonDataLoader.LoadJson<RootObject>("sampleData.json");

// Seed EF from JSON
db.EnsureSeededFromJson();

var answers = new Answers(db);
answers.Scenario10();


Console.WriteLine("\nDone. Press any key to exit...");
Console.ReadKey();
