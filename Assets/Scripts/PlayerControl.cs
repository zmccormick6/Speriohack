using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    public GameObject Level;
    public Vector3 MaxAngle = new Vector3(25, 0, 25);
	public Vector3 levelMove = new Vector3 (0, 0, 0);

    public Text countText;
    public Text winText;

	public float countX = 0f;
	public float countY = 0f;

    private int count;
    private Rigidbody rb;


	// why padding?
    private const float Padding = 10;
    private Vector3 _lastRotation;

    Quaternion origin = Quaternion.identity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        SetCountText();
		origin = Input.gyro.attitude;
        winText.text = "";
    }

    private void Start()
    {
        Input.gyro.enabled = true;

	
        
        _lastRotation = new Vector3(0, 0, 0);
    }

    private void FixedUpdate()
	{
		if (SystemInfo.supportsGyroscope) {
			var rotationRate = new Vector3 (0, 0, 0);

			rotationRate = Input.gyro.rotationRateUnbiased;

			/*if (rotationRate.x > 0.02) 
			{
				//Debug.Log ("1");
				levelMove.x += rotationRate.x;
			}

			if (rotationRate.x < 0.02) 
			{
				//Debug.Log ("2");
				levelMove.x -= rotationRate.x;
				Debug.Log (levelMove.x);
			}

			if (rotationRate.y > 0.02) 
			{
				//Debug.Log ("3");
				levelMove.y += rotationRate.y;
			}

			if (rotationRate.y < 0.02) 
			{
				//Debug.Log ("4");
				levelMove.y -= rotationRate.y;
			}*/


			/*if (levelX < 0) 
			{
				levelX = 360 - levelX;
			}

			if (levelX < 0) 
			{
				levelX = 360 - levelX;
			}*/

			_lastRotation = new Vector3 (-rotationRate.x, 0, -rotationRate.y);

			Level.transform.Rotate (_lastRotation);
			Debug.Log ("Last Rotation: " + _lastRotation);
			Debug.Log ("Rotation Rate: " + rotationRate);
			Debug.Log ("Level: " + Level.transform.eulerAngles);

			/*
			if (Level.transform.eulerAngles.x > MaxAngle.x && Level.transform.eulerAngles.x < MaxAngle.x + Padding)
				Level.transform.eulerAngles = new Vector3 (MaxAngle.x, Level.transform.eulerAngles.y, Level.transform.eulerAngles.z);
			else if (Level.transform.eulerAngles.x < 360 - MaxAngle.x && Level.transform.eulerAngles.x > 360 - MaxAngle.x - Padding)
				Level.transform.eulerAngles = new Vector3 (360 - MaxAngle.x, Level.transform.eulerAngles.y, Level.transform.eulerAngles.z);

			if (Level.transform.eulerAngles.y > MaxAngle.y && Level.transform.eulerAngles.y < MaxAngle.y + Padding)
				Level.transform.eulerAngles = new Vector3 (Level.transform.eulerAngles.x, MaxAngle.y, Level.transform.eulerAngles.z);
			else if (Level.transform.eulerAngles.y < 360 - MaxAngle.y && Level.transform.eulerAngles.y > 360 - MaxAngle.y - Padding)
				Level.transform.eulerAngles = new Vector3 (Level.transform.eulerAngles.x, 360 - MaxAngle.y, Level.transform.eulerAngles.z);

			if (Level.transform.eulerAngles.z > MaxAngle.z && Level.transform.eulerAngles.z < MaxAngle.z + Padding)
				Level.transform.eulerAngles = new Vector3 (Level.transform.eulerAngles.x, Level.transform.eulerAngles.y, MaxAngle.z);
			else if (Level.transform.eulerAngles.z < 360 - MaxAngle.z && Level.transform.eulerAngles.z > 360 - MaxAngle.z - Padding)
				Level.transform.eulerAngles = new Vector3 (Level.transform.eulerAngles.x, Level.transform.eulerAngles.y, 360 - MaxAngle.z);

			*/
		} else {
			Debug.Log ("No Gyro Available");
		}
			
}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 8)
        {
            winText.text = "You Win!";
        }
    }
}