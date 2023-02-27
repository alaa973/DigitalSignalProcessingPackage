using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FIR : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public FILTER_TYPES InputFilterType { get; set; }
        public float InputFS { get; set; }
        public float? InputCutOffFrequency { get; set; }
        public float? InputF1 { get; set; }
        public float? InputF2 { get; set; }
        public float InputStopBandAttenuation { get; set; }
        public float InputTransitionBand { get; set; }
        public Signal OutputHn { get; set; }
        public Signal OutputYn { get; set; }

        public override void Run()
        {
            int N = 0;
            List<float> W = new List<float>();
            //OutputHn = new Signal(new List<float>(), false, new List<float>(), new List<float>(), new List<float>());
            OutputHn = new Signal(new List<float>(), new List<int>(), false);
            OutputYn = new Signal(new List<float>(), new List<int>(), false);
            if (InputStopBandAttenuation <= 21)
            {
                N = (int)Math.Ceiling((float)(0.9 / ((float)InputTransitionBand / InputFS)));
                if (N % 2 == 0)
                {
                    N++;
                }
                for (int i = -N / 2; i <= N/2; i++)
                {
                    W.Add(1);
                }
            }
            else if (InputStopBandAttenuation <= 44)
            {
                N = (int)Math.Ceiling((float)(3.1 / (InputTransitionBand / InputFS)));
                if (N % 2 == 0)
                {
                    N++;
                }
                for (int i = -N / 2; i <= N / 2; i++)
                {
                    W.Add((float)(0.5 + 0.5 * Math.Cos(2 * Math.PI * i / N)));
                }
            }
            else if (InputStopBandAttenuation <= 53)
            {
                N = (int)Math.Ceiling((float)(3.3 / (InputTransitionBand / InputFS)));
                if (N % 2 == 0)
                {
                    N++;
                }
                for (int i = -N / 2; i <= N / 2; i++)
                {
                    W.Add((float)(0.54 + 0.46 * Math.Cos(2 * Math.PI * i / N)));
                }
            }
            else if (InputStopBandAttenuation <= 74)
            {
                N = (int)Math.Ceiling((float)(5.5 / (InputTransitionBand / InputFS)));
                if (N % 2 == 0)
                {
                    N++;
                }
                for (int i = -N / 2; i <= N / 2; i++)
                {
                    W.Add((float)(0.42 + 0.5 * Math.Cos(2 * Math.PI * i / (N - 1))) +
                        (float)(0.08 * Math.Cos(4 * Math.PI * i / (N - 1))));
                }
            }
            //OutputYn = new Signal(new List<float>(), false, new List<float>(), new List<float>(), new List<float>()); 
            if (InputFilterType == FILTER_TYPES.HIGH)
            {
                InputCutOffFrequency -= (float?)0.5 * InputTransitionBand;
                //0.21875
                InputCutOffFrequency /= InputFS;
                int j = 0;
                for (int i = -N / 2; i <= N / 2; i++) 
                {
                    if(i == 0)
                    {
                        OutputHn.Samples.Add((float)(1 - 2 * InputCutOffFrequency));
                        OutputHn.SamplesIndices.Add(i);
                        continue;
                    }
                    OutputHn.SamplesIndices.Add(i);
                    OutputYn.SamplesIndices.Add(i);
                    OutputHn.Samples.Add((float)(-1 * 2 *InputCutOffFrequency *Math.Sin((double)(i * 2 * Math.PI * InputCutOffFrequency)) /
                        (i * 2 * Math.PI * InputCutOffFrequency)));
                    OutputHn.Samples[OutputHn.Samples.Count - 1] = W[i + N/2] * OutputHn.Samples[OutputHn.Samples.Count - 1]; 
                    j++;
                }
            }
            else if (InputFilterType == FILTER_TYPES.LOW)
            {
                InputCutOffFrequency += (float?)0.5 * InputTransitionBand;
                InputCutOffFrequency /= InputFS;
                
                for (int i = -N / 2; i <= N/2; i++)
                {
                    if (i == 0)
                    {
                        OutputHn.Samples.Add((float)(2 * InputCutOffFrequency));
                        OutputHn.SamplesIndices.Add(i);
                        continue;
                    }
                    OutputHn.SamplesIndices.Add(i);
                    OutputYn.SamplesIndices.Add(i);
                    OutputHn.Samples.Add((float)(2 * InputCutOffFrequency * Math.Sin((double)(i * 2 * Math.PI * InputCutOffFrequency)) /
                        (i * 2 * Math.PI * InputCutOffFrequency)));
                    OutputHn.Samples[OutputHn.Samples.Count - 1] = W[i + N/2] * OutputHn.Samples[OutputHn.Samples.Count - 1];
                    
                }
            }
            else if (InputFilterType == FILTER_TYPES.BAND_PASS)
            {
                InputF1 -= (float?)0.5 * InputTransitionBand;
                InputF2 += (float?)0.5 * InputTransitionBand;

                InputF1 /= InputFS;
                InputF2 /= InputFS;
                int j = 0;
                for (int i = -N / 2; i <= N/2; i++)
                {
                    if (i == 0)
                    {
                        OutputHn.Samples.Add((float)(2 * (InputF2 - InputF1)));
                        OutputHn.SamplesIndices.Add(i);
                        continue;
                    }
                    OutputHn.SamplesIndices.Add(i);
                    OutputYn.SamplesIndices.Add(i);
                    OutputHn.Samples.Add((float)((2 * InputF2 * Math.Sin((double)(i * 2 * Math.PI * InputF2)) /
                        (i * 2 * Math.PI * InputF2)) + (-1 * 2 * InputF1 * Math.Sin((double)(i * 2 * Math.PI * InputF1)) /
                        (i * 2 * Math.PI * InputF1))));
                    OutputHn.Samples[OutputHn.Samples.Count - 1] = W[i + N/2] * OutputHn.Samples[OutputHn.Samples.Count - 1];
                    j++;
                }
            }
            else if (InputFilterType == FILTER_TYPES.BAND_STOP)
            {
                InputF1 += (float?)0.5 * InputTransitionBand;
                InputF2 -= (float?)0.5 * InputTransitionBand;

                InputF1 /= InputFS;
                InputF2 /= InputFS;
                int j = 0;
                for (int i = -N / 2; i <= N/2; i++)
                {
                    if (i == 0)
                    {
                        OutputHn.Samples.Add((float)(1 - 2 * (InputF2 - InputF1)));
                        OutputHn.SamplesIndices.Add(i);
                        continue;
                    }
                    OutputHn.SamplesIndices.Add(i);
                    OutputYn.SamplesIndices.Add(i);
                    OutputHn.Samples.Add((float)((2 * InputF1 * Math.Sin((double)(i * 2 * Math.PI * InputF1)) /
                        (i * 2 * Math.PI * InputF1)) + (-1 * 2 * InputF2 * Math.Sin((double)(i * 2 * Math.PI * InputF2)) /
                        (i * 2 * Math.PI * InputF2))));
                    OutputHn.Samples[OutputHn.Samples.Count - 1] = W[i + N/2] * OutputHn.Samples[OutputHn.Samples.Count - 1];
                    j++;
                }
            }
            /*for (int i = 1; i <= N / 2; i++)
            {
                OutputHn.SamplesIndices.Add(i);
                OutputYn.SamplesIndices.Add(i); 
                OutputHn.Samples.Add(OutputHn.Samples[OutputHn.SamplesIndices.IndexOf(-i)]);
            }*/
            DirectConvolution c = new DirectConvolution();
            c.InputSignal1 = InputTimeDomainSignal;
            c.InputSignal2 = OutputHn;
            c.Run();

            OutputYn = c.OutputConvolvedSignal;

        }
    }
}
