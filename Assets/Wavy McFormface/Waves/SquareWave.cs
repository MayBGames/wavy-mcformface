using UnityEngine;

public class SquareWave : Waveform {

	public override float Generate(float ph, float amplitude) {
		if (ph < Mathf.PI) {
			return amplitude;
		} else {
			return -amplitude;
		}
	}

}
