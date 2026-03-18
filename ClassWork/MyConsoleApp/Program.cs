// // See https://aka.ms/new-console-template for more information
// internal class Program
// {
//     static void Main(string[] args)
//     {
//         Totals t = new Totals();
//         t.Sum(10, 5);
//     }
// }

// public class Totals
// {
//     public void Sum(int x, int y)
//     {
//         Console.WriteLine(x + y);
//     }
// }

// class Program
// {
//     static void Main(string[] args)
//     {
//         Console.Write("Enter your age: ");
//         byte age = Convert.ToByte(Console.ReadLine());
//         string message = (age < 3) ? "Hey small guy" : (age > 3 && age < 10) ? "Hey kid" : (age < 18 && age > 10) ? "Hey teen" : (age > 18) ? "Hey big guy" : "invalid age";
//         Console.WriteLine(message);
//     }
// }

class Program{
    static void Main(string[] args){
        string article = " Don't talk to me you idiot.";
        bool check = article.Contains("idiot");
        if(check){
            Console.WriteLine("Rejected");
        }
        else{
            Console.WriteLine("Accepted");
        }
    }
}