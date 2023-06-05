using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GyroMove : MonoBehaviour
{
    //movement
    public float Speed = 10f;
    private float RotSpeed = 90f;
    //private Vector3 moveDirection = Vector3.zero;
    public CharacterController controller;
    public GameObject _Player;
    //Rotation
    private float _initialAngle = 0f;
    private float _appliedGyroAngle = 0f;
    private float _calibrationYAngle = 0f;
    private float _tempSmoothing;
    private Transform _rawGyroRotation;
    //Smoothing
    private float _smoothing = 0.1f;
    //Value of X/Y/Z coordinates
    public Text _xValue;
    public Text _yValue;
    public Text _zValue;
    public Vector3 pos;
    public bool isstrart = false;

    private Rigidbody rb;
    private float DirZ;
    private Gyroscope gyro;
    private Quaternion rotation;
    private bool gyroActive;
    public InputField _inputField;
    public Text _PlayerPosZ;
    public int counter = 0;

    public float timer = 1;

    float savedtimer;

    public void StartGame()
    {
        isstrart = true;

    }
    public void start()
    {
        
        
    }
    private void Awake()
    {
        pos = transform.position;
        savedtimer = timer;
    }

    private void EnablrGyro()
    {
        if (gyroActive)
        {
            return;
        }
        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;

        }
        gyroActive = gyro.enabled;
    }
    public Quaternion GetGyroRotation()
    {

        return rotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isstrart == true)
        {


            _PlayerPosZ.text = transform.position.z.ToString();
            _xValue.text = Input.acceleration.x.ToString();
            _yValue.text = Input.acceleration.y.ToString();
            _zValue.text = Input.acceleration.z.ToString();

            /*if (Input.acceleration.x > 0.30 )
            {
                Vector3 move = new Vector3(*//*Input.acceleration.x * Speed * Time.deltaTime*//* 0f, 0f, Input.acceleration.x * Speed * Time.deltaTime);
                Vector3 rotMovement = transform.TransformDirection(move);
                controller.Move(rotMovement);
                counter++;

            }if (Input.acceleration.x < -0.30)
            {
                Vector3 move = new Vector3(*//*Input.acceleration.x * Speed * Time.deltaTime*//* 0f, 0f, -Input.acceleration.x * Speed * Time.deltaTime);
                Vector3 rotMovement = transform.TransformDirection(move);
                controller.Move(rotMovement);
                counter++;
            }*/


            if (Input.acceleration.x > 0.30)
            {
                Debug.Log("0");

                //if(timer < 0)
                //{
                //timer = savedtimer;
                //}
                if (timer > 0)
                {
                    //timer -= Time.fixedDeltaTime;

                    Vector3 move = new Vector3(/*Input.acceleration.x * Speed * Time.deltaTime*/ 0f, 0f, Input.acceleration.x * Speed * Time.deltaTime);
                    Vector3 rotMovement = transform.TransformDirection(move);
                    controller.Move(rotMovement);
                    counter++;
                    //if (timer == 0)
                    //{
                    //    timer = 5;
                    //    counter = 0;
                    //}
                }


                //timer = Time.time - savedtimer;

            }
            else if (Input.acceleration.x < -0.30)
            {
                Debug.Log("1");

                //if (timer < 0)
                //{
                //timer = savedtimer;
                //}
                if (timer > 0)
                {
                    //timer -= Time.fixedDeltaTime;

                    Vector3 move = new Vector3(/*Input.acceleration.x * Speed * Time.deltaTime*/ 0f, 0f, -Input.acceleration.x * Speed * Time.deltaTime);
                    Vector3 rotMovement = transform.TransformDirection(move);
                    controller.Move(rotMovement);
                    counter++;

                }
                //if (timer == 0)
                //{
                //    timer = 5;
                //    //counter = 0;
                //}

                //timer = Time.time - savedtimer;
            }
            else if (Input.acceleration.x < 0.30 && Input.acceleration.x > 0)
            {
                if (timer > 0)
                {
                    Debug.Log("2");

                    timer -= Time.fixedDeltaTime;
                    Vector3 move = new Vector3(/*Input.acceleration.x * Speed * Time.deltaTime*/ 0f, 0f, Input.acceleration.x * Speed * Time.deltaTime);
                    Vector3 rotMovement = transform.TransformDirection(move);
                    controller.Move(rotMovement);
                }
                else
                {
                    counter = 0;

                }


            }

            else if (Input.acceleration.x > -0.30 && Input.acceleration.x < 0)
            {
                if (timer > 0)
                {
                    Debug.Log("3");
                    timer -= Time.fixedDeltaTime;
                    Vector3 move = new Vector3(/*Input.acceleration.x * Speed * Time.deltaTime*/ 0f, 0f, -Input.acceleration.x * Speed * Time.deltaTime);
                    Vector3 rotMovement = transform.TransformDirection(move);
                    controller.Move(rotMovement);
                }
                else
                {
                    counter = 0;

                }

            }



            //Debug.Log("input.acceleration.x: " + Input.acceleration.x);
            //Debug.Log("input.acceleration.y: " + Input.acceleration.y);
            //Debug.Log("input.acceleration.z: " + Input.acceleration.z);

            ApplyGyroRotation();
            ApplyCalibration();

            //transform.rotation = Quaternion.Slerp(transform.rotation, _rawGyroRotation.rotation,_smoothing);

            //DirZ = Input.acceleration.z * speed;
            //rb.AddForce(transform.forward * speed, ForceMode.Acceleration);
        }
    }
    private IEnumerator Start()
    {
        Input.gyro.enabled = true;
        Application.targetFrameRate = 60;
        _initialAngle = transform.eulerAngles.y;
        _rawGyroRotation = new GameObject("gyroRaw").transform;
        //_rawGyroRotation.position = transform.position;
        //_rawGyroRotation.rotation = transform.rotation;

        yield return new WaitForSeconds(1);

        StartCoroutine(CalibrateYAngle());

    }
    private IEnumerator CalibrateYAngle()
    {
        _tempSmoothing = _smoothing;
        _smoothing = 1;
        _calibrationYAngle = _appliedGyroAngle = _initialAngle;
        yield return null;
        _smoothing = _tempSmoothing;
    }
    private void ApplyGyroRotation()
    {
        _rawGyroRotation.rotation = Input.gyro.attitude;
        _rawGyroRotation.Rotate( 0f, 0f, 180f, Space.Self);
        _rawGyroRotation.Rotate( 90f , 180f, 0f , Space.World);
        _appliedGyroAngle = _rawGyroRotation.eulerAngles.y;


    }
    private void ApplyCalibration()
    {
        _rawGyroRotation.Rotate(0f ,-_calibrationYAngle , 0f , Space.World);

    }
    private void SetEnabled()
    {
        enabled = true;
        StartCoroutine(CalibrateYAngle());
    }

    public void ScoreNumber()
    {
       Speed  = System.Convert.ToInt32( _inputField.text);

    }

    public void Reset()
    {
        _Player.transform.position = pos;
        timer = 5;
        counter = 0;
        isstrart = false;
        //StartCoroutine(delay());
        //_Player.transform.position = new Vector3(0F, 0.755F, 0F);
        //timer = 5;
    }


    //public IEnumerator delay()
    //{

    //    _Player.transform.position = new Vector3(0F, 0.755F, 0F);
    //    timer = 5;
    //    yield return new WaitForSeconds(1);
    //    Debug.Log("here in reset");
    //}
    //void FixedUpdate()
    //{
    //    //rb.velocity = new Vector3(DirZ, 0f, -DirZ);
    //}
}
