using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BroodLord : Ship {
    [HideInInspector]
    //사용 가능한 수리 함정
    public List<Brood> kAvailableBroodList = new List<Brood>();
    //모든 수리 함정
    public List<Brood> kTotalBroodList = new List<Brood>();

    //수리 접수된 아군 배
    public List<Ship> kReceiptShipList = new List<Ship>();

    public override void OnPrepare()
    {
        base.OnPrepare();

        Transform weaponTrans = transform.Find("Weapon");
        for (int i = 0; i < weaponTrans.childCount; i++)
        {
            Brood brood = weaponTrans.GetChild(i).GetComponent<Brood>();
            
            switch (i)
            {
                case 0:
                    brood.transform.localPosition = new Vector3(6.0f, 3.5f, -10.0f);                    
                    break;
                case 1:
                    brood.transform.localPosition = new Vector3(0.0f, 3.5f, -10.0f);
                    break;
                case 2:
                    brood.transform.localPosition = new Vector3(-6.0f, 3.5f, -10.0f);
                    break;
            }

            brood.transform.forward = transform.forward;

            brood.OnPrepare();
            brood.SetParent(this);
            kAvailableBroodList.Add(brood);
            kTotalBroodList.Add(brood);
        }
    }

    void OnDisable()
    {
        for (int i = 0; i < kTotalBroodList.Count; i++)
        {
            kTotalBroodList[i].gameObject.SetActive(true);
            kTotalBroodList[i].transform.parent = transform.Find("Weapon");
        }

        kTotalBroodList.Clear();
        kAvailableBroodList.Clear();
        kReceiptShipList.Clear();

        StopAllCoroutines();
    }

    public override void StateUpdate()
    {
        if (mTargetShipList == null)
            return;

        if (mTargetShipList.Count == 0)
            kState = State.Move;

        BroodManageUpdate();

        switch (kState)
        {  
            case State.Move:
                {
                    MoveForward();
                    Move(transform.forward);
                }
                break;
            case State.Aim:
                break;
            case State.Attack:
                AttackUpdate();
                break;
        }

        VelocityUpdate();
    }

    public void WorkComplete(Brood _brood, Ship _target)
    {
        kAvailableBroodList.Add(_brood);
        kReceiptShipList.Remove(_target);
    }

    List<Ship> mRepairShipList = new List<Ship>();    
    void BroodManageUpdate()
    {
        for (int i = 0; i < kTotalBroodList.Count; i++)
            kTotalBroodList[i].TargetLostUpdate();

        if (kAvailableBroodList.Count == 0)
            return;

        mRepairShipList.Clear();

        for(int i = 0; i < mOurShipList.Count; i++)
        {
            Ship ship = mOurShipList[i];
            if (kReceiptShipList.Contains(ship))
                continue;
            if (ship.kCurHealthPoint < ship.kTotalHealthPoint ||
                ship.kCurShieldPoint < ship.kTotalShieldPoint )
                mRepairShipList.Add(ship);
        }

        mRepairShipList.Sort(delegate (Ship x, Ship y)
        {
            int xSum = x.kTotalHealthPoint + x.kTotalShieldPoint;
            int ySum = y.kTotalHealthPoint + y.kTotalShieldPoint;

            if (xSum > ySum)
                return 1;
            else if ((xSum < ySum))
                return -1;
            else
                return 0;
        });
        
        for (int i = 0; i < mRepairShipList.Count; i++)
        {
            Ship targetShip = mRepairShipList[i];
            Brood brood = kAvailableBroodList[0];
            brood.SetTarget(targetShip);
            kReceiptShipList.Add(targetShip);
            
            if (kAvailableBroodList.Count == 0)
                break;
        }
    }

    public override void Die()
    {
        for (int i = 0; i < kTotalBroodList.Count; i++)
        {
            Brood brood = kTotalBroodList[i];
            brood.transform.parent = transform;
        }

        base.Die();
    }
}
