# define name of installer
	!system "ExtractVersionInfo.exe"
	!include "App-Version.txt"
	!include "MUI.nsh"
	Name "DC+SB, Version ${Version}"
	OutFile "Build\DC+SB_v${Version}.exe"
 
# define installation directory
	InstallDir $PROGRAMFILES\DC+SB
	InstallDirRegKey HKLM Software\DC+SB InstallLocation
 
# for removing Start Menu shortcut in Windows 7
	RequestExecutionLevel admin

# install pages
	!insertmacro MUI_PAGE_WELCOME
	!insertmacro MUI_PAGE_DIRECTORY
	!insertmacro MUI_PAGE_INSTFILES
		# These indented statements modify settings for MUI_PAGE_FINISH
		!define MUI_FINISHPAGE_NOAUTOCLOSE
		!define MUI_FINISHPAGE_RUN "$INSTDIR\DC+SB.exe"
		!define MUI_FINISHPAGE_RUN_CHECKED
		!define MUI_FINISHPAGE_RUN_TEXT "Start DC+SB"
	!insertmacro MUI_PAGE_FINISH

# uninstall pages
	!insertmacro MUI_UNPAGE_WELCOME
	!insertmacro MUI_UNPAGE_CONFIRM
	!insertmacro MUI_UNPAGE_INSTFILES
	!insertmacro MUI_UNPAGE_FINISH
 
# language
	!insertmacro MUI_LANGUAGE "English"

# start default section
Section
    # creating start menu shortcuts for all users
    SetShellVarContext all

    # set the installation directory as the destination for the following actions
    SetOutPath $INSTDIR
 
    # create the uninstaller
    WriteUninstaller "uninstall.exe"

    # install app files
    File "..\bin\Release\DC+SB.exe"
    File "..\bin\Release\DC+SB.pdb"
    File "..\bin\Release\AutoUpdater.NET.dll"
    File "..\bin\Release\RawInput.dll"
    File "..\bin\Release\NAudio.dll"
    File "..\bin\Release\NAudio.Vorbis.dll"
    File "..\bin\Release\NVorbis.dll"
 
    # create a shortcut named "new shortcut" in the start menu programs directory
    # point the new shortcut at the program uninstaller
    CreateDirectory "$SMPROGRAMS\DC+SB"
    CreateShortCut "$SMPROGRAMS\DC+SB\uninstall.lnk" "$INSTDIR\uninstall.exe"
    CreateShortCut "$SMPROGRAMS\DC+SB\DC+SB.lnk" "$INSTDIR\DC+SB.exe"

    # add application to Add/Remove programs
    WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\DC+SB" \
                 "DisplayName" "DC+SB"
    WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\DC+SB" \
                 "UninstallString" "$\"$INSTDIR\uninstall.exe$\""
    WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\DC+SB" \
                 "QuietUninstallString" "$\"$INSTDIR\uninstall.exe$\" /S"
    WriteRegStr HKLM "Software\DC+SB" \
                 "InstallLocation" "$\"$INSTDIR$\""
SectionEnd
 
# uninstaller section start
Section "uninstall"
 
    # creating start menu shortcuts for all users
    SetShellVarContext all

    # delete the uninstaller and app files
    Delete "$INSTDIR\uninstall.exe"
    Delete "$INSTDIR\DC+SB.exe"
    Delete "$INSTDIR\DC+SB.pdb"
    Delete "$INSTDIR\AutoUpdater.NET.dll"
    Delete "$INSTDIR\RawInput.dll"
    Delete "$INSTDIR\NAudio.dll"
    Delete "$INSTDIR\NAudio.Vorbis.dll"
    Delete "$INSTDIR\NVorbis.dll"
    StrCpy $0 "$INSTDIR"
    Call un.DeleteDirIfEmpty
 
    # remove the link from the start menu
    Delete "$SMPROGRAMS\DC+SB\uninstall.lnk"
    Delete "$SMPROGRAMS\DC+SB\DC+SB.lnk"
    StrCpy $0 "$SMPROGRAMS\DC+SB"
    Call un.DeleteDirIfEmpty

    # remove registry key
    DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\DC+SB"
    DeleteRegKey HKLM "Software\DC+SB"
 
# uninstaller section end
SectionEnd

Function un.DeleteDirIfEmpty
  FindFirst $R0 $R1 "$0\*.*"
  strcmp $R1 "." 0 NoDelete
   FindNext $R0 $R1
   strcmp $R1 ".." 0 NoDelete
    ClearErrors
    FindNext $R0 $R1
    IfErrors 0 NoDelete
     FindClose $R0
     Sleep 1000
     RMDir "$0"
  NoDelete:
   FindClose $R0
FunctionEnd