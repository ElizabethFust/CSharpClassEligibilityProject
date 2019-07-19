using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using FileHelpers;
using Newtonsoft.Json;
using CSharpEligibilityProject.Services;
using EligibilityProject.Menus;





namespace CSharpEligibilityProject
{
    class Program
    {
        static PovertyData[] PovertyRates;

        private static string _DataDirectory = "C:\\Users\\Liz Fust\\source\\repos\\EligibilityProjectPracticeJson";
        private static string _DataFile = "C:\\Users\\Liz Fust\\source\\repos\\EligibilityProjectPracticeJson\\Applicants.json";

        // private static string _ApplicantsPath = "C:\\Users\\Liz Fust\\Source\\Repos\\EligibilityProjectPracticeJson\\Applicants.json";
        static void Main(string[] args)
        {

            PovertyRates = ApplicantService.GetPovertyData();

            if (!Directory.Exists(_DataDirectory))
            {
                Directory.CreateDirectory(_DataDirectory);
            }

            if (!File.Exists(_DataFile))
            {

                File.Create(_DataFile);

            }

            MainMenu.Run();

        }
    }

       
  
    
}

