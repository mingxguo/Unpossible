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

    private GameObject obstacles_object;

    private void OnEnable()
    {
        string name = "Obstacles";
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
        PathCreator path_creator = gameObject.GetComponent<PathCreator>();
        if (path_creator != null)
        {
            VertexPath path = path_creator.path;
            Vector3 position = path.GetPointAtDistance(NextDistance % path.length, endOfPathInstruction);
            Vector3 upward = path.GetDirectionAtDistance(NextDistance % path.length, endOfPathInstruction);
            Vector3 forward = path.GetNormalAtDistance(NextDistance % path.length, endOfPathInstruction);
            Quaternion rotation = Quaternion.LookRotation(forward, upward);
            Instantiate(obstacles[index], position, rotation, obstacles_object.transform);
        }
        NextDistance += 10;
    }
}
