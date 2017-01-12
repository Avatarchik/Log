
// PacketMakerDlg.h : ��� ����
//

#pragma once


// CPacketMakerDlg ��ȭ ����
class CPacketMakerDlg : public CDialogEx
{
// �����Դϴ�.
public:
	CPacketMakerDlg(CWnd* pParent = NULL);	// ǥ�� �������Դϴ�.

// ��ȭ ���� �������Դϴ�.
#ifdef AFX_DESIGN_TIME
	enum { IDD = IDD_PACKETMAKER_DIALOG };
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
	afx_msg void OnBnClickedButtonMake();
	void GetWorkingFolderPath(void);
	void SetWorkingFolderPath(void);

protected:

public:
	CString mDataFolder;
	afx_msg void OnBnClickedButtonLoad();
	CString mServerExportFolder;
	CString mClientExportFolder;
};
