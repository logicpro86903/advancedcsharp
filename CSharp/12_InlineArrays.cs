using System.Runtime.CompilerServices;

namespace CSharp
{
    //.NET 8 / C# 12.0 (2023)
    public class InlineArrays
    {
        // Basic inline array for 4 integers
        [InlineArray(4)]
        public struct IntArray4
        {
            private int _element0;
        }

        // Inline array for 8 bytes
        [InlineArray(8)]
        public struct ByteArray8
        {
            private byte _element0;
        }

        // Inline array for 3 floats (like a 3D vector)
        [InlineArray(3)]
        public struct FloatArray3
        {
            private float _element0;
        }


        public static void Run()
        {
            // Usage examples
            var numbers = new IntArray4();
            numbers[0] = 10;
            numbers[1] = 20;
            numbers[2] = 30;
            numbers[3] = 40;

            Console.WriteLine($"Numbers: [{numbers[0]}, {numbers[1]}, {numbers[2]}, {numbers[3]}]");

            var bytes = new ByteArray8();
            for (int i = 0; i < 8; i++)
            {
                bytes[i] = (byte)(i * 16);
            }

            Console.Write("Bytes: [");
            for (int i = 0; i < 8; i++)
            {
                Console.Write(bytes[i]);
                if (i < 7) Console.Write(", ");
            }
            Console.WriteLine("]");

            var vector = new FloatArray3();
            vector[0] = 1.0f; // X
            vector[1] = 2.0f; // Y  
            vector[2] = 3.0f; // Z

            Console.WriteLine($"Vector: ({vector[0]}, {vector[1]}, {vector[2]})");
        }
    }





    /*

    Inline arrays provide a way to create fixed-size arrays as value types with minimal overhead, 
    offering stack allocation and high performance for scenarios where you need small, 
    fixed-size collections.
    */
}
