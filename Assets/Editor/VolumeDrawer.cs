using UnityEngine;
using UnityEditor;

namespace WaveformComposer {
	[CustomPropertyDrawer(typeof(WaveformComposer.Volume))]
	public class VolumeDrawer : PropertyDrawer {

    public override float GetPropertyHeight (SerializedProperty property, GUIContent label) {
      return EditorGUI.GetPropertyHeight(property) * 2;
    }

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
      float height = EditorGUI.GetPropertyHeight(property);

      Rect label_pos  = new Rect(position.x,      position.y + (height * 0.5f), 95,                  height);
      Rect slider_pos = new Rect(position.x + 95, position.y + (height * 0.5f), position.width - 95, height);

      SerializedProperty volume = property.FindPropertyRelative("Level");

      GUIStyle helper_style = new GUIStyle(GUI.skin.label);
      helper_style.alignment = TextAnchor.MiddleLeft;
      helper_style.fontStyle = FontStyle.Bold;

      EditorGUI.LabelField(label_pos, "Master Volume", helper_style);

      volume.floatValue = EditorGUI.Slider(slider_pos, volume.floatValue, 0.1f, 1);
		}
	}
}
