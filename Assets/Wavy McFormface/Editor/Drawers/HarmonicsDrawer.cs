using UnityEngine;
using UnityEditor;

namespace bsgbryan {
	[CustomPropertyDrawer(typeof(bsgbryan.Harmonics))]
	public class HarmonicsDrawer : PropertyDrawer {

    private SerializedProperty show_properties;
    private SerializedProperty show_details;

    private int total_rows = 1;
    private int detail_rows;

    private float total_height;
    private float row_height;

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
			float width = position.width * 0.5f;

      GUIStyle foldout_style = new GUIStyle(EditorStyles.foldout);
      foldout_style.fontStyle = FontStyle.Bold;

      Rect properties_pos = new Rect(position.x,      position.y, 20, row_height);
      Rect enabled_pos    = new Rect(position.x + 68, position.y, 20, row_height);

      GUIContent property_name = new GUIContent(property.displayName);

      property_name.tooltip = "Harmonics are the octaves above and below the current one. They provide a fuller, richer sound.";

      show_properties.boolValue = EditorGUI.Foldout(properties_pos, show_properties.boolValue, property_name, foldout_style);

      SerializedProperty enabled = property.FindPropertyRelative("Enabled");

      enabled.boolValue = EditorGUI.Toggle(enabled_pos, enabled.boolValue);

      using (new EditorGUI.DisabledScope(enabled.boolValue == false)) {
        if (show_properties.boolValue == true) {
          float row_one      = position.y + row_height;
          float curve_height = row_height * 5;

          Rect lower_pos = new Rect(position.x,         row_one, width, curve_height);
          Rect upper_pos = new Rect(position.x + width, row_one, width, curve_height);

          SerializedProperty lower = property.FindPropertyRelative("LowerCurve");
          SerializedProperty upper = property.FindPropertyRelative("UpperCurve");

          Rect limit = new Rect(0, 0, 1, 1);

          EditorGUI.CurveField(lower_pos, lower, Color.white, limit, GUIContent.none);
          EditorGUI.CurveField(upper_pos, upper, Color.white, limit, GUIContent.none);

          Rect details_pos = new Rect(position.x + 10, position.y + (row_height * 6), 20, row_height);

          show_details.boolValue = EditorGUI.Foldout(details_pos, show_details.boolValue, "Details", foldout_style);

          ConfigieMcWaveface configie = (ConfigieMcWaveface) property.serializedObject.targetObject;

          int low_octave     = configie.PlayControls.LowOctave;
          int high_octave    = configie.PlayControls.HighOctave;
          int current_octave = configie.PlayControls.CurrentOctave;

          float low_diff  = (float) current_octave - low_octave;
          float high_diff = (float) high_octave - current_octave;

          float y_offset   = lower_pos.y + 2f;
          float bar_height = lower_pos.height - 4f;

          Color white = Color.white * (enabled.boolValue ? 0.5f : 0.5f);

          for (float i = 0; i < low_diff; i++) {
            float left = i / low_diff;

            Rect pos = new Rect(lower_pos.x + (lower_pos.width * left), y_offset, 1f, bar_height);

            EditorGUI.DrawRect(pos, white);
          }

          for (float i = 1; i <= high_diff; i++) {
            float left = i / high_diff;

            Rect pos = new Rect(upper_pos.x + (upper_pos.width * left), y_offset, 1f, bar_height);

            EditorGUI.DrawRect(pos, white);
          }

          if (show_details.boolValue == true) {
            AnimationCurve lower_curve = lower.animationCurveValue;
            AnimationCurve upper_curve = upper.animationCurveValue;

            float rows_needed = low_diff < high_diff ? high_diff : low_diff;

            detail_rows = (int) rows_needed;

            float base_width      = position.width * 0.5f;
            float octave_width    = base_width     * 0.4f;
            float strength_width  = base_width     * 0.6f;
            float x_offset        = position.x     + base_width;
            float x_octave_offset = position.x     + octave_width;
            float x_third_offset  = x_offset       + octave_width;
            float row_eight       = position.y     + (row_height * 7);

            Rect lower_octave_text_pos   = new Rect(position.x,      row_eight, octave_width,   row_height);
            Rect lower_strength_text_pos = new Rect(x_octave_offset, row_eight, strength_width, row_height);
            Rect upper_octave_text_pos   = new Rect(x_offset,        row_eight, octave_width,   row_height);
            Rect upper_strength_text_pos = new Rect(x_third_offset,  row_eight, strength_width, row_height);

            GUIContent lower_octave_text = new GUIContent("octave");
            GUIContent upper_octave_text = new GUIContent("octave");
            GUIContent strength_text     = new GUIContent("strength");

            GUIStyle left_style  = new GUIStyle(GUI.skin.label);
            GUIStyle right_style = new GUIStyle(GUI.skin.label);

            left_style.alignment  = TextAnchor.MiddleCenter;
            right_style.alignment = TextAnchor.MiddleLeft;

            lower_octave_text.tooltip = "The octaves below the current one (" + current_octave + ")";
            upper_octave_text.tooltip = "The octaves above the current one (" + current_octave + ")";
            strength_text.tooltip     = "The intensity each octave will be played at";

            EditorGUI.LabelField(lower_octave_text_pos,   lower_octave_text,   left_style);
            EditorGUI.LabelField(lower_strength_text_pos, strength_text,       right_style);
            EditorGUI.LabelField(upper_octave_text_pos,   upper_octave_text,   left_style);
            EditorGUI.LabelField(upper_strength_text_pos, strength_text,       right_style);

            float row_nine = position.y + (row_height * 8);

            for (float i = 0; i < low_diff; i++) {
              float strength = lower_curve.Evaluate(i / low_diff);
              float row_y    = row_nine + (row_height * i);

              Rect lower_octave_pos   = new Rect(position.x,      row_y, octave_width,   row_height);
              Rect lower_strength_pos = new Rect(x_octave_offset, row_y, strength_width, row_height);

              GUIContent octave_text         = new GUIContent(i.ToString());
              GUIContent lower_strength_text = new GUIContent(strength.ToString());

              octave_text.tooltip         = (Mathf.Abs(i - current_octave)) + " octaves below the current one (" + current_octave + ")";
              lower_strength_text.tooltip = (Mathf.Round((1f - strength) * 100f)) + "% quieter than the current octave (" + current_octave + ")";

              EditorGUI.LabelField(lower_octave_pos,   octave_text,         left_style);
              EditorGUI.LabelField(lower_strength_pos, lower_strength_text, right_style);
            }

            for (float i = 1; i <= high_diff; i++) {
              float strength = upper_curve.Evaluate(i / high_diff);
              float row_y    = row_nine + (row_height * (i - 1));

              Rect lower_octave_pos   = new Rect(x_offset,       row_y, octave_width,   row_height);
              Rect lower_strength_pos = new Rect(x_third_offset, row_y, strength_width, row_height);

              GUIContent octave_text         = new GUIContent((i + current_octave).ToString());
              GUIContent upper_strength_text = new GUIContent(strength.ToString());

              octave_text.tooltip         = (Mathf.Abs(current_octave - (current_octave + i))) + " octaves above the current one (" + current_octave + ")";
              upper_strength_text.tooltip = (Mathf.Round((1f - strength) * 100f)) + "% quieter than the current octave (" + current_octave + ")";

              EditorGUI.LabelField(lower_octave_pos,   octave_text,         left_style);
              EditorGUI.LabelField(lower_strength_pos, upper_strength_text, right_style);
            }
          }
        } else {
          show_details.boolValue = false;
        }
      }
		}
	}
}
