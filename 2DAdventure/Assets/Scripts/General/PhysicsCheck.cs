using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class PhysicsCheck: MonoBehaviour
{
    private CapsuleCollider2D coll;
    [Header("¼ì²â²ÎÊý")]
    public Vector2 groundBottomOffset;
    public Vector2 wallLeftOffset;
    public Vector2 wallRightOffset;
    public LayerMask groundLayer;
    public bool manual;

    [Header("¼ì²â×´Ì¬")]
    public bool isGrounded;
    public bool isLeftWalled;
    public bool isRightWalled;
    public float checkGroundRadius;
    public float checkWallRadius;

    private void Awake()
    {
        coll = GetComponent<CapsuleCollider2D>();

        if (!manual)
        {
            wallRightOffset = new Vector2(coll.size.x / 2 +coll.offset.x, coll.size.y/2);
            wallLeftOffset = new Vector2(-coll.size.x / 2 + coll.offset.x, coll.size.y / 2);
        }

    }

    private void Update()
    {
        Check();

    }

    public void Check()
    {
        isGrounded = Physics2D.OverlapCircle((Vector2)transform.position+groundBottomOffset*transform.localScale, checkGroundRadius, groundLayer);
        isLeftWalled = Physics2D.OverlapCircle((Vector2)transform.position + wallLeftOffset, checkWallRadius, groundLayer);
        isRightWalled= Physics2D.OverlapCircle((Vector2)transform.position + wallRightOffset, checkWallRadius, groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + groundBottomOffset, checkGroundRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + wallLeftOffset, checkWallRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + wallRightOffset, checkWallRadius);
    }
}
