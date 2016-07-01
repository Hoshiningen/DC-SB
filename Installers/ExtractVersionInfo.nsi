!define File "..\bin\Release\DC+SB.exe"

OutFile "ExtractVersionInfo.exe"
SilentInstall silent
RequestExecutionLevel user

Section

 ## Get file version
 GetDllVersion "${File}" $R0 $R1
  IntOp $R2 $R0 / 0x00010000
  IntOp $R3 $R0 & 0x0000FFFF
  IntOp $R4 $R1 / 0x00010000
  IntOp $R5 $R1 & 0x0000FFFF
  StrCpy $R1 "$R2.$R3.$R4.$R5"

 ## Write it to a !define for use in main script
 FileOpen $R0 "$EXEDIR\App-Version.txt" w
  FileWrite $R0 '!define Version "$R1"'
 FileClose $R0

 ## Create XML for auto-updater
 FileOpen $R0 "Build\dc+sb.xml" w
  FileWrite $R0 '<?xml version="1.0" encoding="utf-8"?>'
  FileWrite $R0 "$\r$\n"
  FileWrite $R0 '<item>'
  FileWrite $R0 "$\r$\n"
  FileWrite $R0 '<title>New version $R1 is available for DC+SB</title>'
  FileWrite $R0 "$\r$\n"
  FileWrite $R0 '<version>$R1</version>'
  FileWrite $R0 "$\r$\n"
  FileWrite $R0 '<url>http://kalejin.eu/dc+sb/DC+SB_v$R1.exe</url>'
  FileWrite $R0 "$\r$\n"
  FileWrite $R0 '<changelog>http://kalejin.eu/dc+sb/changes.html</changelog>'
  FileWrite $R0 "$\r$\n"
  FileWrite $R0 '</item>'
 FileClose $R0

SectionEnd