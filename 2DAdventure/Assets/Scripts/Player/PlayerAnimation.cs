using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    public PlayerController playerController;   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        animator= GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        physicsCheck= GetComponent<PhysicsCheck>();
        playerController= GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        SetAnimation();

    }


    public void SetAnimation()
    {
        animator.SetFloat("velocityX", Mathf.Abs(rb.linearVelocityX));
        animator.SetFloat("velocityY", rb.linearVelocityY);
        animator.SetBool("isGround", physicsCheck.isGrounded);
        animator.SetBool("isDead", playerController.isDead);
        animator.SetBool("isAttack", playerController.isAttack);
        animator.SetBool("isWalled", playerController.isWallSliding);
        animator.SetBool("isSlide", playerController.isSlide);
    }

    public void PlyaerHurt()
    {
        animator.SetTrigger("hurt");
    }

    public void PlyaerAttack()
    {
        animator.SetTrigger("attack");
    }

    public void PlayerSlide()
    {
        animator.SetTrigger("slide");
    }

}
