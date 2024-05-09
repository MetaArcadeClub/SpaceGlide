using UnityEngine;

// This script acts as a tag to identify objects that are affected by gravity.
public class GravityAffected : MonoBehaviour
{
    [Tooltip("Default mass for this object")]
    public float defaultMass = 1.0f;  // You can set this value in the Unity Editor

    [Tooltip("Mass of this object when under the influence of the time freeze powerup")]
    public float timeFreezeMass = 0.2f;  // You can set this value in the Unity Editor
}

