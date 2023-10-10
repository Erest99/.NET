using System;

namespace MyApp 
{
    internal class Program
    {
        static void Main(string[] args)
        {
            byte number = 0;            
            int count = 10;
            float totalPrice = 20.95f;
            char character = 'A';
            String firstName = "name";
            bool isWorking = false;

            Console.WriteLine(number);
            Console.WriteLine(count);
            Console.WriteLine(totalPrice);
            Console.WriteLine(character);
            Console.WriteLine(firstName);
            Console.WriteLine(isWorking);

            Console.WriteLine("\n{0} {1}", byte.MinValue, byte.MaxValue);

            byte b = 1;
            int i = b;
            Console.WriteLine(i);

            try
            {
                var number2 = "1234";
                byte by = Convert.ToByte(number2);
                Console.WriteLine(by);
            } catch (Exception e){
                Console.WriteLine(e);
            }
        }
    }
}