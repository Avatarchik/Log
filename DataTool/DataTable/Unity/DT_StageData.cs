using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;

public class DT_StageData_Info
{
	/// <summary> Stage ID </summary>
	public int	ID;
	/// <summary> 소환될 함선 아이디 </summary>
	public int	Stage;
	/// <summary> 소환될 함선 아이디 </summary>
	public int	Spot1;
	/// <summary> 소환될 함선 아이디 </summary>
	public int	Spot2;
	/// <summary> 소환될 함선 아이디 </summary>
	public int	Spot3;
	/// <summary> 소환될 함선 아이디 </summary>
	public int	Spot4;
	/// <summary> 소환될 함선 아이디 </summary>
	public int	Spot5;
	/// <summary> 소환될 함선 아이디 </summary>
	public int	Spot6;
	/// <summary> 소환될 함선 아이디 </summary>
	public int	Spot7;
	/// <summary> 소환될 함선 아이디 </summary>
	public int	Spot8;
	/// <summary> 소환될 함선 아이디 </summary>
	public int	Spot9;
	/// <summary> 소환될 함선 아이디 </summary>
	public int	Spot10;
	/// <summary> 소환될 함선 아이디 </summary>
	public int	Spot11;
	/// <summary> 소환될 함선 아이디 </summary>
	public int	Spot12;
	/// <summary> 소환될 함선 아이디 </summary>
	public int	Spot13;
	/// <summary> 소환될 함선 아이디 </summary>
	public int	Spot14;
	/// <summary> 소환될 함선 아이디 </summary>
	public int	Spot15;
}

public sealed class CDT_StageData_Manager
{
	static readonly CDT_StageData_Manager instance =  new CDT_StageData_Manager();
	private Dictionary<string, DT_StageData_Info> m_Table = new Dictionary<string, DT_StageData_Info>();
	private List<DT_StageData_Info> m_List = new List<DT_StageData_Info>();
	private string m_xmlPath = "";

	CDT_StageData_Manager(bool isDataLoad = true)
	{
		if(isDataLoad)
		{
			/*
			if(CDataManagerNavigator.Instance.g_bIsLoadAssetBundle)
				Load();
			else 
				LoadAsset();
			*/
			LoadAsset();
		}
	}

	public static CDT_StageData_Manager Instance
	{
		get
		{
			return instance;
		}
	}
	public int GetInfoSize() { return m_Table.Count; }

	public string GetXMLPath() { return m_xmlPath; }

	public DT_StageData_Info GetInfo(int Id)
	{ 
		if(m_Table.ContainsKey(Id.ToString()))
		{
			return m_Table[Id.ToString()]; 
		}

		return null; 
	}

	public List<DT_StageData_Info> GetInfoList()
	{
		return m_List;
	}

	public DT_StageData_Info GetInfo(string Id)
	{
		if (m_Table.ContainsKey(Id))
		{
			return m_Table[Id];
		}

		return null;
	}

	public DT_StageData_Info GetInfoByIndex(int Index)
	{
		if (m_Table.Count > Index)
		{
			return m_Table.ElementAt(Index).Value; 
	 	}

		return null; 
	 }

	public void LoadAsset()
	{ 
		DataSet ds = new DataSet();

		UnityEngine.TextAsset textAsset = AssetManager.Instance.GetObject("xmlData/DT_StageData") as UnityEngine.TextAsset;
		System.IO.StringReader xmlSR = new System.IO.StringReader(textAsset.text); 

		ds.ReadXml(xmlSR, XmlReadMode.Auto); 

		if (0 == ds.Tables.Count)
			return; 

		Load(ds.Tables[0]); 
	}

	public void Load()
	{ 
		DataSet ds = new DataSet(); 

		ds.EnforceConstraints = false; 
		m_xmlPath = CDataManagerNavigator.Instance.g_szRootPath + "/Resources/xmlData/DT_StageData.xml "; 

		if (CDataManagerNavigator.Instance.g_bIsEncoding)
		{ 
			System.IO.FileStream fsReadXml = new System.IO.FileStream(m_xmlPath, System.IO.FileMode.Open, System.IO.FileAccess.Read); 

			byte[] Key = new byte[32]; 
			byte[] IV = new byte[16]; 
			byte[] FileKey = System.Text.Encoding.UTF8.GetBytes(System.IO.Path.GetFileNameWithoutExtension(m_xmlPath).ToCharArray()); 
			Array.Clear(Key, 0, 32); 
			Array.Copy(FileKey, Key, (FileKey.Length > 32) ? 32 : FileKey.Length); 
			Array.Copy(Key, IV, 16); 

			RijndaelManaged aes = new RijndaelManaged(); 
			ICryptoTransform decryptoTransform = aes.CreateDecryptor(Key, IV); 
			CryptoStream cryptoStream = new CryptoStream(fsReadXml, decryptoTransform, CryptoStreamMode.Read); 

			ds.ReadXml(cryptoStream); 

			fsReadXml.Close(); 
		}
		else
		{ 
			ds.ReadXml(m_xmlPath); 
		}

		ds.EnforceConstraints = true; 

		if (0 == ds.Tables.Count)
			return; 

		Load(ds.Tables[0]);
	}

	public void Load(System.IO.Stream s)
	{ 
		DataSet ds = new DataSet(); 

		ds.EnforceConstraints = false; 
		ds.ReadXml(s); 
		ds.EnforceConstraints = true; 

		Load(ds.Tables[0]); 
	 }

	public void Load(System.IO.TextReader tr)
	{ 
		DataSet ds = new DataSet(); 

		ds.EnforceConstraints = false; 
		ds.ReadXml(tr); 
		ds.EnforceConstraints = true; 

		Load(ds.Tables[0]); 
	 }

	public void Load(System.Xml.XmlReader rd)
	{ 
		DataSet ds = new DataSet(); 

		ds.EnforceConstraints = false; 
		ds.ReadXml(rd); 
		ds.EnforceConstraints = true; 

		Load(ds.Tables[0]); 
	}

	public void Load(DataTable dt)
	{ 
		m_Table.Clear(); 
		foreach(DataRow row in dt.Rows)
		{ 
			DT_StageData_Info info = new DT_StageData_Info
			{ 
				ID = Convert.ToInt32(row[0].ToString())
				,Stage = Convert.ToInt32(row[1].ToString())
				,Spot1 = Convert.ToInt32(row[2].ToString())
				,Spot2 = Convert.ToInt32(row[3].ToString())
				,Spot3 = Convert.ToInt32(row[4].ToString())
				,Spot4 = Convert.ToInt32(row[5].ToString())
				,Spot5 = Convert.ToInt32(row[6].ToString())
				,Spot6 = Convert.ToInt32(row[7].ToString())
				,Spot7 = Convert.ToInt32(row[8].ToString())
				,Spot8 = Convert.ToInt32(row[9].ToString())
				,Spot9 = Convert.ToInt32(row[10].ToString())
				,Spot10 = Convert.ToInt32(row[11].ToString())
				,Spot11 = Convert.ToInt32(row[12].ToString())
				,Spot12 = Convert.ToInt32(row[13].ToString())
				,Spot13 = Convert.ToInt32(row[14].ToString())
				,Spot14 = Convert.ToInt32(row[15].ToString())
				,Spot15 = Convert.ToInt32(row[16].ToString())
			}; 

			m_Table.Add(row[0].ToString(), info);
			m_List.Add(info); 
		}
	}
}
