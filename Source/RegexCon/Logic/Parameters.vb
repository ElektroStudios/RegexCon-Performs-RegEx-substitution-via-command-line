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

Imports RegexCon.Types

#End Region

#Region " Parameters "

Namespace Logic

    Public Module Parameters

        Public ReadOnly ParamInput As New CommandlineParameter(Of String) With {
            .Name = "/Input",
            .ShortName = "/I",
            .Separator = "="c,
            .DefaultValue = "",
            .IsOptional = False
        }

        Public ReadOnly ParamExpression As New CommandlineParameter(Of String) With {
            .Name = "/Expression",
            .ShortName = "/E",
            .Separator = "="c,
            .DefaultValue = "",
            .IsOptional = False
        }

        Public ReadOnly ParamSubstitution As New CommandlineParameter(Of String) With {
            .Name = "/Substitution",
            .ShortName = "/S",
            .Separator = "="c,
            .DefaultValue = "",
            .IsOptional = False
        }

        Public ReadOnly ParamMatchCase As New CommandlineParameter(Of Boolean) With {
            .Name = "/MatchCase",
            .ShortName = "/M",
            .Separator = "="c,
            .DefaultValue = True,
            .IsOptional = True
        }

    End Module

End Namespace

#End Region
