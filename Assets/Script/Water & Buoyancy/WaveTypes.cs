using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeOfWater{
	StraightWaves_X,
	StraightWaves_Z,
	UpBownMvt,
	RollingWaves,
	MovingSea
}
public class WaveTypes : MonoBehaviour {

	//Sinus waves
	public static float SinXWave(
		TypeOfWater type,
		Vector3 position, 
		float speed, 
		float scale,
		float waveDistance,
		float noiseStrength, 
		float noiseWalk,
        float timeSinceStart) 
	{
        float x = position.x;
        float y = 0f;
        float z = position.z;

        //Using only x or z will produce straight waves
		//Using only y will produce an up/down movement
		//x + y + z rolling waves
		//x * z produces a moving sea without rolling waves

		float waveType;
		switch (type)
		{
			case TypeOfWater.StraightWaves_X:
				waveType = x;
			break;			
			case TypeOfWater.StraightWaves_Z:
				waveType = z;
			break;
			case TypeOfWater.UpBownMvt:
				waveType = y;
			break;
			case TypeOfWater.RollingWaves:
				waveType = x + y + z;
			break;
			case TypeOfWater.MovingSea:
				waveType = x * z;
			break;
			default:
				waveType = x;
			break;
		}

        y += Mathf.Sin((timeSinceStart * speed + waveType) / waveDistance) * scale;

        //Add noise to make it more realistic
        y += Mathf.PerlinNoise(x + noiseWalk, y + Mathf.Sin(timeSinceStart * 0.1f)) * noiseStrength;

        return y;
	}
}
