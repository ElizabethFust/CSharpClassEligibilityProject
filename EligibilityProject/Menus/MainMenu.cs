using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FileHelpers;
using CSharpEligibilityProject;
using System.IO;

namespace EligibilityProject.Menus
{
    class MainMenu
    {

        private static int DisplayMenu()
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
            var result = Console.ReadLine();
            var choice = Convert.ToInt32(result);
            return choice;

        }

        public static void Run()
        {
            //create a var to hold the user's selection
            int userInput = 0;

            //continue to loop until a valid
            //number is chosen

            do
            {
                //get the selection
                userInput = DisplayMenu();


                //perform an action based on a selection
                switch (userInput)
                {
                    case 1:
                        GetApplicantInfo();
                        break;
                    case 2:
                        RetrieveApplicantInfo();
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

        public static Applicant GetApplicantInfo()
        {
            Applicant applicant = new Applicant();
            Console.Clear();
            Console.Write("Please enter your first name:\t");
            applicant.FirstName = Console.ReadLine();
            Console.Write("Please enter your last name:\t");
            applicant.LastName = Console.ReadLine();
            Console.Write("Please enter your zip code:\t");
            applicant.ZipCode = Console.ReadLine();
            var foundRecord = GetPovertyRate(applicant.ZipCode);

            if (foundRecord == null)
            {
                HandleBadZip(applicant.ZipCode);
            }

            else
            {
                double povertyrate;

                if (!double.TryParse(foundRecord, out povertyrate))
                {
                    throw new InvalidDataException("Can't parse poverty rate to double.  Poverty rate provided was " + foundRecord);
                }

                Console.WriteLine($"The poverty rate in your zip code is: {foundRecord}");
            }

            Console.WriteLine("If the poverty rate in your zipcode is greater than 27.7%, you are eligible for assistance");
            Console.ReadKey();
            return applicant;
        }

        private static void HandleBadZip(string zipCode)
        {
            Console.WriteLine("HandleBadZip needs to be implemented");
        }

        private static string GetPovertyRate(string Zip)
        {
            string returnValue;
            returnValue = PovertyRates.FirstOrDefault(x => x.LouMsaZip == Zip)?.PovertyRate; //find the first value or the default value
            return returnValue; 
        }
        public static List<Applicant> RetrieveApplicantInfo()
        {

            var applicants = new List<Applicant>();
            var serializer = new JsonSerializer();
            List<Applicant> RetrieveApplicantInfo = new List<Applicant>();

            Console.Clear();
            Console.WriteLine("Please enter your ID:\t");
            //applicantId = Console.ReadLine();
            Console.WriteLine(applicants);
            return applicants;

        }

    }
}
