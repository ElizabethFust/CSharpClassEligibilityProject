using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpEligibilityProject
{
    class Menu
    {
       public static int DisplayMenu()
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("Determine Eligibility for Gathering Strength Foundation Assistance");
                Console.WriteLine("------------------------------------------------------------------");
                Console.WriteLine();
                Console.WriteLine(" 1. Applicant");
                Console.WriteLine(" 2. Existing User");
                Console.WriteLine(" 3. Exit");
                Console.WriteLine();
                Console.Write("Choice: ");
                var result = Console.ReadLine();
                return Convert.ToInt32(result);
            }
        
    }
}
