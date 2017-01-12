#pragma once

#include <string>

enum eDATADEPEN 
{
	COMMENT = 0,
	CLIENT, 
	SERVER, 
	BOTH 
};
enum eDATACOL 
{
	COL_DATANAME = 0, 
	COL_DATAEXPLAIN = 1,
	COL_DATATYPE = 2, 
	COL_DATADEPENDENCY = 3,
};

const std::string SHEET_DEF = "Definition";
const std::string SHEET_DATA = "Data";

const std::wstring DATA_DEFNAME = TEXT("DefinitionName");
const std::wstring DATA_DEFEXPLAIN = TEXT("DefinitionExplain");
const std::wstring DATA_TYPE = TEXT("DataType");
const std::wstring DATA_DEPENDENCY = TEXT("DataDependency");

struct sTABLE_DEFINITION
{
	std::wstring dataName;
	std::wstring dataType;
	std::wstring dataExplain;
	int dataDependancy;

	sTABLE_DEFINITION(void)
	{
		dataName = TEXT("");
		dataType = TEXT("");
		dataExplain = TEXT("");
		dataDependancy = COMMENT;
	}
};
