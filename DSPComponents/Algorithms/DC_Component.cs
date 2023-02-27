using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class DC_Component : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            float mean = 0;
            int number_of_samples = 0;
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                mean += InputSignal.Samples[i];
                number_of_samples = i + 1;
            }
            mean = mean / number_of_samples;
            List<float> Output = new List<float>();
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                Output.Add(InputSignal.Samples[i] - mean);
            }
            OutputSignal = new Signal(Output, true);
        }
    }
}
