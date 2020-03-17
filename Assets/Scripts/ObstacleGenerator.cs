using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

[ExecuteInEditMode]
public class ObstacleGenerator : MonoBehaviour
{
    public GameObject[] obstacles;
    public EndOfPathInstruction endOfPathInstruction;
    public float NextDistance = 0;
    public float DistanceGap = 20;

    private GameObject obstacles_object;
    public int cont = 1;
    private VertexPath path;
    private float middle_point;
    private int half;

    private void OnEnable()
    {

        PathCreator path_creator = gameObject.GetComponent<PathCreator>();

        if (path_creator != null)
        {
            path = path_creator.path;
        }
        middle_point = path.length / 2f;
        half = 0;
    }

    private void GetParentObject(string name)
    {
        var generated_tranform = transform.Find(name);
        if (generated_tranform == null)
        {
            obstacles_object = new GameObject(name);
            obstacles_object.transform.parent = gameObject.transform;
            obstacles_object.transform.localPosition = Vector3.zero;
            obstacles_object.transform.localScale = Vector3.one;
            obstacles_object.transform.localRotation = Quaternion.identity;
        }
        else obstacles_object = generated_tranform.gameObject;
    }

    public void AddObstacle(int index)
    {
        if (NextDistance > path.length)
        {
            ++cont;
            NextDistance = NextDistance % path.length;
        }
        if (NextDistance > middle_point)
        {
            half = 1;
        }
        else
        {
            half = 0;
        }

        string name = "Obstacles" + cont + half;
        GetParentObject(name);
        Vector3 position = path.GetPointAtDistance(NextDistance, endOfPathInstruction);
        Vector3 upward = path.GetDirectionAtDistance(NextDistance, endOfPathInstruction);
        Vector3 forward = path.GetNormalAtDistance(NextDistance, endOfPathInstruction);
        Quaternion rotation = Quaternion.LookRotation(forward, upward);
        Instantiate(obstacles[index], position, rotation, obstacles_object.transform);
        NextDistance += DistanceGap;
    }
}