using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Shifter : Algorithm
    {
        public Signal InputSignal { get; set; }
        public int ShiftingValue { get; set; }
        public Signal OutputShiftedSignal { get; set; }

        public override void Run()
        {
            OutputShiftedSignal = new Signal(new List<float>(), new List<int>(), false, new List<float>(),
                new List<float>(), new List<float>());
            if(InputSignal.Frequencies?.Count > 0)
            {
                OutputShiftedSignal.Frequencies.Add(0);
                OutputShiftedSignal.Frequencies[0] = InputSignal.Frequencies[0];
                if (InputSignal.Frequencies[0] % 2 != 0)
                {
                    ShiftingValue = -1 * ShiftingValue;

                }
            }
            //+ shift left - shift right
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                OutputShiftedSignal.Samples.Add(InputSignal.Samples[i]);
                OutputShiftedSignal.SamplesIndices.Add(InputSignal.SamplesIndices[i] - ShiftingValue);
            }            
        }
    }
}
