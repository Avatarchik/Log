using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using CommonEnum;

public class UserData : SingletonT<UserData>
{
    public const int EQUIPSLOT_COUNT = 5;

    public enum Type
    {
        UserGold,
        UserMaterial,
        UserCristal,
        UserLevel,
        UserExp,

        UserAttackAbility,
        UserMoveAbility,
        UserTurnAbility,
        UserShieldAbility,
        UserBodyAbility,        
        UserIncomingsAbility,
        UserOutgoingsAbility,
        UserResourceAbility,
        UserMilitaryAbility,

        UserHaveUnit,
        UserHaveMilitary
    }
    
    public int gold
    {
        set { PlayerPrefs.SetInt(Type.UserGold.ToString(), value); }
        get { return PlayerPrefs.GetInt(Type.UserGold.ToString(), 10000000); }
    }

    public int material
    {
        set { PlayerPrefs.SetInt(Type.UserMaterial.ToString(), value); }
        get { return PlayerPrefs.GetInt(Type.UserMaterial.ToString(), 0); }
    }

    public int cristal
    {
        set { PlayerPrefs.SetInt(Type.UserCristal.ToString(), value); }
        get { return PlayerPrefs.GetInt(Type.UserCristal.ToString(), 0); }
    }

    public int level
    {
        set { PlayerPrefs.SetInt(Type.UserLevel.ToString(), value); }
        get { return PlayerPrefs.GetInt(Type.UserLevel.ToString(), 1); }
    }

    public int exp
    {
        set { PlayerPrefs.SetInt(Type.UserExp.ToString(), value); }
        get { return PlayerPrefs.GetInt(Type.UserExp.ToString(), 0); }
    }
    
    public int attackAbility
    {
        set { PlayerPrefs.SetInt(Type.UserAttackAbility.ToString(), value); }
        get { return PlayerPrefs.GetInt(Type.UserAttackAbility.ToString(), 0); }
    }

    public int moveAbility
    {
        set { PlayerPrefs.SetInt(Type.UserMoveAbility.ToString(), value); }
        get { return PlayerPrefs.GetInt(Type.UserMoveAbility.ToString(), 0); }
    }

    public int turnAbility
    {
        set { PlayerPrefs.SetInt(Type.UserTurnAbility.ToString(), value); }
        get { return PlayerPrefs.GetInt(Type.UserTurnAbility.ToString(), 0); }
    }

    public int shieldAbility
    {
        set { PlayerPrefs.SetInt(Type.UserShieldAbility.ToString(), value); }
        get { return PlayerPrefs.GetInt(Type.UserShieldAbility.ToString(), 0); }
    }

    public int bodyAbility
    {
        set { PlayerPrefs.SetInt(Type.UserBodyAbility.ToString(), value); }
        get { return PlayerPrefs.GetInt(Type.UserBodyAbility.ToString(), 0); }
    }

    public int incomingsAbility
    {
        set { PlayerPrefs.SetInt(Type.UserIncomingsAbility.ToString(), value); }
        get { return PlayerPrefs.GetInt(Type.UserIncomingsAbility.ToString(), 0); }
    }

    public int outgoingsAbility
    {
        set { PlayerPrefs.SetInt(Type.UserOutgoingsAbility.ToString(), value); }
        get { return PlayerPrefs.GetInt(Type.UserOutgoingsAbility.ToString(), 0); }
    }

    public int resourceAbility
    {
        set { PlayerPrefs.SetInt(Type.UserResourceAbility.ToString(), value); }
        get { return PlayerPrefs.GetInt(Type.UserResourceAbility.ToString(), 0); }
    }

    public int militaryAbility
    {
        set { PlayerPrefs.SetInt(Type.UserMilitaryAbility.ToString(), value); }
        get { return PlayerPrefs.GetInt(Type.UserMilitaryAbility.ToString(), 0); }
    }
    
    public float GetHaveUnitCount(int _shipID)
    {
        return PlayerPrefs.GetFloat(Type.UserHaveUnit.ToString() + _shipID, 0.0f);
    }

    public void SetHaveUnitCount(int _shipID, float _cnt)
    {
        PlayerPrefs.SetFloat(Type.UserHaveUnit.ToString() + _shipID, _cnt);
    }

    public int GetHaveZoneMilitary(string _name)
    {
        return PlayerPrefs.GetInt(Type.UserHaveMilitary.ToString() + _name, 0);
    }

    public void SetHaveZoneMilitary(string _name, int _military)
    {
        PlayerPrefs.SetInt(Type.UserHaveMilitary.ToString() + _name, _military);
    }
}
