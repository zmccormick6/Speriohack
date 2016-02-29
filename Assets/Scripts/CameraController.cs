using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public float Damp = 2.5f;

    private Vector3 offset;

    private void Awake()
    {
        offset = transform.position - player.transform.position;
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, player.transform.position + offset, Time.deltaTime * Damp);
    }
}
