using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using bsgbryan; // Use me ;-)

public class CubeOrgan : MonoBehaviour {

  private WavyMcFormface wavy;

  void Awake() {
    wavy = GetComponent<WavyMcFormface>() as WavyMcFormface;
  }

  void OnCollisionEnter(Collision other) {
    wavy.StartPlaying(3, 5); // D in the sixth octave (they're zero-based)
  }

  void OnCollisionExit(Collision other) {
    wavy.StopPlaying();
  }
}
