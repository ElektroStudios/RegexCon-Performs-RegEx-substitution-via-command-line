' ***********************************************************************
' Author   : Elektro
' Modified : 07-November-2026
' ***********************************************************************

#Region " Public Members Summary "

#Region " Constructors "

' New()

#End Region

#Region " Properties "

' IsOptional As Boolean
' Name As String
' ShortName As String
' Separator As Char
' Value As Object
' DefaultValue As Object

#End Region

#End Region

#Region " Usage Examples "

' Private ReadOnly cmd1 As New CommandlineParameter(Of String) With {
'     .Name = "/Switch1",
'     .ShortName = "/S1",
'     .Separator = "="c,
'     .DefaultValue = "Hello World",
'     .IsOptional = False
' }
'
' Private ReadOnly cmd2 As New CommandlineParameter(Of Boolean) With {
'     .Name = "/Switch2",
'     .Separator = "="c,
'     .DefaultValue = False,
'     .IsOptional = True
' }
'
' Sub Main()
'
'     Dim cmds As New CommandlineParameterCollection()
'     cmds.AddRange({cmd1, cmd2})
'
'     ' See the "Logic.Parsers" module for the argument parser implementation.
'     Parsers.ParseArguments(cmds,
'                            AddressOf ErrorCallbacks.OnSyntaxError,
'                            AddressOf ErrorCallbacks.OnMissingParameterRequired,
'                            AddressOf ErrorCallbacks.OnDuplicatedParameter,
'                            AddressOf ErrorCallbacks.OnUnknownArgument)
'
'     Dim switch1 As String = CStr(cmds("/Switch1").Value)
'     Dim switch2 As Boolean = CBool(cmds("/Switch2").Value)
'
' End Sub

#End Region

#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Linq

#End Region

Namespace Types

#Region " Commandline Parameter "

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Represents a Commandline Parameter.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <example> This is a code example.
    ''' <code>
    ''' Dim cmd1 As New CommandlineParameter(Of String) With {
    '''     .Name = "/Switch1",
    '''     .ShortName = "/S1",
    '''     .Separator = "="c,
    '''     .DefaultValue = "Hello World",
    '''     .IsOptional = False
    ''' }
    ''' 
    ''' Dim cmds As New CommandlineParameterCollection()
    ''' cmds.Add(cmd1)
    ''' 
    ''' ' See the "Logic.Parsers" module for the argument parser implementation.
    ''' Parsers.ParseArguments(cmds,
    '''                        AddressOf ErrorCallbacks.OnSyntaxError,
    '''                        AddressOf ErrorCallbacks.OnMissingParameterRequired,
    '''                        AddressOf ErrorCallbacks.OnDuplicatedParameter,
    '''                        AddressOf ErrorCallbacks.OnUnknownArgument)
    ''' </code>
    ''' </example>
    ''' ----------------------------------------------------------------------------------------------------
    <ImmutableObject(False)>
    Public NotInheritable Class CommandlineParameter : Inherits CommandlineParameter(Of Object)

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="CommandlineParameter"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Sub New()
        End Sub

#End Region

#Region " Hidden Base Members "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Determines whether the specified <see cref="Object"/> is equal to this instance.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="obj">
        ''' Another object to compare to.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' <see langword="True"/> if the specified <see cref="Object"/> is equal to this instance; otherwise, <see langword="False"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <EditorBrowsable(EditorBrowsableState.Never)>
        <DebuggerNonUserCode>
        Public Shadows Function Equals(ByVal obj As Object) As Boolean
            Return MyBase.Equals(obj)
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Determines whether the specified <see cref="Global.System.Object"/> instances are the same instance.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="objA">
        ''' The first object to compare.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="objB">
        ''' The second object to compare.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' <see langword="True"/> if <paramref name="objA"/> is the same instance as <paramref name="objB"/> 
        ''' or if both are <see langword="Nothing"/>; otherwise, <see langword="False"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <EditorBrowsable(EditorBrowsableState.Never)>
        <DebuggerNonUserCode>
        Public Shadows Function ReferenceEquals(ByVal objA As Object, ByVal objB As Object) As Boolean
            Return Object.ReferenceEquals(objA, objB)
        End Function

#End Region

    End Class

#End Region

End Namespace
