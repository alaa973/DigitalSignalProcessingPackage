using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }

        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {
            OutputConvolvedSignal = new Signal(new List<float>(), new List<int>(), InputSignal1.Periodic);
            int minIndex = InputSignal1.SamplesIndices[0] + InputSignal2.SamplesIndices[0];
            int maxIndex = InputSignal1.SamplesIndices[InputSignal1.SamplesIndices.Count - 1] + InputSignal2.SamplesIndices[InputSignal2.SamplesIndices.Count - 1];
            int count1 = InputSignal1.SamplesIndices.Count - 1;
            int count2 = InputSignal2.SamplesIndices.Count - 1;
            for (int i = minIndex; i <= maxIndex; i++)
            {
                float sum = 0;
                for (int j = minIndex; j < InputSignal1.Samples.Count; j++)
                {
                    if ((i - j) < InputSignal2.SamplesIndices[0] || (i - j) > InputSignal2.SamplesIndices[count2]
                        || j < InputSignal1.SamplesIndices[0] || j > InputSignal1.SamplesIndices[count1])
                    {
                        continue;
                    }
                    int Hindex = InputSignal2.SamplesIndices.IndexOf(i - j);
                    int Xindex = InputSignal1.SamplesIndices.IndexOf(j);

                    sum += (float)InputSignal1.Samples[Xindex] * (float)InputSignal2.Samples[Hindex];
                }
                //if (i == maxIndex && sum == 0)
                //{
                //    continue;
                //}
                OutputConvolvedSignal.SamplesIndices.Add(i);
                OutputConvolvedSignal.Samples.Add(sum);
            }
        }
    }
}