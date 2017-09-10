using UnityEngine;

public class SawtoothWave : Waveform {

	public override float Generate(float ph, float amplitude) {
		return amplitude - (amplitude / Mathf.PI * ph);
	}

}
