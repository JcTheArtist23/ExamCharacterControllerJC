using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    public float horizontalMove;
    public float verticalMove;
    private Vector3 playerInput;
    private Vector3 movePlayer;

    private Vector3 camForward;
    private Vector3 camRight;
    public Camera mainCamera;

    public CharacterController player;
    public float playerSpeed;
    public float gravity = 9.8f;
    public float fallVelocity;
    public float jumpForce;
    
    public Animator playerAnimatorController;


    // Start is called before the first frame update
    void Start()
    {
      player = GetComponent<CharacterController>(); 
      playerAnimatorController = GetComponent<Animator>(); 
    }

    // Update is called once per frame
    void Update()
    {
       horizontalMove = Input.GetAxis("Horizontal"); 
       verticalMove = Input.GetAxis("Vertical"); 

       playerInput = new Vector3(horizontalMove, 0 , verticalMove);
       playerInput = Vector3.ClampMagnitude(playerInput, 1);

       playerAnimatorController.SetFloat("PlayerWalkVelocity", playerInput.magnitude * playerSpeed);

       camDirection();

       movePlayer = playerInput.x * camRight + playerInput.z * camForward;

       movePlayer = movePlayer * playerSpeed;

       player.transform.LookAt(player.transform.position + movePlayer);

       SetGravity();

       PlayerSkills();

       player.Move(movePlayer * Time.deltaTime);

       Debug.Log(player.velocity.magnitude);
    }

    void camDirection()
    {
        camForward =mainCamera.transform.forward;
        camRight = mainCamera.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward = camForward.normalized;
        camRight = camRight.normalized;
    }

    void PlayerSkills()
    {
        if(player.isGrounded && Input.GetButtonDown("Jump"))
        {
            fallVelocity = jumpForce;
            movePlayer.y = fallVelocity;
            playerAnimatorController.SetTrigger("PlayerJump");  
        }
    }

    void SetGravity()
    {
       if (player.isGrounded)
        {
           fallVelocity = -gravity * Time.deltaTime ;
           movePlayer.y =fallVelocity;
           
        }
        else
        {
            fallVelocity -= gravity * Time.deltaTime ;
            movePlayer.y = fallVelocity;
            playerAnimatorController.SetFloat("PlayerverticalVelocity", player.velocity.y);
        }
        playerAnimatorController.SetBool("IsGrounded", player.isGrounded);
    }

    private void OnAnimatorMove() 
    {
        
    }

    
}
