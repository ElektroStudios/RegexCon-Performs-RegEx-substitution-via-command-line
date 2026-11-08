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
' Value As T
' DefaultValue As T

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

#Region " Commandline Parameter (Of T) "

    ''' <summary>
    ''' Represents a Commandline Parameter.
    ''' </summary>
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
    ''' <typeparam name="T">
    ''' The type of value that the parameter takes.
    ''' </typeparam>
    <ImmutableObject(False)>
    Public Class CommandlineParameter(Of T)

#Region " Properties "

        ''' <summary>
        ''' Gets or sets a value indicating whether this parameter is required for the application.
        ''' <para></para>
        ''' A value of <see langword="False"/> means the user needs to assign a value for this parameter.
        ''' <para></para>
        ''' A value of <see langword="True"/> means this is an optional parameter so no matter if the user sets a custom value.
        ''' </summary>
        ''' <value>
        ''' <see langword="False"/> if this parameter is required for the application; otherwise, <see langword="True"/>.
        ''' </value>
        Public Property IsOptional As Boolean

        ''' <summary>
        ''' Gets or sets the parameter name.
        ''' </summary>
        ''' <value>
        ''' The parameter name.
        ''' </value>
        Public Property Name As String
            <DebuggerStepThrough>
            Get
                Return Me.nameB
            End Get
            <DebuggerStepThrough>
            Set(ByVal value As String)
                Me.EvaluateName(value)
            End Set
        End Property
        ''' <summary>
        ''' ( Backing Field )
        ''' The parameter name.
        ''' </summary>
        Private nameB As String

        ''' <summary>
        ''' Gets or sets the parameter shortname.
        ''' </summary>
        ''' <value>
        ''' The parameter shortname.
        ''' </value>
        Public Property ShortName As String
            <DebuggerStepThrough>
            Get
                Return Me.shortNameB
            End Get
            <DebuggerStepThrough>
            Set(ByVal value As String)
                Me.EvaluateShortName(value)
            End Set
        End Property
        ''' <summary>
        ''' ( Backing Field )
        ''' The parameter shortname.
        ''' </summary>
        Private shortNameB As String

        ''' <summary>
        ''' Gets or sets the parameter separator.
        ''' <para></para>
        ''' This character separates the parameter from the value in the argument.
        ''' </summary>
        ''' <value>
        ''' The parameter separator.
        ''' </value>
        Public Property Separator As Char
            <DebuggerStepThrough>
            Get
                Return Me.separatorB
            End Get
            <DebuggerStepThrough>
            Set(ByVal value As Char)
                Me.EvaluateSeparator(value)
            End Set
        End Property
        ''' <summary>
        ''' ( Backing Field )
        ''' The parameter separator.
        ''' </summary>
        Private separatorB As Char

        ''' <summary>
        ''' Gets or sets the parameter value.
        ''' <para></para>
        ''' This value should be initially <see langword="Nothing"/> before parsing the commandline arguments of the application;
        ''' <para></para>
        ''' the value of the parameter should be assigned by the end-user when passing an argument to the application.
        ''' <para></para>
        ''' To set a default value for this parameter, use <see cref="CommandlineParameter(Of T).DefaultValue"/> property instead.
        ''' </summary>
        ''' <value>
        ''' The parameter value.
        ''' </value>
        Public Property Value As T

        ''' <summary>
        ''' Gets or sets the default parameter value.
        ''' <para></para>
        ''' This value should be take into account if, after parsing the commandline arguments of the application,
        ''' <see cref="CommandlineParameter(Of T).Value"/> is <see langword="Nothing"/>,
        ''' meaning that the end-user didn't assigned any custom value to this parameter.
        ''' </summary>
        ''' <value>
        ''' The default parameter value.
        ''' </value>
        Public Property DefaultValue As T

#End Region

#Region " Operator Overloading "

        ''' <summary>
        ''' Performs an implicit conversion from <see cref="CommandlineParameter(Of T)"/> to <see cref="CommandlineParameter"/>.
        ''' </summary>
        ''' <param name="cmd">
        ''' The <see cref="CommandlineParameter(Of T)"/> to convert.
        ''' </param>
        ''' <returns>
        ''' The result of the conversion.
        ''' </returns>
        <DebuggerStepThrough>
        Public Shared Widening Operator CType(ByVal cmd As CommandlineParameter(Of T)) As CommandlineParameter

            Return New CommandlineParameter With
                   {
                        .Name = cmd.Name,
                        .ShortName = cmd.ShortName,
                        .Separator = cmd.Separator,
                        .DefaultValue = cmd.DefaultValue,
                        .Value = cmd.Value,
                        .IsOptional = cmd.IsOptional
                    }

        End Operator

#End Region

#Region " Private Methods "

        ''' <summary>
        ''' Evaluates an attempt to assign the parameter name.
        ''' </summary>
        ''' <param name="name">
        ''' The parameter name.
        ''' </param>
        ''' <exception cref="ArgumentNullException">
        ''' The parameter name cannot be null.;name
        ''' </exception>
        '''
        ''' <exception cref="ArgumentException">
        ''' The parameter name cannot contain the separator character.;name
        ''' </exception>
        ''' 
        ''' <exception cref="ArgumentException">
        ''' The parameter name cannot be equals than the parameter shortname.;name
        ''' </exception>
        <DebuggerStepThrough>
        Protected Overridable Sub EvaluateName(ByVal name As String)

            If name Is Nothing Then
                Throw New ArgumentNullException(paramName:="name", message:="The parameter name cannot be null.")

            ElseIf Not (Me.separatorB.Equals(Nothing)) AndAlso (name.Contains(Me.separatorB)) Then
                Throw New ArgumentException(message:="The parameter name cannot contain the separator character.",
                                            paramName:="name")

            ElseIf (name.Equals(Me.shortNameB)) Then
                Throw New ArgumentException(message:="The parameter name cannot be equals than the parameter shortname.",
                                            paramName:="name")

            Else
                Me.nameB = name

            End If

        End Sub

        ''' <summary>
        ''' Evaluates an attempt to assign the parameter shortname.
        ''' </summary>
        ''' <param name="shortName">
        ''' The parameter shortname.
        ''' </param>
        ''' <exception cref="ArgumentException">
        ''' The parameter name cannot contain the separator character.;shortname
        ''' </exception>
        ''' 
        ''' <exception cref="ArgumentException">
        ''' The parameter shortname cannot be equals than the parameter name.;shortname
        ''' </exception>
        <DebuggerStepThrough>
        Protected Overridable Sub EvaluateShortName(ByVal shortname As String)

            ' The shortname is optional, so a null or empty shortname is always accepted.
            If String.IsNullOrEmpty(shortname) Then
                Me.shortNameB = shortname

            ElseIf Not (Me.separatorB.Equals(Nothing)) AndAlso (shortname.Contains(Me.separatorB)) Then
                Throw New ArgumentException(message:="The parameter shortname cannot contain the separator character.",
                                            paramName:="shortname")

            ElseIf (shortname.Equals(Me.nameB)) Then
                Throw New ArgumentException(message:="The parameter shortname cannot be equals than the parameter name.",
                                            paramName:="shortname")

            Else
                Me.shortNameB = shortname

            End If

        End Sub

        ''' <summary>
        ''' Evaluates an attempt to assign the parameter separator.
        ''' </summary>
        ''' <param name="separator">
        ''' The parameter separator.
        ''' </param>
        ''' <exception cref="ArgumentException">
        ''' The parameter separator cannot be any character contained in the parameter name.;separator
        ''' </exception>
        '''
        ''' <exception cref="ArgumentException">
        ''' The parameter separator cannot be any character contained in the parameter shortname.;separator
        ''' </exception>
        <DebuggerStepThrough>
        Protected Overridable Sub EvaluateSeparator(ByVal separator As Char)

            If Not (String.IsNullOrEmpty(Me.nameB)) AndAlso (Me.nameB.Contains(separator)) Then
                Throw New ArgumentException(message:="The parameter separator cannot be any character contained in the parameter name.",
                                            paramName:="separator")

            ElseIf Not (String.IsNullOrEmpty(Me.shortNameB)) AndAlso (Me.shortNameB.Contains(separator)) Then
                Throw New ArgumentException(message:="The parameter separator cannot be any character contained in the parameter shortname.",
                                            paramName:="separator")

            Else
                Me.separatorB = separator

            End If

        End Sub

#End Region

#Region " Constructors "

        ''' <summary>
        ''' Initializes a new instance of the <see cref="CommandlineParameter(Of T)"/> class.
        ''' </summary>
        <DebuggerStepThrough>
        Public Sub New()
        End Sub

#End Region

#Region " Hidden Base Members "

        ''' <summary>
        ''' Determines whether the specified <see cref="Object"/> is equal to this instance.
        ''' </summary>
        ''' <param name="obj">
        ''' Another object to compare to.
        ''' </param>
        ''' <returns>
        ''' <see langword="True"/> if the specified <see cref="Object"/> is equal to this instance; otherwise, <see langword="False"/>.
        ''' </returns>
        <EditorBrowsable(EditorBrowsableState.Never)>
        <DebuggerNonUserCode>
        Public Shadows Function Equals(ByVal obj As Object) As Boolean
            Return MyBase.Equals(obj)
        End Function

        ''' <summary>
        ''' Determines whether the specified <see cref="Global.System.Object"/> instances are the same instance.
        ''' </summary>
        ''' <param name="objA">
        ''' The first object to compare.
        ''' </param>
        ''' <param name="objB">
        ''' The second object to compare.
        ''' </param>
        ''' <returns>
        ''' <see langword="True"/> if <paramref name="objA"/> is the same instance as <paramref name="objB"/> 
        ''' or if both are <see langword="Nothing"/>; otherwise, <see langword="False"/>.
        ''' </returns>
        <EditorBrowsable(EditorBrowsableState.Never)>
        <DebuggerNonUserCode>
        Public Shadows Function ReferenceEquals(ByVal objA As Object, ByVal objB As Object) As Boolean
            Return Object.ReferenceEquals(objA, objB)
        End Function

#End Region

    End Class

#End Region

End Namespace
