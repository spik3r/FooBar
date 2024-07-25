using System;

namespace FooBar.Services
{
    public class MathService
    {

        public MathService()
        {
        }

        public int Add(int num1, int num2)
        {
            Console.WriteLine($"Add called with: Number 1: {num1}, Number 2: {num2}");
            // Console.WriteLine("Add num1:{num1} num2:{num2}", num1, num2);
            return num1 + num2;
        }

    }
}
