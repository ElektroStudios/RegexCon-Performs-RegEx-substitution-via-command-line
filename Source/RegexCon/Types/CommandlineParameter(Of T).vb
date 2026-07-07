' ***********************************************************************
' Author   : Elektro
' Modified : 26-February-2016
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

' Module Module1
' 
'     Private ReadOnly cmd1 As New CommandlineParameter(Of String) With
'     {
'         .Name = "/Switch1",
'         .Separator = "="c,
'         .DefaultValue = "Hello World",
'         .IsOptional = False
'     }
' 
'     Private ReadOnly cmd2 As New CommandlineParameter(Of Boolean) With
'     {
'         .Name = "/Switch2",
'         .Separator = "="c,
'         .DefaultValue = False,
'         .IsOptional = True
'     }
' 
'     Dim cmds As CommandlineParameter() = {cmd1, cmd2}
' 
'     Sub Main()
' 
'         ParseArguments(cmds, AddressOf Cmd_OnSyntaxError, AddressOf Cmd_OnMissingParameterRequired)
' 
'     End Sub
' 
'     ' ----------------------------------------------------------------------------------------------------
'     ' <summary>
'     ' Loop through all the command-line arguments of this application.
'     ' </summary>
'     ' ----------------------------------------------------------------------------------------------------
'     ' <param name="cmds">
'     ' The commandline parameters.
'     ' </param>
'     ' 
'     ' <param name="callbackSyntaxError">
'     ' An encapsulated method that is invoked if a syntax error is found in one of the arguments.
'     ' </param>
'     ' 
'     ' <param name="callbackMissingRequired">
'     ' An encapsulated method that is invoked if a required parameter is missing in the arguments.
'     ' </param>
'     ' ----------------------------------------------------------------------------------------------------
'     Private Sub ParseArguments(ByRef cmds As CommandlineParameter(),
'                                ByVal callbackSyntaxError As Action(Of CommandlineParameter),
'                                ByVal callbackMissingRequired As Action(Of CommandlineParameter))
' 
'         ParseArguments(cmds, Environment.GetCommandLineArgs.Skip(1), callbackSyntaxError, callbackMissingRequired)
' 
'     End Sub
' 
'     ' ----------------------------------------------------------------------------------------------------
'     ' <summary>
'     ' Loop through all the command-line arguments of this application.
'     ' </summary>
'     ' ----------------------------------------------------------------------------------------------------
'     ' <param name="cmds">
'     ' The commandline parameters.
'     ' </param>
'     ' 
'     ' <param name="args">
'     ' The collection of commandline arguments to examine.
'     ' </param>
'     ' 
'     ' <param name="callbackSyntaxError">
'     ' An encapsulated method that is invoked if a syntax error is found in one of the arguments.
'     ' </param>
'     ' 
'     ' <param name="callbackMissingRequired">
'     ' An encapsulated method that is invoked if a required parameter is missing in the arguments.
'     ' </param>
'     ' ----------------------------------------------------------------------------------------------------
'     Private Sub ParseArguments(ByRef cmds As CommandlineParameter(),
'                                ByVal args As IEnumerable(Of String),
'                                ByVal callbackSyntaxError As Action(Of CommandlineParameter),
'                                ByVal callbackMissingRequired As Action(Of CommandlineParameter))
' 
'         Dim cmdRequired As List(Of CommandlineParameter) =
'             (From cmd As CommandlineParameter In cmds
'              Where Not cmd.IsOptional).ToList
' 
'         For Each arg As String In args
' 
'             For Each cmd As CommandlineParameter In cmds
' 
'                 If arg.StartsWith(cmd.Name, StringComparison.OrdinalIgnoreCase) Then
' 
'                     If Not arg.Contains(cmd.Separator) Then
'                         callbackSyntaxError.Invoke(cmd)
'                         Exit Sub
' 
'                     Else
'                         Dim value As String = arg.Substring(arg.IndexOf(cmd.Separator) + 1)
' 
'                         If (cmdRequired.Contains(cmd)) Then
'                             cmdRequired.Remove(cmd)
'                         End If
' 
'                         If String.IsNullOrEmpty(value) Then
'                             cmd.Value = cmd.DefaultValue
'                             Continue For
' 
'                         Else
'                             Try
'                                 cmd.Value = Convert.ChangeType(value, cmd.DefaultValue.GetType())
'                                 Continue For
' 
'                             Catch ex As Exception
'                                 callbackSyntaxError.Invoke(cmd)
'                                 Exit Sub
' 
'                             End Try
' 
'                         End If
' 
'                     End If
' 
'                 End If
' 
'             Next cmd
' 
'         Next arg
' 
'         If (cmdRequired.Any) Then
'             callbackMissingRequired.Invoke(cmdRequired.First)
'         End If
' 
'     End Sub
' 
'     Private Sub Cmd_OnSyntaxError(ByVal cmd As CommandlineParameter)
' 
'         Console.WriteLine(String.Format("[X] Syntax error in parameter: {0}", cmd.Name))
'         Environment.Exit(exitCode:=1)
' 
'     End Sub
' 
'     Private Sub Cmd_OnMissingParameterRequired(ByVal cmd As CommandlineParameter)
' 
'         Console.WriteLine(String.Format("[X] Parameter required: {0}", cmd.Name))
'         Environment.Exit(exitCode:=1)
' 
'     End Sub
' 
' End Module

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

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Represents a Commandline Parameter.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <example> This is a code example.
    ''' <code>
    ''' Module Module1
    ''' 
    '''     Private ReadOnly cmd1 As New CommandlineParameter(Of String) With
    '''     {
    '''         .Name = "/Switch1",
    '''         .Separator = "="c,
    '''         .DefaultValue = "Hello World",
    '''         .IsOptional = False
    '''     }
    ''' 
    '''     Private ReadOnly cmd2 As New CommandlineParameter(Of Boolean) With
    '''     {
    '''         .Name = "/Switch2",
    '''         .Separator = "="c,
    '''         .DefaultValue = False,
    '''         .IsOptional = True
    '''     }
    ''' 
    '''     Dim cmds As CommandlineParameter() = {cmd1, cmd2}
    ''' 
    '''     Sub Main()
    ''' 
    '''         ParseArguments(cmds, AddressOf Cmd_OnSyntaxError, AddressOf Cmd_OnMissingParameterRequired)
    ''' 
    '''     End Sub
    ''' 
    '''     ''' ----------------------------------------------------------------------------------------------------
    '''     ''' &lt;summary&gt;
    '''     ''' Loop through all the command-line arguments of this application.
    '''     ''' &lt;/summary&gt;
    '''     ''' ----------------------------------------------------------------------------------------------------
    '''     ''' &lt;param name="cmds"&gt;
    '''     ''' The commandline parameters.
    '''     ''' &lt;/param>
    '''     ''' 
    '''     ''' &lt;param name="callbackSyntaxError"&gt;
    '''     ''' An encapsulated method that is invoked if a syntax error is found in one of the arguments.
    '''     ''' &lt;/param&gt;
    '''     ''' 
    '''     ''' &lt;param name="callbackMissingRequired"&gt;
    '''     ''' An encapsulated method that is invoked if a required parameter is missing in the arguments.
    '''     ''' &lt;/param&gt;
    '''     ''' ----------------------------------------------------------------------------------------------------
    '''     Private Sub ParseArguments(ByRef cmds As CommandlineParameter(),
    '''                                ByVal callbackSyntaxError As Action(Of CommandlineParameter),
    '''                                ByVal callbackMissingRequired As Action(Of CommandlineParameter))
    ''' 
    '''         ParseArguments(cmds, Environment.GetCommandLineArgs.Skip(1), callbackSyntaxError, callbackMissingRequired)
    ''' 
    '''     End Sub
    ''' 
    '''     ''' ----------------------------------------------------------------------------------------------------
    '''     ''' &lt;summary&gt;
    '''     ''' Loop through all the command-line arguments of this application.
    '''     ''' &lt;/summary&gt;
    '''     ''' ----------------------------------------------------------------------------------------------------
    '''     ''' &lt;param name="cmds"&gt;
    '''     ''' The commandline parameters.
    '''     ''' &lt;/param&gt;
    '''     ''' 
    '''     ''' &lt;param name="args"&gt;
    '''     ''' The collection of commandline arguments to examine.
    '''     ''' &lt;/param&gt;
    '''     ''' 
    '''     ''' &lt;param name="callbackSyntaxError"&gt;
    '''     ''' An encapsulated method that is invoked if a syntax error is found in one of the arguments.
    '''     ''' &lt;/param&gt;
    '''     ''' 
    '''     ''' &lt;param name="callbackMissingRequired"&gt;
    '''     ''' An encapsulated method that is invoked if a required parameter is missing in the arguments.
    '''     ''' &lt;/param&gt;
    '''     ''' ----------------------------------------------------------------------------------------------------
    '''     Private Sub ParseArguments(ByRef cmds As CommandlineParameter(),
    '''                                ByVal args As IEnumerable(Of String),
    '''                                ByVal callbackSyntaxError As Action(Of CommandlineParameter),
    '''                                ByVal callbackMissingRequired As Action(Of CommandlineParameter))
    ''' 
    '''         Dim cmdRequired As List(Of CommandlineParameter) =
    '''             (From cmd As CommandlineParameter In cmds
    '''              Where Not cmd.IsOptional).ToList
    ''' 
    '''         For Each arg As String In args
    ''' 
    '''             For Each cmd As CommandlineParameter In cmds
    ''' 
    '''                 If arg.StartsWith(cmd.Name, StringComparison.OrdinalIgnoreCase) Then
    ''' 
    '''                     If Not arg.Contains(cmd.Separator) Then
    '''                         callbackSyntaxError.Invoke(cmd)
    '''                         Exit Sub
    ''' 
    '''                     Else
    '''                         Dim value As String = arg.Substring(arg.IndexOf(cmd.Separator) + 1)
    ''' 
    '''                         If (cmdRequired.Contains(cmd)) Then
    '''                             cmdRequired.Remove(cmd)
    '''                         End If
    ''' 
    '''                         If String.IsNullOrEmpty(value) Then
    '''                             cmd.Value = cmd.DefaultValue
    '''                             Continue For
    ''' 
    '''                         Else
    '''                             Try
    '''                                 cmd.Value = Convert.ChangeType(value, cmd.DefaultValue.GetType())
    '''                                 Continue For
    ''' 
    '''                             Catch ex As Exception
    '''                                 callbackSyntaxError.Invoke(cmd)
    '''                                 Exit Sub
    ''' 
    '''                             End Try
    ''' 
    '''                         End If
    ''' 
    '''                     End If
    ''' 
    '''                 End If
    ''' 
    '''             Next cmd
    ''' 
    '''         Next arg
    ''' 
    '''         If (cmdRequired.Any) Then
    '''             callbackMissingRequired.Invoke(cmdRequired.First)
    '''         End If
    ''' 
    '''     End Sub
    ''' 
    '''     Private Sub Cmd_OnSyntaxError(ByVal cmd As CommandlineParameter)
    ''' 
    '''         Console.WriteLine(String.Format("[X] Syntax error in parameter: {0}", cmd.Name))
    '''         Environment.Exit(exitCode:=1)
    ''' 
    '''     End Sub
    ''' 
    '''     Private Sub Cmd_OnMissingParameterRequired(ByVal cmd As CommandlineParameter)
    ''' 
    '''         Console.WriteLine(String.Format("[X] Parameter required: {0}", cmd.Name))
    '''         Environment.Exit(exitCode:=1)
    ''' 
    '''     End Sub
    ''' 
    ''' End Module
    ''' </code>
    ''' </example>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <typeparam name="T">
    ''' The type of value that the parameter takes.
    ''' </typeparam>
    ''' ----------------------------------------------------------------------------------------------------
    <ImmutableObject(False)>
    Public Class CommandlineParameter(Of T)

#Region " Properties "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a value indicating whether this parameter is required for the application.
        ''' <para></para>
        ''' A value of <see langword="False"/> means the user needs to assign a value for this parameter.
        ''' <para></para>
        ''' A value of <see langword="True"/> means this is an optional parameter so no matter if the user sets a custom value.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' <see langword="False"/> if this parameter is required for the application; otherwise, <see langword="True"/>.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        Public Property IsOptional As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the parameter name.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' The parameter name.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
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
        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' ( Backing Field )
        ''' The parameter name.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private nameB As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the parameter shortname.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' The parameter shortname.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
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
        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' ( Backing Field )
        ''' The parameter shortname.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private shortNameB As String

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the parameter separator.
        ''' <para></para>
        ''' This character separates the parameter from the value in the argument.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' The parameter separator.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
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
        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' ( Backing Field )
        ''' The parameter separator.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private separatorB As Char

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the parameter value.
        ''' <para></para>
        ''' This value should be initially <see langword="Nothing"/> before parsing the commandline arguments of the application;
        ''' <para></para>
        ''' the value of the parameter should be assigned by the end-user when passing an argument to the application.
        ''' <para></para>
        ''' To set a default value for this parameter, use <see cref="CommandlineParameter(Of T).DefaultValue"/> property instead.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' The parameter value.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        Public Property Value As T

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the default parameter value.
        ''' <para></para>
        ''' This value should be take into account if, after parsing the commandline arguments of the application,
        ''' <see cref="CommandlineParameter(Of T).Value"/> is <see langword="Nothing"/>,
        ''' meaning that the end-user didn't assigned any custom value to this parameter.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' The default parameter value.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        Public Property DefaultValue As T

#End Region

#Region " Operator Overloading "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Performs an implicit conversion from <see cref="CommandlineParameter(Of T)"/> to <see cref="CommandlineParameter"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="cmd">
        ''' The <see cref="CommandlineParameter(Of T)"/> to convert.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The result of the conversion.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
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

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Evaluates an attempt to assign the parameter name.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="name">
        ''' The parameter name.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <exception cref="ArgumentException">
        ''' The parameter name cannot contain the separator character.;name
        ''' </exception>
        ''' 
        ''' <exception cref="ArgumentException">
        ''' The parameter name cannot be equals than the parameter shortname.;name
        ''' </exception>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Protected Overridable Sub EvaluateName(ByVal name As String)

            If Not (Me.separatorB.Equals(Nothing)) AndAlso (name.Contains(Me.separatorB)) Then
                Throw New ArgumentException(message:="The parameter name cannot contain the separator character.",
                                            paramName:="name")

            ElseIf (name.Equals(Me.shortNameB)) Then
                Throw New ArgumentException(message:="The parameter name cannot be equals than the parameter shortname.",
                                            paramName:="name")

            Else
                Me.nameB = name

            End If

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Evaluates an attempt to assign the parameter shortname.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="shortName">
        ''' The parameter shortname.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <exception cref="ArgumentException">
        ''' The parameter name cannot contain the separator character.;shortname
        ''' </exception>
        ''' 
        ''' <exception cref="ArgumentException">
        ''' The parameter shortname cannot be equals than the parameter name.;shortname
        ''' </exception>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Protected Overridable Sub EvaluateShortName(ByVal shortname As String)

            If Not (Me.separatorB.Equals(Nothing)) AndAlso (shortname.Contains(Me.separatorB)) Then
                Throw New ArgumentException(message:="The parameter shortname cannot contain the separator character.",
                                            paramName:="shortname")

            ElseIf (shortname.Equals(Me.nameB)) Then
                Throw New ArgumentException(message:="The parameter shortname cannot be equals than the parameter name.",
                                            paramName:="shortname")

            Else
                Me.shortNameB = shortname

            End If

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Evaluates an attempt to assign the parameter separator.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="separator">
        ''' The parameter separator.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <exception cref="ArgumentException">
        ''' The parameter separator cannot be any character contained in the parameter name.;separator
        ''' </exception>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Protected Overridable Sub EvaluateSeparator(ByVal separator As Char)

            If Not (String.IsNullOrEmpty(Me.nameB)) AndAlso (Me.nameB.Contains(separator)) Then
                Throw New ArgumentException(message:="The parameter separator cannot be any character contained in the parameter name.",
                                            paramName:="separator")

            Else
                Me.separatorB = separator

            End If

        End Sub

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="CommandlineParameter(Of T)"/> class.
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
