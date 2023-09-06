using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    //only used for singleton, making sure there's only one animal in the scene
    private Animal Instance;

    public bool isRunning = false;
    public bool isOnGround = false;

    [SerializeField]
    protected float jumpHeight = 0f;
    [SerializeField]
    protected float runSpeed = 10;
    [SerializeField]
    protected float turnSpeed = 10;

    [SerializeField]
    public Vector3 characterPos { get; protected set; } = new();
    [SerializeField]
    public Vector3 camPos { get; protected set; } = new();
    [SerializeField]
    public Vector3 camDir { get; protected set; } = new();

    private AudioSource animalVoice;
    private Animator animator;

    public virtual void Move(float forward, float turn)
    {
        int run = isRunning ? 2 : 1;
        float forwardSpeed = forward * run;
        float turnAmount = turn;
        animator.SetFloat("forwardSpeed", forwardSpeed);
        animator.SetFloat("turnSpeed", turnAmount);
    }

    public virtual void Speak()
    {
        animalVoice.Play();
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
        animalVoice = GetComponent<AudioSource>(); 
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetBool("onGround", isOnGround);
    }
}
