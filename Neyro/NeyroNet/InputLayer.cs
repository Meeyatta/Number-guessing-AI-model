using System;
using System.IO;

namespace Neyro.NeyroNet
{
    class InputLayer
    {
        private double[,] TrainSet;
        private double[,] TestSet;

        public double[,] pTrainset { get => TrainSet; }
        public double[,] pTestset { get => TestSet; }



        public InputLayer(NetworkMode nm)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string[] tmpArrStr;
            string[] tmpStr;

            switch (nm)
            {
                case NetworkMode.Train:
                    tmpArrStr = File.ReadAllLines(path + "train.txt");
                    TrainSet = new double[tmpArrStr.Length, 16];
                    for (int i = 0; i < tmpArrStr.Length; i++)
                    {
                        tmpStr = tmpArrStr[i].Split(' ');
                        for (int j = 0; j < 16; j++)
                        {
                            TrainSet[i, j] = double.Parse(tmpStr[j]);
                        }
                    }
                    Shuffling_Array_Rows(TrainSet);
                    break;

                case NetworkMode.Test:
                    tmpArrStr = File.ReadAllLines(path + "test.txt");
                    TestSet = new double[tmpArrStr.Length, 16];
                    for (int i = 0; i < tmpArrStr.Length; i++)
                    {
                        tmpStr = tmpArrStr[i].Split(' ');
                        for (int j = 0; j < 16; j++)
                        {
                            TestSet[i, j] = double.Parse(tmpStr[j]);
                        }
                    }
                    Shuffling_Array_Rows(TestSet);
                    break;


            }
        }
        public void Shuffling_Array_Rows(double[,] arr)
        {
            Random rand = new Random();
            if (arr.GetLength(0) == 0) return;

            int rowCount = arr.GetLength(0);
            int colCount = arr.GetLength(1);

            for (int i = rowCount - 1; i > 0; i--)
            {
                int j = rand.Next(i + 1);

                for (int k = 0; k < colCount; k++)
                {
                    double temp = arr[i, k];
                    arr[i, k] = arr[j, k];
                    arr[j, k] = temp;
                }
            }
        }

    }

}
