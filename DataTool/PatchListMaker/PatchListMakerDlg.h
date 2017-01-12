
// PatchListMakerDlg.h : ��� ����
//

#pragma once
#include "afxwin.h"


// CPatchListMakerDlg ��ȭ ����
class CPatchListMakerDlg : public CDialogEx
{
// �����Դϴ�.
public:
	CPatchListMakerDlg(CWnd* pParent = NULL);	// ǥ�� �������Դϴ�.

// ��ȭ ���� �������Դϴ�.
#ifdef AFX_DESIGN_TIME
	enum { IDD = IDD_PATCHLISTMAKER_DIALOG };
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
	void GetWorkingFolderPath(void);
	void SetWorkingFolderPath(void);
	CString mPatchFolder;
public:
	afx_msg void OnBnClickedButtonChangefolder();
	afx_msg void OnBnClickedButtonMakepatchlist();
	CListBox mListDesc;
};
