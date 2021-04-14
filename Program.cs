using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleActionCalculate
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("1. Covid 19 statictic.\n2. Temperature in a half November and full December 2020.");
            int option = Convert.ToInt32(Console.ReadLine());
            if (option == 1)
            {
                IntervalSampling covid = new IntervalSampling(true);
                covid.ResultPrint();
                Console.ReadKey();
            }
            else if (option == 2)
            {
                DiscreteSampling temperature = new DiscreteSampling(true);
                temperature.ShowResult();
                Console.ReadKey();
            }
        }
    }
}
