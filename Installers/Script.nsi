# define pages
Page directory
Page instfiles
UninstPage uninstConfirm
UninstPage instfiles

# define name of installer
!system "ExtractVersionInfo.exe"
!include "App-Version.txt"
Name "DC+SB, Version ${Version}"
OutFile "Build\DC+SB_v${Version}.exe"
 
# define installation directory
InstallDir $PROGRAMFILES\DC+SB
InstallDirRegKey HKLM Software\DC+SB InstallLocation
 
# For removing Start Menu shortcut in Windows 7
RequestExecutionLevel admin

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