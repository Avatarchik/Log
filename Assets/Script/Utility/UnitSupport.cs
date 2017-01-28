using UnityEngine;
using System.Collections;
using CommonEnum;

public class UnitSupport : MonoBehaviour {
    public static string TypeToString(int _shipID)
    {        
        return "Ship/" + ((Model)_shipID).ToString();
    }

    public static string ChildTypeToString(int _shipID)
    {
        return "Ship/" + GetChildUnit(_shipID).ToString();
    }

    public static Model GetChildUnit(int _unitID)
    {
        switch((Model)_unitID)
        {
            case Model.SkeletonSquad:
            case Model.SkeletonArmy:
                return Model.Skeleton;

            case Model.SpearGoblinArmy:
            case Model.SpearGoblinSquad:
                return Model.SpearGoblin;

            case Model.GoblinArmy:
            case Model.GoblinLord:
                return Model.Goblin;

            case Model.ArcherArmy:
            case Model.ArcherSquad:
                return Model.Archer;

            case Model.BarbarianArmy:
            case Model.BarbarianLord:
                return Model.Barbarian;

            case Model.KnightSquad:
            case Model.KnightArmy:
                return Model.Knight;

            case Model.MusketeerSquad:
            case Model.MusketeerArmy:
                return Model.Musketeer;

            case Model.WitchSquad:
                return Model.Witch;

            case Model.SpearGoblinLordSquad:
                return Model.SpearGoblinLord;

            case Model.GoblinLordSquad:
                return Model.GoblinLord;

            case Model.BarbarianLordSquad:
                return Model.BarbarianLord;

            case Model.PrinceSquad:
            case Model.PrinceArmy:
                return Model.Prince;

            case Model.ValkyrieSquad:
            case Model.ValkyrieArmy:
                return Model.Valkyrie;

            case Model.BowlerSquad:
            case Model.BowlerArmy:
                return Model.Bowler;

            case Model.ColossusSquad:
            case Model.ColossusArmy:
                return Model.Colossus;

            case Model.XbowSquad:
            case Model.XbowArmy:
                return Model.Xbow;

            case Model.GiantSquad:
                return Model.Giant;

            case Model.RoyalGiantSquad:
                return Model.RoyalGiant;

            case Model.GiantSkeletonSquad:
                return Model.GiantSkeleton;

            case Model.InfernoDragonSquad:
            case Model.InfernoDragonArmy:
                return Model.InfernoDragon;

            case Model.BomberSquad:
            case Model.BomberArmy:
                return Model.Bomber;

            case Model.ElectricWizardSquad:
            case Model.ElectricWizardArmy:
                return Model.ElectricWizard;

            case Model.GuardianSquad:
                return Model.Guardian;

            case Model.PaladinsSquad:
                return Model.Paladins;

            case Model.BroodLordSquad:
                return Model.BroodLord;
                
            case Model.ScienceVessleSquad:
                return Model.ScienceVessle;
                                
            case Model.ProbeSquad:
            case Model.ProbeArmy:
                return Model.Probe;
                
            case Model.CommandCenterSquad:
            case Model.CommandCenterArmy:
                return Model.CommandCenter;
        
            case Model.CommandoSquad:
            case Model.CommandoArmy:
                return Model.Commando;
        }

        return Model.None;
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
