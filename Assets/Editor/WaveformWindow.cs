using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace WaveformComposer {
	public class WaveformWindow : EditorWindow {
		public static WaveformWindow instance;

		public static void Initialize() {
			instance = (WaveformWindow) EditorWindow.GetWindow(typeof(WaveformWindow), true, "Create a new Waveform");
		}
	}
}