using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveGenerator : MonoBehaviour {

	private Wave wave = new Wave();

	private long hit;
	private long released;

  private int passes = 0;

  [SerializeField]
  public WaveformComposer.WaveformConfig Config;

  float CalculateRise() {
		float rise = 0f;

		if (hit > 0) {
			long ticks_risen = System.DateTime.Now.Ticks - hit;
			long rise_duration = Config.RiseAndDecay.Rise.Time * 10000;

			if (ticks_risen < rise_duration) {
				rise = Config.RiseAndDecay.Rise.Curve.Evaluate((float) ticks_risen / rise_duration);
			} else {
        rise = 1f;
      }
		} else if (released > 0) {
			float diff = (System.DateTime.Now.Ticks - released) * 0.0000001f;
			float decay_duration = Config.RiseAndDecay.Decay.Time * 0.001f;

			if (diff < decay_duration) {
				rise = Config.RiseAndDecay.Decay.Curve.Evaluate(diff / decay_duration);
			} else {
        rise     = 0f;
				hit      = 0;
				released = 0;
			}
		}

		return rise;
	}

	void OnAudioFilterRead(float[] data, int channels) {
    int limit = ((data.Length / 2) * Config.Disruption.Time.Value) / (int) Config.SamplingFrequency;

    float rise = CalculateRise();

    if (rise > 0f) {
  		if (passes >= limit) {
  			passes = 0;
  		}

      int counter = 0;

      for (int i = 0; i < data.Length; i += channels) {
  			data[i] += wave.Evaluate(passes++, limit, rise, counter, data.Length / channels, Config);

        if (channels == 2) {
  				data[i + 1] += data[i];
  			}

        counter++;
      }
    }
	}

  public void StartPlaying() {
    hit = System.DateTime.Now.Ticks;
  }

  public void StopPlaying() {
    hit      = 0;
    released = System.DateTime.Now.Ticks;
  }
}
