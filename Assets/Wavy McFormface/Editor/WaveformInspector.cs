using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace bsgbryan {

  [CustomEditor(typeof(WavyMcFormface))]
  public class WaveformInspector : Editor {

    private WavyMcFormface generator;

    private SerializedObject serialized_configie;
    private SerializedObject serialized_generator;

    private SerializedProperty configie;

    private SerializedProperty volume;
    private SerializedProperty context;
    private SerializedProperty noise;
    private SerializedProperty disruption;
    private SerializedProperty rise_and_decay;
    private SerializedProperty harmonics;
    private SerializedProperty frequency_limit;
    private SerializedProperty waves;

    private void OnEnable () {
      generator            = (WavyMcFormface) target;
      serialized_generator = new SerializedObject(generator);

      if (generator.Configie != null) {
        generator.Configie.Generator = generator;

        serialized_configie = new SerializedObject(generator.Configie);

        volume          = serialized_configie.FindProperty("Volume");
        context         = serialized_configie.FindProperty("PlayControls");
        waves           = serialized_configie.FindProperty("WaveVolumes");
        noise           = serialized_configie.FindProperty("Noise");
        disruption      = serialized_configie.FindProperty("WibbleWobble");
        rise_and_decay  = serialized_configie.FindProperty("Envelope");
        harmonics       = serialized_configie.FindProperty("Harmonics");
        frequency_limit = serialized_configie.FindProperty("FrequencyLimit");
      }

      configie = serialized_generator.FindProperty("Configie");
    }

    private void OnDisable () {
      // Debug.Log ("OnDisable was called...");
    }

    private void OnDestroy () {
      // Debug.Log ("OnDestroy was called...");
    }

    public override void OnInspectorGUI () {
      // DrawDefaultInspector();

      serialized_generator.Update();

      if (serialized_configie != null) {
        serialized_configie.Update();

        GUILayout.Space(10f);

        EditorGUILayout.ObjectField(configie);

        EditorGUILayout.PropertyField(volume);
        EditorGUILayout.PropertyField(waves);
        EditorGUILayout.PropertyField(context);

        EditorGUILayout.PropertyField(rise_and_decay);
        EditorGUILayout.PropertyField(harmonics);
        EditorGUILayout.PropertyField(frequency_limit);
        EditorGUILayout.PropertyField(disruption);
        EditorGUILayout.PropertyField(noise);
      } else if (configie != null) {
        EditorGUILayout.ObjectField(configie);
      }

      serialized_generator.ApplyModifiedProperties();

      if (serialized_configie != null) {
        serialized_configie.ApplyModifiedProperties();
      }
    }
  }

}
