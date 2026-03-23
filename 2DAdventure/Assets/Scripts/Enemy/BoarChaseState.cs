using UnityEngine;

public class BoarChaseState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        //Debug.Log("Chase");
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        currentEnemy.anim.SetBool("run", true);

    }
    public override void LogicUpdate()
    {
        if (currentEnemy.lostTtimeCounter <= 0)
        {
            currentEnemy.SwitchState(NPCState.Patrol);
        }
        if (!currentEnemy.physicsCheck.isGrounded || (currentEnemy.physicsCheck.isLeftWalled && currentEnemy.faceDir.x < 0 || currentEnemy.physicsCheck.isRightWalled && currentEnemy.faceDir.x > 0))
        {
            currentEnemy.transform.localScale = new Vector3(currentEnemy.faceDir.x, 1, 1);
        }

    }

    public override void PhysicsUpdate()
    {

    }


    public override void OnExit()
    {
        currentEnemy.anim.SetBool("run", false);
        currentEnemy.lostTtimeCounter = currentEnemy.lostTime;
    }

}
