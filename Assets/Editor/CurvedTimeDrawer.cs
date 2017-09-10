using UnityEngine;
using UnityEditor;

namespace WaveformComposer {
	[CustomPropertyDrawer(typeof(WaveformComposer.CurvedTime))]
	public class CurvedTimeDrawer : PropertyDrawer {

    private bool show_props = true;

    public override float GetPropertyHeight (SerializedProperty property, GUIContent label) {
      float height = EditorGUI.GetPropertyHeight(property);

      if (show_props == true) {
        return height * 8 + (height * 0.5f);
      } else {
        return height + (height * 0.5f);
      }
    }

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
      float height      = EditorGUI.GetPropertyHeight(property);
      float label_width = 50;

      GUIStyle foldout_style = new GUIStyle(EditorStyles.foldout);
      foldout_style.fontStyle = FontStyle.Bold;

      Rect properties_pos = new Rect(position.x,      position.y, 20, height);
      Rect enabled_pos    = new Rect(position.x + 65, position.y, 20, height);

      SerializedProperty show_properties = property.FindPropertyRelative("ShowProperties");

      show_properties.boolValue = EditorGUI.Foldout(properties_pos, show_properties.boolValue, property.displayName, foldout_style);

      show_props = show_properties.boolValue;

      SerializedProperty enabled = property.FindPropertyRelative("Enabled");

      enabled.boolValue = EditorGUI.Toggle(enabled_pos, enabled.boolValue);

      using (new EditorGUI.DisabledScope(enabled.boolValue == false)) {
        if (show_properties.boolValue == true) {
          Rect label_pos = new Rect(position.x,                        position.y + height, label_width, height * 2);
          Rect row_one   = new Rect(position.x + label_width + 12.5f,  position.y + height, 25,          height);
          Rect row_two   = new Rect(position.x + label_width + 62.5f,  position.y + height, 25,          height);
          Rect row_three = new Rect(position.x + label_width + 112.5f, position.y + height, 25,          height);
          Rect row_four  = new Rect(position.x + label_width + 172.5f, position.y + height, 35,          height);

          SerializedProperty time = property.FindPropertyRelative("Time");

          GUIStyle helper_style = new GUIStyle(GUI.skin.label);
          helper_style.alignment = TextAnchor.MiddleCenter;

          EditorGUI.LabelField(label_pos, "Time", helper_style);

          SerializedProperty hours        = time.FindPropertyRelative("Hours");
          SerializedProperty minutes      = time.FindPropertyRelative("Minutes");
          SerializedProperty seconds      = time.FindPropertyRelative("Seconds");
          SerializedProperty milliseconds = time.FindPropertyRelative("Milliseconds");

          GUIStyle style = new GUIStyle(GUI.skin.textField);
          style.alignment = TextAnchor.MiddleCenter;

          hours.intValue        = EditorGUI.IntField(row_one,   hours.intValue,        style);
          minutes.intValue      = EditorGUI.IntField(row_two,   minutes.intValue,      style);
          seconds.intValue      = EditorGUI.IntField(row_three, seconds.intValue,      style);
          milliseconds.intValue = EditorGUI.IntField(row_four,  milliseconds.intValue, style);

          Rect label_hours_pos        = new Rect(position.x + label_width,       position.y + (height * 2), 50, height);
          Rect label_minutes_pos      = new Rect(position.x + label_width + 50,  position.y + (height * 2), 50, height);
          Rect label_seconds_pos      = new Rect(position.x + label_width + 100, position.y + (height * 2), 50, height);
          Rect label_milliseconds_pos = new Rect(position.x + label_width + 150, position.y + (height * 2), 80, height);

          EditorGUI.LabelField(label_hours_pos,        "hours",        helper_style);
          EditorGUI.LabelField(label_minutes_pos,      "minutes",      helper_style);
          EditorGUI.LabelField(label_seconds_pos,      "seconds",      helper_style);
          EditorGUI.LabelField(label_milliseconds_pos, "milliseconds", helper_style);

          Rect curve_row = new Rect(position.x, position.y + (height * 3), position.width, height * 5);

          SerializedProperty curve = property.FindPropertyRelative("Curve");

          EditorGUI.CurveField(curve_row, curve, Color.magenta, new Rect(0, 0, 1, 1), GUIContent.none);
        }
      }
		}
	}
}
