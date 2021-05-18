using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodySimulation : MonoBehaviour
{
    public CelestialBody[] bodies;

    public void Awake()
    {
        //Time.fixedDeltaTime = Universe.physicsTimeStep;
        Universe.physicsTimeStep = Time.deltaTime;
        bodies = FindObjectsOfType<CelestialBody>();
    }

    public void FixedUpdate()
    {
        for (int i = 0; i < bodies.Length; i++)
        {
            bodies[i].UpdateVelocity(bodies, Universe.physicsTimeStep);
        }

        for (int i = 0; i < bodies.Length; i++)
        {
            bodies[i].UpdatePosition(Universe.physicsTimeStep);
        }
    }
}
