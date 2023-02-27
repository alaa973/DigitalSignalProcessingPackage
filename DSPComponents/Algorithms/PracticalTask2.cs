using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace DSPAlgorithms.Algorithms
{
    public class PracticalTask2 : Algorithm
    {
        public String SignalPath { get; set; }
        public float Fs { get; set; }
        public float miniF { get; set; }
        public float maxF { get; set; }
        public float newFs { get; set; }
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal OutputFreqDomainSignal { get; set; }

        public override void Run()
        {
            Signal InputSignal = LoadSignal(SignalPath);
            DisplaySignal(InputSignal,"2_INPUT_SIGNAL");

            FIR FIR = new FIR();
            FIR.InputFS = Fs;
            FIR.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.BAND_PASS;
            FIR.InputStopBandAttenuation = 50;
            FIR.InputTransitionBand = 500;
            FIR.InputTimeDomainSignal = InputSignal;
            FIR.InputF1 = miniF;
            FIR.InputF2 = maxF;
            FIR.Run();

            DC_Component dc = new DC_Component();
            if (newFs < 2 * maxF)
            {
                Console.WriteLine("newFs is not valid");
                dc.InputSignal = FIR.OutputYn;
                newFs = Fs;
            }
            else
            {
                Sampling s = new Sampling();
                s.L = L;
                s.M = M;
                s.InputSignal = FIR.OutputYn;
                s.Run();
                dc.InputSignal = s.OutputSignal;
            }
            dc.Run();

            DisplaySignal(dc.OutputSignal, "2_DC_SIGNAL");

            Normalizer a = new Normalizer();
            a.InputMinRange = -1;
            a.InputMaxRange = 1;
            a.InputSignal = dc.OutputSignal;
            a.Run();

            DisplaySignal(a.OutputNormalizedSignal, "2_NORMALIZED_SIGNAL");

            DiscreteFourierTransform DFT = new DiscreteFourierTransform();
            DFT.InputTimeDomainSignal = a.OutputNormalizedSignal;
            DFT.InputSamplingFrequency = newFs;
            DFT.Run();

            DisplaySignal(DFT.OutputFreqDomainSignal, "2_DFT_SIGNAL");
            OutputFreqDomainSignal = DFT.OutputFreqDomainSignal;

        }

        public void DisplaySignal(Signal s, String fileName)
        {
            String path = "TestingSignals/"+fileName+".ds";
            using (StreamWriter writer = new StreamWriter(path))
            {
                if(s.FrequenciesAmplitudes == null)
                {
                    writer.WriteLine(0);
                }
                else
                {
                    writer.WriteLine(1);
                }
                if (s.Periodic)
                {
                    writer.WriteLine(1);
                }
                else
                {
                    writer.WriteLine(0);
                }
                if (s.FrequenciesAmplitudes == null)
                {
                    writer.WriteLine(s.Samples.Count);
                    for (int i = 0; i < s.Samples.Count; i++)
                    {
                        writer.Write(i);
                        writer.Write(" ");
                        writer.WriteLine(s.Samples[i]);
                    }
                }
                else
                {
                    writer.WriteLine(s.Frequencies.Count);
                    for (int i = 0; i < s.Frequencies.Count; i++)
                    {
                        writer.Write(s.Frequencies[i]);
                        writer.Write(" ");
                        writer.Write(s.FrequenciesAmplitudes[i]);
                        writer.Write(" ");
                        writer.WriteLine(s.FrequenciesPhaseShifts[i]);
                    }
                }
            }
        }
        public Signal LoadSignal(string filePath)
        {
            Stream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var sr = new StreamReader(stream);

            var sigType = byte.Parse(sr.ReadLine());
            var isPeriodic = byte.Parse(sr.ReadLine());
            long N1 = long.Parse(sr.ReadLine());

            List<float> SigSamples = new List<float>(unchecked((int)N1));
            List<int> SigIndices = new List<int>(unchecked((int)N1));
            List<float> SigFreq = new List<float>(unchecked((int)N1));
            List<float> SigFreqAmp = new List<float>(unchecked((int)N1));
            List<float> SigPhaseShift = new List<float>(unchecked((int)N1));

            if (sigType == 1)
            {
                SigSamples = null;
                SigIndices = null;
            }

            for (int i = 0; i < N1; i++)
            {
                if (sigType == 0 || sigType == 2)
                {
                    var timeIndex_SampleAmplitude = sr.ReadLine().Split();
                    SigIndices.Add(int.Parse(timeIndex_SampleAmplitude[0]));
                    SigSamples.Add(float.Parse(timeIndex_SampleAmplitude[1]));
                }
                else
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }

            if (!sr.EndOfStream)
            {
                long N2 = long.Parse(sr.ReadLine());

                for (int i = 0; i < N2; i++)
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }

            stream.Close();
            return new Signal(SigSamples, SigIndices, isPeriodic == 1, SigFreq, SigFreqAmp, SigPhaseShift);
        }

    }
}
