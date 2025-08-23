using System.Text.Json;

namespace LinqEfPractice.Data
{
    public static class JsonDataLoader
    {
        public static T LoadJson<T>(string fileName)
        {
            var candidates = new[]
            {
                Path.Combine(AppContext.BaseDirectory, "Data", fileName),
                Path.Combine(AppContext.BaseDirectory, fileName),
                Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\Data", fileName))
            };

            var filePath = candidates.FirstOrDefault(File.Exists)
                           ?? throw new FileNotFoundException($"Could not locate {fileName}\nTried:\n{string.Join("\n", candidates)}");

        
            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        }
    }
}
