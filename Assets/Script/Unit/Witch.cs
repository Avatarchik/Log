using UnityEngine;
using System.Collections;

public class Witch : Ship {

    // Update is called once per frame
    public override void StateUpdate()
    {
        if (mTargetShipList == null)
            return;
        
        WeaponCharge();
        WeaponFire();

        EngineActive(false);
        /*
        switch (kState)
        {
            case State.Move:
                {
                    MoveUpdate();

                    if (mMoveTarget == null)
                        MoveForward();
                    else
                        Move(mMoveTarget.transform.position - transform.position);
                }
                break;
            case State.Aim:
                break;
            case State.Attack:
                AttackUpdate();
                break;
        }

        VelocityUpdate();
        */
    }
}
