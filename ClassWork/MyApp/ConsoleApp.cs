using System;
namespace MyApp
{
    // using App2;
    // using b=App3;
    // namespace MyApp
    // {
    //     // internal class Program
    //     // {
    //     //     static void Main(string[] args)
    //     //     {
    //     //         App2.MyClass m = new App2.MyClass();
    //     //         b.MyClass n = new b.MyClass();
    //     //     }
    //     // }
    // }
    // namespace App2
    // {
    //     internal class MyClass
    //     {
    //         public void MyMethod()
    //         {

    //         }
    //     }
    // }
    // namespace App3
    // {
    //     internal class MyClass
    //     {
    //         public void MyMethod()
    //         {

    //         }
    //     }
    // }

    namespace ConsoleApp
    {
        internal class Area
        {
            private int length;
            private int width = 0;
            public Area(int l, int b)
            {
                length = l;
                width = b;
            }
            public Area(int l)
            {
                length = l;
            }
            public int CalculateArea()
            {
                if (width == 0)
                {
                    return length * length;
                }
                else
                {
                    return length * width;
                }

            }
        }

    }
}