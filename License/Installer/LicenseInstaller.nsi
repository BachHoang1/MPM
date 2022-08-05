# author: hoan chau
# purpose: installs and uninstalls license generator application along with dll's.  the license database is created by 
#          the app and will be located in c:\APSLicenseGenerator\Data
# define name of installer
OutFile "License_Generator_Installer.exe"
 
# define installation directory
InstallDir "$PROGRAMFILES32\Applied Physics Systems\LicenseGenerator"
 
# For removing Start Menu shortcut in Windows 7
RequestExecutionLevel admin
 
# start default section
Section
    FindProcDLL::FindProc "License.exe"
    IntCmp $R0 1 0 notRunning
    MessageBox MB_OK|MB_ICONEXCLAMATION "License.exe is running. Please close it first before installing." /SD IDOK
       Abort
    notRunning:
    
    # data folder so that settings can be saved.  can't use program files folder; and roaming folder is too complicated
    # SetOutPath C:\APSLicenseGenerator\Data  
    CreateDirectory C:\APSLicenseGenerator\Reports 

    # set the installation directory as the destination for the following actions
    SetOutPath $INSTDIR
    
    SetOverwrite ifnewer
    File License.exe    
    File License.exe.config
    File SQLite.Designer.dll
    File SQLite.Interop.dll
    File System.Data.SQLite.dll
    File EntityFramework.dll
    File EntityFramework.SqlServer.dll
    File System.Data.SQLite.EF6.dll
    File System.Data.SQLite.Linq.dll

    # create the uninstaller
    WriteUninstaller "$INSTDIR\uninstall.exe"
 
    # create a shortcut named "new shortcut" in the start menu programs directory
    # point the new shortcut at the program uninstaller
    CreateShortCut "$SMPROGRAMS\License Generator.lnk" "$INSTDIR\License.exe" logo.png
    CreateShortCut "$DESKTOP\License Generator.lnk" "$INSTDIR\License.exe" logo.png

SectionEnd
 
# uninstaller section start
Section "uninstall"
    FindProcDLL::FindProc "License.exe"
    IntCmp $R0 1 0 notRunning
    MessageBox MB_OK|MB_ICONEXCLAMATION "License.exe is running. Please close it first before installing." /SD IDOK
       Abort
    notRunning: 

    # delete the uninstaller
    Delete "$INSTDIR\uninstall.exe"
 
    # remove the links from the start menu, etc.
    Delete "$SMPROGRAMS\License Generator.lnk"
    Delete "$DESKTOP\License Generator.lnk"

    # delete files
    Delete "$INSTDIR\License.exe"
    Delete "$INSTDIR\License.exe.config"
    Delete "$INSTDIR\SQLite.Designer.dll"
    Delete "$INSTDIR\SQLite.Interop.dll"
    Delete "$INSTDIR\System.Data.SQLite.dll"    
    Delete "$INSTDIR\EntityFramework.dll"
    Delete "$INSTDIR\EntityFramework.SqlServer.dll"
    Delete "$INSTDIR\System.Data.SQLite.EF6.dll"
    Delete "$INSTDIR\System.Data.SQLite.Linq.dll"

    # delete folders
    #RMDir C:\APSLicenseGenerator\Data
    #RMDir C:\APSLicenseGenerator
    RMDir $INSTDIR

# uninstaller section end
SectionEnd