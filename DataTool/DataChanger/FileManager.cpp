#include "stdafx.h"
#include "FileManager.h"
#include < fstream >

void FileManager::Ctor(void)
{

}

void FileManager::Dtor(void)
{

}

_Check_return_	 bool FileManager::MakeCS(_In_ std::wstring inCurrentPath, _In_ std::wstring inFileName, _In_ std::vector<sTABLE_DEFINITION> inTableEntry)
{
	CString strReadLine;

	std::wstring szXMLSavePath = inCurrentPath + TEXT("\\Unity");
	if (GetFileAttributes(szXMLSavePath.c_str()) == -1)
		::CreateDirectory(szXMLSavePath.c_str(), NULL);

	std::wstring szFilePath = szXMLSavePath + TEXT("\\") + inFileName + TEXT(".cs");

	FILE *file = _wfopen(szFilePath.c_str(), L"wb");
	if (file == NULL)
	{
		return false;
	}

	fputwc(0xFEFF, file);
	fwprintf(file, TEXT("using System;\n"));
	fwprintf(file, TEXT("using System.Data;\n"));
	fwprintf(file, TEXT("using System.Linq;\n"));
	fwprintf(file, TEXT("using System.Collections.Generic;\n"));
	fwprintf(file, TEXT("using System.Security.Cryptography;\n\n"));
	std::wstring szStructClassDefined = TEXT("public class ") + inFileName + TEXT("_Info\n{\n");
	fwprintf(file, szStructClassDefined.c_str());
	std::wstring szMemberDefined;
	std::wstring szMemberDescription;
	for each (auto itor in inTableEntry)
	{
		if (itor.dataDependancy == SERVER || itor.dataDependancy == COMMENT)
			continue;

		szMemberDescription = TEXT("\t/// <summary> ") + itor.dataExplain + TEXT(" </summary>\n");
		fwprintf(file, szMemberDescription.c_str());
		szMemberDefined = TEXT("\tpublic ") + itor.dataType + TEXT("\t") + itor.dataName + TEXT(";\n");
		fwprintf(file, szMemberDefined.c_str());
	}
	fwprintf(file, TEXT("}\n\n"));
	std::wstring szClassName = TEXT("C") + inFileName + TEXT("_Manager");
	std::wstring szContents = TEXT("public sealed class ") + szClassName + TEXT("\n{\n");
	fwprintf(file, szContents.c_str());
	szContents = TEXT("\tstatic readonly ") + szClassName + TEXT(" instance =  new ") + szClassName + TEXT("();\n");
	fwprintf(file, szContents.c_str());
	szContents = TEXT("\tprivate Dictionary<string, ") + inFileName + TEXT("_Info> m_Table = new Dictionary<string, ") + inFileName + TEXT("_Info>();\n");
	fwprintf(file, szContents.c_str());
	szContents = TEXT("\tprivate List<") + inFileName + TEXT("_Info> m_List = new List<") + inFileName + TEXT("_Info>();\n");
	fwprintf(file, szContents.c_str());
	fwprintf(file, TEXT("\tprivate string m_xmlPath = \"\";\n\n"));
	szContents = TEXT("\t") + szClassName + TEXT("(bool isDataLoad = true)\n\t{\n");
	fwprintf(file, szContents.c_str());
	fwprintf(file, TEXT("\t\tif(isDataLoad)\n"));
	fwprintf(file, TEXT("\t\t{\n\t\t\t/*\n\t\t\tif(CDataManagerNavigator.Instance.g_bIsLoadAssetBundle)\n\t\t\t\tLoad();\n\t\t\telse \n\t\t\t\tLoadAsset();\n\t\t\t*/\n\t\t\tLoadAsset();\n\t\t}\n\t}\n\n"));
	szContents = TEXT("\tpublic static ") + szClassName + TEXT(" Instance\n\t{\n\t\tget\n\t\t{\n\t\t\treturn instance;\n\t\t}\n\t}\n");
	fwprintf(file, szContents.c_str());
	fwprintf(file, TEXT("\tpublic int GetInfoSize() { return m_Table.Count; }\n\n\tpublic string GetXMLPath() { return m_xmlPath; }\n\n"));
	szContents = TEXT("\tpublic ") + inFileName + TEXT("_Info GetInfo(int Id)\n\t{ \n\t\tif(m_Table.ContainsKey(Id.ToString()))\n\t\t{\n\t\t\treturn m_Table[Id.ToString()]; \n\t\t}\n\n\t\treturn null; \n\t}\n\n");
	fwprintf(file, szContents.c_str());
	szContents = TEXT("\tpublic List<") + inFileName + TEXT("_Info> GetInfoList()\n\t{\n\t\treturn m_List;\n\t}\n\n");
	fwprintf(file, szContents.c_str());
	szContents = TEXT("\tpublic ") + inFileName + TEXT("_Info GetInfo(string Id)\n\t{\n\t\tif (m_Table.ContainsKey(Id))\n\t\t{\n\t\t\treturn m_Table[Id];\n\t\t}\n\n\t\treturn null;\n\t}\n\n");
	fwprintf(file, szContents.c_str());
	szContents = TEXT("\tpublic ") + inFileName + TEXT("_Info GetInfoByIndex(int Index)\n\t{\n\t\tif (m_Table.Count > Index)\n\t\t{\n\t\t\treturn m_Table.ElementAt(Index).Value; \n\t \t}\n\n\t\treturn null; \n\t }\n\n");
	fwprintf(file, szContents.c_str());

	fwprintf(file, TEXT("\tpublic void LoadAsset()\n\t{ \n\t\tDataSet ds = new DataSet();\n\n"));
	szContents = TEXT("\t\tUnityEngine.TextAsset textAsset = AssetManager.Instance.GetObject(\"xmlData/") + inFileName + TEXT("\") as UnityEngine.TextAsset;\n");
	fwprintf(file, szContents.c_str());
	fwprintf(file, TEXT("\t\tSystem.IO.StringReader xmlSR = new System.IO.StringReader(textAsset.text); \n\n\t\tds.ReadXml(xmlSR, XmlReadMode.Auto); \n\n\t\tif (0 == ds.Tables.Count)\n\t\t\treturn; \n\n\t\tLoad(ds.Tables[0]); \n\t}\n\n"));
	szContents = TEXT("\tpublic void Load()\n\t{ \n\t\tDataSet ds = new DataSet(); \n\n\t\tds.EnforceConstraints = false; \n\t\tm_xmlPath = CDataManagerNavigator.Instance.g_szRootPath + \"/Resources/xmlData/") + inFileName +TEXT(".xml \"; \n\n");
	fwprintf(file, szContents.c_str());
	fwprintf(file, TEXT("\t\tif (CDataManagerNavigator.Instance.g_bIsEncoding)\n\t\t{ \n\t\t\tSystem.IO.FileStream fsReadXml = new System.IO.FileStream(m_xmlPath, System.IO.FileMode.Open, System.IO.FileAccess.Read); \n\n"));
	fwprintf(file, TEXT("\t\t\tbyte[] Key = new byte[32]; \n\t\t\tbyte[] IV = new byte[16]; \n\t\t\tbyte[] FileKey = System.Text.Encoding.UTF8.GetBytes(System.IO.Path.GetFileNameWithoutExtension(m_xmlPath).ToCharArray()); \n"));
	fwprintf(file, TEXT("\t\t\tArray.Clear(Key, 0, 32); \n\t\t\tArray.Copy(FileKey, Key, (FileKey.Length > 32) ? 32 : FileKey.Length); \n\t\t\tArray.Copy(Key, IV, 16); \n\n\t\t\tRijndaelManaged aes = new RijndaelManaged(); \n"));
	fwprintf(file, TEXT("\t\t\tICryptoTransform decryptoTransform = aes.CreateDecryptor(Key, IV); \n\t\t\tCryptoStream cryptoStream = new CryptoStream(fsReadXml, decryptoTransform, CryptoStreamMode.Read); \n\n\t\t\tds.ReadXml(cryptoStream); \n\n"));
	fwprintf(file, TEXT("\t\t\tfsReadXml.Close(); \n\t\t}\n\t\telse\n\t\t{ \n\t\t\tds.ReadXml(m_xmlPath); \n\t\t}\n\n\t\tds.EnforceConstraints = true; \n\n\t\tif (0 == ds.Tables.Count)\n\t\t\treturn; \n\n\t\tLoad(ds.Tables[0]);\n\t}\n\n"));
	fwprintf(file, TEXT("\tpublic void Load(System.IO.Stream s)\n\t{ \n\t\tDataSet ds = new DataSet(); \n\n\t\tds.EnforceConstraints = false; \n\t\tds.ReadXml(s); \n\t\tds.EnforceConstraints = true; \n\n"));
	fwprintf(file, TEXT("\t\tLoad(ds.Tables[0]); \n\t }\n\n\tpublic void Load(System.IO.TextReader tr)\n\t{ \n\t\tDataSet ds = new DataSet(); \n\n\t\tds.EnforceConstraints = false; \n\t\tds.ReadXml(tr); \n\t\tds.EnforceConstraints = true; \n\n"));
	fwprintf(file, TEXT("\t\tLoad(ds.Tables[0]); \n\t }\n\n\tpublic void Load(System.Xml.XmlReader rd)\n\t{ \n\t\tDataSet ds = new DataSet(); \n\n\t\tds.EnforceConstraints = false; \n\t\tds.ReadXml(rd); \n\t\tds.EnforceConstraints = true; \n\n\t\tLoad(ds.Tables[0]); \n\t}\n\n"));

	szContents = TEXT("\tpublic void Load(DataTable dt)\n\t{ \n\t\tm_Table.Clear(); \n\t\tforeach(DataRow row in dt.Rows)\n\t\t{ \n\t\t\t") + inFileName + TEXT("_Info info = new ") + inFileName + TEXT("_Info\n\t\t\t{ \n");
	fwprintf(file, szContents.c_str());

	fwprintf(file, TEXT("\t\t\t\t"));
	int i = 0;
	for each (auto itor in inTableEntry)
	{
		//szMemberDescription = TEXT("\t/// <summary> ") + itor.dataExplain + TEXT(" </summary>\n");
		//fwprintf(file, szMemberDescription.c_str());
		szContents = _T("");
		if (itor.dataDependancy == SERVER || itor.dataDependancy == COMMENT)
			continue;

		if (i != 0 && (itor.dataType.compare(TEXT("")) != 0))
		{
			fwprintf(file, TEXT("\t\t\t\t,"));
		}
		if (itor.dataType.compare(TEXT("int")) == 0)
		{
			szContents = itor.dataName + TEXT(" = Convert.ToInt32(row[") + std::to_wstring(i) + TEXT("].ToString())\n");
		}
		else if (itor.dataType.compare(TEXT("byte")) == 0)
		{
			szContents = itor.dataName + TEXT(" = Convert.ToByte(row[") + std::to_wstring(i) + TEXT("].ToString())\n");
		}
		else if (itor.dataType.compare(TEXT("short")) == 0)
		{
			szContents = itor.dataName + TEXT(" = Convert.ToInt16(row[") + std::to_wstring(i) + TEXT("].ToString())\n");
		}
		else if (itor.dataType.compare(TEXT("long")) == 0)
		{
			szContents = itor.dataName + TEXT(" = Convert.ToInt64(row[") + std::to_wstring(i) + TEXT("].ToString())\n");
		}
		else if (itor.dataType.compare(TEXT("string")) == 0)
		{
			szContents = itor.dataName + TEXT(" = row[") + std::to_wstring(i) + TEXT("].ToString()\n");
		}
		else if (itor.dataType.compare(TEXT("float")) == 0)
		{
			szContents = itor.dataName + TEXT(" = Convert.ToSingle(row[") + std::to_wstring(i) + TEXT("].ToString())\n");
		}
		fwprintf(file, szContents.c_str());
		i++;
	}
	fwprintf(file, TEXT("\t\t\t}; \n\n\t\t\tm_Table.Add(row[0].ToString(), info);\n"));
	fwprintf(file, TEXT("\t\t\tm_List.Add(info); \n\t\t}\n\t}\n}\n"));
	fclose(file);
	//file.Close();
	return true;
}

_Check_return_	bool FileManager::MakeXML(_In_ std::wstring inCurrentPath, _In_ std::wstring inFileName, _In_ std::wstring inXML)
{
//	CStdioFile file;
	std::wstring szSavePath = inCurrentPath + TEXT("\\xmlData");
	if ( GetFileAttributes(szSavePath.c_str()) == -1)
		::CreateDirectory(szSavePath.c_str(), NULL);

	std::wstring szFilePath = szSavePath + TEXT("\\") + inFileName + TEXT(".xml");
	FILE *file = _wfopen(szFilePath.c_str(), L"wb");
	if (file == NULL)
	{
		return false;
	}
	fputwc(0xFEFF, file);
	fwprintf(file, inXML.c_str());
	fclose(file);

	return true;
}

_Check_return_	bool FileManager::Search(_In_ CString inPath, _In_ CString inFileExt, _Out_ std::vector<std::wstring> &filesPath)
{
	CFileFind file;
	CString szSearchPath = inPath + "\\*.*";
	BOOL bFind = file.FindFile(szSearchPath);

// 	if (bFind == false)
// 		::CreateDirectory(inPath, NULL);

	CString strFolderItem, strFileExt;
 	while (bFind)
	{
		bFind = file.FindNextFile();
// 		if (file.IsDirectory() && !file.IsDots())
// 		{
// 			strFolderItem = file.GetFilePath();
// 			Search(strFolderItem.GetBuffer());
// 		}
		strFolderItem = file.GetFilePath();
		strFileExt = strFolderItem.Mid(strFolderItem.ReverseFind('.'));
		if (!file.IsDots())
		{
			strFileExt.MakeUpper();
			inFileExt.MakeUpper();
			if (file.IsDirectory()) continue;
			if ( strFileExt == inFileExt )
			{
				if (strFolderItem.Find('~', 0) == -1 && strFolderItem.Find(TEXT("KM_")) == -1 )
					filesPath.push_back(strFolderItem.GetBuffer());
			}
		}
	}
	return true;
}

_Check_return_	 bool FileManager::MakeNavigator(_In_ std::wstring inCurrentPath, _In_ std::vector<std::wstring> inTableFiles)
{
	std::wstring szXMLSavePath = inCurrentPath + TEXT("\\Unity");
	if (GetFileAttributes(szXMLSavePath.c_str()) == -1)
		::CreateDirectory(szXMLSavePath.c_str(), NULL);

	std::wstring szFilePath = szXMLSavePath + TEXT("\\CDataManagerNavigator.cs");


	FILE *file = _wfopen(szFilePath.c_str(), L"wb");
	if (file == NULL)
	{
		return false;
	}

	fputwc(0xFEFF, file);

	fwprintf(file, TEXT("using UnityEngine;\n"));
	fwprintf(file, TEXT("using System;\n"));
	fwprintf(file, TEXT("using System.Collections.Generic;\n"));
	fwprintf(file, TEXT("\n"));
	fwprintf(file, TEXT("public sealed class CDataManagerNavigator\n"));
	fwprintf(file, TEXT("{\n"));
	fwprintf(file, TEXT("\tstatic readonly CDataManagerNavigator instance = new CDataManagerNavigator();\n"));
	fwprintf(file, TEXT("\tprivate Dictionary<System.Type, System.Object> Navigator = new Dictionary<System.Type, System.Object>();\n"));
	fwprintf(file, TEXT("\n"));
	fwprintf(file, TEXT("\tpublic string g_szRootPath{ get; set; }\n"));
	fwprintf(file, TEXT("\n"));
	fwprintf(file, TEXT("\tpublic bool g_bIsEncoding{ get; set; }\n"));
	fwprintf(file, TEXT("\n"));
	fwprintf(file, TEXT("\tpublic bool g_bIsLoadAssetBundle{ get; set; }\n"));
	fwprintf(file, TEXT("\n"));
	fwprintf(file, TEXT("\tCDataManagerNavigator()\n"));
	fwprintf(file, TEXT("\t{\n"));
	fwprintf(file, TEXT("\t\tg_szRootPath = \".\";\n"));
	fwprintf(file, TEXT("\t\tif (Application.platform == RuntimePlatform.Android ||\n"));
	fwprintf(file, TEXT("\t\t\tApplication.platform == RuntimePlatform.IPhonePlayer)\n"));
	fwprintf(file, TEXT("\t\t{\n"));
	fwprintf(file, TEXT("\t\t\tg_szRootPath = Application.persistentDataPath;\n"));
	fwprintf(file, TEXT("\t\t}\n"));
	fwprintf(file, TEXT("\n"));
	fwprintf(file, TEXT("\t\tg_bIsEncoding = false;\n"));
	fwprintf(file, TEXT("\n"));
	fwprintf(file, TEXT("\t\tg_bIsLoadAssetBundle = false;\n"));
	fwprintf(file, TEXT("\n"));
	fwprintf(file, TEXT("\t}\n"));
	fwprintf(file, TEXT("\n"));
	fwprintf(file, TEXT("\tpublic static CDataManagerNavigator Instance\n"));
	fwprintf(file, TEXT("\t{\n"));
	fwprintf(file, TEXT("\t\tget\n"));
	fwprintf(file, TEXT("\t\t{\n"));
	fwprintf(file, TEXT("\t\t\treturn instance;\n"));
	fwprintf(file, TEXT("\t\t}\n"));
	fwprintf(file, TEXT("\t}\n"));
	fwprintf(file, TEXT("\n"));
	fwprintf(file, TEXT("\tpublic void Load()\n"));
	fwprintf(file, TEXT("\t{\n"));
	fwprintf(file, TEXT("\t\tif (0 != Navigator.Count) return;\n"));
	fwprintf(file, TEXT("\n"));

	std::wstring szDescription;
	for each (auto itor in inTableFiles)
	{
		size_t iFileNameCount = itor.rfind('.') - itor.rfind('\\') - 1;
		std::wstring szFileName = itor.substr(itor.rfind('\\') + 1, iFileNameCount);

		szDescription = TEXT("");
		szDescription = TEXT("\t\tNavigator.Add(typeof(C") + szFileName + TEXT("_Manager), C") + szFileName + TEXT("_Manager.Instance);\n");
		fwprintf(file, szDescription.c_str());
	}

	fwprintf(file, TEXT("\t}\n"));
	fwprintf(file, TEXT("\n"));

	fwprintf(file, TEXT("\tpublic T GetManager<T>()\n"));
	fwprintf(file, TEXT("\t{\n"));
	fwprintf(file, TEXT("\t\tif (0 == Navigator.Count)\n"));
	fwprintf(file, TEXT("\t\t\tLoad();\n"));
	fwprintf(file, TEXT("\n"));
	fwprintf(file, TEXT("\t\tif (false == Navigator.ContainsKey(typeof(T)))\n"));
	fwprintf(file, TEXT("\t\t\treturn default(T);\n"));
	fwprintf(file, TEXT("\n"));
	fwprintf(file, TEXT("\t\treturn (T)Navigator[typeof(T)];\n"));
	fwprintf(file, TEXT("\t}\n"));
	fwprintf(file, TEXT("\n"));
	fwprintf(file, TEXT("}\n"));

	fclose(file);
	return false;
} 

_Must_inspect_result_	void FileManager::Close(void)
{

}
