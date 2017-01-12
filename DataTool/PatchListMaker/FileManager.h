#pragma once
class CFileManager
{
public:
	CFileManager() { Ctor(); }
	~CFileManager() { Dtor(); }

private:
	void Ctor(void);
	void Dtor(void);

public:
	_Check_return_			bool Search(_In_ CString inPath, _In_ CString inFileExt, _Out_ std::vector<sPATCHFILE_INFO> &outFileList);
	_Check_return_			bool WritePatchList(_In_ std::vector<sPATCHFILE_INFO> &inFileList, _In_ std::wstring inCurrentPath);
	_Must_inspect_result_	void Close(void);
};

