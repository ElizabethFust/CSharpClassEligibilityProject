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

        static void Main(string[] args)
        {

            PovertyRates = ApplicantService.GetPovertyData();

            

            MainMenu.Run();

        }
    }

       
  
    
}

