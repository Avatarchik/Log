using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using CommonEnum;

public class LocalData : SingletonT<LocalData>
{
    public const int EQUIPSLOT_COUNT = 5;

    public enum Type
    {
        Language,        
        SoundBGM,
        SoundEffect,
        SoundVoice,
        GameSpeed,
        
        TacticsPage,
        UnitSlot,

        Tutorial,

        StageClearTime,        
        QuitTime,

        Resource,
        Storage,
        Product,        
        MinCheckTime,

        ConqueredZone
    }
    
    public int language
    {
        set {PlayerPrefs.SetInt(Type.Language.ToString(), value);}
        get {return PlayerPrefs.GetInt(Type.Language.ToString(), 0);}
    }

    public float soundEffectVolume
    {
        set {PlayerPrefs.SetFloat(Type.SoundEffect.ToString(), value);}
        get {return PlayerPrefs.GetFloat(Type.SoundEffect.ToString(), 0.5f);}
    }

    public float soundBGMVolume
    {
        set {PlayerPrefs.SetFloat(Type.SoundBGM.ToString(), value);}
        get {return PlayerPrefs.GetFloat(Type.SoundBGM.ToString(), 0.5f);}
    }

    public float soundVoiceVolume
    {
        set {PlayerPrefs.SetFloat(Type.SoundVoice.ToString(), value);}
        get {return PlayerPrefs.GetFloat(Type.SoundVoice.ToString(), 0.5f);}
    }

    public float gameSpeed
    {
        set {PlayerPrefs.SetFloat(Type.GameSpeed.ToString(), value);}
        get {return PlayerPrefs.GetFloat(Type.GameSpeed.ToString(), 1.0f);}
    }

    public int tacticsPage
    {
        set { PlayerPrefs.SetInt(Type.TacticsPage.ToString(), value); }
        get { return PlayerPrefs.GetInt(Type.TacticsPage.ToString(), 0); }
    }

    public string quitTime
    {
        set { PlayerPrefs.SetString(Type.QuitTime.ToString(), value); }
        get { return PlayerPrefs.GetString(Type.QuitTime.ToString(), System.DateTime.Now.ToString()); }
    }

    public int minCheckTime
    {
        set { PlayerPrefs.SetInt(Type.MinCheckTime.ToString(), value); }
        get { return PlayerPrefs.GetInt(Type.MinCheckTime.ToString(), 0); }
    }
    
    public int GetSlotData(int _slotIndex)
    {
        return PlayerPrefs.GetInt(Type.UnitSlot.ToString() + "_" + tacticsPage + "_" + _slotIndex, 0);
    }

    public void SetSlotData(int _slotIndex, int _shipID)
    {
        PlayerPrefs.SetInt(Type.UnitSlot.ToString() + "_" + tacticsPage + "_" + _slotIndex, _shipID);
    }

    public float GetClearTime(int _stageIndex)
    {
        return PlayerPrefs.GetFloat(Type.StageClearTime.ToString() + "_" + _stageIndex, 0);
    }

    public void SetClearTime(int _stageIndex, float _time)
    {
        PlayerPrefs.SetFloat(Type.StageClearTime.ToString() + "_" + _stageIndex, _time);
    }

    public void TutorialFinish()
    {
        PlayerPrefs.SetInt(Type.Tutorial.ToString(), 1);
    }

    public int IsTutorial()
    {
        return PlayerPrefs.GetInt(Type.Tutorial.ToString(), 0);
    }

    public void SetPlanetRes(string _name, ResourceType _type, float _value)
    {
        PlayerPrefs.SetFloat(Type.Resource.ToString() + _name + _type.ToString(), _value);
    }
    public float GetPlanetRes(string _name, ResourceType _type)
    {
        return PlayerPrefs.GetFloat(Type.Resource.ToString() + _name + _type.ToString(), 0);
    }

    public void SetPlanetProductLevel(string _name, int _level)
    {
        PlayerPrefs.SetInt(Type.Product.ToString() + _name, _level);
    }
    public int GetPlanetProductLevel(string _name)
    {
        return PlayerPrefs.GetInt(Type.Product.ToString() + _name, 0);
    }

    public void SetPlanetStorageLevel(string _name, int _value)
    {
        PlayerPrefs.SetInt(Type.Storage.ToString() + _name, _value);
    }
    public int GetPlanetStorageLevel(string _name)
    {
        return PlayerPrefs.GetInt(Type.Storage.ToString() + _name, 0);
    }

    public void SetConqueredZone(int _row, int _col, Nation.Name _name)
    {
        PlayerPrefs.SetInt(Type.ConqueredZone.ToString() + "_" + _col + "_" + _row, (int)_name);
    }

    public Nation.Name GetConqueredZone(int _row, int _col)
    {
        return (Nation.Name)PlayerPrefs.GetInt(Type.ConqueredZone.ToString() + "_" + _col + "_" + _row, 0);
    }
}
 