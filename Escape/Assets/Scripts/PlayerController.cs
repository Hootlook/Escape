using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public Vector2 stateSpeed;
    public float walkSpeed = 1.5f;
    public float jogSpeed = 5;
    public float runSpeed = 8;
    public float gravity = -30;

    public float turnSmoothTime = 0.05f;
    private float turnSmoothVelocity;
    public float speedSmoothTime = 0.1f;

    private float velocityY;
    private float runTimer;

    CharacterController cc;

    public bool isRunning;
    public bool isRolling;
    public bool isJumpingBack;

    private Vector2 input;
    private Vector2 inputDir;

    void Start() => cc = GetComponent<CharacterController>();

    void Update()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        inputDir = input.normalized;

        if(CameraManager.instance.isLocking && !isRunning && !isRolling)
        {
            LockRotation(CameraManager.instance.lockOnTarget);
        }
        else
        {
            FreeRotation(input);
        }
        
        if (Input.GetButton("B")) runTimer += Time.deltaTime;

        isRunning = runTimer > 0.5 && inputDir.magnitude > 0 ? isRunning = true : isRunning = false;

        if (isRolling || isJumpingBack) return;

        if (Input.GetButtonUp("B"))
        {
            if (inputDir.magnitude != 0 && runTimer < 0.3) StartCoroutine(Roll());

            if (inputDir.x == 0 && inputDir.y == 0 && runTimer < 0.3) StartCoroutine(StepBack());

            runTimer = 0;
        }

        Move();
    }

    void Move()
    {
        float walking = input.magnitude < 1 ? walkSpeed : jogSpeed;

        float speed = isRunning ? runSpeed : walking * inputDir.magnitude;

        stateSpeed = new Vector2(speed * input.normalized.x, speed * input.normalized.y);

        velocityY += Time.deltaTime * gravity;

        Vector3 velocity = Vector3.up * velocityY + cc.velocity;

        if (!cc.isGrounded) cc.Move(velocity * Time.deltaTime);
        else velocityY = 0;
    }

    void LockRotation(Transform target)
    {
        Vector3 direction = target.position - transform.position;
        Vector3 flattenedDirection = Vector3.ProjectOnPlane(direction, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(flattenedDirection), 10 * Time.deltaTime);
    }

    void FreeRotation(Vector2 input)
    {
        if (input.normalized != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
        }
    }

    IEnumerator StepBack()
    {
        isJumpingBack = true;
        yield return new WaitForSeconds(0.8f);
        isJumpingBack = false;
    }
    IEnumerator Roll()
    {
        isRolling = true;
        yield return new WaitForSeconds(0.8f);
        isRolling = false;
    }
}
