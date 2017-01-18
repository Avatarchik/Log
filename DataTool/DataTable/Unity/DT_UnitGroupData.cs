using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;

public class DT_UnitGroupData_Info
{
	/// <summary> Stage ID </summary>
	public int	ID;
	/// <summary> 구역 가로 인덱스 </summary>
	public int	Column;
	/// <summary> 구역 세로 인덱스 </summary>
	public int	Row;
	/// <summary> 유닛 아이디 </summary>
	public int	Unit1;
	/// <summary> 유닛 아이디 </summary>
	public int	Unit2;
	/// <summary> 유닛 아이디 </summary>
	public int	Unit3;
	/// <summary> 유닛 아이디 </summary>
	public int	Unit4;
	/// <summary> 유닛 아이디 </summary>
	public int	Unit5;
	/// <summary> 유닛 아이디 </summary>
	public int	Unit6;
	/// <summary> 유닛 아이디 </summary>
	public int	Unit7;
	/// <summary> 유닛 아이디 </summary>
	public int	Unit8;
	/// <summary> 유닛 아이디 </summary>
	public int	Unit9;
	/// <summary> 유닛 아이디 </summary>
	public int	Unit10;
	/// <summary> 유닛 아이디 </summary>
	public int	Unit11;
	/// <summary> 유닛 아이디 </summary>
	public int	Unit12;
	/// <summary> 유닛 아이디 </summary>
	public int	Unit13;
	/// <summary> 유닛 아이디 </summary>
	public int	Unit14;
	/// <summary> 유닛 아이디 </summary>
	public int	Unit15;
}

public sealed class CDT_UnitGroupData_Manager
{
	static readonly CDT_UnitGroupData_Manager instance =  new CDT_UnitGroupData_Manager();
	private Dictionary<string, DT_UnitGroupData_Info> m_Table = new Dictionary<string, DT_UnitGroupData_Info>();
	private List<DT_UnitGroupData_Info> m_List = new List<DT_UnitGroupData_Info>();
	private string m_xmlPath = "";

	CDT_UnitGroupData_Manager(bool isDataLoad = true)
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

	public static CDT_UnitGroupData_Manager Instance
	{
		get
		{
			return instance;
		}
	}
	public int GetInfoSize() { return m_Table.Count; }

	public string GetXMLPath() { return m_xmlPath; }

	public DT_UnitGroupData_Info GetInfo(int Id)
	{ 
		if(m_Table.ContainsKey(Id.ToString()))
		{
			return m_Table[Id.ToString()]; 
		}

		return null; 
	}

	public List<DT_UnitGroupData_Info> GetInfoList()
	{
		return m_List;
	}

	public DT_UnitGroupData_Info GetInfo(string Id)
	{
		if (m_Table.ContainsKey(Id))
		{
			return m_Table[Id];
		}

		return null;
	}

	public DT_UnitGroupData_Info GetInfoByIndex(int Index)
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

		UnityEngine.TextAsset textAsset = AssetManager.Instance.GetObject("xmlData/DT_UnitGroupData") as UnityEngine.TextAsset;
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
		m_xmlPath = CDataManagerNavigator.Instance.g_szRootPath + "/Resources/xmlData/DT_UnitGroupData.xml "; 

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
			DT_UnitGroupData_Info info = new DT_UnitGroupData_Info
			{ 
				ID = Convert.ToInt32(row[0].ToString())
				,Column = Convert.ToInt32(row[1].ToString())
				,Row = Convert.ToInt32(row[2].ToString())
				,Unit1 = Convert.ToInt32(row[3].ToString())
				,Unit2 = Convert.ToInt32(row[4].ToString())
				,Unit3 = Convert.ToInt32(row[5].ToString())
				,Unit4 = Convert.ToInt32(row[6].ToString())
				,Unit5 = Convert.ToInt32(row[7].ToString())
				,Unit6 = Convert.ToInt32(row[8].ToString())
				,Unit7 = Convert.ToInt32(row[9].ToString())
				,Unit8 = Convert.ToInt32(row[10].ToString())
				,Unit9 = Convert.ToInt32(row[11].ToString())
				,Unit10 = Convert.ToInt32(row[12].ToString())
				,Unit11 = Convert.ToInt32(row[13].ToString())
				,Unit12 = Convert.ToInt32(row[14].ToString())
				,Unit13 = Convert.ToInt32(row[15].ToString())
				,Unit14 = Convert.ToInt32(row[16].ToString())
				,Unit15 = Convert.ToInt32(row[17].ToString())
			}; 

			m_Table.Add(row[0].ToString(), info);
			m_List.Add(info); 
		}
	}
}
