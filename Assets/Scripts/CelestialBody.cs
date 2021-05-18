using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialBody : MonoBehaviour
{
    public float mass;
    public float radius;
    //[HideInInspector]
    public Vector3 initialVelocity;
    Vector3 currentVelocity;

    Rigidbody rb;

    public void Awake()
    {
        currentVelocity = initialVelocity;
        rb = GetComponent<Rigidbody>();
    }

    public void UpdateVelocity(CelestialBody[] allBodies, float timeStep)
    {
        foreach (var otherBody in allBodies)
        {
            if (otherBody != this)
            {
                float sqrDst = (otherBody.rb.position - rb.position).sqrMagnitude;
                Vector3 forceDir = (otherBody.rb.position - rb.position).normalized;
                Vector3 force = forceDir * Universe.gravitationalConstant * mass * otherBody.mass / sqrDst;
                Vector3 acceleration = force / mass;
                currentVelocity += acceleration * timeStep;
            }
        }
    }

    public void UpdatePosition(float timeStep)
    {
        rb.position += currentVelocity * timeStep;
    }

    public void OnValidate()
    {
        transform.localScale = new Vector3(radius, radius, radius);
    }
}
