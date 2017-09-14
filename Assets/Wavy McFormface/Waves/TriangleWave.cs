using UnityEngine;

public class TriangleWave : Waveform {

	public override float Generate(float ph, float amplitude) {
		if (ph < Mathf.PI) {
			return -amplitude + (2f * amplitude / Mathf.PI) * ph;
		} else {
			return (amplitude * 3f) - (2f * amplitude / Mathf.PI) * ph;
		}
	}

}
