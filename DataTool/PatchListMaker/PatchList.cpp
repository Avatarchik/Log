#include "stdafx.h"
#include "PatchList.h"
#include <atlconv.h>
#include <algorithm>

void CPatchListMaker::Ctor()
{

}

void CPatchListMaker::Dtor()
{

}

_Check_return_ bool CPatchListMaker::CheckXMLList(_In_ std::vector<sPATCHFILE_INFO>& inSearchFileList)
{
	UINT16 iAdded = false;
	bool bAdded = false;
	for each(auto itor_file in inSearchFileList)
	{
		bAdded = false;
		sPATCHFILE_INFO * pXMLFileInfo = NULL;
		for (std::vector<sPATCHFILE_INFO>::iterator itor = mPatchFileList.begin(); itor != mPatchFileList.end(); itor++)
		{
			sPATCHFILE_INFO* pXMLFileInfo = &(*itor);
			if (itor_file.fileName.compare(pXMLFileInfo->fileName) != 0)
				continue;
			if ((itor_file.fileModifyTimeHigh == pXMLFileInfo->fileModifyTimeHigh) && (itor_file.fileModifyTimeLow == pXMLFileInfo->fileModifyTimeLow))
				continue;

			pXMLFileInfo->fileModifyTimeHigh = itor_file.fileModifyTimeHigh;
			pXMLFileInfo->fileModifyTimeLow = itor_file.fileModifyTimeLow;
			pXMLFileInfo->fileSize = itor_file.fileSize / 1024;
			pXMLFileInfo->patchVersion += 1;
			pXMLFileInfo->XMLExists = FILEADDED_CHANGE;
			bAdded = true;
		}
		if (bAdded == false && pXMLFileInfo != NULL)
		{
			sPATCHFILE_INFO patchFileInfo;
			patchFileInfo.fileModifyTimeHigh = pXMLFileInfo->fileModifyTimeHigh;
			patchFileInfo.fileModifyTimeLow = pXMLFileInfo->fileModifyTimeLow;
			patchFileInfo.fileName = pXMLFileInfo->fileName;
			patchFileInfo.fileSize = pXMLFileInfo->fileSize / 1024;
			patchFileInfo.loadingScenePosition = 0;
			patchFileInfo.patchVersion = 1;
			patchFileInfo.XMLExists = FILEADDED_ADD;
			mPatchFileList.push_back(patchFileInfo);
		}
	}
	return true;
}


_Check_return_ bool CPatchListMaker::ReadPatchListXML(_In_ std::wstring inCurrentPath)
{
	tinyxml2::XMLDocument mXMLDoc;
	std::string szFilePath(inCurrentPath.begin(), inCurrentPath.end());

	std::string szPatchListXMLPath = szFilePath + "\\PatchList.xml";
	if (mXMLDoc.LoadFile(szPatchListXMLPath.c_str()) != tinyxml2::XML_SUCCESS)
		return false;

	const char* csFileName = nullptr;
	std::string szFileName = "";
	tinyxml2::XMLNode* pNode = mXMLDoc.FirstChildElement("Data")->FirstChildElement("Data");
	while (pNode != NULL)
	{
		szFileName = "";
		sPATCHFILE_INFO patchFileList;
		patchFileList.patchVersion = pNode->ToElement()->IntAttribute("Version");
		patchFileList.fileSize = pNode->ToElement()->IntAttribute("Size");
		patchFileList.loadingScenePosition = pNode->ToElement()->IntAttribute("Scene");
		csFileName = pNode->ToElement()->Attribute("FileName");
		patchFileList.patchNameIndex = pNode->ToElement()->IntAttribute("PatchName");
		patchFileList.loadingNameIndex = pNode->ToElement()->IntAttribute("LoadingName");

		if (csFileName != nullptr)
		{
			szFileName = csFileName;
		}
		patchFileList.fileName.assign(szFileName.begin(), szFileName.end());
		pNode = pNode->NextSibling();
		mPatchFileList.push_back(patchFileList);
	}

	return true;
}

_Check_return_ bool CPatchListMaker::ReadPatchFileInfoXML(_In_ std::wstring inCurrentPath)
{
	tinyxml2::XMLDocument mXMLDoc;
	std::string szFilePath(inCurrentPath.begin(), inCurrentPath.end());

	std::string szPatchListXMLPath = szFilePath + "\\PatchFileHistory.info";
	if (mXMLDoc.LoadFile(szPatchListXMLPath.c_str()) != tinyxml2::XML_SUCCESS)
		return false;

	const char* csFileName = nullptr;
	std::string szFileName = "";
	std::wstring wszFileName = L"";
	tinyxml2::XMLNode* pNode = mXMLDoc.FirstChildElement("Data")->FirstChildElement("Data");
	while (pNode != NULL)
	{
		szFileName = "";
		csFileName = pNode->ToElement()->Attribute("FileName");
		if (csFileName != nullptr)
		{
			szFileName = csFileName;
		}
		wszFileName.assign(szFileName.begin(), szFileName.end());
		for(std::vector<sPATCHFILE_INFO>::iterator itor = mPatchFileList.begin() ; itor != mPatchFileList.end(); itor++)
		{
			sPATCHFILE_INFO* pPatchFileInfo = &(*itor);
			if (wszFileName.compare(pPatchFileInfo->fileName) != 0)
				continue;
			pPatchFileInfo->fileModifyTimeHigh = pNode->ToElement()->IntAttribute("ModifyDateHigh");
			pPatchFileInfo->fileModifyTimeLow = pNode->ToElement()->IntAttribute("ModifyDateLow");
		}
		pNode = pNode->NextSibling();
	}

	return true;
}
