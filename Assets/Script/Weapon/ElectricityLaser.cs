using UnityEngine;
using System.Collections;

public class ElectricityLaser : Projectile {
    protected override void OnTargetHit()
    {
        Hit();

        if (mTargetShip.kShieldCollider.enabled == true)
        {
            Shield shield = ObjectPoolManager.Instance.GetGameObejct(StrDef.EFFECT_SHIELD).GetComponent<Shield>();
            shield.Play();
            shield.transform.forward = -transform.forward;
            shield.transform.position = transform.position - transform.forward;
        }
        else
        {
            mTargetShip.AddDebuff(Ship.Debuff.Electricity);
        }

        if (mTargetShip.kIsDie == false)
        {
            int damage = Random.Range(mMinDamage, mMaxDamage + 1);
            mTargetShip.Damaged(damage);
        }

        Release();
    }
}
