using UnityEngine;
using UnityEditor;

namespace WaveformComposer {
	[CustomPropertyDrawer(typeof(WaveformComposer.RiseAndDecay))]
	public class RiseAndDecayDrawer : PropertyDrawer {

    private bool show_details = true;

    public override float GetPropertyHeight (SerializedProperty property, GUIContent label) {
      float height = EditorGUI.GetPropertyHeight(property);

      if (show_details == true) {
        return height * 8 + (height * 0.5f);
      } else {
        return height + (height * 0.5f);
      }
    }

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			float height       = EditorGUI.GetPropertyHeight(property);
      float curve_width  = position.width / 2;
      float other_width  = curve_width / 2;

      GUIStyle foldout_style = new GUIStyle(EditorStyles.foldout);
      foldout_style.fontStyle = FontStyle.Bold;

      Rect foldout_pos = new Rect(position.x, position.y, 20, height);

      show_details = EditorGUI.Foldout(foldout_pos, show_details, property.displayName, foldout_style);

      if (show_details == true) {
        GUIStyle helper_style = new GUIStyle(GUI.skin.label);
        GUIStyle input_style  = new GUIStyle(GUI.skin.textField);

        helper_style.alignment = TextAnchor.MiddleCenter;
        input_style.alignment  = TextAnchor.MiddleCenter;

        Rect rise_label_pos  = new Rect(position.x,                     position.y + height,       other_width, height);
        Rect rise_time_pos   = new Rect(position.x +  other_width,      position.y + height,       other_width, height);
        Rect rise_milli_pos  = new Rect(position.x +  other_width,      position.y + (height * 2), other_width, height);
        Rect decay_label_pos = new Rect(position.x + (other_width * 2), position.y + height,       other_width, height);
        Rect decay_time_pos  = new Rect(position.x + (other_width * 3), position.y + height,       other_width, height);
        Rect decay_milli_pos = new Rect(position.x + (other_width * 3), position.y + (height * 2), other_width, height);

        EditorGUI.LabelField(rise_label_pos,  "rise",         helper_style);
        EditorGUI.LabelField(rise_milli_pos,  "milliseconds", helper_style);
        EditorGUI.LabelField(decay_label_pos, "decay",        helper_style);
        EditorGUI.LabelField(decay_milli_pos, "milliseconds", helper_style);

        Rect rise_curve_pos  = new Rect(position.x,               position.y + (height * 3), curve_width, height * 5);
        Rect decay_curve_pos = new Rect(position.x + curve_width, position.y + (height * 3), curve_width, height * 5);

        SerializedProperty rise  = property.FindPropertyRelative("Rise");
        SerializedProperty decay = property.FindPropertyRelative("Decay");

        SerializedProperty rise_time   = rise.FindPropertyRelative("Time");
        SerializedProperty decay_time  = decay.FindPropertyRelative("Time");
        SerializedProperty rise_curve  = rise.FindPropertyRelative("Curve");
        SerializedProperty decay_curve = decay.FindPropertyRelative("Curve");

        rise_time.intValue  = EditorGUI.IntField(rise_time_pos,  rise_time.intValue,  input_style);
        decay_time.intValue = EditorGUI.IntField(decay_time_pos, decay_time.intValue, input_style);

        EditorGUI.CurveField(rise_curve_pos,  rise_curve,  Color.white, new Rect(0, 0, 1, 1), GUIContent.none);
        EditorGUI.CurveField(decay_curve_pos, decay_curve, Color.white, new Rect(0, 0, 1, 1), GUIContent.none);
      }
		}
	}
}
