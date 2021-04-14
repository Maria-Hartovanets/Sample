using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SampleActionCalculate
{
    class DiscreteSampling
    {
        List<float> arrayTemperature;// elem in general array
        List<float> m;// elem count in array to make frequency table
        List<float> arrayTemperWithoutCopy;
        List<float> w;// elem to make interval statistic array
        readonly string filePath;
        int n = 0;// sample size
        bool option;

        public DiscreteSampling(bool option = false)
        {
            this.option = option;
            filePath = @"D:\Programming\C#\TIMC\SampleActionCalculate\SampleActionCalculate\Data\TextFile1.txt";


            arrayTemperature = CreateList();//new List<float>(){ 0,0,1,1,0,0,0,2,0,-1,0,0,3,0,0,1,1,2,0,3,0,0,0,0,3,0,0,3,-1,-6,0,0,1,1,2,1,1,0,0,-1,-2,-6,0,0,0,3,6,3,0,0};
            m = new List<float>();
            arrayTemperWithoutCopy = new List<float>();
            w = new List<float>();


            n = arrayTemperature.Count;
        }
        List<float> CreateList()
        {
            List<float> arrayTemperatureTemp = new List<float>();

            using (StreamReader str = new StreamReader(filePath))
            {
                string fileStr = "";
                float number = 0;
                while ((fileStr = str.ReadLine()) != null)
                {
                    number = Convert.ToSingle(fileStr);
                    arrayTemperatureTemp.Add(number);

                }
            }

            return arrayTemperatureTemp;
        }
        void PrintArray(List<float> arr)
        {
            foreach (float item in arr)
                Console.Write($"{item} ");
        }
        List<float> CountElem_Create()
        {
            float temp = 0;
            int counter = 0, index = 0;
            Console.WriteLine("Array - Incoming data:");
            PrintArray(arrayTemperature);

            arrayTemperature.Sort();

            do
            {
                if (index < arrayTemperature.Count)
                    temp = arrayTemperature[index];
                counter = 0;
                for (int i = 0; i < arrayTemperature.Count; i++)
                {
                    if (temp == arrayTemperature[i])
                        counter++;
                }
                index += counter;
                m.Add(counter);

            } while (index < arrayTemperature.Count);
            return m;
        }
        List<float> NoDublicatArray_Create()
        {
            int temp = 0;
            int index = 0;

            arrayTemperWithoutCopy.Add(arrayTemperature[0]);
            foreach (float elem in m)
            {
                index = Convert.ToInt32(elem);
                temp += index;
                if (temp < arrayTemperature.Count)
                {
                    arrayTemperWithoutCopy.Add(arrayTemperature[temp]);
                }
            }
            return arrayTemperWithoutCopy;
        }

        List<float> W_Create()
        {
            foreach (float item in m)
                w.Add(item / n);

            return w;
        }

        List<float> EmpiricFunction_Create()
        {
            List<float> empiric = new List<float>();
            empiric.Add(0);
            float summa = 0;
            for (int i = 0; i < w.Count; i++)
            {
                if (i < w.Count - 1)
                {
                    summa += w[i];
                    empiric.Add(summa);
                }
                else
                    empiric.Add(1);
            }
            Console.WriteLine("\nF(x):");
            for (int i = 0; i < empiric.Count; i++)
            {

                if (i > 0 && i <= empiric.Count - 2)
                {
                    Console.WriteLine($"\t| {empiric[i]},   {arrayTemperWithoutCopy[i - 1]} < x <= {arrayTemperWithoutCopy[i]}");
                }
                else if (i == 0)
                {
                    Console.WriteLine($"\t| {empiric[i]},    x <= {arrayTemperWithoutCopy[0]}");
                }
                else if (i == empiric.Count - 1)
                {
                    Console.WriteLine($"\t| {empiric[i]},    x > {arrayTemperWithoutCopy[arrayTemperWithoutCopy.Count - 1]} ");
                }

            }
            PrintArray(empiric);
            return empiric;
        }
        float AverageNumber() //Xb
        {
            float averageNumb = 0;
            for (int i = 0; i < arrayTemperWithoutCopy.Count; i++)
            {
                averageNumb += (arrayTemperWithoutCopy[i] * m[i]);

            }
            averageNumb /= n;
            return averageNumb;
        }
        float Moda()//M
        {
            float max = m[0];
            int indexMax = 0;
            for (int i = 0; i < m.Count; i++)
            {
                if (max < m[i])
                {
                    max = m[i];
                    indexMax = i;
                }
            }
            return arrayTemperWithoutCopy[indexMax];
        }
        float Mediana()
        {
            float summa = 0; ;
            if (arrayTemperWithoutCopy.Count % 2 == 0)
            {
                summa = arrayTemperWithoutCopy[arrayTemperWithoutCopy.Count / 2] + arrayTemperWithoutCopy[(arrayTemperWithoutCopy.Count / 2) - 1];
                summa /= 2;
                return summa;
            }
            else
            {
                return arrayTemperWithoutCopy[arrayTemperWithoutCopy.Count / 2];
            }

        }
        double StatisticDispersion()//Db(x)
        {
            double dispersion = 0;
            double sum = 0;
            for (int i = 0; i < arrayTemperWithoutCopy.Count; i++)
            {
                sum = arrayTemperWithoutCopy[i] - AverageNumber();
                dispersion += (Math.Pow(sum, 2) * m[i]);
            }

            dispersion /= n;
            return dispersion;
        }
        double AverageStaticticError()//S
        {
            double averageStatError = 0;
            averageStatError = Math.Sqrt(StatisticDispersion());

            return averageStatError;
        }
        double CheckedStaticDispresion()//S^2
        {
            double tempCheckeDipres = 0;
            float n1 = n - 1;
            float temp = (n / n1);
            tempCheckeDipres = temp * StatisticDispersion();

            return tempCheckeDipres;
        }
        float AverageLinearDeviation()//p^(x)
        {
            float p = 0;
            float avaregeNumber = AverageNumber();
            for (int i = 0; i < arrayTemperWithoutCopy.Count; i++)
            {
                p += (Math.Abs(arrayTemperWithoutCopy[i] - avaregeNumber) * m[i]);
            }
            p /= n;
            return p;
        }
        float CoefficientVariationOnAvarageLinear()//Vp %
        {
            float Vp = AverageLinearDeviation() / AverageNumber();
            Vp *= 100;
            return Vp;
        }
        double CoefficientVariotionOnAvarageSguareErr()//Vs %
        {
            double Vs = AverageStaticticError() / AverageNumber();
            Vs *= 100;
            return Vs;
        }
        void Asymmetry_and_Excess()//A
        {
            double M0 = 1, M1 = 0, M2 = 0, M3 = 0, M4 = 0, A = 0, E = 0;
            for (int i = 0; i < arrayTemperWithoutCopy.Count; i++)
            {
                M1 += arrayTemperWithoutCopy[i] * m[i];
                M2 += Math.Pow(arrayTemperWithoutCopy[i], 2) * m[i];
                M3 += Math.Pow(arrayTemperWithoutCopy[i], 3) * m[i];
                M4 += Math.Pow(arrayTemperWithoutCopy[i], 4) * m[i];
            }

            M1 = M1 / n;
            M2 = M2 / n;
            M3 = M3 / n;
            M4 = M4 / n;
            Console.WriteLine($"\tM1= {M1}  M2={M2}  M3={M3}  M4={M4}");
            double Mu1 = 0, Mu2 = CheckedStaticDispresion(), Mu3 = 0, Mu4 = 0;

            Mu3 = M3 - 3 * M2 * M1 + 2 * Math.Pow(M1, 3);
            Mu4 = M4 - 4 * M3 * M1 + 6 * M2 * Math.Pow(M1, 2) - 3 * Math.Pow(M1, 4);
            Console.WriteLine($"\tMu1={Mu1} Mu2={Mu2} Mu3={Mu3} Mu4={Mu4}");

            A = Mu3 / Math.Pow(AverageStaticticError(), 3);
            E = Mu4 / Math.Pow(AverageStaticticError(), 4);
            E -= 3;
            Console.WriteLine($"\tAsymmetry ={A} and Excess = {E}");

        }

        public void ShowResult()
        {
            m = CountElem_Create();
            Console.WriteLine(" \n\narray sorted - Incoming data:");
            PrintArray(arrayTemperature);
            Console.WriteLine($"\n\nSize of array: {n}");

            arrayTemperWithoutCopy = NoDublicatArray_Create();
            arrayTemperature.Sort();
            Console.WriteLine("\n sorted array without dublicates: ");
            PrintArray(arrayTemperWithoutCopy);
            Console.WriteLine($"\nSize of sorted array without dublicates: {arrayTemperWithoutCopy.Count}");

            //m = CountElem_Create();
            Console.WriteLine("\n array count of equals elem in arr: ");
            PrintArray(m);

            Console.WriteLine("\n\n w numbers:  ");
            w = W_Create();
            PrintArray(w);


            Console.WriteLine("\n\n array meaning of empiric function: ");
            EmpiricFunction_Create();
            // PrintArray(EmpiricFunction_Create());

            Console.WriteLine($"\n\n\tX(max)={arrayTemperWithoutCopy[arrayTemperWithoutCopy.Count - 1]} and X(min)={arrayTemperWithoutCopy[0]}");
            Console.WriteLine($"\tR (distribution width)={arrayTemperWithoutCopy[arrayTemperWithoutCopy.Count - 1] - arrayTemperWithoutCopy[0]}");

            Console.WriteLine($"\n\tAverage number of variation: {AverageNumber()}");
            Console.WriteLine($"\tModa of variation: {Moda()}");
            Console.WriteLine($"\tMediana of variation: {Mediana()}");

            Console.WriteLine($"\n\tStatistis dispersion: {StatisticDispersion()}");
            Console.WriteLine($"\tAvarage statistic error: {AverageStaticticError()}");
            Console.WriteLine($"\tChecked statistic dispresion: {CheckedStaticDispresion()}");

            Console.WriteLine($"\n\tAvarage linear deviation: {AverageLinearDeviation()}");

            Console.WriteLine($"\n\tCoefficient of variation on avarage liner deviation: {CoefficientVariationOnAvarageLinear()}%");
            Console.WriteLine($"\tCoefficient of variation on avarage stat error: {CoefficientVariotionOnAvarageSguareErr()}%\n");

            Asymmetry_and_Excess();
        }
    }
}
