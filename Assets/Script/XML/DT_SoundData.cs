using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;

public class DT_SoundData_Info
{
	/// <summary> 사운드 ID </summary>
	public int	ID;
	/// <summary> 사운드 명 </summary>
	public string	Name;
	/// <summary> 사운드 설명 </summary>
	public string	Description;
}

public sealed class CDT_SoundData_Manager
{
	static readonly CDT_SoundData_Manager instance =  new CDT_SoundData_Manager();
	private Dictionary<string, DT_SoundData_Info> m_Table = new Dictionary<string, DT_SoundData_Info>();
	private List<DT_SoundData_Info> m_List = new List<DT_SoundData_Info>();
	private string m_xmlPath = "";

	CDT_SoundData_Manager(bool isDataLoad = true)
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

	public static CDT_SoundData_Manager Instance
	{
		get
		{
			return instance;
		}
	}
	public int GetInfoSize() { return m_Table.Count; }

	public string GetXMLPath() { return m_xmlPath; }

	public DT_SoundData_Info GetInfo(int Id)
	{ 
		if(m_Table.ContainsKey(Id.ToString()))
		{
			return m_Table[Id.ToString()]; 
		}

		return null; 
	}

	public List<DT_SoundData_Info> GetInfoList()
	{
		return m_List;
	}

	public DT_SoundData_Info GetInfo(string Id)
	{
		if (m_Table.ContainsKey(Id))
		{
			return m_Table[Id];
		}

		return null;
	}

	public DT_SoundData_Info GetInfoByIndex(int Index)
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

		UnityEngine.TextAsset textAsset = AssetManager.Instance.GetObject("xmlData/DT_SoundData") as UnityEngine.TextAsset;
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
		m_xmlPath = CDataManagerNavigator.Instance.g_szRootPath + "/Resources/xmlData/DT_SoundData.xml "; 

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
			DT_SoundData_Info info = new DT_SoundData_Info
			{ 
				ID = Convert.ToInt32(row[0].ToString())
				,Name = row[1].ToString()
				,Description = row[2].ToString()
			}; 

			m_Table.Add(row[0].ToString(), info);
			m_List.Add(info); 
		}
	}
}
