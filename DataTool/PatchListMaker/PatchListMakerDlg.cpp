
// PatchListMakerDlg.cpp : ���� ����
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


// ���� ���α׷� ������ ���Ǵ� CAboutDlg ��ȭ �����Դϴ�.

class CAboutDlg : public CDialogEx
{
public:
	CAboutDlg();

// ��ȭ ���� �������Դϴ�.
#ifdef AFX_DESIGN_TIME
	enum { IDD = IDD_ABOUTBOX };
#endif

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV �����Դϴ�.

// �����Դϴ�.
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


// CPatchListMakerDlg ��ȭ ����



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


// CPatchListMakerDlg �޽��� ó����

BOOL CPatchListMakerDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	// �ý��� �޴��� "����..." �޴� �׸��� �߰��մϴ�.

	// IDM_ABOUTBOX�� �ý��� ��� ������ �־�� �մϴ�.
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

	// �� ��ȭ ������ �������� �����մϴ�.  ���� ���α׷��� �� â�� ��ȭ ���ڰ� �ƴ� ��쿡��
	//  �����ӿ�ũ�� �� �۾��� �ڵ����� �����մϴ�.
	SetIcon(m_hIcon, TRUE);			// ū �������� �����մϴ�.
	SetIcon(m_hIcon, FALSE);		// ���� �������� �����մϴ�.

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

	return TRUE;  // ��Ŀ���� ��Ʈ�ѿ� �������� ������ TRUE�� ��ȯ�մϴ�.
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

// ��ȭ ���ڿ� �ּ�ȭ ���߸� �߰��� ��� �������� �׸�����
//  �Ʒ� �ڵ尡 �ʿ��մϴ�.  ����/�� ���� ����ϴ� MFC ���� ���α׷��� ��쿡��
//  �����ӿ�ũ���� �� �۾��� �ڵ����� �����մϴ�.

void CPatchListMakerDlg::OnPaint()
{
	if (IsIconic())
	{
		CPaintDC dc(this); // �׸��⸦ ���� ����̽� ���ؽ�Ʈ�Դϴ�.

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		// Ŭ���̾�Ʈ �簢������ �������� ����� ����ϴ�.
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// �������� �׸��ϴ�.
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialogEx::OnPaint();
	}
}

// ����ڰ� �ּ�ȭ�� â�� ���� ���ȿ� Ŀ���� ǥ�õǵ��� �ý��ۿ���
//  �� �Լ��� ȣ���մϴ�.
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
		mListDesc.AddString(L"PatchList.xml������ �о�� �� �����ϴ�.");
		return;
	}
	mListDesc.AddString(L"PatchList.xml������ �о����ϴ�.");
	if (patchListMaker.ReadPatchFileInfoXML(mPatchFolder.GetString()) == false)
	{
		mListDesc.AddString(L"PatchFileHistory.info�� ��� ��� ������ ���ο� ���Ϸ� �ν��մϴ�.");
	}
	patchListMaker.CheckXMLList(patchFileList);
	int iNoneChanged = 0;
	std::wstring szDescription;
	for each(auto itor in patchListMaker.GetPatchFileList())
	{
		szDescription = L"";
		if (itor.XMLExists == FILEADDED_CHANGE)
		{
			szDescription = L"���� :" + itor.fileName + L" " + std::to_wstring(itor.patchVersion);
			mListDesc.AddString(szDescription.c_str());
		}
		else if (itor.XMLExists == FILEADDED_ADD)
		{
			szDescription = L"�߰� :" + itor.fileName + L" " + std::to_wstring(itor.fileSize);
			mListDesc.AddString(szDescription.c_str());
		}
		else
		{
			iNoneChanged++;
		}
	}
	szDescription = L"������� ���� :" + std::to_wstring(iNoneChanged);
	mListDesc.AddString(szDescription.c_str());

	if (fileManager.WritePatchList(patchListMaker.GetPatchFileList(), mPatchFolder.GetString()) == false)
	{
		mListDesc.AddString(L"PatchList.xml������ ������ ���Ͽ����ϴ�.");
	}
	else
	{
		mListDesc.AddString(L"PatchList.xml������ ��������ϴ�.");
	}
	/*
	1. ������ LOCAL�� �ִ� ������ �Ǿ�� �Ѵ�. LOCAL�� �ִ� ���� ������ ���� �о�� ��.
	2. �о�� ������ patchList������ ������.
	3. patchList���� XML�� �ҷ��ͼ� 



�׸��� XML���� �о�� �Ͱ� fileManager���� ã�� �ֵ�� modifyDate�� ��
��ġ�ϸ� ���. ��ġ���� ������ XML ����
	*/

	//���⿡�� XML���� �о����
	
}

void CPatchListMakerDlg::SetWorkingFolderPath(void)
{
	UpdateData(TRUE);
	CRegKey regKey;
	regKey.Create(HKEY_CURRENT_USER, TEXT("SOFTWARE\\KairosNCo\\PatchListMaker"));
	regKey.SetStringValue(_T("WorkingFolder"), mPatchFolder.GetBuffer());
	regKey.Close();
}