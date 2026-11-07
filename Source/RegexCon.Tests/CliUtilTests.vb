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
Imports RegexCon.UserInterface

#End Region

#Region " CliUtil Tests "

''' ----------------------------------------------------------------------------------------------------
''' <summary>
''' Unit tests for <see cref="CliUtil"/> type.
''' </summary>
''' ----------------------------------------------------------------------------------------------------
<TestClass>
Public NotInheritable Class CliUtilTests

#Region " WriteColorText (Delimiters) "

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that the color tokens are not printed and the remaining text is printed in order.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub WriteColorText_ColorTokens_AreNotPrinted()

        Dim output As String = TestHelpers.CaptureConsoleOutput(
            Sub() CliUtil.WriteColorText("*F10*Hello*-F* *B1*World*-B**f7*!*-F*", {"*"c}))

        Assert.AreEqual("Hello World!", output)

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that the text without color tokens is printed as is.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub WriteColorText_PlainText_IsPrintedAsIs()

        Dim output As String = TestHelpers.CaptureConsoleOutput(
            Sub() CliUtil.WriteColorText("Plain text, without tokens.", {"*"c}))

        Assert.AreEqual("Plain text, without tokens.", output)

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that the console colors are restored after writing.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub WriteColorText_AfterWriting_RestoresConsoleColors()

        Dim foreColor As ConsoleColor = Console.ForegroundColor
        Dim backColor As ConsoleColor = Console.BackgroundColor

        TestHelpers.CaptureConsoleOutput(Sub() CliUtil.WriteColorText("*F12**B14*Text", {"*"c}))

        Assert.AreEqual(foreColor, Console.ForegroundColor)
        Assert.AreEqual(backColor, Console.BackgroundColor)

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that <see cref="CliUtil.WriteColorTextLine(String, Char())"/> appends a line break.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub WriteColorTextLine_Delimiters_AppendsNewLine()

        Dim output As String = TestHelpers.CaptureConsoleOutput(
            Sub() CliUtil.WriteColorTextLine("*F10*Line*-F*", {"*"c}))

        Assert.AreEqual($"Line{Environment.NewLine}", output)

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that the whole help text is printed without any color token.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub WriteColorText_HelpSyntaxExtra_PrintsWithoutTokens()

        Dim helpText As String = HelpSection.Help.<SyntaxExtra>.Value

        Dim output As String = TestHelpers.CaptureConsoleOutput(Sub() CliUtil.WriteColorText(helpText, {"*"c}))

        Assert.DoesNotContain("*", output)
        Assert.Contains("/Input", output)
        Assert.Contains("The regular expression.", output)

    End Sub

#End Region

#Region " WriteColorText (Colors) "

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that <see cref="CliUtil.WriteColorText(String, Nullable(Of ConsoleColor), Nullable(Of ConsoleColor))"/> prints the text.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub WriteColorText_Colors_PrintsText()

        Dim output As String = TestHelpers.CaptureConsoleOutput(
            Sub() CliUtil.WriteColorText("Colored", ConsoleColor.Green, ConsoleColor.DarkBlue))

        Assert.AreEqual("Colored", output)

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that <see cref="CliUtil.WriteColorTextLine(String, Nullable(Of ConsoleColor), Nullable(Of ConsoleColor))"/> appends a line break
    ''' and restores the console colors.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub WriteColorTextLine_Colors_AppendsNewLineAndRestoresColors()

        Dim foreColor As ConsoleColor = Console.ForegroundColor
        Dim backColor As ConsoleColor = Console.BackgroundColor

        Dim output As String = TestHelpers.CaptureConsoleOutput(
            Sub() CliUtil.WriteColorTextLine("Colored", ConsoleColor.Red, ConsoleColor.Yellow))

        Assert.AreEqual($"Colored{Environment.NewLine}", output)
        Assert.AreEqual(foreColor, Console.ForegroundColor)
        Assert.AreEqual(backColor, Console.BackgroundColor)

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that <see langword="Nothing"/> colors keep the current console colors.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub WriteColorText_NothingColors_PrintsTextAndKeepsColors()

        Dim foreColor As ConsoleColor = Console.ForegroundColor
        Dim backColor As ConsoleColor = Console.BackgroundColor

        Dim output As String = TestHelpers.CaptureConsoleOutput(
            Sub() CliUtil.WriteColorText("Text", Nothing, Nothing))

        Assert.AreEqual("Text", output)
        Assert.AreEqual(foreColor, Console.ForegroundColor)
        Assert.AreEqual(backColor, Console.BackgroundColor)

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that <see cref="ConsoleColor.Black"/> is accepted as a color.
    ''' <para></para>
    ''' Regression test: before v1.2, <see cref="ConsoleColor.Black"/> (value 0) was compared as <see langword="Nothing"/>, 
    ''' so it was treated as "keep the current color" and it was impossible to use black.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub WriteColorText_BlackColor_IsAccepted()

        Dim output As String = TestHelpers.CaptureConsoleOutput(
            Sub() CliUtil.WriteColorTextLine("Black", ConsoleColor.Black, ConsoleColor.Black))

        Assert.AreEqual($"Black{Environment.NewLine}", output)

    End Sub

#End Region

#Region " Argument Validation "

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that <see langword="Nothing"/> text is rejected by all the overloads.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub WriteColorText_NullText_ThrowsArgumentNullException()

        Assert.ThrowsExactly(Of ArgumentNullException)(Sub() CliUtil.WriteColorText(Nothing, {"*"c}))
        Assert.ThrowsExactly(Of ArgumentNullException)(Sub() CliUtil.WriteColorTextLine(Nothing, {"*"c}))
        Assert.ThrowsExactly(Of ArgumentNullException)(Sub() CliUtil.WriteColorText(Nothing, ConsoleColor.Red, ConsoleColor.Blue))
        Assert.ThrowsExactly(Of ArgumentNullException)(Sub() CliUtil.WriteColorTextLine(Nothing, ConsoleColor.Red, ConsoleColor.Blue))

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that an invalid set of delimiters is rejected.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub WriteColorText_InvalidDelimiters_ThrowsArgumentException()

        Dim nullDelimiters As Char() = Nothing

        Assert.ThrowsExactly(Of ArgumentNullException)(Sub() CliUtil.WriteColorText("Text", nullDelimiters))
        Assert.ThrowsExactly(Of ArgumentException)(Sub() CliUtil.WriteColorText("Text", Array.Empty(Of Char)()))
        Assert.ThrowsExactly(Of ArgumentException)(Sub() CliUtil.WriteColorText("Text", {"*"c, "{"c, "}"c}))

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that two different start/end delimiters are supported.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub WriteColorText_TwoDelimiters_AreSupported()

        Dim output As String = TestHelpers.CaptureConsoleOutput(
            Sub() CliUtil.WriteColorText("{F10}Hello{-F} World", {"{"c, "}"c}))

        Assert.AreEqual("Hello World", output)

    End Sub

#End Region

End Class


#End Region
