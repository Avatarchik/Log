#include "stdafx.h"
#include "ExcelLoader.h"
#include <BasicExcel.hpp>
#include "libxl/libxl.h"
#include <locale>
#include <tinyxml.h>

#define LIBXL

using namespace YExcel;
using namespace libxl;

void ExcelLoader::Ctor(void)
{

}

void ExcelLoader::Dtor(void)
{

}

_Check_return_ XLSLOADER::ReturnError ExcelLoader::Open(_In_ std::wstring inFilePath, _Out_ std::vector<sTABLE_DEFINITION> &outDefEntry, _Out_ std::wstring &outXML)
{
	Book* book = xlCreateXMLBook();
	book->setKey(L"ChungYoung Lee", L"windows-21222f080fc9e70267b26e6baaj3g8lf");
	if (!book)
		return XLSLOADER::ReturnError(XLSLOADER::FAILED, 0, 0);
	if (!book->load(inFilePath.c_str()))
		return XLSLOADER::ReturnError(XLSLOADER::OPEN_FAILED, 0, 0);

	Sheet* defSheet = book->getSheet(0);
	if (defSheet)
	{
		for (int row = defSheet->firstRow() + 1; row < defSheet->lastRow(); ++row)
		{
			bool bFoundName = false;
			sTABLE_DEFINITION tableEntry;
			for (int col = defSheet->firstCol(); col < defSheet->lastCol(); ++col)
			{
				CellType cellType = defSheet->cellType(row, col);

				switch (col)
				{
				case COL_DATANAME:
					if (cellType == CELLTYPE_STRING)
					{
						tableEntry.dataName = defSheet->readStr(row, col);
						bFoundName = true;
					}
					break;
				case COL_DATAEXPLAIN:
					if (cellType == CELLTYPE_STRING)
					{
						tableEntry.dataExplain = defSheet->readStr(row, col);
						std::replace(tableEntry.dataExplain.begin(), tableEntry.dataExplain.end(), '\n', ' ');
					}
					break;
				case COL_DATATYPE:
					if (cellType == CELLTYPE_STRING)
					{
						std::wstring szContents;
						szContents = defSheet->readStr(row, col);
						if (szContents == TEXT("64"))
							tableEntry.dataType = TEXT("string");
						else
							tableEntry.dataType = szContents;
					}
					else if (cellType == CELLTYPE_NUMBER)
					{
// 						if (cell->GetInteger() == 64)
// 						{
						tableEntry.dataType = TEXT("string");
//						}
					}
					break;

				case COL_DATADEPENDENCY:
					if (cellType == CELLTYPE_NUMBER)
					{
						tableEntry.dataDependancy = static_cast<int>(defSheet->readNum(row, col));
					}
					else if (cellType == CELLTYPE_STRING)
					{
						std::wstring szContents;
						szContents = defSheet->readStr(row, col);
						tableEntry.dataDependancy = std::stoi(szContents);
					}
					break;
				}
			}
			if (bFoundName == true) outDefEntry.push_back(tableEntry);
		}
	}

	if (outDefEntry.size() == 0)
		return XLSLOADER::ReturnError(XLSLOADER::NOTFOUND_DEFINE, 0, 0);
	
	outXML.append(TEXT("<?xml version=\"1.0\"  encoding=\"UTF-16\"?>\n<Data>\n"));

	std::vector<std::wstring> vecColName;
	Sheet* dataSheet = book->getSheet(1);
	if (dataSheet)
	{
		for (int col = dataSheet->firstCol(); col < dataSheet->lastCol(); ++col)
		{
			CellType cellType = dataSheet->cellType(dataSheet->firstCol(), col);
			if (cellType == CELLTYPE_STRING)
			{
				vecColName.push_back(dataSheet->readStr(dataSheet->firstCol(), col));
			}
			else
			{
				return XLSLOADER::ReturnError(XLSLOADER::INVALIED_COLNAME, 1, col+1);
			}
		}

		std::wstring szContents;
		for (int row = dataSheet->firstRow() + 1; row < dataSheet->lastRow(); ++row)
		{
			outXML.append(TEXT("\t<Data "));
			for (int col = dataSheet->firstCol(); col < dataSheet->lastCol(); ++col)
			{
				if (outDefEntry[col].dataDependancy == SERVER || outDefEntry[col].dataDependancy == COMMENT)
					continue;

				CellType cellType = dataSheet->cellType(row, col);
				switch (cellType)
				{
				
				case CELLTYPE_STRING:
					szContents = vecColName[col] + TEXT("=\"") + dataSheet->readStr(row, col) + TEXT("\" ");
					break;
				case CELLTYPE_NUMBER:
					if (outDefEntry[col].dataType == _T("float"))
						szContents = vecColName[col] + TEXT("=\"") + std::to_wstring(dataSheet->readNum(row, col)) + TEXT("\" ");
					else
						szContents = vecColName[col] + TEXT("=\"") + std::to_wstring(static_cast<int>(dataSheet->readNum(row, col))) + TEXT("\" ");
					break;
				case CELLTYPE_EMPTY:
				case CELLTYPE_BLANK:
				case CELLTYPE_ERROR:
					//TODO : 여기에서 에러처리 한 번
//					szContents = vecColName[col] + TEXT("=\"\" ");
					return XLSLOADER::ReturnError(XLSLOADER::INVALIED_VALUE, row + 1, col + 1);
				}
				outXML.append(szContents);
			}
			outXML.append(TEXT("/>\n"));
		}
	}
	outXML.append(TEXT("</Data>"));

	return XLSLOADER::ReturnError(XLSLOADER::SUCCESS, 0, 0);
}

_Must_inspect_result_	void ExcelLoader::Close(void)
{

}
