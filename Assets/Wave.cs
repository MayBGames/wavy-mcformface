using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave {

  private System.Random rand = new System.Random();

  private Waveform[] waves = new Waveform[] {
    new SineWave(),
    new SquareWave(),
    new TriangleWave(),
    new SawtoothWave()
  };

  public float Evaluate(int pass, int samples, float rise, int current_iteration, int total_iterations, WaveformComposer.WaveformConfig config) {
    float limit               = 1f;
    float generated_amplitude = 0f;
    float position            = (float) pass / samples;

    if (config.Harmonics.Enabled == true) {
      int upper_limit = config.Context.HighOctave + 1;
      int octaves = upper_limit - config.Context.LowOctave;
      float[] harmonics = new float[octaves];
      float harmonic_sum = 0f;
      int high = config.Context.HighOctave - config.Context.CurrentOctave;
      int octave_range = config.Context.HighOctave - config.Context.LowOctave;

      for (int current = config.Context.LowOctave; current < upper_limit; current++) {
        int harmony = current - config.Context.LowOctave;
        float volume_adjust = 1f;

        if (current < config.Context.CurrentOctave) {
          volume_adjust = config.Harmonics.LowerCurve.Evaluate((float) current / config.Context.CurrentOctave);
        } else if (current > config.Context.CurrentOctave) {
          volume_adjust = config.Harmonics.UpperCurve.Evaluate((current - config.Context.CurrentOctave) / (float) high);
        }

        harmonic_sum += volume_adjust;

        float note = config.Notes[config.Context.NoteIndex] * DetermineOctave(current);

        if (config.Disruption.Enabled == true && config.Disruption.Time.Value > 0) {
          note *= ApplyDisruption(config, position);
        }

        if (config.Noise.Enabled == true) {
          note += note * ApplyNoise(current_iteration, total_iterations, config);
        }

        config.Phases[current] += DetermineIncrement(note, config);

        if (config.FrequencyLimit.Enabled == true) {
          limit = config.FrequencyLimit.Curve.Evaluate((float) harmony / octave_range);
        }

        float amp = Amplitude(config.Volume.Level, rise, volume_adjust);

        harmonics[harmony] += ProcessWaves(current, amp, config) * limit;

        WrapPhaseIfNeeded(current, config);
      }

      float harmony_slice = config.Volume.Level / harmonic_sum;

      for (int i = 0; i < octaves; i++) {
        generated_amplitude += harmonics[i] * harmony_slice;
      }
    } else {
      float note = config.Notes[config.Context.NoteIndex] * DetermineOctave(config.Context.CurrentOctave);

      if (config.Disruption.Enabled == true && config.Disruption.Time.Value > 0) {
        note *= ApplyDisruption(config, position);
      }

      if (config.Noise.Enabled == true) {
        note += note * ApplyNoise(current_iteration, total_iterations, config);
      }

      config.Phases[config.Context.CurrentOctave] += DetermineIncrement(note, config);

      if (config.FrequencyLimit.Enabled == true) {
        int current = config.Context.CurrentOctave;
        int high    = config.Context.HighOctave;
        int low     = config.Context.LowOctave;

        limit = config.FrequencyLimit.Curve.Evaluate((float) current / (high - low));
      }

      float amp = Amplitude(config.Volume.Level, rise, 1f);

      generated_amplitude += ProcessWaves(config.Context.CurrentOctave, amp, config) * limit * config.Volume.Level;

      WrapPhaseIfNeeded(config.Context.CurrentOctave, config);
    }

    return generated_amplitude;
  }

  public float DetermineOctave(int index) {
    float multiplier = 1f;

    if (index > 2) {
      multiplier = (float) Math.Pow(2, index);
    } else if (index == 2) {
      multiplier = index;
    }

    return multiplier;
  }

  float ApplyDisruption(WaveformComposer.WaveformConfig config, float position) {
    return config.Disruption.Curve.Evaluate(position) * 2f;
  }

  float ApplyNoise(int current_iteration, int total_iterations, WaveformComposer.WaveformConfig config) {
    float noise = (float) rand.NextDouble();

    if (noise >= config.Noise.Variance.Min && noise <= config.Noise.Variance.Max) {
      noise = noise - (noise * 0.5f);

      float curve = config.Noise.Curve.Evaluate((float) current_iteration / total_iterations);

      return (noise + curve) * config.Noise.Level;
    } else {
      return 0f;
    }
  }

  float ProcessWaves(int octave, float amp, WaveformComposer.WaveformConfig config) {
    float output     = 0f;
    int   wave_count = 0;

    foreach (float wave in config.Waves.Selected) {
      if (wave > 0f) {
        ++wave_count;
      }
    }

    if (wave_count > 0) {
      float contribution = 1f / wave_count;

      for (int w = 0; w < waves.Length; w++) {
        float vol = config.Waves.Selected[w];

        if (vol > 0f) {
          float generated = waves[w].Generate(config.Phases[octave], amp);

          output += generated * contribution * vol;
        }
      }
    }

    return output;
  }

  float DetermineIncrement(float note, WaveformComposer.WaveformConfig config) {
    return (note * 2.0f * (float) Mathf.PI) / (config.SamplingFrequency * 1000f);
  }

  void WrapPhaseIfNeeded(int index, WaveformComposer.WaveformConfig config) {
    if (config.Phases[index] > (2 * Mathf.PI)) {
      config.Phases[index] -= 2 * Mathf.PI;
    }
  }

  float Amplitude(float volume, float rise, float adjust) {
    return volume * rise * adjust;
  }
}
