!define File "temp\Windar.exe"

RequestExecutionLevel user
OutFile "get_version.exe"
 
Function .onInit

   ## Get file version
   GetDllVersion "${File}" $R0 $R1
   IntOp $R2 $R0 / 0x00010000
   IntOp $R3 $R0 & 0x0000FFFF
   IntOp $R4 $R1 / 0x00010000
   IntOp $R5 $R1 & 0x0000FFFF
   StrCpy $R1 "$R2.$R3.$R4.$R5"

   ## Write it to a !define for use in main script
   FileOpen $R0 "version.txt" w
   FileWrite $R0 '!define VER_MAJOR "$R2"'
   FileWriteByte $R0 "13"
   FileWriteByte $R0 "10"
   FileWrite $R0 '!define VER_MINOR "$R3"'
   FileWriteByte $R0 "13"
   FileWriteByte $R0 "10"
   FileWrite $R0 '!define VER_BUILD "$R4"'
   FileWriteByte $R0 "13"
   FileWriteByte $R0 "10"
   FileWrite $R0 '!define VER_REVISION "$R5"'
   FileWriteByte $R0 "13"
   FileWriteByte $R0 "10"
   FileWrite $R0 '!define VER_DLL "$R1"'
   FileWriteByte $R0 "13"
   FileWriteByte $R0 "10"
   FileClose $R0

   Abort
FunctionEnd

Section
SectionEnd