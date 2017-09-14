/*Thomas Ingram 2016*/
// Taken from: http://vertx.xyz/unity-animationcurve-lerp/

using UnityEngine;
using System.Collections;

public static class CurveExtensions {
    public static AnimationCurve Lerp (this AnimationCurve curve1, AnimationCurve curve2, float t){
        AnimationCurve curve1_ToLerp = new AnimationCurve(curve1.keys);
        AnimationCurve curve2_ToLerp = new AnimationCurve(curve2.keys);
        for(int i = 0; i<curve2.keys.Length; i++){
            bool found = false;
            for(int j = 0; j<curve1_ToLerp.length; j++){
                if(curve2.keys[i].time == curve1_ToLerp.keys[j].time){
                    found = true;
                }
            }
            if(!found){
                curve1_ToLerp.AddKey(curve2.keys[i].time, curve1.Evaluate(curve2.keys[i].time));
            }
        }

        for(int i = 0; i<curve1.keys.Length; i++){
            bool found = false;
            for(int j = 0; j<curve2_ToLerp.length; j++){
                if(curve1.keys[i].time == curve2_ToLerp.keys[j].time){
                    found = true;
                }
            }
            if(!found){
                curve2_ToLerp.AddKey(curve1.keys[i].time, curve2.Evaluate(curve1.keys[i].time));
            }
        }

        AnimationCurve newCurve = new AnimationCurve(curve1_ToLerp.keys);
        for(int i = 0; i<newCurve.keys.Length; i++){
            newCurve.MoveKey(i, curve1_ToLerp[i].Lerp(curve2_ToLerp[i], t));
        }
        return newCurve;
    }

    public static bool EqualTo(this AnimationCurve curve1, AnimationCurve curve2) {
      if (curve1.keys.Length != curve2.keys.Length) {
        return false;
      }

      for (int i = 0; i < curve1.keys.Length; i++) {
        Keyframe key = curve1.keys[i];
        Keyframe two = curve2.keys[i];

        if (key.inTangent  != two.inTangent  ||
            key.outTangent != two.outTangent ||
            key.time       != two.time       ||
            key.value      != two.value
          ) {

          // Debug.Log("in " + (key.inTangent != two.inTangent) + key.inTangent + "::" + two.inTangent);
          // Debug.Log("out " + (key.outTangent != two.outTangent) + key.outTangent + "::" + two.outTangent);
          // Debug.Log("time " + (key.time != two.time) + key.time + "::" + two.time);
          // Debug.Log("value " + (key.value != two.value) + key.value + "::" + two.value);

          return false;
        }
      }

      return true;
    }

    public static Keyframe Lerp (this Keyframe key1, Keyframe key2, float t){
        return new Keyframe(Mathf.Lerp(key1.time,key2.time,t),
            Mathf.Lerp(key1.value,key2.value,t),
            Mathf.Lerp(key1.inTangent,key2.inTangent,t),
            Mathf.Lerp(key1.outTangent,key2.outTangent,t));
    }
}
