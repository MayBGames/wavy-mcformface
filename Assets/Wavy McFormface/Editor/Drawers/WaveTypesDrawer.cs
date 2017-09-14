using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace bsgbryan {
	[CustomPropertyDrawer(typeof(bsgbryan.WaveTypes))]
	public class WaveTypesDrawer : PropertyDrawer {

    private SerializedProperty show_properties;

    private bool sine_selected;
    private bool square_selected;
    private bool triangle_selected;
    private bool sawtooth_selected;

    public override float GetPropertyHeight (SerializedProperty property, GUIContent label) {
      show_properties = property.FindPropertyRelative("ShowProperties");

      float height      = EditorGUI.GetPropertyHeight(property);
      float half_height = height * 0.5f;

      if (show_properties.boolValue == true) {
        return (height * 5f) + half_height;
      } else {
        return height + half_height;
      }
    }

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
      float height = EditorGUI.GetPropertyHeight(property);
      float width  = (position.width * 0.5f) - 10f;

      GUIStyle foldout_style = new GUIStyle(EditorStyles.foldout);
      foldout_style.fontStyle = FontStyle.Bold;

      Rect foldout_pos = new Rect(position.x, position.y, 20, height);

      show_properties.boolValue = EditorGUI.Foldout(foldout_pos, show_properties.boolValue, property.displayName, foldout_style);

      if (show_properties.boolValue == true) {
        float row_one   = position.y + height;
        float row_two   = position.y + (height * 2);
        float row_three = position.y + (height * 3);
        float row_four  = position.y + (height * 4);

        float right_column        = position.x   + width;
        float right_colum_label   = right_column + 10;
        float right_column_slider = right_column + 15;
        float left_column_slider  = position.x   + 5;

        Rect sine_label_pos     = new Rect(position.x,        row_one,   width, height);
        Rect square_label_pos   = new Rect(right_colum_label, row_one,   width, height);
        Rect triangle_label_pos = new Rect(position.x,        row_three, width, height);
        Rect sawtooth_label_pos = new Rect(right_colum_label, row_three, width, height);

        Rect sine_slider_pos     = new Rect(left_column_slider,  row_two,  width, height);
        Rect square_slider_pos   = new Rect(right_column_slider, row_two,  width, height);
        Rect triangle_slider_pos = new Rect(left_column_slider,  row_four, width, height);
        Rect sawtooth_slider_pos = new Rect(right_column_slider, row_four, width, height);

        SerializedProperty selected = property.FindPropertyRelative("Selected");

        float sine_volume     = selected.GetArrayElementAtIndex(0).floatValue;
        float square_volume   = selected.GetArrayElementAtIndex(1).floatValue;
        float triangle_volume = selected.GetArrayElementAtIndex(2).floatValue;
        float sawtooth_volume = selected.GetArrayElementAtIndex(3).floatValue;

        EditorGUI.LabelField(sine_label_pos,     "Sine");
        EditorGUI.LabelField(square_label_pos,   "Square");
        EditorGUI.LabelField(triangle_label_pos, "Triangle");
        EditorGUI.LabelField(sawtooth_label_pos, "Sawtooth");

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
