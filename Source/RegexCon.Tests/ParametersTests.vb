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

Imports RegexCon.Logic
Imports RegexCon.Types

#End Region

#Region " Parameters Tests "

''' <summary>
''' Unit tests for the parameter definitions of the application (<see cref="Parameters"/> module).
''' </summary>
<TestClass>
Public NotInheritable Class ParametersTests

    ''' <summary>
    ''' Verifies the definition of the <c>/Input</c> parameter.
    ''' </summary>
    <TestMethod>
    Public Sub ParamInput_Definition()

        ParametersTests.AssertDefinition(Parameters.ParamInput, "/Input", "/I", "", isOptional:=False)

    End Sub

    ''' <summary>
    ''' Verifies the definition of the <c>/Expression</c> parameter.
    ''' </summary>
    <TestMethod>
    Public Sub ParamExpression_Definition()

        ParametersTests.AssertDefinition(Parameters.ParamExpression, "/Expression", "/E", "", isOptional:=False)

    End Sub

    ''' <summary>
    ''' Verifies the definition of the <c>/Substitution</c> parameter.
    ''' </summary>
    <TestMethod>
    Public Sub ParamSubstitution_Definition()

        ParametersTests.AssertDefinition(Parameters.ParamSubstitution, "/Substitution", "/S", "", isOptional:=False)

    End Sub

    ''' <summary>
    ''' Verifies the definition of the <c>/MatchCase</c> parameter.
    ''' </summary>
    <TestMethod>
    Public Sub ParamMatchCase_Definition()

        ParametersTests.AssertDefinition(Parameters.ParamMatchCase, "/MatchCase", "/M", True, isOptional:=True)

    End Sub

    ''' <summary>
    ''' Verifies that all the application parameters can be registered together without name conflicts.
    ''' </summary>
    <TestMethod>
    Public Sub ApplicationParameters_RegisterWithoutConflicts()

        Dim params As CommandlineParameterCollection = TestHelpers.CreateApplicationParameters()

        Assert.HasCount(4, params)

    End Sub

    ''' <summary>
    ''' Verifies that the parameter definitions are not modified by parsing,
    ''' because the collection stores converted copies of them.
    ''' </summary>
    <TestMethod>
    Public Sub ApplicationParameters_ParsingDoesNotModifyDefinitions()

        Dim params As CommandlineParameterCollection = TestHelpers.CreateApplicationParameters()
        Dim noop As Action(Of CommandlineParameter) = Sub(cmd As CommandlineParameter)
                                                      End Sub
        Dim noopArg As Action(Of String) = Sub(arg As String)
                                           End Sub

        Parsers.ParseArguments(params, {"/I=abc", "/E=b", "/S=X", "/M=False"}, noop, noop, noop, noopArg)

        Assert.IsNull(Parameters.ParamInput.Value)
        Assert.IsFalse(Parameters.ParamMatchCase.Value)
        Assert.IsTrue(Parameters.ParamMatchCase.DefaultValue)

    End Sub

    ''' <summary>
    ''' Asserts the definition of a parameter.
    ''' </summary>
    ''' <param name="param">
    ''' The parameter to verify.
    ''' </param>
    '''
    ''' <param name="name">
    ''' The expected name.
    ''' </param>
    '''
    ''' <param name="shortName">
    ''' The expected short name.
    ''' </param>
    '''
    ''' <param name="defaultValue">
    ''' The expected default value.
    ''' </param>
    '''
    ''' <param name="isOptional">
    ''' The expected optional state.
    ''' </param>
    Private Shared Sub AssertDefinition(Of T)(ByVal param As CommandlineParameter(Of T),
                                              ByVal name As String,
                                              ByVal shortName As String,
                                              ByVal defaultValue As T,
                                              ByVal isOptional As Boolean)

        Assert.AreEqual(name, param.Name)
        Assert.AreEqual(shortName, param.ShortName)
        Assert.AreEqual("="c, param.Separator)
        Assert.AreEqual(defaultValue, param.DefaultValue)
        Assert.AreEqual(isOptional, param.IsOptional)

    End Sub

End Class

#End Region
