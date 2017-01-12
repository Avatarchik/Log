using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;

public class DT_ShipData_Info
{
	/// <summary> 전함 ID </summary>
	public int	ID;
	/// <summary> 전함 이름 </summary>
	public int	Name;
	/// <summary> 전함 설명 </summary>
	public int	Description;
	/// <summary> 함선 레어도(0~6까지 기입) 0 : 일반(커먼/흰색), 1 : 고급(언커먼/녹색), 2 : 희귀(레어/파랑색), 3 : 영웅(에픽/보라색), 4 : 전설(레전드/주황색) </summary>
	public int	Grade;
	/// <summary> 프리팹 및 아이콘 이름 </summary>
	public string	ResourceName;
	/// <summary> 그룹 지정시 참고할 함선 아이디 </summary>
	public int	Reference;
	/// <summary> 최소 공격값 </summary>
	public int	MinAttack;
	/// <summary> 최대 공격값 </summary>
	public int	MaxAttack;
	/// <summary> 한번 공격당 갖는 쿨타임 </summary>
	public int	AttackCoolTime;
	/// <summary> 공격시 런처마다 갖는 쿨타임 </summary>
	public float	LaunchDelay;
	/// <summary> 공격 사거리 및 적발견 범위) </summary>
	public int	AttackRange;
	/// <summary> 선회력 </summary>
	public float	TurnForce;
	/// <summary> 최고 속력 </summary>
	public float	MaxVelocity;
	/// <summary> 체력 </summary>
	public int	BodyAmount;
	/// <summary> 보호막 </summary>
	public int	ShieldAmount;
	/// <summary> 소환 개수 </summary>
	public int	UnitCount;
	/// <summary> 보조 능력치(치유력, 보너스 상승 등등) </summary>
	public float	SubAbility;
	/// <summary> 코스트 (등급에 따른 코스트 상승치 1등급 + 20, 2등급 + 40) </summary>
	public int	Cost;
}

public sealed class CDT_ShipData_Manager
{
	static readonly CDT_ShipData_Manager instance =  new CDT_ShipData_Manager();
	private Dictionary<string, DT_ShipData_Info> m_Table = new Dictionary<string, DT_ShipData_Info>();
	private List<DT_ShipData_Info> m_List = new List<DT_ShipData_Info>();
	private string m_xmlPath = "";

	CDT_ShipData_Manager(bool isDataLoad = true)
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

	public static CDT_ShipData_Manager Instance
	{
		get
		{
			return instance;
		}
	}
	public int GetInfoSize() { return m_Table.Count; }

	public string GetXMLPath() { return m_xmlPath; }

	public DT_ShipData_Info GetInfo(int Id)
	{ 
		if(m_Table.ContainsKey(Id.ToString()))
		{
			return m_Table[Id.ToString()]; 
		}

		return null; 
	}

	public List<DT_ShipData_Info> GetInfoList()
	{
		return m_List;
	}

	public DT_ShipData_Info GetInfo(string Id)
	{
		if (m_Table.ContainsKey(Id))
		{
			return m_Table[Id];
		}

		return null;
	}

	public DT_ShipData_Info GetInfoByIndex(int Index)
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

		UnityEngine.TextAsset textAsset = AssetManager.Instance.GetObject("xmlData/DT_ShipData") as UnityEngine.TextAsset;
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
		m_xmlPath = CDataManagerNavigator.Instance.g_szRootPath + "/Resources/xmlData/DT_ShipData.xml "; 

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
			DT_ShipData_Info info = new DT_ShipData_Info
			{ 
				ID = Convert.ToInt32(row[0].ToString())
				,Name = Convert.ToInt32(row[1].ToString())
				,Description = Convert.ToInt32(row[2].ToString())
				,Grade = Convert.ToInt32(row[3].ToString())
				,ResourceName = row[4].ToString()
				,Reference = Convert.ToInt32(row[5].ToString())
				,MinAttack = Convert.ToInt32(row[6].ToString())
				,MaxAttack = Convert.ToInt32(row[7].ToString())
				,AttackCoolTime = Convert.ToInt32(row[8].ToString())
				,LaunchDelay = Convert.ToSingle(row[9].ToString())
				,AttackRange = Convert.ToInt32(row[10].ToString())
				,TurnForce = Convert.ToSingle(row[11].ToString())
				,MaxVelocity = Convert.ToSingle(row[12].ToString())
				,BodyAmount = Convert.ToInt32(row[13].ToString())
				,ShieldAmount = Convert.ToInt32(row[14].ToString())
				,UnitCount = Convert.ToInt32(row[15].ToString())
				,SubAbility = Convert.ToSingle(row[16].ToString())
				,Cost = Convert.ToInt32(row[17].ToString())
			}; 

			m_Table.Add(row[0].ToString(), info);
			m_List.Add(info); 
		}
	}
}
