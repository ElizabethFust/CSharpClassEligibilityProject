using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using FileHelpers;
using CSharpEligibilityProject.Services;
using System.Text.RegularExpressions;

namespace CSharpEligibilityProject.Menus
{
    class ApplicantMenu
    {
        

        public static void AddApplicant()
        {
            Applicant applicant = new Applicant();
            List<Applicant> applicants = new List<Applicant>();
            applicants = ApplicantService.GetApplicantsFromFile();
            Console.Clear();
            Console.Write("Please enter your first name:\t");
            applicant.FirstName = Console.ReadLine();

                while (!Regex.Match(applicant.FirstName, "^[A-Z][a-zA-Z]*$").Success)
                {
                    Console.Write("Please enter a valid name:\t");
                    applicant.FirstName = Console.ReadLine();

                }
                Console.Write("\nPlease enter your last name:\t");

            applicant.LastName = Console.ReadLine();

                while (!Regex.Match(applicant.LastName, "^[A-Z][a-zA-Z]*$").Success)
                {
                    Console.Write("Please enter a valid name:\t");
                    applicant.LastName = Console.ReadLine();

                }

            Console.Write("\nTo be eligible for assistance, your zip code must be in the Louisville Metropolitan Statistical Area.\t");
            //Console.WriteLine();
            //Console.Write("\nThe zip code should be one of the following:");
            //Console.WriteLine();

            //ApplicantService.ShowZips();

            //Console.ReadLine();

            Console.WriteLine();
            Console.Write("\nPlease Enter your zip code:  ");
            applicant.ZipCode = Console.ReadLine();
                
                while (ApplicantService.FindZip(applicant.ZipCode) == null)
                {
                    Console.Write("You have entered an ineligible zip code.");
                    Console.WriteLine();
                    Console.Write("The zip code must be one of the following:");
                    Console.WriteLine();
                    ApplicantService.ShowZips();
                    Console.WriteLine();
                    Console.Write("Please enter a zip code from the Louisville MSA:  ");
                    applicant.ZipCode = Console.ReadLine();
                }
            
            ApplicantService.GetPRateFromZip(applicant, applicants);
                

            ApplicantService.SaveApplicant(applicant, applicants);

            applicants.Add(applicant);

            System.Threading.Thread.Sleep(1000);
        }


    }
}
