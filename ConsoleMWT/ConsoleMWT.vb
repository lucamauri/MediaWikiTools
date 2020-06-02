Imports System
Imports System.IO

Module ConsoleMWT
    Private Const PromptSymbol As String = "MWT > "
    Private Const ApplicationTitle As String = "MediaWiki Tools"

    Dim LastError As String
    'Dim Arguments As String()
    Sub Main(args As String())
        Console.WriteLine("MediaWiki Tools command-line interface")
        Console.WriteLine()

        If args.Length = 0 Then
            Console.WriteLine("No arguments, entering interactive prompt.")
            'Environment.Exit(1)
            Call RunInteractive()
        Else
            'Arguments = args

            'Select Case args(0).ToLower
            '    Case "sitesandiw", "siw"
            '        Console.WriteLine("Sites and InterWiki processor")
            '        If Not SitesAndIW() Then
            '            Console.WriteLine("ERROR:")
            '            Console.WriteLine(LastError)
            '            Environment.Exit(13)
            '        End If
            '    Case "ver", "version"
            '        Console.WriteLine("This package is version " & GetType(Open.ConsoleMWT.ConsoleMWT).Assembly.GetName().Version.ToString)
            '    Case Else
            '        Console.WriteLine("Argument >" & args(0) & "< not recognized, exiting without action.")
            '        Environment.Exit(1)
            'End Select
            'Environment.Exit(0)
            If ProcessArray(args) Then
                Environment.Exit(0)
            Else
                Console.WriteLine("ERROR:")
                Console.WriteLine(LastError)
                Environment.Exit(1)
            End If
        End If

    End Sub
    Sub RunInteractive()
        Dim ConsoleInput As String
        Dim InputArray As String()


        Console.ForegroundColor = ConsoleColor.Cyan
        Console.Title = ApplicationTitle & " command line console"

        Console.WriteLine("Welcome to " & ApplicationTitle & " console frontend")
        Console.WriteLine("This package is version " & GetType(ConsoleMWT).Assembly.GetName().Version.ToString)
        Console.WriteLine()
        Console.Write(PromptSymbol)

        Do While True
            ConsoleInput = Console.ReadLine()

            InputArray = ConsoleInput.Split(" ")

            If Not ProcessArray(InputArray) Then
                Console.WriteLine("ERROR:")
                Console.WriteLine(LastError)
                Console.WriteLine()
            End If
            Console.Write(PromptSymbol)
        Loop

    End Sub
    Function ProcessArray(CommandAndArgs As String()) As Boolean
        If CommandAndArgs.GetUpperBound(0) < 2 Then
            Return ProcessCommand(CommandAndArgs(0), {})
        Else
            Dim ArgsArray(CommandAndArgs.GetUpperBound(0) - 1) As String

            Array.Copy(CommandAndArgs, 1, ArgsArray, 0, CommandAndArgs.GetLength(0) - 1)

            Return ProcessCommand(CommandAndArgs(0), ArgsArray)
        End If

    End Function

    Function ProcessCommand(Command As String, Arguments As String()) As Boolean
        Select Case Command.ToLower
            Case "sitesandiw", "siw"
                'ConsoleMWT siw H:\pCloud\Docs\WikiTrek\DO2020\masterSitesTest.csv true |
                Console.WriteLine("Sites and InterWiki processor")
                If SitesAndIW(Arguments) Then
                    Return True
                Else
                    Console.WriteLine("ERROR:")
                    Console.WriteLine(LastError)
                    Return False
                End If
            Case "ver", "version"
                Console.WriteLine("This package is version " & GetType(Open.ConsoleMWT.ConsoleMWT).Assembly.GetName().Version.ToString)
                Return True
            Case "quit", "exit", "q"
                Environment.Exit(0)
                Return True
            Case Else
                LastError = "Argument >" & Command & "< not recognized, exiting without action."
                Return False
        End Select
    End Function
    Function SitesAndIW(Arguments As String()) As Boolean
        Dim Tabler As CSVTabler.CSVTabler
        Dim Tooler As MWTCore.SitesAndIW

        If Arguments.Count < 3 Then
            LastError = "Not enough arguments: expecting (<input>, <hasheader> and <separator>), got " & Arguments.Count.ToString
            Return False
        End If

        Try
            Console.WriteLine("Arguments(2)>" & Arguments(2) & "<")
            Tabler = New CSVTabler.CSVTabler(CType(Arguments(1), Boolean), Arguments(2))
            Tabler.LoadFile(Arguments(0))

            If Tabler.ProcessCSV() Then

                If Tabler.TableToText(15) Then
                    Console.WriteLine("TableToText")
                    Console.WriteLine("===========")
                    Console.WriteLine(Tabler.ValuesTableText())

                    Tooler = New MWTCore.SitesAndIW(Tabler.ValuesTable)
                    If Tooler.BuildTexts Then
                        Dim SourceDirInfo As System.IO.DirectoryInfo

                        SourceDirInfo = Tabler.SourceFile.Directory

                        Console.WriteLine("LUACode")
                        Console.WriteLine("=======")
                        'Console.WriteLine(Tooler.LUACode)
                        Call SaveFile(SourceDirInfo, "LUAArray.lua", Tooler.LUACode)

                        Console.WriteLine("Queries")
                        Console.WriteLine("=======")
                        'Console.WriteLine(Tooler.Queries)
                        Call SaveFile(SourceDirInfo, "IWQuery.sql", Tooler.Queries)

                        Console.WriteLine("Sites XML")
                        Console.WriteLine("=========")
                        'Console.WriteLine(Tooler.SitesXML)
                        Call SaveFile(SourceDirInfo, "sites.xml", Tooler.SitesXML)

                        Return True
                    Else
                        LastError = Tooler.LastError
                        Return False
                    End If
                Else
                    LastError = Tabler.ErrorMessage
                    Return False
                End If
            Else
                LastError = Tabler.ErrorMessage
                Return False
            End If
        Catch ex As Exception
            LastError = ex.ToString
            Return False
        End Try
    End Function
    Function SaveFile(Dir As DirectoryInfo, FileName As String, Content As String) As Boolean
        Dim Writer As System.IO.StreamWriter

        Try
            If File.Exists(Path.Combine(Dir.FullName, FileName)) Then
                Console.WriteLine(FileName & " already exist")
                Return False
            End If
            Writer = New System.IO.StreamWriter(Path.Combine(Dir.FullName, FileName))
            Writer.Write(Content)
            Console.WriteLine(FileName & ": file saved")
            Return True
        Catch ex As Exception
            Console.WriteLine("Unable to save " & FileName & " file")
            Return False
        Finally
            Writer.Close()
        End Try
    End Function
End Module
