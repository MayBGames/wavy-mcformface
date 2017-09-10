using UnityEngine;
using UnityEditor;

namespace WaveformComposer {
	[CustomPropertyDrawer(typeof(WaveformComposer.Noise))]
	public class NoiseDrawer : PropertyDrawer {

    private bool show_props = true;

    public override float GetPropertyHeight (SerializedProperty property, GUIContent label) {
      float height = EditorGUI.GetPropertyHeight(property);

      if (show_props == true) {
        return height * 8 + (height * 0.5f);
      } else {
        return height;
      }
    }

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			float height = EditorGUI.GetPropertyHeight(property);

      GUIStyle foldout_style = new GUIStyle(EditorStyles.foldout);
      foldout_style.fontStyle = FontStyle.Bold;

      Rect properties_pos = new Rect(position.x,      position.y, 20, height);
      Rect enabled_pos    = new Rect(position.x + 37, position.y, 20, height);

      SerializedProperty show_properties = property.FindPropertyRelative("ShowProperties");

      show_properties.boolValue = EditorGUI.Foldout(properties_pos, show_properties.boolValue, property.displayName, foldout_style);

      show_props = show_properties.boolValue;

      SerializedProperty enabled = property.FindPropertyRelative("Enabled");

      enabled.boolValue = EditorGUI.Toggle(enabled_pos, enabled.boolValue);

      using (new EditorGUI.DisabledScope(enabled.boolValue == false)) {
        if (show_properties.boolValue == true) {
          Rect variance_label_pos  = new Rect(position.x,      position.y + height,       50,                  height);
          Rect variance_slider_pos = new Rect(position.x + 55, position.y + height,       position.width - 55, height);
          Rect level_label_pos     = new Rect(position.x,      position.y + (height * 2), 50,                  height);
          Rect level_slider_pos    = new Rect(position.x + 55, position.y + (height * 2), position.width - 55, height);
          Rect curve_pos           = new Rect(position.x,      position.y + (height * 3), position.width,      height * 5);

          SerializedProperty variance = property.FindPropertyRelative("Variance");
          SerializedProperty level    = property.FindPropertyRelative("Level");
          SerializedProperty curve    = property.FindPropertyRelative("Curve");

          SerializedProperty min = variance.FindPropertyRelative("Min");
          SerializedProperty max = variance.FindPropertyRelative("Max");

          GUIStyle helper_style = new GUIStyle(GUI.skin.label);
          helper_style.alignment = TextAnchor.MiddleCenter;

          EditorGUI.LabelField(variance_label_pos, "variance", helper_style);
          EditorGUI.LabelField(level_label_pos,    "level",    helper_style);

          float low  = min.floatValue;
          float high = max.floatValue;

          EditorGUI.BeginChangeCheck();

          EditorGUI.MinMaxSlider(variance_slider_pos, ref low, ref high, 0f, 1f);

          if (EditorGUI.EndChangeCheck()) {
            if (low != min.floatValue) {
              min.floatValue = low;
            }

            if (high != max.floatValue) {
              max.floatValue = high;
            }
    			}

          level.floatValue = EditorGUI.Slider(level_slider_pos, level.floatValue, 0f, 10f);

          EditorGUI.CurveField(curve_pos, curve, Color.white, new Rect(0, 0, 1, 1), GUIContent.none);
        }
      }
		}
	}
}
