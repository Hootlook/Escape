using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    public static CameraManager instance;

    private Transform player;

	public float rotationSpeedY = 2;
	public float rotationSpeedX = 2;
	private float horizontal;
	private float vertical;

    public float minDistance = 1.0f;
    public float maxDistance = 4.0f;
    public float smooth = 10.0f;
    private float distance;

    Vector3 normalizedDir;

    public Transform lockOnTarget;
    public bool isLocking;
    private Quaternion rotation;

    void Awake()
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
        player = GameObject.FindGameObjectWithTag("Player").transform;

        normalizedDir = Camera.main.transform.localPosition.normalized;
        distance = Camera.main.transform.localPosition.magnitude;

        Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}


    void Update ()
    {
        if (Input.GetButtonDown("RightAxisClick")) isLocking = !isLocking;

        vertical -= Input.GetAxis("RightAxis Y") * rotationSpeedY;
		horizontal -=  Input.GetAxis("RightAxis X") * rotationSpeedX;
		vertical = Mathf.Clamp(vertical, -80, 80);

        Vector3 desiredCameraPos = transform.TransformPoint(normalizedDir * maxDistance);

        distance = Physics.Linecast(transform.position, desiredCameraPos, out RaycastHit hit) ? Mathf.Clamp((hit.distance * 0.87f), minDistance, minDistance) : maxDistance;

        Camera.main.transform.localPosition = Vector3.Lerp(Camera.main.transform.localPosition, normalizedDir * distance, Time.deltaTime * smooth);
    }

	void LateUpdate()
	{
        Vector3 playerPosition = player.position + (Vector3.up * (player.localScale.y + 0.75f));

        if (isLocking)
        {
            var targetRotation = Quaternion.LookRotation(lockOnTarget.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.1f);
            transform.position = playerPosition + Vector3.forward * -minDistance;
            horizontal = transform.eulerAngles.y;
            vertical = transform.eulerAngles.x;
        }
        else
        {
            rotation = Quaternion.Euler(vertical, horizontal, 0);
            transform.position = playerPosition + rotation * Vector3.forward * -minDistance;
            transform.LookAt(playerPosition);
        }
    }
}
