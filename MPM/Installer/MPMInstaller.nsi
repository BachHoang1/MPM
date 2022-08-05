# author: hoan chau
# purpose: installs and uninstalls mpm application along with any associated files and folders

!include "nsDialogs.nsh"
!include "winmessages.nsh"
!include "logiclib.nsh"
!include "MUI2.nsh"

!define MUI_ICON "logo.ico"
!define MUI_HEADERIMAGE
!define MUI_HEADERIMAGE_BITMAP "logo.png"
!define MUI_HEADERIMAGE_RIGHT

Name "Display 2.0"
# define name of installer
OutFile "APS_Display_Installer.exe"
 
# define installation directory
InstallDir "$PROGRAMFILES32\Applied Physics Systems\Display 2.0"
 
# For removing Start Menu shortcut in Windows 7
RequestExecutionLevel admin

# verify application is not currently running
!define MUI_PAGE_CUSTOMFUNCTION_SHOW eulaPage
!insertmacro MUI_PAGE_LICENSE eula.rtf
Page Custom eulaPage
 
Var dialog
Var hwnd
 
Function eulaPage
ClearErrors
	FileOpen $0 $EXEDIR r
	IfErrors exit
	System::Call 'kernel32::GetFileSize(i r0, i 0) i .r1'
	IntOp $1 $1 + 1 ; for terminating zero
	System::Alloc $1
	Pop $2
	System::Call 'kernel32::ReadFile(i r0, i r2, i r1, *i .r3, i 0)'
	FileClose $0
	FindWindow $0 "#32770" "" $HWNDPARENT
	GetDlgItem $0 $0 1000
	SendMessage $0 ${EM_SETLIMITTEXT} $1 0
	SendMessage $0 ${WM_SETTEXT} 0 $2
	System::Free $2
exit:
FunctionEnd

Section sectionEulaPage

SectionEnd

Page Custom pgServerEnter pgServerLeave
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_LANGUAGE "English"


var server
 
var radioServerSelection
var radioClientSelection
 
Function pgServerEnter
!insertmacro MUI_HEADER_TEXT "This installs Display as a server or client" "There can only be one Display setup as a Server" 
	nsDialogs::Create 1018
		Pop $dialog
	${NSD_CreateRadioButton} 0 0 40% 6% "Server Setup"
		Pop $radioServerSelection
		${NSD_AddStyle} $radioServerSelection ${WS_GROUP}
		${NSD_OnClick} $radioServerSelection RadioClick
	${NSD_CreateRadioButton} 0 12% 40% 6% "Client Setup"
		Pop $radioClientSelection
		${NSD_OnClick} $radioClientSelection RadioClick

 	#default to server
    	${NSD_Check} $radioServerSelection
	StrCpy $server "1"

	nsDialogs::Show
FunctionEnd
 
Function RadioClick
	Pop $hwnd
	${If} $hwnd == $radioServerSelection
	    #MessageBox MB_OK "onClick:radioServerSelection"
	    StrCpy $server "1"
	${ElseIf} $hwnd == $radioClientSelection
	    #MessageBox MB_OK "onClick:radioClientSelection"
        StrCpy $server "0"
	${EndIf}
FunctionEnd

Function pgServerLeave
		
FunctionEnd

Section server
	#${If} $hwnd == $radioServerSelection
	#    MessageBox MB_OK "onClick:radioServerSelection"
	#${ElseIf} $hwnd == $radioClientSelection
	#    MessageBox MB_OK "onClick:radioClientSelection"
	#${EndIf}
SectionEnd

# start default section
Section "install"
        
    FindProcDLL::FindProc "MPM.exe"
    IntCmp $R0 1 0 notRunning
    MessageBox MB_OK|MB_ICONEXCLAMATION "Display 2.0 is running. Please close it first before installing." /SD IDOK
       Abort
    notRunning:
 
    #GetFileTime C:\APS\Data\Log.db $0 $1

    CreateDirectory C:\APS\backup
    #CopyFiles C:\APS\Data\Log.db C:\APS\backup\$0_$1_Log.db

    # data folder so that settings can be saved.  can't use program files folder; and roaming folder is too complicated
    CreateDirectory C:\APS\Data
    CreateDirectory C:\APS\Data\Jobs
	CreateDirectory C:\APS\Data\Jobs\Default

    SetOutPath C:\APS\Data    
    File BlankLog.db	
	File .\Data\XMLFileSettingsDefault.xml
	File .\Data\XMLFileProtocolCommandsDefault.xml
    File .\Data\XMLFileWITSDefault.xml	
	
	SetOverwrite ifnewer
	File .\Data\DockPanel.config
	File .\Data\DockPanel.temp.config

    ${If} $server == "1"
        File .\Data\Server\XMLFileFormSettingsDefault.xml
    ${ElseIf} $server == "0"
        File .\Data\Client\XMLFileFormSettingsDefault.xml
    ${EndIf}
    
	SetOutPath C:\APS\Data\Jobs\Default
	File Log.db 
	File .\Data\XMLFileProtocolCommandsDefault.xml
    File .\Data\XMLFileWITSDefault.xml
	${If} $server == "1"
        File .\Data\Server\XMLFileFormSettingsDefault.xml
    ${ElseIf} $server == "0"
        File .\Data\Client\XMLFileFormSettingsDefault.xml
    ${EndIf}
		
	Rename C:\APS\Data\Jobs\Default\XMLFileProtocolCommandsDefault.xml C:\APS\Data\Jobs\Default\XMLFileProtocolCommands.xml
	Rename C:\APS\Data\Jobs\Default\XMLFileWITSDefault.xml C:\APS\Data\Jobs\Default\XMLFileWITS.xml
	Rename C:\APS\Data\Jobs\Default\XMLFileFormSettingsDefault.xml C:\APS\Data\Jobs\Default\XMLFileFormSettings.xml
	
    # set the installation directory as the destination for the following actions
    SetOutPath $INSTDIR
    
    SetOverwrite ifnewer
    File MPM.exe
    File MPM.pdb
    File MPM.exe.config
	File EntityFramework.dll
    File SQLite.Designer.dll
    File SQLite.Interop.dll
    File System.Data.SQLite.dll
	File System.Data.SQLite.EF6.dll
	File System.Data.SQLite.Linq.dll
    File Eula.rtf
    File Microsoft.AspNetCore.Connections.Abstractions.dll
    File Microsoft.AspNetCore.Connections.Abstractions.xml           
    File Microsoft.AspNetCore.Http.Connections.Client.dll
    File Microsoft.AspNetCore.Http.Connections.Client.xml            
    File Microsoft.AspNetCore.Http.Connections.Common.dll
    File Microsoft.AspNetCore.Http.Connections.Common.xml            
    File Microsoft.AspNetCore.Http.Features.dll
    File Microsoft.AspNetCore.Http.Features.xml                      
    File Microsoft.AspNetCore.SignalR.Client.Core.dll
    File Microsoft.AspNetCore.SignalR.Client.Core.xml                
    File Microsoft.AspNetCore.SignalR.Client.dll
    File Microsoft.AspNetCore.SignalR.Client.xml                     
    File Microsoft.AspNetCore.SignalR.Common.dll
    File Microsoft.AspNetCore.SignalR.Common.xml                     
    File Microsoft.AspNetCore.SignalR.Protocols.Json.dll
    File Microsoft.AspNetCore.SignalR.Protocols.Json.xml             
    File Microsoft.Bcl.AsyncInterfaces.dll
    File Microsoft.Bcl.AsyncInterfaces.xml                           
    File Microsoft.Extensions.Configuration.Abstractions.dll
    File Microsoft.Extensions.Configuration.Abstractions.xml         
    File Microsoft.Extensions.Configuration.Binder.dll
    File Microsoft.Extensions.Configuration.Binder.xml               
    File Microsoft.Extensions.Configuration.dll
    File Microsoft.Extensions.Configuration.xml                      
    File Microsoft.Extensions.DependencyInjection.Abstractions.dll
    File Microsoft.Extensions.DependencyInjection.Abstractions.xml   
    File Microsoft.Extensions.DependencyInjection.dll
    File Microsoft.Extensions.DependencyInjection.xml                
    File Microsoft.Extensions.Logging.Abstractions.dll
    File Microsoft.Extensions.Logging.Abstractions.xml               
    File Microsoft.Extensions.Logging.dll
    File Microsoft.Extensions.Logging.xml                            
    File Microsoft.Extensions.Options.dll
    File Microsoft.Extensions.Options.xml                            
    File Microsoft.Extensions.Primitives.dll
    File Microsoft.Extensions.Primitives.xml                         
    File Microsoft.Win32.Primitives.dll
    File netstandard.dll                                             
    File Newtonsoft.Json.dll
    File Newtonsoft.Json.xml                                                                   
    File System.AppContext.dll
    File System.Buffers.dll                                          
    File System.Buffers.xml
    File System.Collections.Concurrent.dll                           
    File System.Collections.dll
    File System.Collections.NonGeneric.dll                           
    File System.Collections.Specialized.dll
    File System.ComponentModel.Annotations.dll                       
    File System.ComponentModel.dll
    File System.ComponentModel.EventBasedAsync.dll                   
    File System.ComponentModel.Primitives.dll
    File System.ComponentModel.TypeConverter.dll                     
    File System.Console.dll
    File System.Data.Common.dll                                      
    File System.Diagnostics.Contracts.dll                            
    File System.Diagnostics.Debug.dll
    File System.Diagnostics.FileVersionInfo.dll                      
    File System.Diagnostics.Process.dll
    File System.Diagnostics.StackTrace.dll                           
    File System.Diagnostics.TextWriterTraceListener.dll
    File System.Diagnostics.Tools.dll                                
    File System.Diagnostics.TraceSource.dll
    File System.Diagnostics.Tracing.dll                              
    File System.Drawing.Primitives.dll
    File DSPLib.dll
    File DSPLib.pdb
    File System.Dynamic.Runtime.dll                                  
    File System.Globalization.Calendars.dll
    File System.Globalization.dll                                    
    File System.Globalization.Extensions.dll
    File System.IO.Compression.dll                                   
    File System.IO.Compression.ZipFile.dll
    File System.IO.dll                                               
    File System.IO.FileSystem.dll
    File System.IO.FileSystem.DriveInfo.dll                          
    File System.IO.FileSystem.Primitives.dll
    File System.IO.FileSystem.Watcher.dll                            
    File System.IO.IsolatedStorage.dll
    File System.IO.MemoryMappedFiles.dll                             
    File System.IO.Pipelines.dll
    File System.IO.Pipelines.xml                                     
    File System.IO.Pipes.dll
    File System.IO.UnmanagedMemoryStream.dll                         
    File System.Linq.dll
    File System.Linq.Expressions.dll                                 
    File System.Linq.Parallel.dll
    File System.Linq.Queryable.dll                                   
    File System.Memory.dll
    File System.Memory.xml                                           
    File System.Net.Http.dll
    File System.Net.NameResolution.dll                               
    File System.Net.NetworkInformation.dll
    File System.Net.Ping.dll                                         
    File System.Net.Primitives.dll
    File System.Net.Requests.dll                                     
    File System.Net.Security.dll
    File System.Net.Sockets.dll                                      
    File System.Net.WebHeaderCollection.dll
    File System.Net.WebSockets.Client.dll                            
    File System.Net.WebSockets.dll
    File System.Numerics.Vectors.dll                                 
    File System.Numerics.Vectors.xml
    File System.ObjectModel.dll                                      
    File System.Reflection.dll
    File System.Reflection.Extensions.dll                            
    File System.Reflection.Primitives.dll
    File System.Resources.Reader.dll                                 
    File System.Resources.ResourceManager.dll
    File System.Resources.Writer.dll                                 
    File System.Runtime.CompilerServices.Unsafe.dll
    File System.Runtime.CompilerServices.Unsafe.xml                  
    File System.Runtime.CompilerServices.VisualC.dll
    File System.Runtime.dll                                          
    File System.Runtime.Extensions.dll
    File System.Runtime.Handles.dll                                  
    File System.Runtime.InteropServices.dll
    File System.Runtime.InteropServices.RuntimeInformation.dll       
    File System.Runtime.Numerics.dll
    File System.Runtime.Serialization.Formatters.dll                 
    File System.Runtime.Serialization.Json.dll
    File System.Runtime.Serialization.Primitives.dll                 
    File System.Runtime.Serialization.Xml.dll
    File System.Security.Claims.dll                                  
    File System.Security.Cryptography.Algorithms.dll
    File System.Security.Cryptography.Csp.dll                        
    File System.Security.Cryptography.Encoding.dll
    File System.Security.Cryptography.Primitives.dll                 
    File System.Security.Cryptography.X509Certificates.dll
    File System.Security.Principal.dll                               
    File System.Security.SecureString.dll
    File System.Text.Encoding.dll                                    
    File System.Text.Encoding.Extensions.dll
    File System.Text.Encodings.Web.dll                               
    File System.Text.Encodings.Web.xml
    File System.Text.Json.dll                                        
    File System.Text.Json.xml
    File System.Text.RegularExpressions.dll                          
    File System.Threading.Channels.dll
    File System.Threading.Channels.xml                               
    File System.Threading.dll
    File System.Threading.Overlapped.dll                             
    File System.Threading.Tasks.dll
    File System.Threading.Tasks.Extensions.dll                       
    File System.Threading.Tasks.Extensions.xml
    File System.Threading.Tasks.Parallel.dll                         
    File System.Threading.Thread.dll
    File System.Threading.ThreadPool.dll                             
    File System.Threading.Timer.dll
    File System.ValueTuple.dll                                       
    File System.ValueTuple.xml
    File System.Xml.ReaderWriter.dll                                 
    File System.Xml.XDocument.dll
    File System.Xml.XmlDocument.dll                                  
    File System.Xml.XmlSerializer.dll
    File System.Xml.XPath.dll                                        
    File System.Xml.XPath.XDocument.dll
	File WeifenLuo.WinFormsUI.Docking.ThemeVS2015.dll
	File WinFormsUI.dll
	File attributes.txt

    # create the uninstaller
    WriteUninstaller "$INSTDIR\uninstall.exe"
 
    # create a shortcut named "new shortcut" in the start menu programs directory
    # point the new shortcut at the program uninstaller
    CreateShortCut "$SMPROGRAMS\Display.lnk" "$INSTDIR\MPM.exe" logo.png
    Delete "$DESKTOP\Display.lnk"
    Delete "$DESKTOP\Display 2.3.2.2.lnk"
    Delete "$DESKTOP\Display 2.3.3.3.lnk"
    Delete "$DESKTOP\Display 2.3.4.4.lnk"
    Delete "$DESKTOP\Display 2.3.5.5.lnk"
    Delete "$DESKTOP\Display 2.3.6.6.lnk"
    Delete "$DESKTOP\Display 2.3.7.7.lnk"
    Delete "$DESKTOP\Display 2.3.8.8.lnk"
    Delete "$DESKTOP\Display 2.3.9.9.lnk"
    Delete "$DESKTOP\Display 2.3.10.10.lnk"
    Delete "$DESKTOP\Display 2.3.11.11.lnk"
    Delete "$DESKTOP\Display 2.4.0.0.lnk"
    Delete "$DESKTOP\Display 2.4.1.1.lnk"
    Delete "$DESKTOP\Display 2.4.2.2.lnk"
    Delete "$DESKTOP\Display 2.4.3.3.lnk"
    Delete "$DESKTOP\Display 2.4.4.4.lnk"
    Delete "$DESKTOP\Display 2.5.0.0.lnk"
    Delete "$DESKTOP\Display 2.5.1.1.lnk"
    Delete "$DESKTOP\Display 2.5.2.2.lnk"
    Delete "$DESKTOP\Display 2.5.3.3.lnk"
    Delete "$DESKTOP\Display 2.5.4.4.lnk"
    Delete "$DESKTOP\Display 2.5.5.5.lnk"
    Delete "$DESKTOP\Display 2.5.6.6.lnk"
    Delete "$DESKTOP\Display 2.5.7.7.lnk"
    Delete "$DESKTOP\Display 2.5.8.8.lnk"
    Delete "$DESKTOP\Display 2.5.9.9.lnk"
    Delete "$DESKTOP\Display 2.5.10.10.lnk"
    Delete "$DESKTOP\Display 2.5.11.11.lnk"
    Delete "$DESKTOP\Display 2.5.12.12.lnk"
    Delete "$DESKTOP\Display 2.5.13.13.lnk"
    Delete "$DESKTOP\Display 2.5.14.14.lnk"
    Delete "$DESKTOP\Display 2.5.15.15.lnk"
    Delete "$DESKTOP\Display 2.5.17.17.lnk"
    Delete "$DESKTOP\Display 2.5.18.18.lnk"
    Delete "$DESKTOP\Display 2.5.19.19.lnk"
    Delete "$DESKTOP\Display 2.5.20.20.lnk"
    Delete "$DESKTOP\Display 2.5.21.21.lnk"
    Delete "$DESKTOP\Display 2.5.22.22.lnk"
    Delete "$DESKTOP\Display 2.6.0.0.lnk"
    Delete "$DESKTOP\Display 2.6.1.1.lnk"
    Delete "$DESKTOP\Display 2.6.2.2.lnk"
    Delete "$DESKTOP\Display 2.6.3.3.lnk"
    Delete "$DESKTOP\Display 2.6.4.4.lnk"
    Delete "$DESKTOP\Display 2.6.5.5.lnk"
    Delete "$DESKTOP\Display 2.6.5.6.lnk"
    Delete "$DESKTOP\Display 2.6.6.7.lnk"
    Delete "$DESKTOP\Display 2.6.7.8.lnk"
    Delete "$DESKTOP\Display 2.6.8.9.lnk"
	Delete "$DESKTOP\Display 2.6.9.10.lnk"
	Delete "$DESKTOP\Display 2.6.10.11.lnk"
	Delete "$DESKTOP\Display 2.6.11.12.lnk"
	Delete "$DESKTOP\Display 2.7.0.0.lnk"
	Delete "$DESKTOP\Display 2.7.1.1.lnk"
	Delete "$DESKTOP\Display 2.7.2.2.lnk"
	Delete "$DESKTOP\Display 2.7.3.3.lnk"
	Delete "$DESKTOP\Display 2.8.0.0.lnk"
	Delete "$DESKTOP\Display 2.8.1.1.lnk"
	Delete "$DESKTOP\Display 2.8.2.2.lnk"
	Delete "$DESKTOP\Display 2.8.3.3.lnk"
	Delete "$DESKTOP\Display 2.8.4.4.lnk"
	Delete "$DESKTOP\Display 2.8.5.5.lnk"
	Delete "$DESKTOP\Display 2.8.6.6.lnk"
	Delete "$DESKTOP\Display 2.8.7.7.lnk"
	Delete "$DESKTOP\Display 2.8.8.8.lnk"
	Delete "$DESKTOP\Display 2.8.9.9.lnk"
	Delete "$DESKTOP\Display 2.8.10.10.lnk"
	Delete "$DESKTOP\Display 2.9.0.0.lnk"
	Delete "$DESKTOP\Display 2.9.1.1.lnk"
	Delete "$DESKTOP\Display 2.9.2.2.lnk"
	Delete "$DESKTOP\Display 2.9.3.3.lnk"
	Delete "$DESKTOP\Display 2.9.4.4.lnk"
	Delete "$DESKTOP\Display 2.10.0.0.lnk"
	Delete "$DESKTOP\Display 2.10.1.1.lnk"
	Delete "$DESKTOP\Display 2.10.2.2.lnk"
	Delete "$DESKTOP\Display 2.10.3.3.lnk"
	Delete "$DESKTOP\Display 2.10.4.4.lnk"
	Delete "$DESKTOP\Display 2.10.5.5.lnk"
        Delete "$DESKTOP\Display 2.10.6.6.lnk"
        Delete "$DESKTOP\Display 2.10.6.7.lnk"
    CreateShortCut "$DESKTOP\Display 2.10.6.8.lnk" "$INSTDIR\MPM.exe" logo.png    
    
SectionEnd
 
# uninstaller section start
Section "uninstall"
 
    FindProcDLL::FindProc "MPM.exe"
    IntCmp $R0 1 0 notRunning
    MessageBox MB_OK|MB_ICONEXCLAMATION "Display 2.0 is running. Please close it first before uninstalling." /SD IDOK
       Abort
    notRunning:


    # delete the uninstaller
    Delete "$INSTDIR\uninstall.exe"
 
    # remove the links from the start menu, etc.
    Delete "$SMPROGRAMS\Display.lnk"
    Delete "$DESKTOP\Display 2.10.6.7.lnk"

    # delete files
    Delete "$INSTDIR\MPM.exe"
    Delete "$INSTDIR\MPM.pdb"
    Delete "$INSTDIR\MPM.exe.config"
	Delete "$INSTDIR\EntityFramework.dll"
    Delete "$INSTDIR\SQLite.Designer.dll"
    Delete "$INSTDIR\SQLite.Interop.dll"
    Delete "$INSTDIR\System.Data.SQLite.dll"
	Delete "$INSTDIR\System.Data.SQLite.EF6.dll"
	Delete "$INSTDIR\System.Data.SQLite.Linq.dll"
    Delete "C:\APS\Data\XMLFileFormSettingsDefault.xml"
    Delete "C:\APS\Data\XMLFileProtocolCommandsDefault.xml"
    Delete "C:\APS\Data\XMLFileWITSDefault.xml"
	Delete "C:\APS\Data\XMLFileSettingsDefault.xml"
    Delete "$INSTDIR\eula.rtf"
    Delete "$INSTDIR\Microsoft.AspNetCore.Connections.Abstractions.dll"
    Delete "$INSTDIR\Microsoft.AspNetCore.Connections.Abstractions.xml"           
    Delete "$INSTDIR\Microsoft.AspNetCore.Http.Connections.Client.dll"
    Delete "$INSTDIR\Microsoft.AspNetCore.Http.Connections.Client.xml"            
    Delete "$INSTDIR\Microsoft.AspNetCore.Http.Connections.Common.dll"
    Delete "$INSTDIR\Microsoft.AspNetCore.Http.Connections.Common.xml"            
    Delete "$INSTDIR\Microsoft.AspNetCore.Http.Features.dll"
    Delete "$INSTDIR\Microsoft.AspNetCore.Http.Features.xml"                      
    Delete "$INSTDIR\Microsoft.AspNetCore.SignalR.Client.Core.dll"
    Delete "$INSTDIR\Microsoft.AspNetCore.SignalR.Client.Core.xml"                
    Delete "$INSTDIR\Microsoft.AspNetCore.SignalR.Client.dll"
    Delete "$INSTDIR\Microsoft.AspNetCore.SignalR.Client.xml"                     
    Delete "$INSTDIR\Microsoft.AspNetCore.SignalR.Common.dll"
    Delete "$INSTDIR\Microsoft.AspNetCore.SignalR.Common.xml"                     
    Delete "$INSTDIR\Microsoft.AspNetCore.SignalR.Protocols.Json.dll"
    Delete "$INSTDIR\Microsoft.AspNetCore.SignalR.Protocols.Json.xml"             
    Delete "$INSTDIR\Microsoft.Bcl.AsyncInterfaces.dll"
    Delete "$INSTDIR\Microsoft.Bcl.AsyncInterfaces.xml"                           
    Delete "$INSTDIR\Microsoft.Extensions.Configuration.Abstractions.dll"
    Delete "$INSTDIR\Microsoft.Extensions.Configuration.Abstractions.xml"         
    Delete "$INSTDIR\Microsoft.Extensions.Configuration.Binder.dll"
    Delete "$INSTDIR\Microsoft.Extensions.Configuration.Binder.xml"               
    Delete "$INSTDIR\Microsoft.Extensions.Configuration.dll"
    Delete "$INSTDIR\Microsoft.Extensions.Configuration.xml"                      
    Delete "$INSTDIR\Microsoft.Extensions.DependencyInjection.Abstractions.dll"
    Delete "$INSTDIR\Microsoft.Extensions.DependencyInjection.Abstractions.xml"   
    Delete "$INSTDIR\Microsoft.Extensions.DependencyInjection.dll"
    Delete "$INSTDIR\Microsoft.Extensions.DependencyInjection.xml"                
    Delete "$INSTDIR\Microsoft.Extensions.Logging.Abstractions.dll"
    Delete "$INSTDIR\Microsoft.Extensions.Logging.Abstractions.xml"               
    Delete "$INSTDIR\Microsoft.Extensions.Logging.dll"
    Delete "$INSTDIR\Microsoft.Extensions.Logging.xml"                            
    Delete "$INSTDIR\Microsoft.Extensions.Options.dll"
    Delete "$INSTDIR\Microsoft.Extensions.Options.xml"                            
    Delete "$INSTDIR\Microsoft.Extensions.Primitives.dll"
    Delete "$INSTDIR\Microsoft.Extensions.Primitives.xml"                         
    Delete "$INSTDIR\Microsoft.Win32.Primitives.dll"
    Delete "$INSTDIR\netstandard.dll"                                             
    Delete "$INSTDIR\Newtonsoft.Json.dll"
    Delete "$INSTDIR\Newtonsoft.Json.xml"                                                                   
    Delete "$INSTDIR\System.AppContext.dll"
    Delete "$INSTDIR\System.Buffers.dll"                                          
    Delete "$INSTDIR\System.Buffers.xml"
    Delete "$INSTDIR\System.Collections.Concurrent.dll"                           
    Delete "$INSTDIR\System.Collections.dll"
    Delete "$INSTDIR\System.Collections.NonGeneric.dll"                           
    Delete "$INSTDIR\System.Collections.Specialized.dll"
    Delete "$INSTDIR\System.ComponentModel.Annotations.dll"                       
    Delete "$INSTDIR\System.ComponentModel.dll"
    Delete "$INSTDIR\System.ComponentModel.EventBasedAsync.dll"                   
    Delete "$INSTDIR\System.ComponentModel.Primitives.dll"
    Delete "$INSTDIR\System.ComponentModel.TypeConverter.dll"                     
    Delete "$INSTDIR\System.Console.dll"
    Delete "$INSTDIR\System.Data.Common.dll"                                      
    Delete "$INSTDIR\System.Diagnostics.Contracts.dll"                            
    Delete "$INSTDIR\System.Diagnostics.Debug.dll"
    Delete "$INSTDIR\System.Diagnostics.FileVersionInfo.dll"                      
    Delete "$INSTDIR\System.Diagnostics.Process.dll"
    Delete "$INSTDIR\System.Diagnostics.StackTrace.dll"                           
    Delete "$INSTDIR\System.Diagnostics.TextWriterTraceListener.dll"
    Delete "$INSTDIR\System.Diagnostics.Tools.dll"                                
    Delete "$INSTDIR\System.Diagnostics.TraceSource.dll"
    Delete "$INSTDIR\System.Diagnostics.Tracing.dll"                              
    Delete "$INSTDIR\System.Drawing.Primitives.dll"
    Delete "$INSTDIR\DSPLib.dll"
    Delete "$INSTDIR\DSPLib.pdb"
    Delete "$INSTDIR\System.Dynamic.Runtime.dll"                                  
    Delete "$INSTDIR\System.Globalization.Calendars.dll"
    Delete "$INSTDIR\System.Globalization.dll"                                    
    Delete "$INSTDIR\System.Globalization.Extensions.dll"
    Delete "$INSTDIR\System.IO.Compression.dll"                                   
    Delete "$INSTDIR\System.IO.Compression.ZipFile.dll"
    Delete "$INSTDIR\System.IO.dll"                                               
    Delete "$INSTDIR\System.IO.FileSystem.dll"
    Delete "$INSTDIR\System.IO.FileSystem.DriveInfo.dll"                          
    Delete "$INSTDIR\System.IO.FileSystem.Primitives.dll"
    Delete "$INSTDIR\System.IO.FileSystem.Watcher.dll"                            
    Delete "$INSTDIR\System.IO.IsolatedStorage.dll"
    Delete "$INSTDIR\System.IO.MemoryMappedFiles.dll"                             
    Delete "$INSTDIR\System.IO.Pipelines.dll"
    Delete "$INSTDIR\System.IO.Pipelines.xml"                                     
    Delete "$INSTDIR\System.IO.Pipes.dll"
    Delete "$INSTDIR\System.IO.UnmanagedMemoryStream.dll"                         
    Delete "$INSTDIR\System.Linq.dll"
    Delete "$INSTDIR\System.Linq.Expressions.dll"                                 
    Delete "$INSTDIR\System.Linq.Parallel.dll"
    Delete "$INSTDIR\System.Linq.Queryable.dll"                                   
    Delete "$INSTDIR\System.Memory.dll"
    Delete "$INSTDIR\System.Memory.xml"                                           
    Delete "$INSTDIR\System.Net.Http.dll"
    Delete "$INSTDIR\System.Net.NameResolution.dll"                               
    Delete "$INSTDIR\System.Net.NetworkInformation.dll"
    Delete "$INSTDIR\System.Net.Ping.dll"                                         
    Delete "$INSTDIR\System.Net.Primitives.dll"
    Delete "$INSTDIR\System.Net.Requests.dll"                                     
    Delete "$INSTDIR\System.Net.Security.dll"
    Delete "$INSTDIR\System.Net.Sockets.dll"                                      
    Delete "$INSTDIR\System.Net.WebHeaderCollection.dll"
    Delete "$INSTDIR\System.Net.WebSockets.Client.dll"                            
    Delete "$INSTDIR\System.Net.WebSockets.dll"
    Delete "$INSTDIR\System.Numerics.Vectors.dll"                                 
    Delete "$INSTDIR\System.Numerics.Vectors.xml"
    Delete "$INSTDIR\System.ObjectModel.dll"                                      
    Delete "$INSTDIR\System.Reflection.dll"
    Delete "$INSTDIR\System.Reflection.Extensions.dll"                            
    Delete "$INSTDIR\System.Reflection.Primitives.dll"
    Delete "$INSTDIR\System.Resources.Reader.dll"                                 
    Delete "$INSTDIR\System.Resources.ResourceManager.dll"
    Delete "$INSTDIR\System.Resources.Writer.dll"                                 
    Delete "$INSTDIR\System.Runtime.CompilerServices.Unsafe.dll"
    Delete "$INSTDIR\System.Runtime.CompilerServices.Unsafe.xml"                  
    Delete "$INSTDIR\System.Runtime.CompilerServices.VisualC.dll"
    Delete "$INSTDIR\System.Runtime.dll"                                          
    Delete "$INSTDIR\System.Runtime.Extensions.dll"
    Delete "$INSTDIR\System.Runtime.Handles.dll"                                  
    Delete "$INSTDIR\System.Runtime.InteropServices.dll"
    Delete "$INSTDIR\System.Runtime.InteropServices.RuntimeInformation.dll"       
    Delete "$INSTDIR\System.Runtime.Numerics.dll"
    Delete "$INSTDIR\System.Runtime.Serialization.Formatters.dll"                 
    Delete "$INSTDIR\System.Runtime.Serialization.Json.dll"
    Delete "$INSTDIR\System.Runtime.Serialization.Primitives.dll"                 
    Delete "$INSTDIR\System.Runtime.Serialization.Xml.dll"
    Delete "$INSTDIR\System.Security.Claims.dll"                                  
    Delete "$INSTDIR\System.Security.Cryptography.Algorithms.dll"
    Delete "$INSTDIR\System.Security.Cryptography.Csp.dll"                        
    Delete "$INSTDIR\System.Security.Cryptography.Encoding.dll"
    Delete "$INSTDIR\System.Security.Cryptography.Primitives.dll"                 
    Delete "$INSTDIR\System.Security.Cryptography.X509Certificates.dll"
    Delete "$INSTDIR\System.Security.Principal.dll"                               
    Delete "$INSTDIR\System.Security.SecureString.dll"
    Delete "$INSTDIR\System.Text.Encoding.dll"                                    
    Delete "$INSTDIR\System.Text.Encoding.Extensions.dll"
    Delete "$INSTDIR\System.Text.Encodings.Web.dll"                               
    Delete "$INSTDIR\System.Text.Encodings.Web.xml"
    Delete "$INSTDIR\System.Text.Json.dll"                                        
    Delete "$INSTDIR\System.Text.Json.xml"
    Delete "$INSTDIR\System.Text.RegularExpressions.dll"                          
    Delete "$INSTDIR\System.Threading.Channels.dll"
    Delete "$INSTDIR\System.Threading.Channels.xml"                               
    Delete "$INSTDIR\System.Threading.dll"
    Delete "$INSTDIR\System.Threading.Overlapped.dll"                             
    Delete "$INSTDIR\System.Threading.Tasks.dll"
    Delete "$INSTDIR\System.Threading.Tasks.Extensions.dll"                       
    Delete "$INSTDIR\System.Threading.Tasks.Extensions.xml"
    Delete "$INSTDIR\System.Threading.Tasks.Parallel.dll"                         
    Delete "$INSTDIR\System.Threading.Thread.dll"
    Delete "$INSTDIR\System.Threading.ThreadPool.dll"                             
    Delete "$INSTDIR\System.Threading.Timer.dll"
    Delete "$INSTDIR\System.ValueTuple.dll"                                       
    Delete "$INSTDIR\System.ValueTuple.xml"
    Delete "$INSTDIR\System.Xml.ReaderWriter.dll"                                 
    Delete "$INSTDIR\System.Xml.XDocument.dll"
    Delete "$INSTDIR\System.Xml.XmlDocument.dll"                                  
    Delete "$INSTDIR\System.Xml.XmlSerializer.dll"
    Delete "$INSTDIR\System.Xml.XPath.dll"                                        
    Delete "$INSTDIR\System.Xml.XPath.XDocument.dll"
	Delete "$INSTDIR\WeifenLuo.WinFormsUI.Docking.ThemeVS2015.dll"
	Delete "$INSTDIR\WinFormsUI.dll"
	Delete "$INSTDIR\attributes.txt"

    # delete folders
    RMDir C:\APS\Data
    RMDir C:\APS
    RMDir $INSTDIR

# uninstaller section end
SectionEnd