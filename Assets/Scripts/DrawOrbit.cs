using System;
using UnityEngine;

[ExecuteInEditMode]
public class DrawOrbit : MonoBehaviour
{
    public int numSteps = 1000;//预测步长
    public float timeStep = 0.1f; //物理时间间隔
    public bool usePhysicsTimeStep; // 使用物理事件间隔


    public float width = 1;

    void Start()
    {
        if (Application.isPlaying)
        {
            HideOrbits();
        }
    }

    void Update()
    {

        DrawOrbits();
        if (!Application.isPlaying)
        {
           
        }
    }

    [SerializeField]
    Star[] stars;
    /// <summary>
    /// 刻画轨道
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void DrawOrbits()
    {
        stars = FindObjectsOfType<Star>();

        var virtualStar = new virtualStar[stars.Length];
        var drawPoints = new Vector3[stars.Length][];

        //初始化虚拟星
        for (int i = 0; i < virtualStar.Length; i++)
        {
            virtualStar[i] = new virtualStar(stars[i]);
            drawPoints[i] = new Vector3[numSteps];

        }

        //开始模拟
        for (int step_i = 0; step_i < numSteps; step_i++)
        {
            for (int i = 0; i < virtualStar.Length; i++)
            {
                virtualStar[i].velocity += CalculateAcceleration(i, virtualStar) * timeStep;
            }

            //更新位置
            for (int i = 0; i < virtualStar.Length; i++)
            {
                Vector3 newPos = virtualStar[i].position + virtualStar[i].velocity * timeStep;
                virtualStar[i].position = newPos;
                drawPoints[i][step_i] = newPos;

                //virtualStar[i].position += virtualStar[i].velocity * timeStep;
                //drawPoints[i][step_i] = virtualStar[i].position;
            }
        }

        //画线
        for (int i = 0; i < virtualStar.Length; i++)
        {
            LineRenderer lineRenderer = stars[i].GetComponentInChildren<LineRenderer>();
            lineRenderer.enabled = true;
            lineRenderer.positionCount = drawPoints[i].Length;
            lineRenderer.SetPositions(drawPoints[i]);

            lineRenderer.widthMultiplier = width;
        }
    }

    /// <summary>
    /// 计算加速度
    /// </summary>
    /// <param name="i"></param>
    /// <param name="virtualStars"></param>
    /// <returns></returns>
    Vector3 CalculateAcceleration(int i, virtualStar[] virtualStars)
    {
        Vector3 acceleration = Vector3.zero;

        for (int j = 0; j < virtualStars.Length; j++)
        {
            if (i == j)
            {
                continue;
            }
            Vector3 dis = virtualStars[j].position - virtualStars[i].position;
            float disSqr = dis.sqrMagnitude;
            Vector3 disDir = dis.normalized;
            acceleration += disDir * Universe.gravitationalConstant * virtualStars[j].mass / disSqr;
        }
        return acceleration;
    }

    private void HideOrbits()
    {
        stars = FindObjectsOfType<Star>();

        foreach (var item in stars)
        {
            item.GetComponentInChildren<LineRenderer>().positionCount = 0;
        }
    }


    void OnValidate()
    {
        if (usePhysicsTimeStep)
        {
            timeStep = Universe.physicsTimeStep;
        }
    }


}
public class virtualStar
{
    public Vector3 position;
    public Vector3 velocity;
    public float mass;
    public float ρ;
    public virtualStar(Star body)
    {
        position = body.rigidbody.position;
        velocity = body.OriginalDir;
        mass = body.Mass;
        ρ = body.ρ;
    }
}