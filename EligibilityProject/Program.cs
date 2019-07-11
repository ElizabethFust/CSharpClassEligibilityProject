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

        private string _DataDirectory;
        private string _DataFile = "";
        static void Main(string[] args)
        {
            PovertyRates = GetPovertyData();
 
            Run();


        }

        
        //query PovertyRateByZip csv file to get an array of the zip codes and their poverty rates (see in Main PovertyRates = GetPovertyRates();
        private static PovertyData[] GetPovertyData()
        {
            //try
            //{
                //List<PovertyData> zipResults = new List<PovertyData>();
                //populate results with stuff from csv file
                
                
                var engine = new FileHelperEngine<PovertyData>();
                var results = engine.ReadFile("PovertyRateByZip.csv");


            //foreach (PovertyData result in results)
            //{
            //    string LouMsaZip = result.LouMsaZip;
            //    string PovertyRate = result.PovertyRate;
            //}

            return results;
 
                        //if(zipResults.Any())
                        //{
                        //    foreach(var zip in zipResults)
                        //    {
                        //        string LouMsaZip = zip.LouMsaZip;
                        //        string PovertyRate = zip.PovertyRate;
                        //    }

                        //}
                
            //}
            //catch(Exception error)
            //{
            //    Console.WriteLine(error.Message);
            //    Console.ReadKey();
            //}

            //return results;   
        }

        //Use Linq to find the poverty rate of the zip code entered by user.  Remember in Main that PovertyRates = GetPovertyRates();
        

        //Get the applicant's information; retrieve poverty rate; determine eligibility (to have program determine eligibility, I'll need to have foundRecord be a double and pull only the poverty rate from the csv file, then use if/else statement to compare it to 27.7%.
        


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

