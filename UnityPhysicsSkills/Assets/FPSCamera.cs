using UnityEngine;
using System.Collections;
public class FPSCamera : MonoBehaviour
{
    public float flySpeed = 50;
    public float shiftMulti = 2;
    public float rotSpeed = 100;
    public int yLimit = 80;

    /// <summary>
    /// 移动、旋转阻尼
    /// </summary>
    public float Dampening = 5.0f;

    private bool recordRot;
    private float xDeg = 0.0f;
    private float yDeg = 0.0f;
    private Quaternion currentRotation;
    private Quaternion desiredRotation;
    private Quaternion rotation;

    private Vector3 desiredPos;

    void Awake()
    {
        desiredPos = transform.position;
        desiredRotation = transform.rotation;
    }

    void LateUpdate()
    {
        #region 键盘WASD移动相机
        if (Input.GetKey(KeyCode.W))
        {
            if (Input.GetKey(KeyCode.LeftShift))
                desiredPos += transform.forward * flySpeed * shiftMulti * Time.deltaTime;
            else
                desiredPos += transform.forward * flySpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            if (Input.GetKey(KeyCode.LeftShift))
                desiredPos += transform.forward * -flySpeed * shiftMulti * Time.deltaTime;
            else
                desiredPos += transform.forward * -flySpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            if (Input.GetKey(KeyCode.LeftShift))
                desiredPos += transform.right * -flySpeed * shiftMulti * Time.deltaTime;
            else
                desiredPos += transform.right * -flySpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (Input.GetKey(KeyCode.LeftShift))
                desiredPos += transform.right * flySpeed * shiftMulti * Time.deltaTime;
            else
                desiredPos += transform.right * flySpeed * Time.deltaTime;
        }
        #endregion

        if (transform.position != desiredPos)
        {
            transform.position = Vector3.Lerp(transform.position, desiredPos, Time.deltaTime * Dampening);
        }


        #region 鼠标右键旋转相机        
        if (Input.GetMouseButton(1))
        {
            if (!recordRot)
            {
                xDeg = transform.rotation.eulerAngles.y;
                yDeg = transform.rotation.eulerAngles.x;
                recordRot = true;
            }
            xDeg += Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime;
            yDeg -= Input.GetAxis("Mouse Y") * rotSpeed * Time.deltaTime;


            yDeg = ClampYAngle(yDeg, yLimit);
            // set camera rotation 
            desiredRotation = Quaternion.Euler(yDeg, xDeg, 0);
        }
        else
            recordRot = false;
        #endregion

        if (transform.rotation != desiredRotation)
            transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime * Dampening);

    }
    float ClampYAngle(float yAngle, float yLimit)
    {
        if (yAngle > yLimit && yAngle <= 90)
            yAngle = yLimit;
        else if (yAngle > 90 && yAngle < 360 - yLimit)
            yAngle = 360 - yLimit;

        return yAngle;
    }
}