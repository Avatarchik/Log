#pragma once

namespace XLSLOADER
{
	enum ExcelLoaderErrorCode
	{
		SUCCESS = 0,
		FAILED,
		OPEN_FAILED,
		NOTFOUND_PACKETNAME,
		NOTFOUND_PACKETNUMBER,
		NOTMATCH_SHEETDEFINE,
		INVALIED_PACKETSTARTNUMBER,
		INVALIED_COLNAME,
		INVALIED_VALUE,
	};

	struct ReturnError
	{
		ExcelLoaderErrorCode errorCode;
		std::wstring sheetName;
		int		errorRow;
		int		errorCol;
		bool	serverOnly;

		ReturnError(ExcelLoaderErrorCode inErrorCode, std::wstring inSheetName, int inRow, int inCol, bool inServerOnly = false)
		{
			errorCode = inErrorCode;
			sheetName = inSheetName;
			errorRow = inRow;
			errorCol = inCol;
			serverOnly = inServerOnly;
		}
	};
}

class ExcelLoader
{
public:
	ExcelLoader() { Ctor(); }
	~ExcelLoader() { Dtor(); }

private:
	void Ctor(void);
	void Dtor(void);

public:
	_Check_return_			 XLSLOADER::ReturnError Open(_In_ std::wstring inFilePath, _Out_ std::wstring &outPIDL, _Out_ std::wstring &outClientManager, _Out_ std::wstring &outPacketDef);
	_Must_inspect_result_	void Close(void);
};

