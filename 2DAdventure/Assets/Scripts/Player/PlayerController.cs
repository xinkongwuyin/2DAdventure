using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Processors;
using System;
public class PlayerController : MonoBehaviour
{
    [Header("监听事件")]
    public SceneLoadEventSQ loadEventSQ;
    public VoidEventsSQ loadSceneAfter;
    public VoidEventsSQ loadDataEvent;
    public VoidEventsSQ backToMenuEvent;

    public PlayerInputController inputController;
    public Vector2 inputDirection;
    public bool isRunning;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private PhysicsCheck physicsCheck;
    private PlayerInput _internalPlayerInput;
    private PlayerAnimation playerAnimation;
    public Collider2D coll;
    public bool isSlide;


    [Header("基本参数")]
    public float moveSpeed;
    public float jumpForce;
    public float wallSlideSpeed;
    public float slideSpeed;
    private float wallJumpTimer;
    public float wallJumpDistance;
    public float hurtForce;

    [Header("物理材质")]
    public PhysicsMaterial2D normal;
    public PhysicsMaterial2D wall;

    [Header("状态")]
    public bool isHurt;

    public bool isDead;
    public bool isAttack;
    public bool isWallSliding;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        rb= GetComponent<Rigidbody2D>();
        physicsCheck= GetComponent<PhysicsCheck>();
        spriteRenderer= GetComponent<SpriteRenderer>();
        playerAnimation= GetComponent<PlayerAnimation>();
        inputController = new PlayerInputController();
        coll= GetComponent<Collider2D>();

        //绑定跳跃事件
        inputController.GamePlay.JUMP.started += Jump;
        //绑定攻击事件
        inputController.GamePlay.ATTACK.started += PlayerAttack;
        //绑定滑铲事件
        inputController.GamePlay.SLIDE.started += Slide;

        isAttack = false;
        isDead = false;
        isHurt = false;
        inputController.Enable();
        
    }



    // Update is called once per frame
    private void OnEnable()
    {
        loadEventSQ.LoadRequestEvent += OnLoadRequestEvent;
        loadSceneAfter.OnEventRaised+= OnLoadSceneAfter;
        loadDataEvent.OnEventRaised += OnLoadDataEvent;
        backToMenuEvent.OnEventRaised += OnLoadDataEvent;
    }



    private void OnDisable()
    {
        inputController.Disable();
        loadEventSQ.LoadRequestEvent -= OnLoadRequestEvent;
        loadSceneAfter.OnEventRaised -= OnLoadSceneAfter;
        loadDataEvent.OnEventRaised -= OnLoadDataEvent;
        backToMenuEvent.OnEventRaised -= OnLoadDataEvent;
    }



    private void Update()
    {
        inputDirection = inputController.GamePlay.Move.ReadValue<Vector2>();
        isRunning=inputController.GamePlay.RUN.IsPressed();
        CheckState();
        WallSlide();
    }
    
    private void FixedUpdate()
    {
        if (!isHurt&&!isAttack&&!isSlide)
        {
            Move();
        }
       
    }

    //测试
    /*
    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log(other.name)
    }
    */
    //加载禁止动作
    private void OnLoadRequestEvent(GameSceneSQ arg0, Vector3 arg1, bool arg2)
    {
        inputController.GamePlay.Disable();
    }
    //加载完成允许动作
    private void OnLoadSceneAfter()
    {
        inputController.GamePlay.Enable();
    }

    private void OnLoadDataEvent()
    {
        isDead = false;
    }

    public void Move()
    {
        if (physicsCheck.isLeftWalled)
        {

            rb.linearVelocity = new Vector2(moveSpeed * Time.deltaTime * (inputDirection.x>0?inputDirection.x:0), rb.linearVelocity.y);
        }
        else if (physicsCheck.isRightWalled)
        {
            rb.linearVelocity = new Vector2(moveSpeed * Time.deltaTime * (inputDirection.x < 0 ? inputDirection.x : 0), rb.linearVelocity.y);
        }
        else
        {
            if (isRunning)
            {
                rb.linearVelocity = new Vector2(moveSpeed * Time.deltaTime * inputDirection.x, rb.linearVelocity.y);
            }
            else
                rb.linearVelocity = new Vector2(moveSpeed * Time.deltaTime * inputDirection.x / 2, rb.linearVelocity.y);
        }


        //面向facedir
        if (inputDirection.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (inputDirection.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

    }

    private void Jump(InputAction.CallbackContext obj)
    {
       //Debug.Log("Jump");
        if (physicsCheck.isGrounded)
        rb.AddForce(transform.up*jumpForce, ForceMode2D.Impulse);
        else if (isWallSliding)
        {
            float wallDir = physicsCheck.isLeftWalled ? 1 : -1;
            Vector2 wallJumpDirection = new Vector2(wallDir, 1.2f);
            wallJumpTimer = 0.2f;

            // 这里的关键是：不仅向上跳，还要向墙的反方向推
            // 假设你的墙体检测能给出墙的方向，或者简单地取当前角色面对方向的反向


            rb.linearVelocity = Vector2.zero; 
            rb.AddForce(wallJumpDirection.normalized * jumpForce*wallJumpDistance, ForceMode2D.Impulse);

  
        }
    }

    private void PlayerAttack(InputAction.CallbackContext obj)
    {
        playerAnimation.PlyaerAttack();
        isAttack = true;
    }

    private void Slide(InputAction.CallbackContext obj)
    {
        if (physicsCheck.isGrounded && inputDirection.x != 0)
        {
            isSlide = true;
            GetComponent<Character>().invulnerable = true;
            rb.AddForce(new Vector2(inputDirection.x * slideSpeed, 0), ForceMode2D.Impulse);
            playerAnimation.PlayerSlide();
        }
    }

    public void CheckState()
    {
        coll.sharedMaterial = physicsCheck.isGrounded ? normal : wall;
        if (wallJumpTimer <= 0) { 
            isWallSliding = !physicsCheck.isGrounded && (physicsCheck.isLeftWalled && inputDirection.x < 0) || (physicsCheck.isRightWalled && inputDirection.x > 0);
            if (physicsCheck.isGrounded) 
                   isWallSliding = false;
        }
        else
        {
            isWallSliding = false;
            wallJumpTimer -= Time.deltaTime;
        }
    }

    public void WallSlide()
    {
        if (isWallSliding)
        {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, wallSlideSpeed);
        }
    }

    #region Unity Events
    public void GetHurt(Transform attacker)
    {
        isHurt = true;
        playerAnimation.PlyaerHurt();
        rb.linearVelocity = Vector2.zero;
        Vector2 dir = new Vector2(transform.position.x-attacker.position.x,0).normalized;
        rb.AddForce(dir  * hurtForce, ForceMode2D.Impulse);
    }

    public void PlyaDie()
    {
        isDead = true;
        inputController.GamePlay.Disable();
        Debug.Log("Player Died");

    }



    #endregion

}
