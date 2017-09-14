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

  public float Evaluate(int note_index, float octave, float rise, float position, float pass, bsgbryan.ConfigieMcWaveface configie) {
    float generated_amplitude = 0f;

    if (configie.Harmonics.Enabled == true) {
      float low         = (float) configie.PlayControls.LowOctave;
      float high        = (float) configie.PlayControls.HighOctave;
      float upper_limit = configie.PlayControls.HighOctave + 1f;

      for (float current = low; current < upper_limit; current++) {
        float volume_adjust = 1f;

        if (current < octave) {
          volume_adjust = configie.Harmonics.CurrentLowerCurve.Evaluate(current / octave);
        } else if (current > octave) {
          volume_adjust = configie.Harmonics.CurrentUpperCurve.Evaluate(current / high);
        }

        generated_amplitude += ProcessNote(note_index, current, rise, position, volume_adjust, pass, configie);
      }
    } else {
      generated_amplitude = ProcessNote(note_index, octave, rise, position, 1f, pass, configie);
    }

    return generated_amplitude;
  }

  public float DetermineOctave(float index) {
    float multiplier = (index + 1f);

    if (index > 2) {
      multiplier = (float) Math.Pow(2, index);
    } else if (index == 2) {
      multiplier = 4f;
    }

    return multiplier;
  }

  float ProcessNote(int note_index, float octave, float rise, float position, float volume_adjust, float pass, bsgbryan.ConfigieMcWaveface configie) {
    float limit = 1f;
    float note  = configie.Notes[note_index] * DetermineOctave(octave);

    if (configie.WibbleWobble.Enabled == true && configie.WibbleWobble.Time.Value > 0) {
      note *= ApplyWibbleWobble(configie, position);
    }

    if (configie.Noise.Enabled == true) {
      note += note * ApplyNoise(pass, configie);
    }

    configie.Phases[(int) octave] += DetermineIncrement(note, configie);

    float high  = configie.PlayControls.HighOctave + 1;
    float low   = configie.PlayControls.LowOctave;
    float range = high - low;
    float vol   = configie.Volume.CurrentLevel;

    // Debug.Log("volume " + vol);

    if (configie.FrequencyLimit.Enabled == true) {
      limit = configie.FrequencyLimit.CurrentCurve.Evaluate((octave - low) / range);
    }

    float amp = Amplitude(vol, volume_adjust) * rise;
    float mod = 1f / range;

    WrapPhaseIfNeeded((int) octave, configie);

    return ProcessWaveVolumes((int) octave, amp, configie) * limit * vol * mod;
  }

  float ApplyWibbleWobble(bsgbryan.ConfigieMcWaveface configie, float position) {
    return configie.WibbleWobble.CurrentCurve.Evaluate(position) * 2f;
  }

  float ApplyNoise(float pass, bsgbryan.ConfigieMcWaveface configie) {
    float noise = (float) rand.NextDouble();

    if (noise >= configie.Noise.Variance.Min && noise <= configie.Noise.Variance.Max) {
      noise = noise - (noise * 0.5f);

      float curve = configie.Noise.CurrentCurve.Evaluate(pass);

      return (noise + curve) * configie.Noise.Level;
    } else {
      return 0f;
    }
  }

  float ProcessWaveVolumes(int octave, float amp, bsgbryan.ConfigieMcWaveface configie) {
    float output = 0f;

    for (int w = 0; w < waves.Length; w++) {
      float vol = configie.WaveVolumes.CurrentSelected[w];

      output += waves[w].Generate(configie.Phases[octave], amp * vol);
    }

    return output;
  }

  float DetermineIncrement(float note, bsgbryan.ConfigieMcWaveface configie) {
    return (note * 2.0f * Mathf.PI) / (float) configie.SampleRate;
  }

  void WrapPhaseIfNeeded(int index, bsgbryan.ConfigieMcWaveface configie) {
    if (configie.Phases[index] > (2 * Mathf.PI)) {
      configie.Phases[index] -= 2 * Mathf.PI;
    }
  }

  float Amplitude(float volume, float adjust) {
    return volume * adjust;
  }
}
