using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    //.NET 8 / C# 12.0 (2023)
    public class CollectionExpressions
    {
        public static void Run()
        {
            
            // Old way - explicit array initialization
            //int[] numbers = new int[] { 1, 2, 3, 4, 5 };
            // or with type inference
            //int[] numbers = { 1, 2, 3, 4, 5 };

            //string[] names = new string[] { "Alice", "Bob", "Charlie" };

            ///////////////////////////////////////////////////////

            // Array creation with collection expressions
            int[] numbers = [1, 2, 3, 4, 5];
            string[] names = ["Alice", "Bob", "Charlie"];
            double[] values = [3.14, 2.71, 1.41];

            // List creation
            List<int> numbersList = [10, 20, 30, 40];
            List<string> cities = ["New York", "London", "Tokyo"];

            // Other collection types
            HashSet<string> uniqueNames = ["John", "Jane", "Bob", "Alice"];
            //Queue<int> queue = [1, 2, 3, 4, 5];
            //Stack<string> stack = ["bottom", "middle", "top"];

            // Empty collections
            int[] emptyArray = [];
            List<string> emptyList = [];
            HashSet<int> emptySet = [];

            Console.WriteLine($"Numbers: [{string.Join(", ", numbers)}]");
            Console.WriteLine($"Names: [{string.Join(", ", names)}]");
            Console.WriteLine($"Cities count: {cities.Count}");
            Console.WriteLine($"Unique names: {uniqueNames.Count}");

            // Combining arrays and collections using spread operator
            int[] first = [1, 2, 3];
            int[] second = [4, 5, 6];
            List<int> third = [7, 8, 9];

            // Spread collections into new array
            int[] combined = [.. first, .. second, .. third];
            int[] withExtra = [0, .. first, 10, .. second, 20, .. third, 30];

            Console.WriteLine($"Combined: [{string.Join(", ", combined)}]");
            Console.WriteLine($"With extra: [{string.Join(", ", withExtra)}]");

            // Spread with different collection types
            string[] fruits = ["apple", "banana"];
            List<string> vegetables = ["carrot", "broccoli"];
            HashSet<string> grains = ["rice", "wheat"];

            string[] allFoods = [.. fruits, .. vegetables, .. grains, "milk", "cheese"];
            Console.WriteLine($"All foods: [{string.Join(", ", allFoods)}]");

            // Spread with LINQ results
            var evenNumbers = Enumerable.Range(1, 10).Where(x => x % 2 == 0);
            var oddNumbers = Enumerable.Range(1, 10).Where(x => x % 2 == 1);

            int[] allNumbers = [.. evenNumbers, .. oddNumbers];
            int[] sortedNumbers = [.. allNumbers.OrderBy(x => x)];

            Console.WriteLine($"All numbers: [{string.Join(", ", allNumbers)}]");
            Console.WriteLine($"Sorted: [{string.Join(", ", sortedNumbers)}]");

        }
    }
}
