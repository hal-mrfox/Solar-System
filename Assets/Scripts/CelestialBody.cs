using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CelestialBody : MonoBehaviour
{
    public float mass;
    public float radius;
    //[HideInInspector]
    public Vector3 initialVelocity;
    Vector3 currentVelocity;

    Rigidbody rb;

    [ColorUsage(false, true)] public Color color;

    public void Awake()
    {
        currentVelocity = initialVelocity;
        rb = GetComponent<Rigidbody>();

        var properties = new MaterialPropertyBlock();
        properties.SetColor("_Color", color);
        GetComponent<Renderer>().SetPropertyBlock(properties);
    }

    public void UpdateVelocity(CelestialBody[] allBodies, float timeStep)
    {
        foreach (var otherBody in allBodies)
        {
            if (otherBody != this)
            {
                float sqrDst = (otherBody.rb.position - rb.position).sqrMagnitude;
                Vector3 forceDir = (otherBody.rb.position - rb.position) / Mathf.Sqrt(sqrDst);
                Vector3 force = forceDir * Universe.GravitationalConstant * mass * otherBody.mass / sqrDst;
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

        var properties = new MaterialPropertyBlock();
        properties.SetColor("_Color", color);
        GetComponent<Renderer>().SetPropertyBlock(properties);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(CelestialBody))]
public class CelestialBodyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Set Preview Body"))
        {
            FindObjectOfType<OrbitDisplay>().centralBody = (CelestialBody)target;
        }
    }
}
#endif