' ***********************************************************************
' Author   : ElektroStudios
' Modified : 26-February-2016
' Usage    : Use *F##* as the ForeColor beginning delimiter, use *-F* to restore the console forecolor.
'            Use *B##* as the BackColor beginning delimiter, use *-B* to restore the console BackColor.
' ***********************************************************************

#Region " ConsoleColor Enumeration Helper "

' Black = 0
' DarkBlue = 1
' DarkGreen = 2
' DarkCyan = 3
' DarkRed = 4
' DarkMagenta = 5
' DarkYellow = 6
' Gray = 7
' DarkGray = 8
' Blue = 9
' Green = 10
' Cyan = 11
' Red = 12
' Magenta = 13
' Yellow = 14
' White = 15

#End Region

#Region " Option Statements "

Option Explicit On
Option Strict On
Option Infer Off

#End Region

#Region " Imports "

Imports RegexCon.Tools
Imports System.Text

#End Region

#Region " Help Section "

Namespace UserInterface

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Manages the Help documentation of a console application, with support for colorization capabilities.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class HelpSection

#Region " Private Fields "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the name of the current process.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private Shared ReadOnly processName As String =
            Process.GetCurrentProcess.MainModule.ModuleName

        ' ''' ----------------------------------------------------------------------------------------------------
        ' ''' <summary>
        ' ''' Use this var into an Xml if need to escape a 'GreaterThan' character.
        ' ''' </summary>
        ' ''' ----------------------------------------------------------------------------------------------------
        'Private ReadOnly greaterThanChar As Char = ">"c

        ' ''' ----------------------------------------------------------------------------------------------------
        ' ''' <summary>
        ' ''' Use this var into an Xml if need to escape a 'LowerThan' character.
        ' ''' </summary>
        ' ''' ----------------------------------------------------------------------------------------------------
        'Private ReadOnly lowerThanChar As Char = "<"c

#End Region

#Region " Help Xml "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Contains help information such as author name, application syntax and example usages.
        ''' <para></para>
        ''' These strings are color-delimited to print a colorized output console,
        ''' using the <c>WriteColorText</c> methods written by ElektroStudios.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared ReadOnly Help As XElement =
            <Help>
                <!-- Application title -->
                <Title>RegExCon .:: By ElektroStudios ::.</Title>

                <!-- Application name -->
                <Name>*F7*RegExCon*-F*</Name>

                <!-- Application author -->
                <Author>*f7*ElektroStudios*-F*</Author>

                <!-- Application version -->
                <Version>*F7*1.1*-F*</Version>

                <!-- Copyright information -->
                <Copyright>*F7*Copyright © ElektroStudios 2016-2024*-F*</Copyright>

                <!-- Website information -->
                <Website>*F7*https://github.com/ElektroStudios/RegexCon-Performs-RegEx-substitution-via-command-line*-F*</Website>

                <!-- Application Logotype -->
                <Logo>
  *F10* .----------------.                        .----------------.              
  *F10*| .--------------. |                      | .--------------. |             
  *F10*| |  _______     | |                      | |     ______   | |             
  *F10*| | |_   __ \    | |                      | |   .' ___  |  | |             
  *F10*| |   | |__) |   | |                      *F10*| |  / .'   \_|  | |             
  *F10*| |   |  __ /    | |*F15*  ___  __ _  _____  _ *F10*| |  | |         | |*F15*  ___  _ __  
  *F10*| |  _| |  \ \_  | |*F15*// _ \/ _` |/ _ \ \/ /*F10*| |  \ `.___.'\  | |*F15* / _ \| '_ \ 
  *F10*| | |____| |___| | |*F15*\  __/ (_| |  __/&gt;  &lt; *F10*| |   `._____.'  | |*F15*| (_) | | | |
  *F10*| |              | |*F15* \___|\__, |\___/_/\_\*F10*| |              | |*F15* \___/|_| |_|
  *F10*| '--------------' |*F15*      __/ |           *F10*| '--------------' |             
  *F10* '----------------' *F15*     |___/            *F10* '----------------'              
*-F*</Logo>

                <!-- Application Logotype NOT colored -->
                <Logo>
 .----------------.                        .----------------.              
| .--------------. |                      | .--------------. |             
| |  _______     | |                      | |     ______   | |             
| | |_   __ \    | |                      | |   .' ___  |  | |             
| |   | |__) |   | |                      | |  / .'   \_|  | |             
| |   |  __ /    | |  ___  __ _  _____  _ | |  | |         | |  ___  _ __  
| |  _| |  \ \_  | |// _ \/ _` |/ _ \ \/ /| |  \ `.___.'\  | | / _ \| '_ \ 
| | |____| |___| | |\  __/ (_| |  __/&gt;  &lt; | |   `._____.'  | || (_) | | | |
| |              | | \___|\__, |\___/_/\_\| |              | | \___/|_| |_|
| '--------------' |      __/ |           | '--------------' |             
 '----------------'      |___/             '----------------'                          
</Logo>


                <!-- Separator shape -->
                <Separator>
*F10*------------------------------------------------------>>>>*-F*</Separator>

                <!-- Application Syntax -->
                <Syntax>
*F11*[+]*-F* *F7*Syntax*-F*
*F11*---------------------------------------------------------*-F*
<%= processName %> *F10*/I=*F7*(input)*-F* *F10*/E=*F7*(expression)*-F* *F10*/S=*F7*(substitution)*-F*</Syntax>

                <!-- Application Syntax (Additional Specifications) -->
                <SyntaxExtra>
*F11*[+]*-F* *F7*Switches*-F*
*F11*---------------------------------------------------------*-F*
*F10*/Input        *F7*or *F10*/I*-F* *F3*| *F7*The input string.*-F*
*F10*/Expression   *F7*or *F10*/E*-F* *F3*| *F7*The regular expression.*-F*
*F10*/Substitution *F7*or *F10*/S*-F* *F3*| *F7*The substitution string.*-F*
*F10*/MatchCase    *F7*or *F10*/M*-F* *F3*| *F7*Matches characters in a case-sensitive manner.*-F*
*F10*/?                *F3*  | *F7*Shows this help.*-F*

*F11*[+]*-F* *F7*Switch value types*-F*
*F11*---------------------------------------------------------*-F*
*F10*/Input        *F7*or *F10*/I*-F* *F3*| *F7*Any string*-F*
*F10*/Expression   *F7*or *F10*/E*-F* *F3*| *F7*Any string*-F*
*F10*/Substitution *F7*or *F10*/S*-F* *F3*| *F7*Any string*-F*
*F10*/MatchCase    *F7*or *F10*/M*-F* *F3*| *F7*True*F3*/*F7*False*-F*
</SyntaxExtra>

                <!-- Application Usage Examples -->
                <UsageExamples>
*F11*[+]*-F* *F7*Usage examples*-F*
*F11*---------------------------------------------------------*-F*
*F7*(Regular string substitution)*-F*
/I="Hello World" /E="^(.+)\s(.+)$" /S="$2 $1" /M=True

*F7*(Named-group string substitution)*-F*
/I="Hello World" /E="^(?&lt;one&gt;.+)\s(?&lt;two&gt;.+)$" /S="${two} ${one}"
</UsageExamples>

            </Help>

#End Region

#Region " Public Methods "

        Friend Shared Sub PrintHelp()

            Dim sb As New StringBuilder
            With sb
                .AppendLine(HelpSection.Help.<Logo>.Value)
                .AppendLine(HelpSection.Help.<Separator>.Value)
                .AppendLine(String.Format("*F7*Name......: {0}", HelpSection.Help.<Name>.Value))
                .AppendLine(String.Format("*F7*Version...: {0}", HelpSection.Help.<Version>.Value))
                .AppendLine(String.Format("*F7*Author....: {0}", HelpSection.Help.<Author>.Value))
                .AppendLine(String.Format("*F7*Copyright.: {0}", HelpSection.Help.<Copyright>.Value))
                .AppendLine(String.Format("*F7*Website...: {0}", HelpSection.Help.<Website>.Value))
                .AppendLine(HelpSection.Help.<Separator>.Value)
                .AppendLine(HelpSection.Help.<Syntax>.Value)
                .AppendLine(HelpSection.Help.<SyntaxExtra>.Value)
                .AppendLine(HelpSection.Help.<UsageExamples>.Value)
            End With

            CliUtil.WriteColorText(sb.ToString, {"*"c})
            Environment.Exit(exitCode:=0)

        End Sub

#End Region

    End Class

End Namespace

#End Region
