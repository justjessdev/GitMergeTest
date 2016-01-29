using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{

    public Transform playerCamera;

    //translate values
    public float walkSpeed = 2.0f;
    public float runSpeed = 6.0f;

    //rotation values
    public bool inverted = false;
    public float lookSensitivity = 5.0f;
    private float maxUpwardAngle;
    private float maxDownwardAngle;
    private float xRotation;
    private float yRotation;
    private float dampSpeed;
    private float dampDiagonal;

    //headbob values
    private float timer = 0.0f;
    private float bobRate = 0.15f;
    private float bobLength = 0.06f;
    private float headCenter = 0.0f;

    //temporary is main player bool
    public bool isMainPlayer = true;

    //private CharacterController controller;
    //public static Vector3 objectCollision = new Vector3(0.0f, 0.0f, 0.0f);

    public override void OnStartLocalPlayer()
    {
        
        //maxUpwardAngle = 90;
        //maxDownwardAngle = 90;
        //dampDiagonal = 0.7071f;
        //headCenter = playerCamera.localPosition.y;
    }

    void Awake()
    {
        maxUpwardAngle = 90;
        maxDownwardAngle = 90;
        dampDiagonal = 0.7071f;
        headCenter = playerCamera.localPosition.y;
    }

    public Vector3 HeadBob()
    {
        float wave = 0.0f;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 headPos = playerCamera.localPosition;

        if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
        {
            timer = 0.0f;
        }
        else
        {
            wave = Mathf.Sin(timer);
            timer = timer + bobRate;
            if (timer > Mathf.PI * 2)
            {
                timer = timer - (Mathf.PI * 2);
            }
        }
        if (wave != 0)
        {
            float displacement = wave * bobLength;
            float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
            displacement = totalAxes * displacement;
            headPos.y = displacement + headCenter;
        }
        else
        {
            headPos.y = headCenter;
        }

        return headPos;
    }

    public void TranslateCharacter(Vector3 move)
    {

        transform.Translate(move.x, move.y, move.z);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, Mathf.Infinity))// 1 << 9))
        {
            float distanceToGround = hit.distance;
            float diff = Mathf.Clamp(distanceToGround, 0.0f, 2.0f);

            if (distanceToGround < 0.0f)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + (0.0f - diff), transform.position.z);
            }
            else if (distanceToGround > 0.0f)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - (diff - 0.0f), transform.position.z);
            }
        }
    }

    public bool HandleInput()
    {

        float moveSpeed;

        float vert = Input.GetAxis("Vertical");
        float horz = Input.GetAxis("Horizontal");
        //float run = Input.GetAxis("Run");
        bool run = Input.GetButton("Run");

        dampSpeed = Time.deltaTime;

        if (vert == 0 && horz == 0)
        {
            return false;
        }

        if (run)
        {
            moveSpeed = runSpeed;
        }
        else
        {
            moveSpeed = walkSpeed;
        }

        //forward-left
        if (vert > 0 && horz < 0)
        {
            TranslateCharacter(new Vector3(-moveSpeed * dampSpeed * dampDiagonal, 0, moveSpeed * dampSpeed * dampDiagonal));
            return true;
            //forward-right
        }
        else if (vert > 0 && horz > 0)
        {
            TranslateCharacter(new Vector3(moveSpeed * dampSpeed * dampDiagonal, 0, moveSpeed * dampSpeed * dampDiagonal));
            return true;
            //backwards-left
        }
        else if (vert < 0 && horz < 0)
        {
            TranslateCharacter(new Vector3(-moveSpeed * dampSpeed * dampDiagonal, 0, -moveSpeed * dampSpeed * dampDiagonal));
            return true;
            //backwards-right
        }
        else if (vert < 0 && vert > 0)
        {
            TranslateCharacter(new Vector3(moveSpeed * dampSpeed * dampDiagonal, 0, -moveSpeed * dampSpeed * dampDiagonal));
            return true;
        }

        //single directions
        if (vert > 0)
        {
            TranslateCharacter(new Vector3(0, 0, moveSpeed * dampSpeed));
        }
        if (vert < 0)
        {
            TranslateCharacter(new Vector3(0, 0, -moveSpeed * dampSpeed));
        }
        if (horz < 0)
        {
            TranslateCharacter(new Vector3(-moveSpeed * dampSpeed, 0, 0));
        }
        if (horz > 0)
        {
            TranslateCharacter(new Vector3(moveSpeed * dampSpeed, 0, 0));
        }


        return true;
    }

    public void RotateCharacter()
    {
        //check if inverted, add up mouse values
        if (inverted == false)
        {
            yRotation += Input.GetAxis("Mouse Y") * lookSensitivity * -1f;
            xRotation += Input.GetAxis("Mouse X") * lookSensitivity;
        }
        else if (inverted == true)
        {
            yRotation += Input.GetAxis("Mouse Y") * lookSensitivity;
            xRotation += Input.GetAxis("Mouse X") * lookSensitivity;
        }
        //clamp mouse values
        yRotation = Mathf.Clamp(yRotation, -maxUpwardAngle, maxDownwardAngle);

        //apply headbob
        //playerCamera.localPosition = HeadBob();

        //rotate camera (vertical)
        Quaternion vert = Quaternion.Euler(yRotation, 0, 0);
        playerCamera.localRotation = Quaternion.Lerp(playerCamera.localRotation, vert, 50.0f * Time.deltaTime);

        //rotate character (horizontal)
        Quaternion horz = Quaternion.AngleAxis(xRotation, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, horz, 50.0f * Time.deltaTime);

    }

    // Update is called once per frame
    public void Update()
    {
        if (!isMainPlayer) return;
        //if (!isLocalPlayer) return;

        HandleInput();

        RotateCharacter();

        //GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);

    }

    void FixedUpdate()
    {
        //if (!isMainPlayer) return;
        if (!isLocalPlayer) return;
        GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
    }
}
