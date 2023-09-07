using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Controller : MonoBehaviour
{
    public static Controller Instance { get; protected set; }
    public Camera MainCamera;
    public Transform CameraPosition;

    //REMOVE LATER! FOR TESTING ONLY!
    public Animal testingAnimal;

    // ENCAPSULATION: ensure that animal can only be set if its not already, or only be cleared if it is set
    #region ENCAPSULATION
    bool isAnimalSet;
    private Animal m_SelectedAnimal;
    public Animal SelectedAnimal 
    {
        get { return m_SelectedAnimal; }
        set 
        {
            if (value == null)
            {
                m_SelectedAnimal = null;
                isAnimalSet = false;
            }
            else if (!isAnimalSet)
            {
                m_SelectedAnimal = value;
                isAnimalSet = true;
                m_SelectedAnimal.gameObject.transform.SetParent(CameraPosition.parent, false);
                m_SelectedAnimal.gameObject.transform.localPosition = new(0, -0.95f, 0);
                m_SelectedAnimal.gameObject.transform.localRotation = Quaternion.identity;
            }
            else
            {
                Debug.LogError("Animal is already set, please clear before setting another!");
            }
        }
    }
    #endregion

    [Header("Control Settings")]
    public float MouseSensitivity = 10.0f;
    public float PlayerSpeed = 5.0f;
    public float RunningSpeed = 7.0f;
    public float JumpSpeed = 5.0f;
    
    float m_VerticalSpeed = 0.0f;
    bool m_IsPaused = false;
    
    float m_VerticalAngle, m_HorizontalAngle, m_HorizontalCamAngle;
    public float Speed { get; private set; } = 0.0f;

    public bool LockControl { get; set; }

    public bool Grounded => m_Grounded;

    CharacterController m_CharacterController;

    bool m_Grounded;
    float m_GroundedTimer;
    float m_SpeedAtJump = 0.0f;

    void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;

        //REMOVE LATER! FOR TESTING ONLY!
        SelectedAnimal = testingAnimal;

        m_IsPaused = false;
        m_Grounded = true;
        SelectedAnimal.isOnGround = m_Grounded;

        MainCamera.transform.SetParent(CameraPosition, false);
        MainCamera.transform.localPosition = SelectedAnimal.camPos;
        MainCamera.transform.localRotation = Quaternion.identity;
        CameraPosition.localPosition = SelectedAnimal.characterPos;
        CameraPosition.rotation = Quaternion.Euler(SelectedAnimal.transform.rotation.eulerAngles + SelectedAnimal.camDir);
        m_CharacterController = GetComponent<CharacterController>();

        m_VerticalAngle = 0.0f;
        m_HorizontalAngle = transform.localEulerAngles.y;
    }

    //local variables that need to be accessed by multiple functions
    #region multiFunctionLocalVariables
    bool loosedGrounding = false;
    bool running;
    float actualSpeed;
    float turnPlayer;
    float verticalInput;
    Vector3 move = Vector3.zero;
    Vector3 currentAngles;
    #endregion
    void Update()
    {
        HandleGrounding();

        Speed = 0;
        if (!LockControl)
        {
            PlayerControl();
        }

        HandleGravity();
    }

    // ABSTRACTION: handle all the messy code in these few functions, so others only have to worry about calling the funtions, and not how they work
    #region ABSTRACTION
    private void HandleGrounding()
    {
        bool wasGrounded = m_Grounded;

        if (!m_CharacterController.isGrounded)
        {
            if (m_Grounded)
            {
                m_GroundedTimer += Time.deltaTime;
                if (m_GroundedTimer >= 0.5f)
                {
                    loosedGrounding = true;
                    m_Grounded = false;
                    SelectedAnimal.isOnGround = m_Grounded;
                }
            }
        }
        else
        {
            m_GroundedTimer = 0.0f;
            m_Grounded = true;
            SelectedAnimal.isOnGround = m_Grounded;
        }
    }

    private const float _minimumHeldDuration = 0.25f;
    private float _spacePressedTime = 0;
    private bool _spaceHeld = false;
    private void PlayerControl()
    {
        if (m_Grounded && Input.GetButtonDown("Jump"))
        {
            m_VerticalSpeed = SelectedAnimal.jumpHeight;
            m_Grounded = false;
            SelectedAnimal.isOnGround = m_Grounded;
            loosedGrounding = true;
        }

        verticalInput = Input.GetAxisRaw("Vertical");
        running = Input.GetButton("Run");
        actualSpeed = (verticalInput > 0) ? running ? RunningSpeed : PlayerSpeed : PlayerSpeed; //pervent running backwards...

        if (loosedGrounding)
        {
            m_SpeedAtJump = actualSpeed;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            // User has pressed the Space key. We don't know if they'll release or hold it, so keep track of when they started holding it.
            _spacePressedTime = Time.timeSinceLevelLoad;
            _spaceHeld = false;
        }
        else if (Input.GetKeyUp(KeyCode.E)) {
            if (!_spaceHeld)
            {
                SelectedAnimal.SpeakShort();
                // TODO: Perform the action for when Space is pressed.
            }
            _spaceHeld = false;
            SelectedAnimal.SpeakStop();
        }

        if (Input.GetKey(KeyCode.E)) {
            if (Time.timeSinceLevelLoad - _spacePressedTime > _minimumHeldDuration)
            {
                // Player has held the Space key for .25 seconds. Consider it "held"
                _spaceHeld = true;
                SelectedAnimal.SpeakHold();
            }
        }

        CharacterMove();

        CameraMove();
    }

    private void CharacterMove()
    {
        // Move around with WASD
        move = new Vector3(0, 0, verticalInput);
        if (move.sqrMagnitude > 1.0f)
            move.Normalize();

        float usedSpeed = m_Grounded ? actualSpeed : m_SpeedAtJump;

        move = move * usedSpeed * Time.deltaTime;

        move = transform.TransformDirection(move);
        m_CharacterController.Move(move);

        // Turn player
        turnPlayer = Input.GetAxis("Horizontal") * 0.5f;
        m_HorizontalAngle = m_HorizontalAngle + turnPlayer;

        if (m_HorizontalAngle > 360) m_HorizontalAngle -= 360.0f;
        if (m_HorizontalAngle < 0) m_HorizontalAngle += 360.0f;

        currentAngles = transform.localEulerAngles;
        currentAngles.y = m_HorizontalAngle;
        transform.localEulerAngles = currentAngles;
    }

    private void CameraMove()
    {
        // Camera look up/down/left/right
        var turnCam = -Input.GetAxis("Mouse Y");
        var turnCam2 = Input.GetAxis("Mouse X");
        turnCam = turnCam * MouseSensitivity;
        turnCam2 = turnCam2 * MouseSensitivity;
        m_VerticalAngle = Mathf.Clamp(turnCam + m_VerticalAngle, -89.0f, 89.0f);
        m_HorizontalCamAngle = Mathf.Clamp(turnCam2 + m_HorizontalCamAngle, -89.0f, 89.0f);
        currentAngles = CameraPosition.transform.localEulerAngles;
        currentAngles.x = m_VerticalAngle;
        currentAngles.y = m_HorizontalCamAngle;
        CameraPosition.transform.localEulerAngles = currentAngles;

        Speed = move.magnitude / (PlayerSpeed * Time.deltaTime);
        SelectedAnimal.Move(verticalInput * ( (verticalInput > 0) ? running ? 2 : 1 : 1), turnPlayer); //pervent running backwards...
    }

    private void HandleGravity()
    {
        // Fall down / gravity
        m_VerticalSpeed = m_VerticalSpeed - 10.0f * Time.deltaTime;
        if (m_VerticalSpeed < -10.0f)
            m_VerticalSpeed = -10.0f; // max fall speed
        var verticalMove = new Vector3(0, m_VerticalSpeed * Time.deltaTime, 0);
        var flag = m_CharacterController.Move(verticalMove);
        if ((flag & CollisionFlags.Below) != 0)
            m_VerticalSpeed = 0;
    }
    #endregion

    public void DisplayCursor(bool display)
    {
        m_IsPaused = display;
        Cursor.lockState = display ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = display;
    }

}
