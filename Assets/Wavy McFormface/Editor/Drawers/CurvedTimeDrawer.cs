using UnityEngine;
using UnityEditor;

namespace bsgbryan {
	[CustomPropertyDrawer(typeof(bsgbryan.CurvedTime))]
	public class CurvedTimeDrawer : PropertyDrawer {

    private SerializedProperty show_properties;

    private float height;
    private float double_height;

    public override float GetPropertyHeight (SerializedProperty property, GUIContent label) {
      show_properties = property.FindPropertyRelative("ShowProperties");

      if (height == 0f) {
        height = EditorGUI.GetPropertyHeight(property);

        double_height  = height * 2f;
      }

      if (show_properties.boolValue == true) {
        return height * 8 + (height * 0.5f);
      } else {
        return height + (height * 0.5f);
      }
    }

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
      float label_width = 50;

      GUIStyle foldout_style = new GUIStyle(EditorStyles.foldout);
      foldout_style.fontStyle = FontStyle.Bold;

      Rect properties_pos = new Rect(position.x,      position.y, 20, height);
      Rect enabled_pos    = new Rect(position.x + 89, position.y, 20, height);

      show_properties.boolValue = EditorGUI.Foldout(properties_pos, show_properties.boolValue, property.displayName, foldout_style);

      SerializedProperty enabled = property.FindPropertyRelative("Enabled");

      enabled.boolValue = EditorGUI.Toggle(enabled_pos, enabled.boolValue);

      using (new EditorGUI.DisabledScope(enabled.boolValue == false)) {
        float row_one  = position.y + height;
        float x_offset = position.x + label_width;

        if (show_properties.boolValue == true) {
          Rect label_pos        = new Rect(position.x,        row_one, label_width, double_height);
          Rect hours_pos        = new Rect(x_offset + 12.5f,  row_one, 25,          height);
          Rect minutes_pos      = new Rect(x_offset + 62.5f,  row_one, 25,          height);
          Rect seconds_pos      = new Rect(x_offset + 112.5f, row_one, 25,          height);
          Rect milliseconds_pos = new Rect(x_offset + 172.5f, row_one, 35,          height);

          SerializedProperty time = property.FindPropertyRelative("Time");

          GUIStyle helper_style = new GUIStyle(GUI.skin.label);
          helper_style.alignment = TextAnchor.MiddleCenter;

          EditorGUI.LabelField(label_pos, "Time", helper_style);

          using (new EditorGUI.DisabledScope(EditorApplication.isPlaying == true)) {
            SerializedProperty hours        = time.FindPropertyRelative("Hours");
            SerializedProperty minutes      = time.FindPropertyRelative("Minutes");
            SerializedProperty seconds      = time.FindPropertyRelative("Seconds");
            SerializedProperty milliseconds = time.FindPropertyRelative("Milliseconds");

            GUIStyle style = new GUIStyle(GUI.skin.textField);
            style.alignment = TextAnchor.MiddleCenter;

            hours.intValue        = EditorGUI.IntField(hours_pos,        hours.intValue,        style);
            minutes.intValue      = EditorGUI.IntField(minutes_pos,      minutes.intValue,      style);
            seconds.intValue      = EditorGUI.IntField(seconds_pos,      seconds.intValue,      style);
            milliseconds.intValue = EditorGUI.IntField(milliseconds_pos, milliseconds.intValue, style);

            float row_three = position.y + double_height;

            Rect label_hours_pos        = new Rect(x_offset,       row_three, 50, height);
            Rect label_minutes_pos      = new Rect(x_offset + 50,  row_three, 50, height);
            Rect label_seconds_pos      = new Rect(x_offset + 100, row_three, 50, height);
            Rect label_milliseconds_pos = new Rect(x_offset + 150, row_three, 80, height);

            EditorGUI.LabelField(label_hours_pos,        "hours",        helper_style);
            EditorGUI.LabelField(label_minutes_pos,      "minutes",      helper_style);
            EditorGUI.LabelField(label_seconds_pos,      "seconds",      helper_style);
            EditorGUI.LabelField(label_milliseconds_pos, "milliseconds", helper_style);
          }

          Rect curve_row = new Rect(position.x, position.y + (height * 3), position.width, height * 5);

          SerializedProperty curve = property.FindPropertyRelative("Curve");

          EditorGUI.CurveField(curve_row, curve, Color.white, new Rect(0, 0, 1, 1), GUIContent.none);
        }
      }
		}
	}
}
