#pragma once

namespace XLSLOADER
{
	enum ExcelLoaderErrorCode
	{
		SUCCESS = 0,
		FAILED,
		OPEN_FAILED,
		NOTFOUND_DEFINE,
		INVALIED_COLNAME,
		INVALIED_VALUE,
	};

	struct ReturnError
	{
		ExcelLoaderErrorCode errorCode;
		int		iErrorRow;
		int		iErrorCol;

		ReturnError(ExcelLoaderErrorCode inErrorCode, int inRow, int inCol)
		{
			errorCode = inErrorCode;
			iErrorRow = inRow;
			iErrorCol = inCol;
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
	_Check_return_			XLSLOADER::ReturnError Open(_In_ std::wstring inFilePath, _Out_ std::vector<sTABLE_DEFINITION> &outDefEntry, _Out_ std::wstring &outXML);
	_Must_inspect_result_	void Close(void);
};

