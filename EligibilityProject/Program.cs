using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using FileHelpers;
using Newtonsoft.Json;





namespace CSharpEligibilityProject
{
    class Program
    {
        static PovertyData[] PovertyRates;

        private static string _DataDirectory = "C:\\Users\\Liz Fust\\source\\repos\\CSharpClassEligibilityProject\\EligibilityProject";
        private static string _DataFile = "C:\\Users\\Liz Fust\\source\\repos\\CSharpClassEligibilityProject\\EligibilityProject\\Applicants.json";
        
        static void Main(string[] args)
        {
            PovertyRates = GetPovertyData();
 
            MainMenu.Run();


        }

       
        

        //method to create ID number
        public static int GetId(List<Applicant> applicants)
        {
            int returnValue = 1;

            try
            {
                if(applicants.Any())
                {
                    var applicant = applicants.OrderByDescending(a => a.ApplicantId).FirstOrDefault();
                    int id = applicant.ApplicantId;
                    id++;
                    returnValue = id;
                }

            }

            catch (Exception ex)
            {
                ex.Data.Add("GetId Error", "An error occurred while trying to get your Id number.");
                throw;
            }

            return returnValue;
        }

        //Method to add user's info including the user's Id created by GetId method above, and the newly acquired poverty rate to json file, which were saved using SaveApplicantInfo method.
        public Applicant AddApplicant(Applicant applicant, List<Applicant> applicants)
        {
            Console.Write("Do you want to save your information, Yes or No?");
            var choice = Console.ReadLine();

            if (choice.ToLower() == "yes")
            {
                try
                {
                    int newApplicantId = GetId(applicants);
                    applicant.ApplicantId = newApplicantId;
                    applicants.Add(applicant);
                    SaveApplicantInfo(applicants);
                    Console.WriteLine("Your information will be saved. Retain your Id number to retrieve your information later");
                    return applicant;
                }
                catch (Exception ex)
                {
                    ex.Data.Add("InstantiationError",
                    $"An error occurred while trying to create the service.");
                    throw;
                }
            }
            else if (choice.ToLower() == "no")
            {
                DisplayMenu();
            }
                return applicant;
        }
        public void SaveApplicantInfo(List<Applicant> applicants)
        {
            try
            {
                string jsonData = JsonConvert.SerializeObject(applicants);

                if (!string.IsNullOrEmpty(jsonData))
                {
                    File.WriteAllText(_DataFile, jsonData);
                }
            }

            catch (Exception ex)
            {
                ex.Data.Add("SaveError", $"An error occurred while trying to save your information");
                throw;
            }

        }

        //Have user enter Id; retrieve applicant info associated with that Id
       

        
    }
    
}

