using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FastCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }

        public override void Run()
        {
            OutputNonNormalizedCorrelation = new List<float>();
            OutputNormalizedCorrelation = new List<float>();

            DiscreteFourierTransform DFT1 = new DiscreteFourierTransform();
            DFT1.InputTimeDomainSignal = new DSPAlgorithms.DataStructures.Signal(InputSignal1.Samples, false);
            DFT1.Run();

            DiscreteFourierTransform DFT2 = new DiscreteFourierTransform();
            if (InputSignal2 == null)
            {
                InputSignal2 = InputSignal1;
            }
            DFT2.InputTimeDomainSignal = new DSPAlgorithms.DataStructures.Signal(InputSignal2.Samples, false);
            DFT2.Run();

            List<float> Amplitudes = new List<float>();
            List<float> PhaseShifts = new List<float>();
            Complex temp;
            Complex x;
            Complex y;

            for (int j = 0; j < InputSignal1.Samples.Count; j++)
            {
                x = Complex.FromPolarCoordinates(DFT1.OutputFreqDomainSignal.FrequenciesAmplitudes[j], DFT1.OutputFreqDomainSignal.FrequenciesPhaseShifts[j]);
                y = Complex.FromPolarCoordinates(DFT2.OutputFreqDomainSignal.FrequenciesAmplitudes[j], DFT2.OutputFreqDomainSignal.FrequenciesPhaseShifts[j]);
                temp = Complex.Multiply(Complex.Conjugate(x), y);
                Amplitudes.Add((float)temp.Magnitude);
                PhaseShifts.Add((float)temp.Phase);
            }

            InverseDiscreteFourierTransform IDFT = new InverseDiscreteFourierTransform();
            List<float> frequencies = new List<float>();
            IDFT.InputFreqDomainSignal = new DSPAlgorithms.DataStructures.Signal(true, frequencies, Amplitudes, PhaseShifts);
            IDFT.Run();

            float sum1 = 0, sum2 = 0;
            for (int i = 0; i < InputSignal1.Samples.Count; i++)
            {
                sum1 += InputSignal1.Samples[i] * InputSignal1.Samples[i];
                sum2 += InputSignal2.Samples[i] * InputSignal2.Samples[i];
            }
            sum1 = sum1 * sum2;
            sum1 = (float)Math.Sqrt(sum1);
            sum1 /= InputSignal1.Samples.Count;

            float nonn, n;
            //List<float> Output = new List<float>();
            for (int i = 0; i < InputSignal1.Samples.Count; i++)
            {
                nonn = IDFT.OutputTimeDomainSignal.Samples[i] / (InputSignal1.Samples.Count);
                n = (IDFT.OutputTimeDomainSignal.Samples[i] / (InputSignal1.Samples.Count)) / sum1;
                OutputNonNormalizedCorrelation.Add(IDFT.OutputTimeDomainSignal.Samples[i] / (InputSignal1.Samples.Count));
                OutputNormalizedCorrelation.Add((IDFT.OutputTimeDomainSignal.Samples[i] / (InputSignal1.Samples.Count)) / sum1);
            }

        }
    }
}