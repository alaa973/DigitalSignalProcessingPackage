using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class MovingAverage : Algorithm
    {
        public Signal InputSignal { get; set; }
        public int InputWindowSize { get; set; }
        public Signal OutputAverageSignal { get; set; }

        public override void Run()
        {
            List<float> Output = new List<float>();

            for (int i = 0; i < InputSignal.Samples.Count - InputWindowSize + 1; i++)
            {
                Output.Add(InputSignal.Samples[i]);
                for (int j = i + 1; j < i + InputWindowSize; j++)
                {
                    Output[i] += InputSignal.Samples[j];
                }
                Output[i] = Output[i] / InputWindowSize;
            }
            OutputAverageSignal = new Signal(Output, true);
        }
    }
}
