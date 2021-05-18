using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Universe : MonoBehaviour
{
    public const float gravitationalConstant = 0.0001f;
    public const float physicsTimeStep = .01f;

    public bool useFastTimeStep;
    public const float fastTimeStep = 0.1f;
}
