using UnityEngine;
using System.Collections;
using CommonEnum;

public class ShipSupport : MonoBehaviour {
    public static string TypeToString(int _shipID)
    {
        DT_ShipData_Info info = CDT_ShipData_Manager.Instance.GetInfo(_shipID);
        if (info == null)
            return "";

        return "Ship/" + info.ResourceName;
    }


    public static bool IsSingleSpawn(Model _model)
    {
        if (_model == Model.Skeleton || _model == Model.Archer || _model == Model.SpearGoblin ||
            _model == Model.Goblin || _model == Model.Barbarian || _model == Model.Knight ||
            _model == Model.Musketeer || _model == Model.Witch || _model == Model.GoblinLord ||
            _model == Model.BarbarianLord || _model == Model.Prince || _model == Model.Valkyrie ||
            _model == Model.Bowler || _model == Model.Colossus || _model == Model.Xbow ||
            _model == Model.Giant || _model == Model.RoyalGiant || _model == Model.GiantSkeleton ||
            _model == Model.InfernoDragon || _model == Model.Bomber || _model == Model.ElectricWizard ||
            _model == Model.Guardian || _model == Model.Paladins || _model == Model.BroodLord ||
            _model == Model.ScienceVessle || _model == Model.Probe || _model == Model.CommandCenter ||
            _model == Model.Commando)
            return true;

        return false;
    }

    public static string RefTypeToString(int _shipID)
    {
        DT_ShipData_Info info = CDT_ShipData_Manager.Instance.GetInfo(_shipID);        
        return TypeToString(info.Reference);
    }

    public static string GetGradeString(Grade _grade)
    {
        switch(_grade)
        {
            case Grade.Common:
                return LocalizationManager.Instance.GetLocalValue(10);
            case Grade.Uncommon:
                return LocalizationManager.Instance.GetLocalValue(11);
            case Grade.Rare:
                return LocalizationManager.Instance.GetLocalValue(12);
            case Grade.Epic:
                return LocalizationManager.Instance.GetLocalValue(13);
            case Grade.Legendary:
                return LocalizationManager.Instance.GetLocalValue(14);
            case Grade.Artifact:
                return LocalizationManager.Instance.GetLocalValue(15);
        }

        return "";
    }
}
