using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class QuantizationAndEncoding : Algorithm
    {
        // You will have only one of (InputLevel or InputNumBits), the other property will take a negative value
        // If InputNumBits is given, you need to calculate and set InputLevel value and vice versa
        public int InputLevel { get; set; }
        public int InputNumBits { get; set; }
        public Signal InputSignal { get; set; }
        public Signal OutputQuantizedSignal { get; set; }
        public List<int> OutputIntervalIndices { get; set; }
        public List<string> OutputEncodedSignal { get; set; }
        public List<float> OutputSamplesError { get; set; }

        public override void Run()
        {
            OutputIntervalIndices = new List<int>();
            OutputEncodedSignal = new List<string>();
            OutputSamplesError = new List<float>();
            if (InputLevel <= 0)
            {
                InputLevel = (int)Math.Pow(2, InputNumBits);
            }
            float max, min, range, start, end;
            max = InputSignal.Samples.Max();
            min = InputSignal.Samples.Min();
            range = (max - min) / InputLevel;
            List<float> MidPoints = new List<float>();
            start = min;
            start = (float)Math.Round(start, 3);
            end = start + range;
            end = (float)Math.Round(end, 3);
            List<Tuple<float, float>> ranges = new List<Tuple<float, float>>();
            for (int i = 0; i < InputLevel; i ++)
            {
                ranges.Add(Tuple.Create(start, end));
                MidPoints.Add((start + end) / 2);
                start = end;
                start = (float)Math.Round(start, 3);
                end = end + range;
                end = (float)Math.Round(end, 3);

            }
            List<float> output = new List<float>();
            OutputQuantizedSignal = new Signal(output, false);
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                string binary = string.Empty;
                for(int j = 0; j < ranges.Count; j++)
                {
                    if(InputSignal.Samples[i] >= ranges[j].Item1 && InputSignal.Samples[i] <= ranges[j].Item2)
                    {
                        OutputQuantizedSignal.Samples.Add((ranges[j].Item1+ ranges[j].Item2)/2);
                        OutputSamplesError.Add(OutputQuantizedSignal.Samples[i] - InputSignal.Samples[i]);
                        OutputIntervalIndices.Add(j + 1);
                        binary = Convert.ToString(j, 2);
                        break;
                    }
                }
                while (binary.Length < Math.Log(InputLevel, 2)) 
                {
                    binary = '0' + binary;
                   
                }
                OutputEncodedSignal.Add(binary);
            }
        }
    }
}
