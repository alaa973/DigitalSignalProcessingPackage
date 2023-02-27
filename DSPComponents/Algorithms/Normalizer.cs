using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Normalizer : Algorithm
    {
        public Signal InputSignal { get; set; }
        public float InputMinRange { get; set; }
        public float InputMaxRange { get; set; }
        public Signal OutputNormalizedSignal { get; set; }

        public override void Run()
        {
            float max , min;
            max = InputSignal.Samples.Max();
            min = InputSignal.Samples.Min();           
            List<float> l = new List<float>();
            OutputNormalizedSignal = new Signal(l, true);
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                OutputNormalizedSignal.Samples.Add
                    (InputMinRange + (((InputSignal.Samples[i] - min) * (InputMaxRange - InputMinRange)) / (max - min)));
            }
        }
    }
}
