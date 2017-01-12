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
	_Check_return_			bool MakePIDL(_In_ std::wstring inCurrentPath, _In_ std::wstring inServerExportPath, _In_ std::wstring inFileName, _In_ std::wstring inPIDL, _In_ bool inOnlyServer = false);
	_Check_return_			bool MakeUnityScript(_In_ std::wstring inCurrPath, _In_ std::wstring inDataPath, _In_ std::wstring inFileName, _In_ std::wstring inClientPacketManager);
	_Check_return_			bool MakePacketDef(_In_ std::wstring inCurrPath, _In_ std::wstring inDataPath, _In_ std::wstring inFileName, _In_ std::wstring inPacketDef);
	_Check_return_			bool Search(_In_ CString inPath, _In_ CString inFileExt, _Out_ std::vector<std::wstring> &filesPath);
	_Must_inspect_result_	void Close(void) { NULL; }
};

