using UnityEngine;
using UnityEditor;

namespace bsgbryan {
	[CustomPropertyDrawer(typeof(bsgbryan.Envelope))]
	public class EnvelopeDrawer : PropertyDrawer {

  private SerializedProperty show_properties;

    public override float GetPropertyHeight (SerializedProperty property, GUIContent label) {
      show_properties = property.FindPropertyRelative("ShowProperties");

      float height = EditorGUI.GetPropertyHeight(property);

      if (show_properties.boolValue == true) {
        return height * 8f + (height * 0.5f);
      } else {
        return height + (height * 0.5f);
      }
    }

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			float height       = EditorGUI.GetPropertyHeight(property);
      float curve_width  = position.width * 0.5f;
      float other_width  = curve_width    * 0.5f;

      GUIStyle foldout_style = new GUIStyle(EditorStyles.foldout);
      foldout_style.fontStyle = FontStyle.Bold;

      Rect foldout_pos = new Rect(position.x, position.y, 20, height);

      show_properties.boolValue = EditorGUI.Foldout(foldout_pos, show_properties.boolValue, property.displayName, foldout_style);

      if (show_properties.boolValue == true) {
        GUIStyle helper_style = new GUIStyle(GUI.skin.label);
        GUIStyle input_style  = new GUIStyle(GUI.skin.textField);

        helper_style.alignment = TextAnchor.MiddleCenter;
        input_style.alignment  = TextAnchor.MiddleCenter;

        float row_one        = position.y  + height;
        float row_two        = position.y  + (height * 2);
        float row_three      = position.y  + (height * 3);
        float x_offset       = position.x  +  other_width;
        float other_three    = other_width * 3;
        float x_three_offset = position.x  + other_three;
        float curve_height   = height      * 5;

        Rect attack_label_pos = new Rect(position.x,                     row_one, other_width, height);
        Rect attack_time_pos  = new Rect(x_offset,                       row_one, other_width, height);
        Rect attack_milli_pos = new Rect(x_offset,                       row_two, other_width, height);
        Rect decay_label_pos  = new Rect(position.x + (other_width * 2), row_one, other_width, height);
        Rect decay_time_pos   = new Rect(x_three_offset,                 row_one, other_width, height);
        Rect decay_milli_pos  = new Rect(x_three_offset,                 row_two, other_width, height);

        EditorGUI.LabelField(attack_label_pos, "attack",       helper_style);
        EditorGUI.LabelField(attack_milli_pos, "milliseconds", helper_style);
        EditorGUI.LabelField(decay_label_pos,  "decay",        helper_style);
        EditorGUI.LabelField(decay_milli_pos,  "milliseconds", helper_style);

        Rect attack_curve_pos  = new Rect(position.x,               row_three, curve_width, curve_height);
        Rect decay_curve_pos = new Rect(position.x + curve_width, row_three, curve_width, curve_height);

        SerializedProperty attack  = property.FindPropertyRelative("Attack");
        SerializedProperty decay = property.FindPropertyRelative("Decay");

        SerializedProperty attack_time  = attack.FindPropertyRelative("Time");
        SerializedProperty decay_time   = decay.FindPropertyRelative("Time");
        SerializedProperty attack_curve = attack.FindPropertyRelative("Curve");
        SerializedProperty decay_curve  = decay.FindPropertyRelative("Curve");

        using (new EditorGUI.DisabledScope(EditorApplication.isPlaying == true)) {
          attack_time.intValue = EditorGUI.IntField(attack_time_pos, attack_time.intValue, input_style);
          decay_time.intValue  = EditorGUI.IntField(decay_time_pos,  decay_time.intValue,  input_style);
        }

        Rect limit = new Rect(0, 0, 1, 1);

        EditorGUI.CurveField(attack_curve_pos, attack_curve, Color.white, limit, GUIContent.none);
        EditorGUI.CurveField(decay_curve_pos,  decay_curve,  Color.white, limit, GUIContent.none);
      }
		}
	}
}
