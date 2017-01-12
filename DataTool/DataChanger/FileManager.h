#pragma once
class FileManager
{
public:
	FileManager() { Ctor(); }
	~FileManager() { Dtor(); }

private:
	void Ctor(void);
	void Dtor(void);

public:
	_Check_return_			bool MakeCS(_In_ std::wstring inCurrentPath, _In_ std::wstring inFileName, _In_ std::vector<sTABLE_DEFINITION> inTableEntry);
	_Check_return_			bool MakeXML(_In_ std::wstring inCurrentPath, _In_ std::wstring inFileName, _In_ std::wstring inXML);
	_Check_return_			bool Search(_In_ CString inPath, _In_ CString inFileExt, _Out_ std::vector<std::wstring> &filesPath);
	_Check_return_			bool MakeNavigator(_In_ std::wstring inCurrentPath, _In_ std::vector<std::wstring> inTableFiles);
	_Must_inspect_result_	void Close(void);
};

