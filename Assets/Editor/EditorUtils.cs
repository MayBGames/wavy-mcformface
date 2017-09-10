using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace WaveformComposer {
	public static class EditorUtils {

    public static T CreateAsset<T>(string path)
      where T : ScriptableObject {
      T dataClass = (T) ScriptableObject.CreateInstance<T>();
      AssetDatabase.CreateAsset(dataClass, path);
      AssetDatabase.Refresh();
      AssetDatabase.SaveAssets();
      return dataClass;
    }
	}
}
