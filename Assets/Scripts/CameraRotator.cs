using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using PathCreation.Examples;

public class CameraRotator : MonoBehaviour
{
    public float RotateSpeed;

    public PathFollower follower;
    public EndOfPathInstruction endOfPathInstruction;
    public PathCreator pathCreator;
    
    private Vector3 offset;
    private Vector3 offset_local;
    private Vector3 follower_position;
    private Quaternion follower_rotation;
    private Matrix4x4 basis;

    // Update is called once per frame

    private void Start()
    {
        follower_position = follower.GetComponent<Transform>().position;
        offset_local = new Vector3(1.5f, 0, 0);
        ResetGlobalPosition();
        ResetGlobalRotation();
        Debug.Log(transform.position - follower_position);
        //Matrix4x4 b = CreateBasis(new Vector3(1, 0, 1), new Vector3(3, -1, 0), new Vector3(2, 4, 2));
        //Debug.Log(BasisChangeWorldToLocal(Vector3.forward, b));
    }

    private void ResetGlobalPosition()
    {
        Vector3 tangent = pathCreator.path.GetDirectionAtDistance(follower.distanceTravelled, endOfPathInstruction).normalized;
        Vector3 normal = pathCreator.path.GetNormalAtDistance(follower.distanceTravelled, endOfPathInstruction);
        Vector3 r = Vector3.Cross(normal, tangent).normalized;

        basis = CreateBasis(normal, tangent, r);
        offset = BasisChangeLocalToWorld(offset_local, basis);

        transform.position = follower_position + offset;
    }

    private void ResetGlobalRotation()
    {
        Vector3 tangent = pathCreator.path.GetDirectionAtDistance(follower.distanceTravelled, endOfPathInstruction).normalized;
        Quaternion rotation = Quaternion.LookRotation(tangent, offset);
        transform.rotation = rotation;
    }

    public Vector3 GetPostion()
    {
        return transform.position;
    }

    public Quaternion GetRotation()
    {
        return transform.rotation;
    }

    public Vector3 GetDirection()
    {
       return 2 * pathCreator.path.GetDirectionAtDistance(follower.distanceTravelled, endOfPathInstruction).normalized;
    }
    
    void Update()
    {
        follower_position = follower.GetComponent<Transform>().position;
        follower_rotation = follower.GetComponent<Transform>().rotation;
        ResetGlobalPosition();
        Vector3 tangent = pathCreator.path.GetDirectionAtDistance(follower.distanceTravelled, endOfPathInstruction).normalized;

        if (Input.GetKey("left"))
        {
            transform.RotateAround(follower_position, tangent, RotateSpeed * Time.deltaTime);
            //transform.rotation = follower.GetComponent<Transform>().rotation;
            //RotateAroundPivot(transform, follower_position, follower_rotation);
        }

        else if (Input.GetKey("right"))
        {
            transform.RotateAround(follower_position, tangent, -RotateSpeed * Time.deltaTime);
        }
        ResetGlobalRotation();
        offset = transform.position - follower_position;
        offset_local = BasisChangeWorldToLocal(offset, basis);
    }


    public static void RotateAroundPivot(Transform trans, Vector3 Pivot, Quaternion Angle)
    {
        Vector3 final = Angle * (trans.position - Pivot) + Pivot;
        trans.position = final;
    }

    Matrix4x4 CreateBasis(Vector3 n, Vector3 t, Vector3 r)
    {
        return new Matrix4x4(n, t, r, new Vector4(0, 0, 0, 1));
    }
    // devolver las coordenadas del vector v en la base {n, t, r} (vectores columna)
    Vector3 BasisChangeWorldToLocal(Vector3 v, Matrix4x4 basis)
    {
        Matrix4x4 inv = basis.inverse;
        return inv.MultiplyPoint3x4(v);
    }
    Vector3 BasisChangeLocalToWorld(Vector3 v, Matrix4x4 basis)
    {
        return basis.MultiplyPoint3x4(v);
    }
}

