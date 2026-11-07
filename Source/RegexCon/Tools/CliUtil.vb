' ***********************************************************************
' Author   : Elektro
' Modified : 07-November-2026
' ***********************************************************************

#Region " Public Members Summary "

#Region " Methods "

' CliUtil.WriteColorText(String, Char())
' CliUtil.WriteColorText(String, ConsoleColor?, ConsoleColor?)
' CliUtil.WriteColorTextLine(String, Char())
' CliUtil.WriteColorTextLine(String, ConsoleColor?, ConsoleColor?)

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

#End Region

#Region " CLI Util "

Namespace Tools

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Contains Console User-Interface related utilities.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class CliUtil

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Prevents a default instance of the <see cref="CliUtil"/> class from being created.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerNonUserCode>
        Private Sub New()
        End Sub

#End Region

#Region " Hidden Base Members "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Determines whether the specified <see cref="System.Object"/> instances are considered equal.
        ''' </summary>
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
        <EditorBrowsable(EditorBrowsableState.Never)>
        <DebuggerNonUserCode>
        Public Shadows Function ReferenceEquals(ByVal objA As Object, ByVal objB As Object) As Boolean
            Return Object.ReferenceEquals(objA, objB)
        End Function

#End Region

#Region " Public Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Writes colored text on the Console.
        ''' <para></para>
        ''' Use <c>*F##*</c> as the start delimiter of the ForeColor, use <c>*-F*</c> as the end delimiter of the ForeColor.
        ''' <para></para>
        ''' Use <c>*B##*</c> as the start delimiter of the BackColor, use <c>*-B*</c> as the end delimiter of the BackColor.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <example> This is a code example.
        ''' <code>
        ''' WriteColorText("*F10*Hello *F14*World!*-F*", {"*"c})
        ''' </code>
        ''' </example>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="text">
        ''' The color-delimited text to write.
        ''' </param>
        ''' 
        ''' <param name="delimiters">
        ''' A set of 1 or 2 delimiters to parse the color-delimited string.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Shared Sub WriteColorText(ByVal text As String,
                                         ByVal delimiters As Char())

            If text Is Nothing Then
                Throw New ArgumentNullException(paramName:=NameOf(text))
            End If

            If delimiters Is Nothing Then
                Throw New ArgumentNullException(paramName:=NameOf(delimiters))
            End If

            If (delimiters.Length < 1) OrElse (delimiters.Length > 2) Then
                Throw New ArgumentException(message:="A set of 1 or 2 delimiters is required.",
                                            paramName:=NameOf(delimiters))
            End If

            ' Save the current console colors to later restore them.
            Dim oldForedColor As ConsoleColor = Console.ForegroundColor
            Dim oldBackColor As ConsoleColor = Console.BackgroundColor

            ' Split the string to retrieve and parse the color-delimited strings.
            Dim stringParts As String() =
                text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries)

            ' Parse the string parts.
            For Each part As String In stringParts

                If (part.ToUpper Like "F#") OrElse (part.ToUpper Like "F##") Then
                    ' Use the new ForeColor.
                    Console.ForegroundColor = DirectCast(CInt(part.Substring(1)), ConsoleColor)

                ElseIf (part.ToUpper Like "B#") OrElse (part.ToUpper Like "B##") Then
                    ' Use the new BackgroundColor.
                    Console.BackgroundColor = DirectCast(CInt(part.Substring(1)), ConsoleColor)

                ElseIf (part.ToUpper Like "-F") Then
                    ' Use the saved Forecolor.
                    Console.ForegroundColor = oldForedColor

                ElseIf (part.ToUpper Like "-B") Then
                    ' Use the saved BackgroundColor.
                    Console.BackgroundColor = oldBackColor

                Else ' String part is not a delimiter so we can print it.
                    Console.Write(part)

                End If

            Next part

            ' Restore the saved console colors.
            Console.ForegroundColor = oldForedColor
            Console.BackgroundColor = oldBackColor

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Writes colored text on the Console.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <example> This is a code example.
        ''' <code>
        ''' WriteColorText(" Hello World! ", ConsoleColor.Blue, ConsoleColor.Blue)
        ''' WriteColorText(" Hello World! ", ConsoleColor.Blue, Nothing)
        ''' </code>
        ''' </example>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="text">
        ''' The text to write.
        ''' </param>
        ''' 
        ''' <param name="foreColor">
        ''' The text color, or <see langword="Nothing"/> to keep the current text color.
        ''' </param>
        ''' 
        ''' <param name="backColor">
        ''' The background color, or <see langword="Nothing"/> to keep the current background color.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Shared Sub WriteColorText(ByVal text As String,
                                         ByVal foreColor As ConsoleColor?,
                                         ByVal backColor As ConsoleColor?)

            If text Is Nothing Then
                Throw New ArgumentNullException(paramName:=NameOf(text))
            End If

            ' Save the current console colors to later restore them.
            Dim oldForeColor As ConsoleColor = Console.ForegroundColor
            Dim oldBackColor As ConsoleColor = Console.BackgroundColor

            ' Set the new console colors ("Nothing" keeps the current color).
            Console.ForegroundColor = foreColor.GetValueOrDefault(oldForeColor)
            Console.BackgroundColor = backColor.GetValueOrDefault(oldBackColor)

            ' Print the text.
            Console.Write(text)

            ' Restore the saved console colors.
            Console.ForegroundColor = oldForeColor
            Console.BackgroundColor = oldBackColor

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Writes colored text on the Console and adds an empty line at the end.
        ''' <para></para>
        ''' Use <c>*F##*</c> as the start delimiter of the ForeColor, use <c>*-F*</c> as the end delimiter of the ForeColor.
        ''' <para></para>
        ''' Use <c>*B##*</c> as the start delimiter of the BackColor, use <c>*-B*</c> as the end delimiter of the BackColor.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <example> This is a code example.
        ''' <code>
        ''' WriteColorTextLine("{B15}{F12} Hello World! {-F}{-B}", {"{"c, "}"c})
        ''' WriteColorTextLine($"*B15**F12* {"Hello"} *F0*{"World!"} *-F**-B*", {"*"c})
        ''' </code>
        ''' </example>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="text">
        ''' The color-delimited text to write.
        ''' </param>
        ''' 
        ''' <param name="delimiters">
        ''' A set of 1 or 2 delimiters to parse the color-delimited string.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Shared Sub WriteColorTextLine(ByVal text As String,
                                             ByVal delimiters As Char())

            If text Is Nothing Then
                Throw New ArgumentNullException(paramName:=NameOf(text))
            End If

            CliUtil.WriteColorText($"{text}{Environment.NewLine}", delimiters)

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Writes colored text on the Console and adds an empty line at the end.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <example> This is a code example.
        ''' <code>
        ''' WriteColorTextLine(" Hello World! ", ConsoleColor.Magenta, ConsoleColor.Gray)
        ''' WriteColorTextLine(" Hello World! ", ConsoleColor.Magenta, Nothing)
        ''' </code>
        ''' </example>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="text">
        ''' The text to write.
        ''' </param>
        ''' 
        ''' <param name="foreColor">
        ''' The text color, or <see langword="Nothing"/> to keep the current text color.
        ''' </param>
        ''' 
        ''' <param name="backColor">
        ''' The background color, or <see langword="Nothing"/> to keep the current background color.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Shared Sub WriteColorTextLine(ByVal text As String,
                                             ByVal foreColor As ConsoleColor?,
                                             ByVal backColor As ConsoleColor?)

            If text Is Nothing Then
                Throw New ArgumentNullException(paramName:=NameOf(text))
            End If

            CliUtil.WriteColorText($"{text}{Environment.NewLine}", foreColor, backColor)

        End Sub

#End Region

    End Class

End Namespace

#End Region
