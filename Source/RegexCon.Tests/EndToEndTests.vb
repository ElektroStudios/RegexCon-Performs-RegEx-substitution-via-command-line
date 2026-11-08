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

Imports Microsoft.VisualStudio.TestTools.UnitTesting

#End Region

#Region " End-To-End Tests "

''' <summary>
''' End-to-end tests that run the RegexCon application in a child process,
''' verifying its standard output and exit code.
''' <para></para>
''' These tests cover the code paths that terminate the process (help, errors and successful substitution).
''' </summary>
<TestClass>
Public NotInheritable Class EndToEndTests

#Region " Class Initialization "

    ''' <summary>
    ''' Verifies that the application under test exists before running the tests.
    ''' </summary>
    ''' <param name="context">
    ''' The test context.
    ''' </param>
    <ClassInitialize>
    Public Shared Sub ClassInitialize(ByVal context As TestContext)

        If Not File.Exists(TestHelpers.ApplicationPath) Then
            Throw New FileNotFoundException("The application under test was not found.", TestHelpers.ApplicationPath)
        End If

    End Sub

#End Region

#Region " Successful Substitution "

    ''' <summary>
    ''' Verifies the "Regular string substitution" usage example of the help.
    ''' </summary>
    <TestMethod>
    Public Sub Run_RegularSubstitutionExample_PrintsResult()

        Dim result As ProcessResult = TestHelpers.RunApplication("/I=Hello World", "/E=^(.+)\s(.+)$", "/S=$2 $1", "/M=True")

        Assert.AreEqual(0, result.ExitCode, result.ToString())
        Assert.AreEqual("World Hello", result.OutputLine, result.ToString())
        Assert.AreEqual(String.Empty, result.StandardError, result.ToString())

    End Sub

    ''' <summary>
    ''' Verifies the "Named-group string substitution" usage example of the help.
    ''' </summary>
    <TestMethod>
    Public Sub Run_NamedGroupSubstitutionExample_PrintsResult()

        Dim result As ProcessResult = TestHelpers.RunApplication("/I=Hello World", "/E=^(?<one>.+)\s(?<two>.+)$", "/S=${two} ${one}")

        Assert.AreEqual(0, result.ExitCode, result.ToString())
        Assert.AreEqual("World Hello", result.OutputLine, result.ToString())

    End Sub

    ''' <summary>
    ''' Verifies that the long parameter names produce the same result as the short names.
    ''' </summary>
    <TestMethod>
    Public Sub Run_LongParameterNames_PrintsResult()

        Dim result As ProcessResult = TestHelpers.RunApplication("/Input=2026-11-07", "/Expression=(\d+)-(\d+)-(\d+)", "/Substitution=$3/$2/$1", "/MatchCase=False")

        Assert.AreEqual(0, result.ExitCode, result.ToString())
        Assert.AreEqual("07/11/2026", result.OutputLine, result.ToString())

    End Sub

    ''' <summary>
    ''' Verifies that the input containing quotes and backslashes is received and printed correctly.
    ''' </summary>
    <TestMethod>
    Public Sub Run_InputWithQuotesAndBackslashes_PrintsResult()

        Dim result As ProcessResult = TestHelpers.RunApplication("/I=C:\Folder\""File"".txt", "/E=\\", "/S=/")

        Assert.AreEqual(0, result.ExitCode, result.ToString())
        Assert.AreEqual("C:/Folder/""File"".txt", result.OutputLine, result.ToString())

    End Sub

    ''' <summary>
    ''' Verifies that the substitution is case-sensitive when <c>/MatchCase</c> is omitted.
    ''' <para></para>
    ''' Regression test: before v1.2 the substitution was case-insensitive by default,
    ''' contradicting the documented default value of <c>/MatchCase</c> (<see langword="True"/>).
    ''' </summary>
    <TestMethod>
    Public Sub Run_MatchCaseOmitted_IsCaseSensitive()

        Dim result As ProcessResult = TestHelpers.RunApplication("/I=Hello hello", "/E=hello", "/S=X")

        Assert.AreEqual(0, result.ExitCode, result.ToString())
        Assert.AreEqual("Hello X", result.OutputLine, result.ToString())

    End Sub

    ''' <summary>
    ''' Verifies that the substitution is case-insensitive when <c>/MatchCase=False</c> is specified.
    ''' </summary>
    <TestMethod>
    Public Sub Run_MatchCaseFalse_IsCaseInsensitive()

        Dim result As ProcessResult = TestHelpers.RunApplication("/I=Hello hello", "/E=hello", "/S=X", "/M=False")

        Assert.AreEqual(0, result.ExitCode, result.ToString())
        Assert.AreEqual("X X", result.OutputLine, result.ToString())

    End Sub

    ''' <summary>
    ''' Verifies that an empty substitution string removes the matches.
    ''' </summary>
    <TestMethod>
    Public Sub Run_EmptySubstitution_RemovesMatches()

        Dim result As ProcessResult = TestHelpers.RunApplication("/I=a1b2c3", "/E=\d", "/S=")

        Assert.AreEqual(0, result.ExitCode, result.ToString())
        Assert.AreEqual("abc", result.OutputLine, result.ToString())

    End Sub

#End Region

#Region " Errors "

    ''' <summary>
    ''' Verifies that an invalid regular expression is reported with exit code 1.
    ''' </summary>
    <TestMethod>
    Public Sub Run_InvalidExpression_ReportsErrorWithExitCode1()

        Dim result As ProcessResult = TestHelpers.RunApplication("/I=abc", "/E=(abc", "/S=X")

        EndToEndTests.AssertIsError(result, "[X] Expression validation has failed: ""(abc""")

    End Sub

    ''' <summary>
    ''' Verifies that a missing required parameter is reported with exit code 1.
    ''' </summary>
    <TestMethod>
    Public Sub Run_MissingRequiredParameter_ReportsErrorWithExitCode1()

        Dim result As ProcessResult = TestHelpers.RunApplication("/E=a", "/S=b")

        EndToEndTests.AssertIsError(result, "[X] Parameter /Input (or /I) is required.")

    End Sub

    ''' <summary>
    ''' Verifies that an invalid parameter value is reported as a syntax error with exit code 1.
    ''' </summary>
    <TestMethod>
    Public Sub Run_InvalidMatchCaseValue_ReportsSyntaxErrorWithExitCode1()

        Dim result As ProcessResult = TestHelpers.RunApplication("/I=abc", "/E=a", "/S=b", "/M=Maybe")

        EndToEndTests.AssertIsError(result, "[X] Syntax error in parameter: /MatchCase (or /M)")

    End Sub

    ''' <summary>
    ''' Verifies that an unknown argument is reported with exit code 1, and no substitution is performed.
    ''' <para></para>
    ''' Regression test: before v1.2 the unknown arguments were silently ignored.
    ''' </summary>
    <TestMethod>
    Public Sub Run_UnknownArgument_ReportsErrorWithExitCode1()

        Dim result As ProcessResult = TestHelpers.RunApplication("/I=abc", "/E=b", "/S=X", "/Verbose=True")

        EndToEndTests.AssertIsError(result, "[X] Unknown argument: ""/Verbose=True"". Use /? to show the help.")

    End Sub

    ''' <summary>
    ''' Verifies that a parameter specified more than once is reported with exit code 1.
    ''' <para></para>
    ''' Regression test: before v1.2 the last occurrence silently overwrote the previous ones.
    ''' </summary>
    <TestMethod>
    Public Sub Run_DuplicatedParameter_ReportsErrorWithExitCode1()

        Dim result As ProcessResult = TestHelpers.RunApplication("/I=abc", "/E=b", "/S=X", "/Substitution=Y")

        EndToEndTests.AssertIsError(result, "[X] Parameter /Substitution (or /S) is specified more than once.")

    End Sub

    ''' <summary>
    ''' Verifies that a parameter specified without separator and value is reported as a syntax error.
    ''' </summary>
    <TestMethod>
    Public Sub Run_ParameterWithoutSeparator_ReportsSyntaxErrorWithExitCode1()

        Dim result As ProcessResult = TestHelpers.RunApplication("/I=abc", "/E=b", "/S")

        EndToEndTests.AssertIsError(result, "[X] Syntax error in parameter: /Substitution (or /S)")

    End Sub

    ''' <summary>
    ''' Verifies that an empty regular expression is reported with exit code 1.
    ''' <para></para>
    ''' Regression test: before v1.2 an empty expression was accepted, 
    ''' inserting the substitution string between every character of the input.
    ''' </summary>
    <TestMethod>
    Public Sub Run_EmptyExpression_ReportsErrorWithExitCode1()

        Dim result As ProcessResult = TestHelpers.RunApplication("/I=abc", "/E=", "/S=X")

        EndToEndTests.AssertIsError(result, "[X] The regular expression cannot be empty.")

    End Sub

    ''' <summary>
    ''' Asserts that the process terminated with exit code 1, writing the specified error message 
    ''' to the standard error and nothing to the standard output.
    ''' </summary>
    ''' <param name="result">
    ''' The process result.
    ''' </param>
    ''' 
    ''' <param name="expectedError">
    ''' The expected error message.
    ''' </param>
    Private Shared Sub AssertIsError(ByVal result As ProcessResult, ByVal expectedError As String)

        Assert.AreEqual(1, result.ExitCode, result.ToString())
        Assert.AreEqual(expectedError, result.StandardError.TrimEnd(ControlChars.Cr, ControlChars.Lf), result.ToString())
        Assert.AreEqual(String.Empty, result.StandardOutput, "Errors must not be written to the standard output.")

    End Sub

#End Region

#Region " Help "

    ''' <summary>
    ''' Verifies that the help is printed with exit code 0 when no arguments are specified.
    ''' </summary>
    <TestMethod>
    Public Sub Run_NoArguments_PrintsHelpWithExitCode0()

        Dim result As ProcessResult = TestHelpers.RunApplication()

        Assert.AreEqual(0, result.ExitCode, result.ToString())
        EndToEndTests.AssertIsHelp(result)

    End Sub

    ''' <summary>
    ''' Verifies that the help is printed with exit code 0 when the <c>/?</c> switch is specified, 
    ''' even if other (invalid) arguments are specified before it.
    ''' </summary>
    <TestMethod>
    Public Sub Run_HelpSwitch_PrintsHelpWithExitCode0()

        Dim result As ProcessResult = TestHelpers.RunApplication("/I=abc", "/M=Maybe", "/Unknown", "/?")

        Assert.AreEqual(0, result.ExitCode, result.ToString())
        EndToEndTests.AssertIsHelp(result)

    End Sub

    ''' <summary>
    ''' Asserts that the process output is the help text, without color tokens.
    ''' </summary>
    ''' <param name="result">
    ''' The process result.
    ''' </param>
    Private Shared Sub AssertIsHelp(ByVal result As ProcessResult)

        Assert.Contains("Name......: RegExCon", result.StandardOutput, result.ToString())
        Assert.Contains("[+] Syntax", result.StandardOutput, result.ToString())
        Assert.Contains("[+] Switches", result.StandardOutput, result.ToString())
        Assert.Contains("[+] Usage examples", result.StandardOutput, result.ToString())
        Assert.Contains("[+] Exit codes", result.StandardOutput, result.ToString())
        Assert.DoesNotContain("*F7*", result.StandardOutput, result.ToString())
        Assert.AreEqual(String.Empty, result.StandardError, result.ToString())

    End Sub

#End Region

End Class

#End Region
