using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using bsgbryan;

public class WibblyWobbly : MonoBehaviour {

  public GameObject player;

  private WavyMcFormface wavy;

  void Awake() {
		wavy = GetComponent<WavyMcFormface>() as WavyMcFormface;

    wavy.Configie.Volume.Level = 0f;
  }

	void Start() {
    wavy.StartPlaying(3, 5);
	}

  void Update() {
    Transform my_trans     = GetComponent<Transform>() as Transform;
    Transform player_trans = player.GetComponent<Transform>() as Transform;

    float distance = Vector3.Distance(my_trans.position, player_trans.position);

    if (10f - distance > 0f) {
      wavy.Configie.Volume.Level = (10f - distance) * 0.05f;

      if (5f - distance > 0f) {
        UpdateTangents((5f - distance) * 0.4f);
      }
    } else {
      wavy.Configie.Volume.Level = 0f;

      UpdateTangents(0f);
    }
	}

  void UpdateTangents(float amount) {
    float ninety_degrees = 1.5708f;

    AnimationCurve wibbly_wobbly = wavy.Configie.WibbleWobble.Curve;

    Keyframe left_key  = wibbly_wobbly.keys[0];
    Keyframe right_key = wibbly_wobbly.keys[1];

    left_key.outTangent = ninety_degrees * amount;
    right_key.inTangent = ninety_degrees * amount;

    wibbly_wobbly.MoveKey(0, left_key);
    wibbly_wobbly.MoveKey(1, right_key);
  }
}
