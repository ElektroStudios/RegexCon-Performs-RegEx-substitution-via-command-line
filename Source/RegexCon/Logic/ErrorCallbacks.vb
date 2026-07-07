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

#Region " Error Callbacks "

Namespace Logic

    Friend Module ErrorCallbacks

        Friend Sub OnSyntaxError(ByVal cmd As CommandlineParameter)

            Console.WriteLine(String.Format("[X] Syntax error in parameter: {0} (or {1})", cmd.Name, cmd.ShortName))
            Environment.Exit(exitCode:=1)

        End Sub

        Friend Sub OnMissingParameterRequired(ByVal cmd As CommandlineParameter)

            Console.WriteLine(String.Format("[X] Parameter {0} (or {1}) is required. ", cmd.Name, cmd.ShortName))
            Environment.Exit(exitCode:=1)

        End Sub

        Friend Sub OnRegExValidationFailed(ByVal expr As String)

            Console.WriteLine(String.Format("[X] Expression validation has failed: ""{0}""", expr))
            Environment.Exit(exitCode:=1)

        End Sub

    End Module

End Namespace

#End Region
