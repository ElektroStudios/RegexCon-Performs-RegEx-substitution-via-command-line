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

            Parsers.ParseArguments(params,
                                   AddressOf ErrorCallbacks.OnSyntaxError,
                                   AddressOf ErrorCallbacks.OnMissingParameterRequired,
                                   AddressOf ErrorCallbacks.OnDuplicatedParameter,
                                   AddressOf ErrorCallbacks.OnUnknownArgument)

            If String.IsNullOrEmpty(Fields.Expression) Then
                ErrorCallbacks.OnEmptyExpression()

            ElseIf (Validate) AndAlso Not RegExUtil.ValidateExpression(Fields.Expression) Then
                ErrorCallbacks.OnRegExValidationFailed(Fields.Expression)

            Else
                Console.WriteLine(RegExUtil.Substitute(Fields.Input, Fields.Expression, Fields.Substitution, Fields.MatchCase))
                Environment.Exit(exitCode:=0)

            End If

        End Sub

    End Module

End Namespace
