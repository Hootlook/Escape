using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float currentSpeed;
    public float stateSpeed;
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

        if (inputDir != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
        }

        if (Input.GetButton("B")) runTimer += Time.deltaTime;

        isRunning = runTimer > 0.5 ? isRunning = true : isRunning = false;

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

        stateSpeed = isRunning ? runSpeed : walking * inputDir.magnitude;

        currentSpeed = Mathf.SmoothDamp(currentSpeed, stateSpeed, ref currentSpeed, speedSmoothTime);

        velocityY += Time.deltaTime * gravity;
        Vector3 velocity = Vector3.up * velocityY;

        cc.Move(velocity * Time.deltaTime);

        currentSpeed = new Vector2(cc.velocity.x, cc.velocity.z).magnitude;

        if (cc.isGrounded) velocityY = 0;
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
