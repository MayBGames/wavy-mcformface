using UnityEngine;
using UnityEditor;

namespace WaveformComposer {
	[CustomPropertyDrawer(typeof(WaveformComposer.SonicContext))]
	public class SonicContextDrawer : PropertyDrawer {

    private bool show_details = true;

    private string[] note_names = new string[] {
      "C",
      "C#/Db",
      "D",
      "D#/Eb",
      "E",
      "F",
      "F#/Gb",
      "G",
      "G#/Ab",
      "A",
      "A#/Bb",
      "B"
    };

    public override float GetPropertyHeight (SerializedProperty property, GUIContent label) {
      if (show_details == true) {
        float height = EditorGUI.GetPropertyHeight(property);
        return height * 3 + (height * 0.5f);
      } else {
        return EditorGUI.GetPropertyHeight(property);
      }
    }

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
      float height       = EditorGUI.GetPropertyHeight(property);
      float width        = position.width / 2;
      float label_offset = width / 2;
      float label_width  = label_offset * 1.25f;
      float input_width  = 20;

      GUIStyle foldout_style = new GUIStyle(EditorStyles.foldout);
      foldout_style.fontStyle = FontStyle.Bold;

      Rect foldout_pos = new Rect(position.x, position.y, 20, height);

      show_details = EditorGUI.Foldout(foldout_pos, show_details, property.displayName, foldout_style);

      if (show_details == true) {
        GUIStyle helper_style = new GUIStyle(GUI.skin.label);
        GUIStyle input_style  = new GUIStyle(GUI.skin.textField);

        helper_style.alignment = TextAnchor.MiddleCenter;
        input_style.alignment  = TextAnchor.MiddleCenter;

        Rect low_label_pos     = new Rect(position.x,                                 position.y + height,       label_width, height);
        Rect current_label_pos = new Rect(position.x,                                 position.y + (height * 2), label_width, height);
        Rect note_label_pos    = new Rect(position.x + label_width + input_width + 5, position.y + (height * 2), 35,          height);
        Rect current_input_pos = new Rect(position.x + label_width,                   position.y + (height * 2), input_width, height);

        EditorGUI.LabelField(low_label_pos,     "octave range",   helper_style);
        EditorGUI.LabelField(current_label_pos, "current octave", helper_style);
        EditorGUI.LabelField(note_label_pos,    "note",           helper_style);

        SerializedProperty low_octave     = property.FindPropertyRelative("LowOctave");
        SerializedProperty high_octave    = property.FindPropertyRelative("HighOctave");
        SerializedProperty current_octave = property.FindPropertyRelative("CurrentOctave");
        SerializedProperty note_index     = property.FindPropertyRelative("NoteIndex");

        if (current_octave.intValue > high_octave.intValue) {
          current_octave.intValue = high_octave.intValue;
        }

        if (current_octave.intValue < low_octave.intValue) {
          current_octave.intValue = low_octave.intValue;
        }

        current_octave.intValue = EditorGUI.IntField(current_input_pos, current_octave.intValue, input_style);

        Rect octave_range_pos = new Rect(position.x + label_width + 5,                position.y + height,       position.width - label_width - 50,               height);
        Rect note_range_pos   = new Rect(position.x + label_width + input_width + 45, position.y + (height * 2), position.width - label_width - input_width - 95, height);
        Rect range_label_pos  = new Rect(position.x + position.width - 40,            position.y + height,       40,                                              height);
        Rect note_name_pos    = new Rect(position.width - 30,                         position.y + (height * 2), 40,                                              height);

        float low  = (float) low_octave.intValue;
        float high = (float) high_octave.intValue;

        float note = (float) note_index.intValue;

        EditorGUI.LabelField(range_label_pos, ((int) low) + " - " + ((int) high), helper_style);
        EditorGUI.LabelField(note_name_pos,   note_names[note_index.intValue],    helper_style);

        EditorGUI.BeginChangeCheck();

        EditorGUI.MinMaxSlider(octave_range_pos, ref low,  ref high, 0, 9);
        EditorGUI.MinMaxSlider(note_range_pos,   ref note, ref note, 0, 11);

        if (EditorGUI.EndChangeCheck()) {
          int new_low  = (int) Mathf.Round(low);
          int new_high = (int) Mathf.Round(high);
          int new_note = (int) Mathf.Round(note);

          if (new_low != low_octave.intValue) {
    				low_octave.intValue = new_low;
          }

          if (new_high != high_octave.intValue) {
    				high_octave.intValue = new_high;
          }

          if (new_note != note_index.intValue) {
    				note_index.intValue = new_note;
          }
  			}
      }
		}
	}
}
