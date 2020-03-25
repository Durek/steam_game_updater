Module Module1
    Sub Main()
        ' initialize steamcmd wrapper
        steamcmd.setup(My.Settings.username, My.Settings.password, My.Settings.steamappsPath)

        ' check for update
        If steamcmd.checkForUpdate(My.Settings.appId) Then
            Console.WriteLine("Update available")

            ' wait for user confirmation to update while in debugmode
            If My.Settings.debugMode Then
                Console.Write("   < Press Enter to update >")
                Console.ReadLine()
            End If

            ' run update
            If steamcmd.update(My.Settings.appId, My.Settings.validate) Then
                Console.WriteLine("Update successful")
            Else
                Console.WriteLine("Update failed")
            End If
        Else
            Console.WriteLine("Already up to date.")
        End If

        ' wait for user confirmation to close while in debugmode
        If My.Settings.debugMode Then
            Console.Write("   < Press Enter to close >")
            Console.ReadLine()
        End If
    End Sub

End Module
