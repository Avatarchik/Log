using UnityEngine;
using System;
using System.Collections.Generic;

public sealed class CDataManagerNavigator
{
	static readonly CDataManagerNavigator instance = new CDataManagerNavigator();
	private Dictionary<System.Type, System.Object> Navigator = new Dictionary<System.Type, System.Object>();

	public string g_szRootPath{ get; set; }

	public bool g_bIsEncoding{ get; set; }

	public bool g_bIsLoadAssetBundle{ get; set; }

	CDataManagerNavigator()
	{
		g_szRootPath = ".";
		if (Application.platform == RuntimePlatform.Android ||
			Application.platform == RuntimePlatform.IPhonePlayer)
		{
			g_szRootPath = Application.persistentDataPath;
		}

		g_bIsEncoding = false;

		g_bIsLoadAssetBundle = false;

	}

	public static CDataManagerNavigator Instance
	{
		get
		{
			return instance;
		}
	}

	public void Load()
	{
		if (0 != Navigator.Count) return;

		Navigator.Add(typeof(CDT_LocalizingData_Manager), CDT_LocalizingData_Manager.Instance);
		Navigator.Add(typeof(CDT_PlanetData_Manager), CDT_PlanetData_Manager.Instance);
		Navigator.Add(typeof(CDT_SoundData_Manager), CDT_SoundData_Manager.Instance);
		Navigator.Add(typeof(CDT_StageData_Manager), CDT_StageData_Manager.Instance);
		Navigator.Add(typeof(CDT_UnitData_Manager), CDT_UnitData_Manager.Instance);
		Navigator.Add(typeof(CDT_UnitGroupData_Manager), CDT_UnitGroupData_Manager.Instance);
	}

	public T GetManager<T>()
	{
		if (0 == Navigator.Count)
			Load();

		if (false == Navigator.ContainsKey(typeof(T)))
			return default(T);

		return (T)Navigator[typeof(T)];
	}

}
