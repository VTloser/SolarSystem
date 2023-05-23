using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Rigidbody))]
public class Star : MonoBehaviour
{

    public float Mass;
    public float Radius;
    public Vector3 OriginalDir;
    public float ρ;

    [HideInInspector]
    public Vector3 currentVelocity;

    public Rigidbody rigidbody;



    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        currentVelocity = OriginalDir;

        // ρ = m/v
        ρ = Mass / 4 / 3 * Mathf.PI * Mathf.Pow(Radius, 3);
    }

    //F = G*m1*m2 /(r*r)
    public void UpdateVelocity(Star[] stars, float timeStep)
    {
        foreach (var otherStart in stars)
        {
            if (otherStart != this)
            {
                Vector3 dis = otherStart.rigidbody.position - this.rigidbody.position;
                float disSqr = dis.sqrMagnitude;
                Vector3 disDir = dis.normalized;
                Vector3 force = disDir * Universe.gravitationalConstant * Mass * otherStart.Mass / disSqr;
                //F = am
                Vector3 a = force / Mass;
                //v= at
                currentVelocity += a * timeStep;
            }
        }
    }

    public void UpdatePosition(float timeStep)
    {
        // s= vt
        rigidbody.MovePosition(rigidbody.position + currentVelocity * timeStep);
    }


    public void UpdateRochelimit(Star[] stars)
    {
        // d= 1.26f*R*pow（(ρM/ρm)，1/3）
        foreach (var otherStart in stars)
        {
            if (otherStart != this)
            {
                float d = 1.26f * otherStart.Radius * Mathf.Pow(otherStart.ρ / this.ρ, 1 / 3);
                Vector3 dis = otherStart.transform.position - this.transform.position;

                if (dis.sqrMagnitude < Mathf.Pow(d, 2))
                {
                    Debug.Log("BOOM");
                }
            }
        }
    }


}
