using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using FileHelpers;
using CSharpEligibilityProject;
using System.Linq;


namespace CSharpEligibilityProject.Services
{
    public class ApplicantService
    {

        private static string _DataDirectory = "C:\\Users\\Liz Fust\\source\\repos\\CSharpClassEligibilityProject\\EligibilityProject";
        private static string _DataFile = "C:\\Users\\Liz Fust\\source\\repos\\CSharpClassEligibilityProject\\EligibilityProject\\Applicants.json";
        static PovertyData[] PovertyRates;


        //Beginning of Methods referenced in ApplicantMenu (to implement choice 1 -- new applicant)
        public static List<Applicant> GetApplicantsFromFile()
        {
            List<Applicant> returnValue = new List<Applicant>();

            try
            {

                if (File.Exists(_DataFile))
                {
                    string jsonData = File.ReadAllText(_DataFile);

                    if (!String.IsNullOrEmpty(jsonData))
                    {
                        //deserialize the file back into a list
                        returnValue = JsonConvert.DeserializeObject<List<Applicant>>(jsonData);
                    }
                }
                else
                {

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

        public static List<Applicant> GetPRateFromZip(Applicant applicant, List<Applicant> applicants)
        {
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
                applicant.PovertyRate = povertyrate;
                Console.WriteLine($"The poverty rate in your zip code is: {foundRecord}");

            }

            Console.WriteLine("If the poverty rate in your zipcode is greater than 27.7%, you are eligible for assistance");

            return applicants;

        }

        private static void HandleBadZip(string zipCode)
        {
            Console.WriteLine("HandleBadZip needs to be implemented");
        }

        private static string GetPovertyRate(string Zip)
        {
            PovertyRates = GetPovertyData();
            string returnValue;
            returnValue = PovertyRates.FirstOrDefault(x => x.LouMsaZip == Zip)?.PovertyRate; //find the first value or show the default value. ?. is null-conditional--show poverty rate if not null.
            return returnValue;
        }

        public static void SaveApplicant(Applicant applicant, List<Applicant> applicants)
        {
            Console.Write("\nDo you want to save your information, Yes or No?  ");
            var choice = Console.ReadLine();

            if (choice.ToLower() == "yes")
            {
                int newId = GetId(applicants);
                applicant.ApplicantId = newId;
                applicants.Add(applicant);
                Console.WriteLine($"Your Id number is:  {newId}.  Save this number to retrieve your information.");
                System.Threading.Thread.Sleep(4000);
                try
                {

                    string jsonData = JsonConvert.SerializeObject(applicants);


                    if (!string.IsNullOrEmpty(jsonData))
                    {

                        File.WriteAllText(_DataFile, jsonData);

                        Console.WriteLine("Your information has been saved");

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

        public static int GetId(List<Applicant> applicants)
        {
            int returnValue = 1;

            try
            {
                if (applicants.Any())
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
        //End of Methods referenced in ApplicantMenu


        //query PovertyRateByZip csv file using FileHelpers nuget package. Shortens code needed to read the file.  Don't need foreach statement to iterate through file;
        public static PovertyData[] GetPovertyData()
        {
            //PovertyData[] results = new PovertyData[];
            //try
            //{

            var engine = new FileHelperEngine<PovertyData>();
            var results = engine.ReadFile("PovertyRateByZip.csv");

            return results;

            //}
            //catch (Exception error)
            //    {
            //        Console.WriteLine(error.Message);
            //        Console.ReadKey();
            //    }


        }
        public static void RetrieveApplicantInfo()
        {
            Applicant applicant = new Applicant();
            List<Applicant> applicants = new List<Applicant>();
            List<Applicant> _applicantList;
            Console.Clear();
            Console.Write("Please enter your ID number:\t");
            var result = Console.ReadLine();
            applicant.ApplicantId = Convert.ToInt32(result);
            _applicantList = GetApplicantsFromFile();


            var appToShow = _applicantList.SingleOrDefault(a => a.ApplicantId == applicant.ApplicantId);
            if (appToShow != null)
            {
                Console.Write($"\nApplicant ID: {appToShow.ApplicantId} \nApplicant's first name: {appToShow.FirstName} \nApplicant's last name: {appToShow.LastName} \nApplicant's zip code:  {appToShow.ZipCode} \nPoverty rate of applicant's zip code: {appToShow.PovertyRate} ");
                Console.WriteLine();
                Console.WriteLine("\nTo return to the Main Menu, press enter.  To change your zip code press z.");
                var userinput = Console.ReadLine();
                if (userinput.ToLower() == "z")
                {
                   
                    Console.Write("\nPlease enter the new zip code:\t");
                    var newZip = Console.ReadLine();
                    var newAppInfo = _applicantList.First(i => i.ZipCode == appToShow.ZipCode);

                    File.WriteAllText("Path", File.ReadAllText("Path").Replace("SearchString", "Replacement"));

                    //_applicantList [_applicantList.FindIndex(i => i.Equals(appToShow.ZipCode))] = newZip;

                    //Edit();
                    Console.ReadKey();
                }

                else
                {
                    return;
                }
                
            }
            else
            {

                Console.Write($"ERROR: Could not find player with ID: {applicant.ApplicantId}.");
                System.Threading.Thread.Sleep(1000);
            }

        }

        public static void Edit()
        {
            Applicant applicant = new Applicant();
            List<Applicant> applicants = new List<Applicant>();
            
            var oldZip = applicant.ZipCode;
            Console.Write("\nPlease enter the new zip code:\t");
            var newZip = Console.ReadLine();
            GetPRateFromZip(applicant, applicants);
            System.Threading.Thread.Sleep(2000);
            var newPR = 
            var oldPR = 
            var appNewPovRate = applicant.PovertyRate(oldPR, newPR);
            var appNewZip = applicant.ZipCode.Replace(oldZip, newZip);
            try
            {

                string jsonData = JsonConvert.SerializeObject(applicants);


                if (!string.IsNullOrEmpty(jsonData))
                {

                    File.WriteAllText(_DataFile, jsonData);

                    Console.WriteLine("Your information has been saved");

                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("SaveError", $"An error occurred while trying to save to list.");
                throw;
            }

        }


    }
}
