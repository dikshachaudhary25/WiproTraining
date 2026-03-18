using System;
namespace MyApp
{
    internal class Doctor
    {
        public int DoctorId { get; set; }
        public string Name { get; set; }
        public string Specialisation { get; set; }
        public int MRN { get; set; }
        
        public void DisplayInfo()
        {
            Console.WriteLine($"MRN: {MRN}");
            Console.WriteLine($"DoctorId: {DoctorId}");
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"Specialisation: {Specialisation} cm");
        }
    }
}