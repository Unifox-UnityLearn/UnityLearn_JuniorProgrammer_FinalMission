using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// INHERITANCE: this is the base animal class that specific animals derive from
public class Animal : MonoBehaviour
{
    //only used for singleton, making sure there's only one animal in the scene
    private Animal Instance;

    public bool isRunning = false;
    public bool isOnGround = false;

    [SerializeField]
    public float jumpHeight { get; protected set; } = 0f;
    [SerializeField]
    protected float runSpeed = 10;
    [SerializeField]
    protected float turnSpeed = 10;

    public Vector3 characterPos { get { return m_characterPos; } protected set { m_characterPos = value; } }
    [SerializeField]
    private Vector3 m_characterPos;
    public Vector3 camPos { get { return m_camPos; } protected set { m_camPos = value; } }
    [SerializeField]
    private Vector3 m_camPos;
    public Vector3 camDir { get { return m_camDir; } protected set { m_camDir = value; } }
    [SerializeField]
    private Vector3 m_camDir;

    [SerializeField]
    private AudioSource animalVoiceShort;
    [SerializeField]
    private AudioSource animalVoiceHold;
    [SerializeField]
    private Animator animator;

    public virtual void Move(float forward, float turn)
    {
        int run = isRunning ? 2 : 1;
        float forwardSpeed = forward * run;
        float turnAmount = turn;
        animator.SetFloat("forwardSpeed", forwardSpeed);
        animator.SetFloat("turnSpeed", turnAmount);
    }

    public virtual void SpeakShort()
    {
        animalVoiceShort.Play();
    }

    public virtual void SpeakHold()
    {
        if (!animalVoiceHold.isPlaying)
        {
            animalVoiceHold.Play();
        }
    }

    public virtual void SpeakStop()
    {
        animalVoiceHold.Stop();
    }

    protected virtual void Start() 
    {
        Debug.Log("Base Start");

        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetBool("onGround", isOnGround);
    }
}
