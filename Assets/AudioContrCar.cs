using UnityEngine;
using System.Collections;

public class AudioContrCar : MonoBehaviour {

	public AudioClip s_motor;
	public AnimationCurve curvePitch;
	float maxMotor;
	AudioSource source;
	WheelJoint2D wheel;
	// Use this for initialization
	void Start () {
		wheel = GetComponent<WheelJoint2D> ();
		maxMotor = wheel.motor.maxMotorTorque;
		curvePitch.AddKey (0, 0.8f);
		curvePitch.AddKey (15000, 3);
		source = GetComponent<AudioSource> ();
		source.clip = s_motor;
		source.Play ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float currentSpeed = Mathf.Abs(wheel.motor.motorSpeed);
		if (Time.timeScale >= 0.99f) {
			source.pitch = curvePitch.Evaluate (currentSpeed);
		} else
			source.pitch = 0.15f;
	}
}