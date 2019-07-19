using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FileHelpers;
using CSharpEligibilityProject.Menus;
using CSharpEligibilityProject;
using CSharpEligibilityProject.Services;
using System.IO;

namespace EligibilityProject.Menus
{
    class MainMenu
    {
        //static PovertyData[] PovertyRates;
        //PovertyRates = GetPovertyData();


        public static int DisplayMenu()
        {
            Console.Clear();
            Console.Title = "Eligibility for Assistance";
            Console.WriteLine();
            Console.WriteLine("Determine Eligibility for Gathering Strength Foundation Assistance");
            Console.WriteLine("------------------------------------------------------------------");
            Console.WriteLine();
            Console.WriteLine(" 1. Enter 1 below to Determine your eligibility.");
            Console.WriteLine(" 2. Enter 2 below to Retrieve your data.");
            Console.WriteLine(" 3. Exit");
            Console.WriteLine();
            Console.Write("Enter a number to proceed: ");

            //Handle if something other than number entered
            var result = Console.ReadLine();

            int.TryParse(result, out var choice);
            return choice;
        }

        public static void Run()
        {
            int userInput = 0;


            do
            {
                //get the selection
                userInput = DisplayMenu();


                switch (userInput)
                {
                    case 1: //for a new Applicant; to get their information
                        ApplicantMenu.AddApplicant();
                        break;
                    case 2: //for when an applicant returns; can look up their info using their ID#
                        ApplicantService.RetrieveApplicantInfo();
                        //Remember to provide way for user to exit program again
                        break;
                    case 3:
                        Console.WriteLine("Exiting...");
                        System.Threading.Thread.Sleep(2000);
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine();
                        Console.WriteLine(" Error: Invalid Choice");
                        System.Threading.Thread.Sleep(1000);
                        break;
                        //test
                }

            } while (userInput != 3);
        }
    }
}

