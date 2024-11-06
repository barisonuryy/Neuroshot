using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public Animator animator;
    public PlayerMovementGrappling playerMovement;

    private void Update()
    {
        // Walking Animation
        bool isWalking = playerMovement.state == PlayerMovementGrappling.MovementState.walking;
        animator.SetBool("isWalking", isWalking);

        // Running Animation
        bool isRunning = playerMovement.state == PlayerMovementGrappling.MovementState.sprinting;
        animator.SetBool("isRunning", isRunning);

        // Crouching Animation
        bool isCrouching = playerMovement.state == PlayerMovementGrappling.MovementState.crouching;
        animator.SetBool("isCrouching", isCrouching);
        bool isCrouchWalk=playerMovement.state==PlayerMovementGrappling.MovementState.crouchWalk;
        animator.SetBool("isCrouchWalking",isCrouchWalk);
        bool isIdle = playerMovement.state == PlayerMovementGrappling.MovementState.idle;
        animator.SetBool("isIdle", isIdle);
        bool isAir = playerMovement.state == PlayerMovementGrappling.MovementState.air;
        animator.SetBool("isAir", !playerMovement.fallGrounded);
        bool isGrappling = playerMovement.activeGrapple;
        animator.SetBool("isGrappling", isGrappling);
        bool isDashing=playerMovement.dashing;
        animator.SetBool("isDashing", isDashing);
        bool isJumpGround=playerMovement.grounded;
        animator.SetBool("isAirJump",isJumpGround);
        bool isJumping=playerMovement.canJump;
        animator.SetBool("isJumping",isJumping);
        
        bool isSwinging = playerMovement.swinging;
        animator.SetBool("isSwinging", isSwinging);

        // Grappling Animation
       

        Debug.Log("isWalking" + isWalking + " isRunning" + isRunning+" isCrouching"+isCrouching+" isGrappling"+isGrappling+" isSwinging"+isSwinging+" isDashing"+isDashing);
        // Jump Animation
        if (Input.GetKeyDown(playerMovement.jumpKey) && playerMovement.grounded)
        {
            animator.SetTrigger("Jump");
        }
    }
}
