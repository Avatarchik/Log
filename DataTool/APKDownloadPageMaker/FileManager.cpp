#include "stdafx.h"
#include "FileManager.h"


CFileManager::CFileManager()
{
}


CFileManager::~CFileManager()
{
}

void CFileManager::Ctor(void)
{

}

void CFileManager::Dtor(void)
{

}

_Check_return_	bool CFileManager::Search(_Out_ std::wstring & outSelectedFileName, _Out_ std::wstring & outFileModifyTime)
{
	CFileFind file;
	CString szSearchPath = "*.*";
	BOOL bFind = file.FindFile(szSearchPath);

	CString szSelectedFile;
	CFileStatus selectedFileStatus;

	// 	if (bFind == false)
	// 		::CreateDirectory(inPath, NULL);

	CString strFolderItem, strFileExt;
	CFileStatus fileStatus;
	while (bFind)
	{
		bFind = file.FindNextFile();
		strFolderItem = file.GetFilePath();
		strFileExt = strFolderItem.Mid(strFolderItem.ReverseFind('.'));
		if (!file.IsDots())
		{
			strFileExt.MakeUpper();
			if (file.IsDirectory()) continue;
			if (strFileExt == ".APK")
			{
				CFile::GetStatus(strFolderItem, fileStatus);
				if (fileStatus.m_mtime > selectedFileStatus.m_mtime)
				{
					szSelectedFile = strFolderItem;
					selectedFileStatus = fileStatus;
				}
			}
		}
	}

	CString sDate = selectedFileStatus.m_mtime.Format("%Y-%m-%d %H:%M:%S");

	outSelectedFileName = szSelectedFile.GetBuffer();
	outFileModifyTime = sDate.GetBuffer();
	return true;
}

//#define _KINMOSA
#define _ARPEGIO

_Check_return_	 bool CFileManager::MakeHTML(_In_ std::wstring inBuildNumber, _In_ std::wstring inFileName, _In_ std::wstring inDate)
{
	CString strReadLine;

	std::wstring szFilePath = TEXT("index.html");

	FILE *file = _wfopen(szFilePath.c_str(), L"wb");
	if (file == NULL)
	{
		return false;
	}

	fputwc(0xFEFF, file);

	fwprintf(file, TEXT("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\n"));
	fwprintf(file, TEXT("<html xmlns=\"http://www.w3.org/1999/xhtml\">\n"));
	fwprintf(file, TEXT("<head>\n"));
	fwprintf(file, TEXT("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" / >\n"));
	fwprintf(file, TEXT("<meta name = \"viewport\" content = \"width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0\">\n"));
	std::wstring szContents;
#if defined(_ARPEGIO)
	szContents = TEXT("<title>Blue Steel of Arpegio 0.1") + inBuildNumber + TEXT("(") + inBuildNumber + TEXT(")</title>\n");
#elif defined(_KINMOSA)
	szContents = TEXT("<title>KinMosa 0.1.") + inBuildNumber + TEXT("</title>\n");
#endif
	fwprintf(file, szContents.c_str());
	fwprintf(file, TEXT("<style type = \"text/css\">\n"));
	fwprintf(file, TEXT("body{ background:#fff;margin:0;padding:0;font-family:arial,helvetica,sans-serif;text-align:center;padding:10px;color:#333;font-size:16px; }\n"));
	fwprintf(file, TEXT("#container{width:300px; margin:0 auto;}\n"));
	fwprintf(file, TEXT("h1{margin:0;padding:0;font-size:14px;}\n"));
	fwprintf(file, TEXT("p{font-size:13px;}\n"));
	fwprintf(file, TEXT(".link{background:#ecf5ff;border-top:1px solid #fff;border:1px solid #dfebf8;margin-top:.5em;padding:.3em;}\n"));
	fwprintf(file, TEXT(".link a{text-decoration:none;font-size:15px;display:block;color:#069;}\n"));
	fwprintf(file, TEXT(".last_updated{font-size:x-small;text-align:right;font-style:italic;}\n"));
	fwprintf(file, TEXT(".created_with{font-size:x-small;text-align:center;}\n"));
	fwprintf(file, TEXT("</style>\n"));
	fwprintf(file, TEXT("</head>\n"));
	fwprintf(file, TEXT("<body>\n"));
	fwprintf(file, TEXT("\n\n"));
	fwprintf(file, TEXT("<div id=\"container\">\n"));
	fwprintf(file, TEXT("\n\n"));
	fwprintf(file, TEXT("<p><img src='AppIcon60x60@2x.png' length='57' width='57'/></p>\n"));
	fwprintf(file, TEXT("\n\n"));
	fwprintf(file, TEXT("<h1>Android Install Link</h1>\n"));
	fwprintf(file, TEXT("\n\n"));
#if defined(_ARPEGIO)
	szContents = TEXT("<div class=\"link\"><a href=\"") + inFileName + TEXT("\">Tap Here to Install<br/>Blue Steel of Arpegio 0.1.") + inBuildNumber +  TEXT("(") + inBuildNumber + TEXT(") <br/>Directly On Your Device.</a></div>\n");
#elif defined(_KINMOSA)
	szContents = TEXT("<div class=\"link\"><a href=\"") + inFileName + TEXT("\">Tap Here to Install<br/>ª­ªóª¤ªí«â«¶«¤«¯ 0.1.") + inBuildNumber + TEXT("<br/>Directly On Your Device.</a></div>\n");
#endif
	fwprintf(file, szContents.c_str());
	fwprintf(file, TEXT("\n"));
	fwprintf(file, TEXT("<p><strong>Link didn't work?</strong><br/>\n"));
	fwprintf(file, TEXT("Make sure you're visiting this page on your device, not your computer.</p>\n"));
	fwprintf(file, TEXT("\n"));
	szContents = TEXT("<p class = \"last_updated\">Last Updated: ") + inDate + TEXT(".</p>\n");
	fwprintf(file, szContents.c_str());
	fwprintf(file, TEXT("\n"));
	fwprintf(file, TEXT("</div>\n"));
	fwprintf(file, TEXT("\n"));
	fwprintf(file, TEXT("</body>\n"));
	fwprintf(file, TEXT("</html>\n"));

	fclose(file);
	//file.Close();
	return true;
}