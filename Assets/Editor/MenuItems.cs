using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace WaveformComposer {
	public static class MenuItems {

		[MenuItem ("Tools/Waveform Composer/New Waveform %#&_w")]
		private static void NewWaveform() {
      string path = EditorUtility.SaveFilePanelInProject(
        "New Waveform",
        "Waveforms",
        "asset",
        "What do you want to name this new Waveform?");

      if(path != "") {
          WaveformConfig config = (WaveformConfig) ScriptableObject.CreateInstance<WaveformConfig>();

          config.Volume.Level = 0.5f;

          config.Waves.Selected[0] = 1f;

          config.Context.LowOctave     = 2;
          config.Context.HighOctave    = 7;
          config.Context.CurrentOctave = 4;
          config.Context.NoteIndex     = 9;

          config.RiseAndDecay.Rise.Curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
          config.RiseAndDecay.Rise.Time  = 200;

          config.RiseAndDecay.Decay.Curve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
          config.RiseAndDecay.Decay.Time  = 1000;

          config.Harmonics.LowerCurve = AnimationCurve.EaseInOut(0f, 0.5f, 1f, 1f);
          config.Harmonics.UpperCurve = AnimationCurve.EaseInOut(0f, 0.5f, 1f, 0f);

          config.FrequencyLimit.Curve = AnimationCurve.EaseInOut(0f, 1f, 1f, 1f);

          config.Disruption.Curve = AnimationCurve.EaseInOut(0f, 0.5f, 1f, 0.5f);

          config.Noise.Variance.Min = 0.25f;
          config.Noise.Variance.Max = 0.75f;
          config.Noise.Level        = 0.5f;
          config.Noise.Curve        = AnimationCurve.EaseInOut(0f, 0.5f, 1f, 0.5f);

          AssetDatabase.CreateAsset(config, path);
          AssetDatabase.Refresh();
          AssetDatabase.SaveAssets();
      }
		}
	}
}
