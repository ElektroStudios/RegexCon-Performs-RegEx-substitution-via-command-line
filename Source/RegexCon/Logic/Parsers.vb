' ***********************************************************************
' Author   : Elektro
' Modified : 07-November-2026
' ***********************************************************************

#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Globalization

Imports RegexCon.Types
Imports RegexCon.UserInterface

#End Region

#Region " Parsers "

Namespace Logic

    ''' <summary>
    ''' Provides the command-line argument parser of this application.
    ''' </summary>
    Friend Module Parsers

#Region " Constants "

        ''' <summary>
        ''' The command-line switch that prints the help.
        ''' </summary>
        Friend Const HelpSwitch As String = "/?"

#End Region

#Region " Public Methods "

        ''' <summary>
        ''' Parses all the command-line arguments of this application,
        ''' then assigns the parsed values to the <see cref="Fields"/> module.
        ''' </summary>
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
        '''
        ''' <param name="callbackDuplicated">
        ''' An encapsulated method that is invoked if a parameter is specified more than once in the arguments.
        ''' </param>
        '''
        ''' <param name="callbackUnknownArgument">
        ''' An encapsulated method that is invoked if an argument does not match any parameter.
        ''' </param>
        Friend Sub ParseArguments(ByVal cmds As CommandlineParameterCollection,
                                  ByVal callbackSyntaxError As Action(Of CommandlineParameter),
                                  ByVal callbackMissingRequired As Action(Of CommandlineParameter),
                                  ByVal callbackDuplicated As Action(Of CommandlineParameter),
                                  ByVal callbackUnknownArgument As Action(Of String))

            Parsers.ParseArguments(cmds, Environment.GetCommandLineArgs.Skip(1),
                                   callbackSyntaxError, callbackMissingRequired, callbackDuplicated, callbackUnknownArgument)
            Fields.SetFields()

        End Sub

        ''' <summary>
        ''' Parses the specified command-line arguments.
        ''' </summary>
        ''' <param name="cmds">
        ''' The commandline parameters.
        ''' </param>
        '''
        ''' <param name="args">
        ''' The collection of commandline arguments to examine.
        ''' </param>
        '''
        ''' <param name="callbackSyntaxError">
        ''' An encapsulated method that is invoked if a syntax error is found in one of the arguments:
        ''' a value that cannot be converted to the parameter type,
        ''' or a parameter specified without the separator character (e.g. <c>/I</c> instead of <c>/I=value</c>).
        ''' </param>
        '''
        ''' <param name="callbackMissingRequired">
        ''' An encapsulated method that is invoked if a required parameter is missing in the arguments.
        ''' </param>
        '''
        ''' <param name="callbackDuplicated">
        ''' An encapsulated method that is invoked if a parameter is specified more than once in the arguments,
        ''' either by its name or by its short name.
        ''' </param>
        '''
        ''' <param name="callbackUnknownArgument">
        ''' An encapsulated method that is invoked if an argument does not match any parameter.
        ''' </param>
        ''' <remarks>
        ''' If there are no arguments, or any argument is the help switch (<c>/?</c>),
        ''' the help is printed before parsing any other argument.
        ''' <para></para>
        ''' Every parameter is first initialized to its <see cref="CommandlineParameter(Of T).DefaultValue"/>,
        ''' so the optional parameters that are omitted in the arguments take their default value.
        ''' <para></para>
        ''' The parsing stops at the first error, after invoking the corresponding callback.
        ''' </remarks>
        ''' <exception cref="ArgumentNullException">
        ''' <paramref name="cmds"/>, <paramref name="args"/> or any of the callbacks is <see langword="Nothing"/>.
        ''' </exception>
        '''
        ''' <exception cref="ArgumentException">
        ''' <paramref name="args"/> contains a <see langword="Nothing"/> argument.
        ''' </exception>
        '''
        ''' <exception cref="InvalidOperationException">
        ''' A parameter has no default value, so its value type cannot be determined.
        ''' </exception>
        Friend Sub ParseArguments(ByVal cmds As CommandlineParameterCollection,
                                  ByVal args As IEnumerable(Of String),
                                  ByVal callbackSyntaxError As Action(Of CommandlineParameter),
                                  ByVal callbackMissingRequired As Action(Of CommandlineParameter),
                                  ByVal callbackDuplicated As Action(Of CommandlineParameter),
                                  ByVal callbackUnknownArgument As Action(Of String))

            If cmds Is Nothing Then
                Throw New ArgumentNullException(paramName:=NameOf(cmds))
            End If

            If args Is Nothing Then
                Throw New ArgumentNullException(paramName:=NameOf(args))
            End If

            If callbackSyntaxError Is Nothing Then
                Throw New ArgumentNullException(paramName:=NameOf(callbackSyntaxError))
            End If

            If callbackMissingRequired Is Nothing Then
                Throw New ArgumentNullException(paramName:=NameOf(callbackMissingRequired))
            End If

            If callbackDuplicated Is Nothing Then
                Throw New ArgumentNullException(paramName:=NameOf(callbackDuplicated))
            End If

            If callbackUnknownArgument Is Nothing Then
                Throw New ArgumentNullException(paramName:=NameOf(callbackUnknownArgument))
            End If

            Dim argList As List(Of String) = args.ToList()
            If argList.Contains(Nothing) Then
                Throw New ArgumentException(message:="The argument collection cannot contain null arguments.",
                                            paramName:=NameOf(args))
            End If

            ' The help has priority over any other argument.
            If (argList.Count = 0) OrElse (argList.Contains(Parsers.HelpSwitch)) Then
                HelpSection.PrintHelp()
                Exit Sub
            End If

            ' Assign the default value to every parameter before parsing,
            ' so the parameters that are omitted in the arguments take their default value.
            For Each cmd As CommandlineParameter In cmds
                If cmd.DefaultValue Is Nothing Then
                    Throw New InvalidOperationException(
                        $"The parameter {cmd.Name} has no default value, so its value type cannot be determined.")
                End If
                cmd.Value = cmd.DefaultValue
            Next cmd

            Dim parsedCmds As New List(Of CommandlineParameter)

            For Each arg As String In argList

                Dim cmd As CommandlineParameter = Parsers.FindParameterForArgument(cmds, arg)

                If cmd Is Nothing Then
                    ' A known parameter specified without separator and value (e.g. "/I") is a syntax error.
                    Dim bareCmd As CommandlineParameter = cmds.Find(arg)
                    If bareCmd IsNot Nothing Then
                        callbackSyntaxError.Invoke(bareCmd)
                    Else
                        callbackUnknownArgument.Invoke(arg)
                    End If
                    Exit Sub
                End If

                If parsedCmds.Contains(cmd) Then
                    callbackDuplicated.Invoke(cmd)
                    Exit Sub
                End If
                parsedCmds.Add(cmd)

                Dim value As String = arg.Substring(arg.IndexOf(cmd.Separator) + 1)
                If String.IsNullOrEmpty(value) Then
                    cmd.Value = cmd.DefaultValue
                    Continue For
                End If

                Try
                    cmd.Value = Convert.ChangeType(value, cmd.DefaultValue.GetType(), CultureInfo.InvariantCulture)

                Catch ex As Exception When (TypeOf ex Is FormatException) OrElse
                                           (TypeOf ex Is InvalidCastException) OrElse
                                           (TypeOf ex Is OverflowException)
                    callbackSyntaxError.Invoke(cmd)
                    Exit Sub

                End Try

            Next arg

            Dim missingCmd As CommandlineParameter =
                (From cmd As CommandlineParameter In cmds
                 Where (Not cmd.IsOptional) AndAlso (Not parsedCmds.Contains(cmd))).FirstOrDefault()

            If missingCmd IsNot Nothing Then
                callbackMissingRequired.Invoke(missingCmd)
            End If

        End Sub

#End Region

#Region " Private Methods "

        ''' <summary>
        ''' Finds the parameter that matches the specified argument,
        ''' this is, an argument that starts with the parameter name or short name followed by the separator character
        ''' (case-insensitive).
        ''' </summary>
        ''' <param name="cmds">
        ''' The commandline parameters.
        ''' </param>
        '''
        ''' <param name="arg">
        ''' The argument.
        ''' </param>
        ''' <returns>
        ''' The matching <see cref="CommandlineParameter"/>,
        ''' or <see langword="Nothing"/> if the argument does not match any parameter.
        ''' </returns>
        Private Function FindParameterForArgument(ByVal cmds As CommandlineParameterCollection,
                                                  ByVal arg As String) As CommandlineParameter

            For Each cmd As CommandlineParameter In cmds

                Dim matchesName As Boolean =
                    arg.StartsWith($"{cmd.Name}{cmd.Separator}", StringComparison.OrdinalIgnoreCase)

                Dim matchesShortName As Boolean =
                    Not String.IsNullOrEmpty(cmd.ShortName) AndAlso
                    arg.StartsWith($"{cmd.ShortName}{cmd.Separator}", StringComparison.OrdinalIgnoreCase)

                If matchesName OrElse matchesShortName Then
                    Return cmd
                End If

            Next cmd

            Return Nothing

        End Function

#End Region

    End Module

End Namespace

#End Region
