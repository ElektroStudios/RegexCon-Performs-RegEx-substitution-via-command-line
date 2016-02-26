' ***********************************************************************
' Author   : Elektro
' Modified : 26-February-2016
' ***********************************************************************

#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Text.RegularExpressions

Imports RegexCon.Logic
Imports RegexCon.Tools
Imports RegexCon.Types

#End Region

Namespace UserInterface

    ' RegExCon.exe /Input="Black Coast - Trndsttr (Feat. M. Maggie)" /Expression="^(?<artist>.+?(?=\s*-\s*))\s*-\s*(?<title>.+?(?=(?:\(?(ft|feat|featuring)\.)|$))\(?(?<feat>(ft|feat|featuring)\.[^()\n]+)?\)?(?<subtitle>.+)?$" /Substitution="${artist} ${feat} - ${title} ${subtitle}" /MatchCase=False

    Public Module Main

        Sub Main()

            Console.Title = HelpSection.Help.<Title>.Value

            params = New CommandlineParameterCollection
            With params
                .Add(Parameters.ParamInput)
                .Add(Parameters.ParamExpression)
                .Add(Parameters.ParamSubstitution)
                .Add(Parameters.ParamMatchCase)
            End With

            Parsers.ParseArguments(params, AddressOf ErrorCallbacks.OnSyntaxError, AddressOf ErrorCallbacks.OnMissingParameterRequired)

            If (Validate) AndAlso Not RegExUtil.ValidateExpression(Fields.Expression) Then
                ErrorCallbacks.OnRegExValidationFailed(Fields.Expression)

            Else
                Fields.Rgx = New Regex(Fields.Expression, If(Fields.MatchCase, RegexOptions.None, RegexOptions.IgnoreCase))

                Console.WriteLine(Rgx.Replace(Fields.Input, Fields.Substitution))
                Environment.Exit(exitCode:=0)

            End If

        End Sub

    End Module

End Namespace
