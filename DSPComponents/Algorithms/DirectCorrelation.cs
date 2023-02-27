using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }

        public override void Run()
        {
            int len = InputSignal1.Samples.Count;
            
            if (!(InputSignal2?.Samples.Count > 0))
            {                
                InputSignal2 = new Signal(new List<float>(InputSignal1.Samples), InputSignal1.Periodic);
            }
            OutputNonNormalizedCorrelation = new List<float>();
            OutputNormalizedCorrelation = new List<float>();
            float sum1 = 0, sum2 = 0;
            for(int i = 0; i < len; i++)
            {
                sum1 += InputSignal1.Samples[i] * InputSignal1.Samples[i];
                sum2 += InputSignal2.Samples[i] * InputSignal2.Samples[i];
            }
            sum1 = sum1 * sum2;
            sum1 = (float)Math.Sqrt(sum1);
            sum1 /= len;
            for (int j = 0; j < len; j++)
            {
                float sum = 0;
                for(int i = 0; i < len; i++)
                {
                     sum += InputSignal1.Samples[i] * InputSignal2.Samples[i + j];                  
                }               
                if (InputSignal2.Periodic)
                {
                    InputSignal2.Samples.Add(InputSignal2.Samples[j]);
                }
                else
                {
                    InputSignal2.Samples.Add(0);
                }
                OutputNonNormalizedCorrelation
                        .Add(sum/len);
                OutputNormalizedCorrelation.Add((sum / len) / sum1);
            }
        }
    }
}