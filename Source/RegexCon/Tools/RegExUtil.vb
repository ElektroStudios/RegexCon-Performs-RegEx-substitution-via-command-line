' ***********************************************************************
' Author   : Elektro
' Modified : 07-November-2026
' ***********************************************************************

#Region " Public Members Summary "

#Region " Functions "

' Substitute(String, String, String, Boolean) As String
' ValidateExpression(String) As Boolean

#End Region

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
Imports System.Text.RegularExpressions

#End Region

#Region " RegEx Util "

Namespace Tools

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Contains RegEx related utilities.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <ImmutableObject(True)>
    Public NotInheritable Class RegExUtil

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Prevents a default instance of the <see cref="RegExUtil"/> class from being created.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerNonUserCode>
        Private Sub New()
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
        ''' Determines whether the specified <see cref="System.Object"/> instances are the same instance.
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

#Region " Public Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Replaces all the strings that match the specified regular expression with the specified substitution string.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <example> This is a code example.
        ''' <code>
        ''' Dim result As String = Substitute("Hello World", "^(.+)\s(.+)$", "$2 $1", matchCase:=True) ' "World Hello"
        ''' </code>
        ''' </example>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="input">
        ''' The input string.
        ''' </param>
        '''
        ''' <param name="pattern">
        ''' The regular expression pattern to match.
        ''' </param>
        '''
        ''' <param name="replacement">
        ''' The substitution string.
        ''' </param>
        '''
        ''' <param name="matchCase">
        ''' If <see langword="True"/>, matches characters in a case-sensitive manner; 
        ''' otherwise, uses <see cref="RegexOptions.IgnoreCase"/>.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The resulting string after performing the substitution.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <exception cref="ArgumentNullException">
        ''' <paramref name="input"/>, <paramref name="pattern"/> or <paramref name="replacement"/> is <see langword="Nothing"/>.
        ''' </exception>
        '''
        ''' <exception cref="ArgumentException">
        ''' <paramref name="pattern"/> is not a valid regular expression.
        ''' </exception>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Shared Function Substitute(ByVal input As String,
                                          ByVal pattern As String,
                                          ByVal replacement As String,
                                          ByVal matchCase As Boolean) As String

            If input Is Nothing Then
                Throw New ArgumentNullException(paramName:=NameOf(input))
            End If

            If pattern Is Nothing Then
                Throw New ArgumentNullException(paramName:=NameOf(pattern))
            End If

            If replacement Is Nothing Then
                Throw New ArgumentNullException(paramName:=NameOf(replacement))
            End If

            Dim options As RegexOptions = If(matchCase, RegexOptions.None, RegexOptions.IgnoreCase)
            Dim rgx As New Regex(pattern, options)
            Return rgx.Replace(input, replacement)

        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Validates the specified regular expression.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <example> This is a code example.
        ''' <code>
        ''' Dim success As Boolean = ValidateExpression("[0-9]++") ' False
        ''' </code>
        ''' </example>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="expr">
        ''' The regular expression.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' <see langword="True"/> if pattern validation success, <see langword="False"/> otherwise.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Shared Function ValidateExpression(ByVal expr As String) As Boolean

            Try
                Dim test As New Regex(pattern:=expr)
                Return True

            Catch ex As ArgumentException ' Invalid pattern (or ArgumentNullException for a null pattern).
                Return False

            End Try

        End Function

#End Region

    End Class

End Namespace

#End Region
