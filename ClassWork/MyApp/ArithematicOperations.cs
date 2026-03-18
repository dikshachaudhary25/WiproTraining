using System;
namespace MyApp
{
    internal class ArithematicOperations
    {
        // public int Calculatesquare(int x)
        // {
        //     int square = x * x;
        //     return square;
        // }
        // public int Calculatecube(int x)
        // {
        //     int cube = x * x * x;
        //     return cube;
        // }
        // public int Calculatequad(int x)
        // {
        //     int quad = x * x * x * x;
        //     return quad;

        // }
        // public int Calculate(int x,out int y)
        // {
        //     int square = x * x;
        //     y = x * x * x;
        //     int quad = x * x * x * x;
        //     Console.WriteLine("The square is " + square);
        //     Console.WriteLine("The cube is " + y);
        //     Console.Write("The quad is ");
        //     return quad;


        // }
        public int[] Calculate(int x)
        {
            int[] a = new int[3];
            a[0] = x * x;
            a[1] = x * x * x;
            a[2] = x * x * x * x;
            return a;
        }
    }
}