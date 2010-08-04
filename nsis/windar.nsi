; DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
;
; Copyright (C) 2009, 2010 Steven Robertson <steve@playnode.org>
;
; Windar - Playdar for Windows
;
; Windar is free software; you can redistribute it and/or modify it
; under the terms of the GNU Lesser General Public License (LGPL) as published
; by the Free Software Foundation; either version 2.1 of the License, or (at
; your option) any later version.
;
; This software is distributed in the hope that it will be useful,
; but WITHOUT ANY WARRANTY; without even the implied warranty of
; MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
; GNU Lesser General Public License version 2.1 for more details
; (a copy is included in the LICENSE file that accompanied this code).
;
; You should have received a copy of the GNU Lesser General Public License
; along with this program.  If not, see <http://www.gnu.org/licenses/>.

;-----------------------------------------------------------------------------
;Get version information from Windar exe.
;-----------------------------------------------------------------------------
!system "get_version.exe"
!include "version.txt"
!define VERSION "${VER_MAJOR}.${VER_MINOR}.${VER_BUILD}"

;-----------------------------------------------------------------------------
;Installer build timestamp.
;-----------------------------------------------------------------------------
!define /date BUILD_TIME "built on %Y/%m/%d at %I:%M %p  (rev. ${VER_REVISION})"

;-----------------------------------------------------------------------------
;Some installer script options (comment-out options not required)
;-----------------------------------------------------------------------------
;!define OPTION_DEBUG_BUILD
!define OPTION_LICENSE_AGREEMENT
!define OPTION_BUNDLE_C_REDIST
;!define OPTION_BUNDLE_MPLAYER
!define OPTION_BUNDLE_RESOLVERS
;!define OPTION_BUNDLE_NAPSTER_RESOLVER
;!define OPTION_BUNDLE_SCROBBLER
!define OPTION_SECTION_SC_START_MENU
;!define OPTION_SECTION_SC_START_MENU_STARTUP
!define OPTION_SECTION_SC_DESKTOP
!define OPTION_SECTION_SC_QUICK_LAUNCH
!define OPTION_FINISHPAGE
!define OPTION_FINISHPAGE_LAUNCHER
!define OPTION_FINISHPAGE_RELEASE_NOTES

;-----------------------------------------------------------------------------
;Other required definitions.
;-----------------------------------------------------------------------------
!define REDIST_DLL_VERSION 8.0.50727.4053

;-----------------------------------------------------------------------------
;Initial installer setup and definitions.
;-----------------------------------------------------------------------------
Name "Windar"
Caption "Playdar for Windows"
BrandingText "Windar ${VERSION}  -- ${BUILD_TIME}"
OutFile "windar-${VERSION}.exe"
InstallDir "$PROGRAMFILES\Windar"
InstallDirRegKey HKCU "Software\Windar" ""
InstType Standard
InstType Full
InstType Minimal
SetCompressor /SOLID lzma
RequestExecutionLevel user ;Now using the UAC plugin.
ReserveFile windar.ini
ReserveFile '${NSISDIR}\Plugins\InstallOptions.dll'

;-----------------------------------------------------------------------------
;Include some required header files.
;-----------------------------------------------------------------------------
!include LogicLib.nsh ;Used by APPDATA uninstaller.
!include nsDialogs.nsh ;Used by APPDATA uninstaller.
!include MUI2.nsh ;Used by APPDATA uninstaller.
!include InstallOptions.nsh ;Required by MUI2 to support old MUI_INSTALLOPTIONS.
!include Memento.nsh ;Remember user selections.
!include WinVer.nsh ;Windows version detection.
!include WordFunc.nsh ;Used by VersionCompare macro function.
!include UAC.nsh ;Used by the UAC elevation to install as user or admin.

;-----------------------------------------------------------------------------
;Memento selections stored in registry.
;-----------------------------------------------------------------------------
!define MEMENTO_REGISTRY_ROOT HKLM
!define MEMENTO_REGISTRY_KEY Software\Microsoft\Windows\CurrentVersion\Uninstall\Windar

;-----------------------------------------------------------------------------
;Modern User Interface (MUI) defintions and setup.
;-----------------------------------------------------------------------------
!define MUI_ABORTWARNING
!define MUI_ICON installer.ico
!define MUI_UNICON installer.ico
!define MUI_WELCOMEFINISHPAGE_BITMAP welcome.bmp
!define MUI_WELCOMEPAGE_TITLE "Windar ${VERSION} Setup$\r$\nInstaller Build Revision ${VER_REVISION}"
!define MUI_WELCOMEPAGE_TEXT "This wizard will guide you through the installation.$\r$\n$\r$\n$_CLICK"
!define MUI_HEADERIMAGE
!define MUI_HEADERIMAGE_BITMAP page_header.bmp
!define MUI_COMPONENTSPAGE_SMALLDESC
!define MUI_FINISHPAGE_TITLE "Windar Install Completed"
!define MUI_FINISHPAGE_LINK "Click here to visit the Windar website."
!define MUI_FINISHPAGE_LINK_LOCATION "http://windar.org/"
!define MUI_FINISHPAGE_NOREBOOTSUPPORT
!ifdef OPTION_FINISHPAGE_RELEASE_NOTES
   !define MUI_FINISHPAGE_SHOWREADME_NOTCHECKED
   !define MUI_FINISHPAGE_SHOWREADME "$INSTDIR\NOTES.txt"
   !define MUI_FINISHPAGE_SHOWREADME_TEXT "Show release notes"
!endif
!ifdef OPTION_FINISHPAGE_LAUNCHER
   !define MUI_FINISHPAGE_NOAUTOCLOSE
   !define MUI_FINISHPAGE_RUN
   !define MUI_FINISHPAGE_RUN_FUNCTION "LaunchWindar"
!endif

;-----------------------------------------------------------------------------
;Page macros.
;-----------------------------------------------------------------------------
!insertmacro MUI_PAGE_WELCOME
!ifdef OPTION_LICENSE_AGREEMENT
   !insertmacro MUI_PAGE_LICENSE "LICENSE.txt"
!endif
Page custom PageReinstall PageLeaveReinstall
!insertmacro MUI_PAGE_COMPONENTS
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
!ifdef OPTION_FINISHPAGE
   !insertmacro MUI_PAGE_FINISH
!endif
!insertmacro MUI_UNPAGE_CONFIRM
UninstPage custom un.UnPageProfile un.UnPageProfileLeave
!insertmacro MUI_UNPAGE_INSTFILES

;-----------------------------------------------------------------------------
;Other MUI macros.
;-----------------------------------------------------------------------------
!insertmacro MUI_LANGUAGE "English"

##############################################################################
#                                                                            #
#   FINISH PAGE LAUNCHER FUNCTIONS                                           #
#                                                                            #
##############################################################################

Function LaunchWindar
   ${UAC.CallFunctionAsUser} LaunchWindarAsUser
FunctionEnd

Function LaunchWindarAsUser
   Exec "$INSTDIR\Windar.exe"
FunctionEnd

##############################################################################
#                                                                            #
#   PROCESS HANDLING FUNCTIONS AND MACROS                                    #
#                                                                            #
##############################################################################

!macro CheckForProcess processName gotoWhenFound gotoWhenNotFound
   Processes::FindProcess ${processName}
   StrCmp $R0 "0" ${gotoWhenNotFound} ${gotoWhenFound}
!macroend

!macro WaitOnProcessShutdown processName sleepMillis gotoWhenNotFound
   !insertmacro CheckForProcess ${processName} 0 ${gotoWhenNotFound}
   Sleep ${sleepMillis}
!macroend

!macro ConfirmEndProcess processName
   MessageBox MB_YESNO|MB_ICONEXCLAMATION \
     "Found ${processName} process(s) which need to be stopped.$\nDo you want the installer to stop these for you?" \
     IDYES process_${processName}_kill IDNO process_${processName}_ended
   process_${processName}_kill:
      DetailPrint "Killing ${processName} processes."
      Processes::KillProcess ${processName}
      Sleep 1500
      StrCmp $R0 "1" process_${processName}_ended
      DetailPrint "Process to kill not found!"
   process_${processName}_ended:
!macroend

!macro CheckAndConfirmEndProcess processName
   !insertmacro CheckForProcess ${processName} 0 no_process_${processName}_to_end
   !insertmacro ConfirmEndProcess ${processName}
   no_process_${processName}_to_end:
!macroend

!macro WaitOnProcessToEnd processName

   ;Wait up to 10 seconds for the process to end.
   !insertmacro WaitOnProcessShutdown ${processName} 500 waiting_on_process_ended ; 1
   !insertmacro WaitOnProcessShutdown ${processName} 500 waiting_on_process_ended ; 2
   !insertmacro WaitOnProcessShutdown ${processName} 500 waiting_on_process_ended ; 3
   !insertmacro WaitOnProcessShutdown ${processName} 500 waiting_on_process_ended ; 4
   !insertmacro WaitOnProcessShutdown ${processName} 500 waiting_on_process_ended ; 5
   !insertmacro WaitOnProcessShutdown ${processName} 500 waiting_on_process_ended ; 6
   !insertmacro WaitOnProcessShutdown ${processName} 500 waiting_on_process_ended ; 7
   !insertmacro WaitOnProcessShutdown ${processName} 500 waiting_on_process_ended ; 8
   !insertmacro WaitOnProcessShutdown ${processName} 500 waiting_on_process_ended ; 9
   !insertmacro WaitOnProcessShutdown ${processName} 500 waiting_on_process_ended ;10
   !insertmacro WaitOnProcessShutdown ${processName} 500 waiting_on_process_ended ;11
   !insertmacro WaitOnProcessShutdown ${processName} 500 waiting_on_process_ended ;12
   !insertmacro WaitOnProcessShutdown ${processName} 500 waiting_on_process_ended ;13
   !insertmacro WaitOnProcessShutdown ${processName} 500 waiting_on_process_ended ;14
   !insertmacro WaitOnProcessShutdown ${processName} 500 waiting_on_process_ended ;15
   !insertmacro WaitOnProcessShutdown ${processName} 500 waiting_on_process_ended ;16
   !insertmacro WaitOnProcessShutdown ${processName} 500 waiting_on_process_ended ;17
   !insertmacro WaitOnProcessShutdown ${processName} 500 waiting_on_process_ended ;18
   !insertmacro WaitOnProcessShutdown ${processName} 500 waiting_on_process_ended ;19
   !insertmacro WaitOnProcessShutdown ${processName} 500 waiting_on_process_ended ;20

   ;If not completed offer to kill it.
   !insertmacro ConfirmEndProcess ${processName}

   waiting_on_process_ended:
!macroend

!macro ShutdownWindarTray un
   Function ${un}ShutdownWindarTray   
      !insertmacro CheckForProcess "Windar.exe" 0 skip_tray_shutdown

      SetDetailsPrint textonly
      DetailPrint "Shutting down the Windar tray application."
      SetDetailsPrint listonly
      
      ;Write a file to prompt the tray app to shutdown.
      FileOpen $0 "$INSTDIR\SHUTDOWN" w
      FileWrite $0 "x"
      FileClose $0
      
      ;Give the tray app some time to shutdown.
      SetDetailsPrint textonly
      DetailPrint "Pausing to allow Windar tray to shutdown, if running."
      SetDetailsPrint listonly
      !insertmacro WaitOnProcessToEnd "Windar.exe"
      
      skip_tray_shutdown:
   FunctionEnd
!macroend

!insertmacro ShutdownWindarTray ""
!insertmacro ShutdownWindarTray "un."

!macro KillErlang un
   Function ${un}KillErlang
      !insertmacro CheckAndConfirmEndProcess "epmd.exe"
      !insertmacro CheckAndConfirmEndProcess "erl.exe"
   FunctionEnd
!macroend

!insertmacro KillErlang ""
!insertmacro KillErlang "un."

##############################################################################
#                                                                            #
#   C REDISTRIBUTABLE INSTALLER                                              #
#                                                                            #
##############################################################################

!define VCRUNTIME_SETUP_NAME "vcredist_x86.exe"

!ifdef OPTION_BUNDLE_C_REDIST
   Function IsDllVersionGoodEnough
      IntCmp 0 $R0 normal0 normal0 negative0
      normal0:
         IntOp $R2 $R0 >> 16
         Goto continue0
      negative0:
         IntOp $R2 $R0 & 0x7FFF0000
         IntOp $R2 $R2 >> 16
         IntOp $R2 $R2 | 0x8000
      continue0:
         IntOp $R3 $R0 & 0x0000FFFF
         IntCmp 0 $R1 normal1 normal1 negative1
      normal1:
         IntOp $R4 $R1 >> 16
         Goto continue1
      negative1:
         IntOp $R4 $R1 & 0x7FFF0000
         IntOp $R4 $R4 >> 16
         IntOp $R4 $R4 | 0x8000
      continue1:
         IntOp $R5 $R1 & 0x0000FFFF
         StrCpy $2 "$R2.$R3.$R4.$R5"
         ${VersionCompare} $2 ${REDIST_DLL_VERSION} $R0
         Return
   FunctionEnd

   Function RequireCRedist
      SetDetailsPrint textonly
      DetailPrint "Checking for the Microsoft C runtime version required."
      SetDetailsPrint listonly

      IfFileExists $SYSDIR\msvcr80.dll MaybeFoundInSystem
      SearchSxs:
         FindFirst $0 $1 $WINDIR\WinSxS\x86*
      Loop:
         StrCmp $1 "" NotFound
         IfFileExists $WINDIR\WinSxS\$1\msvcr80.dll MaybeFoundInSxs
         FindNext $0 $1
         Goto Loop
      MaybeFoundInSxs:
         GetDllVersion $WINDIR\WinSxS\$1\msvcr80.dll $R0 $R1
         Call IsDllVersionGoodEnough
         FindNext $0 $1
         IntCmp 2 $R0 Loop
         Goto Found
      MaybeFoundInSystem:
         GetDllVersion $SYSDIR\msvcr80.dll $R0 $R1
         Call IsDllVersionGoodEnough
         IntCmp 2 $R0 SearchSxS
      Found:
         Return
      NotFound:
         MessageBox MB_ICONINFORMATION|MB_OK "Windar Setup determined that you \
            do not have the Microsoft C runtime version required. It will now be installed."
         SetOutPath "$INSTDIR"
         File "payload\${VCRUNTIME_SETUP_NAME}"
         SetDetailsPrint textonly
         DetailPrint "Installing the Microsoft C runtime redistributable."
         SetDetailsPrint listonly
         ExecWait '"$INSTDIR\${VCRUNTIME_SETUP_NAME}" /q:a /c:"VCREDI~1.EXE /q:a /c:""msiexec /i vcredist.msi /qb!"" "'
         Delete "$INSTDIR\${VCRUNTIME_SETUP_NAME}"
   FunctionEnd
!endif

##############################################################################
#                                                                            #
#   .NET FRAMEWORK INSTALLER                                                 #
#                                                                            #
##############################################################################

!define NET_URL "http://download.microsoft.com/download/5/6/7/567758a3-759e-473e-bf8f-52154438565a/dotnetfx.exe"

!insertmacro VersionCompare

Function GetDotNETVersion
   Push $0
   Push $1
   System::Call "mscoree::GetCORVersion(w .r0, i ${NSIS_MAX_STRLEN}, *i) i .r1 ?u"
   StrCmp $1 "error" 0 +2
      StrCpy $0 "not found"
   Pop $1
   Exch $0
FunctionEnd

Function RequireDotNet
   SetDetailsPrint textonly
   DetailPrint "Checking for .NET Framework 2.0 or newer."
   SetDetailsPrint listonly

   Call GetDotNETVersion
   Pop $0
   ${If} $0 == "not found"
      Call GetDotNetFramework
   ${Else}
      StrCpy $0 $0 "" 1 # skip "v"
      ${VersionCompare} $0 "2.0" $1
      ${If} $1 == 2
         Call GetDotNetFramework
      ${EndIf}
   ${EndIf}
FunctionEnd

Function GetDotNetFramework
   SetDetailsPrint textonly
   DetailPrint "Downloading .NET Framework 2.0"
   SetDetailsPrint listonly
   
   MessageBox MB_OK "Playnode uses .NET Framework 2.0, which will now be downloaded and installed" 
   StrCpy $2 "$TEMP\The .NET Framework.exe"
   nsisdl::download /TIMEOUT=30000 ${NET_URL} $2
   Pop $R0 ;Get the return value
   StrCmp $R0 "success" +4
      MessageBox MB_OK "Download failed: $R0"
      UAC::Unload
      Quit

   SetDetailsPrint textonly
   DetailPrint "Installing the .NET Framework."
   SetDetailsPrint listonly

   ExecWait '"$2" /q:a /c:"install /l /q"'
   Delete '"$2"'
FunctionEnd

##############################################################################
#                                                                            #
#   RE-INSTALLER FUNCTIONS                                                   #
#                                                                            #
##############################################################################

Function PageReinstall
   ReadRegStr $R0 HKLM "Software\Windar" ""
   StrCmp $R0 "" 0 +2
   Abort

   ;Detect version
   ReadRegDWORD $R0 HKLM "Software\Windar" "VersionMajor"
   IntCmp $R0 ${VER_MAJOR} minor_check new_version older_version
   minor_check:
      ReadRegDWORD $R0 HKLM "Software\Windar" "VersionMinor"
      IntCmp $R0 ${VER_MINOR} build_check new_version older_version
   build_check:
      ReadRegDWORD $R0 HKLM "Software\Windar" "VersionBuild"
      IntCmp $R0 ${VER_BUILD} revision_check new_version older_version
   revision_check:
      ReadRegDWORD $R0 HKLM "Software\Windar" "VersionRevision"
      IntCmp $R0 ${VER_REVISION} same_version new_version older_version

   new_version:
      !insertmacro INSTALLOPTIONS_WRITE "windar.ini" "Field 1" "Text" "An older version of Windar is installed on your system. It is recommended that you uninstall the current version before installing. Select the operation you want to perform and click Next to continue."
      !insertmacro INSTALLOPTIONS_WRITE "windar.ini" "Field 2" "Text" "Uninstall before installing"
      !insertmacro INSTALLOPTIONS_WRITE "windar.ini" "Field 3" "Text" "Do not uninstall"
      !insertmacro MUI_HEADER_TEXT "Already Installed" "Choose how you want to install Windar."
      StrCpy $R0 "1"
      Goto reinst_start

   older_version:
      !insertmacro INSTALLOPTIONS_WRITE "windar.ini" "Field 1" "Text" "A newer version of Windar is already installed! It is not recommended that you install an older version. If you really want to install this older version, it is better to uninstall the current version first. Select the operation you want to perform and click Next to continue."
      !insertmacro INSTALLOPTIONS_WRITE "windar.ini" "Field 2" "Text" "Uninstall before installing"
      !insertmacro INSTALLOPTIONS_WRITE "windar.ini" "Field 3" "Text" "Do not uninstall"
      !insertmacro MUI_HEADER_TEXT "Already Installed" "Choose how you want to install Windar."
      StrCpy $R0 "1"
      Goto reinst_start

   same_version:
      !insertmacro INSTALLOPTIONS_WRITE "windar.ini" "Field 1" "Text" "Windar ${VERSION} is already installed.$\r$\nSelect the operation you want to perform and click Next to continue."
      !insertmacro INSTALLOPTIONS_WRITE "windar.ini" "Field 2" "Text" "Add/Reinstall components"
      !insertmacro INSTALLOPTIONS_WRITE "windar.ini" "Field 3" "Text" "Uninstall Windar"
      !insertmacro MUI_HEADER_TEXT "Already Installed" "Choose the maintenance option to perform."
      StrCpy $R0 "2"

   reinst_start:
      !insertmacro INSTALLOPTIONS_DISPLAY "windar.ini"
FunctionEnd

Function PageLeaveReinstall
   !insertmacro INSTALLOPTIONS_READ $R1 "windar.ini" "Field 2" "State"
   StrCmp $R0 "1" 0 +2
   StrCmp $R1 "1" reinst_uninstall reinst_done
   StrCmp $R0 "2" 0 +3
   StrCmp $R1 "1" reinst_done reinst_uninstall
   reinst_uninstall:
      ReadRegStr $R1 HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Windar" "UninstallString"
      HideWindow
      ClearErrors
      ExecWait '$R1 _?=$INSTDIR'
      IfErrors no_remove_uninstaller
      IfFileExists "$INSTDIR\Windar.exe" no_remove_uninstaller
      Delete $R1
      RMDir $INSTDIR
   no_remove_uninstaller:
      StrCmp $R0 "2" 0 +3
      UAC::Unload
      Quit
      BringToFront
   reinst_done:
FunctionEnd

##############################################################################
#                                                                            #
#   INSTALLER SECTIONS                                                       #
#                                                                            #
##############################################################################

Section "Playdar core & Windar tray application" SEC_WINDAR
   SectionIn 1 2 3 RO
   SetDetailsPrint listonly
   
   ;Shutdown Windar in case Add/Remove re-installer option used.
   Call ShutdownWindarTray
   Call KillErlang

   Call RequireCRedist
   Call RequireDotNet

   SetDetailsPrint textonly
   DetailPrint "Installing the OpenSSL DLL."
   SetDetailsPrint listonly

   IfFileExists "$WINDIR\system32\libeay32.dll" openssl_lib_installed
      SetOutPath "$WINDIR\system32"
      File payload\libeay32.dll
   openssl_lib_installed:

   ;Erlang and Playdar payload.
   SetOutPath "$INSTDIR"
      
   SetDetailsPrint textonly
   DetailPrint "Installing minimum Erlang components."
   SetDetailsPrint listonly

   File /r payload\minimerl

   SetDetailsPrint textonly
   DetailPrint "Installing Playdar core."
   SetDetailsPrint listonly

   File /r payload\playdar

   SetDetailsPrint textonly
   DetailPrint "Installing py2exe shared components."
   SetDetailsPrint listonly

   ;py2exe payload.
   !ifdef OPTION_BUNDLE_RESOLVERS
      SetOutPath "$INSTDIR\playdar\py2exe"
      File payload\playdar_python_resolvers\dist\_ctypes.pyd
      File payload\playdar_python_resolvers\dist\_hashlib.pyd
      File payload\playdar_python_resolvers\dist\_socket.pyd
      File payload\playdar_python_resolvers\dist\_ssl.pyd
      File payload\playdar_python_resolvers\dist\bz2.pyd
      File payload\playdar_python_resolvers\dist\library.zip
      File payload\playdar_python_resolvers\dist\pyexpat.pyd
      File payload\playdar_python_resolvers\dist\python26.dll
      File payload\playdar_python_resolvers\dist\select.pyd
      File payload\playdar_python_resolvers\dist\unicodedata.pyd
      File payload\playdar_python_resolvers\dist\w9xpopen.exe
   !endif

   ;Bat script to launch playdar-core in command window.
   SetOutPath "$INSTDIR\playdar"
   File payload\playdar-core.bat

   SetDetailsPrint textonly
   DetailPrint "Installing the Windar application components."
   SetDetailsPrint listonly

   SetOutPath "$INSTDIR"

   ;Windar application components:
   File temp\ErlangTerms.dll
   File temp\Windar.exe
   File temp\Windar.Common.dll
   File temp\Windar.PlaydarDaemon.dll
   File temp\Windar.PluginAPI.dll

   ;Configuration
   File temp\Windar.exe.config

   ;Debug build PDB files:
   !ifdef OPTION_DEBUG_BUILD
      File temp\ErlangTerms.pdb
      File temp\Windar.pdb
      File temp\Windar.Common.pdb
      File temp\Windar.PlaydarDaemon.pdb
      File temp\Windar.PluginAPI.pdb
   !endif

   ;Other libs:
   File temp\log4net.dll
   File temp\Newtonsoft.Json.Net20.dll

   ;REAME, License & copyright files.
   File NOTES.txt
   File /oname=COPYING.txt ..\COPYING
   File /oname=LICENSE.txt ..\LICENSE

   ;Player plugin for Windar.
   !ifdef OPTION_BUNDLE_MPLAYER
      SetDetailsPrint textonly
      DetailPrint "Installing the Player plugin for Windar."
      SetDetailsPrint listonly
      SetOutPath "$INSTDIR"
      File /r payload\mplayer
      File temp\Windar.PlayerPlugin.dll
      !ifdef OPTION_DEBUG_BUILD
         File temp\Windar.PlayerPlugin.pdb
      !endif
   !endif
SectionEnd

SectionGroup "Shortcuts"

!ifdef OPTION_SECTION_SC_START_MENU
   ${MementoSection} "Start Menu Program Group" SEC_START_MENU
      SectionIn 1 2 3
      SetDetailsPrint textonly
      DetailPrint "Adding shortcuts for the Windar program group to the Start Menu."
      SetDetailsPrint listonly
      SetShellVarContext all
      RMDir /r "$SMPROGRAMS\Windar"
      CreateDirectory "$SMPROGRAMS\Windar"
      CreateShortCut "$SMPROGRAMS\Windar\Windar.lnk" "$INSTDIR\Windar.exe"
      CreateShortCut "$SMPROGRAMS\Windar\Playdar Core (Shell).lnk" "$INSTDIR\playdar\playdar-core.bat"
      CreateShortCut "$SMPROGRAMS\Windar\COPYING.lnk" "$INSTDIR\COPYING.txt"
      CreateShortCut "$SMPROGRAMS\Windar\Windar Licence.lnk" "$INSTDIR\LICENSE.txt"
      CreateShortCut "$SMPROGRAMS\Windar\Release Notes.lnk" "$INSTDIR\NOTES.txt"
      CreateShortCut "$SMPROGRAMS\Windar\Uninstall.lnk" "$INSTDIR\uninstall.exe"
      SetShellVarContext current
   ${MementoSectionEnd}
!endif

!ifdef OPTION_SECTION_SC_START_MENU_STARTUP
   ${MementoSection} "Start Menu Startup Folder Shortcut" SEC_STARTUP_FOLDER
      SectionIn 1 2
      SetDetailsPrint textonly
      DetailPrint "Adding shortcut in Startup folder to start Windar on login."
      SetDetailsPrint listonly
      SetShellVarContext all
      CreateShortCut "$SMPROGRAMS\Startup\Windar.lnk" "$INSTDIR\Windar.exe"
      SetShellVarContext current
   ${MementoSectionEnd}
!endif

!ifdef OPTION_SECTION_SC_DESKTOP
   ${MementoSection} "Desktop Shortcut" SEC_DESKTOP
      SectionIn 1 2
      SetDetailsPrint textonly
      DetailPrint "Creating Desktop Shortcuts"
      SetDetailsPrint listonly
      CreateShortCut "$DESKTOP\Windar.lnk" "$INSTDIR\Windar.exe"
   ${MementoSectionEnd}
!endif

!ifdef OPTION_SECTION_SC_QUICK_LAUNCH
   ${MementoSection} "Quick Launch Shortcut" SEC_QUICK_LAUNCH
      SectionIn 1 2
      SetDetailsPrint textonly
      DetailPrint "Creating Quick Launch Shortcut"
      SetDetailsPrint listonly
      CreateShortCut "$QUICKLAUNCH\Windar.lnk" "$INSTDIR\Windar.exe"
   ${MementoSectionEnd}
!endif

SectionGroupEnd

!ifdef OPTION_BUNDLE_RESOLVERS
   SectionGroup "Additional resolver modules"

   ${MementoUnselectedSection} "AOL Music Index" SEC_AOL_RESOLVER
      SectionIn 2
      SetDetailsPrint textonly
      DetailPrint "Installing resolver for the AOL Music Index."
      SetDetailsPrint listonly
      SetOutPath "$INSTDIR\playdar\playdar_modules"
      File /r payload\playdar_modules\aolmusic
   ${MementoSectionEnd}

   ${MementoUnselectedSection} "Audiofarm.org" SEC_AUDIOFARM_RESOLVER
      SectionIn 2
      SetDetailsPrint textonly
      DetailPrint "Installing resolver for Audiofarm."
      SetDetailsPrint listonly
      SetOutPath "$INSTDIR\playdar\py2exe"
      File /r payload\playdar_python_resolvers\dist\audiofarm_resolver.exe
   ${MementoSectionEnd}

   ${MementoUnselectedSection} "The Echo Nest" SEC_ECHONEST_RESOLVER
      SectionIn 2
      SetDetailsPrint textonly
      DetailPrint "Installing resolver for The Echo Nest."
      SetDetailsPrint listonly
      SetOutPath "$INSTDIR\playdar\py2exe"
      File /r payload\playdar_python_resolvers\dist\echonest-resolver.exe
   ${MementoSectionEnd}

   ${MementoUnselectedSection} "Jamendo" SEC_JAMENDO_RESOLVER
      SectionIn 2
      SetDetailsPrint textonly
      DetailPrint "Installing resolver for Jamendo."
      SetDetailsPrint listonly
      SetOutPath "$INSTDIR\playdar\playdar_modules"
      File /r payload\playdar_modules\jamendo
   ${MementoSectionEnd}

   ${MementoUnselectedSection} "Magnatune" SEC_MAGNATUNE_RESOLVER
      SectionIn 2
      SetDetailsPrint textonly
      DetailPrint "Installing resolver for Magnatune."
      SetDetailsPrint listonly
      SetOutPath "$INSTDIR\playdar\playdar_modules"
      File /r payload\playdar_modules\magnatune
   ${MementoSectionEnd}

   ${MementoUnselectedSection} "MP3tunes" SEC_MP3TUNES_RESOLVER
      SectionIn 2
      SetDetailsPrint textonly
      DetailPrint "Installing resolver for MP3tunes."
      SetDetailsPrint listonly
      SetOutPath "$INSTDIR\playdar\py2exe"
      File /r payload\playdar_python_resolvers\dist\mp3tunes-resolver.exe
      SetOutPath "$INSTDIR"
      File temp\Windar.MP3tunesPlugin.dll
      !ifdef OPTION_DEBUG_BUILD
         File temp\Windar.MP3tunesPlugin.pdb
      !endif
   ${MementoSectionEnd}

   !ifdef OPTION_BUNDLE_NAPSTER_RESOLVER
      ${MementoUnselectedSection} "Napster" SEC_NAPSTER_RESOLVER
         SectionIn 2
         SetDetailsPrint textonly
         DetailPrint "Installing resolver for Napster."
         SetDetailsPrint listonly
         SetOutPath "$INSTDIR\playdar\py2exe"
         File /r payload\playdar_python_resolvers\dist\napster_resolver.exe
         SetOutPath "$INSTDIR"
         File temp\Windar.NapsterPlugin.dll
         !ifdef OPTION_DEBUG_BUILD
            File temp\Windar.NapsterPlugin.pdb
         !endif
      ${MementoSectionEnd}
   !endif
   
   SectionGroupEnd
!endif

!ifdef OPTION_BUNDLE_SCROBBLER
   ${MementoUnselectedSection} "Audioscrobbler support" SEC_SCROBBLER
      SectionIn 2
      SetDetailsPrint textonly
      DetailPrint "Installing the scrobbler module and Windar plugin."
      SetDetailsPrint listonly
      SetOutPath "$INSTDIR\playdar\playdar_modules"
      File /r payload\playdar_modules\audioscrobbler
      SetOutPath "$INSTDIR"
      File temp\Windar.ScrobblerPlugin.dll
      !ifdef OPTION_DEBUG_BUILD
         File temp\Windar.ScrobblerPlugin.pdb
      !endif
   ${MementoSectionEnd}
!endif

${MementoSectionDone}

;Installer section descriptions
;--------------------------------
!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN

!insertmacro MUI_DESCRIPTION_TEXT ${SEC_WINDAR} "Playdar core and Windar tray application with a cut-down version of Erlang."
!insertmacro MUI_DESCRIPTION_TEXT ${SEC_SCROBBLER} "Scrobbing support for Last.fm/audioscrobbler."
!insertmacro MUI_DESCRIPTION_TEXT ${SEC_AUDIOFARM_RESOLVER} "Audiofarm.org resolver script."
!insertmacro MUI_DESCRIPTION_TEXT ${SEC_AOL_RESOLVER} "Resolves to web content in the AOL Music Index."
!insertmacro MUI_DESCRIPTION_TEXT ${SEC_ECHONEST_RESOLVER} "The Echo Nest resolver."
!insertmacro MUI_DESCRIPTION_TEXT ${SEC_JAMENDO_RESOLVER} "Resolver for free and legal music downloads on Jamendo."
!insertmacro MUI_DESCRIPTION_TEXT ${SEC_MAGNATUNE_RESOLVER} "Resolver for free content on Magnatune."
!insertmacro MUI_DESCRIPTION_TEXT ${SEC_MP3TUNES_RESOLVER} "Resolver for your MP3tunes locker. Requires a free or paid account."
!ifdef OPTION_BUNDLE_NAPSTER_RESOLVER
   !insertmacro MUI_DESCRIPTION_TEXT ${SEC_NAPSTER_RESOLVER} "Resolver for Napster. Paid account has full streams, otherwise provides 30 second samples."
!endif
!ifdef OPTION_SECTION_SC_START_MENU
   !insertmacro MUI_DESCRIPTION_TEXT ${SEC_START_MENU} "Windar program group, with shortcuts for Windar, info, and the Windar Uninstaller."
!endif
!ifdef OPTION_SECTION_SC_DESKTOP
   !insertmacro MUI_DESCRIPTION_TEXT ${SEC_DESKTOP} "Desktop shortcut for Windar."
!endif
!ifdef OPTION_SECTION_SC_QUICK_LAUNCH
   !insertmacro MUI_DESCRIPTION_TEXT ${SEC_QUICK_LAUNCH} "Quick Launch shortcut for Windar."
!endif
!ifdef OPTION_SECTION_SC_START_MENU_STARTUP
   !insertmacro MUI_DESCRIPTION_TEXT ${SEC_STARTUP_FOLDER} "Startup folder shortcut to start Windar on login."
!endif

!insertmacro MUI_FUNCTION_DESCRIPTION_END

Section -post

   ;Uninstaller file.
   SetDetailsPrint textonly
   DetailPrint "Writing Uninstaller"
   SetDetailsPrint listonly
   WriteUninstaller $INSTDIR\uninstall.exe

   ;Write the required erl.ini file.
   SetOutPath "$INSTDIR\minimerl\bin"
   File temp\erlini.exe
   ExecWait '"$INSTDIR\minimerl\bin\erlini.exe"'
   Delete "$INSTDIR\minimerl\bin\erlini.exe"

   ;Registry keys required for installer version handling and uninstaller.
   SetDetailsPrint textonly
   DetailPrint "Writing Installer Registry Keys"
   SetDetailsPrint listonly

   ;Version numbers used to detect existing installation version for comparisson.
   WriteRegStr HKLM "Software\Windar" "" $INSTDIR
   WriteRegDWORD HKLM "Software\Windar" "VersionMajor" "${VER_MAJOR}"
   WriteRegDWORD HKLM "Software\Windar" "VersionMinor" "${VER_MINOR}"
   WriteRegDWORD HKLM "Software\Windar" "VersionRevision" "${VER_REVISION}"
   WriteRegDWORD HKLM "Software\Windar" "VersionBuild" "${VER_BUILD}"

   ;Add or Remove Programs entry.
   WriteRegExpandStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Windar" "UninstallString" '"$INSTDIR\uninstall.exe"'
   WriteRegExpandStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Windar" "InstallLocation" "$INSTDIR"
   WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Windar" "DisplayName" "Windar"
   WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Windar" "Publisher" "Playnode projects (Open Source)"
   WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Windar" "DisplayIcon" "$INSTDIR\uninstall.exe,0"
   WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Windar" "DisplayVersion" "${VERSION}"
   WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Windar" "VersionMajor" "${VER_MAJOR}"
   WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Windar" "VersionMinor" "${VER_MINOR}.${VER_REVISION}"
   WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Windar" "URLInfoAbout" "http://windar.org/"
   WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Windar" "HelpLink" "http://playnode.org/"
   WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Windar" "NoModify" "1"
   WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Windar" "NoRepair" "1"

   SetDetailsPrint textonly
   DetailPrint "Installation Complete"
   SetDetailsPrint listonly

   SetDetailsPrint textonly
   DetailPrint "Finsihed."
SectionEnd

##############################################################################
#                                                                            #
#   UNINSTALLER SECTION                                                      #
#                                                                            #
##############################################################################

Var UnPageProfileDialog
Var UnPageProfileCheckbox
Var UnPageProfileCheckbox_State
Var UnPageProfileEditBox

Function un.UnPageProfile
   !insertmacro MUI_HEADER_TEXT "Uninstall Windar" "Remove Windar's profile folder from your computer."
   nsDialogs::Create /NOUNLOAD 1018
   Pop $UnPageProfileDialog

   ${If} $UnPageProfileDialog == error
      Abort
   ${EndIf}

   ${NSD_CreateLabel} 0 0 100% 12u "Do you want to delete the profile folder?"
   Pop $0

   ${NSD_CreateText} 0 13u 100% 12u "$APPDATA\Windar"
   Pop $UnPageProfileEditBox
   SendMessage $UnPageProfileEditBox ${EM_SETREADONLY} 1 0

   ${NSD_CreateLabel} 0 46u 100% 24u "Leave unchecked to keep the profile folder for later use or check to delete the profile folder."
   Pop $0

   ${NSD_CreateCheckbox} 0 71u 100% 8u "Yes, also delete the profile folder."
   Pop $UnPageProfileCheckbox

   nsDialogs::Show
FunctionEnd

Function un.UnPageProfileLeave
   ${NSD_GetState} $UnPageProfileCheckbox $UnPageProfileCheckbox_State
FunctionEnd

Section Uninstall
   IfFileExists $INSTDIR\Windar.exe windar_installed
      MessageBox MB_YESNO "It does not appear that Windar is installed in the directory '$INSTDIR'.$\r$\nContinue anyway (not recommended)?" IDYES windar_installed
      Abort "Uninstall aborted by user"
   windar_installed:
   
   ;Shutdown Windar in case Add/Remove re-installer option used.
   Call un.ShutdownWindarTray
   Call un.KillErlang

   ;Delete registry keys.
   DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Windar"
   DeleteRegValue HKLM "Software\Windar" "VersionBuild"
   DeleteRegValue HKLM "Software\Windar" "VersionMajor"
   DeleteRegValue HKLM "Software\Windar" "VersionMinor"
   DeleteRegValue HKLM "Software\Windar" "VersionRevision"
   DeleteRegValue HKLM "Software\Windar" ""
   DeleteRegKey HKLM "Software\Windar"

   ;Start menu shortcuts.
   !ifdef OPTION_SECTION_SC_START_MENU
      SetShellVarContext all
      RMDir /r "$SMPROGRAMS\Windar"
      SetShellVarContext current
   !endif

   ;Startup folder shortcuts.
   IfFileExists "$SMPROGRAMS\Startup\Windar.lnk" 0 +2
      Delete "$SMPROGRAMS\Startup\Windar.lnk"
   IfFileExists "$SMPROGRAMS\Startup\Playdar Core.lnk" 0 +2
      Delete "$SMPROGRAMS\Startup\Playdar Core.lnk"

   ;Desktop shortcut.
   !ifdef OPTION_SECTION_SC_DESKTOP
      IfFileExists "$DESKTOP\Windar.lnk" 0 +2
         Delete "$DESKTOP\Windar.lnk"
   !endif

   ;Quick Launch shortcut.
   !ifdef OPTION_SECTION_SC_QUICK_LAUNCH
      IfFileExists "$QUICKLAUNCH\Windar.lnk" 0 +2
         Delete "$QUICKLAUNCH\Windar.lnk"
   !endif

   ;Remove all the Program Files.
   RMDir /r $INSTDIR\minimerl
   RMDir /r $INSTDIR\playdar
   RMDir /r $INSTDIR
  
   ;Uninstall User Data if option is checked, otherwise skip.
   ${If} $UnPageProfileCheckbox_State == ${BST_CHECKED}
      RMDir /r "$APPDATA\Windar"
   ${EndIf}

   SetDetailsPrint textonly
   DetailPrint "Finsihed."
SectionEnd

##############################################################################
#                                                                            #
#   NSIS Event Handler Functions                                             #
#                                                                            #
##############################################################################

Function .onInit
   !insertmacro INSTALLOPTIONS_EXTRACT "windar.ini"

   ;Warn user if system is older than Windows XP.
   ${IfNot} ${AtLeastWinXP}
      MessageBox MB_OK "Unsupported on anything older than Windows XP."
      ;UAC::Unload
      ;Quit
   ${EndIf}

   ;Remove Quick Launch option from Windows 7 as no longer applicable.
   ${IfNot} ${AtMostWinVista}
      SectionSetText ${SEC_QUICK_LAUNCH} "Quick Launch Shortcut (N/A)"
      SectionSetFlags ${SEC_QUICK_LAUNCH} ${SF_RO}
      SectionSetInstTypes ${SEC_QUICK_LAUNCH} 0
   ${EndIf}

   ${MementoSectionRestore}

   UAC_Elevate:
       UAC::RunElevated 
       StrCmp 1223 $0 UAC_ElevationAborted ; UAC dialog aborted by user?
       StrCmp 0 $0 0 UAC_Err ; Error?
       StrCmp 1 $1 0 UAC_Success ;Are we the real deal or just the wrapper?
       Quit
    
   UAC_Err:
       MessageBox mb_iconstop "Unable to elevate, error $0"
       Abort
    
   UAC_ElevationAborted:
       # elevation was aborted, run as normal?
       ;MessageBox mb_iconstop "This installer requires admin access, aborting!"
       Abort
    
   UAC_Success:
       StrCmp 1 $3 +4 ;Admin?
       StrCmp 3 $1 0 UAC_ElevationAborted ;Try again?
       MessageBox mb_iconstop "This installer requires admin access, try again"
       goto UAC_Elevate 

   ;Prevent multiple instances.
   System::Call 'kernel32::CreateMutexA(i 0, i 0, t "windarInstaller") i .r1 ?e'
   Pop $R0
   StrCmp $R0 0 +3
      MessageBox MB_OK|MB_ICONEXCLAMATION "The installer is already running."
      Abort
FunctionEnd

Function .onInstSuccess
   ${MementoSectionSave}
    UAC::Unload ;Must call unload!
FunctionEnd

Function .onInstFailed
    UAC::Unload ;Must call unload!
FunctionEnd
