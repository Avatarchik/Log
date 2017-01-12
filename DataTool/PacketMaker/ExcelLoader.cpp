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

_Check_return_ XLSLOADER::ReturnError ExcelLoader::Open(_In_ std::wstring inFilePath, _Out_ std::wstring &outPIDL, _Out_ std::wstring &outUnity, _Out_ std::wstring &outPacketDef)
{
	std::vector<sTABLE_DEFINITION> vDefEntry;
	std::vector<sPACKET_VARIABLE> vVariableEntry;
	std::string szFilePath(inFilePath.begin(), inFilePath.end());

	Book* book = xlCreateXMLBook();
	book->setKey(L"ChungYoung Lee", L"windows-21222f080fc9e70267b26e6baaj3g8lf");
	if (!book)
		return XLSLOADER::ReturnError(XLSLOADER::FAILED, TEXT(""), 0, 0);
	if (!book->load(inFilePath.c_str()))
		return XLSLOADER::ReturnError(XLSLOADER::OPEN_FAILED, TEXT(""), 0, 0);

#define START_ROW			2
#define SHEET_DEFINE_COUNT	2
#define COLUMN_USED			0
#define COLUMN_NAME			1
#define COLUMN_PARAM_START	3
#define COLUMN_PARAM_SIZE	3

	bool bOnlyServer = false;

	INT32 iSheetCount = 0;
	Sheet* defSheet = book->getSheet(0);
	if (defSheet)
	{
		std::wstring szNeeded = defSheet->readStr(0, 0);
		if (szNeeded == TEXT("ONLY_SERVER")) 
			bOnlyServer = true;

		if (bOnlyServer == false)
		{
			outUnity.append(TEXT("using UnityEngine;\n"));
			outUnity.append(TEXT("using System;\n"));
			outUnity.append(TEXT("using System.Collections;\n"));
			outUnity.append(TEXT("using System.Collections.Generic;\n"));
			outUnity.append(TEXT("using Nettention.Proud;\n\n"));
		}

		for (int row = defSheet->firstRow() + START_ROW; row < defSheet->lastRow(); ++row)
		{
			bool bFoundName = false;
			sTABLE_DEFINITION tableEntry;
			for (int col = defSheet->firstCol(); col < defSheet->lastCol(); ++col)
			{
				CellType cellType = defSheet->cellType(row, col);

				switch (col)
				{
				case COL_PACKET_NAME:
					if (cellType == CELLTYPE_STRING)
					{
						tableEntry.packetName = defSheet->readStr(row, col);
						bFoundName = true;
					}
					else
					{
						XLSLOADER::ReturnError(XLSLOADER::NOTFOUND_PACKETNAME, defSheet->name(), row, col);
					}
					break;
				case COL_PACKET_EXPLAIN:
					if (cellType == CELLTYPE_STRING)
					{
						tableEntry.packetExplain = defSheet->readStr(row, col);
						std::replace(tableEntry.packetExplain.begin(), tableEntry.packetExplain.end(), '\n', ' ');
					}
					else
					{
						//ERROR
					}
					break;
				case COL_PACKET_NUMBER:
					if (cellType == CELLTYPE_NUMBER)
					{
						tableEntry.packetStartNumber = static_cast<int>(defSheet->readNum(row, col));
					}
					else
					{
						XLSLOADER::ReturnError(XLSLOADER::NOTFOUND_PACKETNUMBER, defSheet->name(), row, col);
					}
					break;
				case COL_PACKET_MARSHALER:
					if (cellType == CELLTYPE_STRING)
					{
						tableEntry.packetMarshaler = defSheet->readStr(row, col);
					}
					else
					{
						//ERROR
					}
				}
			}
			iSheetCount++;
			if (bFoundName == true) vDefEntry.push_back(tableEntry);
		}
	}

	outPIDL.append(TEXT("package KNC_KM_ProtocolClass;\n\n"));

// 	if (vDefEntry.size()+2 != iSheetCount)
// 	{
// 		return XLSLOADER::ReturnError(XLSLOADER::NOTMATCH_SHEETDEFINE, TEXT(""), 0, 0);
// 	}

	Sheet* renameSheet = book->getSheet(1);
	if (renameSheet)
	{
		for (int row = renameSheet->firstRow() + 1; row < renameSheet->lastRow(); ++row)
		{
			if ((renameSheet->readStr(row, 0) != NULL) && (renameSheet->readStr(row, 1) != NULL))
				outPIDL.append(TEXT("rename cpp(") + std::wstring(renameSheet->readStr(row, 0)) + TEXT(", ") + std::wstring(renameSheet->readStr(row, 1)) + TEXT(");\n"));
		}
	}
	outPIDL.append(TEXT("\n"));

	std::wstring szPakcetName = TEXT("");
	std::wstring szPacketDef = TEXT("");
	std::wstring szMarshaler = TEXT("");

	INT32 iStartNumber = 0;

	for (INT32 i = SHEET_DEFINE_COUNT; i < iSheetCount + SHEET_DEFINE_COUNT; i++)
	{
		iStartNumber = 0;
		Sheet* dataSheet = book->getSheet(i);
		if (dataSheet)
		{
			szMarshaler = TEXT("");
			for each(auto itor in vDefEntry)
			{
				if (itor.packetName.compare(dataSheet->name()) == 0)
				{
					iStartNumber = itor.packetStartNumber;
					if (itor.packetMarshaler != TEXT("None"))
					{
						szMarshaler.append(TEXT("[marshaler(cs) = PacketMarshaler]"));
					}
					break;
				}
			}
			if (iStartNumber == 0)
			{
				return XLSLOADER::ReturnError(XLSLOADER::INVALIED_PACKETSTARTNUMBER, TEXT(""), 0, 0);
			}

			std::wstring szContents;
			szContents = szMarshaler + TEXT("global ") + std::wstring(dataSheet->name()) + TEXT(" ") + std::to_wstring(iStartNumber) + TEXT("\n");
			outPIDL.append(szContents);
			outPIDL.append(TEXT("{\n"));
			INT32 iCurrentCol = COLUMN_PARAM_START;

			std::wstring szPacketDefUpper = TEXT("");

			for (int row = dataSheet->firstRow() + 1; row < dataSheet->lastRow(); ++row)
			{
				if (dataSheet->readNum(row, COLUMN_USED) == 0)
					continue;

				szPakcetName = std::wstring(dataSheet->readStr(row, 1));
				szPacketDefUpper = szPakcetName;
				outPIDL.append(TEXT("\t") + szPakcetName + TEXT("("));
				std::transform(szPacketDefUpper.begin(), szPacketDefUpper.end(), szPacketDefUpper.begin(), toupper);
				szPacketDef.append(TEXT("\tpublic const int PACKET_") + szPacketDefUpper + TEXT("= ") + std::to_wstring(iStartNumber) + TEXT("; \n"));
				if (bOnlyServer == false)
				{
					outUnity.append(TEXT("public class ") + std::wstring(dataSheet->readStr(row, COLUMN_NAME)) + TEXT("\n"));
					outUnity.append(TEXT("{\n"));
				}
				iStartNumber = iStartNumber + 1;
				vVariableEntry.clear();

				//아예 비어있는 녀석인지는 확인해야 합니다.
				if (dataSheet->readStr(row, iCurrentCol) != NULL)
				{
					for (;;)
					{
						outPIDL.append(TEXT("[in] ") + std::wstring(dataSheet->readStr(row, iCurrentCol)) + TEXT(" ") + std::wstring(dataSheet->readStr(row, iCurrentCol + 1)));
						sPACKET_VARIABLE variable;
						variable.vaType = std::wstring(dataSheet->readStr(row, iCurrentCol));
						variable.vaName = std::wstring(dataSheet->readStr(row, iCurrentCol + 1));
						vVariableEntry.push_back(variable);

						iCurrentCol = iCurrentCol + COLUMN_PARAM_SIZE;
						if (iCurrentCol > dataSheet->lastCol())
						{
							iCurrentCol = COLUMN_PARAM_START;
							break;
						}
						if (dataSheet->readStr(row, iCurrentCol) == NULL)
						{
							iCurrentCol = COLUMN_PARAM_START;
							break;
						}
						outPIDL.append(TEXT(", "));

					}
				}
				
				outPIDL.append(TEXT(");\n"));

				if (bOnlyServer == false)
				{
					for each (auto itor in vVariableEntry)
					{
						outUnity.append(TEXT("\tpublic ") + itor.vaType + TEXT(" ") + itor.vaName + TEXT(";\n"));
					}
					outUnity.append(TEXT("\tpublic ") + szPakcetName + TEXT("("));
					int iEntryCount = (int)vVariableEntry.size();
					int iCurrentEntry = 0;
					for each (auto itor in vVariableEntry)
					{
						outUnity.append(itor.vaType + TEXT(" _") + itor.vaName);
						iCurrentEntry++;
						if (iCurrentEntry < iEntryCount)
							outUnity.append(TEXT(", "));
					}
					outUnity.append(TEXT(")\n\t{\n"));
					for each (auto itor in vVariableEntry)
					{
						outUnity.append(TEXT("\t\t") + itor.vaName + TEXT(" = _") + itor.vaName + TEXT(";\n"));
					}
					outUnity.append(TEXT("\t}\n}\n\n"));
				}
			}
		}
		outPIDL.append(TEXT("}\n\n"));
	}

	if (bOnlyServer == false)
	{
		outPacketDef.append(szPacketDef);
	}

	return XLSLOADER::ReturnError(XLSLOADER::SUCCESS, TEXT(""), 0, 0, bOnlyServer);;
}

_Must_inspect_result_	void ExcelLoader::Close(void)
{

}

/*이건 잠시 폐쇄. 클라이언트에서 이녀석을 너무 자유롭게 쓰고 있어서 정적으로 쓰는 예를 확신하게 찾아서 만들어내야 함.
_Check_return_	XLSLOADER ExcelLoader::OpenStruct(_Out_ std::wstring &outStructServer, _Out_ std::wstring &outStructClient)
{
	std::string szFilePath(TEXT("PacketStruct.xlsx"));
	std::vector<sVARIABLEDEF> vVariableEntry;

	Book* book = xlCreateXMLBook();
	book->setKey(L"ChungYoung Lee", L"windows-21222f080fc9e70267b26e6baaj3g8lf");
	if (!book)
		return XLSLOADER::ReturnError(XLSLOADER::FAILED, TEXT(""), 0, 0);
	if (!book->load(inFilePath.c_str()))
		return XLSLOADER::ReturnError(XLSLOADER::OPEN_FAILED, TEXT(""), 0, 0);

#define START_ROW 2

	INT32 iSheetCount = 0;
	Sheet* defSheet = book->getSheet(0);
	std::wstring szClassName = TEXT("");
	std::wstring szType = TEXT("");
	std::wstring szVariable = TEXT("");
	if (defSheet)
	{
		outStructServer.append(TEXT("#pragma once\n\n"));

		outStructClient.append(TEXT("using UnityEngine;\n"));
		outStructClient.append(TEXT("using System.Collections;\n"));
		outStructClient.append(TEXT("using System.Collections.Generic;\n"));
		outStructClient.append(TEXT("using Nettention.Proud;\n\n"));

		for (int row = defSheet->firstRow() + START_ROW; row < defSheet->lastRow(); ++row)
		{
			szClassName = dataSheet->readStr(row, COLUMN_NAME);

			for (;;)
			{
				if (dataSheet->readStr(row, iCurrentCol) != NULL)
				{
					outPIDL.append(TEXT("[in] ") + std::wstring(dataSheet->readStr(row, iCurrentCol)) + TEXT(" ") + std::wstring(dataSheet->readStr(row, iCurrentCol + 1)));
					sVARIABLEDEF variable;
					variable.vaType = GetCShopTypetoCppType(dataSheet->readStr(row, iCurrentCol), &variable.vaTypeString);
					variable.vaName = std::wstring(dataSheet->readStr(row, iCurrentCol + 1));
					if (dataSheet->readStr(row, iCurrentCol + 2) != NULL)
						variable.vaComments = std::wstring(dataSheet->readStr(row, iCurrentCol + 2));
					vVariableEntry.push_back(variable);

					iCurrentCol = iCurrentCol + 3;
					if (iCurrentCol > dataSheet->lastCol())
					{
						iCurrentCol = 2;
						break;
					}
					if (dataSheet->readStr(row, iCurrentCol) == NULL)
					{
						iCurrentCol = 2;
						break;
					}
				}
			}

			outStructServer.append(TEXT("class ") + szClassName + "\n{\npublic:\n");
			outStructClient.append(TEXT("public class ") + szClassName);

			for each (auto itor in vVariableEntry)
			{
				outStructClient.append(TEXT("\tpublic ") + itor.vaTypeString + TEXT("\t") + itor.vaName + TEXT(";\n"));
			}
			outStructClient.append(TEXT("\npublic ") + szClassName + "()\n{");

			for each (auto itor in vVariableEntry)
			{
				outStructServer.append(TEXT("\t") + itor.vaTypeString + TEXT(" ") + itor.vaName + TEXT("\n\n"));
				outStructServer.append(TEXT("\t") + szClassName + TEXT("()\n\t\t{"));
				if (itor.vaType == VARIABLETYPE_NUMBER)
				{
					outStructServer.append(TEXT("\t\t") + itor.vaName + TEXT(" = 0;\n"));
					outStructClient.append(TEXT("\t\t") + itor.vaName + TEXT(" = 0;\n));
				}
				else if (itor.vaType == VARIABLETYPE_STRING)
				{
					outStructServer.append(TEXT("\t\t") + itor.vaName + TEXT(" = TEXT("");\n"));
					outStructClient.append(TEXT("\t\t") + itor.vaName + TEXT(" = string.Empty;\n"));
				}
				else if (itor.vaType == VARIABLETYPE_HOSTID)
				{
					outStructServer.append(TEXT("\t\t") + itor.vaName + TEXT(" = HostID_None;\n"));
					outStructClient.append(TEXT("\t\t") + itor.vaName + TEXT(" = HostID.HostID_None;\n"));
				}
				else if (itor.vaType == VARIABLETYPE_BOOL)
				{
					outStructServer.append(TEXT("\t\t") + itor.vaName + TEXT(" = false;\n"));
					outStructClient.append(TEXT("\t\t") + itor.vaName + TEXT(" = false;\n"));
				}
				else if (itor.vaType == VARIABLETYPE_OBJECT)
				{
					outStructServer.append(TEXT("\t\t") + itor.vaName + TEXT(" = NULL;\n"));
					outStructClient.append(TEXT("\t\t") + itor.vaName + TEXT(" = null;\n"));
				}
				else if (itor.vaType == VARIABLETYPE_FASTARRAY)
				{
					outStructServer.append(TEXT("\t\t") + itor.vaName + TEXT(".Clear();\n"));
					outStructClient.append(TEXT("\t\t") + itor.vaName + TEXT(".Clear();\n"));
				}
			}
			outStructServer.append(TEXT("\t}\n};"));
			outStructClient.append(TEXT("\t}\n\n"));

			outStructServer.append(TEXT("namespace Proud\n{\n"));
			outStructServer.append(TEXT("\tinline CMessage& operator<<(CMessage &msg, const ") + szClassName + TEXT("& obj)\n\t{\n"));

			for each (auto itor in vVariableEntry)
			{
				outStructServer.append(TEXT("\t\tmsg << obj.") + itor.vaName + TEXT(";\n"));
			}
			outStructServer.append(TEXT("\t\treturn msg;\n\t}\n\n"));
			outStructServer.append(TEXT("\tinline CMessage& operator>>(CMessage &msg, ") + szClassName + TEXT("& obj)\n\t{\n"));

			for each (auto itor in vVariableEntry)
			{
				outStructServer.append(TEXT("\t\tmsg >> obj.") + itor.vaName + TEXT(";\n"));
			}
			outStructServer.append(TEXT("\t\treturn msg;\n\t}\n}\n"));
			vVariableEntry.clear();
		}
	}

	return XLSLOADER::ReturnError(XLSLOADER::SUCCESS, TEXT(""), 0, 0, bOnlyServer);;
}
*/