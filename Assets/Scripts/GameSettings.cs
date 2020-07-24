using UnityEngine;

public class GameSettings : MonoBehaviour {
    public  string settingsId;

    [Range(0, 100)] public  float minShotSpeed;
    [Range(0, 100)] public  float maxShotSpeed;
    [Range(0, 100)] public  float shotStrength;
    [Range(0, 100)] public  float curveMultiplier;
    [Range(0, 100)] public  float aiSpeed;
    [Range(0, 100)] public  float aiSpeedMag;
    [Range(0, 100)] public  int visionDepth;
}