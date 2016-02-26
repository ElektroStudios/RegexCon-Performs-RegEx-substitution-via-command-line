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

Imports RegexCon.Types

#End Region

#Region " Fields "

Namespace Logic

    Public Module Fields

        Public Params As CommandlineParameterCollection
        Public Rgx As Regex
        Public ReadOnly Validate As Boolean = True

        Public Input As String
        Public Expression As String
        Public Substitution As String
        Public MatchCase As Boolean

        Public Sub SetFields()

            Fields.Input = CStr(Params("/Input").Value)
            Fields.Expression = CStr(Params("/Expression").Value)
            Fields.Substitution = CStr(Params("/Substitution").Value)
            Fields.MatchCase = CBool(Params("/MatchCase").Value)

        End Sub

    End Module

End Namespace

#End Region
