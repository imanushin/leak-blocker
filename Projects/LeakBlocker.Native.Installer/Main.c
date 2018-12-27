//#pragma comment(linker,"\"/manifestdependency:type='win32' name='Microsoft.Windows.Common-Controls' version='6.0.0.0' processorArchitecture='*' publicKeyToken='6595b64144ccf1df' language='*'\"")

#define WIN32_LEAN_AND_MEAN

#include "../CommonProperties/version.h"   
#include "Data.h"
#include <windows.h>
#include <wininet.h>
#include <softpub.h>
#include <shlobj.h>

int _fltused = 0;
extern int WINAPI WinMain(HINSTANCE hinstExe, HINSTANCE hinstPrev, LPSTR pszCmdLine, int nCmdShow);
typedef void (__cdecl *_PVFV)();
#pragma data_seg(".CRT$XCA")
_PVFV __xc_a[] = { NULL };
#pragma data_seg(".CRT$XCZ")
_PVFV __xc_z[] = { NULL };
#pragma data_seg()  
#pragma comment(linker, "/merge:.CRT=.rdata")
#pragma function(memcpy, memset)
static _PVFV * pf_atexitlist = 0;
static unsigned max_atexitlist_entries = 0;
static unsigned cur_atexitlist_entries = 0;
void __cdecl WinMainCRTStartup() 
{
    int mainret;
    char *lpszCommandLine;
    STARTUPINFO StartupInfo;
    _PVFV * pfbegin = __xc_a;
    _PVFV * pfend = __xc_z;
    lpszCommandLine = GetCommandLine();
    if (*lpszCommandLine == '"') 
    {
        lpszCommandLine++; 
        while (*lpszCommandLine && (*lpszCommandLine != '"'))
            lpszCommandLine++;
        if (*lpszCommandLine == '"')
            lpszCommandLine++;
    }
    else
    {
        while (*lpszCommandLine > ' ')
            lpszCommandLine++;
    }
    while (*lpszCommandLine && (*lpszCommandLine <= ' '))
        lpszCommandLine++;
    StartupInfo.dwFlags = 0;
    GetStartupInfo(&StartupInfo);    
    max_atexitlist_entries = 32;
    pf_atexitlist = (_PVFV*)HeapAlloc(GetProcessHeap(), HEAP_ZERO_MEMORY, max_atexitlist_entries * sizeof(_PVFV*));
    while (pfbegin < pfend)
    {
        if (*pfbegin != NULL)
            (**pfbegin)();
        ++pfbegin;
    }
    mainret = WinMain(GetModuleHandle(NULL), NULL, lpszCommandLine, StartupInfo.dwFlags & STARTF_USESHOWWINDOW ? StartupInfo.wShowWindow : SW_SHOWDEFAULT );
    pfbegin = pf_atexitlist;
    pfend = pf_atexitlist + cur_atexitlist_entries;
    while (pfbegin < pfend)
    {
        if (*pfbegin != NULL)
            (**pfbegin)();
        ++pfbegin;
    }
    ExitProcess(mainret);
}
void * __cdecl memcpy(void *dst, const void *src, size_t count) 
{
    void * ret = dst;
    while (count--) 
    {
        *(char *)dst = *(char *)src;
        dst = (char *)dst + 1;
        src = (char *)src + 1;
    }
    return(ret);
}
void * __cdecl memset(void *dst, int val, size_t count) 
{
    void *start = dst;
    while (count--) 
    {
        *(char*)dst = (char)val;
        dst = (char *)dst + 1;
    }
    return(start);
}

typedef struct 
{
    HWND Window;
    HWND Text;
    HWND Button;
    SIZE Size;
} LICENSE_WINDOW, *PLICENSE_WINDOW;

LRESULT CALLBACK WindowProc(HWND hwnd, UINT uMsg, WPARAM wParam, LPARAM lParam);
LRESULT CALLBACK LicenseWindowProc(HWND hwnd, UINT uMsg, WPARAM wParam, LPARAM lParam);
DWORD WINAPI WorkerThread(LPVOID context);
VOID ShowError(LPCTSTR message);
VOID ButtonInstallClick();
HANDLE RunProcess(LPCTSTR file);
VOID DownloadFile(LPCTSTR link, LPCTSTR path, LONG step);
VOID RunProcessAndWait(LPCTSTR commandLine, LONG step);
VOID ShowLicense();
HWND InitializeWindow(LPCTSTR caption, LONG left, LONG top, LONG width, LONG height, LPCTSTR windowClass, LONG identifier, LONG style, LONG extendedStyle, HFONT font, HWND parent, HINSTANCE instance);
FLOAT Arctg(FLOAT value);
VOID WriteLogMessage(LPCSTR format, ...);

#define MAIN_WINDOW_CLASS TEXT("LeakBlockerInstaller_MainWindowClass")
#define LICENSE_WINDOW_CLASS TEXT("LeakBlockerInstaller_LicenseWindowClass")

#define PROGRESS_STEP_LENGTH 1000
#define PROGRESS_STEPS      7

#define BUFFER_SIZE 65536

#define PROCESS_WAIT_STEP 1000
#define PROCESS_TIMEOUT (30 * 60 * 1000)

#define FileExists(path) ((GetFileAttributes(path) != INVALID_FILE_ATTRIBUTES) || ((GetLastError() != ERROR_PATH_NOT_FOUND) && (GetLastError() != ERROR_FILE_NOT_FOUND)))
#define FolderExists(path) FileExists(path) 

HWND mainWindow;
HWND labelWelcome;
HWND labelRequirements;
HWND buttonInstall;
HWND linkLicense;
HWND progressInstall;
HWND labelInstall;
HWND buttonClose;
HWND labelResult;
HWND imageResult;

HANDLE workerThread;

LONG currentStep;
BOOL started;
BOOL completed;

SIZE dpi = { 96, 96 };

CRITICAL_SECTION logCriticalSection;
TCHAR logFile[65536];
TCHAR logBuffer[65536];

#define ScaleX(x) MulDiv(x, dpi.cx, 96)
#define ScaleY(y) MulDiv(y, dpi.cy, 96)

HFONT font;
HFONT largeFont;

OSVERSIONINFOEX systemVersion = { sizeof(OSVERSIONINFOEX) };

#define CONTROL_COLOR (HBRUSH)(COLOR_BTNFACE + 1) // (HBRUSH)GetStockObject(WHITE_BRUSH)

int CALLBACK WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
{
    WNDCLASSEX windowClass = {0};
    MSG message = {0};
    LONG mainWindowPositionX;
    LONG mainWindowPositionY;
    HDC deviceContext = GetDC(NULL);
    LOGFONT logicalFont = {0};
    HANDLE largeIcon;
    INITCOMMONCONTROLSEX controlsInitialization = { sizeof(INITCOMMONCONTROLSEX) };
    ICONINFO iconData = {0};
    TCHAR buffer[65536];

    UNREFERENCED_PARAMETER(nCmdShow);
    UNREFERENCED_PARAMETER(lpCmdLine);
    UNREFERENCED_PARAMETER(hPrevInstance);
 
    InitCommonControlsEx(&controlsInitialization);

    InitializeCriticalSection(&logCriticalSection);
        
    if(ExpandEnvironmentStrings("%ALLUSERSPROFILE%\\Delta Corvi", logFile, 65536) == 0)
        ShowError(ERROR_UNKNOWN);
    if(!FolderExists(logFile))
        CreateDirectory(logFile, NULL);    
    ZeroMemory(logFile, sizeof(logFile));

    if(ExpandEnvironmentStrings(TEXT("%ALLUSERSPROFILE%\\Delta Corvi\\Logs"), logFile, 65536) == 0)
        ShowError(ERROR_UNKNOWN);
    if(!FolderExists(logFile))
        CreateDirectory(logFile, NULL);
    ZeroMemory(logFile, sizeof(logFile));
    ExpandEnvironmentStrings(TEXT("%ALLUSERSPROFILE%\\Delta Corvi\\Logs\\LeakBlockerNativeInstaller.log"), logFile, 65536);

    WriteLogMessage(TEXT("Installer started."));
    
    ZeroMemory(buffer, sizeof(buffer));            
    if(GetEnvironmentVariable(TEXT("ProgramW6432"), buffer, 65536) == 0)
    {
        ZeroMemory(buffer, sizeof(buffer));     
        if(GetEnvironmentVariable(TEXT("PROGRAMFILES"), buffer, 65536) == 0)
        {
            WriteLogMessage(TEXT("GetEnvironmentVariable failed with error %1!u!."), GetLastError());
            ShowError(ERROR_UNKNOWN);
        }
    }

    if(!SetEnvironmentVariable(PROGRAMFILES_PATH, buffer))
    {
        WriteLogMessage(TEXT("SetEnvironmentVariable failed with error %1!u!."), GetLastError());
        ShowError(ERROR_UNKNOWN);
    }
    
    ZeroMemory(buffer, sizeof(buffer));  
    if(ExpandEnvironmentStrings(COMPANY_FOLDER, buffer, 65536) == 0)
        ShowError(ERROR_UNKNOWN);
    if(!FolderExists(buffer))
        CreateDirectory(buffer, NULL);    
    ZeroMemory(buffer, sizeof(buffer));

    if(!GetVersionEx((LPOSVERSIONINFO)&systemVersion))
        WriteLogMessage(TEXT("GetVersionEx failed with error %1!u!."), GetLastError());
        
    GetObject(GetStockObject(DEFAULT_GUI_FONT), sizeof(LOGFONT), &logicalFont); 
    font = CreateFontIndirect(&logicalFont);
    logicalFont.lfHeight = (LONG)(logicalFont.lfHeight * 1.5f);
    largeFont = CreateFontIndirect(&logicalFont);

    if (deviceContext)
    {
        dpi.cx = GetDeviceCaps(deviceContext, LOGPIXELSX);
        dpi.cy = GetDeviceCaps(deviceContext, LOGPIXELSY);
        ReleaseDC(NULL, deviceContext);
    }

    mainWindowPositionX = GetSystemMetrics(SM_CXSCREEN) / 2 - ScaleX(MAIN_WINDOW_WIDTH) / 2;
    mainWindowPositionY = GetSystemMetrics(SM_CYSCREEN) / 2 - ScaleY(MAIN_WINDOW_HEIGHT)/ 2;

    windowClass.cbSize        = sizeof(WNDCLASSEX);
    windowClass.lpfnWndProc   = WindowProc;
    windowClass.hInstance     = hInstance;
    windowClass.hCursor       = LoadCursor(NULL, IDC_ARROW);
    windowClass.hbrBackground = CONTROL_COLOR;
    windowClass.lpszClassName = MAIN_WINDOW_CLASS;
    windowClass.hIcon         = LoadIcon(hInstance, MAKEINTRESOURCE(2));
    windowClass.hIconSm       = LoadIcon(hInstance, MAKEINTRESOURCE(2));
    
    if(!RegisterClassEx(&windowClass))
        ShowError(ERROR_UNKNOWN);
    
    ZeroMemory(&windowClass, sizeof(windowClass));

    windowClass.cbSize        = sizeof(WNDCLASSEX);
    windowClass.lpfnWndProc   = LicenseWindowProc;
    windowClass.hCursor       = LoadCursor(NULL, IDC_ARROW);
    windowClass.hbrBackground = CONTROL_COLOR;
    windowClass.lpszClassName = LICENSE_WINDOW_CLASS;

    if(!RegisterClassEx(&windowClass))
    {
        WriteLogMessage(TEXT("RegisterClassEx failed with error %1!u!."), GetLastError());
        ShowError(ERROR_UNKNOWN);
    }

    mainWindow = InitializeWindow(MAIN_WINDOW_TITLE, mainWindowPositionX, mainWindowPositionY, MAIN_WINDOW_WIDTH, MAIN_WINDOW_HEIGHT, MAIN_WINDOW_CLASS, 0, MAIN_WINDOW_STYLE, MAIN_WINDOW_EXTENDED_STYLE, font, NULL, hInstance);
    labelWelcome = InitializeWindow(LABEL_WELCOME_CAPTION, LABEL_WELCOME_LEFT, LABEL_WELCOME_TOP, LABEL_WELCOME_WIDTH, LABEL_WELCOME_HEIGHT, TEXT("Static"), LABEL_WELCOME_IDENTIFIER, LABEL_WELCOME_STYLE, 0, largeFont, mainWindow, NULL);
    labelRequirements = InitializeWindow(LABEL_REQUIREMENTS_CAPTION, LABEL_REQUIREMENTS_LEFT, LABEL_REQUIREMENTS_TOP, LABEL_REQUIREMENTS_WIDTH, LABEL_REQUIREMENTS_HEIGHT, TEXT("Static"), LABEL_REQUIREMENTS_IDENTIFIER, LABEL_REQUIREMENTS_STYLE, 0, font, mainWindow, NULL);
    linkLicense = InitializeWindow(LINK_LICENSE_CAPTION, LINK_LICENSE_LEFT, LINK_LICENSE_TOP, LINK_LICENSE_WIDTH, LINK_LICENSE_HEIGHT, TEXT("SysLink"), LINK_LICENSE_IDENTIFIER, LINK_LICENSE_STYLE, 0, font, mainWindow, NULL);
    buttonInstall = InitializeWindow(BUTTON_INSTALL_CAPTION, BUTTON_INSTALL_LEFT, BUTTON_INSTALL_TOP, BUTTON_INSTALL_WIDTH, BUTTON_INSTALL_HEIGHT, TEXT("Button"), BUTTON_INSTALL_IDENTIFIER, BUTTON_INSTALL_STYLE, 0, largeFont, mainWindow, NULL);
    buttonClose = InitializeWindow(BUTTON_CLOSE_CAPTION, BUTTON_CLOSE_LEFT, BUTTON_CLOSE_TOP, BUTTON_CLOSE_WIDTH, BUTTON_CLOSE_HEIGHT, TEXT("Button"), BUTTON_CLOSE_IDENTIFIER, BUTTON_CLOSE_STYLE, 0, font, mainWindow, NULL);
    progressInstall = InitializeWindow(TEXT(""), PROGRESS_INSTALL_LEFT, PROGRESS_INSTALL_TOP, PROGRESS_INSTALL_WIDTH, PROGRESS_INSTALL_HEIGHT, TEXT("msctls_progress32"), PROGRESS_INSTALL_IDENTIFIER, PROGRESS_INSTALL_STYLE, 0, font, mainWindow, NULL);
    labelInstall = InitializeWindow(TEXT(""), LABEL_INSTALL_LEFT, LABEL_INSTALL_TOP, LABEL_INSTALL_WIDTH, LABEL_INSTALL_HEIGHT, TEXT("Static"), LABEL_INSTALL_IDENTIFIER, LABEL_INSTALL_STYLE, 0, font, mainWindow, NULL);
    labelResult = InitializeWindow(LABEL_RESULT_CAPTION, LABEL_RESULT_LEFT, LABEL_RESULT_TOP, LABEL_RESULT_WIDTH, LABEL_RESULT_HEIGHT, TEXT("Static"), LABEL_RESULT_IDENTIFIER, LABEL_RESULT_STYLE, 0, font, mainWindow, NULL);
    imageResult = InitializeWindow(TEXT(""), IMAGE_RESULT_LEFT, IMAGE_RESULT_TOP, IMAGE_RESULT_WIDTH, IMAGE_RESULT_HEIGHT, TEXT("Static"), IMAGE_RESULT_IDENTIFIER, IMAGE_RESULT_STYLE, 0, font, mainWindow, NULL);

    if(!RemoveMenu(GetSystemMenu(mainWindow, FALSE), SC_MAXIMIZE, MF_BYCOMMAND))
        WriteLogMessage(TEXT("RemoveMenu failed with error %1!u!."), GetLastError());
    if(!RemoveMenu(GetSystemMenu(mainWindow, FALSE), SC_SIZE, MF_BYCOMMAND))
        WriteLogMessage(TEXT("RemoveMenu failed with error %1!u!."), GetLastError());
    if(!RemoveMenu(GetSystemMenu(mainWindow, FALSE), SC_RESTORE, MF_BYCOMMAND))
        WriteLogMessage(TEXT("RemoveMenu failed with error %1!u!."), GetLastError());

    largeIcon = LoadImage(hInstance, MAKEINTRESOURCE(2), IMAGE_ICON, LOGO_SIZE, LOGO_SIZE, LR_SHARED | LR_LOADTRANSPARENT | LR_CREATEDIBSECTION);
    if(largeIcon == NULL)
    {
        WriteLogMessage(TEXT("Using small icons."));
        largeIcon = LoadImage(hInstance, MAKEINTRESOURCE(2), IMAGE_ICON, 64, 64, LR_SHARED | LR_LOADTRANSPARENT | LR_CREATEDIBSECTION);
        if(largeIcon == NULL)
            WriteLogMessage(TEXT("LoadImage failed with error %1!u!."), GetLastError());     
    }

    GetIconInfo((HICON)largeIcon, &iconData);
    largeIcon = iconData.hbmColor;

    SendMessage(imageResult, STM_SETIMAGE, IMAGE_BITMAP, (LPARAM)largeIcon);

    if(!RedrawWindow(mainWindow, NULL, NULL, RDW_ALLCHILDREN | RDW_ERASE | RDW_INVALIDATE | RDW_ERASENOW | RDW_UPDATENOW))
        WriteLogMessage(TEXT("RedrawWindow failed with error %1!u!."), GetLastError());
    SendMessage(progressInstall, PBM_SETRANGE, (WPARAM)NULL, MAKELPARAM(0, PROGRESS_STEP_LENGTH * PROGRESS_STEPS));
    
    if ((systemVersion.dwMajorVersion * 10 + systemVersion.dwMinorVersion + systemVersion.wServicePackMajor) < 54)
    {
        WriteLogMessage(TEXT("System is not supported (%1!u! %2!u! %3!u!)."), systemVersion.dwMajorVersion, systemVersion.dwMinorVersion, systemVersion.wServicePackMajor);
        ShowError(ERROR_UNSUPPORTED);
    }

    while(GetMessage(&message, NULL, 0, 0) > 0)
    {
        TranslateMessage(&message);
        DispatchMessage(&message);
    }
    
    WriteLogMessage(TEXT("Installer stopped.\r\n"));

    DeleteCriticalSection(&logCriticalSection);
    return 0;
}

HWND InitializeWindow(LPCTSTR caption, LONG left, LONG top, LONG width, LONG height, LPCTSTR windowClass, LONG identifier, LONG style, LONG extendedStyle, HFONT font, HWND parent, HINSTANCE instance)
{
    RECT rect;

    HWND result = CreateWindowEx(extendedStyle, windowClass, caption, style, ScaleX(left), ScaleY(top), ScaleX(width), ScaleY(height), parent, (HMENU)identifier, instance, NULL);
    if(result == NULL)
    {
        WriteLogMessage(TEXT("CreateWindowEx failed with error %1!u!."), GetLastError());

        WriteLogMessage(TEXT("%1!s!."), windowClass);
        

        ShowError(ERROR_UNKNOWN);
    }
    SendMessage(result, WM_SETFONT, (WPARAM)font, (LPARAM)TRUE);

    if(instance != NULL)
    {
        if(!GetClientRect(result, &rect))
            WriteLogMessage(TEXT("GetClientRect failed with error %1!u!."), GetLastError());
        else if(!SetWindowPos(result, 0, 0, 0, 2 * ScaleX(width) - rect.right, 2 * ScaleY(height) - rect.bottom, SWP_NOMOVE | SWP_NOOWNERZORDER | SWP_NOZORDER))
            WriteLogMessage(TEXT("SetWindowPos failed with error %1!u!."), GetLastError());
    }

    return result;
}

LRESULT CALLBACK WindowProc(HWND hwnd, UINT uMsg, WPARAM wParam, LPARAM lParam)
{
    DWORD waitResult;

    switch (uMsg)
    {
    case WM_CTLCOLORBTN:
    case WM_CTLCOLORSTATIC:
        SetBkMode((HDC)wParam, TRANSPARENT);
        return (LRESULT)CONTROL_COLOR;

    case WM_CLOSE:   
        WriteLogMessage(TEXT("Closing installer."));
        if(workerThread != NULL)
        {
            waitResult = WaitForSingleObject(workerThread, 0);
            if((waitResult != WAIT_OBJECT_0) && (waitResult != WAIT_TIMEOUT))
            {
                WriteLogMessage(TEXT("WaitForSingleObject failed with error %1!u!."), GetLastError());
                ShowError(ERROR_UNKNOWN);
            }
            if(waitResult == WAIT_TIMEOUT)
            {
                WriteLogMessage(TEXT("Waiting for confirmation."));
                if(MessageBox(NULL, CONFIRMATION_CANCEL, ERROR_DIALOG_CAPTION, MB_ICONQUESTION | MB_YESNO) == IDNO)
                {
                    WriteLogMessage(TEXT("User cancelled closing."));
                    return 0;
                }
            }
            else
                WriteLogMessage(TEXT("Confirmation is not required."));
        }
        WriteLogMessage(TEXT("Closing the window."));
        DestroyWindow(hwnd);
        return 0;
    case WM_KEYDOWN:
        if(wParam != VK_RETURN)
            break;
        if(completed)
        {
            SendMessage(mainWindow, WM_COMMAND, BUTTON_CLOSE_IDENTIFIER, 0);
            return 0;
        }
        if(!started)
        {
            SendMessage(mainWindow, WM_COMMAND, BUTTON_INSTALL_IDENTIFIER, 0);
            return 0;
        }
        break;

    case WM_DESTROY:
        PostQuitMessage(0);
        ExitProcess(0);
        //return 0;

    case WM_COMMAND:
        switch(wParam)
        {
        case BUTTON_INSTALL_IDENTIFIER:
            ButtonInstallClick();
            return 0;

        case BUTTON_CLOSE_IDENTIFIER:
            if(completed)
                RunProcess(APPLICATION_COMMANDLINE);
            SendMessage(hwnd, WM_CLOSE, 0, 0);
            return 0;
        }
        break;

    case WM_NOTIFY:
        if((wParam == LINK_LICENSE_IDENTIFIER) && ((NMHDR*)lParam)->code == NM_CLICK)
        {
            ShowLicense();
            return 0;
        }
        break;
    }
    return DefWindowProc(hwnd, uMsg, wParam, lParam);
}

VOID ShowError(LPCTSTR message)
{
    MessageBox(mainWindow, message, ERROR_DIALOG_CAPTION, MB_ICONERROR | MB_OK);
    ExitProcess(1);
}

VOID DownloadFile(LPCTSTR link, LPCTSTR path, LONG step)
{
    DWORD fileSize;
    DWORD dataSize;
    DWORD readBytes = 0;
    HANDLE fileHandle;
    TCHAR filePath[65536];
    WCHAR unicodeFilePath[65536];
    WINTRUST_FILE_INFO fileData = {0};
    WINTRUST_DATA winTrustData = {0}; 
    GUID policy = WINTRUST_ACTION_GENERIC_VERIFY_V2;
    LONG status;  
    BYTE buffer[BUFFER_SIZE];  
#ifdef WEB
    HINTERNET internetHandle;
    HINTERNET linkHandle; 
#else
    HRSRC resourceHandle;
    HGLOBAL dataHandle;
    LPBYTE dataPointer;  
#endif
    
    WriteLogMessage(TEXT("File downloading started."));

    ZeroMemory(filePath, sizeof(filePath));
    if(ExpandEnvironmentStrings(PRODUCT_FOLDER, filePath, 65536) == 0)
    {
        WriteLogMessage(TEXT("ExpandEnvironmentStrings failed with error %1!u!."), GetLastError());
        ShowError(ERROR_UNKNOWN);
    }
    if(!FolderExists(filePath))
        CreateDirectory(filePath, NULL);    
    ZeroMemory(filePath, sizeof(filePath));
    if(ExpandEnvironmentStrings(path, filePath, 65536) == 0)
    {
        WriteLogMessage(TEXT("ExpandEnvironmentStrings failed with error %1!u!."), GetLastError());
        ShowError(ERROR_UNKNOWN);
    }

    if(FileExists(filePath))
    {        
        WriteLogMessage(TEXT("File already exists. Checking signature."));

        ZeroMemory(unicodeFilePath, sizeof(unicodeFilePath));
#ifdef UNICODE
        CopyMemory(unicodeFilePath, filePath, min(sizeof(filePath), sizeof(unicodeFilePath)));
#else
        if(MultiByteToWideChar(CP_ACP, MB_COMPOSITE, filePath, -1, unicodeFilePath, 65536) == 0)
        {
            WriteLogMessage(TEXT("MultiByteToWideChar failed with error %1!u!."), GetLastError());
            ShowError(ERROR_UNKNOWN);
        }
#endif
        fileData.cbStruct = sizeof(fileData);
        fileData.pcwszFilePath = unicodeFilePath;
           
        winTrustData.cbStruct = sizeof(winTrustData);    
        winTrustData.dwUIChoice = WTD_UI_NONE;
        winTrustData.dwUnionChoice = WTD_CHOICE_FILE;
        winTrustData.pFile = &fileData;
        
        status = WinVerifyTrust(NULL, &policy, &winTrustData);
        WriteLogMessage(TEXT("Signature check result is %1!u!."), status);
        if((status == ERROR_SUCCESS) || (status == TRUST_E_EXPLICIT_DISTRUST) || (status == TRUST_E_SUBJECT_NOT_TRUSTED) || (status == CRYPT_E_SECURITY_SETTINGS))
        {
            SendMessage(progressInstall, PBM_SETPOS, (WPARAM)(step * PROGRESS_STEP_LENGTH), (LPARAM)NULL);
            WriteLogMessage(TEXT("Signature is valid. Downloading is not required."));
            return;
        }
    }
        
    WriteLogMessage(TEXT("Creating file."));
    fileHandle = CreateFile(filePath, GENERIC_WRITE, 0, NULL, CREATE_ALWAYS, 0, NULL);
    if(fileHandle == INVALID_HANDLE_VALUE)
    {
        WriteLogMessage(TEXT("CreateFile failed with error %1!u!."), GetLastError());
        ShowError(ERROR_UNKNOWN);
    }
    
#ifdef WEB
    internetHandle = InternetOpen(TEXT("Leak Blocker Native Installer"), INTERNET_OPEN_TYPE_PRECONFIG, NULL, NULL, 0);
    if(internetHandle == NULL)
    {
        WriteLogMessage(TEXT("InternetOpen failed with error %1!u!."), GetLastError());
        ShowError(ERROR_UNKNOWN);
    }

    linkHandle = InternetOpenUrl(internetHandle, link, NULL, 0, INTERNET_FLAG_EXISTING_CONNECT, (DWORD_PTR)NULL);
    if(linkHandle == NULL)
    {
        WriteLogMessage(TEXT("InternetOpenUrl failed with error %1!u!."), GetLastError());
        ShowError(ERROR_NETWORK);
    }
    
    dataSize = sizeof(fileSize);
    if(!HttpQueryInfo(linkHandle, HTTP_QUERY_CONTENT_LENGTH | HTTP_QUERY_FLAG_NUMBER, &fileSize, &dataSize, NULL))
    {
        WriteLogMessage(TEXT("HttpQueryInfo failed with error %1!u!."), GetLastError());
        ShowError(ERROR_NETWORK);    
    }
#else
    resourceHandle = FindResource(NULL, link, RT_RCDATA);
    if(resourceHandle == NULL)
    {
        WriteLogMessage(TEXT("FindResource failed with error %1!u!."), GetLastError());
        ShowError(ERROR_UNKNOWN);
    }

    dataHandle = LoadResource(NULL, resourceHandle);
    if(dataHandle == NULL)
    {
        WriteLogMessage(TEXT("LoadResource failed with error %1!u!."), GetLastError());
        ShowError(ERROR_UNKNOWN);
    }

    fileSize = SizeofResource(NULL, resourceHandle);
    if(fileSize == 0)
    {
        WriteLogMessage(TEXT("SizeofResource failed with error %1!u!."), GetLastError());
        ShowError(ERROR_UNKNOWN);
    }

    dataPointer = (LPBYTE)LockResource(dataHandle);
    if(dataPointer == NULL)
    {
        WriteLogMessage(TEXT("LockResource failed with error %1!u!."), GetLastError());
        ShowError(ERROR_UNKNOWN);
    }
#endif
    
    WriteLogMessage(TEXT("Starting writing."));
    while(readBytes < fileSize)
    {
        ZeroMemory(buffer, sizeof(buffer));

#ifdef WEB
        if(!InternetReadFile(linkHandle, buffer, BUFFER_SIZE, &dataSize))
        {
            WriteLogMessage(TEXT("InternetReadFile failed with error %1!u!."), GetLastError());
            ShowError(ERROR_NETWORK);
        }
#else
        dataSize = min(BUFFER_SIZE, (fileSize - readBytes));
        CopyMemory(buffer, &dataPointer[readBytes], dataSize);
#endif
        
        if(dataSize <= 0)
            break;

        if(!WriteFile(fileHandle, buffer, dataSize, &dataSize, NULL))
        {
            WriteLogMessage(TEXT("WriteFile failed with error %1!u!."), GetLastError());
            ShowError(ERROR_UNKNOWN);
        }
        readBytes += dataSize;

        SendMessage(progressInstall, PBM_SETPOS, (WPARAM)((step - 1) * PROGRESS_STEP_LENGTH + (LONG)((float)readBytes / (float)fileSize * 1000.0f)), (LPARAM)NULL);
    }
    
    CloseHandle(fileHandle);
    WriteLogMessage(TEXT("File downloading completed."));
}

DWORD WINAPI WorkerThread(LPVOID context)
{
    HKEY key;
    DWORD value;
    DWORD dataSize = sizeof(value);
    BOOL wicInstalled = FALSE;
    BOOL netFrameworkInstalled = FALSE;
    LSTATUS error;
    BOOL is64;

    UNREFERENCED_PARAMETER(context);
    
    WriteLogMessage(TEXT("Installer thread started."));

    if(!IsWow64Process(GetCurrentProcess(), &is64))
        WriteLogMessage(TEXT("IsWow64Process failed with error %1!u!."), GetLastError());

    WriteLogMessage(TEXT("Checking windows imaging component."));
    error = RegOpenKeyEx(HKEY_LOCAL_MACHINE, TEXT("SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Client"), 0, KEY_QUERY_VALUE, &key);
    if(error == ERROR_SUCCESS)
    {
        error = RegQueryValueEx(key, TEXT("Install"), NULL, NULL, (LPBYTE)&value, &dataSize);
        if(error == ERROR_SUCCESS)
        {
            WriteLogMessage(TEXT("Registry value is %1!u!."), value);
            netFrameworkInstalled = (value == 1);
        }
        else
            WriteLogMessage(TEXT("RegQueryValueEx failed with error %1!u!."), error);
    }
    else
        WriteLogMessage(TEXT("RegOpenKeyEx failed with error %1!u!."), error);

    if(systemVersion.dwMajorVersion >= 6)
        wicInstalled = TRUE;
    else
    {
        WriteLogMessage(TEXT("Checking net framework."));
        error = RegOpenKeyEx(HKEY_LOCAL_MACHINE, TEXT("SOFTWARE\\Microsoft\\Windows Imaging Component"), 0, KEY_QUERY_VALUE, &key);
        if(error == ERROR_SUCCESS)
        {
            error = RegQueryValueEx(key, TEXT("InstalledVersion"), NULL, NULL, (LPBYTE)&value, &dataSize);
            if((error == ERROR_SUCCESS) || (error == ERROR_MORE_DATA))
            {
                WriteLogMessage(TEXT("Registry exists."), value);
                wicInstalled = TRUE;
            }
            else
                WriteLogMessage(TEXT("RegQueryValueEx failed with error %1!u!."), error);
        }
        else
            WriteLogMessage(TEXT("RegOpenKeyEx failed with error %1!u!."), error);
    }
    
    WriteLogMessage(TEXT("Downloading WIC64."));
    currentStep = 1;
    if(!SetWindowText(labelInstall, LABEL_INSTALL_CAPTION_1))
        WriteLogMessage(TEXT("SetWindowText failed with error %1!u!."), GetLastError());
    DownloadFile(WIC64_DOWNLOAD_LINK, WIC64_FILE, currentStep);

    WriteLogMessage(TEXT("Downloading WIC32."));
    currentStep = 2;
    if(!SetWindowText(labelInstall, LABEL_INSTALL_CAPTION_1))
        WriteLogMessage(TEXT("SetWindowText failed with error %1!u!."), GetLastError());
    DownloadFile(WIC32_DOWNLOAD_LINK, WIC32_FILE, currentStep);

    WriteLogMessage(TEXT("Downloading net framework."));
    currentStep = 3;
    if(!SetWindowText(labelInstall, LABEL_INSTALL_CAPTION_2))
        WriteLogMessage(TEXT("SetWindowText failed with error %1!u!."), GetLastError());
    DownloadFile(NET_FRAMEWORK_DOWNLOAD_LINK, NET_FRAMEWORK_FILE, currentStep);
    
    WriteLogMessage(TEXT("Downloading program module."));
    currentStep = 4;
    if(!SetWindowText(labelInstall, LABEL_INSTALL_CAPTION_3))
        WriteLogMessage(TEXT("SetWindowText failed with error %1!u!."), GetLastError());
    DownloadFile(MAIN_MODULE_DOWNLOAD_LINK, MAIN_MODULE_FILE, currentStep);

    if(!wicInstalled)
    {
        currentStep = 5;
        if(!SetWindowText(labelInstall, LABEL_INSTALL_CAPTION_4))
            WriteLogMessage(TEXT("SetWindowText failed with error %1!u!."), GetLastError());
        if(!netFrameworkInstalled)
        {
            WriteLogMessage(TEXT("Installing WIC."));
            RunProcessAndWait(is64 ? WIC64_COMMANDLINE : WIC32_COMMANDLINE, currentStep);
        }
        else
            SendMessage(progressInstall, PBM_SETPOS, (WPARAM)(currentStep * PROGRESS_STEP_LENGTH), (LPARAM)NULL);
    }

    currentStep = 6;
    if(!SetWindowText(labelInstall, LABEL_INSTALL_CAPTION_5))
        WriteLogMessage(TEXT("SetWindowText failed with error %1!u!."), GetLastError());
    if(!netFrameworkInstalled)
    {
        WriteLogMessage(TEXT("Installing net framework."));
        RunProcessAndWait(NET_FRAMEWORK_COMMANDLINE, currentStep);
    }
    else
        SendMessage(progressInstall, PBM_SETPOS, (WPARAM)(currentStep * PROGRESS_STEP_LENGTH), (LPARAM)NULL);
    
    WriteLogMessage(TEXT("Installing product."));
    currentStep = 7;
    if(!SetWindowText(labelInstall, LABEL_INSTALL_CAPTION_6))
        WriteLogMessage(TEXT("SetWindowText failed with error %1!u!."), GetLastError());
    RunProcessAndWait(MAIN_MODULE_COMMANDLINE, currentStep);
        
    ShowWindow(progressInstall, SW_HIDE);
    ShowWindow(labelInstall, SW_HIDE);
     
    ShowWindow(imageResult, SW_SHOW);
    ShowWindow(labelResult, SW_SHOW);

    SetWindowText(buttonClose, BUTTON_CLOSE_CAPTION_ALTERNATIVE);
    completed = TRUE;
    
    WriteLogMessage(TEXT("Installer thread stopped."));
    return 0;
}

VOID RunProcessAndWait(LPCTSTR commandLine, LONG step)
{
    HANDLE process;
    LONG totalTime = 0;
    DWORD waitResult;
    DWORD exitCode;

    process = RunProcess(commandLine);

    WriteLogMessage(TEXT("Waiting for process."));

    while(totalTime < PROCESS_TIMEOUT)
    {
        waitResult = WaitForSingleObject(process, PROCESS_WAIT_STEP);
        if(waitResult == WAIT_OBJECT_0)
        {
            SendMessage(progressInstall, PBM_SETPOS, (WPARAM)(step * PROGRESS_STEP_LENGTH), (LPARAM)NULL);

            if(!GetExitCodeProcess(process, &exitCode))
            {
                WriteLogMessage(TEXT("GetExitCodeProcess failed with error %1!u!."), GetLastError());
                ShowError(ERROR_UNKNOWN);
            }

            if(exitCode != 0)
            {
                WriteLogMessage(TEXT("External process returned %1!u!."), exitCode);
                ShowError(ERROR_EXTERNAL_INSTALLER);
            }
            else
                WriteLogMessage(TEXT("External process completed successfully."));

            break;
        }

        if(waitResult != WAIT_TIMEOUT)
        {
            WriteLogMessage(TEXT("WaitForSingleObject failed with error %1!u!."), GetLastError());
            ShowError(ERROR_UNKNOWN);
        }

        totalTime += PROCESS_WAIT_STEP;
        SendMessage(progressInstall, PBM_SETPOS, (WPARAM)((step - 1) * PROGRESS_STEP_LENGTH + (LONG)(Arctg((float)totalTime / 30000.0f) / (3.141592654f / 2.0f) * 1000.0f)), (LPARAM)NULL);
    }

    WriteLogMessage(TEXT("Process completed."));
}

HANDLE RunProcess(LPCTSTR commandLine)
{
    TCHAR buffer[65536];
    STARTUPINFO processStartInformation = {0};
    PROCESS_INFORMATION processInformation = {0};

#ifndef UNICODE
    WriteLogMessage(TEXT("Starting process %1!s!."), commandLine);
#else
    WriteLogMessage(TEXT("Starting process %1!ws!."), commandLine);
#endif
    
    ZeroMemory(buffer, sizeof(buffer));
    processStartInformation.cb = sizeof(processStartInformation);
    
    if(ExpandEnvironmentStrings(commandLine, buffer, 65536) == 0)
    {
        WriteLogMessage(TEXT("ExpandEnvironmentStrings failed with error %1!u!."), GetLastError());
        ShowError(ERROR_UNKNOWN);
    }

    if(!CreateProcess(NULL, buffer, NULL, NULL, FALSE, 0, NULL, NULL, &processStartInformation, &processInformation))
    {
        WriteLogMessage(TEXT("CreateProcess failed with error %1!u!."), GetLastError());
        ShowError(ERROR_UNKNOWN);
    }

    return processInformation.hProcess;
}

VOID ButtonInstallClick()
{
    started = TRUE;

    ShowWindow(labelRequirements, SW_HIDE);
    ShowWindow(labelWelcome, SW_HIDE);
    ShowWindow(buttonInstall, SW_HIDE);
    ShowWindow(linkLicense, SW_HIDE);
    
    ShowWindow(progressInstall, SW_SHOW);
    ShowWindow(labelInstall, SW_SHOW);
    ShowWindow(buttonClose, SW_SHOW);
    
    WriteLogMessage(TEXT("Starting installation."));

    workerThread = CreateThread(NULL, 0, WorkerThread, NULL, 0, NULL);
    if(workerThread == NULL)
    {
        WriteLogMessage(TEXT("CreateThread failed with error %1!u!."), GetLastError());
        ShowError(ERROR_UNKNOWN);
    }
}

VOID ShowLicense()
{
    MSG message = {0};
    LONG licenseWindowPositionX = (GetSystemMetrics(SM_CXSCREEN) - LICENSE_WINDOW_WIDTH) / 2;
    LONG licenseWindowPositionY = (GetSystemMetrics(SM_CYSCREEN) - LICENSE_WINDOW_HEIGHT) / 2;
    LICENSE_WINDOW description = {0};
    
    WriteLogMessage(TEXT("Showing license window."));

    description.Window = InitializeWindow(LICENSE_WINDOW_TITLE, licenseWindowPositionX, licenseWindowPositionY, LICENSE_WINDOW_WIDTH, LICENSE_WINDOW_HEIGHT, LICENSE_WINDOW_CLASS, 0, LICENSE_WINDOW_STYLE, LICENSE_WINDOW_EXTENDED_STYLE, font, mainWindow, GetModuleHandle(NULL));
    description.Button = InitializeWindow(BUTTON_LICENSE_CAPTION, BUTTON_LICENSE_LEFT, BUTTON_LICENSE_TOP, BUTTON_LICENSE_WIDTH, BUTTON_LICENSE_HEIGHT, TEXT("Button"), BUTTON_LICENSE_IDENTIFIER, BUTTON_LICENSE_STYLE, 0, font, description.Window, NULL);
    description.Text = InitializeWindow(TEXT_LICENSE_CAPTION, TEXT_LICENSE_LEFT, TEXT_LICENSE_TOP, TEXT_LICENSE_WIDTH, TEXT_LICENSE_HEIGHT, TEXT("EDIT"), TEXT_LICENSE_IDENTIFIER, TEXT_LICENSE_STYLE, TEXT_LICENSE_EXTENDED_STYLE, font, description.Window, NULL);
   
    SetWindowLongPtr(description.Window, GWLP_USERDATA, (LONG)&description);

    EnableWindow(mainWindow, FALSE);

    SetFocus(description.Button);

    while(GetMessage(&message, NULL, 0, 0) > 0)
    {
        TranslateMessage(&message);
        DispatchMessage(&message);
    }
    
    EnableWindow(mainWindow, TRUE);
    
    if(!SetForegroundWindow(mainWindow))
        WriteLogMessage(TEXT("SetForegroundWindow failed with error %1!u!."), GetLastError());

    WriteLogMessage(TEXT("returning to main window."));
}

LRESULT CALLBACK LicenseWindowProc(HWND hwnd, UINT uMsg, WPARAM wParam, LPARAM lParam)
{
    PLICENSE_WINDOW description = (PLICENSE_WINDOW)GetWindowLongPtr(hwnd, GWLP_USERDATA);
    RECT windowRectangle = {0};  
    SIZE newSize = {0};
    SIZE delta = {0};
    BOOL firstResize = TRUE;
    RECT controlRectangle = {0};
    SIZE controlSize = {0};
    POINT screenOffset = {0};
    POINT newPosition = {0};

    switch (uMsg)
    {
    case WM_CTLCOLORBTN:
    case WM_CTLCOLORSTATIC:
        SetBkMode((HDC)wParam, TRANSPARENT);
        return (LRESULT)CONTROL_COLOR;

    case WM_CLOSE:   
        DestroyWindow(hwnd);
        return 0;

    case WM_DESTROY:
        PostQuitMessage(0);
        return 0;

    case WM_COMMAND:
        switch(wParam)
        {
        case BUTTON_LICENSE_IDENTIFIER:
            SendMessage(hwnd, WM_CLOSE, 0, 0);
            return 0;
        }
        break;

    case WM_GETMINMAXINFO:
        ((MINMAXINFO*)lParam)->ptMinTrackSize.x = ScaleX(LICENSE_WINDOW_MINWIDTH);
        ((MINMAXINFO*)lParam)->ptMinTrackSize.y = ScaleY(LICENSE_WINDOW_MINHEIGHT);
        return 0;

    case WM_SIZE:
        if(description == NULL)
            break;
  
        if(!GetWindowRect(hwnd, &windowRectangle))
            break;
        
        newSize.cx = (windowRectangle.right - windowRectangle.left);
        newSize.cy = (windowRectangle.bottom - windowRectangle.top);
        delta.cx = (newSize.cx - description->Size.cx);
        delta.cy = (newSize.cy - description->Size.cy);

        firstResize = ((description->Size.cx == 0) || (description->Size.cy == 0));
        
        description->Size.cx = newSize.cx;
        description->Size.cy = newSize.cy;

        if (firstResize)
            break;

        GetWindowRect(description->Text, &controlRectangle);
        
        controlSize.cx = (controlRectangle.right - controlRectangle.left + delta.cx);
        controlSize.cy = (controlRectangle.bottom - controlRectangle.top + delta.cy);
        SetWindowPos(description->Text, NULL, 0, 0, controlSize.cx, controlSize.cy, SWP_NOMOVE | SWP_NOZORDER);
                      
        GetWindowRect(description->Button, &controlRectangle);
            
        ClientToScreen(hwnd, &screenOffset);

        newPosition.x = (controlRectangle.left - screenOffset.x + delta.cx);
        newPosition.y = (controlRectangle.top - screenOffset.y + delta.cy);
    
        SetWindowPos(description->Button, NULL, newPosition.x, newPosition.y, 0, 0, SWP_NOSIZE | SWP_NOZORDER);

        return 0;
    }
    return DefWindowProc(hwnd, uMsg, wParam, lParam);
}

FLOAT Arctg(FLOAT value)
{ 
  FLOAT sign = 1.0f;
  FLOAT x = value;
  FLOAT y = 0.0f;
  if(value == 0.0f) 
      return 0.0f;  
  if(x < 0.0f) 
  {
     sign = -1.0f;
     x *= -1.0f;
  }
  x = (x - 1.0f) / (x + 1.0f);   
  y = x * x;
  x = ((((((((0.0028662257f * y - 0.0161657367f) * y + 0.0429096138f) * y - 0.0752896400f) * y + 0.1065626393f) * y - 0.1420889944f) * y + 0.1999355085f) * y - 0.3333314528f) * y + 1.0f) * x;
  x = 0.785398163397f + sign * x;
  return (x < 0.00001f) ? 0.0f : x;
}

VOID WriteLogMessage(LPCTSTR format, ...)
{
    va_list ap;
    HANDLE file;
    DWORD messageLength;
    SYSTEMTIME time = {0};

    EnterCriticalSection(&logCriticalSection);
    
    GetSystemTime(&time);

    file = CreateFile(logFile, GENERIC_WRITE, FILE_SHARE_READ, NULL, OPEN_ALWAYS, 0, NULL);
    if(file == INVALID_HANDLE_VALUE)
        return;
    
    SetFilePointer(file, 0, NULL, FILE_END);

    va_start(ap, format);
    
    RtlZeroMemory(logBuffer, sizeof(logBuffer));
    messageLength = (DWORD)GetDateFormat(LOCALE_INVARIANT, 0, &time, TEXT("\r\nyyyy-MM-dd "), logBuffer, 65536); 
    WriteFile(file, logBuffer, messageLength * sizeof(TCHAR), &messageLength, NULL);
    
    RtlZeroMemory(logBuffer, sizeof(logBuffer));
    messageLength = (DWORD)GetTimeFormat(LOCALE_INVARIANT, 0, &time, TEXT("HH-mm-ss >> "), logBuffer, 65536); 
    WriteFile(file, logBuffer, messageLength * sizeof(TCHAR), &messageLength, NULL);

    RtlZeroMemory(logBuffer, sizeof(logBuffer));
    messageLength = FormatMessage(FORMAT_MESSAGE_FROM_STRING, format, 0, 0, logBuffer, 65535, &ap);
    WriteFile(file, logBuffer, messageLength * sizeof(TCHAR), &messageLength, NULL);

    va_end(ap);

    CloseHandle(file);

    LeaveCriticalSection(&logCriticalSection);
}
