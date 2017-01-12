// APKDownloadPageMaker.cpp : 콘솔 응용 프로그램에 대한 진입점을 정의합니다.
//

#include "stdafx.h"
#include "FileManager.h"

int main(int argc, WCHAR* argv[])
{
	CFileManager fileManager;
	std::wstring szRecentFilePath, szRecentFileDate;
	fileManager.Search(szRecentFilePath, szRecentFileDate);

	std::wstring szRecentFileName = PathFindFileName(szRecentFilePath.c_str());
	
	size_t iHeadSepPos = szRecentFilePath.find('_');
	size_t iTailsSepPos = szRecentFilePath.rfind('_');
	std::wstring szBuildNumber = szRecentFilePath.substr(iHeadSepPos + 1, iTailsSepPos - iHeadSepPos -1);

	fileManager.MakeHTML(szBuildNumber, szRecentFileName, szRecentFileDate);
    return 0;
}

