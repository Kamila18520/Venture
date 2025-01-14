using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovementController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;         
    public float rotationSpeed = 10f;      
    public CinemachineFreeLook cmFreeLook;


    [Header("Jump")]
    public float jumpForce = 7f;          
    public float liftForce = 2f;           
    public float maxJumpHoldTime = 2.5f;  
    public bool isGrounded;                
    public LayerMask groundLayer;          
    [SerializeField] private PlayerValues jumpEnergy;

    // Komponenty
    private Rigidbody rb;                  
    public Animator Anim;                  
    public ParticleSystem fire;            
    private bool isStanding = true;        
    private bool isWalking = false;        
    private bool isJumping = false;        

    [Header("Sounds")]
    [SerializeField] private AudioSource jumpBackpack;
    [SerializeField] private AudioSource footsteps;



    private void Start()
    {
        fire.Stop();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(isWalking)
        {
            footsteps.enabled = true;
        }
        else
        {
            footsteps.enabled=false;
        }
        RaycastGroundCheck();
        Move();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
            fire.Play();
        }

        if(jumpEnergy.currentValue <=0f)
        {
            fire.Stop();
        }

        if (Input.GetKey(KeyCode.Space))
        {
            jumpEnergy.RemoveValue(jumpForce * 2 * Time.deltaTime);

            if (jumpEnergy.currentValue > 0)
            {
                rb.AddForce(Vector3.up * liftForce * Time.deltaTime, ForceMode.VelocityChange);
            }
            else
            {
                isJumping = false;  
                fire.Stop();
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
            fire.Stop();
        }
        UpdateAnimationStates();
    }



    private void RaycastGroundCheck()
    {
        isGrounded = Physics.Raycast(transform.position + new Vector3(0f, 0.1f, 0f), Vector3.down, 0.3f, groundLayer);

        if (isGrounded)
        {
            isJumping = false;       
            isStanding = true;
            isWalking = false;
            jumpEnergy.AddValue(Time.deltaTime);
        }
    }

    private void Move()
    {
        float horizontal = Input.GetAxis("Horizontal"); 
        float vertical = Input.GetAxis("Vertical");
        float cmMove = cmFreeLook.m_XAxis.m_InputAxisValue;
        Vector3 direction = transform.forward * vertical;

        if (vertical != 0)
        {
            Vector3 move = direction * moveSpeed * Time.deltaTime;
            rb.MovePosition(rb.position + move);
            isStanding = false;
            isWalking = true;
        }
        else
        {
            isStanding = true;
            isWalking = false;
        }

        if (horizontal != 0 && cmMove ==0)
        {
            Quaternion rotation = Quaternion.Euler(0f, horizontal * rotationSpeed, 0f);
            transform.rotation = transform.rotation * rotation;
        }
        else
        {
            if(Time.timeScale !=0f)
            {
            float value = cmMove > 1 ? 1 : (cmMove <-1 ? -1 : cmMove);
            Quaternion rotation = Quaternion.Euler(0f, value * rotationSpeed, 0f);
            transform.rotation = transform.rotation * rotation;
            }
        }
    }

    private void Jump()
    {
        if (jumpEnergy.currentValue >0)
        {
            jumpBackpack.Play();
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); 
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); 
            isJumping = true;
            isStanding = false;
            isWalking = false;
        }
    }

    private void UpdateAnimationStates()
    {
        if (!isGrounded)
        {
            isWalking = false;
            isStanding = false;
        }
        Anim.SetBool("IsJumping", !isGrounded || isJumping); 
        Anim.SetBool("IsRunning", isWalking);  
        Anim.SetBool("IsIdle", isStanding);    
    }
}
