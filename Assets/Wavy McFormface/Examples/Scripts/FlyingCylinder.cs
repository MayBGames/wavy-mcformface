using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using bsgbryan;

public class FlyingCylinder : MonoBehaviour {

  public GameObject player;

  private WavyMcFormface wavy;

  void Awake() {
		wavy = GetComponent<WavyMcFormface>() as WavyMcFormface;

    wavy.Configie.Volume.Level = 0f;
    // wavy.Configie.Volume.Level = 0.5f;
  }

	void Start() {
    // Note D (the fourth note) in the sixth octave
    wavy.StartPlaying(3, 5);
	}

	void Update() {
    Transform my_trans     = GetComponent<Transform>() as Transform;
    Transform player_trans = player.GetComponent<Transform>() as Transform;

    float distance = Vector3.Distance(my_trans.position, player_trans.position);

    if (10f - distance > 0f) {
      float distance_adjusted = (10f - distance) * 0.05f;

      // The volume will rise as you approach
      wavy.Configie.Volume.Level = distance_adjusted;

      // WaveVolumes.Selected[1] is the Square Wave
      // wavy.Configie.WaveVolumes.Selected[1] = distance_adjusted;
    } else {
      // Silence the entire Formface
      wavy.Configie.Volume.Level = 0f;

      // Silence the Square Wave
      // wavy.Configie.WaveVolumes.Selected[1] = 0f;
    }
	}
}
