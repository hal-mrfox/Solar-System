using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Universe : MonoBehaviour
{
    public const float GravitationalConstant = 0.0001f;
    public static float physicsTimeStep = .02f;

    public bool useFastTimeStep;
    public const float fastTimeStep = 0.1f;
}
