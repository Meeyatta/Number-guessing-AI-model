using Neyro.NeyroNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Neyro
{
    public partial class Form1 : Form
    {
        public double[] inputPixels;
        private Network network;

        public Form1()
        {
            InitializeComponent();

            inputPixels = new double[15];

            network = new Network();
        }

        private void Changing_State_Pixel_Button_Click(object sender, EventArgs e)
        {
            if (((Button)sender).BackColor == Color.White)
            {
                ((Button)sender).BackColor = Color.Black;
                inputPixels[((Button)sender).TabIndex] = 1d;
            }
            else
            {
                ((Button)sender).BackColor = Color.White;
                inputPixels[((Button)sender).TabIndex] = 0d;
            }
        }

        private void button_ClearField_Click(object sender, EventArgs e)
        {
            foreach (Control control in this.Controls)
            {
                if (control is Button button && button.TabIndex >= 0 && button.TabIndex < inputPixels.Length)
                {
                    button.BackColor = Color.White;
                    inputPixels[button.TabIndex] = 0d;
                }
            }
        }

        private void buttonRecognize_Click(object sender, EventArgs e)
        { 
            network.ForwardPass(network, inputPixels);
            labelOutput.Text = network.Fact.ToList().IndexOf(network.Fact.Max()).ToString();
            labelProbability.Text = (100 * network.Fact.Max()).ToString("0.00") + " %";
        }

        private void buttonTraining_Click(object sender, EventArgs e)
        {
            chart_Eavr.Series[0].Points.Clear();
            chartacc.Series[0].Points.Clear();

            network.Train(network);
            for (int i = 0; i < network.E_error_avr.Length; i++)
                chart_Eavr.Series[0].Points.AddY(network.E_error_avr[i]);
            for (int i = 0; i < network.Accuracy.Count(); i++)
                chartacc.Series[0].Points.AddY(100 * network.Accuracy[i]);
            Debug.WriteLine(network.Fact);
            MessageBox.Show("Training succesful", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void chart_Eavr_Click(object sender, EventArgs e)
        {

        }

        private void button_SaveTrainSample_Click(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "train.txt";
            string tmpStr = numericUpDown_NecessaryOutput.Value.ToString();

            for (int i = 0; i < inputPixels.Length; i++)
            {
                tmpStr += " " + inputPixels[i].ToString();
            }
            tmpStr += '\n';

            File.AppendAllText(path, tmpStr);
        }

        private void button_Save_Test_Sample_Click(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "test.txt";
            string tmpStr = numericUpDown_NecessaryOutput.Value.ToString();

            for (int i = 0; i < inputPixels.Length; i++)
            {
                tmpStr += " " + inputPixels[i].ToString();
            }
            tmpStr += '\n';

            File.AppendAllText(path, tmpStr);
        }

        private void button_test(object sender, EventArgs e)
        {
            chart_Eavr.Series[0].Points.Clear();
            network.Test(network);
            for (int i = 0; i < network.E_error_avr.Length; i++)
                chart_Eavr.Series[0].Points.AddY(network.E_error_avr[i]);

            MessageBox.Show("Testing succesful", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button_null(object sender, EventArgs e)
        {
            string pathDirWeights = AppDomain.CurrentDomain.BaseDirectory;
            int totalNulled = 0;
            
            string pathFileWeights = pathDirWeights + "hidden_layer1memory.csv";
            if (!File.Exists(pathFileWeights)) return;
            string[] tmpStrWeights = File.ReadAllLines(pathFileWeights);
            totalNulled += NullWeights(tmpStrWeights, pathFileWeights);

            pathFileWeights = pathDirWeights + "hidden_layer2memory.csv";
            if (!File.Exists(pathFileWeights)) return;
            tmpStrWeights = File.ReadAllLines(pathFileWeights);
            totalNulled += NullWeights(tmpStrWeights, pathFileWeights);

            pathFileWeights = pathDirWeights + "output_layermemory.csv";
            if (!File.Exists(pathFileWeights)) return;
            tmpStrWeights = File.ReadAllLines(pathFileWeights);
            totalNulled += NullWeights(tmpStrWeights, pathFileWeights);

            MessageBox.Show("Nulled a total of " + totalNulled + " weights", "Nulled weights", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        int NullWeights(string[] ogWeights, string path)
        {
            string[] newWeights = new string[ogWeights.Count()];
            int totalNulled = 0;
            Random random = new Random();
            for (int i = 0; i < ogWeights.Count(); i++)
            {
                string[] line_split = ogWeights[i].Split(';');
                int length = line_split.Count();
                if (length < 3) continue;

                int nulledCount = random.Next(length/3, length/2);
                while (nulledCount > 0)
                {
                    int nullout = random.Next(0, length - 1);
                    double nulled = double.Parse(line_split[nullout]);
                    while (nulled == 0) { nullout = random.Next(0, length - 1); nulled = double.Parse(line_split[nullout]); }
                    line_split[nullout] = "0";
                    nulledCount--;
                    totalNulled++;
                }
                string line_joined = string.Join(";", line_split);
                newWeights[i] = line_joined;
            }

            File.WriteAllLines(path, newWeights);
            return totalNulled;
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }
    }

}
