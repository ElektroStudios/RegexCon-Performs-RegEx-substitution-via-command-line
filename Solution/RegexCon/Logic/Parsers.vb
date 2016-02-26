' ***********************************************************************
' Author   : Elektro
' Modified : 26-February-2016
' ***********************************************************************

#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports RegexCon.Types
Imports RegexCon.UserInterface

#End Region

#Region " Parsers "

Namespace Logic

    Friend Module Parsers

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Loop through all the command-line arguments of this application.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="cmds">
        ''' The commandline parameters.
        ''' </param>
        ''' 
        ''' <param name="callbackSyntaxError">
        ''' An encapsulated method that is invoked if a syntax error is found in one of the arguments.
        ''' </param>
        ''' 
        ''' <param name="callbackMissingRequired">
        ''' An encapsulated method that is invoked if a required parameter is missing in the arguments.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        Friend Sub ParseArguments(ByVal cmds As CommandlineParameterCollection,
                                  ByVal callbackSyntaxError As Action(Of CommandlineParameter),
                                  ByVal callbackMissingRequired As Action(Of CommandlineParameter))

            Parsers.ParseArguments(cmds, Environment.GetCommandLineArgs.Skip(1), callbackSyntaxError, callbackMissingRequired)
            Fields.SetFields()

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Loop through all the command-line arguments of this application.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="cmds">
        ''' The commandline parameters.
        ''' </param>
        ''' 
        ''' <param name="args">
        ''' The collection of commandline arguments to examine.
        ''' </param>
        ''' 
        ''' <param name="callbackSyntaxError">
        ''' An encapsulated method that is invoked if a syntax error is found in one of the arguments.
        ''' </param>
        ''' 
        ''' <param name="callbackMissingRequired">
        ''' An encapsulated method that is invoked if a required parameter is missing in the arguments.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        Private Sub ParseArguments(ByVal cmds As CommandlineParameterCollection,
                                   ByVal args As IEnumerable(Of String),
                                   ByVal callbackSyntaxError As Action(Of CommandlineParameter),
                                   ByVal callbackMissingRequired As Action(Of CommandlineParameter))

            If Not (args.Any) Then
                HelpSection.PrintHelp()
            End If

            Dim cmdRequired As List(Of CommandlineParameter) =
                (From cmd As CommandlineParameter In cmds
                 Where Not cmd.IsOptional).ToList

            For Each arg As String In args

                For Each cmd As CommandlineParameter In cmds

                    If (arg.StartsWith(cmd.Name & cmd.Separator, StringComparison.OrdinalIgnoreCase)) OrElse
                       (arg.StartsWith(cmd.ShortName & cmd.Separator, StringComparison.OrdinalIgnoreCase)) Then

                        Dim value As String = arg.Substring(arg.IndexOf(cmd.Separator) + 1)

                        If (cmdRequired.Contains(cmd)) Then
                            cmdRequired.Remove(cmd)
                        End If

                        If String.IsNullOrEmpty(value) Then
                            cmd.Value = cmd.DefaultValue
                            Continue For

                        Else
                            Try
                                cmd.Value = Convert.ChangeType(value, cmd.DefaultValue.GetType())
                                Continue For

                            Catch ex As Exception
                                callbackSyntaxError.Invoke(cmd)
                                Exit Sub

                            End Try

                        End If

                    ElseIf arg.Equals("/?") Then
                        HelpSection.PrintHelp()

                    End If

                Next cmd

            Next arg

            If (cmdRequired.Any) Then
                callbackMissingRequired.Invoke(cmdRequired.First)
            End If

        End Sub

    End Module

End Namespace

#End Region
