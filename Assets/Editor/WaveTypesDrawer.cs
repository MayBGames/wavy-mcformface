using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace WaveformComposer {
	[CustomPropertyDrawer(typeof(WaveformComposer.WaveTypes))]
	public class WaveTypesDrawer : PropertyDrawer {

    private bool sine_selected;
    private bool square_selected;
    private bool triangle_selected;
    private bool sawtooth_selected;

    private bool show_properties = true;

    public override float GetPropertyHeight (SerializedProperty property, GUIContent label) {
      float height = EditorGUI.GetPropertyHeight(property);

      if (show_properties == true) {
        return height * 5 + (height * 0.5f);
      } else {
        return height;
      }
    }

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
      float height = EditorGUI.GetPropertyHeight(property);
      float width  = (position.width * 0.5f) - 10f;

      GUIStyle foldout_style = new GUIStyle(EditorStyles.foldout);
      foldout_style.fontStyle = FontStyle.Bold;

      Rect foldout_pos = new Rect(position.x, position.y, 20, height);

      show_properties = EditorGUI.Foldout(foldout_pos, show_properties, property.displayName, foldout_style);

      if (show_properties == true) {
        Rect sine_toggle_pos     = new Rect(position.x,              position.y + height,       width, height);
        Rect square_toggle_pos   = new Rect(position.x + width + 10, position.y + height,       width, height);
        Rect triangle_toggle_pos = new Rect(position.x,              position.y + (height * 3), width, height);
        Rect sawtooth_toggle_pos = new Rect(position.x + width + 10, position.y + (height * 3), width, height);

        Rect sine_slider_pos     = new Rect(position.x + 5,          position.y + (height * 2), width, height);
        Rect square_slider_pos   = new Rect(position.x + width + 15, position.y + (height * 2), width, height);
        Rect triangle_slider_pos = new Rect(position.x + 5,          position.y + (height * 4), width, height);
        Rect sawtooth_slider_pos = new Rect(position.x + width + 15, position.y + (height * 4), width, height);

        SerializedProperty selected = property.FindPropertyRelative("Selected");

        float sine_volume     = selected.GetArrayElementAtIndex(0).floatValue;
        float square_volume   = selected.GetArrayElementAtIndex(1).floatValue;
        float triangle_volume = selected.GetArrayElementAtIndex(2).floatValue;
        float sawtooth_volume = selected.GetArrayElementAtIndex(3).floatValue;

        EditorGUI.LabelField(sine_toggle_pos,     "Sine");
        EditorGUI.LabelField(square_toggle_pos,   "Square");
        EditorGUI.LabelField(triangle_toggle_pos, "Triangle");
        EditorGUI.LabelField(sawtooth_toggle_pos, "Sawtooth");

        sine_volume     = EditorGUI.Slider(sine_slider_pos,     sine_volume,     0f, 1f);
        square_volume   = EditorGUI.Slider(square_slider_pos,   square_volume,   0f, 1f);
        triangle_volume = EditorGUI.Slider(triangle_slider_pos, triangle_volume, 0f, 1f);
        sawtooth_volume = EditorGUI.Slider(sawtooth_slider_pos, sawtooth_volume, 0f, 1f);

        SerializedProperty sine_index     = selected.GetArrayElementAtIndex(0);
        SerializedProperty square_index   = selected.GetArrayElementAtIndex(1);
        SerializedProperty triangle_index = selected.GetArrayElementAtIndex(2);
        SerializedProperty sawtooth_index = selected.GetArrayElementAtIndex(3);

        sine_index.floatValue     = sine_volume;
        square_index.floatValue   = square_volume;
        triangle_index.floatValue = triangle_volume;
        sawtooth_index.floatValue = sawtooth_volume;
      }
		}
	}
}
