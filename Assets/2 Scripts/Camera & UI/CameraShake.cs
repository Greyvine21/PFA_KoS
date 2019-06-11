using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    static private CameraShake s_instance = null;

    static public CameraShake GetInstanceShake()
    {
        return s_instance;
    }

    void Awake()
    {
#if UNITY_EDITOR
        if (s_instance != null)
            Debug.LogError("There's more than one CameraShake instance in the scene");
#endif

        s_instance = this;
    }
    
    static public IEnumerator Shake(float duration, float magnitude)
    {
        print("Shake");
        Vector3 originalPos = GetInstanceShake().transform.localPosition;

        float elapsed = 0.0f;

        while(elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            GetInstanceShake().transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        GetInstanceShake().transform.localPosition = originalPos;
    }
}
