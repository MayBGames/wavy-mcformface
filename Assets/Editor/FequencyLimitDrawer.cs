using UnityEngine;
using UnityEditor;

namespace WaveformComposer {
	[CustomPropertyDrawer(typeof(WaveformComposer.FrequencyLimit))]
	public class FrequencyLimitDrawer : PropertyDrawer {

    private bool show_props = true;
    private bool show_deets = false;

    private int total_rows = 1;
    private int detail_rows;

    private float total_height;
    private float row_height;

    public override float GetPropertyHeight (SerializedProperty property, GUIContent label) {
      row_height = EditorGUI.GetPropertyHeight(property);

      if (show_props == true) {
        total_rows = 7;
      } else {
        total_rows = 1;
      }

      if (show_deets == true) {
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

      SerializedProperty show_properties = property.FindPropertyRelative("ShowProperties");
      SerializedProperty show_details    = property.FindPropertyRelative("ShowDetails");

      show_properties.boolValue = EditorGUI.Foldout(properties_pos, show_properties.boolValue, property_name, foldout_style);

      show_props = show_properties.boolValue;

      SerializedProperty enabled = property.FindPropertyRelative("Enabled");

      enabled.boolValue = EditorGUI.Toggle(enabled_pos, enabled.boolValue);

      using (new EditorGUI.DisabledScope(enabled.boolValue == false)) {
        if (show_properties.boolValue == true) {
          Rect curve_pos = new Rect(position.x, position.y + row_height, position.width, row_height * 5);

          SerializedProperty curve = property.FindPropertyRelative("Curve");

          EditorGUI.CurveField(curve_pos, curve, Color.white, new Rect(0, 0, 1, 1), GUIContent.none);

          Rect details_pos = new Rect(position.x + 10, position.y + (row_height * 6), 20, row_height);

          show_details.boolValue = EditorGUI.Foldout(details_pos, show_details.boolValue, "Details", foldout_style);

          show_deets = show_details.boolValue;

          if (show_details.boolValue == true) {
            WaveformConfig config = (WaveformConfig) property.serializedObject.targetObject;

            AnimationCurve anim_curve = curve.animationCurveValue;

            int low_octave     = config.Context.LowOctave;
            int high_octave    = config.Context.HighOctave;
            int current_octave = config.Context.CurrentOctave;

            int rows_needed = high_octave - low_octave + 1;

            detail_rows = rows_needed;

            float base_width = position.width * 0.5f;

            Rect octave_text_pos    = new Rect(position.x,                                    position.y + (row_height * 7), base_width * 0.4f, row_height);
            Rect strength_text_pos  = new Rect(position.x + (base_width * 0.4f),              position.y + (row_height * 7), base_width * 0.6f, row_height);
            Rect frequency_text_pos = new Rect(position.x + base_width,                       position.y + (row_height * 7), base_width * 0.6f, row_height);
            Rect position_text_pos  = new Rect(position.x + base_width + (base_width * 0.6f), position.y + (row_height * 7), base_width * 0.4f, row_height);

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

              float row_y          = position.y + (row_height * 8) + (row_height * (i - low_octave));
              float octave_width   = base_width * 0.4f;
              float strength_width = base_width * 0.6f;

              Rect octave_pos    = new Rect(position.x,                               row_y, octave_width,   row_height);
              Rect strength_pos  = new Rect(position.x + octave_width,                row_y, strength_width, row_height);
              Rect frequency_pos = new Rect(position.x + base_width,                  row_y, strength_width, row_height);
              Rect position_pos  = new Rect(position.x + base_width + strength_width, row_y, octave_width,   row_height);

              GUIContent i_octave_text    = new GUIContent(i.ToString());
              GUIContent i_strength_text  = new GUIContent(strength.ToString());
              GUIContent i_frequency_text = new GUIContent("42");
              GUIContent i_position_text  = new GUIContent(pos.ToString());

              if (i < current_octave) {
                i_octave_text.tooltip = (Mathf.Abs(i - current_octave)) + " octaves below the current one (" + current_octave + ")";
                left_style.fontStyle  = FontStyle.Normal;
                right_style.fontStyle = FontStyle.Normal;
              } else if (i == current_octave) {
                i_octave_text.tooltip = "The current octave";
                left_style.fontStyle  = FontStyle.Bold;
                right_style.fontStyle = FontStyle.Bold;
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

            // for (float i = 0; i < low_diff; i++) {
            //   float strength = lower_curve.Evaluate(i / low_diff);
            //
            //   float row_y          = position.y + (row_height * 8) + (row_height * i);
            //   float octave_width   = base_width * 0.4f;
            //   float strength_width = base_width * 0.6f;
            //
            //   Rect lower_octave_pos   = new Rect(position.x,                row_y, octave_width,   row_height);
            //   Rect lower_strength_pos = new Rect(position.x + octave_width, row_y, strength_width, row_height);
            //
            //   GUIContent octave_text         = new GUIContent(i.ToString());
            //   GUIContent lower_strength_text = new GUIContent(strength.ToString());
            //
            //   octave_text.tooltip         = (Mathf.Abs(i - current_octave)) + " octaves below the current one (" + current_octave + ")";
            //   lower_strength_text.tooltip = (Mathf.Round((1f - strength) * 100f)) + "% quieter than the current octave (" + current_octave + ")";
            //
            //   EditorGUI.LabelField(lower_octave_pos,   octave_text,         left_style);
            //   EditorGUI.LabelField(lower_strength_pos, lower_strength_text, right_style);
            // }
            //
            // for (float i = 1; i <= high_diff; i++) {
            //   float strength = upper_curve.Evaluate(i / high_diff);
            //
            //   float row_x          = base_width;
            //   float row_y          = position.y + (row_height * 8) + (row_height * (i - 1));
            //   float octave_width   = base_width * 0.4f;
            //   float strength_width = base_width * 0.6f;
            //
            //   Rect lower_octave_pos   = new Rect(position.x + row_x,                row_y, octave_width,   row_height);
            //   Rect lower_strength_pos = new Rect(position.x + row_x + octave_width, row_y, strength_width, row_height);
            //
            //   GUIContent octave_text         = new GUIContent((i + current_octave).ToString());
            //   GUIContent upper_strength_text = new GUIContent(strength.ToString());
            //
            //   octave_text.tooltip         = (Mathf.Abs(current_octave - (current_octave + i))) + " octaves above the current one (" + current_octave + ")";
            //   upper_strength_text.tooltip = (Mathf.Round((1f - strength) * 100f)) + "% quieter than the current octave (" + current_octave + ")";
            //
            //   EditorGUI.LabelField(lower_octave_pos,   octave_text,         left_style);
            //   EditorGUI.LabelField(lower_strength_pos, upper_strength_text, right_style);
            // }
          }
        } else {
          show_details.boolValue = false;
          show_deets             = false;
        }
      }
		}
	}
}
