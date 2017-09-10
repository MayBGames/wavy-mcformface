using UnityEngine;

public class SineWave : Waveform {

	public override float Generate(float ph, float amplitude) {
		return Mathf.Sin(ph) * amplitude;
	}

}
