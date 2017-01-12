#include "stdafx.h"
#include "FileManager.h"
#include < fstream >

void FileManager::Ctor(void)
{

}

void FileManager::Dtor(void)
{

}

_Check_return_	bool FileManager::MakePIDL(_In_ std::wstring inCurrentPath, _In_ std::wstring inServerExportPath, _In_ std::wstring inFileName, _In_ std::wstring inPIDL, _In_ bool inOnlyServer)
{
	std::wstring szSavePath = TEXT("");
	
	if (inOnlyServer == true)
	{
		if (inServerExportPath.size() == 0)
			szSavePath = inCurrentPath + TEXT("\\PIDL_SERVER");
		else
			szSavePath = inServerExportPath;
	}
	else
	{
		szSavePath = inCurrentPath + TEXT("\\PIDL");
	}
	
	if ( GetFileAttributes(szSavePath.c_str()) == -1)
		::CreateDirectory(szSavePath.c_str(), NULL);

	std::wstring szFilePath = szSavePath + TEXT("\\") + inFileName + TEXT(".PIDL");

	FILE *file = NULL;
	_wfopen_s(&file, szFilePath.c_str(), L"wb");
	if (file == NULL)
	{
		return false;
	}
	fputwc(0xFEFF, file);
	fwprintf(file, inPIDL.c_str());
	fclose(file);

	if (inOnlyServer == false)
		MakePIDL(inCurrentPath, inServerExportPath, inFileName, inPIDL, true); //클라서버 양쪽에서 모두 쓰면 PIDL_SERVER폴더에도 복하사기 위함입니다.

	return true;
}

_Check_return_ bool FileManager::MakeUnityScript(_In_ std::wstring inCurrPath, _In_ std::wstring inDataPath, _In_ std::wstring inFileName, _In_ std::wstring inUnityScript)
{

	std::wstring szSavePath = TEXT("");

	if (inCurrPath.size() == 0)
	{
		szSavePath = inDataPath + TEXT("\\unity");
	}
	else
	{
		szSavePath = inCurrPath;
	}

	if (GetFileAttributes(szSavePath.c_str()) == -1)
		::CreateDirectory(szSavePath.c_str(), NULL);

	std::wstring szFilePath = szSavePath + TEXT("\\PacketManager_") + inFileName +TEXT(".cs");

	FILE *file = NULL;
	_wfopen_s(&file, szFilePath.c_str(), L"wb");
	if (file == NULL)
	{
		return false;
	}
	fputwc(0xFEFF, file);
	fwprintf(file, inUnityScript.c_str());
	fclose(file);
	return true;
}

_Check_return_ bool FileManager::MakePacketDef(_In_ std::wstring inCurrentPath, _In_ std::wstring inDataPath, _In_ std::wstring inFileName, _In_ std::wstring inPacketDef)
{
	std::wstring szSavePath = TEXT("");
	if (inCurrentPath.size() == 0)
	{
		szSavePath = inDataPath + TEXT("\\unity");
	}
	else
	{
		szSavePath = inCurrentPath;
	}

	if (GetFileAttributes(szSavePath.c_str()) == -1)
		::CreateDirectory(szSavePath.c_str(), NULL);

	std::wstring szFilePath = szSavePath + TEXT("\\PacketDef.cs");
	std::wstring szContents = TEXT("");
	szContents.append(TEXT("public class PacketDef\n{\n"));
	szContents.append(inPacketDef);
	szContents.append(TEXT("}\n"));

	FILE *file = NULL;
	_wfopen_s(&file, szFilePath.c_str(), L"wb");
	if (file == NULL)
	{
		return false;
	}
	fputwc(0xFEFF, file);
	fwprintf(file, szContents.c_str());
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
				if ( strFolderItem.Find('~', 0) == -1)
					filesPath.push_back(strFolderItem.GetBuffer());
			}
		}
	}
	return true;
}
