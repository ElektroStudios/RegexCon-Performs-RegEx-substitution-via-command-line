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

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports RegexCon.Types

#End Region

#Region " CommandlineParameter Tests "

''' ----------------------------------------------------------------------------------------------------
''' <summary>
''' Unit tests for <see cref="CommandlineParameter(Of T)"/> and <see cref="CommandlineParameter"/> types.
''' </summary>
''' ----------------------------------------------------------------------------------------------------
<TestClass>
Public NotInheritable Class CommandlineParameterTests

#Region " Properties "

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that all the properties keep the assigned values.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub Properties_AssignedValues_AreReturned()

        Dim param As New CommandlineParameter(Of Integer) With {
            .Name = "/Number",
            .ShortName = "/N",
            .Separator = ":"c,
            .DefaultValue = 10,
            .Value = 20,
            .IsOptional = True
        }

        Assert.AreEqual("/Number", param.Name)
        Assert.AreEqual("/N", param.ShortName)
        Assert.AreEqual(":"c, param.Separator)
        Assert.AreEqual(10, param.DefaultValue)
        Assert.AreEqual(20, param.Value)
        Assert.IsTrue(param.IsOptional)

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies the default state of a new instance.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub Constructor_NewInstance_HasDefaultState()

        Dim param As New CommandlineParameter(Of Boolean)()

        Assert.IsNull(param.Name)
        Assert.IsNull(param.ShortName)
        Assert.AreEqual(ChrW(0), param.Separator)
        Assert.IsFalse(param.Value)
        Assert.IsFalse(param.DefaultValue)
        Assert.IsFalse(param.IsOptional)

    End Sub

#End Region

#Region " Name / ShortName Validation "

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that a name that contains the separator character is rejected.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub Name_ContainsSeparator_ThrowsArgumentException()

        Dim param As New CommandlineParameter(Of String) With {.Separator = "="c}

        Dim ex As ArgumentException =
            Assert.ThrowsExactly(Of ArgumentException)(Sub() param.Name = "/In=put")

        Assert.AreEqual("name", ex.ParamName)
        Assert.IsNull(param.Name)

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that a short name that contains the separator character is rejected.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub ShortName_ContainsSeparator_ThrowsArgumentException()

        Dim param As New CommandlineParameter(Of String) With {.Separator = "="c}

        Dim ex As ArgumentException =
            Assert.ThrowsExactly(Of ArgumentException)(Sub() param.ShortName = "/I=")

        Assert.AreEqual("shortname", ex.ParamName)
        Assert.IsNull(param.ShortName)

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that a name equal to the current short name is rejected.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub Name_EqualsShortName_ThrowsArgumentException()

        Dim param As New CommandlineParameter(Of String) With {.ShortName = "/I"}

        Dim ex As ArgumentException =
            Assert.ThrowsExactly(Of ArgumentException)(Sub() param.Name = "/I")

        Assert.AreEqual("name", ex.ParamName)

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that a short name equal to the current name is rejected.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub ShortName_EqualsName_ThrowsArgumentException()

        Dim param As New CommandlineParameter(Of String) With {.Name = "/Input"}

        Dim ex As ArgumentException =
            Assert.ThrowsExactly(Of ArgumentException)(Sub() param.ShortName = "/Input")

        Assert.AreEqual("shortname", ex.ParamName)

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that the name can contain any character when the separator was not assigned yet.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub Name_SeparatorNotAssigned_AcceptsAnyCharacter()

        Dim param As New CommandlineParameter(Of String) With {.Name = "/A=B"}

        Assert.AreEqual("/A=B", param.Name)

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that a <see langword="Nothing"/> name is rejected with <see cref="ArgumentNullException"/>.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub Name_Nothing_ThrowsArgumentNullException()

        Dim param As New CommandlineParameter(Of String) With {.Separator = "="c}

        Dim ex As ArgumentNullException =
            Assert.ThrowsExactly(Of ArgumentNullException)(Sub() param.Name = Nothing)

        Assert.AreEqual("name", ex.ParamName)

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that the short name is optional, so <see langword="Nothing"/> and empty values are accepted.
    ''' <para></para>
    ''' Regression test: before v1.2, assigning a <see langword="Nothing"/> short name
    ''' threw a <see cref="NullReferenceException"/>.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub ShortName_NothingOrEmpty_IsAccepted()

        Dim param As New CommandlineParameter(Of String) With {
            .Name = "/Input",
            .Separator = "="c,
            .ShortName = "/I"
        }

        param.ShortName = Nothing
        Assert.IsNull(param.ShortName)

        param.ShortName = String.Empty
        Assert.AreEqual(String.Empty, param.ShortName)

    End Sub

#End Region

#Region " Separator Validation "

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that a separator contained in the parameter name is rejected.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub Separator_ContainedInName_ThrowsArgumentException()

        Dim param As New CommandlineParameter(Of String) With {.Name = "/Input"}

        Dim ex As ArgumentException =
            Assert.ThrowsExactly(Of ArgumentException)(Sub() param.Separator = "p"c)

        Assert.AreEqual("separator", ex.ParamName)
        Assert.AreEqual(ChrW(0), param.Separator)

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that a separator contained in the parameter short name is rejected.
    ''' <para></para>
    ''' Regression test: before v1.2 only the name was validated,
    ''' so a short name containing the separator was silently accepted.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub Separator_ContainedInShortName_ThrowsArgumentException()

        Dim param As New CommandlineParameter(Of String) With {
            .Name = "/Input",
            .ShortName = "/I:"
        }

        Dim ex As ArgumentException =
            Assert.ThrowsExactly(Of ArgumentException)(Sub() param.Separator = ":"c)

        Assert.AreEqual("separator", ex.ParamName)
        Assert.AreEqual(ChrW(0), param.Separator)

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that a valid separator is accepted.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub Separator_NotContainedInNames_IsAccepted()

        Dim param As New CommandlineParameter(Of String) With {
            .Name = "/Input",
            .ShortName = "/I",
            .Separator = "="c
        }

        Assert.AreEqual("="c, param.Separator)

    End Sub

#End Region

#Region " Widening Conversion "

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that the widening conversion to <see cref="CommandlineParameter"/> copies every property.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub WideningConversion_CopiesAllProperties()

        Dim typed As New CommandlineParameter(Of Boolean) With {
            .Name = "/MatchCase",
            .ShortName = "/M",
            .Separator = "="c,
            .DefaultValue = True,
            .Value = False,
            .IsOptional = True
        }

        Dim untyped As CommandlineParameter = typed

        Assert.AreEqual(typed.Name, untyped.Name)
        Assert.AreEqual(typed.ShortName, untyped.ShortName)
        Assert.AreEqual(typed.Separator, untyped.Separator)
        Assert.AreEqual(True, untyped.DefaultValue)
        Assert.AreEqual(False, untyped.Value)
        Assert.AreEqual(typed.IsOptional, untyped.IsOptional)
        Assert.IsInstanceOfType(Of Boolean)(untyped.DefaultValue)

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that a parameter without short name (as in the documented usage example) can be converted.
    ''' <para></para>
    ''' Regression test: before v1.2 this conversion threw a <see cref="NullReferenceException"/>.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub WideningConversion_WithoutShortName_Succeeds()

        Dim typed As New CommandlineParameter(Of String) With {
            .Name = "/Switch1",
            .Separator = "="c,
            .DefaultValue = "Hello World",
            .IsOptional = False
        }

        Dim params As CommandlineParameter() = {typed}

        Assert.AreEqual("/Switch1", params(0).Name)
        Assert.IsNull(params(0).ShortName)
        Assert.AreEqual("Hello World", params(0).DefaultValue)

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that the widening conversion creates a new independent instance every time.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub WideningConversion_CreatesIndependentInstances()

        Dim typed As New CommandlineParameter(Of String) With {
            .Name = "/Input",
            .Separator = "="c,
            .DefaultValue = "default"
        }

        Dim first As CommandlineParameter = typed
        Dim second As CommandlineParameter = typed
        first.Value = "changed"

        Assert.AreNotSame(first, second)
        Assert.IsNull(second.Value)
        Assert.IsNull(typed.Value)

    End Sub

#End Region

End Class

#End Region
