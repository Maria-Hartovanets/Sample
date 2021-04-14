using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SampleActionCalculate
{
    class IntervalSampling
    {

        float size = 0; // sample size
        int k = 0;// count of intervals

        List<float> arrayGeneral;// elem in general array
                                 // List<float> arrayCovidNoDublicate;// elem in general array
        List<float> arrayWithoutCopy;
        List<float> arrayCutToInterval;// elem in general array to diahram
        List<float> M_CountInInterval;// elem count to make frequency table
        List<float> N_CountDublicateVar;// elem count to to calculate
        List<float> w;// elem to make interval statistic array
        List<float> h;//elem to use in statistic

        readonly string filePath;

        bool option;
        public IntervalSampling(bool option = false)
        {
            this.option = option;
            filePath = @"D:\Programming\C#\TIMC\SampleActionCalculate\SampleActionCalculate\Data\TextFile2.txt";


            arrayGeneral = CreateList();//new List<float>() { 0,3,8,25,14,20,71,56,168,278,250,360,402,555,761,908,577,244,678,234,869,675,879
                                        //,335,632,1773,1301,1200,1800,1890,2190,2000,1900,1234,890,657,345,450,1205,1202,1500,1453,1342,1100,870,960,597,760,769,543,674};// elem in general array
                                        //arrayCovidNoDublicate = new List<float>();// elem in general array
            arrayWithoutCopy = new List<float>();
            arrayCutToInterval = new List<float>();// elem in general array
            M_CountInInterval = new List<float>() { 1, 12, 6, 5, 8, 3 }; //{16,11,9, 8,3,4}; // elem count to make frequency table
            N_CountDublicateVar = new List<float>(); // elem count to calculate
            w = new List<float>(); // elem to make interval statistic array
            h = new List<float>(); //elem to use in statistic

            size = arrayGeneral.Count;

            k = 6;//1 + Convert.ToInt32(Math.Log2(size));
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
        void ShowArray(List<float> arr)
        {
            foreach (float item in arr)
                Console.Write($"{item} ");
        }
        List<float> Interval_Create()
        {
            float sum = arrayGeneral[0];
            float step = (arrayGeneral[arrayGeneral.Count - 1] - arrayGeneral[0]) / k;

            //float temp = arrayCovid[arrayCovid.Count-1]/k;
            while (sum <= arrayGeneral[arrayGeneral.Count - 1])
            {

                arrayCutToInterval.Add(sum);
                sum += step;
            }
            return arrayCutToInterval;
        }
        List<float> NoDublicatArray_Create()
        {
            int temp = 0;
            int index = 0;

            arrayWithoutCopy.Add(arrayGeneral[0]);
            foreach (float elem in N_CountDublicateVar)
            {
                index = Convert.ToInt32(elem);
                temp += index;
                if (temp < arrayGeneral.Count)
                {
                    arrayWithoutCopy.Add(arrayGeneral[temp]);
                }
            }
            return arrayWithoutCopy;
        }


        List<float> CountElem_Create()
        {
            float temp = 0;
            int counter = 0, index = 0;

            Console.WriteLine("Sorted array");
            arrayGeneral.Sort();
            ShowArray(arrayGeneral);
            Console.WriteLine();

            do
            {
                if (index < arrayGeneral.Count)
                    temp = arrayGeneral[index];
                counter = 0;
                for (int i = 0; i < arrayGeneral.Count; i++)
                {
                    if (temp == arrayGeneral[i])
                        counter++;

                }

                index += counter;
                N_CountDublicateVar.Add(counter);

            } while (index < arrayGeneral.Count);
            Console.WriteLine("\n n array(count of elem in incoming data):  ");
            arrayWithoutCopy = NoDublicatArray_Create();
            return N_CountDublicateVar;
        }

        List<float> W_Create()
        {
            float summa = 0, temp = 0;

            foreach (float elemM in M_CountInInterval)
                w.Add(elemM / (size));

            foreach (float elemW in w)// just for me to check a difference in calculation
                summa += elemW;

            if (summa < 1)
                temp = 1 - summa;

            // Console.WriteLine("\t\t\tError in w "+temp) ;
            return w;
        }

        List<float> H_Create()
        {

            for (int i = 1; i < arrayCutToInterval.Count; i++)
            {
                h.Add(w[i - 1] / (arrayCutToInterval[i] - arrayCutToInterval[i - 1]));
            }

            return h;
        }

        List<float> Empirical_FunctionCreate()
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
            // ShowArray(empiric);
            for (int i = 0; i < empiric.Count; i++)
            {

                if (i > 0 && i < empiric.Count - 1)
                {
                    Console.WriteLine($"\t| {empiric[i]},   x = {(arrayCutToInterval[i + 1] + arrayCutToInterval[i]) / 2}");
                }
                else if (i == 0)
                {
                    Console.WriteLine($"\t| {empiric[i]},    x <= {(arrayCutToInterval[0] + arrayCutToInterval[1]) / 2}");
                }
                else if (i == empiric.Count - 1)
                {
                    Console.WriteLine($"\t| {empiric[i]},    x >= {arrayCutToInterval[arrayCutToInterval.Count - 1]} ");
                }

            }
            return empiric;
        }
        float AverageNumber() //Xb
        {
            float averageNumb = 0;
            for (int i = 0; i < arrayWithoutCopy.Count; i++)
            {
                averageNumb += (arrayWithoutCopy[i] * N_CountDublicateVar[i]);
            }
            averageNumb /= size;
            return averageNumb;
        }
        float Moda()
        {
            float max = -1;
            int indexMax = 0;

            for (int i = 0; i < N_CountDublicateVar.Count; i++)
            {
                if (max < N_CountDublicateVar[i])
                {
                    max = N_CountDublicateVar[i];
                    indexMax = i;
                }
            }
            return arrayWithoutCopy[indexMax];
        }
        float Mediana()
        {
            float summa = 0;

            if (arrayGeneral.Count % 2 == 0)
            {

                summa = arrayGeneral[arrayGeneral.Count / 2] + arrayGeneral[(arrayGeneral.Count / 2) - 1];
                summa /= 2;
                return summa;
            }
            else
            {
                return arrayGeneral[arrayGeneral.Count / 2];
            }

        }
        double StatisticDispersion()//Db(x)
        {
            double dispersion = 0;
            double sum = 0;
            for (int i = 1; i < k; i++)
            {
                sum = arrayGeneral[i] - AverageNumber();
                dispersion += (Math.Pow(sum, 2) * N_CountDublicateVar[i] / size);
            }

            //dispersion /= n;
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
            float n = size - 1;
            float temp = size / n;
            tempCheckeDipres = temp * StatisticDispersion();

            return tempCheckeDipres;
        }
        float AverageLinearDeviation()//p^(x)
        {
            float p = 0;
            for (int i = 0; i < arrayWithoutCopy.Count; i++)
            {
                p += (Math.Abs(arrayWithoutCopy[i] - AverageNumber()) * N_CountDublicateVar[i]);
            }
            p /= size;
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
        void Asymmetry_and_Excess()//A E
        {
            double M0 = 1, M1 = 0, M2 = 0, M3 = 0, M4 = 0, A = 0, E = 0;
            //M1 = AverageNumber();
            for (int i = 0; i < arrayWithoutCopy.Count; i++)
            {
                M1 += arrayWithoutCopy[i] * N_CountDublicateVar[i];
                M2 += Math.Pow(arrayGeneral[i], 2) * N_CountDublicateVar[i];
                M3 += Math.Pow(arrayGeneral[i], 3) * N_CountDublicateVar[i];
                M4 += Math.Pow(arrayGeneral[i], 4) * N_CountDublicateVar[i];
            }

            M1 = M1 / size;
            M2 = M2 / size;
            M3 = M3 / size;
            M4 = M4 / size;
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

        public void ResultPrint()
        {
            Console.WriteLine("Incoming data - array Covid:  ");
            ShowArray(arrayGeneral);




            Console.WriteLine($"\nSample size: {size - 1}");


            N_CountDublicateVar = CountElem_Create();
            arrayCutToInterval = Interval_Create();



            ShowArray(N_CountDublicateVar);

            Console.WriteLine("\n\n Interval array Covid:  ");
            ShowArray(arrayCutToInterval);

            Console.WriteLine("\n m numbers(contains in intervals):  ");
            ShowArray(M_CountInInterval);


            Console.WriteLine("\n\n w numbers for creating function:  ");
            w = W_Create();
            ShowArray(w);

            Console.WriteLine("\n\n h numbers for making a histohram:  ");
            h = H_Create();
            ShowArray(h);

            Console.WriteLine("\n\n Empirical function meanings: ");
            Empirical_FunctionCreate();


            float moda = Moda();
            float mediana = Mediana();
            //arrayCovid.RemoveAt(0);

            Console.WriteLine($"\n\n\tX(max)={arrayGeneral[arrayGeneral.Count - 1]} and X(min)={arrayGeneral[0]}");
            Console.WriteLine($"\tR (distribution width)={arrayGeneral[arrayGeneral.Count - 1] - arrayGeneral[0]}");

            Console.WriteLine($"\n\tAverage number of variation: {AverageNumber()}");
            Console.WriteLine($"\tModa of variation: {moda}");
            Console.WriteLine($"\tMediana of variation: {mediana}");

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
