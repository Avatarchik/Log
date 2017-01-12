
// PacketMakerDlg.cpp : ���� ����
//

#include "stdafx.h"
#include "PacketMaker.h"
#include "PacketMakerDlg.h"
#include "afxdialogex.h"
#include "FileManager.h"
#include "ExcelLoader.h"

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


// CPacketMakerDlg ��ȭ ����



CPacketMakerDlg::CPacketMakerDlg(CWnd* pParent /*=NULL*/)
	: CDialogEx(IDD_PACKETMAKER_DIALOG, pParent)
	, mDataFolder(_T(""))
	, mServerExportFolder(_T(""))
	, mClientExportFolder(_T(""))
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CPacketMakerDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
	DDX_Text(pDX, IDC_EDIT_DATAFOLDER_PATH, mDataFolder);
	DDX_Text(pDX, IDC_EDIT_SERVEREXPORT_PATH, mServerExportFolder);
	DDX_Text(pDX, IDC_EDIT_CLIENTEXPORT_PATH, mClientExportFolder);
}

BEGIN_MESSAGE_MAP(CPacketMakerDlg, CDialogEx)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_BN_CLICKED(ID_BUTTON_MAKE, &CPacketMakerDlg::OnBnClickedButtonMake)
	ON_BN_CLICKED(IDC_BUTTON_LOAD, &CPacketMakerDlg::OnBnClickedButtonLoad)
END_MESSAGE_MAP()


// CPacketMakerDlg �޽��� ó����

BOOL CPacketMakerDlg::OnInitDialog()
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

void CPacketMakerDlg::OnSysCommand(UINT nID, LPARAM lParam)
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

void CPacketMakerDlg::OnPaint()
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
HCURSOR CPacketMakerDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}



void CPacketMakerDlg::OnBnClickedButtonMake()
{
	UpdateData(TRUE);
	WCHAR szCurrentDirectory[MAX_PATH];
	memset(szCurrentDirectory, 0x00, MAX_PATH);
	GetCurrentDirectory(MAX_PATH, szCurrentDirectory);
	if (mDataFolder == "")
	{
		mDataFolder = szCurrentDirectory;
	}
	std::vector<std::wstring> tableFilesPath;
	FileManager fileManager;
	fileManager.Search(mDataFolder, TEXT(".xlsx"), tableFilesPath);

	std::wstring szDescription;
	for each (auto itor in tableFilesPath)
	{
		//int iLastListCount = mListDescription.GetCount();
		szDescription = TEXT("Find file : ") + itor;
		//mListDescription.AddString(szDescription.c_str());
	}

	size_t iFileNameCount;
	std::wstring szFileName;
	std::wstring szPIDL;
	std::wstring szUnityScript;
	std::wstring szPacketDef;
	//	CString szFileName = mDataFolderPath.Mid(mDataFolderPath.ReverseFind('\\'), iFileNameCount);

	std::vector<sTABLE_DEFINITION> dataEntry;
	ExcelLoader ExcelLoader;
	szPacketDef = TEXT("");
	std::wstring szMessage;
	for each (auto itor in tableFilesPath)
	{
		dataEntry.clear();
		szPIDL = TEXT("");
		szUnityScript = TEXT("");
		XLSLOADER::ReturnError returnValue = ExcelLoader.Open(itor.c_str(), szPIDL, szUnityScript, szPacketDef);
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
			case XLSLOADER::NOTFOUND_PACKETNAME:
				szMessage.append(std::wstring(itor.c_str()) + TEXT(" ������ ��Ŷ�̸� ��Ʈ�� �����.") + std::to_wstring(returnValue.errorRow) + TEXT(":") + std::to_wstring(returnValue.errorCol));
				GetDlgItem(IDC_STATIC_DATAFOLDER_PATH)->SetWindowText(szMessage.c_str());
				break;
			case XLSLOADER::NOTFOUND_PACKETNUMBER:
				szMessage.append(std::wstring(itor.c_str()) + TEXT(" ��Ŷ��ȣ�� ���� �ȵǸ� �ȵǿ�.") + std::to_wstring(returnValue.errorRow) + TEXT(":") + std::to_wstring(returnValue.errorCol));
				GetDlgItem(IDC_STATIC_DATAFOLDER_PATH)->SetWindowText(szMessage.c_str());
				break;
			case XLSLOADER::NOTMATCH_SHEETDEFINE:
				szMessage.append(std::wstring(itor.c_str()) + TEXT(" ������ ��Ŷ�̸��� ��Ʈ ������ �޶��.") + std::to_wstring(returnValue.errorRow) + TEXT(":") + std::to_wstring(returnValue.errorCol));
				GetDlgItem(IDC_STATIC_DATAFOLDER_PATH)->SetWindowText(szMessage.c_str());
				break;
			case XLSLOADER::INVALIED_PACKETSTARTNUMBER:
				szMessage.append(std::wstring(itor.c_str()) + TEXT(" ��Ŷ���۹�ȣ�� �̻��ؿ�.") + std::to_wstring(returnValue.errorRow) + TEXT(":") + std::to_wstring(returnValue.errorCol));
				GetDlgItem(IDC_STATIC_DATAFOLDER_PATH)->SetWindowText(szMessage.c_str());
				break;
			}
			return;
		}
		iFileNameCount = itor.rfind('.') - itor.rfind('\\') - 1;
		szFileName = itor.substr(itor.rfind('\\') + 1, iFileNameCount);
		fileManager.MakePIDL(mDataFolder.GetBuffer(), mServerExportFolder.GetBuffer(), szFileName, szPIDL.c_str(), returnValue.serverOnly);
		if (szUnityScript.size() != 0)
			fileManager.MakeUnityScript(mClientExportFolder.GetBuffer(), mDataFolder.GetBuffer(), szFileName, szUnityScript.c_str());
		if (szPacketDef.size() != 0)
			fileManager.MakePacketDef(mClientExportFolder.GetBuffer(), mDataFolder.GetBuffer(), szFileName, szPacketDef.c_str());
		szDescription = TEXT("Make Complete : ") + itor;
	}
	

	CString pidlFolder;
	pidlFolder.Format(TEXT("%s\\PIDL\\"), mDataFolder.GetString());
	std::vector<std::wstring> pidlFilesPath;
	fileManager.Search(pidlFolder, TEXT(".PIDL"), pidlFilesPath);

	CString pidlOutFolder;
	pidlOutFolder.Format(TEXT("%s\\unity\\"), mDataFolder.GetString());

//	PROCESS_INFORMATION processInfo;
//	STARTUPINFO startupInfo;
//	startupInfo.cb = sizeof(STARTUPINFO);
	std::wstring szPIDLExcutePath;
	szPIDLExcutePath.append(std::wstring(szCurrentDirectory) + TEXT("\\PIDL.exe"));

	for each (auto itor in pidlFilesPath)
	{
		std::wstring pidlRunOption;
		if (mClientExportFolder.IsEmpty() == true)
			pidlRunOption.append(TEXT("-cs ") + itor + TEXT(" -outdir ") + pidlOutFolder.GetString());
		else
			pidlRunOption.append(TEXT("-cs ") + itor + TEXT(" -outdir ") + mClientExportFolder.GetString());
		//::CreateProcess(TEXT("PIDL.exe"), (LPWSTR)pidlRunOption.c_str(), NULL, NULL, FALSE, 0, NULL, NULL, &startupInfo, &processInfo);
		ShellExecute(NULL, TEXT("open"), szPIDLExcutePath.c_str(), pidlRunOption.c_str(), NULL, SW_SHOW);
	}

	szMessage.append(std::to_wstring((int)tableFilesPath.size()) + TEXT("�� ������ ��ȯ�Ǿ����ϴ�."));
	GetDlgItem(IDC_STATIC_DATAFOLDER_PATH)->SetWindowText(szMessage.c_str());
}

void CPacketMakerDlg::GetWorkingFolderPath(void)
{
	DWORD dwType = REG_SZ;
	DWORD dwSize = 2048;
	HKEY hKey;
	LONG lResult;
	TCHAR csBuffer[2048];

	mDataFolder.Empty();
	lResult = RegOpenKeyEx(HKEY_CURRENT_USER, TEXT("SOFTWARE\\KairosNCo\\PacketMaker"), 0, KEY_READ, &hKey);
	if (lResult == ERROR_SUCCESS)
	{
		RegQueryValueEx(hKey, _T("WorkingFolder"), NULL, &dwType, (LPBYTE)csBuffer, &dwSize);
		mDataFolder.SetString(csBuffer);
		memset(csBuffer, 0x00, sizeof(csBuffer));
		dwSize = 2048;
		lResult = RegQueryValueEx(hKey, _T("ClientExportFolder"), NULL, &dwType, (LPBYTE)csBuffer, &dwSize);
		mClientExportFolder.SetString(csBuffer);
		memset(csBuffer, 0x00, sizeof(csBuffer));
		dwSize = 2048;
		RegQueryValueEx(hKey, _T("ServerExportFolder"), NULL, &dwType, (LPBYTE)csBuffer, &dwSize);
		mServerExportFolder.SetString(csBuffer);
		UpdateData(FALSE);
	}
}

void CPacketMakerDlg::SetWorkingFolderPath(void)
{
	UpdateData(TRUE);
	CRegKey regKey;
	regKey.Create(HKEY_CURRENT_USER, TEXT("SOFTWARE\\KairosNCo\\PacketMaker"));
	regKey.SetStringValue(_T("WorkingFolder"), mDataFolder.GetBuffer());
	regKey.SetStringValue(_T("ServerExportFolder"), mServerExportFolder.GetBuffer());
	regKey.SetStringValue(_T("ClientExportFolder"), mClientExportFolder.GetBuffer());
	regKey.Close();
}


void CPacketMakerDlg::OnBnClickedButtonLoad()
{
	SetWorkingFolderPath();
}
