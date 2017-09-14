using System;
using UnityEngine;
using UnityEditor;

namespace bsgbryan {
	[CustomPropertyDrawer(typeof(bsgbryan.FrequencyLimit))]
	public class FrequencyLimitDrawer : PropertyDrawer {

    private SerializedProperty show_properties;
    private SerializedProperty show_details;

    private int total_rows = 1;
    private int detail_rows;

    private float total_height;
    private float row_height;

    float DetermineOctave(int index) {
      float multiplier = (index + 1f);

      if (index > 2) {
        multiplier = (float) Math.Pow(2, index);
      } else if (index == 2) {
        multiplier = 4f;
      }

      return multiplier;
    }

    public override float GetPropertyHeight (SerializedProperty property, GUIContent label) {
      show_properties = property.FindPropertyRelative("ShowProperties");
      show_details    = property.FindPropertyRelative("ShowDetails");

      row_height = EditorGUI.GetPropertyHeight(property);

      if (show_properties.boolValue == true) {
        total_rows = 7;
      } else {
        total_rows = 1;
      }

      if (show_details.boolValue == true) {
        total_rows += detail_rows + 1;
      }

      total_height = (row_height * total_rows) + (row_height * 0.5f);

      return total_height;
    }

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			GUIStyle foldout_style = new GUIStyle(EditorStyles.foldout);
      foldout_style.fontStyle = FontStyle.Bold;

      Rect properties_pos = new Rect(position.x,      position.y, 20, row_height);
      Rect enabled_pos    = new Rect(position.x + 95, position.y, 20, row_height);

      GUIContent property_name = new GUIContent(property.displayName);

      property_name.tooltip = "Use this curve to limit the volume for frequencies across the entire rendered spectrum";

      show_properties.boolValue = EditorGUI.Foldout(properties_pos, show_properties.boolValue, property_name, foldout_style);

      SerializedProperty enabled = property.FindPropertyRelative("Enabled");

      enabled.boolValue = EditorGUI.Toggle(enabled_pos, enabled.boolValue);

      using (new EditorGUI.DisabledScope(enabled.boolValue == false)) {
        if (show_properties.boolValue == true) {
          Rect curve_pos = new Rect(position.x, position.y + row_height, position.width, row_height * 5);

          SerializedProperty curve = property.FindPropertyRelative("Curve");

          EditorGUI.CurveField(curve_pos, curve, Color.white, new Rect(0, 0, 1, 1), GUIContent.none);

          ConfigieMcWaveface configie = (ConfigieMcWaveface) property.serializedObject.targetObject;

          int   low_octave     = configie.PlayControls.LowOctave;
          int   high_octave    = configie.PlayControls.HighOctave;
          int   current_octave = configie.PlayControls.CurrentOctave;
          float note           = configie.Notes[configie.PlayControls.NoteIndex];

          Color white = Color.white * (enabled.boolValue ? 0.5f : 0.375f);

          for (int i = low_octave; i <= high_octave; i++) {
            float left = ((float) (i - low_octave) / (high_octave - low_octave));

            if (i == current_octave) {
              white = Color.white * (enabled.boolValue ? 0.75f : 0.5f);
            } else if (i == current_octave + 1) {
              white = Color.white * (enabled.boolValue ? 0.5f : 0.375f);
            }

            Rect pos = new Rect(curve_pos.x + (curve_pos.width * left), curve_pos.y + 2f, 1f, curve_pos.height - 4f);

            EditorGUI.DrawRect(pos, white);
          }

          Rect details_pos = new Rect(position.x + 10, position.y + (row_height * 6), 20, row_height);

          show_details.boolValue = EditorGUI.Foldout(details_pos, show_details.boolValue, "Details", foldout_style);

          if (show_details.boolValue == true) {
            AnimationCurve anim_curve = curve.animationCurveValue;

            int rows_needed = high_octave - low_octave + 1;

            detail_rows = rows_needed;

            float base_width      = position.width * 0.5f;
            float half_base_width = base_width * 0.5f;
            float octave_width    = base_width * 0.4f;
            float strength_width  = base_width * 0.6f;
            float row_eight       = position.y + (row_height * 7);
            float x_offset        = position.x + base_width;
            float x_octave_offset = position.x + octave_width;
            float x_third_offset  = x_offset + half_base_width;
            float row_nine        = position.y + (row_height * 8);

            Rect octave_text_pos    = new Rect(position.x,      row_eight, octave_width,    row_height);
            Rect strength_text_pos  = new Rect(x_octave_offset, row_eight, strength_width,  row_height);
            Rect frequency_text_pos = new Rect(x_offset,        row_eight, half_base_width, row_height);
            Rect position_text_pos  = new Rect(x_third_offset,  row_eight, half_base_width, row_height);

            GUIContent octave_text    = new GUIContent("octave");
            GUIContent frequency_text = new GUIContent("frequency");
            GUIContent strength_text  = new GUIContent("strength");
            GUIContent position_text  = new GUIContent("position");

            GUIStyle left_style  = new GUIStyle(GUI.skin.label);
            GUIStyle right_style = new GUIStyle(GUI.skin.label);

            left_style.alignment  = TextAnchor.MiddleCenter;
            right_style.alignment = TextAnchor.MiddleLeft;

            octave_text.tooltip    = "All octaves currently being rendered";
            frequency_text.tooltip = "The frequency for [note] in the octave listed";
            strength_text.tooltip  = "The intensity each note will be played at for the given octave";
            position_text.tooltip  = "The position on the curve for [note] at the given octave";

            EditorGUI.LabelField(octave_text_pos,    octave_text,    left_style);
            EditorGUI.LabelField(strength_text_pos,  strength_text,  right_style);
            EditorGUI.LabelField(frequency_text_pos, frequency_text, right_style);
            EditorGUI.LabelField(position_text_pos,  position_text,  right_style);

            for (int i = low_octave; i <= high_octave; i++) {
              float pos      = (float) (i - low_octave) / (high_octave - low_octave);
              float strength = anim_curve.Evaluate(pos);
              float row_y    = row_nine + (row_height * (i - low_octave));

              Rect octave_pos    = new Rect(position.x,      row_y, octave_width,    row_height);
              Rect strength_pos  = new Rect(x_octave_offset, row_y, strength_width,  row_height);
              Rect frequency_pos = new Rect(x_offset,        row_y, half_base_width, row_height);
              Rect position_pos  = new Rect(x_third_offset,  row_y, half_base_width, row_height);

              float freq = note * DetermineOctave(i);

              GUIContent i_octave_text    = new GUIContent(i.ToString());
              GUIContent i_strength_text  = new GUIContent(strength.ToString());
              GUIContent i_frequency_text = new GUIContent(freq.ToString());
              GUIContent i_position_text  = new GUIContent(pos.ToString());

              if (i < current_octave) {
                i_octave_text.tooltip = (Mathf.Abs(i - current_octave)) + " octaves below the current one (" + current_octave + ")";
                left_style.fontStyle  = FontStyle.Normal;
                right_style.fontStyle = FontStyle.Normal;
              } else if (i == current_octave) {
                i_octave_text.tooltip = "The current octave";
                left_style.fontStyle  = FontStyle.Italic;
                right_style.fontStyle = FontStyle.Italic;
              } else {
                i_octave_text.tooltip = (Mathf.Abs(current_octave - i)) + " octaves above the current one (" + current_octave + ")";
                left_style.fontStyle  = FontStyle.Normal;
                right_style.fontStyle = FontStyle.Normal;
              }

              i_strength_text.tooltip  = Mathf.Round(strength * 100f) + "% of the max volume level";
              i_frequency_text.tooltip = "[Frequency tooltp goes here]";
              i_position_text.tooltip  = Mathf.Round(pos * 100f) + "% to the right on the curve";

              EditorGUI.LabelField(octave_pos,    i_octave_text,    left_style);
              EditorGUI.LabelField(strength_pos,  i_strength_text,  right_style);
              EditorGUI.LabelField(frequency_pos, i_frequency_text, right_style);
              EditorGUI.LabelField(position_pos,  i_position_text,  right_style);
            }
          }
        } else {
          show_details.boolValue = false;
        }
      }
		}
	}
}
