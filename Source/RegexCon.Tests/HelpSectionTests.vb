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

Imports System.Reflection

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports RegexCon.Tools
Imports RegexCon.Types
Imports RegexCon.UserInterface

#End Region

#Region " HelpSection Tests "

''' <summary>
''' Unit tests for <see cref="HelpSection"/> type.
''' <para></para>
''' These tests keep the help text consistent with the assembly metadata and the parameter definitions.
''' </summary>
<TestClass>
Public NotInheritable Class HelpSectionTests

#Region " Private Methods "

    ''' <summary>
    ''' Gets the value of the specified help element, with the color tokens removed.
    ''' </summary>
    ''' <param name="elementName">
    ''' The name of the help element.
    ''' </param>
    ''' <returns>
    ''' The plain text value of the help element.
    ''' </returns>
    Private Shared Function GetPlainHelpValue(ByVal elementName As String) As String

        Dim element As XElement = HelpSection.Help.Element(elementName)
        If element Is Nothing Then
            Throw New InvalidOperationException($"The help element was not found: {elementName}")
        End If

        Return TestHelpers.CaptureConsoleOutput(Sub() CliUtil.WriteColorText(element.Value, {"*"c}))

    End Function

#End Region

#Region " Tests "

    ''' <summary>
    ''' Verifies that all the elements used by the help printer exist and are not empty.
    ''' </summary>
    ''' <param name="elementName">
    ''' The name of the help element.
    ''' </param>
    <TestMethod>
    <DataRow("Title")>
    <DataRow("Name")>
    <DataRow("Author")>
    <DataRow("Version")>
    <DataRow("Copyright")>
    <DataRow("Website")>
    <DataRow("Logo")>
    <DataRow("Separator")>
    <DataRow("Syntax")>
    <DataRow("SyntaxExtra")>
    <DataRow("UsageExamples")>
    Public Sub Help_RequiredElement_ExistsAndIsNotEmpty(ByVal elementName As String)

        Dim element As XElement = HelpSection.Help.Element(elementName)

        Assert.IsNotNull(element, $"Missing help element: {elementName}")
        Assert.IsFalse(String.IsNullOrWhiteSpace(element.Value), $"Empty help element: {elementName}")

    End Sub

    ''' <summary>
    ''' Verifies that the version shown in the help matches the assembly version (major.minor).
    ''' </summary>
    <TestMethod>
    Public Sub Help_Version_MatchesAssemblyVersion()

        Dim version As Version = GetType(HelpSection).Assembly.GetName().Version
        Dim expected As String = $"{version.Major}.{version.Minor}"

        Assert.AreEqual(expected, HelpSectionTests.GetPlainHelpValue("Version"))

    End Sub

    ''' <summary>
    ''' Verifies that the copyright shown in the help matches the assembly copyright.
    ''' </summary>
    <TestMethod>
    Public Sub Help_Copyright_MatchesAssemblyCopyright()

        Dim attribute As AssemblyCopyrightAttribute =
            GetType(HelpSection).Assembly.GetCustomAttribute(Of AssemblyCopyrightAttribute)()

        Assert.IsNotNull(attribute)
        Assert.AreEqual(attribute.Copyright, HelpSectionTests.GetPlainHelpValue("Copyright"))

    End Sub

    ''' <summary>
    ''' Verifies that the switches section documents the name and short name of every parameter.
    ''' </summary>
    <TestMethod>
    Public Sub Help_SyntaxExtra_DocumentsAllParameters()

        Dim syntaxExtra As String = HelpSectionTests.GetPlainHelpValue("SyntaxExtra")

        For Each param As CommandlineParameter In TestHelpers.CreateApplicationParameters()
            Assert.Contains(param.Name, syntaxExtra, $"Undocumented parameter: {param.Name}")
            Assert.Contains($"or {param.ShortName}", syntaxExtra, $"Undocumented short name: {param.ShortName}")
        Next param

        Assert.Contains("/?", syntaxExtra)

    End Sub

    ''' <summary>
    ''' Verifies that the syntax section documents the required parameters using their short name and separator.
    ''' </summary>
    <TestMethod>
    Public Sub Help_Syntax_DocumentsRequiredParameters()

        Dim syntax As String = HelpSectionTests.GetPlainHelpValue("Syntax")

        For Each param As CommandlineParameter In TestHelpers.CreateApplicationParameters()
            If Not param.IsOptional Then
                Assert.Contains($"{param.ShortName}{param.Separator}", syntax, $"Undocumented parameter: {param.Name}")
            End If
        Next param

    End Sub

    ''' <summary>
    ''' Verifies that the website shown in the help points to the project repository.
    ''' </summary>
    <TestMethod>
    Public Sub Help_Website_PointsToRepository()

        Dim website As String = HelpSectionTests.GetPlainHelpValue("Website")

        Assert.IsTrue(Uri.IsWellFormedUriString(website, UriKind.Absolute), website)
        Assert.StartsWith("https://github.com/ElektroStudios/", website)

    End Sub

#End Region

End Class

#End Region
