using UnityEngine;
using System.Collections;

//To be able to change the different physics parameters real time
public class DebugPhysics : MonoBehaviour 
{
    public static DebugPhysics current;

    //Force 2 - Pressure Drag Force
    [Header("Force 2 - Pressure Drag Force")]
    public float velocityReference;

    [Header("Pressure Drag")]
    public float PressureCoef_1 = 10f;   //C_PD1
    public float PressureCoef_2 = 10f;   //C_PD2
    [Range(0,1)] public float PressureFalloff = 0.5f;    //F_p

    [Header("Suction Drag")]
    public float SuctionCoef_1 = 10f;   //C_PD1
    public float SuctionCoef_2 = 10f;   //C_PD2
    [Range(0,1)] public float SuctionFalloff = 0.5f;    //F_s

    //Force 3 - Slamming Force
    [Header("Force 3 - Slamming Force")]
    //Power used to ramp up slamming force
    public float slammingPower = 2f;
    //public float acc_max;
    public float slammingCheat;

    void Start() 
	{
        current = this;
	}
}
