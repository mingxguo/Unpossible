using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using PathCreation;

[ExecuteInEditMode]
public class TubeGenerator : MonoBehaviour
{
    public PathCreator pathCreator;
    public EndOfPathInstruction endOfPathInstruction;
    public Object segment;

    private GameObject tube;
    private float distance = 0;
   

    private void OnEnable()
    {
        string name = "Tube";
        var generated_tranform = transform.Find(name);
        if(generated_tranform == null)
        {
            tube = Create(name, gameObject);
            //Debug.Log("create tube");
            GenerateTube();
        }
    }
    
    private GameObject Create(string name, GameObject parent)
    {
        var res = new GameObject(name);
        res.transform.parent = parent.transform;
        res.transform.localPosition = Vector3.zero;
        res.transform.localScale = Vector3.one;
        res.transform.localRotation = Quaternion.identity;
        //Debug.Log("Tube created");
        return res;
    }

    private void GenerateTube()
    {
        float length = pathCreator.path.length;
        for(float distance = 0; distance < length; distance += 0.1f)
        {
            Vector3 position = pathCreator.path.GetPointAtDistance(distance, endOfPathInstruction);
            Vector3 upward = pathCreator.path.GetDirectionAtDistance(distance, endOfPathInstruction);
            Vector3 forward = pathCreator.path.GetNormalAtDistance(distance, endOfPathInstruction);
            Quaternion rotation = Quaternion.LookRotation(forward, upward);
            Instantiate(segment, position, rotation, tube.transform);
        }
    }
}
