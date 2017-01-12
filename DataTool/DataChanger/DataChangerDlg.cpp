
// DataChangerDlg.cpp : 구현 파일
//

#include "stdafx.h"
#include "DataChanger.h"
#include "DataChangerDlg.h"
#include "afxdialogex.h"
#include "FileManager.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// 응용 프로그램 정보에 사용되는 CAboutDlg 대화 상자입니다.

class CAboutDlg : public CDialogEx
{
public:
	CAboutDlg();

// 대화 상자 데이터입니다.
#ifdef AFX_DESIGN_TIME
	enum { IDD = IDD_ABOUTBOX };
#endif

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 지원입니다.

// 구현입니다.
protected:
	DECLARE_MESSAGE_MAP()
};

CAboutDlg::CAboutDlg() : CDialogEx(IDD_ABOUTBOX)
{
}

void CAboutDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CAboutDlg, CDialogEx)
END_MESSAGE_MAP()


// CDataChangerDlg 대화 상자



CDataChangerDlg::CDataChangerDlg(CWnd* pParent /*=NULL*/)
	: CDialogEx(IDD_DATACHANGER_DIALOG, pParent)
	, mDataFolder(_T(""))
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CDataChangerDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_LIST_DESCRIPTION, mListDescription);
	DDX_Text(pDX, IDC_EDIT_DATAFOLDER_PATH, mDataFolder);
}

BEGIN_MESSAGE_MAP(CDataChangerDlg, CDialogEx)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_BN_CLICKED(IDC_BUTTON_LOAD, &CDataChangerDlg::OnBnClickedButtonLoad)
	ON_BN_CLICKED(IDC_BUTTON_START, &CDataChangerDlg::OnBnClickedButtonStart)
END_MESSAGE_MAP()


// CDataChangerDlg 메시지 처리기

BOOL CDataChangerDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	// 시스템 메뉴에 "정보..." 메뉴 항목을 추가합니다.

	// IDM_ABOUTBOX는 시스템 명령 범위에 있어야 합니다.
	ASSERT((IDM_ABOUTBOX & 0xFFF0) == IDM_ABOUTBOX);
	ASSERT(IDM_ABOUTBOX < 0xF000);

	CMenu* pSysMenu = GetSystemMenu(FALSE);
	if (pSysMenu != NULL)
	{
		BOOL bNameValid;
		CString strAboutMenu;
		bNameValid = strAboutMenu.LoadString(IDS_ABOUTBOX);
		ASSERT(bNameValid);
		if (!strAboutMenu.IsEmpty())
		{
			pSysMenu->AppendMenu(MF_SEPARATOR);
			pSysMenu->AppendMenu(MF_STRING, IDM_ABOUTBOX, strAboutMenu);
		}
	}

	// 이 대화 상자의 아이콘을 설정합니다.  응용 프로그램의 주 창이 대화 상자가 아닐 경우에는
	//  프레임워크가 이 작업을 자동으로 수행합니다.
	SetIcon(m_hIcon, TRUE);			// 큰 아이콘을 설정합니다.
	SetIcon(m_hIcon, FALSE);		// 작은 아이콘을 설정합니다.

//	ShowWindow(SW_MAXIMIZE);

	GetWorkingFolderPath();
	if (mDataFolder == TEXT(""))
	{
		WCHAR szCurrentDirectory[MAX_PATH];
		memset(szCurrentDirectory, 0x00, MAX_PATH);
		if (mDataFolder == "")
			GetCurrentDirectory(MAX_PATH, szCurrentDirectory);
		mDataFolder = szCurrentDirectory;
	}
	GetDlgItem(IDC_EDIT_DATAFOLDER_PATH)->SetWindowText(mDataFolder);

	return TRUE;  // 포커스를 컨트롤에 설정하지 않으면 TRUE를 반환합니다.
}

void CDataChangerDlg::OnSysCommand(UINT nID, LPARAM lParam)
{
	if ((nID & 0xFFF0) == IDM_ABOUTBOX)
	{
		CAboutDlg dlgAbout;
		dlgAbout.DoModal();
	}
	else
	{
		CDialogEx::OnSysCommand(nID, lParam);
	}
}

// 대화 상자에 최소화 단추를 추가할 경우 아이콘을 그리려면
//  아래 코드가 필요합니다.  문서/뷰 모델을 사용하는 MFC 응용 프로그램의 경우에는
//  프레임워크에서 이 작업을 자동으로 수행합니다.

void CDataChangerDlg::OnPaint()
{
	if (IsIconic())
	{
		CPaintDC dc(this); // 그리기를 위한 디바이스 컨텍스트입니다.

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		// 클라이언트 사각형에서 아이콘을 가운데에 맞춥니다.
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// 아이콘을 그립니다.
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialogEx::OnPaint();
	}

}

// 사용자가 최소화된 창을 끄는 동안에 커서가 표시되도록 시스템에서
//  이 함수를 호출합니다.
HCURSOR CDataChangerDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}



void CDataChangerDlg::OnBnClickedButtonLoad()
{
	UpdateData(TRUE);
	SetWorkingFolderPath();
	GetDlgItem(IDC_STATIC_DATAFOLDER_PATH)->SetWindowText(TEXT("폴더 변경이 완료되었습니다."));
}


void CDataChangerDlg::OnBnClickedButtonStart()
{
	WCHAR szCurrentDirectory[MAX_PATH];
	memset(szCurrentDirectory, 0x00, MAX_PATH);
	if (mDataFolder == "")
	{
		GetCurrentDirectory(MAX_PATH, szCurrentDirectory);
		mDataFolder = szCurrentDirectory;
	}
	std::vector<std::wstring> tableFilesPath;
	FileManager fileManager;
	fileManager.Search(mDataFolder, TEXT(".xlsx") , tableFilesPath);

	std::wstring szDescription;
	mListDescription.ResetContent();
	for each (auto itor in tableFilesPath)
	{
		//int iLastListCount = mListDescription.GetCount();

		szDescription = TEXT("Find file : ") + itor;
		mListDescription.AddString(szDescription.c_str());
	}

	size_t iFileNameCount;
	std::wstring szFileName;
	std::wstring szwXML;
//	CString szFileName = mDataFolderPath.Mid(mDataFolderPath.ReverseFind('\\'), iFileNameCount);

	std::vector<sTABLE_DEFINITION> dataEntry;
	ExcelLoader ExcelLoader;
	for each (auto itor in tableFilesPath)
	{
		dataEntry.clear();
		szwXML = TEXT("");
		//여기서 하나씩 열어 놓은 다음에 구조체를 만들자.
		XLSLOADER::ReturnError returnValue = ExcelLoader.Open(itor.c_str(), dataEntry, szwXML);
		std::wstring szMessage;
		if (returnValue.errorCode != XLSLOADER::SUCCESS)
		{
			switch (returnValue.errorCode)
			{
				case XLSLOADER::FAILED:
					GetDlgItem(IDC_STATIC_DATAFOLDER_PATH)->SetWindowText(TEXT("EXCEL LOADER를 불러올 수 없습니다."));
					break;
				case XLSLOADER::OPEN_FAILED:
					szMessage.append(std::wstring(itor.c_str()) + TEXT("EXCEL파일을 열 수 없습니다. "));
					GetDlgItem(IDC_STATIC_DATAFOLDER_PATH)->SetWindowText(szMessage.c_str());
					break;
				case XLSLOADER::NOTFOUND_DEFINE:
					szMessage.append(std::wstring(itor.c_str()) + TEXT(" Define이 잘못 되었습니다.") + std::to_wstring(returnValue.iErrorRow) + TEXT(":") + std::to_wstring(returnValue.iErrorCol));
					GetDlgItem(IDC_STATIC_DATAFOLDER_PATH)->SetWindowText(szMessage.c_str());
					break;
				case XLSLOADER::INVALIED_COLNAME:
					szMessage.append(std::wstring(itor.c_str()) + TEXT("데이터의 컬럼 이름이 잘못되었습니다.") + std::to_wstring(returnValue.iErrorRow) + TEXT(":") + std::to_wstring(returnValue.iErrorCol));
					GetDlgItem(IDC_STATIC_DATAFOLDER_PATH)->SetWindowText(szMessage.c_str());
					break;
				case XLSLOADER::INVALIED_VALUE:
					szMessage.append(std::wstring(itor.c_str()) + TEXT("값이 잘못되었습니다.") + std::to_wstring(returnValue.iErrorRow) + TEXT(":") + std::to_wstring(returnValue.iErrorCol));
					GetDlgItem(IDC_STATIC_DATAFOLDER_PATH)->SetWindowText(szMessage.c_str());
					break;
			}
			return;
		}
		iFileNameCount = itor.rfind('.') - itor.rfind('\\') - 1;
		szFileName = itor.substr(itor.rfind('\\')+1, iFileNameCount);
		fileManager.MakeCS(mDataFolder.GetBuffer(), szFileName, dataEntry);
		fileManager.MakeXML(mDataFolder.GetBuffer(), szFileName, szwXML.c_str());
		szDescription = TEXT("Make Complete : ") + itor;
		mListDescription.AddString(szDescription.c_str());
	}
	GetDlgItem(IDC_STATIC_DATAFOLDER_PATH)->SetWindowText(TEXT("변환이 완료되었습니다."));
	fileManager.MakeNavigator(mDataFolder.GetBuffer(), tableFilesPath);
}

void CDataChangerDlg::GetWorkingFolderPath(void)
{
	DWORD dwType = REG_SZ;
	DWORD dwSize = MAX_PATH;
	HKEY hKey;
	LONG lResult;
	TCHAR csBuffer[MAX_PATH];

	mDataFolder.Empty();
	lResult = RegOpenKeyEx(HKEY_CURRENT_USER, TEXT("SOFTWARE\\KairosNCo\\DataChanger"), 0, KEY_READ, &hKey);
	if (lResult == ERROR_SUCCESS)
	{
		RegQueryValueEx(hKey, _T("WorkingFolder"), NULL, &dwType, (LPBYTE)csBuffer, &dwSize);
		mDataFolder.SetString(csBuffer);
		UpdateData(FALSE);
	}
}

void CDataChangerDlg::SetWorkingFolderPath(void)
{
	UpdateData(TRUE);
	CRegKey regKey;
	regKey.Create(HKEY_CURRENT_USER, TEXT("SOFTWARE\\KairosNCo\\DataChanger"));
	regKey.SetStringValue(_T("WorkingFolder"), mDataFolder.GetBuffer());
	regKey.Close();
}