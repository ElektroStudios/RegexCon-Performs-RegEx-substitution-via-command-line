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
Imports System.Threading

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports RegexCon.Logic
Imports RegexCon.Types

#End Region

#Region " Parsers Tests "

''' ----------------------------------------------------------------------------------------------------
''' <summary>
''' Unit tests for the command-line argument parser (<see cref="Parsers"/> module).
''' <para></para>
''' Note: the code paths that print the help (no arguments, or <c>/?</c>) terminate the process,
''' so they are covered by <see cref="EndToEndTests"/> instead.
''' <para></para>
''' The parser is strict: unknown arguments, duplicated parameters and parameters without value are errors.
''' </summary>
''' ----------------------------------------------------------------------------------------------------
<TestClass>
Public NotInheritable Class ParsersTests

#Region " Private Fields "

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' The parameters reported by the syntax error callback.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    Private syntaxErrors As List(Of CommandlineParameter)

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' The parameters reported by the missing required parameter callback.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    Private missingRequired As List(Of CommandlineParameter)

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' The parameters reported by the duplicated parameter callback.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    Private duplicated As List(Of CommandlineParameter)

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' The arguments reported by the unknown argument callback.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    Private unknownArgs As List(Of String)

#End Region

#Region " Test Initialization "

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Resets the callback recorders before each test.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestInitialize>
    Public Sub TestInitialize()

        Me.syntaxErrors = New List(Of CommandlineParameter)()
        Me.missingRequired = New List(Of CommandlineParameter)()
        Me.duplicated = New List(Of CommandlineParameter)()
        Me.unknownArgs = New List(Of String)()

    End Sub

#End Region

#Region " Private Methods "

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Parses the specified arguments using recording callbacks.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <param name="params">
    ''' The parameters to fill.
    ''' </param>
    '''
    ''' <param name="args">
    ''' The command-line arguments.
    ''' </param>
    ''' ----------------------------------------------------------------------------------------------------
    Private Sub Parse(ByVal params As CommandlineParameterCollection, ParamArray args As String())

        Parsers.ParseArguments(params, args,
                               Sub(cmd As CommandlineParameter) Me.syntaxErrors.Add(cmd),
                               Sub(cmd As CommandlineParameter) Me.missingRequired.Add(cmd),
                               Sub(cmd As CommandlineParameter) Me.duplicated.Add(cmd),
                               Sub(arg As String) Me.unknownArgs.Add(arg))

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Asserts that no callback was invoked.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    Private Sub AssertNoErrors()

        Assert.IsEmpty(Me.syntaxErrors, "Unexpected syntax error callback.")
        Assert.IsEmpty(Me.missingRequired, "Unexpected missing parameter callback.")
        Assert.IsEmpty(Me.duplicated, "Unexpected duplicated parameter callback.")
        Assert.IsEmpty(Me.unknownArgs, "Unexpected unknown argument callback.")

    End Sub

#End Region

#Region " Valid Arguments "

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that the values are parsed using the long parameter names.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub ParseArguments_LongNames_AssignsValues()

        Dim params As CommandlineParameterCollection = TestHelpers.CreateApplicationParameters()

        Me.Parse(params, "/Input=Hello World", "/Expression=^(.+)\s(.+)$", "/Substitution=$2 $1", "/MatchCase=False")

        Me.AssertNoErrors()
        Assert.AreEqual("Hello World", params("/Input").Value)
        Assert.AreEqual("^(.+)\s(.+)$", params("/Expression").Value)
        Assert.AreEqual("$2 $1", params("/Substitution").Value)
        Assert.AreEqual(False, params("/MatchCase").Value)

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that the values are parsed using the short parameter names.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub ParseArguments_ShortNames_AssignsValues()

        Dim params As CommandlineParameterCollection = TestHelpers.CreateApplicationParameters()

        Me.Parse(params, "/I=abc", "/E=b", "/S=X", "/M=True")

        Me.AssertNoErrors()
        Assert.AreEqual("abc", params("/Input").Value)
        Assert.AreEqual("b", params("/Expression").Value)
        Assert.AreEqual("X", params("/Substitution").Value)
        Assert.AreEqual(True, params("/MatchCase").Value)

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that the parameter names are matched case-insensitive.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub ParseArguments_MixedCaseNames_AssignsValues()

        Dim params As CommandlineParameterCollection = TestHelpers.CreateApplicationParameters()

        Me.Parse(params, "/input=abc", "/EXPRESSION=b", "/s=X", "/m=false")

        Me.AssertNoErrors()
        Assert.AreEqual("abc", params("/Input").Value)
        Assert.AreEqual("b", params("/Expression").Value)
        Assert.AreEqual("X", params("/Substitution").Value)
        Assert.AreEqual(False, params("/MatchCase").Value)

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that only the first separator splits the name from the value,
    ''' so the value can contain the separator character.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub ParseArguments_ValueContainsSeparator_KeepsWholeValue()

        Dim params As CommandlineParameterCollection = TestHelpers.CreateApplicationParameters()

        Me.Parse(params, "/I=a=b=c", "/E=(?<name>=)", "/S=${name}=")

        Me.AssertNoErrors()
        Assert.AreEqual("a=b=c", params("/Input").Value)
        Assert.AreEqual("(?<name>=)", params("/Expression").Value)
        Assert.AreEqual("${name}=", params("/Substitution").Value)

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that an empty value assigns the default value of the parameter.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub ParseArguments_EmptyValues_AssignsDefaultValues()

        Dim params As CommandlineParameterCollection = TestHelpers.CreateApplicationParameters()

        Me.Parse(params, "/I=", "/E=", "/S=", "/M=")

        Me.AssertNoErrors()
        Assert.AreEqual("", params("/Input").Value)
        Assert.AreEqual("", params("/Expression").Value)
        Assert.AreEqual("", params("/Substitution").Value)
        Assert.AreEqual(True, params("/MatchCase").Value)

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that an omitted optional parameter takes its default value.
    ''' <para></para>
    ''' Regression test: before v1.2, omitting <c>/MatchCase</c> left its value as <see langword="False"/>
    ''' instead of its documented default value (<see langword="True"/>),
    ''' so the substitution was performed case-insensitive.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub ParseArguments_OmittedMatchCase_TakesDefaultValueTrue()

        Dim params As CommandlineParameterCollection = TestHelpers.CreateApplicationParameters()

        Me.Parse(params, "/I=abc", "/E=b", "/S=X")

        Me.AssertNoErrors()
        Assert.AreEqual(True, params("/MatchCase").Value)

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that the values are converted to the type of the default value.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub ParseArguments_IntegerParameter_ConvertsValue()

        Dim params As New CommandlineParameterCollection()
        params.Add(TestHelpers.CreateParameter("/Number", "/N", 0, isOptional:=False))

        Me.Parse(params, "/N=42")

        Me.AssertNoErrors()
        Assert.IsInstanceOfType(Of Integer)(params("/Number").Value)
        Assert.AreEqual(42, params("/Number").Value)

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that a parameter without short name is matched by its name.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub ParseArguments_ParameterWithoutShortName_MatchesByName()

        Dim params As New CommandlineParameterCollection()
        params.Add(TestHelpers.CreateParameter("/Switch1", Nothing, "default", isOptional:=False))

        Me.Parse(params, "/switch1=value")

        Me.AssertNoErrors()
        Assert.AreEqual("value", params("/Switch1").Value)

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that the values are converted using the invariant culture, 
    ''' so the result does not depend on the regional settings of the user.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub ParseArguments_DoubleParameter_UsesInvariantCulture()

        Dim params As New CommandlineParameterCollection()
        params.Add(TestHelpers.CreateParameter("/Ratio", "/R", 0.0R, isOptional:=False))

        Dim previousCulture As CultureInfo = Thread.CurrentThread.CurrentCulture
        Try
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("es-ES") ' Uses "," as decimal separator.
            Me.Parse(params, "/R=1.5")
        Finally
            Thread.CurrentThread.CurrentCulture = previousCulture
        End Try

        Me.AssertNoErrors()
        Assert.AreEqual(1.5R, params("/Ratio").Value)

    End Sub

#End Region

#Region " Invalid Arguments "

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that an argument that does not match any parameter invokes the unknown argument callback 
    ''' and stops the parsing.
    ''' <para></para>
    ''' Regression test: before v1.2 the unknown arguments were silently ignored.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <param name="unknownArg">
    ''' The unknown argument.
    ''' </param>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    <DataRow("/Unknown=1")>
    <DataRow("garbage")>
    <DataRow("/Inputs=abc")>
    <DataRow("/Inputs")>
    <DataRow("I=abc")>
    <DataRow("/I:abc")>
    <DataRow("")>
    Public Sub ParseArguments_UnknownArgument_InvokesUnknownArgumentCallback(ByVal unknownArg As String)

        Dim params As CommandlineParameterCollection = TestHelpers.CreateApplicationParameters()

        Me.Parse(params, "/I=abc", unknownArg, "/E=b", "/S=X")

        Assert.HasCount(1, Me.unknownArgs)
        Assert.AreEqual(unknownArg, Me.unknownArgs(0))
        Assert.IsEmpty(Me.syntaxErrors)
        Assert.IsEmpty(Me.duplicated)
        Assert.IsEmpty(Me.missingRequired, "The parsing must stop after an unknown argument.")
        Assert.AreEqual("", params("/Expression").Value, "Arguments after the unknown argument must not be parsed.")

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that a parameter without short name does not match arguments that start with the separator.
    ''' <para></para>
    ''' Regression test: before v1.2, a parameter without short name matched any argument starting with "=".
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub ParseArguments_ParameterWithoutShortName_DoesNotMatchBareSeparator()

        Dim params As New CommandlineParameterCollection()
        params.Add(TestHelpers.CreateParameter("/Switch1", Nothing, "default", isOptional:=False))

        Me.Parse(params, "=value")

        Assert.HasCount(1, Me.unknownArgs)
        Assert.AreEqual("=value", Me.unknownArgs(0))
        Assert.AreEqual("default", params("/Switch1").Value)

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that a parameter specified more than once invokes the duplicated parameter callback 
    ''' and stops the parsing.
    ''' <para></para>
    ''' Regression test: before v1.2 the last occurrence silently overwrote the previous ones.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <param name="first">
    ''' The first occurrence of the parameter.
    ''' </param>
    ''' 
    ''' <param name="second">
    ''' The second occurrence of the parameter.
    ''' </param>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    <DataRow("/I=first", "/I=second", DisplayName:="Short name twice")>
    <DataRow("/Input=first", "/input=second", DisplayName:="Name twice")>
    <DataRow("/I=first", "/Input=second", DisplayName:="Short name and name")>
    <DataRow("/I=", "/I=second", DisplayName:="Empty value and value")>
    Public Sub ParseArguments_DuplicatedParameter_InvokesDuplicatedCallback(ByVal first As String, ByVal second As String)

        Dim params As CommandlineParameterCollection = TestHelpers.CreateApplicationParameters()

        Me.Parse(params, first, "/E=b", second, "/S=X")

        Assert.HasCount(1, Me.duplicated)
        Assert.AreEqual("/Input", Me.duplicated(0).Name)
        Assert.IsEmpty(Me.syntaxErrors)
        Assert.IsEmpty(Me.unknownArgs)
        Assert.IsEmpty(Me.missingRequired, "The parsing must stop after a duplicated parameter.")
        Assert.AreNotEqual("second", params("/Input").Value)

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that a known parameter specified without separator and value (e.g. <c>/I</c>) 
    ''' invokes the syntax error callback for that parameter.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <param name="bareSwitch">
    ''' The parameter specified without separator.
    ''' </param>
    ''' 
    ''' <param name="expectedName">
    ''' The expected name of the reported parameter.
    ''' </param>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    <DataRow("/I", "/Input")>
    <DataRow("/input", "/Input")>
    <DataRow("/M", "/MatchCase")>
    Public Sub ParseArguments_ParameterWithoutSeparator_InvokesSyntaxErrorCallback(ByVal bareSwitch As String, ByVal expectedName As String)

        Dim params As CommandlineParameterCollection = TestHelpers.CreateApplicationParameters()

        Me.Parse(params, bareSwitch, "/E=b", "/S=X")

        Assert.HasCount(1, Me.syntaxErrors)
        Assert.AreEqual(expectedName, Me.syntaxErrors(0).Name)
        Assert.IsEmpty(Me.unknownArgs)
        Assert.IsEmpty(Me.missingRequired)

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that a parameter without default value is rejected, because its value type cannot be determined.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub ParseArguments_ParameterWithoutDefaultValue_ThrowsInvalidOperationException()

        Dim params As New CommandlineParameterCollection()
        params.Add(TestHelpers.CreateParameter("/Switch1", "/S1", Nothing, isOptional:=False))

        Assert.ThrowsExactly(Of InvalidOperationException)(Sub() Me.Parse(params, "/S1=value"))

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that an argument collection that contains <see langword="Nothing"/> is rejected.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub ParseArguments_NullArgumentInCollection_ThrowsArgumentException()

        Dim params As CommandlineParameterCollection = TestHelpers.CreateApplicationParameters()

        Dim ex As ArgumentException =
            Assert.ThrowsExactly(Of ArgumentException)(Sub() Me.Parse(params, "/I=abc", Nothing))

        Assert.AreEqual("args", ex.ParamName)

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that a value that cannot be converted invokes the syntax error callback.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub ParseArguments_InvalidBooleanValue_InvokesSyntaxErrorCallback()

        Dim params As CommandlineParameterCollection = TestHelpers.CreateApplicationParameters()

        Me.Parse(params, "/I=abc", "/E=b", "/S=X", "/M=Maybe")

        Assert.HasCount(1, Me.syntaxErrors)
        Assert.AreEqual("/MatchCase", Me.syntaxErrors(0).Name)
        Assert.IsEmpty(Me.missingRequired, "The parsing must stop after a syntax error.")

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that the parsing stops at the first syntax error.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub ParseArguments_SyntaxError_StopsParsing()

        Dim params As New CommandlineParameterCollection()
        params.Add(TestHelpers.CreateParameter("/Number", "/N", 0, isOptional:=False))
        params.Add(TestHelpers.CreateParameter("/Text", "/T", "", isOptional:=False))

        Me.Parse(params, "/N=NaN", "/T=abc")

        Assert.HasCount(1, Me.syntaxErrors)
        Assert.AreEqual("/Number", Me.syntaxErrors(0).Name)
        Assert.AreEqual("", params("/Text").Value, "Arguments after the syntax error must not be parsed.")

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that a missing required parameter invokes the callback with the first missing parameter.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub ParseArguments_MissingRequired_InvokesCallbackWithFirstMissing()

        Dim params As CommandlineParameterCollection = TestHelpers.CreateApplicationParameters()

        Me.Parse(params, "/S=X")

        Assert.IsEmpty(Me.syntaxErrors)
        Assert.HasCount(1, Me.missingRequired)
        Assert.AreEqual("/Input", Me.missingRequired(0).Name)

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that only the required parameters are reported as missing (the optional ones are not).
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub ParseArguments_MissingSubstitution_ReportsSubstitution()

        Dim params As CommandlineParameterCollection = TestHelpers.CreateApplicationParameters()

        Me.Parse(params, "/I=abc", "/E=b")

        Assert.HasCount(1, Me.missingRequired)
        Assert.AreEqual("/Substitution", Me.missingRequired(0).Name)

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that a required parameter given with an empty value is not reported as missing.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub ParseArguments_RequiredWithEmptyValue_IsNotMissing()

        Dim params As CommandlineParameterCollection = TestHelpers.CreateApplicationParameters()

        Me.Parse(params, "/I=abc", "/E=b", "/S=")

        Me.AssertNoErrors()

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that <see langword="Nothing"/> arguments are rejected.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub ParseArguments_NullArguments_ThrowsArgumentNullException()

        Dim params As CommandlineParameterCollection = TestHelpers.CreateApplicationParameters()
        Dim args As String() = {"/I=abc"}
        Dim noop As Action(Of CommandlineParameter) = Sub(cmd As CommandlineParameter)
                                                      End Sub
        Dim noopArg As Action(Of String) = Sub(arg As String)
                                           End Sub

        Assert.AreEqual("cmds", Assert.ThrowsExactly(Of ArgumentNullException)(
            Sub() Parsers.ParseArguments(Nothing, args, noop, noop, noop, noopArg)).ParamName)

        Assert.AreEqual("args", Assert.ThrowsExactly(Of ArgumentNullException)(
            Sub() Parsers.ParseArguments(params, Nothing, noop, noop, noop, noopArg)).ParamName)

        Assert.AreEqual("callbackSyntaxError", Assert.ThrowsExactly(Of ArgumentNullException)(
            Sub() Parsers.ParseArguments(params, args, Nothing, noop, noop, noopArg)).ParamName)

        Assert.AreEqual("callbackMissingRequired", Assert.ThrowsExactly(Of ArgumentNullException)(
            Sub() Parsers.ParseArguments(params, args, noop, Nothing, noop, noopArg)).ParamName)

        Assert.AreEqual("callbackDuplicated", Assert.ThrowsExactly(Of ArgumentNullException)(
            Sub() Parsers.ParseArguments(params, args, noop, noop, Nothing, noopArg)).ParamName)

        Assert.AreEqual("callbackUnknownArgument", Assert.ThrowsExactly(Of ArgumentNullException)(
            Sub() Parsers.ParseArguments(params, args, noop, noop, noop, Nothing)).ParamName)

    End Sub

#End Region

#Region " Fields "

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that <see cref="Fields.SetFields"/> copies the parsed values to the <see cref="Fields"/> module.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub SetFields_AfterParsing_CopiesParsedValues()

        Dim params As CommandlineParameterCollection = TestHelpers.CreateApplicationParameters()
        Me.Parse(params, "/I=Hello World", "/E=world", "/S=There", "/M=False")

        Dim previousParams As CommandlineParameterCollection = Fields.Params
        Try
            Fields.Params = params
            Fields.SetFields()

            Assert.AreEqual("Hello World", Fields.Input)
            Assert.AreEqual("world", Fields.Expression)
            Assert.AreEqual("There", Fields.Substitution)
            Assert.IsFalse(Fields.MatchCase)
        Finally
            Fields.Params = previousParams
        End Try

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that <see cref="Fields.SetFields"/> gets <c>MatchCase = True</c> when <c>/MatchCase</c> is omitted.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub SetFields_OmittedMatchCase_IsTrue()

        Dim params As CommandlineParameterCollection = TestHelpers.CreateApplicationParameters()
        Me.Parse(params, "/I=abc", "/E=b", "/S=X")

        Dim previousParams As CommandlineParameterCollection = Fields.Params
        Try
            Fields.Params = params
            Fields.SetFields()

            Assert.IsTrue(Fields.MatchCase)
        Finally
            Fields.Params = previousParams
        End Try

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that the regular expression validation is enabled by default.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub Validate_IsEnabled()

        Assert.IsTrue(Fields.Validate)

    End Sub

#End Region

End Class

#End Region
