using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class StarManager : MonoBehaviour
{
    Star[] stars;

    void Awake()
    {
        stars = FindObjectsOfType<Star>();
        Time.fixedDeltaTime = Universe.physicsTimeStep;
    }

    void FixedUpdate()
    {
        foreach (var item in stars)
        {
            item.UpdateVelocity(stars, Universe.physicsTimeStep);
        }

        foreach (var item in stars)
        {
            item.UpdatePosition(Universe.physicsTimeStep);
        }

        //foreach (var item in stars)
        //{
        //    item.UpdateRochelimit(stars);
        //}

    }

    //计算加速度



}
