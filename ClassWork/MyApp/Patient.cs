using System;
namespace MyApp
{
    public class Patient
    {
        public int MRN { get; set; }
        public string PatientName { get; set; }
        public int Age { get; set; }
        public float BodyWeight { get; set; }
        public float Height { get; set; }

        public void DisplayInfo()
        {
            Console.WriteLine($"MRN: {MRN}");
            Console.WriteLine($"Name: {PatientName}");
            Console.WriteLine($"Age: {Age}");
            Console.WriteLine($"Weight: {BodyWeight} kg");
            Console.WriteLine($"height: {Height} cm");
        }
    }
}
