'--------------------------------------------------------------------------------
'	London Bridge Phoenix Software 
'	(C) Copyright 2004-2005
' 	Post Build Routine 
'	Author: Muthu Nagarathinam
'--------------------------------------------------------------------------------
'
' Globl Decleration
Dim g_myPath		' 
Dim g_objArgs
Dim g_fso
Dim g_wsShell
Dim g_logFile
Dim g_sdkPath
'


'
' Invoke Main
'
Main()
'
sub Main()
	'
	'
	Dim  sConfigName
	Dim sProjOutDir
	Dim sProjOutName
	Dim sProjOutExtension 
	Dim sCurrentFile
	
	Set g_wsShell = CreateObject("WScript.Shell")
	'
	

	' Validate the parameters 
	'

	Set objArgs = Wscript.Arguments
	if ( objArgs.Count <> 4 ) then
		Wscript.echo "COPY  cscript /NOLOGO $(SolutionDir)..\Common\PostBuild.vbs  $(ConfigurationName) $(TargetDir) $(TargetName) $(TargetExt)"
		WScript.Quit (1)
	end if
	'
	' Read the Input parameters
	'
	sConfigName = LCase(objArgs(0))
	sProjOutDir = LCase(objArgs(1))			
	sProjOutName = LCase(objArgs(2))
	sProjOutExtension = LCase(objArgs(3))
	'
	Set g_fso = CreateObject("Scripting.FileSystemObject")
	'
	g_myPath = g_fso.GetParentFolderName(Wscript.ScriptFullName)
	g_myPath = g_fso.GetParentFolderName(g_myPath)
	myDestPath = g_fso.GetParentFolderName(g_myPath) + "\Output\"
	wsvcOutPath = g_fso.GetParentFolderName(g_myPath) + "\wsvcs\phoenixxm\bin\"
	
	copyToWsvcsDir =  g_fso.FolderExists( wsvcOutPath ) 
	'
	Wscript.Echo ""
	Wscript.Echo "------------- Post Build START:" +  Wscript.ScriptFullName + " -----------"
	

	sCurrentFile = sProjOutName + sProjOutExtension
	if (sConfigName = "debug" ) then

		g_sdkPath = g_wsShell.RegRead( "HKLM\SOFTWARE\Microsoft\.NETFramework\sdkInstallRootv1.1" )
		vbQuote = """"
		
		'execString = vbQuote + g_sdkPath + "bin\sn.exe "+ vbQuote +  " –Vr " + vbQuote  + sProjOutDir + sProjOutName + sProjOutExtension + vbQuote +  + "> C:\\Text.Out"
		execString = vbQuote + g_sdkPath + "bin\sn.exe " + vbQuote + " -Vr *,5782ecf393b4839c"
		Set oExec = g_wsShell.Exec( execString)
		Do While oExec.Status = 0
			Wscript.Sleep 100
		Loop

		WScript.Echo "Un Verify Assembly Status " & oExec.Status
		'g_wsShell.Run	execString, 10, true
		'g_wsShell.Run "C:\\Shark\\Latest\\Common\\UVerify.bat " + vbQuote + sProjOutDir + sProjOutName + sProjOutExtension + vbQuote, 1, true

		DeployFile sProjOutDir + sProjOutName + ".pdb", myDestPath + sProjOutName + ".pdb"
		
		if( copyToWsvcsDir ) then
			DeployFile sProjOutDir + sProjOutName + ".pdb", wsvcOutPath + sProjOutName + ".pdb"
			DeployFile sProjOutDir + sCurrentFile, wsvcOutPath + sCurrentFile
		end if
	end if

	
	'
	DeployFile sProjOutDir + sCurrentFile, myDestPath + sCurrentFile
	
	'
	sCurrentFile = sProjOutName + ".xml"
	If g_fso.FileExists( sProjOutDir + sCurrentFile ) then
		DeployFile sProjOutDir + sCurrentFile, myDestPath + sCurrentFile
	end if
	Wscript.Echo "------------- Post Build END:  " +  Wscript.ScriptFullName + " m----------"

	WScript.Quit (0)
end sub




' Ensures the read-only flag is not set on the specified file
Sub RemoveReadOnlyFlag( ByVal path )

  Dim file

  If g_fso.FileExists( path ) Then
    Set file = g_fso.GetFile( path )
    If( file.Attributes And 1 ) Then
      file.Attributes = file.Attributes - 1
    End if
  End if

End Sub


Sub DeployFile( ByVal sourcePath, ByVal destinationPath )

  RemoveReadOnlyFlag destinationPath

  If Not IsEmpty(g_logFile) Then
    g_logFile.WriteLine "Copying file '" & sourcePath & "' to '" & destinationPath & "'"
  Else
    Wscript.Echo( "Copying file '" & sourcePath & "' to '" & destinationPath & "'" )
  End If
  g_fso.CopyFile sourcePath, destinationPath, true
 
End Sub



'''''''''''''COMMENTED CODE''''''''''
'g_winPath = g_wsShell.ExpandEnvironmentStrings( "%windir%" )
'g_netPath = g_fso.BuildPath( g_winPath, "Microsoft.NET\Framework\v1.1.4322" )

'Wscript.Echo execString 
'Wscript.Echo "UVerify.bat " + vbQuote + sProjOutDir + sProjOutName + sProjOutExtension + vbQuote
'g_wsShell.Exec execString
