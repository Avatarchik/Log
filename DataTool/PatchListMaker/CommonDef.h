#pragma once

enum eFILEADDED
{
	FILEADDED_NONE = 0,
	FILEADDED_ADD,
	FILEADDED_CHANGE,
	FILEADDED_DELETE,
};

struct sPATCHFILE_INFO
{
	INT32 loadingScenePosition;
	INT32 patchVersion;
	std::wstring fileName;
	ULONG fileModifyTimeHigh;
	ULONG fileModifyTimeLow;
	UINT32 fileSize;
	eFILEADDED XMLExists;
	UINT32 patchNameIndex;
	UINT32 loadingNameIndex;

	sPATCHFILE_INFO()
	{
		loadingScenePosition = -1;
		patchVersion = 0;
		fileName = TEXT("");
		fileModifyTimeHigh = 0;
		fileModifyTimeLow = 0;
		fileSize = 0;
		XMLExists = FILEADDED_NONE;
		patchNameIndex = 0;
		loadingNameIndex = 0;
	}
};