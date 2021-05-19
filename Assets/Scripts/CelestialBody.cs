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
    Vector3 force;

    [Space(20)]
    public float orbitalSpeed;
    public CelestialBody otherBody;

    public new Rigidbody rigidbody;

    [ColorUsage(false, true)] public Color color;

    public void Awake()
    {
        //currentVelocity = initialVelocity;
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = initialVelocity;

        var properties = new MaterialPropertyBlock();
        properties.SetColor("_Color", color);
        GetComponent<Renderer>().SetPropertyBlock(properties);
    }

    public float CalculateOrbitalVelocity(CelestialBody body)
    {
        //float distance = Vector3.Distance(transform.position, body.transform.position);
        //float period = Mathf.Sqrt(distance * distance * distance);
        float standardGravitationalParamter = Universe.GravitationalConstant * mass;
        float distance = Vector3.Distance(transform.position, body.transform.position);
        float semiMajorAxis = Vector3.Distance(transform.position, body.transform.position);

        return Mathf.Sqrt(standardGravitationalParamter * (2f / distance - 1f / semiMajorAxis));
    }

    public void UpdateVelocity(CelestialBody[] allBodies, float timeStep)
    {
        force = Vector3.zero;

        foreach (var otherBody in allBodies)
        {
            if (otherBody != this)
            {
                float sqrDst = (otherBody.rigidbody.position - rigidbody.position).sqrMagnitude;
                Vector3 forceDir = (otherBody.rigidbody.position - rigidbody.position) / Mathf.Sqrt(sqrDst);
                force += forceDir * Universe.GravitationalConstant * mass * otherBody.mass / sqrDst;
                //Vector3 acceleration = force / mass;
                //currentVelocity += acceleration * timeStep;
            }
        }
    }

    public void UpdatePosition(float timeStep)
    {
        //rb.position += currentVelocity * timeStep;
        rigidbody.AddForce(force);
    }

    public void OnValidate()
    {
        transform.localScale = new Vector3(radius, radius, radius);

        if (rigidbody)
        {
            rigidbody.mass = mass;
        }

        var properties = new MaterialPropertyBlock();
        properties.SetColor("_Color", color);
        GetComponent<Renderer>().SetPropertyBlock(properties);

        if (otherBody)
        {
            orbitalSpeed = CalculateOrbitalVelocity(otherBody);
        }
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