namespace CSharp
{
    //.NET 6 / C# 10.0 (2021)
    public class GlobalUsings
    {
        public static void Run()
        {
            string[] strings = { "apple", "banana", "cherry" };
            strings.Where(s => s.StartsWith("b"))
                   .ToList()
                   .ForEach(s => Console.WriteLine(s)); 

        }
    }
}
