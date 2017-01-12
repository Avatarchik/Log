
// DataChangerDlg.h : ��� ����
//

#pragma once

#include "ExcelLoader.h"
#include "afxwin.h"

// CDataChangerDlg ��ȭ ����
class CDataChangerDlg : public CDialogEx
{
// �����Դϴ�.
public:
	CDataChangerDlg(CWnd* pParent = NULL);	// ǥ�� �������Դϴ�.

// ��ȭ ���� �������Դϴ�.
#ifdef AFX_DESIGN_TIME
	enum { IDD = IDD_DATACHANGER_DIALOG };
#endif

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV �����Դϴ�.

// �����Դϴ�.
protected:
	HICON m_hIcon;

	// ������ �޽��� �� �Լ�
	virtual BOOL OnInitDialog();
	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnBnClickedButtonLoad();

protected:
	ExcelLoader mExecelLoader;
	

public:
	afx_msg void OnBnClickedButtonStart();
	void GetWorkingFolderPath(void);
	void SetWorkingFolderPath(void);


protected:
public:
	CListBox mListDescription;
	CString mDataFolder;
};
