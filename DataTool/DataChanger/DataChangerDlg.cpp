
// DataChangerDlg.cpp : ���� ����
//

#include "stdafx.h"
#include "DataChanger.h"
#include "DataChangerDlg.h"
#include "afxdialogex.h"
#include "FileManager.h"

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


// CDataChangerDlg ��ȭ ����



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


// CDataChangerDlg �޽��� ó����

BOOL CDataChangerDlg::OnInitDialog()
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

	return TRUE;  // ��Ŀ���� ��Ʈ�ѿ� �������� ������ TRUE�� ��ȯ�մϴ�.
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

// ��ȭ ���ڿ� �ּ�ȭ ���߸� �߰��� ��� �������� �׸�����
//  �Ʒ� �ڵ尡 �ʿ��մϴ�.  ����/�� ���� ����ϴ� MFC ���� ���α׷��� ��쿡��
//  �����ӿ�ũ���� �� �۾��� �ڵ����� �����մϴ�.

void CDataChangerDlg::OnPaint()
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
HCURSOR CDataChangerDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}



void CDataChangerDlg::OnBnClickedButtonLoad()
{
	UpdateData(TRUE);
	SetWorkingFolderPath();
	GetDlgItem(IDC_STATIC_DATAFOLDER_PATH)->SetWindowText(TEXT("���� ������ �Ϸ�Ǿ����ϴ�."));
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
		//���⼭ �ϳ��� ���� ���� ������ ����ü�� ������.
		XLSLOADER::ReturnError returnValue = ExcelLoader.Open(itor.c_str(), dataEntry, szwXML);
		std::wstring szMessage;
		if (returnValue.errorCode != XLSLOADER::SUCCESS)
		{
			switch (returnValue.errorCode)
			{
				case XLSLOADER::FAILED:
					GetDlgItem(IDC_STATIC_DATAFOLDER_PATH)->SetWindowText(TEXT("EXCEL LOADER�� �ҷ��� �� �����ϴ�."));
					break;
				case XLSLOADER::OPEN_FAILED:
					szMessage.append(std::wstring(itor.c_str()) + TEXT("EXCEL������ �� �� �����ϴ�. "));
					GetDlgItem(IDC_STATIC_DATAFOLDER_PATH)->SetWindowText(szMessage.c_str());
					break;
				case XLSLOADER::NOTFOUND_DEFINE:
					szMessage.append(std::wstring(itor.c_str()) + TEXT(" Define�� �߸� �Ǿ����ϴ�.") + std::to_wstring(returnValue.iErrorRow) + TEXT(":") + std::to_wstring(returnValue.iErrorCol));
					GetDlgItem(IDC_STATIC_DATAFOLDER_PATH)->SetWindowText(szMessage.c_str());
					break;
				case XLSLOADER::INVALIED_COLNAME:
					szMessage.append(std::wstring(itor.c_str()) + TEXT("�������� �÷� �̸��� �߸��Ǿ����ϴ�.") + std::to_wstring(returnValue.iErrorRow) + TEXT(":") + std::to_wstring(returnValue.iErrorCol));
					GetDlgItem(IDC_STATIC_DATAFOLDER_PATH)->SetWindowText(szMessage.c_str());
					break;
				case XLSLOADER::INVALIED_VALUE:
					szMessage.append(std::wstring(itor.c_str()) + TEXT("���� �߸��Ǿ����ϴ�.") + std::to_wstring(returnValue.iErrorRow) + TEXT(":") + std::to_wstring(returnValue.iErrorCol));
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
	GetDlgItem(IDC_STATIC_DATAFOLDER_PATH)->SetWindowText(TEXT("��ȯ�� �Ϸ�Ǿ����ϴ�."));
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