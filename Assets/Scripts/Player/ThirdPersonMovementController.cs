using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovementController : MonoBehaviour
{
    // ------------------------------------
    // Ustawienia ruchu
    // ------------------------------------
    [Header("Movement")]
    public float moveSpeed = 5f;           // Pr�dko�� poruszania si�
    public float rotationSpeed = 10f;      // Pr�dko�� rotacji
    public CinemachineFreeLook cmFreeLook;

    // ------------------------------------
    // Ustawienia skoku
    // ------------------------------------
    [Header("Jump")]

    public float jumpForce = 7f;           // Si�a pocz�tkowa skoku
    public float liftForce = 2f;           // Delikatna si�a unoszenia po skoku
    public float maxJumpHoldTime = 2.5f;   // Maksymalny czas unoszenia w g�r�
    public bool isGrounded;                // Flaga kontaktu z ziemi�
    public LayerMask groundLayer;          // Warstwa reprezentuj�ca ziemi�
    [SerializeField] private PlayerValues jumpEnergy;

    // ------------------------------------
    // Komponenty
    // ------------------------------------
    private Rigidbody rb;                  // Komponent Rigidbody
    public Animator Anim;                  // Komponent Animator
    public ParticleSystem fire;            // System cz�steczek ognia

    // ------------------------------------


    // Flagi stanu gracza
    private bool isStanding = true;        // Flaga pozycji stoj�cej
    private bool isWalking = false;        // Flaga poruszania si�
    private bool isJumping = false;        // Flaga skoku

    [Header("Movement")]
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
       // JumpForceUpdate();
        // Sprawdzamy, czy gracz jest na ziemi
        RaycastGroundCheck();
        // Przemieszczamy posta�
        Move();

        // Obs�uga skoku
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
            fire.Play();
        }


        if(jumpEnergy.currentValue <=0f)
        {
            fire.Stop();
        }

        // Je�li gracz jest w trakcie skoku, dodajemy delikatn� si�� unoszenia
        if (Input.GetKey(KeyCode.Space))
        {
            jumpEnergy.RemoveValue(jumpForce * 2 * Time.deltaTime);

            if (jumpEnergy.currentValue > 0)
            {
                rb.AddForce(Vector3.up * liftForce * Time.deltaTime, ForceMode.VelocityChange);
            }
            else
            {
                isJumping = false;  // Po przekroczeniu limitu czasu, gracz zacznie opada�
                fire.Stop();
            }
        }
 


        // Po zwolnieniu klawisza skoku przestajemy unoszenie
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
            fire.Stop();
        }

        // Ustawiamy animacje
        UpdateAnimationStates();
    }



    private void RaycastGroundCheck()
    {
        // Wysy�amy promie� w d�, aby sprawdzi�, czy gracz stoi na ziemi
        isGrounded = Physics.Raycast(transform.position + new Vector3(0f, 0.1f, 0f), Vector3.down, 0.3f, groundLayer);

        if (isGrounded)
        {
            isJumping = false;       // Resetujemy stan skoku
            // Ustawiamy stany dla animacji
            isStanding = true;
            isWalking = false;

            jumpEnergy.AddValue(Time.deltaTime);
        }
    }

    private void Move()
    {
        // Pobieramy warto�ci wej�ciowe z osi
        float horizontal = Input.GetAxis("Horizontal"); // Rotacja
        float vertical = Input.GetAxis("Vertical");     // Ruch do przodu/ty�u
        float cmMove = cmFreeLook.m_XAxis.m_InputAxisValue;

        
          // Tworzymy kierunek ruchu wzgl�dem osi pionowej

          Vector3 direction = transform.forward * vertical;


        // Ruch w prz�d i ty�
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

        // Obr�t w miejscu przy u�yciu osi poziomej
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


            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // Zerujemy pionow� pr�dko��, aby skok by� sta�y
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // Pocz�tkowy "wybuch" skoku


            // Gracz zacz�� skaka�
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
        // Ustawiamy animacje na podstawie stanu gracza
        Anim.SetBool("IsJumping", !isGrounded || isJumping); // Aktywujemy animacj� skoku
        Anim.SetBool("IsRunning", isWalking);  // Aktywujemy animacj� biegania
        Anim.SetBool("IsIdle", isStanding);    // Aktywujemy animacj� stania
    }
}
