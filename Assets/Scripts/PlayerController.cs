using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using PathCreation.Examples;

public class PlayerController : MonoBehaviour
{
    private PathCreator level;
    private GameObject follower;
    private float distance;

    private Vector3 offset;
    private Vector3 offset_local;
    private Matrix4x4 current_basis;
    
    public void SetStartPosition(float d, Vector3 start_offset)
    {
        distance = d;
        follower.transform.position = level.path.GetPointAtDistance(distance, EndOfPathInstruction.Loop);
        offset_local = start_offset;
        ResetGlobalPosition();
        ResetGlobalRotation();
    }

    private void Awake()
    {
        follower = transform.parent.gameObject;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        level = GameObject.FindWithTag("Level").GetComponent<PathCreator>();
        //follower_position = follower.GetComponent<Transform>().position;
        //offset_local = new Vector3(1.5f,0f, 0);
        //ResetGlobalPosition();
        //ResetGlobalRotation();

        // Set rigidbody and collision
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().detectCollisions = true;
        GetComponent<Collider>().isTrigger = true;
    }

    // Kinematic rigidbody
    void OnTriggerEnter(Collider col)
    {
        GameController.Instance.DetectedPlayerCollision(col);
    }

    #region PRIVATE_FUNCTIONS
    private void ResetGlobalPosition()
    {
        if (level.path == null) Debug.Log("null");
        Vector3 tangent = level.path.GetDirectionAtDistance(distance, EndOfPathInstruction.Loop).normalized;
        Vector3 normal = level.path.GetNormalAtDistance(distance, EndOfPathInstruction.Loop);
        Vector3 r = Vector3.Cross(normal, tangent).normalized;

        current_basis = Createcurrent_basis(normal, tangent, r);
        offset = current_basisChangeLocalToWorld(offset_local, current_basis);

        transform.position = follower.transform.position + offset;
    }

    private void ResetGlobalRotation()
    {
        Vector3 tangent = level.path.GetDirectionAtDistance(distance, EndOfPathInstruction.Loop).normalized;
        Quaternion rotation = Quaternion.LookRotation(tangent, offset);
        transform.rotation = rotation;

        //get leaning
        //Vector3 tangent = pathCreator.path.GetDirectionAtDistance(follower.DistanceTravelled, EndOfPathInstruction.Loop).normalized;
        //Vector3 r = Vector3.Cross(offset.normalized, tangent).normalized;
        //current_basis = Createcurrent_basis(offset.normalized, tangent, r);
        //Vector3 leaning_up = current_basisChangeLocalToWorld(leaning_offset, current_basis);
        //Vector3 leaning_offset_perp = new Vector3(-leaning_offset.y, leaning_offset.x, 0);
        //Vector3 leaning_forw = current_basisChangeLocalToWorld(leaning_offset_perp, current_basis);
        //Quaternion rotation = Quaternion.LookRotation(leaning_forw, leaning_up);
        //transform.rotation = rotation;
    }
    #endregion

    #region GET
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
       return level.path.GetDirectionAtDistance(distance, EndOfPathInstruction.Loop).normalized;
    }

    public float GetDistance()
    {
        return distance;
    }
    #endregion

    void Update()
    {
        if (!GameController.Instance.PlayerIsDead())
        {

            // Update follower position and rotation
            distance += GameController.Instance.GetPlayerSpeed() * Time.deltaTime;
            follower.transform.position = level.path.GetPointAtDistance(distance, EndOfPathInstruction.Loop);
            follower.transform.rotation = level.path.GetRotationAtDistance(distance, EndOfPathInstruction.Loop);

            //Update speed
            GameController.Instance.UpdatePlayerSpeed();

            // Update player position
            Vector3 follower_position = follower.transform.position;
            ResetGlobalPosition();
            Vector3 tangent = level.path.GetDirectionAtDistance(distance, EndOfPathInstruction.Loop).normalized;

            // Acelerator
            if (!GameController.Instance.IsTapControl())
            {
                transform.RotateAround(follower.transform.position, tangent, -GameController.Instance.GetRotateSpeed() * Input.acceleration.x);
            }
            // Touch screen
            else
            {
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    Vector2 pos = touch.position;
                    // Left side of the screen
                    if (pos.x < GameController.XResolution / 2)
                    {
                        transform.RotateAround(follower_position, tangent, GameController.Instance.GetRotateSpeed() * Time.deltaTime * 2);
                    }
                    // Right side of the screen
                    else
                    {
                        transform.RotateAround(follower_position, tangent, -GameController.Instance.GetRotateSpeed() * Time.deltaTime * 2);
                    }
                }
            }
            // Keyboard
            if (Input.GetKey("left"))
            {
                transform.RotateAround(follower_position, tangent, GameController.Instance.GetRotateSpeed() * 5f * Time.deltaTime);
            }

            else if (Input.GetKey("right"))
            {
                transform.RotateAround(follower_position, tangent, -GameController.Instance.GetRotateSpeed() * 5f * Time.deltaTime);
            }

            // Update player rotation
            offset = transform.position - follower_position;
            offset_local = current_basisChangeWorldToLocal(offset, current_basis);
            ResetGlobalRotation();
        }
    }

    Matrix4x4 Createcurrent_basis(Vector3 n, Vector3 t, Vector3 r)
    {
        return new Matrix4x4(n, t, r, new Vector4(0, 0, 0, 1));
    }
    // devolver las coordenadas del vector v en la base {n, t, r} (vectores columna)
    Vector3 current_basisChangeWorldToLocal(Vector3 v, Matrix4x4 current_basis)
    {
        Matrix4x4 inv = current_basis.inverse;
        return inv.MultiplyPoint3x4(v);
    }
    Vector3 current_basisChangeLocalToWorld(Vector3 v, Matrix4x4 current_basis)
    {
        return current_basis.MultiplyPoint3x4(v);
    }
    
}

