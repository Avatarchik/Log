
// DataChanger.h : PROJECT_NAME ���� ���α׷��� ���� �� ��� �����Դϴ�.
//

#pragma once

#ifndef __AFXWIN_H__
	#error "PCH�� ���� �� ������ �����ϱ� ���� 'stdafx.h'�� �����մϴ�."
#endif

#include "resource.h"		// �� ��ȣ�Դϴ�.


// CDataChangerApp:
// �� Ŭ������ ������ ���ؼ��� DataChanger.cpp�� �����Ͻʽÿ�.
//

class CDataChangerApp : public CWinApp
{
public:
	CDataChangerApp();

// �������Դϴ�.
public:
	virtual BOOL InitInstance();

// �����Դϴ�.

	DECLARE_MESSAGE_MAP()
};

extern CDataChangerApp theApp;