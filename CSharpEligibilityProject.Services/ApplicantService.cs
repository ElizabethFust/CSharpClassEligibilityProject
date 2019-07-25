using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using FileHelpers;
using CSharpEligibilityProject;
using System.Linq;
using System.Data;


namespace CSharpEligibilityProject.Services
{
    public class ApplicantService
    {
                
        public static string currentDirectory = Directory.GetCurrentDirectory();
        public static DirectoryInfo directory = new DirectoryInfo(currentDirectory);

        private static string _DataFile = Path.Combine(directory.FullName, "Applicants.json");
        public static string FileName = Path.Combine(directory.FullName, "PovertyRateByZip.csv");
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

            double povertyrate;

                if (!double.TryParse(foundRecord, out povertyrate))
                {
                    throw new InvalidDataException("Can't parse poverty rate to double.  Poverty rate provided was " + foundRecord);

                }
                applicant.PovertyRate = povertyrate;
                Console.WriteLine($"The poverty rate in your zip code is: {foundRecord}");
                Console.WriteLine("If the poverty rate in your zipcode is greater than 27.7%, you are eligible for assistance");
            
           

            return applicants;

        }

        //private static void HandleBadZip(Applicant applicant, string Zip)
        //{
        //    var foundRecord = FindZip(applicant.ZipCode);
        //    if(foundRecord == null)
        //    {

        //    }
        //}

        public static void ShowZips()
        {
            var engine = new FileHelperEngine<PovertyData>();
            engine.Options.IgnoreFirstLines = 1;
            var records = engine.ReadFile(FileName);
            Console.WriteLine("Louisville MSA zip Codes:");
            foreach (var record in records)
            {

                Console.Write(record.LouMsaZip + " ");

            }
        }


        public static string FindZip(string ZipCode)
        {
            var zipCodes = GetPovertyData();
            string returnValue;
            returnValue = zipCodes.FirstOrDefault(x => x.LouMsaZip == ZipCode)?.LouMsaZip; //find the first value or show the default value. ?. is null-conditional--show poverty rate if not null.
            return returnValue;
        }
        public static string GetPovertyRate(string Zip)
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
                        Console.WriteLine();
                        Console.WriteLine("Press any key to return to the Main Menu.");
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

            //return results;
        }

        //public static PovertyData[] GetZipData()
        //{
        //    var engine = new FileHelperEngine<PovertyData>();
        //    var results = engine.ReadFile("PovertyRateByZip.csv");
        //    return results;
        //}
        public static void RetrieveApplicantInfo()
        {
            Applicant applicant = new Applicant();
            List<Applicant> applicants = new List<Applicant>();
            List<Applicant> _applicantList;
            Console.Clear();
            Console.Write("Please enter your ID number:\t");
            var input = Console.ReadLine();
            int num = -1;
            if (!int.TryParse(input, out num))
            {
                Console.WriteLine("invalid ID");
                Console.WriteLine("Please enter your ID number:\t");
                Console.ReadKey();
            }
            else
            {
                applicant.ApplicantId = num;
            }

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

                    var oldZip = appToShow.ZipCode;
                    Console.WriteLine($"Your current zip code is:  {oldZip}");
                    Console.Write("\nPlease enter the new zip code:\t");
                    appToShow.ZipCode = Console.ReadLine();
                    var newPR = GetPovertyRate(appToShow.ZipCode);
                    if (newPR == null)
                    {
                        Console.WriteLine("To be eligible for assistance, your zip code must be a five-digit code \nwithin the Louisville Metropolitan Statistical Area.");
                        ShowZips();
                        Console.WriteLine();
                        Console.Write("Please enter a valid zip code:  "); 
                    }

                    else
                    {
                        double povertyrate;

                        if (!double.TryParse(newPR, out povertyrate))
                        {
                            throw new InvalidDataException("Can't parse poverty rate to double.  Poverty rate provided was " + newPR);

                        }
                        appToShow.PovertyRate = povertyrate;
                        Console.WriteLine($"The poverty rate in your zip code is: {newPR}");

                    }

                    Console.WriteLine("If the poverty rate in your zipcode is greater than 27.7%, you are eligible for assistance");
                    SaveNewInfo(_applicantList);
                    //Console.WriteLine($"Your new zip code, {appToShow.ZipCode}, and your new poverty rate, {newPR}, have been saved.");
                    Console.WriteLine("Press any key to return to the Main Menu.");
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
                System.Threading.Thread.Sleep(2000);
            }

        }

        public static void SaveNewInfo(List<Applicant> applicants)
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
                ex.Data.Add("SaveError",
                    $"An error occurred while trying to save the list.");
                throw;
            }
        }

    }
}
