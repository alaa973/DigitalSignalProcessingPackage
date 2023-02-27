using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Folder : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputFoldedSignal { get; set; }
        public override void Run()
        {             
            OutputFoldedSignal = new Signal(new List<float>(), new List<int>(), false, new List<float>(),
                new List<float>(), new List<float>());
            if(InputSignal.Frequencies?.Count > 0)
            {
                OutputFoldedSignal.Frequencies.Add(0);
                OutputFoldedSignal.Frequencies[0] = ++InputSignal.Frequencies[0];
            }
            else
            {
                OutputFoldedSignal.Frequencies.Add(0);
                OutputFoldedSignal.Frequencies[0]++;

            }
            //-3 -2 -1 0 1
            //-1 0 1 2 3
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                OutputFoldedSignal.SamplesIndices.Add(-1 * InputSignal.SamplesIndices[i]);
            }
            OutputFoldedSignal.SamplesIndices.Sort();
           
            for (int i = InputSignal.Samples.Count - 1; i >= 0; i--)
            {
                OutputFoldedSignal.Samples.Add(InputSignal.Samples[i]);
            }
        }
    }
}
