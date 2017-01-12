using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LocalData : SingletonT<LocalData>
{
    public const int EQUIPSLOT_COUNT = 5;

    public enum Type
    {
        Language,        
        SoundBGM,
        SoundEffect,
        SoundVoice,        
        SaveGold,
        GameSpeed,
        TacticsPage,
        ShipSlot,
        ShipLevel,
        StageClearTime,
    }
    
    public int language
    {
        set
        {
            PlayerPrefs.SetInt(Type.Language.ToString(), value);
        }
        get
        {
            return PlayerPrefs.GetInt(Type.Language.ToString(), 0);
        }
    }
    
    public long Gold
    {
        set
        {
            PlayerPrefs.SetString(Type.SaveGold.ToString(), value.ToString());
        }
        get
        {
            return long.Parse(PlayerPrefs.GetString(Type.SaveGold.ToString() ));
        }
    }
    
    public float soundEffectVolume
    {
        set
        {
            PlayerPrefs.SetFloat(Type.SoundEffect.ToString(), value);
        }
        get
        {
            return PlayerPrefs.GetFloat(Type.SoundEffect.ToString(), 0.5f);
        }
    }

    public float soundBGMVolume
    {
        set
        {
            PlayerPrefs.SetFloat(Type.SoundBGM.ToString(), value);
        }
        get
        {
            return PlayerPrefs.GetFloat(Type.SoundBGM.ToString(), 0.5f);
        }
    }

    public float soundVoiceVolume
    {
        set
        {
            PlayerPrefs.SetFloat(Type.SoundVoice.ToString(), value);
        }
        get
        {
            return PlayerPrefs.GetFloat(Type.SoundVoice.ToString(), 0.5f);
        }
    }

    public float gameSpeed
    {
        set
        {
            PlayerPrefs.SetFloat(Type.GameSpeed.ToString(), value);
        }
        get
        {
            return PlayerPrefs.GetFloat(Type.GameSpeed.ToString(), 1.0f);
        }
    }

    public int tacticsPage
    {
        set
        {
            PlayerPrefs.SetInt(Type.TacticsPage.ToString(), value);
        }
        get
        {
            return PlayerPrefs.GetInt(Type.TacticsPage.ToString(), 0);
        }
    }
    
    public int GetShipLevel(int _shipID)
    {
        return PlayerPrefs.GetInt(Type.ShipLevel.ToString() + _shipID, 1);
    }

    public void SetShipLevel(int _shipID, int _level)
    {
        PlayerPrefs.SetInt(Type.ShipLevel.ToString() + _shipID, _level);
    }

    public int GetSlotData(int _slotIndex)
    {
        return PlayerPrefs.GetInt(Type.ShipSlot.ToString() + "_" + tacticsPage + "_" + _slotIndex, 0);
    }

    public void SetSlotData(int _slotIndex, int _shipID)
    {
        PlayerPrefs.SetInt(Type.ShipSlot.ToString() + "_" + tacticsPage + "_" + _slotIndex, _shipID);
    }

    public float GetClearTime(int _stageIndex)
    {
        return PlayerPrefs.GetFloat(Type.StageClearTime.ToString() + "_" + _stageIndex, 0);
    }

    public void SetClearTime(int _stageIndex, float _time)
    {
        PlayerPrefs.SetFloat(Type.StageClearTime.ToString() + "_" + _stageIndex, _time);
    }
}
 