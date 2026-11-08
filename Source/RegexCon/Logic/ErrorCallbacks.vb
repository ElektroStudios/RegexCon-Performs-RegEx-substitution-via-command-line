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

Imports RegexCon.Types

#End Region

#Region " Error Callbacks "

Namespace Logic

    ''' <summary>
    ''' Provides the error handlers of this application.
    ''' <para></para>
    ''' Every handler writes the error message to the standard error stream (so it does not get mixed
    ''' with the substitution result written to the standard output),
    ''' then terminates the process with exit code <see cref="ErrorCallbacks.ErrorExitCode"/>.
    ''' </summary>
    Friend Module ErrorCallbacks

#Region " Constants "

        ''' <summary>
        ''' The exit code used when the application terminates because of an error.
        ''' </summary>
        Friend Const ErrorExitCode As Integer = 1

#End Region

#Region " Public Methods "

        ''' <summary>
        ''' Handles a syntax error in a parameter.
        ''' </summary>
        ''' <param name="cmd">
        ''' The parameter.
        ''' </param>
        Friend Sub OnSyntaxError(ByVal cmd As CommandlineParameter)

            ErrorCallbacks.Fail($"[X] Syntax error in parameter: {ErrorCallbacks.GetDisplayName(cmd)}")

        End Sub

        ''' <summary>
        ''' Handles a required parameter that is missing in the arguments.
        ''' </summary>
        ''' <param name="cmd">
        ''' The parameter.
        ''' </param>
        Friend Sub OnMissingParameterRequired(ByVal cmd As CommandlineParameter)

            ErrorCallbacks.Fail($"[X] Parameter {ErrorCallbacks.GetDisplayName(cmd)} is required.")

        End Sub

        ''' <summary>
        ''' Handles a parameter that is specified more than once in the arguments.
        ''' </summary>
        ''' <param name="cmd">
        ''' The parameter.
        ''' </param>
        Friend Sub OnDuplicatedParameter(ByVal cmd As CommandlineParameter)

            ErrorCallbacks.Fail($"[X] Parameter {ErrorCallbacks.GetDisplayName(cmd)} is specified more than once.")

        End Sub

        ''' <summary>
        ''' Handles an argument that does not match any parameter.
        ''' </summary>
        ''' <param name="arg">
        ''' The unknown argument.
        ''' </param>
        Friend Sub OnUnknownArgument(ByVal arg As String)

            ErrorCallbacks.Fail($"[X] Unknown argument: ""{arg}"". Use {Parsers.HelpSwitch} to show the help.")

        End Sub

        ''' <summary>
        ''' Handles an empty regular expression.
        ''' </summary>
        Friend Sub OnEmptyExpression()

            ErrorCallbacks.Fail("[X] The regular expression cannot be empty.")

        End Sub

        ''' <summary>
        ''' Handles an invalid regular expression.
        ''' </summary>
        ''' <param name="expr">
        ''' The regular expression.
        ''' </param>
        Friend Sub OnRegExValidationFailed(ByVal expr As String)

            ErrorCallbacks.Fail($"[X] Expression validation has failed: ""{expr}""")

        End Sub

#End Region

#Region " Private Methods "

        ''' <summary>
        ''' Gets the display name of the specified parameter, including its short name if it has one.
        ''' </summary>
        ''' <param name="cmd">
        ''' The parameter.
        ''' </param>
        ''' <returns>
        ''' The display name (e.g. <c>/Input (or /I)</c>).
        ''' </returns>
        ''' <exception cref="ArgumentNullException">
        ''' <paramref name="cmd"/> is <see langword="Nothing"/>.
        ''' </exception>
        Private Function GetDisplayName(ByVal cmd As CommandlineParameter) As String

            If cmd Is Nothing Then
                Throw New ArgumentNullException(paramName:=NameOf(cmd))
            End If

            Return If(String.IsNullOrEmpty(cmd.ShortName), cmd.Name, $"{cmd.Name} (or {cmd.ShortName})")

        End Function

        ''' <summary>
        ''' Writes the specified error message to the standard error stream,
        ''' then terminates the process with exit code <see cref="ErrorCallbacks.ErrorExitCode"/>.
        ''' </summary>
        ''' <param name="message">
        ''' The error message.
        ''' </param>
        Private Sub Fail(ByVal message As String)

            Console.Error.WriteLine(message)
            Environment.Exit(exitCode:=ErrorCallbacks.ErrorExitCode)

        End Sub

#End Region

    End Module

End Namespace

#End Region
