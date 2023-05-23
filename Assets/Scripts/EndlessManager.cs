using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessManager : MonoBehaviour
{
    public float distanceThreshold = 1000;
    List<Transform> physicsObjects;
    public GameObject[] games;
    public GameObject Player;
    public event System.Action PostFloatingOriginUpdate;
    Vector3 Origion;


    void Awake()
    {
        physicsObjects = new List<Transform>();
        foreach (var item in games)
        {
            physicsObjects.Add(item.transform);
        }
        Origion = Player.transform.position;
    }

    void LateUpdate()
    {
        UpdateFloatingOrigin();
        PostFloatingOriginUpdate?.Invoke();
    }

    void UpdateFloatingOrigin()
    {
        Vector3 originOffset = Player.transform.position - Origion;
        float dstFromOrigin = originOffset.magnitude;

        if (dstFromOrigin > distanceThreshold)
        {
            foreach (Transform t in physicsObjects)
            {
                t.position -= originOffset;
            }
        }
    }

}