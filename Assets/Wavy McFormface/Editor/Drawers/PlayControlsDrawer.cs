using UnityEngine;
using UnityEditor;

namespace bsgbryan {
  [CustomPropertyDrawer(typeof(bsgbryan.PlayControls))]
  public class PlayControlsDrawer : PropertyDrawer {

    private SerializedProperty show_properties;

    private string[] note_names = new string[] {
      "C",     "C#/Db", "D",     "D#/Eb",
      "E",     "F",     "F#/Gb", "G",
      "G#/Ab", "A",     "A#/Bb", "B"
    };

    private bool[] playing = new bool[12] {
      false, false, false, false,
      false, false, false, false,
      false, false, false, false
    };

    private float height;
    private float half_height;
    private float quarter_height;
    private float double_height;
    private float triple_height;

    public override float GetPropertyHeight (SerializedProperty property, GUIContent label) {
      show_properties = property.FindPropertyRelative("ShowProperties");

      if (height == 0f) {
        height = EditorGUI.GetPropertyHeight(property);

        half_height    = height * 0.5f;
        quarter_height = height * 0.25f;
        double_height  = height * 2f;
        triple_height  = height * 3f;
      }

      if (show_properties.boolValue == true) {
        return height * 10f + half_height;
      } else {
        return height + half_height;
      }
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
      float width        = position.width * 0.5f;
      float label_offset = width          * 0.5f;
      float label_width  = label_offset   * 1.25f;

      GUIStyle foldout_style = new GUIStyle(EditorStyles.foldout);
      foldout_style.fontStyle = FontStyle.Bold;

      Rect foldout_pos = new Rect(position.x, position.y, 20f, height);

      show_properties.boolValue = EditorGUI.Foldout(foldout_pos, show_properties.boolValue, property.displayName, foldout_style);

      if (show_properties.boolValue == true) {
        GUIStyle helper_style = new GUIStyle(GUI.skin.label);
        GUIStyle input_style  = new GUIStyle(GUI.skin.textField);

        helper_style.alignment = TextAnchor.MiddleCenter;
        input_style.alignment  = TextAnchor.MiddleCenter;

        float row_one = position.y + height;
        float row_two = position.y + double_height;
        float input_width = position.width - label_width;
        float x_label_offset = position.x + label_width;

        Rect low_label_pos     = new Rect(position.x,     row_one, label_width, height);
        Rect current_label_pos = new Rect(position.x,     row_two, label_width, height);
        Rect current_input_pos = new Rect(x_label_offset, row_two, input_width, height);

        EditorGUI.LabelField(low_label_pos,     "octave range",   helper_style);
        EditorGUI.LabelField(current_label_pos, "current octave", helper_style);

        SerializedProperty low_octave  = property.FindPropertyRelative("LowOctave");
        SerializedProperty high_octave = property.FindPropertyRelative("HighOctave");

        bsgbryan.ConfigieMcWaveface configie = (bsgbryan.ConfigieMcWaveface) property.serializedObject.targetObject;

        if (configie.PlayControls.CurrentOctave > high_octave.intValue) {
          configie.PlayControls.CurrentOctave = high_octave.intValue;
        }

        if (configie.PlayControls.CurrentOctave < low_octave.intValue) {
          configie.PlayControls.CurrentOctave = low_octave.intValue;
        }

        configie.PlayControls.CurrentOctave = EditorGUI.IntSlider(current_input_pos, configie.PlayControls.CurrentOctave, low_octave.intValue, high_octave.intValue);

        Rect octave_range_pos = new Rect(x_label_offset + 5f,               row_one, input_width - 50f, height);
        Rect range_label_pos  = new Rect(position.x + position.width - 40f, row_one, 40f,               height);

        float low  = (float) low_octave.intValue;
        float high = (float) high_octave.intValue;

        EditorGUI.LabelField(range_label_pos, ((int) low) + " - " + ((int) high),   helper_style);

        EditorGUI.BeginChangeCheck();

        EditorGUI.MinMaxSlider(octave_range_pos, ref low, ref high, 0, 9);

        if (EditorGUI.EndChangeCheck()) {
          int new_low  = (int) Mathf.Round(low);
          int new_high = (int) Mathf.Round(high);

          if (new_low != low_octave.intValue) {
            low_octave.intValue = new_low;
          }

          if (new_high != high_octave.intValue) {
            high_octave.intValue = new_high;
          }

          if (configie.PlayControls.CurrentOctave > high_octave.intValue) {
            configie.PlayControls.CurrentOctave = high_octave.intValue;
          }

          if (configie.PlayControls.CurrentOctave < low_octave.intValue) {
            configie.PlayControls.CurrentOctave = low_octave.intValue;
          }
        }

        float note_label_width = position.width * 0.25f;
        float button_width     = note_label_width * 0.5f;

        GUIStyle label_style  = new GUIStyle(GUI.skin.label);
        label_style.alignment = TextAnchor.MiddleCenter;

        int note = 0;

        for (int i = 0; i < 3; i++) {
          for (int j = 0; j < 4; j++) {
            float label_x  = position.x + (note_label_width * j);
            float play_x   = label_x    + (note_label_width * 0.5f);

            float row_height_offset = quarter_height * (i + 1);

            float label_y  = position.y + triple_height + ((height * i) * 2f)                  + row_height_offset;
            float button_y = position.y + triple_height + (((height * (i + 1)) * 2f) - height) + row_height_offset;

            Rect label_pos = new Rect(label_x, label_y,  note_label_width, height);
            Rect tap_pos   = new Rect(label_x, button_y, button_width,     height);
            Rect play_pos  = new Rect(play_x,  button_y, button_width,     height);

            EditorGUI.LabelField(label_pos, note_names[note], label_style);

            if (GUI.Button(tap_pos, "tap")) {
              configie.Generator.Tap(note, configie.PlayControls.CurrentOctave);
            }

            Color original_background_color = GUI.backgroundColor;

            GUI.backgroundColor = playing[note] ? Color.green : original_background_color;

            if (GUI.Button(play_pos, playing[note] ? "stop" : "play")) {
              if (playing[note] == true) {
                configie.Generator.StopPlaying();
              } else {
                configie.Generator.StartPlaying(note, configie.PlayControls.CurrentOctave);
              }

              playing[note] = !playing[note];
            }

            GUI.backgroundColor = original_background_color;

            ++note;
          }
        }
      }
    }
  }
}
