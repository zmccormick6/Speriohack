// MAGD 490 -- iphone accel data with low pass filter -- NH 2016

using UnityEngine;
using System.Collections;

public class accelControl : MonoBehaviour {

	public GameObject Level;
	public GameObject Ball;
	public GameObject Camera;

	[Range(0, 90)]
	public float horAngleLimit;

	[Range(0, 90)]
	public float verAngleLimit;

	[Range(0.00f, 1.00f)]
	public float lpfAlpha;

	[Range(5.0f, 500.0f)]
	public float maxDistanceFromCenterX;

	[Range(5.0f, 500.0f)]
	public float maxDistanceFromCenterY;

	float distanceFromCenter;

	[Range(0, 90)]
	public float horDistanceAngleLimit;

	[Range(0, 90)]
	public float verDistanceAngleLimit;



	float _prevAccelX;
	float _prevAccelY;
	float _curAccelX;
	float _curAccelY; 

	// Use this for initialization
	void Start () {
		// try to zero out the level rotation before receiving phone accel data
		Level.transform.rotation = Quaternion.identity;
		_curAccelX = 0f;
		_curAccelY = 0f; 
	}
		
	void Update(){
		#region Accelerometer Data
		// assign raw axis data to a private "current" variable
		_curAccelX = Input.acceleration.x;
		_curAccelY = Input.acceleration.y;

		#endregion

		//Debug.Log ((Pivot.transform.position.x - Ball.transform.position.x) +", "+  (Pivot.transform.position.y - Ball.transform.position.y )+ ", " + (Pivot.transform.position.z - Ball.transform.position.z));
		//Debug.Log (Level.transform.position.x + ", "+  Level.transform.position.y + ", " + Level.transform.position.z);


		#region Low Pass Filter
		// Apply a low pass filter: https://en.wikipedia.org/wiki/Low-pass_filter
		//http://www.kircherelectronics.com/blog/index.php/11-android/sensors/8-low-pass-filter-the-basics
		// math for the smoothing. refer to links above. The smaller the "lpfAlpha" value, the smoother the data. 
		_curAccelX = (_curAccelX * lpfAlpha) + (_prevAccelX * (1.0f - lpfAlpha));
		_curAccelY = (_curAccelY * lpfAlpha) + (_prevAccelY * (1.0f - lpfAlpha));


		// aggregating the smoothed data points, into a Quaternion.Euler to be sent to the Level's transform.rotation.

		float scaledHorAngle = ((1f-Mathf.Clamp((Mathf.Abs(Ball.transform.position.x)/maxDistanceFromCenterX),0,1)) * (horAngleLimit-horDistanceAngleLimit))+horDistanceAngleLimit;
		float scaledVerAngle = ((1f-Mathf.Clamp((Mathf.Abs(Ball.transform.position.z)/maxDistanceFromCenterY),0,1)) * (verAngleLimit-verDistanceAngleLimit))+verDistanceAngleLimit;
		print((1f-Mathf.Clamp((Mathf.Abs(Ball.transform.position.x)/maxDistanceFromCenterX),0,1))+", "+ (1f-Mathf.Clamp((Mathf.Abs(Ball.transform.position.z)/maxDistanceFromCenterY),0,1))+ ", " + scaledHorAngle+ ", " + scaledVerAngle);

//		print((1f-Mathf.Clamp((Mathf.Abs(Ball.transform.position.x)/maxDistanceFromCenterX),0,1))+", "+ (1f-Mathf.Clamp((Mathf.Abs(Ball.transform.position.y)/maxDistanceFromCenterY),0,1))+ ", " + scaledHorAngle+ ", " + scaledVerAngle);



		Level.transform.rotation = Quaternion.Euler (_curAccelY * scaledVerAngle, 0f, -_curAccelX * scaledHorAngle);
		//print (Level.transform.rotation.eulerAngles);

		//ORIGINAL
		//Level.transform.rotation = Quaternion.Euler (_curAccelY * VerAngleLimit, 0f, -_curAccelX * HorAngleLimit);


		//Level.transform.rotation = Quaternion.Euler (_curAccelY * horAngleLimit, 0f, -_curAccelX * verAngleLimit);
		//Pivot.transform.rotation = Quaternion.Euler (_curAccelY * horAngleLimit, 0f, -_curAccelX * verAngleLimit);

		// the smoothing algorithm needs to know the previous data point to help smooth it out, so we assign the current data value to a private var,
		// to be call in the future cycle.
		_prevAccelX = _curAccelX;
		_prevAccelY = _curAccelY;
		#endregion


		// This works by itself -- this is just using the RAW DATA. Uncomment this line to see how jumpy the data is. 
		//Level.transform.rotation = Quaternion.Euler(Input.acceleration.y * horAngleLimit ,0f, -Input.acceleration.x * verAngleLimit );

		//Debug.Log (Level.transform.rotation.eulerAngles + " Input: " + Input.acceleration);

		Camera.transform.position = new Vector3(Ball.transform.position.x, 30f, Ball.transform.position.z);
	}
}
