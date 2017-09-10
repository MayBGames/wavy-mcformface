using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace WaveformComposer {

  [CustomEditor(typeof(WaveGenerator))]
  public class WaveformInspector : Editor {

    private WaveGenerator generator;

    private SerializedObject serialized_config;
    private SerializedObject serialized_generator;

    private SerializedProperty config;

    private SerializedProperty volume;
    private SerializedProperty context;
    private SerializedProperty noise;
    private SerializedProperty disruption;
    private SerializedProperty rise_and_decay;
    private SerializedProperty harmonics;
    private SerializedProperty frequency_limit;
    private SerializedProperty waves;

    private void OnEnable () {
      generator            = (WaveGenerator) target;
      serialized_generator = new SerializedObject(generator);

      if (generator.Config != null) {
        serialized_config = new SerializedObject(generator.Config);

        volume          = serialized_config.FindProperty("Volume");
        context         = serialized_config.FindProperty("Context");
        waves           = serialized_config.FindProperty("Waves");
        noise           = serialized_config.FindProperty("Noise");
        disruption      = serialized_config.FindProperty("Disruption");
        rise_and_decay  = serialized_config.FindProperty("RiseAndDecay");
        harmonics       = serialized_config.FindProperty("Harmonics");
        frequency_limit = serialized_config.FindProperty("FrequencyLimit");
      }

      config = serialized_generator.FindProperty("Config");
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

      if (serialized_config != null) {
        serialized_config.Update();

        EditorGUILayout.ObjectField(config);

        EditorGUILayout.PropertyField(volume);
        EditorGUILayout.PropertyField(waves);
        EditorGUILayout.PropertyField(context);

        if (GUILayout.Button("Play")) {
          generator.StartPlaying();
        }

        if (GUILayout.Button("Stop")) {
          generator.StopPlaying();
        }

        EditorGUILayout.PropertyField(rise_and_decay);
        EditorGUILayout.PropertyField(harmonics);
        EditorGUILayout.PropertyField(frequency_limit);
        EditorGUILayout.PropertyField(disruption);
        EditorGUILayout.PropertyField(noise);
      } else if (config != null) {
        EditorGUILayout.ObjectField(config);
      }

      serialized_generator.ApplyModifiedProperties();

      if (serialized_config != null) {
        serialized_config.ApplyModifiedProperties();
      }
    }
  }

}
