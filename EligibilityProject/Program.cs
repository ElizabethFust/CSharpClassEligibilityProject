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
            PovertyRates = GetPovertyRates();

                                //to display contents of csv file to console:
                                //String st = File.ReadAllText("PovertyRateByZip.csv");
                                //Console.WriteLine(st);
                                //Console.ReadKey();
            
            //Display the menu. 
            Run();


        }

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

        //query PovertyRateByZip csv file to get an array of the zip codes and their poverty rates (see in Main PovertyRates = GetPovertyRates();
        private static PovertyData[] GetPovertyRates()
        {
            //try
            //{
                //List<PovertyData> zipResults = new List<PovertyData>();
                //populate results with stuff from csv file
                
                
                var engine = new FileHelperEngine<PovertyData>();
                var results = engine.ReadFile("PovertyRateByZip.csv");


            foreach (PovertyData result in results)
            {
                string LouMsaZip = result.LouMsaZip;
                string PovertyRate = result.PovertyRate;
            }

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
        private static PovertyData GetPovertyRate(string Zip)
        {
            PovertyData returnValue = new PovertyData();
            returnValue = PovertyRates.FirstOrDefault(x => x.LouMsaZip == Zip); //find the first value or the default value
            return returnValue; //error for not finding zip code (maybe list out all zip codes that are valid)
        }

        //Get the applicant's information; retrieve poverty rate; determine eligibility (to have program determine eligibility, I'll need to have foundRecord be a double and pull only the poverty rate from the csv file, then use if/else statement to compare it to 27.7%.
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
            Console.WriteLine($"The poverty rate in your zip code is: {foundRecord.PovertyRate}%");
            Console.WriteLine("If the poverty rate in your zipcode is greater than 27.7%, you are eligible for assistance");
            return applicant;
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
        public static List<Applicant> RetrieveApplicantInfo()
            {
               
                var applicants = new List<Applicant>();
                var serializer = new JsonSerializer();
                List<Applicant> RetrieveApplicantInfo = new List<Applicant>();
                
                Console.Clear();
                Console.WriteLine("Please enter your ID:\t");
                //applicantId = Console.ReadLine();
                return applicants;
                Console.WriteLine(applicants);
            }

        
    }
    
}

