
// PacketMaker.h : PROJECT_NAME ���� ���α׷��� ���� �� ��� �����Դϴ�.
//

#pragma once

#ifndef __AFXWIN_H__
	#error "PCH�� ���� �� ������ �����ϱ� ���� 'stdafx.h'�� �����մϴ�."
#endif

#include "resource.h"		// �� ��ȣ�Դϴ�.


// CPacketMakerApp:
// �� Ŭ������ ������ ���ؼ��� PacketMaker.cpp�� �����Ͻʽÿ�.
//

class CPacketMakerApp : public CWinApp
{
public:
	CPacketMakerApp();

// �������Դϴ�.
public:
	virtual BOOL InitInstance();

// �����Դϴ�.

	DECLARE_MESSAGE_MAP()
};

extern CPacketMakerApp theApp;