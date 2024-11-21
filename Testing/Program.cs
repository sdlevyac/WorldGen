namespace Testing
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int x = -1;
            int y = 50;
            int rem = x % y;

            // If the signs of x and y are the same, we get `rem + 0`, otherwise we get `rem + y`
            Console.WriteLine(rem + (((x ^ y) >>> 31)) * y);
        }
    }
}
