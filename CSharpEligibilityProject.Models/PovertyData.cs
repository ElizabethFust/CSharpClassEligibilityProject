using System;
using System.Collections.Generic;
using System.Text;
using FileHelpers;


namespace CSharpEligibilityProject
{
    [DelimitedRecord(",")]
    public class PovertyData
    {
        public string LouMsaZip { get; set; }
        public string PovertyRate { get; set; }

    }
}

