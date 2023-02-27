﻿using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Sampling : Algorithm
    {
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }
        public override void Run()
        {
            if(M == 0 && L == 0)
            {
                Console.WriteLine("ERROR: M and L can't both be zeros");
            }
            Signal input = new Signal(new List<float>(), false);
            OutputSignal = new Signal(new List<float>(), new List<int>(), false);
            Signal output = new Signal(new List<float>(), false);
            int l = InputSignal.Samples.Count;
            int idx = InputSignal.SamplesIndices[0];
            FIR fir = new FIR();
            fir.InputFilterType = FILTER_TYPES.LOW;
            fir.InputFS = 8000;
            fir.InputStopBandAttenuation = 50;
            fir.InputCutOffFrequency = 1500;
            fir.InputTransitionBand = 500;
            if (M == 0 && L != 0)
            {
                for(int i = 0; i< l; i++)
                {
                    input.Samples.Add(InputSignal.Samples[i]);
                    input.SamplesIndices.Add(idx++);
                    if(i != InputSignal.Samples.Count - 1)
                    {
                        for(int j = 0; j < L - 1; j++)
                        {
                            input.Samples.Add(0);
                            input.SamplesIndices.Add(idx++);
                        }
                    }
                }
                fir.InputTimeDomainSignal = input;
                fir.Run();
                OutputSignal = fir.OutputYn;

            }
            else if (M != 0 && L == 0)
            {

                fir.InputTimeDomainSignal = InputSignal;
                fir.Run();
                output = fir.OutputYn;
                int id = output.SamplesIndices[0];

                for (int i = 0; i < output.Samples.Count; i += M)
                {
                    OutputSignal.Samples.Add(output.Samples[i]);
                    OutputSignal.SamplesIndices.Add(id++);
                }

            }
            else if(M != 0 && L != 0)
            {
                for (int i = 0; i < l; i++)
                {
                    input.Samples.Add(InputSignal.Samples[i]);
                    input.SamplesIndices.Add(idx++);
                    if (i != InputSignal.Samples.Count - 1)
                    {
                        for (int j = 0; j < L - 1; j++)
                        {
                            input.Samples.Add(0);
                            input.SamplesIndices.Add(idx++);
                        }
                    }
                }
                fir.InputTimeDomainSignal = input;
                fir.Run();
                output = fir.OutputYn;
                int id = output.SamplesIndices[0];
                for(int i = 0; i< output.Samples.Count; i+=M)
                {
                    OutputSignal.Samples.Add(output.Samples[i]);
                    OutputSignal.SamplesIndices.Add(id++);
                }
            }

        }
    }

}