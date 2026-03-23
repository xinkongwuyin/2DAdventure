using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody2D rb;
    [HideInInspector]public Animator anim;
    [HideInInspector]public PhysicsCheck physicsCheck;

    [Header("基本参数")]
    public float normalSpeed;
    public float chaseSpeed;
    public float currentSpeed;
    public Vector3 faceDir;
    public float hurtForce;

    public Transform attacker;

    [Header("检测")]
    public Vector2 centerOffset;
    public Vector2 checkSize;
    public float checkDistance;
    public LayerMask attackLayer;


    [Header("计时器")]
    public float waitTime;
    public float waitTimeCounter;
    public bool wait;
    public float lostTime;
    public float lostTtimeCounter;

    [Header("状态")]
    public bool isHurt;
    public bool isDead;

    protected BaseState patrolState;
    private BaseState currentState;
    protected BaseState chaseState;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        physicsCheck = GetComponent<PhysicsCheck>();
        currentSpeed = normalSpeed;

        
        //waitTimeCounter = waitTime;
    }

    private void OnEnable()
    {
        currentState = patrolState;
        currentState.OnEnter(this);
    }


    // Update is called once per frame
    private void Update()
    {
        faceDir=new Vector3(-transform.localScale.x,0,0);
             
        currentState.LogicUpdate();
        TimerCounter();
    }

    private void FixedUpdate()
    {
        if(!isHurt&&!wait)
            Move();
        currentState.PhysicsUpdate();
    }

    private void OnDisable()
    {
        currentState.OnExit();
    }
    public virtual void Move()
    {
        if (!isDead)
        {
            rb.linearVelocity = new Vector2(faceDir.x * currentSpeed * Time.deltaTime, rb.linearVelocity.y);
        }
    }

    public void TimerCounter()
    {
        if (wait)
        {
            waitTimeCounter -= Time.deltaTime;
            if (waitTimeCounter <= 0)
            {
                wait = false;
                waitTimeCounter = waitTime;
                transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            }
        }

        if (!FoundPlayer()&&lostTtimeCounter>0)
        {
            lostTtimeCounter -= Time.deltaTime;
        }

    }







    #region 事件执行方法

    public void onTakeDemage(Transform attackTrans)
    {
        attacker = attackTrans;
        if (attackTrans.position.x - transform.position.x > 0)
        {
            transform.localScale=new Vector3(-1,1,1);
        } 
        if(attackTrans.position.x - transform.position.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        isHurt = true;
        anim.SetTrigger("hurt");

        Vector2 dir=new Vector2(transform.position.x - attackTrans.position.x, 0).normalized;
        rb.linearVelocity = Vector2.zero;
        StartCoroutine(OnHurt(dir));

    }

    IEnumerator OnHurt(Vector2 dir)
    {
      
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);
        isHurt = false;
    }

    public void OnDie() 
    {
        gameObject.layer = 2; //设置为Ignore Raycast层，避免被攻击到
        anim.SetBool("dead",true);
        isDead = true;
    }

    public void DestryAfterAnimation()
    {
        Destroy(this.gameObject);
    }

    public bool FoundPlayer()
    {
        var temp = Physics2D.BoxCast(transform.position + (Vector3)centerOffset, checkSize, 0, faceDir, checkDistance, attackLayer);
        return temp;
    }

    public void SwitchState(NPCState state)
    {
        var newState = state switch
        {
            NPCState.Patrol => patrolState,
            NPCState.Chase => chaseState,
            _ => null
        };

        currentState.OnExit();
        currentState=newState;
        currentState.OnEnter(this);
    }

    #endregion

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + (Vector3)centerOffset + new Vector3(checkDistance * -transform.localScale.x, 0), 0.2f);
    }
}
