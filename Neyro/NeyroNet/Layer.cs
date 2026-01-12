using System;
using System.IO;
using System.Net.Http.Headers;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Runtime.Serialization.Formatters;
using System.Windows.Forms;

namespace Neyro.NeyroNet
{
    abstract class Layer
    {
        protected string name_Layer;
        string pathDirWeights;
        string pathFileWeights;
        protected int NumOfNeurons;
        protected int NumOfPrevNeurons;
        protected const double LearningRate = 0.0644;
        protected const double Momentum = 0.05d;
        protected double[,] LastDeltaWeights;
        protected Neuron[] Neurons;

        public Neuron[] pNeurons { get => Neurons; set => Neurons = value; }

        public double[] Data
        {
            set
            {
                for (int i = 0; i < NumOfNeurons; i++)
                {
                    pNeurons[i].Activator(value);
                }
            }
        }

        protected Layer(int non, int nopn, NeuronType nt, string nm_Layer)
        {
            NumOfNeurons = non;
            NumOfPrevNeurons = nopn;
            pNeurons = new Neuron[non];
            name_Layer = nm_Layer;
            pathDirWeights = AppDomain.CurrentDomain.BaseDirectory + "memory\\";
            pathFileWeights = pathDirWeights + name_Layer + "_memory.csv";

            LastDeltaWeights = new double[non, nopn + 1];
            double[,] Weights;

            if (File.Exists(pathFileWeights))
                Weights = WeightInitialize(MemoryMode.GET, pathFileWeights);
            else
            {
                Directory.CreateDirectory(pathDirWeights);
                Weights = WeightInitialize(MemoryMode.INIT, pathFileWeights);
            }

            for (int i = 0; i < non; i++)
            {
                double[] tmp_weights = new double[nopn + 1];
                for (int j = 0; j < nopn + 1; j++)
                {
                    tmp_weights[j] = Weights[i, j];
                    pNeurons[i] = new Neuron(tmp_weights, nt);
                }
            }

        }

        public double[,] WeightInitialize(MemoryMode mm, string path)
        {
            char[] delim = new char[] { ';', ' ' };
            string[] tmpStrWeights;
            double[,] weights = new double[NumOfNeurons, NumOfPrevNeurons + 1];

            switch (mm)
            {
                case MemoryMode.GET:
                    tmpStrWeights = File.ReadAllLines(path);
                    string[] memory_elemnt;
                    for (int i = 0; i < NumOfNeurons; i++)
                    {
                        memory_elemnt = tmpStrWeights[i].Split(delim);
                        for (int j = 0; j < NumOfPrevNeurons + 1; j++)
                        {
                            weights[i, j] = double.Parse(memory_elemnt[j].Replace(',', '.'),
                                System.Globalization.CultureInfo.InvariantCulture);
                        }
                    }
                    break;

                case MemoryMode.SET:
                    tmpStrWeights = new string[NumOfNeurons];
                    for (int i = 0; i < NumOfNeurons; i++)
                    {
                        string[] memory_elemnt_set = new string[NumOfPrevNeurons + 1];
                        for (int j = 0; j < NumOfPrevNeurons + 1; j++)
                        {
                            memory_elemnt_set[j] = pNeurons[i].Weights[j].ToString(System.Globalization.CultureInfo.InvariantCulture);
                        }
                        tmpStrWeights[i] = string.Join(";", memory_elemnt_set);
                    }
                    File.WriteAllLines(path, tmpStrWeights);
                    break;

                case MemoryMode.INIT:
                    Random rand = new Random();
                    for (int i = 0; i < NumOfNeurons; i++)
                    {
                        for (int j = 0; j < NumOfPrevNeurons + 1; j++)
                        {
                            weights[i, j] = rand.NextDouble() - 0.5;
                        }
                    }
                    break;
            }

            return weights;
        }

        abstract public void Recognize(Network net, Layer nextLayer); 
        abstract public double[] BackwardPass(double[] stuff, bool save);


    }
}
