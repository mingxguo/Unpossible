using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using PathCreation.Examples;

public class PlayerController : MonoBehaviour
{
    public PathFollower follower;
    public EndOfPathInstruction endOfPathInstruction;
    public PathCreator pathCreator;
    
    private Vector3 offset;
    private Vector3 offset_local;
    private Vector3 leaning_offset;
    private Vector3 follower_position;
    private Quaternion follower_rotation;
    private Matrix4x4 basis;

    private Gyroscope gyro;
    private Rigidbody rb;
  

    private void Start()
    {
        //GameController.RotateSpeed = SettingsMenu.rotate_speed_slider.value;
        Debug.Log(GameController.RotateSpeed);
        follower_position = follower.GetComponent<Transform>().position;
        offset_local = new Vector3(-1.5f,0, 0);
        leaning_offset = new Vector3(1.5f, -0.3f, 0);
        ResetGlobalPosition();
        ResetGlobalRotation();
        Debug.Log(transform.position - follower_position);
        //Matrix4x4 b = CreateBasis(new Vector3(1, 0, 1), new Vector3(3, -1, 0), new Vector3(2, 4, 2));
        //Debug.Log(BasisChangeWorldToLocal(Vector3.forward, b));


        gyro = Input.gyro;
        gyro.enabled = true;
        Debug.Log("gyro: " + gyro.attitude);

        // Rigidbody

        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.detectCollisions = true;
        GetComponent<Collider>().isTrigger = true;
    }

    // Kinematic rigidbody
    void OnTriggerEnter(Collider col)
    {
        GameController.Instance.DetectedPlayerCollision(col);
    }

    private void ResetGlobalPosition()
    {
        if (pathCreator.path == null) Debug.Log("null");
        Vector3 tangent = pathCreator.path.GetDirectionAtDistance(follower.DistanceTravelled, endOfPathInstruction).normalized;
        Vector3 normal = pathCreator.path.GetNormalAtDistance(follower.DistanceTravelled, endOfPathInstruction);
        Vector3 r = Vector3.Cross(normal, tangent).normalized;

        basis = CreateBasis(normal, tangent, r);
        offset = BasisChangeLocalToWorld(offset_local, basis);

        transform.position = follower_position + offset;
    }

    private void ResetGlobalRotation()
    {
        Vector3 tangent = pathCreator.path.GetDirectionAtDistance(follower.DistanceTravelled, endOfPathInstruction).normalized;
        Quaternion rotation = Quaternion.LookRotation(tangent, offset);
        transform.rotation = rotation;

        //get leaning
        //Vector3 tangent = pathCreator.path.GetDirectionAtDistance(follower.DistanceTravelled, endOfPathInstruction).normalized;
        //Vector3 r = Vector3.Cross(offset.normalized, tangent).normalized;
        //basis = CreateBasis(offset.normalized, tangent, r);
        //Vector3 leaning_up = BasisChangeLocalToWorld(leaning_offset, basis);
        //Vector3 leaning_offset_perp = new Vector3(-leaning_offset.y, leaning_offset.x, 0);
        //Vector3 leaning_forw = BasisChangeLocalToWorld(leaning_offset_perp, basis);
        //Quaternion rotation = Quaternion.LookRotation(leaning_forw, leaning_up);
        //transform.rotation = rotation;
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
       return pathCreator.path.GetDirectionAtDistance(follower.DistanceTravelled, endOfPathInstruction).normalized;
    }
    
    void Update()
    {
        if (!GameController.GameOver)
        {
            follower_position = follower.GetComponent<Transform>().position;
            follower_rotation = follower.GetComponent<Transform>().rotation;
            ResetGlobalPosition();
            Vector3 tangent = pathCreator.path.GetDirectionAtDistance(follower.DistanceTravelled, endOfPathInstruction).normalized;

            // Acelerator
            transform.RotateAround(follower_position, tangent, -GameController.RotateSpeed * Input.acceleration.x / 20f);
            Debug.Log(Input.acceleration.x);

            // Gyroscope
            // gyro acceleration : not working
            // transform.RotateAround(follower_position, tangent, -GameController.RotateSpeed * Input.gyro.userAcceleration.x);

            //transform.RotateAround(follower_position, tangent, -GameController.RotateSpeed * Input.gyro.attitude.x);

            tryAngles();

            if (Input.GetKey("left"))
            {
                transform.RotateAround(follower_position, tangent, GameController.RotateSpeed * Time.deltaTime);
                //transform.rotation = follower.GetComponent<Transform>().rotation;
                //RotateAroundPivot(transform, follower_position, follower_rotation);
            }

            else if (Input.GetKey("right"))
            {
                transform.RotateAround(follower_position, tangent, -GameController.RotateSpeed * Time.deltaTime);
            }
            offset = transform.position - follower_position;
            offset_local = BasisChangeWorldToLocal(offset, basis);
            ResetGlobalRotation();
        }
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

    private float getRoll()
    {

        Quaternion referenceRotation = new Quaternion(0, 0, 1, 0);
        Quaternion deviceRotation = Input.gyro.attitude;
        Quaternion eliminationOfXY = Quaternion.Inverse(
            Quaternion.FromToRotation(referenceRotation * Vector3.forward,
                                      deviceRotation * Vector3.forward)
        );
        Quaternion rotationZ = eliminationOfXY * deviceRotation;
        float roll = rotationZ.eulerAngles.z;
        Debug.Log("gyro: " + gyro.attitude + "\nRoll: " + roll);

        return roll;
    }

    private void tryAngles()
    {
        Quaternion gyro = Input.gyro.attitude;
        //gyro = Quaternion.Euler(gyro.eulerAngles + new Vector3(90, 0, 0));
        Vector3 rot = gyro.eulerAngles;
        rot.x = 0; //is X up while the phone is flat? Z is up in THIS game world.
        rot.y = 0;
        Quaternion aux = Quaternion.Euler(rot);//Quaternion.Lerp (transform.rotation, gyro, Time.time * 5);
        Debug.Log("X: " + Input.gyro.rotationRateUnbiased.x + ", Y: " + Input.gyro.rotationRateUnbiased.y + ", Z: " + Input.gyro.rotationRateUnbiased.z);
       // Debug.Log("X: " + gyro.eulerAngles.x + ", Y: " + gyro.eulerAngles.y + ", Z: " + gyro.eulerAngles.z);


    }
}

