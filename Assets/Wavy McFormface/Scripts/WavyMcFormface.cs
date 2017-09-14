using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bsgbryan {
  public class WavyMcFormface : MonoBehaviour {

    [SerializeField]
    public bsgbryan.ConfigieMcWaveface Configie;

    private Wave wave = new Wave();

    private int samples_per_second;

    private bool playing = false;

    #region Play related properties
      private int   note;
      private float octave;
      private long  hit;
      private long  released;
      private bool  tapped;
    #endregion

    #region Lerp related properties
      private float passes = 0;

      private float previous_rise  = 0f;
      private float rise_lerp_goal = 0f;

      private float loose_lerp_iterations;
      private float detailed_lerp_iterations = 1024f;

      private int   noise_variance_min_lerp_progress = 0;
      private int   noise_variance_max_lerp_progress = 0;
      private int   noise_level_lerp_progress        = 0;
      private int   master_volume_lerp_progress      = 0;
      private int[] wave_volumes_lerp_progress       = new int[4] {
        0, 0, 0, 0
      };

      private int lower_harmonic_lerp_progress  = 0;
      private int upper_harmonic_lerp_progress  = 0;
      private int frequency_limit_lerp_progress = 0;
      private int disruption_lerp_progress      = 0;
      private int noise_curve_lerp_progress     = 0;

      private bool lower_harmonic_is_lerping  = false;
      private bool upper_harmonic_is_lerping  = false;
      private bool frequency_limit_is_lerping = false;
      private bool disruption_is_lerping      = false;
      private bool noise_curve_is_lerping     = false;
    #endregion

    void Start() {
      samples_per_second    = Configie.SampleRate / 1000;
      loose_lerp_iterations = (float) samples_per_second;
    }

    /*
      If a note was tapped and it's been playing for its Attack time, stop
      playing it
     */
    void Update() {
      if (tapped == true && hit > 0 && System.DateTime.Now.Ticks - hit > (Configie.Envelope.Attack.Time * 10000) * 2) {
        StopPlaying();
      }
    }

    float CalculateAttack() {
      if (hit > 0) {
        float ticks_risen   = (System.DateTime.Now.Ticks - hit) * 0.0001f;
        float rise_duration = Configie.Envelope.Attack.Time;

        if (ticks_risen < rise_duration) {
          return Configie.Envelope.Attack.Curve.Evaluate(ticks_risen / rise_duration);
        } else {
          return 1f;
        }
      } else if (released > 0) {
        float diff           = (System.DateTime.Now.Ticks - released) * 0.0001f;
        float decay_duration = Configie.Envelope.Decay.Time;

        if (diff < decay_duration) {
          return Configie.Envelope.Decay.Curve.Evaluate(diff / decay_duration);
        } else {
          released = 0;

          return 0f;
        }
      }

      return 0f;
    }

    /*
      This builds our waveform
     */
    void OnAudioFilterRead(float[] data, int channels) {
      // #if UNITY_EDITOR
      //   long started = System.DateTime.Now.Ticks;
      // #endif

      if (playing == true) {
        float position  = 0f;
        float pass      = 0f;
        float counter   = 0f;
        bool  d_enable  = Configie.WibbleWobble.Enabled;
        int   limit     = 0;
        int   length    = data.Length;
        float len       = (float) length - 2;
        float max_count = len / channels;

        if (d_enable == true) {
          limit = ((data.Length / 2) * Configie.WibbleWobble.Time.Value) / samples_per_second;

          if (passes >= limit) {
            passes = 0;
          }
        }

        rise_lerp_goal = CalculateAttack();

        for (int i = 0; i < length; i += channels) {
          pass = counter++ / max_count;

          if (d_enable == true) {
            position = passes++ / limit;
          }

          float rise = SmoothFloatTransition(
            previous_rise,
            rise_lerp_goal,
            pass);

          DoFastLerps();

          data[i] = wave.Evaluate(note, octave, rise, position, pass, Configie);

          if (channels == 2) {
            data[i + 1] = data[i];
          }
        }

        DoSlowLerps();

        previous_rise = rise_lerp_goal;

        if (previous_rise == 0f) {
          playing = false;
        }
      }
      // #if UNITY_EDITOR
      //   long finish = System.DateTime.Now.Ticks;
      //
      //   Debug.Log("total wave generation time " + ((finish - started) * 0.0001f));
      // #endif
    }

    /*
      These are called fast lerps because the only take 1/48th second to complete
     */
    void DoFastLerps() {
      #region Volume.Level lerp
        if (master_volume_lerp_progress == 0 &&
            Configie.Volume.CurrentLevel != Configie.Volume.Level) {
          Configie.Volume.PreviousLevel = Configie.Volume.CurrentLevel;
        }

        Configie.Volume.CurrentLevel = SmoothFloatTransition(
          Configie.Volume.PreviousLevel,
          Configie.Volume.Level,
          master_volume_lerp_progress++ / detailed_lerp_iterations
        );

        if (Configie.Volume.CurrentLevel == Configie.Volume.Level) {
          Configie.Volume.PreviousLevel = Configie.Volume.CurrentLevel;
          master_volume_lerp_progress = 0;
        }
      #endregion

      #region WaveVolumes lerp
        for (int s = 0; s < Configie.WaveVolumes.Selected.Length; s++) {
          if (wave_volumes_lerp_progress[s] == 0 &&
              Configie.WaveVolumes.CurrentSelected[s] != Configie.WaveVolumes.Selected[s]) {
            Configie.WaveVolumes.PreviousSelected[s] = Configie.WaveVolumes.CurrentSelected[s];
          }

          Configie.WaveVolumes.CurrentSelected[s] = SmoothFloatTransition(
            Configie.WaveVolumes.PreviousSelected[s],
            Configie.WaveVolumes.Selected[s],
            wave_volumes_lerp_progress[s]++ / detailed_lerp_iterations
          );

          if (Configie.WaveVolumes.CurrentSelected[s] == Configie.WaveVolumes.Selected[s]) {
            Configie.WaveVolumes.PreviousSelected[s] = Configie.WaveVolumes.CurrentSelected[s];
            wave_volumes_lerp_progress[s]    = 0;
          }
        }
      #endregion

      #region Noise.Variance.Min lerp
        if (noise_variance_min_lerp_progress == 0 &&
            Configie.Noise.Variance.CurrentMin != Configie.Noise.Variance.Min) {
          Configie.Noise.Variance.PreviousMin = Configie.Noise.Variance.CurrentMin;
        }

        Configie.Noise.Variance.CurrentMin = SmoothFloatTransition(
          Configie.Noise.Variance.PreviousMin,
          Configie.Noise.Variance.Min,
          noise_variance_min_lerp_progress++ / detailed_lerp_iterations
        );

        if (Configie.Noise.Variance.CurrentMin == Configie.Noise.Variance.Min) {
          Configie.Noise.Variance.PreviousMin = Configie.Noise.Variance.CurrentMin;
          noise_variance_min_lerp_progress = 0;
        }
      #endregion

      #region Noise.Variance.Max lerp
        if (noise_variance_max_lerp_progress == 0 &&
            Configie.Noise.Variance.CurrentMax != Configie.Noise.Variance.Max) {
          Configie.Noise.Variance.PreviousMax = Configie.Noise.Variance.CurrentMax;
        }

        Configie.Noise.Variance.CurrentMax = SmoothFloatTransition(
          Configie.Noise.Variance.PreviousMax,
          Configie.Noise.Variance.Max,
          noise_variance_max_lerp_progress++ / detailed_lerp_iterations
        );

        if (Configie.Noise.Variance.CurrentMax == Configie.Noise.Variance.Max) {
          Configie.Noise.Variance.PreviousMax = Configie.Noise.Variance.CurrentMax;
          noise_variance_max_lerp_progress = 0;
        }
      #endregion

      #region Noise.Level lerp
        if (noise_level_lerp_progress == 0 &&
            Configie.Noise.CurrentLevel != Configie.Noise.Level) {
          Configie.Noise.PreviousLevel = Configie.Noise.CurrentLevel;
        }

        Configie.Noise.CurrentLevel = SmoothFloatTransition(
          Configie.Noise.PreviousLevel,
          Configie.Noise.Level,
          noise_level_lerp_progress++ / detailed_lerp_iterations
        );

        if (Configie.Noise.CurrentLevel == Configie.Noise.Level) {
          Configie.Noise.PreviousLevel = Configie.Noise.CurrentLevel;
          noise_level_lerp_progress = 0;
        }
      #endregion
    }

    /*
      These are called slow lerps because they take a full second (48 calls to
      OnAudioFilterRead) to complete
     */
    void DoSlowLerps() {
      #region Harmonics.LowerCurve lerp
        if (lower_harmonic_is_lerping == false &&
            Configie.Harmonics.CurrentLowerCurve.EqualTo(Configie.Harmonics.LowerCurve) == false) {
          Configie.Harmonics.PreviousLowerCurve.keys = Configie.Harmonics.CurrentLowerCurve.keys;
          Configie.Harmonics.FrozenLowerCurve.keys   = Configie.Harmonics.LowerCurve.keys;

          lower_harmonic_is_lerping = true;
        }

        if (lower_harmonic_is_lerping == true) {
          float lower_harmonic_lerp_position = lower_harmonic_lerp_progress++ / loose_lerp_iterations;

          Configie.Harmonics.CurrentLowerCurve = Configie.Harmonics.PreviousLowerCurve.Lerp(
            Configie.Harmonics.FrozenLowerCurve,
            lower_harmonic_lerp_position
          );

          if (Configie.Harmonics.CurrentLowerCurve.EqualTo(Configie.Harmonics.FrozenLowerCurve) == true) {
            Configie.Harmonics.PreviousLowerCurve.keys = Configie.Harmonics.FrozenLowerCurve.keys;
            lower_harmonic_lerp_progress             = 0;
            lower_harmonic_is_lerping                = false;
          }
        }
      #endregion

      #region Harmonics.UpperCurve lerp
        if (upper_harmonic_is_lerping == false &&
            Configie.Harmonics.CurrentUpperCurve.EqualTo(Configie.Harmonics.UpperCurve) == false) {
          Configie.Harmonics.PreviousUpperCurve.keys = Configie.Harmonics.CurrentUpperCurve.keys;
          Configie.Harmonics.FrozenUpperCurve.keys   = Configie.Harmonics.UpperCurve.keys;

          upper_harmonic_is_lerping = true;
        }

        if (upper_harmonic_is_lerping == true) {
          float upper_harmonic_lerp_position = upper_harmonic_lerp_progress++ / loose_lerp_iterations;

          Configie.Harmonics.CurrentUpperCurve = Configie.Harmonics.CurrentUpperCurve.Lerp(
            Configie.Harmonics.FrozenUpperCurve,
            upper_harmonic_lerp_position
          );

          if (Configie.Harmonics.CurrentUpperCurve.EqualTo(Configie.Harmonics.FrozenUpperCurve)) {
            Configie.Harmonics.PreviousUpperCurve.keys = Configie.Harmonics.FrozenUpperCurve.keys;
            upper_harmonic_lerp_progress             = 0;
            upper_harmonic_is_lerping                = false;
          }
        }
      #endregion

      #region FrequencyLimit.Curve lerp
        if (frequency_limit_is_lerping == false &&
            Configie.FrequencyLimit.CurrentCurve.EqualTo(Configie.FrequencyLimit.Curve) == false) {
          Configie.FrequencyLimit.PreviousCurve.keys = Configie.FrequencyLimit.CurrentCurve.keys;
          Configie.FrequencyLimit.FrozenCurve.keys   = Configie.FrequencyLimit.Curve.keys;

          frequency_limit_is_lerping = true;
        }

        if (frequency_limit_is_lerping == true) {
          float frequency_limit_lerp_position = frequency_limit_lerp_progress++ / loose_lerp_iterations;

          Configie.FrequencyLimit.CurrentCurve = Configie.FrequencyLimit.PreviousCurve.Lerp(
            Configie.FrequencyLimit.FrozenCurve,
            frequency_limit_lerp_position
          );

          if (Configie.FrequencyLimit.CurrentCurve.EqualTo(Configie.FrequencyLimit.FrozenCurve)) {
            Configie.FrequencyLimit.PreviousCurve.keys = Configie.FrequencyLimit.FrozenCurve.keys;
            frequency_limit_lerp_progress            = 0;
            frequency_limit_is_lerping               = false;
          }
        }
      #endregion

      #region Disruption.Curve lerp
        if (disruption_is_lerping == false &&
            Configie.WibbleWobble.CurrentCurve.EqualTo(Configie.WibbleWobble.Curve) == false) {
          Configie.WibbleWobble.PreviousCurve.keys = Configie.WibbleWobble.CurrentCurve.keys;
          Configie.WibbleWobble.FrozenCurve.keys   = Configie.WibbleWobble.Curve.keys;

          disruption_is_lerping = true;
        }

        if (disruption_is_lerping == true) {
          float disruption_lerp_position = disruption_lerp_progress++ / loose_lerp_iterations;

          Configie.WibbleWobble.CurrentCurve = Configie.WibbleWobble.PreviousCurve.Lerp(
            Configie.WibbleWobble.FrozenCurve,
            disruption_lerp_position
          );

          if (Configie.WibbleWobble.CurrentCurve.EqualTo(Configie.WibbleWobble.FrozenCurve)) {
            Configie.WibbleWobble.PreviousCurve.keys = Configie.WibbleWobble.FrozenCurve.keys;
            disruption_lerp_progress             = 0;
            disruption_is_lerping                = false;
          }
        }
      #endregion

      #region Noise.Curve lerp
        if (noise_curve_is_lerping == false &&
            Configie.Noise.CurrentCurve.EqualTo(Configie.Noise.Curve) == false) {
          Configie.Noise.PreviousCurve.keys = Configie.Noise.CurrentCurve.keys;
          Configie.Noise.FrozenCurve.keys   = Configie.Noise.Curve.keys;

          noise_curve_is_lerping = true;
        }

        if (noise_curve_is_lerping == true) {
          float noise_curve_lerp_position = noise_curve_lerp_progress++ / loose_lerp_iterations;

          Configie.Noise.CurrentCurve = Configie.Noise.PreviousCurve.Lerp(
            Configie.Noise.FrozenCurve,
            noise_curve_lerp_position
          );

          if (Configie.Noise.CurrentCurve.EqualTo(Configie.Noise.FrozenCurve)) {
            Configie.Noise.PreviousCurve.keys = Configie.Noise.FrozenCurve.keys;
            noise_curve_lerp_progress       = 0;
            noise_curve_is_lerping          = false;
          }
        }
      #endregion
    }

    /*
      Smoothly transition from one float value to another
     */
    float SmoothFloatTransition(float old_volume, float new_volume, float position) {
      if (old_volume != new_volume) {
        return Mathf.Lerp(old_volume, new_volume, position);
      } else {
        return new_volume;
      }
    }

    /*
      Mark when a note started playing and delegate to StartPlaying
      to actually begin play
     */
    public void StartPlaying(int n, int o) {
      hit     = System.DateTime.Now.Ticks;
      note    = n;
      octave  = (float) o;

      playing = true;
    }

    /*
      Tapping a note plays it for double its Attack. The note, rises over
      the Attack time, sustains for the Atack time, and then decays over
      the Decay time.
     */
    public void Tap(int note, int octave) {
      tapped = true;

      StartPlaying(note, octave);
    }

    /*
      Reset all necessary properties to ensure our note decays and stops
      playing
     */
    public void StopPlaying() {
      hit      = 0;
      released = System.DateTime.Now.Ticks;

      tapped = false;
    }
  }
}
