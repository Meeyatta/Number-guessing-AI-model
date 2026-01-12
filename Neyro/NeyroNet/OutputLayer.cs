
using System.Linq;
using System.Runtime.Remoting;

namespace Neyro.NeyroNet
{
    class OutputLayer : Layer
    {

        public OutputLayer(int non, int nonp, NeuronType nt, string type) :
            base(non, nonp, nt, type)
        { }

        public override void Recognize(Network net, Layer nextLayer)
        {
            double e_sum = 0;
            for (int i = 0; i < Neurons.Length; i++)
                e_sum += Neurons[i].Output;
            for (int i = 0; i < Neurons.Length; i++)
            {
                net.Fact[i] = Neurons[i].Output / e_sum;
            }
            
        }

        public override double[] BackwardPass(double[] errors, bool save)
        {
            Neuron[] temp = Neurons;
            double[] gr_sum = new double[NumOfPrevNeurons + 1];
            for (int j = 0; j < NumOfPrevNeurons; j++)
            {
                double sum = 0;
                for (int k = 0; k < NumOfNeurons; k++)
                    sum += Neurons[k].Weights[j+1] * errors[k];
                gr_sum[j] = sum;
            }
            for (int i = 0; i < NumOfNeurons; i++)
                for (int n = 0; n < NumOfPrevNeurons + 1; n++)
                {
                    double deltaw;
                    if (n == 0)
                        deltaw = Momentum * LastDeltaWeights[i, 0] + LearningRate * errors[i];
                    else
                        deltaw = Momentum * LastDeltaWeights[i, n] + LearningRate * Neurons[i].Inputs[n - 1] * errors[i];
                    LastDeltaWeights[i, n] = deltaw;
                    Neurons[i].Weights[n] += deltaw;
                }

            if (!save) Neurons = temp;
            return gr_sum;

        }
    }
}
