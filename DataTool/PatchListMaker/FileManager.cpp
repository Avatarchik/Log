#include "stdafx.h"
#include "FileManager.h"
#include < fstream >
#include "PatchList.h"

void CFileManager::Ctor(void)
{

}

void CFileManager::Dtor(void)
{

}

_Check_return_	bool CFileManager::Search(_In_ CString inPath, _In_ CString inFileExt, _Out_ std::vector<sPATCHFILE_INFO> &outFileList)
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
				if (strFolderItem.Find('~', 0) == -1 && strFolderItem.Find(TEXT("KM_")) == -1)
				{
					sPATCHFILE_INFO patchFileInfo;
					patchFileInfo.fileName = file.GetFileName();
					FILETIME lastWriteTime;
					file.GetLastWriteTime(&lastWriteTime);
					patchFileInfo.fileModifyTimeHigh = lastWriteTime.dwHighDateTime;
					patchFileInfo.fileModifyTimeLow = lastWriteTime.dwLowDateTime;
					patchFileInfo.fileSize = static_cast<UINT32>(file.GetLength());
					outFileList.push_back(patchFileInfo);
				}
			}
		}
	}
	return true;
}

_Check_return_			bool CFileManager::WritePatchList(_In_ std::vector<sPATCHFILE_INFO> &inFileList, _In_ std::wstring inCurrentPath)
{
	std::wstring szSavePath = inCurrentPath + TEXT("\\PatchList.xml");
	FILE *file = NULL;
	_wfopen_s(&file, szSavePath.c_str(), L"wt");
	if (file == NULL)
	{
		return false;
	}
//	fputwc(0xFEFF, file);
	std::wstring xmlData;
	xmlData.append(TEXT("<Data>\n"));
	for each(auto itor in inFileList)
	{
		//std::wstring szLine;
		xmlData.append(L"\t<Data FileName=\"" + itor.fileName + L"\" Version=\"" + std::to_wstring(itor.patchVersion) + L"\" Scene=\"" + std::to_wstring(itor.loadingScenePosition)
			+ L"\" Size=\"" + std::to_wstring(itor.fileSize) + L"\" PatchName=\"" + std::to_wstring(itor.patchNameIndex) + L"\" LoadingName=\"" + std::to_wstring(itor.loadingNameIndex) + L"\"/>\n");
	}
	xmlData.append(TEXT("</Data>"));
	fwprintf(file, xmlData.c_str());
	fclose(file);

	szSavePath = inCurrentPath + TEXT("\\PatchFileHistory.info");
	_wfopen_s(&file, szSavePath.c_str(), L"wt");
	if (file == NULL)
	{
		return false;
	}
	fputwc(0xFEFF, file);
	xmlData = L"";

	xmlData.append(TEXT("<Data>\n"));
	for each(auto itor in inFileList)
	{
		//std::wstring szLine;
		xmlData.append(L"\t<Data FileName=\"" + itor.fileName + L"\" ModifyDateHigh=\"" + std::to_wstring(itor.fileModifyTimeHigh)
			+ L"\" ModifyDateLow=\"" + std::to_wstring(itor.fileModifyTimeLow) + L"\" />\n");
	}
	xmlData.append(TEXT("</Data>"));
	fwprintf(file, xmlData.c_str());
	fclose(file);

	return true;
}

_Must_inspect_result_	void CFileManager::Close(void)
{

}
