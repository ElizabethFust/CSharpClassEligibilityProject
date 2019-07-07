using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using FileHelpers;


namespace CSharpEligibilityProject
{
    [DelimitedRecord(",")]
    public class PovertyData  //DO I NEED THIS SEPARATE??????
    {
        public string LouMsaZip { get; set; }
        public string PovertyRate { get; set; }

    }
}
