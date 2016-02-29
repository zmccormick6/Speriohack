//Accelerometer implemented by Nick Hwang

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Move : MonoBehaviour 
{
	private Rigidbody rb;
	public float speed; //speed of the ball
	public Text countText,finishText;
	private int count; //Colectable count
	private int deathCount;
	private Vector3 startingPosition;

	public string mainMenu;

	public GameObject Level;
	[Tooltip("Max Horizonal Rotation")]
	[Range(0, 90)]
	public float horAngleLimit = 45f;
	[Tooltip("Max the Vertical Rotation")]
	[Range(0, 90)]
	public float verAngleLimit = 45f;
	[Tooltip("Low Pass Filter for Accel Data")]
	[Range(0.00f, 1.00f)]
	public float lpfAlpha = .05f;

	float _prevAccelX;
	float _prevAccelY;
	float _curAccelX;
	float _curAccelY;

	Quaternion origin = Quaternion.identity;


	//fixed number of collectables
	public int collectNumber=10;

	//used to reset the collectables after falling into the death pit
	private GameObject[] collect;

	void Start()
	{
		Level.transform.rotation = Quaternion.identity;
		_curAccelX = 0f;
		_curAccelY = 0f; 

		rb = GetComponent<Rigidbody> ();
		count = 0;
		setCountText ();
		finishText.text = "";
		//startingPosition=new Vector3(15, 37, -20);
		collect = GameObject.FindGameObjectsWithTag ("Collectable");

	}

	void Update()
	{
		// assign raw axis data to a private "current" variable
		_curAccelX = Input.acceleration.x;
		_curAccelY = Input.acceleration.y;

		// math for the smoothing. refer to links above. The smaller the "lpfAlpha" value, the smoother the data. 
		_curAccelX = (_curAccelX * lpfAlpha) + (_prevAccelX * (1.0f - lpfAlpha));
		_curAccelY = (_curAccelY * lpfAlpha) + (_prevAccelY * (1.0f - lpfAlpha));

		// aggregating the smoothed data points, into a Quaternion.Euler to be sent to the Level's transform.rotation.
		Level.transform.rotation = Quaternion.Euler (_curAccelY * horAngleLimit, 0f, -_curAccelX * verAngleLimit);

		// the smoothing algorithm needs to know the previous data point to help smooth it out, so we assign the current data value to a private var,
		// to be call in the future cycle.
		_prevAccelX = _curAccelX;
		_prevAccelY = _curAccelY;

		Debug.Log (Level.transform.rotation.eulerAngles + " Input: " + Input.acceleration);

	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag ("Collectable")) 
		{
			other.gameObject.SetActive (false);
			count++;
			setCountText ();
		} 
		else if (other.gameObject.CompareTag ("Finish")) 
		{
			other.gameObject.SetActive (false);
			finishText.text = "FIN!";
			loadMainMenu ();
		} 
		else if (other.gameObject.CompareTag ("Death")) 
		{
			reset ();
		}
	}

	void setCountText()
	{
		countText.text = "Count: " + count.ToString ();
	}

	void reset()
	{
		count = 0;
		deathCount++;
		setCountText ();
		rb.position = startingPosition;

		for (int i = 0; i <= collectNumber; i++) 
		{
			collect [i].SetActive (true);
		}
	}

	public void loadMainMenu()
	{
			SceneManager.LoadScene (mainMenu);
	}
}
