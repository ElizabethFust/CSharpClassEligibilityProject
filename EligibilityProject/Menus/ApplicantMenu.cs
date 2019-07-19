﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using FileHelpers;
using CSharpEligibilityProject.Services;

namespace CSharpEligibilityProject.Menus
{
    class ApplicantMenu
    {
        
        //private static string _DataDirectory = "C:\\Users\\Liz Fust\\source\\repos\\C:\\Users\\Liz Fust\\source\\repos\\CSharpClassEligibilityProject\\EligibilityProject";
        private static string _DataFile = "C:\\Users\\Liz Fust\\source\\repos\\CSharpClassEligibilityProject\\EligibilityProject\\Applicants.json";
        static PovertyData[] PovertyRates;


        public static void AddApplicant()
        {
            Applicant applicant = new Applicant();
            List<Applicant> applicants = new List<Applicant>();
            applicants = ApplicantService.GetApplicantsFromFile();
            Console.Clear();
            Console.Write("Please enter your first name:\t");
            applicant.FirstName = Console.ReadLine();
            Console.Write("\nPlease enter your last name:\t");
            applicant.LastName = Console.ReadLine();
            Console.Write("\nPlease enter your zip code:\t");
            applicant.ZipCode = Console.ReadLine();

            ApplicantService.GetPRateFromZip(applicant, applicants);

            ApplicantService.SaveApplicant(applicant, applicants);
            //applicants = GetApplicantsFromFile();
            applicants.Add(applicant);

            System.Threading.Thread.Sleep(1000);
        }


    }
}