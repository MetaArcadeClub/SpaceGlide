using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomTimeController : MonoBehaviour
{
    public static float globalTimeScale = 1f;
    public float localTimeScale = 1f;

    void Update()
    {
        // Use local time scale
        transform.position += (Vector3.right * localTimeScale * globalTimeScale * Time.deltaTime);
    }

    public static void SetGlobalTimeScale(float scale)
    {
        globalTimeScale = scale;
    }
}
