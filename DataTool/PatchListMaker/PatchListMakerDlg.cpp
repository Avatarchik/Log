
// PatchListMakerDlg.cpp : 구현 파일
//

#include "stdafx.h"
#include "PatchListMaker.h"
#include "PatchListMakerDlg.h"
#include "FileManager.h"
#include "afxdialogex.h"
#include "PatchList.h"

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


// CPatchListMakerDlg 대화 상자



CPatchListMakerDlg::CPatchListMakerDlg(CWnd* pParent /*=NULL*/)
	: CDialogEx(IDD_PATCHLISTMAKER_DIALOG, pParent)
	, mPatchFolder(_T(""))
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CPatchListMakerDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
	DDX_Text(pDX, IDC_EDIT_PATCHFOLDER, mPatchFolder);
	DDX_Control(pDX, IDC_LIST_DESC, mListDesc);
}

BEGIN_MESSAGE_MAP(CPatchListMakerDlg, CDialogEx)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_BN_CLICKED(IDC_BUTTON_CHANGEFOLDER, &CPatchListMakerDlg::OnBnClickedButtonChangefolder)
	ON_BN_CLICKED(IDC_BUTTON_MAKEPATCHLIST, &CPatchListMakerDlg::OnBnClickedButtonMakepatchlist)
END_MESSAGE_MAP()


// CPatchListMakerDlg 메시지 처리기

BOOL CPatchListMakerDlg::OnInitDialog()
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

	GetWorkingFolderPath();
	if (mPatchFolder == TEXT(""))
	{
		WCHAR szCurrentDirectory[MAX_PATH];
		memset(szCurrentDirectory, 0x00, MAX_PATH);
		if (mPatchFolder == "")
			GetCurrentDirectory(MAX_PATH, szCurrentDirectory);
		mPatchFolder = szCurrentDirectory;
	}
	GetDlgItem(IDC_EDIT_PATCHFOLDER)->SetWindowText(mPatchFolder);

	return TRUE;  // 포커스를 컨트롤에 설정하지 않으면 TRUE를 반환합니다.
}

void CPatchListMakerDlg::OnSysCommand(UINT nID, LPARAM lParam)
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

void CPatchListMakerDlg::OnPaint()
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
HCURSOR CPatchListMakerDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}

void CPatchListMakerDlg::OnBnClickedButtonChangefolder()
{
	SetWorkingFolderPath();
}

void CPatchListMakerDlg::GetWorkingFolderPath(void)
{
	DWORD dwType = REG_SZ;
	DWORD dwSize = MAX_PATH;
	HKEY hKey;
	LONG lResult;
	TCHAR csBuffer[MAX_PATH];

	mPatchFolder.Empty();
	lResult = RegOpenKeyEx(HKEY_CURRENT_USER, TEXT("SOFTWARE\\KairosNCo\\PatchListMaker"), 0, KEY_READ, &hKey);
	if (lResult == ERROR_SUCCESS)
	{
		RegQueryValueEx(hKey, _T("WorkingFolder"), NULL, &dwType, (LPBYTE)csBuffer, &dwSize);
		mPatchFolder.SetString(csBuffer);
		UpdateData(FALSE);
	}
}

void CPatchListMakerDlg::OnBnClickedButtonMakepatchlist()
{
	CFileManager fileManager;
	std::vector<sPATCHFILE_INFO> patchFileList;
	fileManager.Search(mPatchFolder.GetString(), TEXT(".uni"), patchFileList);

	CPatchListMaker patchListMaker;
	if (patchListMaker.ReadPatchListXML(mPatchFolder.GetString()) == false)
	{
		mListDesc.AddString(L"PatchList.xml파일을 읽어올 수 없습니다.");
		return;
	}
	mListDesc.AddString(L"PatchList.xml파일을 읽었습니다.");
	if (patchListMaker.ReadPatchFileInfoXML(mPatchFolder.GetString()) == false)
	{
		mListDesc.AddString(L"PatchFileHistory.info가 없어서 모든 파일을 새로운 파일로 인식합니다.");
	}
	patchListMaker.CheckXMLList(patchFileList);
	int iNoneChanged = 0;
	std::wstring szDescription;
	for each(auto itor in patchListMaker.GetPatchFileList())
	{
		szDescription = L"";
		if (itor.XMLExists == FILEADDED_CHANGE)
		{
			szDescription = L"변경 :" + itor.fileName + L" " + std::to_wstring(itor.patchVersion);
			mListDesc.AddString(szDescription.c_str());
		}
		else if (itor.XMLExists == FILEADDED_ADD)
		{
			szDescription = L"추가 :" + itor.fileName + L" " + std::to_wstring(itor.fileSize);
			mListDesc.AddString(szDescription.c_str());
		}
		else
		{
			iNoneChanged++;
		}
	}
	szDescription = L"변경없는 파일 :" + std::to_wstring(iNoneChanged);
	mListDesc.AddString(szDescription.c_str());

	if (fileManager.WritePatchList(patchListMaker.GetPatchFileList(), mPatchFolder.GetString()) == false)
	{
		mListDesc.AddString(L"PatchList.xml파일을 만들지 못하였습니다.");
	}
	else
	{
		mListDesc.AddString(L"PatchList.xml파일을 만들었습니다.");
	}
	/*
	1. 기준은 LOCAL에 있는 파일이 되어야 한다. LOCAL에 있는 파일 정보를 먼저 읽어올 것.
	2. 읽어온 파일을 patchList쪽으로 보낸다.
	3. patchList에서 XML을 불러와서 



그리고 XML에서 읽어온 것과 fileManager에서 찾은 애들과 modifyDate를 비교
일치하면 통과. 일치하지 않으면 XML 변경
	*/

	//여기에서 XML파일 읽어오기
	
}

void CPatchListMakerDlg::SetWorkingFolderPath(void)
{
	UpdateData(TRUE);
	CRegKey regKey;
	regKey.Create(HKEY_CURRENT_USER, TEXT("SOFTWARE\\KairosNCo\\PatchListMaker"));
	regKey.SetStringValue(_T("WorkingFolder"), mPatchFolder.GetBuffer());
	regKey.Close();
}