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
' Value As Object
' DefaultValue As Object

#End Region

#End Region

#Region " Usage Examples "

' Module Module1
' 
'     Private ReadOnly cmd1 As New CommandlineParameter With
'     {
'         .Name = "/Switch1",
'         .Separator = "="c,
'         .DefaultValue = "Hello World",
'         .IsOptional = False
'     }
' 
'     Private ReadOnly cmd2 As New CommandlineParameter With
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

#Region " Commandline Parameter "

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
    '''     Private ReadOnly cmd2 As New CommandlineParameter With
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
