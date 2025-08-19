using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class WheelController : MonoBehaviour
{
    float GetHorizontalSpeed()
    {
        Vector3 horizontalVelocity = new Vector3(carRb.linearVelocity.x, 0, carRb.linearVelocity.z);
        return horizontalVelocity.magnitude;
    }

    public enum ControlMode
    {
        Keyboard,
        Buttons
    };

    public enum Axel
    {
        Front,
        Rear
    }

    [Serializable]
    public struct Wheel
    {
        public GameObject wheelModel;
        public WheelCollider wheelCollider;
        public GameObject wheelEffectObj;
        public ParticleSystem smokeParticle;
        public Axel axel;
    }

    public ControlMode control;

    public float maxAcceleration = 30.0f;
    public float brakeAcceleration = 50.0f;

    public float turnSensitivity = 1.0f;
    public float maxSteerAngle = 30.0f;

    [FormerlySerializedAs("_centerOfMass")] public Vector3 centerOfMass;

    public List<Wheel> wheels;

    float moveInput;
    float steerInput;

    private Rigidbody carRb;

    [FormerlySerializedAs("_steerAngle")] public float steerAngle;
    public float boosterForce = 1000f;
    public float maxSpeed = 5f; // Maximum speed in Unity units per second

    public AudioSource crashSound;
    public AudioSource driftSound;
    public AudioSource nitroSound;
    
    public GameObject nitroEffect1;
    public GameObject nitroEffect2;
    public GameObject nitroEffect3;
    public GameObject nitroEffect4;
    
    public float nitroDuration = 1f;
    
    public float checkRightSteerAngle = 20f;
    public float checkLeftSteerAngle = -20f;
    
    void Start()
    {
        carRb = GetComponent<Rigidbody>();
        carRb.centerOfMass = centerOfMass;
        nitroEffect1.SetActive(false);
        nitroEffect2.SetActive(false);
        nitroEffect3.SetActive(false);
        nitroEffect4.SetActive(false);
        driftSound.Stop();
        ValidateWheels();
    }

    void ValidateWheels()
    {
        for (int i = 0; i < wheels.Count; i++)
        {
            if (wheels[i].wheelEffectObj == null)
            {
                Debug.LogError($"Wheel at index {i} in WheelController has no wheelEffectObj assigned. Please assign it in the Inspector.");
            }
        }
    }

    void Update()
    {
        GetInputs();
        AnimateWheels();
        WheelEffects();
        
    }

    void LateUpdate()
    {
        Move();
        Steer();
    }

    public void MoveInput(float input)
    {
        moveInput = input;
    }

    public void SteerInput(float input)
    {
        steerInput = input;
    }

    void GetInputs()
    {
        if (control == ControlMode.Keyboard)
        {
            moveInput = Input.GetAxis("Vertical");
            steerInput = Input.GetAxis("Horizontal");
        }
    }

    void Move()
    {
        float currentSpeed = GetHorizontalSpeed();

        
        foreach (var wheel in wheels)
        {
            if (currentSpeed < maxSpeed || boosterForce > 800f) // allow overspeeding when boosting
            {
                wheel.wheelCollider.motorTorque = moveInput * boosterForce * maxAcceleration;
            }
            else
            {
                wheel.wheelCollider.motorTorque = 0f; // stop applying torque when over speed
            }
        }
    }


    void Steer()
    {
        foreach (var wheel in wheels)
        {
            if (wheel.axel == Axel.Front)
            {
                steerAngle = steerInput * turnSensitivity * maxSteerAngle;
                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, steerAngle, 0.6f);


            }
        }
    }
    
    
    void AnimateWheels()
    {
        foreach (var wheel in wheels)
        {
            Quaternion rot;
            Vector3 pos;
            wheel.wheelCollider.GetWorldPose(out pos, out rot);
            wheel.wheelModel.transform.position = pos;
            wheel.wheelModel.transform.rotation = rot;
        }
    }

    void WheelEffects()
    {
        foreach (var wheel in wheels)
        {
            if (wheel.wheelEffectObj == null)
            {
                // Skip this wheel, error already logged in ValidateWheels
                continue;
            }
            //var dirtParticleMainSettings = wheel.smokeParticle.main;
            if ((steerAngle < checkLeftSteerAngle || steerAngle > checkRightSteerAngle) && wheel.wheelCollider.isGrounded)
            {
                var trail = wheel.wheelEffectObj.GetComponentInChildren<TrailRenderer>();
                if (trail != null)
                {
                    trail.emitting = true;
                }
                else
                {
                    Debug.LogWarning($"TrailRenderer not found in wheelEffectObj for wheel: {wheel}");
                }
                nitroEffect1.SetActive(true);
                nitroEffect2.SetActive(true);
                if (driftSound.isPlaying == false)
                    driftSound.Play();
            }
            else
            {
                driftSound.Stop();
                var trail = wheel.wheelEffectObj.GetComponentInChildren<TrailRenderer>();
                if (trail != null)
                {
                    trail.emitting = false;
                }
                nitroEffect1.SetActive(false);
                nitroEffect2.SetActive(false);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boost"))
        {
            nitroEffect1.SetActive(true);
            nitroEffect2.SetActive(true);
            nitroEffect3.SetActive(true);
            nitroEffect4.SetActive(true);
            nitroSound.Play();
            boosterForce = 3000;
            Invoke(nameof(ResetBosterForce), nitroDuration);
        }
    }

    private void ResetBosterForce()
    {
        nitroEffect1.SetActive(false);
        nitroEffect2.SetActive(false);
        nitroEffect3.SetActive(false);
        nitroEffect4.SetActive(false);
        boosterForce = 800;
    }

    private void OnCollisionEnter(Collision collision)
    {
        crashSound.Play();
        
    }
}
