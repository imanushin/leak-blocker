

#include <windows.h>

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
    GetStartupInfo( &StartupInfo );
    
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