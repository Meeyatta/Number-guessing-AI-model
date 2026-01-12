namespace Neyro.NeyroNet
{
    class HiddenLayer : Layer
    {
        public HiddenLayer(int non, int nonp, NeuronType nt, string type) :
            base(non, nonp, nt, type)
        { 
        
        }

        public override void Recognize(Network net, Layer nextLayer)
        {
            double[] hidden_out = new double[NumOfNeurons];
            for (int i = 0; i < NumOfNeurons; i++)
            {
                hidden_out[i] = Neurons[i].Output;
            }
                
            nextLayer.Data = hidden_out;
        }

        public override double[] BackwardPass(double[] gr_sums, bool save)
        {
            Neuron[] temp = Neurons;

            double[] gr_sum = new double[NumOfPrevNeurons];
            for (int j = 0; j < NumOfPrevNeurons; j++)
            {
                double sum = 0;
                for (int k = 0; k < NumOfNeurons; k++)
                {
                    sum += Neurons[k].Weights[j+1] * Neurons[k].Derivative * gr_sums[k];
                }
                    
                gr_sum[j] = sum;
            }
            for (int i = 0; i < NumOfNeurons; i++)
                for (int n = 0; n < NumOfPrevNeurons + 1; n++)
                {
                    double deltaw;
                    if (n == 0)
                        deltaw = Momentum * LastDeltaWeights[i, 0] + LearningRate * Neurons[i].Derivative * gr_sums[i];
                    else
                        deltaw = Momentum * LastDeltaWeights[i, n] + LearningRate * Neurons[i].Inputs[n - 1] * Neurons[i].Derivative * gr_sums[i];
                    LastDeltaWeights[i, n] = deltaw;
                    Neurons[i].Weights[n] += deltaw;
                }

            if (!save) Neurons = temp;
            return gr_sum;


        }


    }
}
