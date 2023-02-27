using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FastConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }

        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {
            int maxIndex;
            OutputConvolvedSignal = new Signal(new List<float>(), false, new List<float>(), new List<float>(), new List<float>());
            if (InputSignal1.Samples.Count != InputSignal2.Samples.Count)
            {
                maxIndex = InputSignal1.Samples.Count + InputSignal2.Samples.Count - 1;
                while (InputSignal1.Samples.Count != maxIndex)
                {
                    InputSignal1.Samples.Add(0);
                }
                while (InputSignal2.Samples.Count != maxIndex)
                {
                    InputSignal2.Samples.Add(0);
                }
            }
            maxIndex = InputSignal1.Samples.Count;

            DiscreteFourierTransform DFT1 = new DiscreteFourierTransform();
            DFT1.InputTimeDomainSignal = new DSPAlgorithms.DataStructures.Signal(InputSignal1.Samples, false);
            DFT1.Run();

            DiscreteFourierTransform DFT2 = new DiscreteFourierTransform();
            DFT2.InputTimeDomainSignal = new DSPAlgorithms.DataStructures.Signal(InputSignal2.Samples, false);
            DFT2.Run();

            List<float> Amplitudes = new List<float>();
            List<float> PhaseShifts = new List<float>();
            OutputConvolvedSignal.SamplesIndices = InputSignal2.SamplesIndices;
            Complex temp;
            Complex x;
            Complex y;

            for (int j = 0; j < maxIndex; j++)
            {
                x = Complex.FromPolarCoordinates(DFT1.OutputFreqDomainSignal.FrequenciesAmplitudes[j], DFT1.OutputFreqDomainSignal.FrequenciesPhaseShifts[j]);
                y = Complex.FromPolarCoordinates(DFT2.OutputFreqDomainSignal.FrequenciesAmplitudes[j], DFT2.OutputFreqDomainSignal.FrequenciesPhaseShifts[j]);
                temp = Complex.Multiply(x, y);
                Amplitudes.Add((float)temp.Magnitude);
                PhaseShifts.Add((float)temp.Phase);
            }

            InverseDiscreteFourierTransform IDFT = new InverseDiscreteFourierTransform();
            List<float> frequencies = new List<float>();
            IDFT.InputFreqDomainSignal = new DSPAlgorithms.DataStructures.Signal(true, frequencies, Amplitudes, PhaseShifts);
            IDFT.Run();
            List<float> output = new List<float>();
            float rounded;
            for (int i = 0; i < IDFT.OutputTimeDomainSignal.Samples.Count; i++)
            {

                rounded = (float)Math.Round(IDFT.OutputTimeDomainSignal.Samples[i] * 100f) / 100f;
                OutputConvolvedSignal.Samples.Add(IDFT.OutputTimeDomainSignal.Samples[i]);
            }

        }
    }
}
