using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class InverseDiscreteFourierTransform : Algorithm
    {
        public Signal InputFreqDomainSignal { get; set; }
        public Signal OutputTimeDomainSignal { get; set; }

        public override void Run()
        {
            List<float> freq = new List<float>();
            List<float> samples = new List<float>();
            List<float> freqAmp = new List<float>();
            List<float> phase = new List<float>();
            OutputTimeDomainSignal = new Signal(samples, false, freq, freqAmp, phase);

            Complex sum = 0;
            for (int i = 0; i < InputFreqDomainSignal.FrequenciesAmplitudes.Count; i++)
            {
                sum = 0;
                for (int j = 0; j < InputFreqDomainSignal.FrequenciesAmplitudes.Count; j++)
                {
                    float num = (float)(i * j * 2 * Math.PI / InputFreqDomainSignal.FrequenciesAmplitudes.Count);
                    Complex x = Complex.FromPolarCoordinates(InputFreqDomainSignal.FrequenciesAmplitudes[j], InputFreqDomainSignal.FrequenciesPhaseShifts[j]);
                    Complex c = new Complex(Math.Cos(num), Math.Sin(num));
                    sum += Complex.Multiply(c, x);
                }
                OutputTimeDomainSignal.Samples.Add((float)sum.Real / InputFreqDomainSignal.FrequenciesAmplitudes.Count);
            }
        }
    }
}
