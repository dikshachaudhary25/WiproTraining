// See https://aka.ms/new-console-template for more information
using System;
using MyApp;
namespace NewApp;
class Program
{
    static void Main()
    {
        Patient p = new Patient();

        p.MRN = "MRN00123";
        p.PatientName = "Diksha Chaudhary";
        p.Age = 23;
        p.Weight = 60f;
        p.Height = 170.0f;

        p.DisplayInfo();
    }
}
