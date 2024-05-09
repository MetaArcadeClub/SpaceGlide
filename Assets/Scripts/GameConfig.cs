using UnityEngine;

[CreateAssetMenu(menuName = "Config/GameConfig")]
public class GameConfig : ScriptableObject
{
    public float initialCometSpeedFactor;
    public float cometSpeedIncreaseValue;
    public float scoreMultiplier;
    public float timeToIncreaseMultiplier;
    public float timeToIncreaseFallSpeed;
}
