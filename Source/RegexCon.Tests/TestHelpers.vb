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

Imports System.IO
Imports System.Text

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports RegexCon.Logic
Imports RegexCon.Tools
Imports RegexCon.Types

#End Region

' Tests share global state (Console.Out redirection, Logic.Fields module and child processes),
' so they must not run in parallel.
<Assembly: DoNotParallelize>

#Region " Test Helpers "

''' ----------------------------------------------------------------------------------------------------
''' <summary>
''' Provides shared helper methods for the unit tests.
''' </summary>
''' ----------------------------------------------------------------------------------------------------
Friend Module TestHelpers

#Region " Parameter Helpers "

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Creates a new <see cref="CommandlineParameterCollection"/> that contains the same parameters
    ''' registered by the application entry point (<c>UserInterface.Main</c>).
    ''' <para></para>
    ''' Every call returns new <see cref="CommandlineParameter"/> instances,
    ''' so the tests do not share parameter state between them.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <returns>
    ''' The resulting <see cref="CommandlineParameterCollection"/>.
    ''' </returns>
    ''' ----------------------------------------------------------------------------------------------------
    Friend Function CreateApplicationParameters() As CommandlineParameterCollection

        Dim params As New CommandlineParameterCollection()
        params.Add(Parameters.ParamInput)
        params.Add(Parameters.ParamExpression)
        params.Add(Parameters.ParamSubstitution)
        params.Add(Parameters.ParamMatchCase)
        Return params

    End Function

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Creates a new <see cref="CommandlineParameter"/> with the specified values.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <param name="name">
    ''' The parameter name.
    ''' </param>
    '''
    ''' <param name="shortName">
    ''' The parameter short name. It can be <see langword="Nothing"/>.
    ''' </param>
    '''
    ''' <param name="defaultValue">
    ''' The parameter default value.
    ''' </param>
    '''
    ''' <param name="isOptional">
    ''' A value indicating whether the parameter is optional.
    ''' </param>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <returns>
    ''' The resulting <see cref="CommandlineParameter"/>.
    ''' </returns>
    ''' ----------------------------------------------------------------------------------------------------
    Friend Function CreateParameter(ByVal name As String,
                                    ByVal shortName As String,
                                    ByVal defaultValue As Object,
                                    ByVal isOptional As Boolean) As CommandlineParameter

        Dim param As New CommandlineParameter()
        param.Separator = "="c
        param.Name = name
        param.ShortName = shortName
        param.DefaultValue = defaultValue
        param.IsOptional = isOptional
        Return param

    End Function

#End Region

#Region " Console Helpers "

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Invokes the specified method while the standard output is redirected,
    ''' and returns everything that was written to the console.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <param name="action">
    ''' The method to invoke.
    ''' </param>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <returns>
    ''' The text written to the console.
    ''' </returns>
    ''' ----------------------------------------------------------------------------------------------------
    Friend Function CaptureConsoleOutput(ByVal action As Action) As String

        If action Is Nothing Then
            Throw New ArgumentNullException(paramName:=NameOf(action))
        End If

        Dim originalOut As TextWriter = Console.Out
        Using writer As New StringWriter()
            Console.SetOut(writer)
            Try
                action.Invoke()
            Finally
                Console.SetOut(originalOut)
            End Try
            Return writer.ToString()
        End Using

    End Function

#End Region

#Region " Process Helpers "

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Gets the full path of the RegexCon application assembly under test.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    Friend ReadOnly Property ApplicationPath As String
        Get
            Return GetType(RegExUtil).Assembly.Location
        End Get
    End Property

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Runs the RegexCon application with the specified command-line arguments,
    ''' and waits for it to exit.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <param name="args">
    ''' The command-line arguments. Each argument is quoted automatically.
    ''' </param>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <returns>
    ''' A <see cref="ProcessResult"/> with the exit code and the standard output of the process.
    ''' </returns>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <exception cref="TimeoutException">
    ''' The process did not exit in the expected time.
    ''' </exception>
    ''' ----------------------------------------------------------------------------------------------------
    Friend Function RunApplication(ParamArray args As String()) As ProcessResult

        Dim appPath As String = TestHelpers.ApplicationPath
        Dim quotedArgs As String = String.Join(" ", args.Select(Function(arg As String) TestHelpers.QuoteArgument(arg)))

        Dim startInfo As New ProcessStartInfo() With {
            .UseShellExecute = False,
            .CreateNoWindow = True,
            .RedirectStandardOutput = True,
            .RedirectStandardError = True
        }

        ' When the application is built as a .dll (modern .NET), it must be hosted by "dotnet".
        If appPath.EndsWith(".dll", StringComparison.OrdinalIgnoreCase) Then
            startInfo.FileName = "dotnet"
            startInfo.Arguments = $"{TestHelpers.QuoteArgument(appPath)} {quotedArgs}"
        Else
            startInfo.FileName = appPath
            startInfo.Arguments = quotedArgs
        End If

        Using proc As Process = Process.Start(startInfo)
            Dim standardErrorTask As Threading.Tasks.Task(Of String) = proc.StandardError.ReadToEndAsync()
            Dim standardOutput As String = proc.StandardOutput.ReadToEnd()

            If Not proc.WaitForExit(milliseconds:=30000) Then
                proc.Kill()
                Throw New TimeoutException($"The process did not exit in the expected time: {appPath} {quotedArgs}")
            End If

            Return New ProcessResult(proc.ExitCode, standardOutput, standardErrorTask.Result)
        End Using

    End Function

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Quotes the specified command-line argument following the Windows command-line parsing rules
    ''' (the same rules used by <c>CommandLineToArgvW</c> and the .NET runtime).
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <param name="arg">
    ''' The argument to quote.
    ''' </param>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <returns>
    ''' The quoted argument.
    ''' </returns>
    ''' ----------------------------------------------------------------------------------------------------
    Friend Function QuoteArgument(ByVal arg As String) As String

        If arg Is Nothing Then
            Throw New ArgumentNullException(paramName:=NameOf(arg))
        End If

        Dim sb As New StringBuilder()
        sb.Append(""""c)

        Dim backslashCount As Integer = 0
        For Each c As Char In arg
            If c = "\"c Then
                backslashCount += 1

            ElseIf c = """"c Then
                ' Backslashes that precede a quote must be doubled, and the quote itself escaped.
                sb.Append("\"c, (backslashCount * 2) + 1)
                sb.Append(c)
                backslashCount = 0

            Else
                sb.Append("\"c, backslashCount)
                sb.Append(c)
                backslashCount = 0

            End If
        Next c

        ' Backslashes that precede the closing quote must be doubled.
        sb.Append("\"c, backslashCount * 2)
        sb.Append(""""c)

        Return sb.ToString()

    End Function

#End Region

End Module

#End Region

#Region " Process Result "

''' ----------------------------------------------------------------------------------------------------
''' <summary>
''' Represents the result of running the RegexCon application in a child process.
''' </summary>
''' ----------------------------------------------------------------------------------------------------
Friend NotInheritable Class ProcessResult

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Gets the process exit code.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    Friend ReadOnly Property ExitCode As Integer

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Gets the text written by the process to the standard output.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    Friend ReadOnly Property StandardOutput As String

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Gets the text written by the process to the standard error.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    Friend ReadOnly Property StandardError As String

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Gets the standard output without the trailing line break written by <see cref="Console.WriteLine(String)"/>.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    Friend ReadOnly Property OutputLine As String
        Get
            Dim output As String = Me.StandardOutput
            If output.EndsWith(vbCrLf, StringComparison.Ordinal) Then
                Return output.Substring(0, output.Length - 2)
            ElseIf output.EndsWith(vbLf, StringComparison.Ordinal) Then
                Return output.Substring(0, output.Length - 1)
            Else
                Return output
            End If
        End Get
    End Property

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Initializes a new instance of the <see cref="ProcessResult"/> class.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <param name="exitCode">
    ''' The process exit code.
    ''' </param>
    '''
    ''' <param name="standardOutput">
    ''' The text written by the process to the standard output.
    ''' </param>
    '''
    ''' <param name="standardError">
    ''' The text written by the process to the standard error.
    ''' </param>
    ''' ----------------------------------------------------------------------------------------------------
    Friend Sub New(ByVal exitCode As Integer, ByVal standardOutput As String, ByVal standardError As String)

        If standardOutput Is Nothing Then
            Throw New ArgumentNullException(paramName:=NameOf(standardOutput))
        End If

        If standardError Is Nothing Then
            Throw New ArgumentNullException(paramName:=NameOf(standardError))
        End If

        Me.ExitCode = exitCode
        Me.StandardOutput = standardOutput
        Me.StandardError = standardError

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Returns a string that describes this process result, useful in assertion messages.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <returns>
    ''' A string that describes this process result.
    ''' </returns>
    ''' ----------------------------------------------------------------------------------------------------
    Public Overrides Function ToString() As String
        Return $"ExitCode={Me.ExitCode}; StdOut=""{Me.StandardOutput}""; StdErr=""{Me.StandardError}"""
    End Function

End Class

#End Region
