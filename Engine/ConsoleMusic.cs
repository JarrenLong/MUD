using NAudio.Wave;
using System;
using System.Threading;
using System.Collections.Generic;

namespace MUD
{
  public enum Note
  {
    Rest = 0,
    C0 = 16,
    CSharp0 = 17,
    D0 = 18,
    Dsharp0 = 19,
    E0 = 21,
    F0 = 22,
    Fsharp0 = 23,
    G0 = 25,
    Gsharp0 = 26,
    A0 = 28,
    Asharp0 = 29,
    B0 = 31,
    C1 = 33,
    CSharp1 = 35,
    D1 = 37,
    Dsharp1 = 39,
    E1 = 41,
    F1 = 44,
    Fsharp1 = 46,
    G1 = 49,
    Gsharp1 = 52,
    A1 = 55,
    Asharp1 = 58,
    B1 = 62,
    C2 = 65,
    CSharp2 = 69,
    D2 = 73,
    Dsharp2 = 78,
    E2 = 82,
    F2 = 87,
    Fsharp2 = 93,
    G2 = 98,
    Gsharp2 = 104,
    A2 = 110,
    Asharp2 = 117,
    B2 = 123,
    C3 = 131,
    CSharp3 = 139,
    D3 = 147,
    Dsharp3 = 156,
    E3 = 165,
    F3 = 175,
    Fsharp3 = 185,
    G3 = 196,
    Gsharp3 = 208,
    A3 = 220,
    Asharp3 = 233,
    B3 = 247,
    C4 = 262,
    CSharp4 = 277,
    D4 = 294,
    Dsharp4 = 311,
    E4 = 330,
    F4 = 349,
    Fsharp4 = 370,
    G4 = 392,
    Gsharp4 = 415,
    A4 = 440,
    Asharp4 = 466,
    B4 = 494,
    C5 = 523,
    CSharp5 = 554,
    D5 = 587,
    Dsharp5 = 622,
    E5 = 659,
    F5 = 698,
    Fsharp5 = 740,
    G5 = 784,
    Gsharp5 = 831,
    A5 = 880,
    Asharp5 = 932,
    B5 = 988,
    C6 = 1047,
    CSharp6 = 1109,
    D6 = 1175,
    Dsharp6 = 1245,
    E6 = 1319,
    F6 = 1397,
    Fsharp6 = 1480,
    G6 = 1568,
    Gsharp6 = 1661,
    A6 = 1760,
    Asharp6 = 1865,
    B6 = 1976,
    C7 = 2093,
    CSharp7 = 2217,
    D7 = 2349,
    Dsharp7 = 2489,
    E7 = 2637,
    F7 = 2794,
    Fsharp7 = 2960,
    G7 = 3136,
    Gsharp7 = 3322,
    A7 = 3520,
    Asharp7 = 3729,
    B7 = 3951,
    C8 = 4186,
    CSharp8 = 4435,
    D8 = 4699,
    Dsharp8 = 4978,
    E8 = 5274,
    F8 = 5588,
    Fsharp8 = 5920,
    G8 = 6272,
    Gsharp8 = 6645,
    A8 = 7040,
    Asharp8 = 7459,
    B8 = 7902
  }

  public enum NoteSpeed
  {
    None = 0,
    Sixteenth = 1,
    Eighth = 2,
    Quarter = 4,
    Half = 8,
    Whole = 16,
  }

  public class ConsoleMusic : IDisposable
  {
    private readonly Dictionary<Note, float> NoteFrequencies = new Dictionary<Note, float>
    {
      { Note.Rest, 0.0f },
      { Note.C0, 16.35f },
      { Note.CSharp0, 17.32f },
      { Note.D0, 18.35f },
      { Note.Dsharp0, 19.45f },
      { Note.E0, 20.6f },
      { Note.F0, 21.83f },
      { Note.Fsharp0, 23.12f },
      { Note.G0, 24.5f },
      { Note.Gsharp0, 25.96f },
      { Note.A0, 27.5f },
      { Note.Asharp0, 29.14f },
      { Note.B0, 30.87f },
      { Note.C1, 32.7f },
      { Note.CSharp1, 34.65f },
      { Note.D1, 36.71f },
      { Note.Dsharp1, 38.89f },
      { Note.E1, 41.2f },
      { Note.F1, 43.65f },
      { Note.Fsharp1, 46.25f },
      { Note.G1, 49f },
      { Note.Gsharp1, 51.91f },
      { Note.A1, 55f },
      { Note.Asharp1, 58.27f },
      { Note.B1, 61.74f },
      { Note.C2, 65.41f },
      { Note.CSharp2, 69.3f },
      { Note.D2, 73.42f },
      { Note.Dsharp2, 77.78f },
      { Note.E2, 82.41f },
      { Note.F2, 87.31f },
      { Note.Fsharp2, 92.5f },
      { Note.G2, 98f },
      { Note.Gsharp2, 103.83f },
      { Note.A2, 110f },
      { Note.Asharp2, 116.54f },
      { Note.B2, 123.47f },
      { Note.C3, 130.81f },
      { Note.CSharp3, 138.59f },
      { Note.D3, 146.83f },
      { Note.Dsharp3, 155.56f },
      { Note.E3, 164.81f },
      { Note.F3, 174.61f },
      { Note.Fsharp3, 185f },
      { Note.G3, 196f },
      { Note.Gsharp3, 207.65f },
      { Note.A3, 220f },
      { Note.Asharp3, 233.08f },
      { Note.B3, 246.94f },
      { Note.C4, 261.63f },
      { Note.CSharp4, 277.18f },
      { Note.D4, 293.66f },
      { Note.Dsharp4, 311.13f },
      { Note.E4, 329.63f },
      { Note.F4, 349.23f },
      { Note.Fsharp4, 369.99f },
      { Note.G4, 392f },
      { Note.Gsharp4, 415.3f },
      { Note.A4, 440f },
      { Note.Asharp4, 466.16f },
      { Note.B4, 493.88f },
      { Note.C5, 523.25f },
      { Note.CSharp5, 554.37f },
      { Note.D5, 587.33f },
      { Note.Dsharp5, 622.25f },
      { Note.E5, 659.25f },
      { Note.F5, 698.46f },
      { Note.Fsharp5, 739.99f },
      { Note.G5, 783.99f },
      { Note.Gsharp5, 830.61f },
      { Note.A5, 880f },
      { Note.Asharp5, 932.33f },
      { Note.B5, 987.77f },
      { Note.C6, 1046.5f },
      { Note.CSharp6, 1108.73f },
      { Note.D6, 1174.66f },
      { Note.Dsharp6, 1244.51f },
      { Note.E6, 1318.51f },
      { Note.F6, 1396.91f },
      { Note.Fsharp6, 1479.98f },
      { Note.G6, 1567.98f },
      { Note.Gsharp6, 1661.22f },
      { Note.A6, 1760f },
      { Note.Asharp6, 1864.66f },
      { Note.B6, 1975.53f },
      { Note.C7, 2093f },
      { Note.CSharp7, 2217.46f },
      { Note.D7, 2349.32f },
      { Note.Dsharp7, 2489.02f },
      { Note.E7, 2637.02f },
      { Note.F7, 2793.83f },
      { Note.Fsharp7, 2959.96f },
      { Note.G7, 3135.96f },
      { Note.Gsharp7, 3322.44f },
      { Note.A7, 3520f },
      { Note.Asharp7, 3729.31f },
      { Note.B7, 3951.07f },
      { Note.C8, 4186.01f },
      { Note.CSharp8, 4434.92f },
      { Note.D8, 4698.63f },
      { Note.Dsharp8, 4978.03f },
      { Note.E8, 5274.04f },
      { Note.F8, 5587.65f },
      { Note.Fsharp8, 5919.91f },
      { Note.G8, 6271.93f },
      { Note.Gsharp8, 6644.88f },
      { Note.A8, 7040f },
      { Note.Asharp8, 7458.62f },
      { Note.B8, 7902.13f }
    };

    public double BeatsPerMinute { get; set; } = 60;
    public int BeatsPerMeasure { get; set; } = 4;
    public NoteSpeed WhoGetsThebeat { get; set; } = NoteSpeed.Quarter;
    public float Volume
    {
      get { return sineWaveProvider.Amplitude; }
      set { sineWaveProvider.Amplitude = value; }
    }

    public void Play(Note n, NoteSpeed s)
    {
      double time = (1000 / (BeatsPerMinute / 60)) * ((double)s / (double)WhoGetsThebeat);

      sineWaveProvider.Frequency = NoteFrequencies[n];
      Thread.Sleep((int)time);

      /// 16th note rest
      sineWaveProvider.Frequency = 0;
      time = (1000 / (BeatsPerMinute / 60)) * ((double)NoteSpeed.Sixteenth / (double)WhoGetsThebeat);
      Thread.Sleep((int)time);
    }

    internal class SineWaveProvider32 : WaveProvider32
    {
      int sample;

      public SineWaveProvider32()
      {
        Frequency = 0;
        Amplitude = 0.1f; // Let's not hurt our ears
      }

      public float Frequency { get; set; }
      public float Amplitude { get; set; }

      public override int Read(float[] buffer, int offset, int sampleCount)
      {
        int sampleRate = WaveFormat.SampleRate;
        for (int n = 0; n < sampleCount; n++)
        {
          buffer[n + offset] = (float)(Amplitude * Math.Sin((2 * Math.PI * sample * Frequency) / sampleRate));
          sample++;
          if (sample >= sampleRate)
            sample = 0;
        }

        return sampleCount;
      }
    }

    private WaveOut waveOut = new WaveOut(WaveCallbackInfo.FunctionCallback());
    private SineWaveProvider32 sineWaveProvider = new SineWaveProvider32();

    public ConsoleMusic()
    {
      sineWaveProvider = new SineWaveProvider32();
      sineWaveProvider.Amplitude = 0.1f;
      sineWaveProvider.Frequency = 0;

      waveOut = new WaveOut(WaveCallbackInfo.FunctionCallback());
      waveOut.Init(sineWaveProvider);
      waveOut.Play();
    }

    public void Dispose()
    {
      StopSound();

      sineWaveProvider = null;
    }
    public void StopSound()
    {
      if (waveOut != null)
      {
        waveOut.Stop();
        waveOut.Dispose();
        waveOut = null;
      }
    }

    public void Test()
    {
      ThreadPool.QueueUserWorkItem((x) =>
      {
        foreach (Note n in Enum.GetValues(typeof(Note)))
          Play(n, NoteSpeed.Half);

        StopSound();
      });
    }

    public void Test2()
    {
      ThreadPool.QueueUserWorkItem((x) =>
      {
        BeatsPerMinute = 80;

        // Mary Had A Little Lamb
        Play(Note.B4, NoteSpeed.Quarter);
        Play(Note.A4, NoteSpeed.Quarter);
        Play(Note.Gsharp4, NoteSpeed.Quarter);
        Play(Note.A4, NoteSpeed.Quarter);
        Play(Note.B4, NoteSpeed.Quarter);
        Play(Note.B4, NoteSpeed.Quarter);
        Play(Note.B4, NoteSpeed.Half);
        Play(Note.Rest, NoteSpeed.Quarter);
        Play(Note.A4, NoteSpeed.Quarter);
        Play(Note.A4, NoteSpeed.Quarter);
        Play(Note.A4, NoteSpeed.Half);
        Play(Note.Rest, NoteSpeed.Quarter);
        Play(Note.B4, NoteSpeed.Quarter);
        Play(Note.Fsharp4, NoteSpeed.Quarter);
        Play(Note.Fsharp4, NoteSpeed.Half);
        Play(Note.Rest, NoteSpeed.Half);

        StopSound();
      });
    }

    public void Test3()
    {
      ThreadPool.QueueUserWorkItem((x) =>
      {
        // Super Mario Bros - Castle Theme intro
        BeatsPerMinute = 200;

        Play(Note.E4, NoteSpeed.Eighth);
        Play(Note.E4, NoteSpeed.Eighth);
        Play(Note.Rest, NoteSpeed.Eighth);
        Play(Note.E4, NoteSpeed.Eighth);
        Play(Note.Rest, NoteSpeed.Eighth);
        Play(Note.C4, NoteSpeed.Eighth);
        Play(Note.E4, NoteSpeed.Eighth);
        Play(Note.Rest, NoteSpeed.Eighth);
        Play(Note.G4, NoteSpeed.Eighth);
        Play(Note.Rest, NoteSpeed.Half);
        Play(Note.G3, NoteSpeed.Quarter);
        Play(Note.Rest, NoteSpeed.Half);
        Play(Note.Rest, NoteSpeed.Quarter);

        //Play(Note.D4, NoteSpeed.Eighth);
        //Play(Note.Rest, NoteSpeed.Quarter);
        //Play(Note.B4, NoteSpeed.Eighth);
        //Play(Note.Rest, NoteSpeed.Quarter);
        //Play(Note.G3, NoteSpeed.Eighth);
        //Play(Note.Rest, NoteSpeed.Quarter);

        StopSound();
      });
    }
  }
}
