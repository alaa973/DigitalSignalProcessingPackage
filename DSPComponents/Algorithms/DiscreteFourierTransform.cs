using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DiscreteFourierTransform : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public float InputSamplingFrequency { get; set; }
        public Signal OutputFreqDomainSignal { get; set; }

        public override void Run()
        {
            List<float> freq = new List<float>();
            List<float> samples = new List<float>();
            List<float> freqAmp = new List<float>();
            List<float> phase = new List<float>();
            OutputFreqDomainSignal = new Signal(samples, false, freq, freqAmp, phase);
            Complex sum = 0;
            float samplingFrequency;
            for (int i = 0; i < InputTimeDomainSignal.Samples.Count; i++)
            {
                sum = 0;
                for (int j = 0; j < InputTimeDomainSignal.Samples.Count; j++)
                {
                    double num = (i * j * 2 * Math.PI / InputTimeDomainSignal.Samples.Count);
                    Complex c = new Complex(Math.Cos(num) * InputTimeDomainSignal.Samples[j],
                    -1 * InputTimeDomainSignal.Samples[j] * Math.Sin(num));
                    sum += c;
                }
                samplingFrequency = (float)(2 * Math.PI * InputSamplingFrequency / InputTimeDomainSignal.Samples.Count)*i;
                OutputFreqDomainSignal.Frequencies.Add((float)Math.Round(samplingFrequency, 1));
                OutputFreqDomainSignal.FrequenciesAmplitudes.Add((float)sum.Magnitude);
                OutputFreqDomainSignal.FrequenciesPhaseShifts.Add((float)sum.Phase);
                
            }
        }
    }
}
