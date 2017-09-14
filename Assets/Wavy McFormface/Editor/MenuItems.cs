using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace bsgbryan {
	public static class MenuItems {

		[MenuItem ("Tools/Wavy McFormface/New Waveform %#&_w")]
		private static void NewWaveform() {
      string path = EditorUtility.SaveFilePanelInProject(
        "New Waveform",
        "Waveforms",
        "asset",
        "What do you want to name this new Waveform?");

      if(path != "") {
          ConfigieMcWaveface configie = (ConfigieMcWaveface) ScriptableObject.CreateInstance<ConfigieMcWaveface>();

          configie.Volume.Level = 0.5f;

          configie.WaveVolumes.Selected[0] = 1f;

          configie.PlayControls.LowOctave     = 2;
          configie.PlayControls.HighOctave    = 7;
          configie.PlayControls.CurrentOctave = 4;
          configie.PlayControls.NoteIndex     = 9;

          configie.Envelope.Attack.Curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
          configie.Envelope.Attack.Time  = 200;

          configie.Envelope.Decay.Curve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
          configie.Envelope.Decay.Time  = 1000;

          configie.Harmonics.Enabled    = true;
          configie.Harmonics.LowerCurve = AnimationCurve.EaseInOut(0f, 0.5f, 1f, 1f);
          configie.Harmonics.UpperCurve = AnimationCurve.EaseInOut(0f, 0.5f, 1f, 0f);

          configie.FrequencyLimit.Curve = AnimationCurve.EaseInOut(0f, 1f, 1f, 1f);

          configie.WibbleWobble.Curve = AnimationCurve.EaseInOut(0f, 0.5f, 1f, 0.5f);

          configie.Noise.Variance.Min = 0.25f;
          configie.Noise.Variance.Max = 0.75f;
          configie.Noise.Level        = 0.5f;
          configie.Noise.Curve        = AnimationCurve.EaseInOut(0f, 0.5f, 1f, 0.5f);

          AssetDatabase.CreateAsset(configie, path);
          AssetDatabase.Refresh();
          AssetDatabase.SaveAssets();

          char[] path_delimiters = { '/' };
          char[] name_delimiters = { '.' };

          string[] path_tokens = path.Split(path_delimiters);
          string[] name_tokens = path_tokens[path_tokens.Length - 1].Split(name_delimiters);

          GameObject new_waveform = new GameObject(name_tokens[0]);

          new_waveform.AddComponent<AudioSource>();
          bsgbryan.WavyMcFormface wavy = new_waveform.AddComponent<bsgbryan.WavyMcFormface>() as bsgbryan.WavyMcFormface;

          wavy.Configie = configie;
      }
		}
	}
}
