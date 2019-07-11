using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using FileHelpers;
using CSharpEligibilityProject;

namespace EligibilityMenus
{
    class ApplicantMenu
    {

        private static string _DataDirectory = "C:\\Users\\Liz Fust\\source\\repos\\EligibilityProjectPracticeJson";
        private static string _DataFile = "C:\\Users\\Liz Fust\\source\\repos\\EligibilityProjectPracticeJson\\Applicants.json";

        public static List<Applicant> applicants { get; set; }

        private static List<Applicant> GetApplicantsFromFile()
        {
            List<Applicant> returnValue = new List<Applicant>();

            try
            {
                //always make sure the file exists before attempting to access it
                if (File.Exists(_DataFile))
                {
                    //read the file
                    string jsonData = File.ReadAllText(_DataFile);

                    if (!String.IsNullOrEmpty(jsonData))
                    {
                        //deserialize the file back into a list
                        returnValue = JsonConvert.DeserializeObject<List<Applicant>>(jsonData);
                    }
                }
                else
                {
                    //we couldn't find the file, throw an error
                    throw new Exception($"GetAllError: Unable to find file: {_DataFile}");
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("GetAllError",
                    $"An error occurred while trying to get players.");
                throw;
            }

            return returnValue;
        }


        //gather input from user; turn input into a List; serialize List to Json file to store
        public static List<Applicant> AddApplicant(Applicant applicant, List<Applicant> applicants)
        {
            //Applicant applicant = new Applicant();
            //List<Applicant> returnValue = new List<Applicant>();
            //var _applicants = new List<Applicant>();
            Console.Clear();
            Console.Write("Please enter your first name:\t");
            applicant.FirstName = Console.ReadLine();
            Console.Write("Please enter your last name:\t");
            applicant.LastName = Console.ReadLine();
            Console.Write("Please enter your zip code:\t");
            applicant.ZipCode = Console.ReadLine();
            applicants.Add(applicant);
            SaveApplicant(applicants);
            return applicants;

        }

        public static void SaveApplicant(List<Applicant> applicants)
        {
            Console.Write("Do you want to save your information, Yes or No?  ");
            var choice = Console.ReadLine();

            if (choice.ToLower() == "yes")
            {

                try
                {
                    string jsonData = JsonConvert.SerializeObject(applicants);


                    if (!string.IsNullOrEmpty(jsonData))
                    {

                        File.WriteAllText(_DataFile, jsonData);
                        //File.AppendAllText(_DataFile, jsonData);
                        Console.WriteLine("Your information has been saved");
                        Console.ReadKey();
                    }
                }
                catch (Exception ex)
                {
                    ex.Data.Add("SaveError", $"An error occurred while trying to save list.");
                    throw;
                }
            }
            else if (choice.ToLower() == "no")
            {
                Console.WriteLine("Goode bye.");
                System.Threading.Thread.Sleep(1000);
            }
        
    }
}
