#pragma once
class CFileManager
{
public:
	CFileManager();
	~CFileManager();

	_Check_return_ bool Search(_Out_ std::wstring & outSelectedFileNam, _Out_ std::wstring & outFileModifyTime);
	_Check_return_ bool MakeHTML(_In_ std::wstring inBuildNumber, _In_ std::wstring inFileName, _In_ std::wstring inDate);

private:
	void Ctor(void);
	void Dtor(void);
};

