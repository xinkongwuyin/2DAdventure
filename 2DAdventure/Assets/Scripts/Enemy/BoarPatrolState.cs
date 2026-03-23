using UnityEngine;

public class BoarPatrolState: BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;

    }
    public override void LogicUpdate()
    {
        //todo
        if(currentEnemy.FoundPlayer())
        {
            currentEnemy.SwitchState(NPCState.Chase);
        }
        if (!currentEnemy.physicsCheck.isGrounded || (currentEnemy.physicsCheck.isLeftWalled && currentEnemy.faceDir.x < 0 || currentEnemy.physicsCheck.isRightWalled && currentEnemy.faceDir.x > 0))
        {
            currentEnemy.rb.linearVelocity = Vector2.zero;
            currentEnemy.wait = true;
            currentEnemy.anim.SetBool("walk", false);
        }
        else
        {
            currentEnemy.anim.SetBool("walk", true);
        }
    }

    public override void PhysicsUpdate() 
    {

    }


    public override void OnExit()
    {
        currentEnemy.anim.SetBool("walk", false);
        Debug.Log("Exit");
    }
}
