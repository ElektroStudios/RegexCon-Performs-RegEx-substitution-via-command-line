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

''' ----------------------------------------------------------------------------------------------------
''' <summary>
''' Unit tests for <see cref="RegExUtil"/> type.
''' </summary>
''' ----------------------------------------------------------------------------------------------------
<TestClass>
Public NotInheritable Class RegExUtilTests

#Region " ValidateExpression "

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that valid regular expressions pass the validation.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <param name="expression">
    ''' The regular expression.
    ''' </param>
    ''' ----------------------------------------------------------------------------------------------------
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

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that invalid regular expressions fail the validation.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <param name="expression">
    ''' The regular expression.
    ''' </param>
    ''' ----------------------------------------------------------------------------------------------------
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

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that a <see langword="Nothing"/> expression fails the validation.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub ValidateExpression_Nothing_ReturnsFalse()

        Assert.IsFalse(RegExUtil.ValidateExpression(Nothing))

    End Sub

#End Region

#Region " Substitute "

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies the substitutions documented in the usage examples of the help.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <param name="pattern">
    ''' The regular expression.
    ''' </param>
    '''
    ''' <param name="replacement">
    ''' The substitution string.
    ''' </param>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    <DataRow("^(.+)\s(.+)$", "$2 $1", DisplayName:="Regular string substitution")>
    <DataRow("^(?<one>.+)\s(?<two>.+)$", "${two} ${one}", DisplayName:="Named-group string substitution")>
    Public Sub Substitute_HelpUsageExamples_SwapsWords(ByVal pattern As String, ByVal replacement As String)

        Assert.AreEqual("World Hello", RegExUtil.Substitute("Hello World", pattern, replacement, matchCase:=True))

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that a case-sensitive substitution does not match characters with different case.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub Substitute_MatchCaseTrue_IsCaseSensitive()

        Assert.AreEqual("Hello hello X", RegExUtil.Substitute("Hello hello HELLO", "HELLO", "X", matchCase:=True))

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that a case-insensitive substitution matches characters with any case.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub Substitute_MatchCaseFalse_IsCaseInsensitive()

        Assert.AreEqual("X X X", RegExUtil.Substitute("Hello hello HELLO", "HELLO", "X", matchCase:=False))

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that all the matches are replaced, not only the first one.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub Substitute_MultipleMatches_ReplacesAll()

        Assert.AreEqual("a-b-c-d", RegExUtil.Substitute("a b  c   d", "\s+", "-", matchCase:=True))

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that the input is returned unchanged when there are no matches.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub Substitute_NoMatches_ReturnsInputUnchanged()

        Assert.AreEqual("Hello World", RegExUtil.Substitute("Hello World", "\d+", "X", matchCase:=True))

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that an empty substitution string removes the matches.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub Substitute_EmptyReplacement_RemovesMatches()

        Assert.AreEqual("Track.mp3", RegExUtil.Substitute("01 - Track.mp3", "^\d+\s*-\s*", "", matchCase:=True))

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that the substitution special tokens (<c>$0</c>, <c>$$</c>) are supported.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub Substitute_SpecialTokens_AreSupported()

        Assert.AreEqual("[abc] $", RegExUtil.Substitute("abc", "^.+$", "[$0] $$", matchCase:=True))

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies the artist/title substitution sample documented in the source code of the application.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub Substitute_SongTitleSample_MovesFeaturingAfterArtist()

        Const Pattern As String = "^(?<artist>.+?(?=\s*-\s*))\s*-\s*(?<title>.+?(?=(?:\(?(ft|feat|featuring)\.)|$))\(?(?<feat>(ft|feat|featuring)\.[^()\n]+)?\)?(?<subtitle>.+)?$"
        Const Replacement As String = "${artist} ${feat} - ${title} ${subtitle}"

        Dim result As String = RegExUtil.Substitute("Black Coast - Trndsttr (Feat. M. Maggie)", Pattern, Replacement, matchCase:=False)

        Assert.AreEqual("Black Coast Feat. M. Maggie - Trndsttr", result.Trim())

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that an invalid regular expression throws <see cref="ArgumentException"/>.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub Substitute_InvalidPattern_ThrowsArgumentException()

        Assert.Throws(Of ArgumentException)(Sub() RegExUtil.Substitute("abc", "(abc", "X", matchCase:=True))

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that <see langword="Nothing"/> arguments throw <see cref="ArgumentNullException"/>.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub Substitute_NullArguments_ThrowsArgumentNullException()

        Assert.AreEqual("input", Assert.ThrowsExactly(Of ArgumentNullException)(
            Sub() RegExUtil.Substitute(Nothing, "a", "b", matchCase:=True)).ParamName)

        Assert.AreEqual("pattern", Assert.ThrowsExactly(Of ArgumentNullException)(
            Sub() RegExUtil.Substitute("a", Nothing, "b", matchCase:=True)).ParamName)

        Assert.AreEqual("replacement", Assert.ThrowsExactly(Of ArgumentNullException)(
            Sub() RegExUtil.Substitute("a", "a", Nothing, matchCase:=True)).ParamName)

    End Sub

#End Region

End Class

#End Region
