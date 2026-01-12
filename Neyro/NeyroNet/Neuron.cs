using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;


namespace Neyro.NeyroNet
{
    class Neuron
    {
        //поля
        private NeuronType type;
        private double[] weights;
        private double[] inputs;
        private double output;
        private double derivative;//производная

        //константы для функции активации
        private double a = 0.01d;

        //свойства
        public double[] Weights { get => weights; set => weights = value; }
        public double[] Inputs { get => inputs; set => inputs = value; }

        public double Output { get => output; }

        public double Derivative { get => derivative; }

        //конструктор

        public Neuron(double[] memoryWeights,NeuronType typeNeuron)
        {
            type = typeNeuron;
            weights = memoryWeights;
        }

        public void Activator(double[] i)
        {
            inputs =i;
            double sum = weights[0];
            for(int j = 0; j < inputs.Length; j++)
            {
                sum += inputs[j] * weights[j + 1];
            }
            switch (type)
            {
                case NeuronType.Hidden: // Для нейронов скрытого слоя
                    output = GipTanh(sum);
                    derivative = GipTanh_Derivativator(sum);
                    break;

                case NeuronType.Output: 
                    output = Exp(sum);
                    break;
            }
        }
        private double GipTanh(double x)
        {
            return (Exp(x) - Exp(-x)) / (Exp(x) + Exp(-x));   
        }

     
        private double GipTanh_Derivativator(double x)
        {
            return 1 - (GipTanh(x) * GipTanh(x));      
        }


    }
}
