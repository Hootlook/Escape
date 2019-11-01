using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

	public float rotationSpeedY = 2;
	public float rotationSpeedX = 2;
	private float horizontal;
	private float vertical;
	public Transform player;

    public float minDistance = 1.0f;
    public float maxDistance = 4.0f;
    public float smooth = 10.0f;
    private float distance;
    Vector3 dollyDir;

    public static CameraManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning("Only one camera can be instantiated !");
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Start ()
    {
        dollyDir = Camera.main.transform.localPosition.normalized;
        distance = Camera.main.transform.localPosition.magnitude;

        Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}


    void Update ()
    {
		vertical -= Input.GetAxis("RightAxis Y") * rotationSpeedY;
		horizontal -=  Input.GetAxis("RightAxis X") * rotationSpeedX;
		vertical = Mathf.Clamp(vertical, -80, 80);

        Vector3 desiredCameraPos = transform.TransformPoint(dollyDir * maxDistance);

        distance = Physics.Linecast(transform.position, desiredCameraPos, out RaycastHit hit) ? Mathf.Clamp((hit.distance * 0.87f), minDistance, minDistance) : maxDistance;

        Camera.main.transform.localPosition = Vector3.Lerp(Camera.main.transform.localPosition, dollyDir * distance, Time.deltaTime * smooth);
    }

	private void LateUpdate()
	{
        Vector3 target = player.position + (Vector3.up * (player.localScale.y + 0.75f));
        Quaternion rotation = Quaternion.Euler(vertical, horizontal, 0);
		transform.position = target + rotation * Vector3.forward * -minDistance;
		transform.LookAt(target);
	}
}
