
// PatchListMaker.h : PROJECT_NAME ���� ���α׷��� ���� �� ��� �����Դϴ�.
//

#pragma once

#ifndef __AFXWIN_H__
	#error "PCH�� ���� �� ������ �����ϱ� ���� 'stdafx.h'�� �����մϴ�."
#endif

#include "resource.h"		// �� ��ȣ�Դϴ�.


// CPatchListMakerApp:
// �� Ŭ������ ������ ���ؼ��� PatchListMaker.cpp�� �����Ͻʽÿ�.
//

class CPatchListMakerApp : public CWinApp
{
public:
	CPatchListMakerApp();

// �������Դϴ�.
public:
	virtual BOOL InitInstance();

// �����Դϴ�.

	DECLARE_MESSAGE_MAP()
};

extern CPatchListMakerApp theApp;