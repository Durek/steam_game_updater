Public Class steamcmd
    Structure settings_s
        Dim username As String
        Dim password As String
        Dim steamcmdPath As String
        Dim steamappsPath As String
    End Structure

    Private Shared settings As New settings_s

    Private Shared Function runShell(cmd As String, path As String, args As String, Optional forceClose As Boolean = True) As String
        ' run commands using cmd
        Dim p As New Process
        p.StartInfo.FileName = "cmd.exe"
        p.StartInfo.Arguments = "/k " + cmd + " " + args + " & exit"
        p.StartInfo.WorkingDirectory = path
        p.StartInfo.CreateNoWindow = True
        p.StartInfo.UseShellExecute = False
        p.StartInfo.RedirectStandardOutput = True
        p.Start()

        ' read stdout
        Dim output As String = p.StandardOutput.ReadToEnd
        p.WaitForExit()

        If forceClose Then
            ' making sure the process is closed and not running in background
            Try
                Process.GetProcessById(p.Id).Kill()
            Catch
            End Try
        End If

        Return output
    End Function

    Public Shared Function run(Optional args As String = "")
        Dim _args As String = "+@ShutdownOnFailedCommand 1 +@NoPromptForPassword 1 +login " +
            settings.username + " " + settings.password +
            " +force_install_dir """ + settings.steamappsPath + """ " +
            args + " +quit"

        Return runShell("steamcmd.exe", settings.steamcmdPath, _args)
    End Function

    Public Shared Sub setup(username As String, password As String, steamappsPath As String, Optional forceSingleSteamcmd As Boolean = True)

        ' steamcmd likes to keep running, especially when updating
        ' if an instance of this tool crashed, there might be a steamcmd instance running thats locking our game files
        If forceSingleSteamcmd Then
            For Each p As Process In Process.GetProcessesByName("steamcmd")
                p.Kill()
            Next
        End If

        ' download, extract and run steamcmd
        ' steamcmd is downloaded from valve https://steamcdn-a.akamaihd.net/client/installer/steamcmd.zip
        ' the zip file contains only the steamcmd.exe
        ' steamcmd will update and load additional files on the first run
        If Not IO.File.Exists("steamcmd\steamcmd.exe") Then
            IO.Directory.CreateDirectory("steamcmd")
            Using wc As New Net.WebClient
                wc.DownloadFile("https://steamcdn-a.akamaihd.net/client/installer/steamcmd.zip", "steamcmd.zip")
            End Using
            IO.Compression.ZipFile.ExtractToDirectory("steamcmd.zip", "steamcmd")
            IO.File.Delete("steamcmd.zip")
            run()
        End If

        settings.username = username
        settings.password = password
        settings.steamcmdPath = Environment.CurrentDirectory + "\steamcmd"
        settings.steamappsPath = steamappsPath
    End Sub

    Public Shared Function checkForUpdate(appId As String) As Boolean
        Dim out As String
        ' app_status returns various information about the an installed game
        out = run("+app_status " + appId)

        For Each s As String In out.Split(vbNewLine)
            ' the "install state" entry can have multiple values.
            ' we are interested in "Update Required". others may be "Fully Installed", "Update Queued", "Update Running". there are probably more.
            ' these values are separated with commas plus an additional comma at the end
            ' to make it easy we're just checking wether a line contains "install state" and "Update Required"
            If s.Contains("install state") And s.Contains("Update Required") Then
                Return True
            End If
        Next

        Return False
    End Function

    Public Shared Function update(appId As String, Optional validate As Boolean = True) As Boolean
        Dim out As String

        ' you may want to skip validating game files if you're operating in a small time window or usingslow drives
        If validate Then
            out = run("+app_update " + appId + " validate")
        Else
            out = run("+app_update " + appId)
        End If

        ' the output will contain "Success!" if the update was successful. big surprise huh?
        If out.Contains("Success!") Then
            Return True
        End If
        Return False
    End Function

End Class
