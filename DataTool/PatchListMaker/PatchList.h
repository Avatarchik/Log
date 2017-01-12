#pragma once

#include <tinyxml2.h>

class CPatchListMaker
{
public:
	CPatchListMaker() { Ctor(); }
	~CPatchListMaker() { Dtor(); }

private:
	void Ctor(void);
	void Dtor(void);

public:
	_Check_return_ bool ReadPatchListXML(_In_ std::wstring inCurrentPath);
	_Check_return_ bool ReadPatchFileInfoXML(_In_ std::wstring inCurrentPath);
	_Check_return_ bool CheckXMLList(_In_ std::vector<sPATCHFILE_INFO>& inSearchFileList);
	_Check_return_ std::vector<sPATCHFILE_INFO> GetPatchFileList(void) { return mPatchFileList; }
		
private:
	std::vector<sPATCHFILE_INFO> mPatchFileList;
};

