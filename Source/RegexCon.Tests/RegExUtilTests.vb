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

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports RegexCon.Tools

#End Region

#Region " RegExUtil Tests "

''' <summary>
''' Unit tests for <see cref="RegExUtil"/> type.
''' </summary>
<TestClass>
Public NotInheritable Class RegExUtilTests

#Region " ValidateExpression "

    ''' <summary>
    ''' Verifies that valid regular expressions pass the validation.
    ''' </summary>
    ''' <param name="expression">
    ''' The regular expression.
    ''' </param>
    <TestMethod>
    <DataRow("")>
    <DataRow("abc")>
    <DataRow("^(.+)\s(.+)$")>
    <DataRow("^(?<one>.+)\s(?<two>.+)$")>
    <DataRow("[0-9]{1,3}(?:\.[0-9]{1,3}){3}")>
    <DataRow("(?i)hello")>
    Public Sub ValidateExpression_ValidExpression_ReturnsTrue(ByVal expression As String)

        Assert.IsTrue(RegExUtil.ValidateExpression(expression))

    End Sub

    ''' <summary>
    ''' Verifies that invalid regular expressions fail the validation.
    ''' </summary>
    ''' <param name="expression">
    ''' The regular expression.
    ''' </param>
    <TestMethod>
    <DataRow("[0-9]++")>
    <DataRow("(abc")>
    <DataRow("abc)")>
    <DataRow("[a-")>
    <DataRow("*abc")>
    <DataRow("(?<name>abc")>
    <DataRow("\")>
    Public Sub ValidateExpression_InvalidExpression_ReturnsFalse(ByVal expression As String)

        Assert.IsFalse(RegExUtil.ValidateExpression(expression))

    End Sub

    ''' <summary>
    ''' Verifies that a <see langword="Nothing"/> expression fails the validation.
    ''' </summary>
    <TestMethod>
    Public Sub ValidateExpression_Nothing_ReturnsFalse()

        Assert.IsFalse(RegExUtil.ValidateExpression(Nothing))

    End Sub

#End Region

End Class

#End Region
