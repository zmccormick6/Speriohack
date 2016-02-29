//Accelerometer implemented by Nick Hwang

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MoveOne : MonoBehaviour 
{
	private Rigidbody rb;
	public float speed; //speed of the ball
	public Text countText, timeText;
	private int count; //Colectable count
	private int deathCount;
	private Vector3 startingPosition;

    public string timeUpOne;
    public string mainMenu;
	public string deathScreenOne;
	public string winScreenOne;
    public float timeLeft = 90.0f;

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
	public int collectNumber = 10;

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
        startingPosition = transform.position;
		collect = GameObject.FindGameObjectsWithTag ("Collectable");

	}

	void FixedUpdate()
	{
		// assign raw axis data to a private "current" variable
		_curAccelX = Input.acceleration.x;
		_curAccelY = -1 * Input.acceleration.y;

		// math for the smoothing. refer to links above. The smaller the "lpfAlpha" value, the smoother the data. 
		_curAccelX = (_curAccelX * lpfAlpha) + (_prevAccelX * (1.0f - lpfAlpha));
		_curAccelY = (_curAccelY * lpfAlpha) + (_prevAccelY * (1.0f - lpfAlpha));

		// aggregating the smoothed data points, into a Quaternion.Euler to be sent to the Level's transform.rotation.
		Vector3 movement = new Vector3(_curAccelX * horAngleLimit, 0f, -_curAccelY * verAngleLimit);

		//Level.transform.rotation = Quaternion.Euler (_curAccelY * horAngleLimit, 0f, -_curAccelX * verAngleLimit);

		// the smoothing algorithm needs to know the previous data point to help smooth it out, so we assign the current data value to a private var,
		// to be call in the future cycle.
		_prevAccelX = _curAccelX;
		_prevAccelY = _curAccelY;

		rb.AddForce (movement * speed);
        Debug.Log (Level.transform.rotation.eulerAngles + " Input: " + Input.acceleration);

        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            SceneManager.LoadScene(timeUpOne);
        }

		timeText.text = timeLeft.ToString("Time: 000");
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
            PlayerPrefs.SetInt("level_two_enabled", 1);
			SceneManager.LoadScene (winScreenOne);
		} 
		else if (other.gameObject.CompareTag ("Death")) 
		{
			SceneManager.LoadScene (deathScreenOne);
		}
	}

	void setCountText()
	{
        countText.text = "Score: " + count;
	}

	void reset()
	{
		count = 0;
		deathCount++;
		setCountText ();
		transform.position = startingPosition;

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
