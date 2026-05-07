using UnityEngine;
using System.Collections;

public class AudioContrCar : MonoBehaviour {

	public AudioClip s_motor;
	
	AnimationCurve curvePitch;
	AudioSource source;
	WheelJoint2D wheel;


	void Start()
	{
		wheel = GetComponent<WheelJoint2D>();
		curvePitch = new();

#if !UNITY_WEBGL
		curvePitch.AddKey (0, 0.8f);
#else
		curvePitch.AddKey(0, 1f);
#endif
		curvePitch.AddKey(15000, 3);
		source = GetComponent<AudioSource>();
		source.clip = s_motor;
		source.Play();
	}
	

	void FixedUpdate () {
		float currentSpeed = Mathf.Abs(wheel.motor.motorSpeed);
		if (Time.timeScale >= 0.99f) {
			source.pitch = curvePitch.Evaluate (currentSpeed);
		} else
			source.pitch = 0.15f;
	}
}