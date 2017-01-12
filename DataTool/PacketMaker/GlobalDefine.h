#pragma once

#include <string>

enum eDATADEPEN 
{ 
	CLIENT = 1, 
	SERVER, 
	BOTH 
};
enum eDATACOL 
{
	COL_PACKET_NAME = 0, 
	COL_PACKET_EXPLAIN = 1,
	COL_PACKET_NUMBER = 4,
	COL_PACKET_MARSHALER =5,
};

const std::string SHEET_DEF = "Definition";
const std::string SHEET_DATA = "Data";

const std::wstring DATA_DEFNAME = TEXT("DefinitionName");
const std::wstring DATA_DEFEXPLAIN = TEXT("DefinitionExplain");
const std::wstring DATA_TYPE = TEXT("DataType");
const std::wstring DATA_DEPENDENCY = TEXT("DataDependency");

struct sTABLE_DEFINITION
{
	std::wstring packetName;
	std::wstring dataType;
	std::wstring packetExplain;
	int packetStartNumber;
	std::wstring packetMarshaler;

	sTABLE_DEFINITION(void)
	{
		packetName = TEXT("");
		dataType = TEXT("");
		packetExplain = TEXT("");
		packetStartNumber = BOTH;
		packetMarshaler = TEXT("");
	}
};

struct sPACKET_VARIABLE
{
	std::wstring vaType;
	std::wstring vaName;
};
