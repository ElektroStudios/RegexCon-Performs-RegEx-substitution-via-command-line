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

#Region " CommandlineParameterCollection Tests "

''' ----------------------------------------------------------------------------------------------------
''' <summary>
''' Unit tests for <see cref="CommandlineParameterCollection"/> type.
''' </summary>
''' ----------------------------------------------------------------------------------------------------
<TestClass>
Public NotInheritable Class CommandlineParameterCollectionTests

#Region " Add "

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that distinct parameters are added in order.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub Add_DistinctParameters_AddsInOrder()

        Dim params As New CommandlineParameterCollection()
        Dim first As CommandlineParameter = TestHelpers.CreateParameter("/Input", "/I", "", isOptional:=False)
        Dim second As CommandlineParameter = TestHelpers.CreateParameter("/Expression", "/E", "", isOptional:=False)

        params.Add(first)
        params.Add(second)

        Assert.HasCount(2, params)
        Assert.AreSame(first, params(0))
        Assert.AreSame(second, params(1))

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that a parameter with a duplicated name is rejected (case-insensitive).
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub Add_DuplicatedName_ThrowsArgumentException()

        Dim params As New CommandlineParameterCollection()
        params.Add(TestHelpers.CreateParameter("/Input", "/I", "", isOptional:=False))

        Dim duplicated As CommandlineParameter = TestHelpers.CreateParameter("/INPUT", "/X", "", isOptional:=False)
        Dim ex As ArgumentException =
            Assert.ThrowsExactly(Of ArgumentException)(Sub() params.Add(duplicated))

        Assert.AreEqual("param", ex.ParamName)
        Assert.HasCount(1, params)

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that a parameter with a duplicated short name is rejected (case-insensitive).
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub Add_DuplicatedShortName_ThrowsArgumentException()

        Dim params As New CommandlineParameterCollection()
        params.Add(TestHelpers.CreateParameter("/Input", "/I", "", isOptional:=False))

        Dim duplicated As CommandlineParameter = TestHelpers.CreateParameter("/Other", "/i", "", isOptional:=False)
        Assert.ThrowsExactly(Of ArgumentException)(Sub() params.Add(duplicated))

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that a parameter whose name equals the short name of an existing parameter is rejected.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub Add_NameEqualsExistingShortName_ThrowsArgumentException()

        Dim params As New CommandlineParameterCollection()
        params.Add(TestHelpers.CreateParameter("/Input", "/I", "", isOptional:=False))

        Dim conflicting As CommandlineParameter = TestHelpers.CreateParameter("/I", "/Z", "", isOptional:=False)
        Assert.ThrowsExactly(Of ArgumentException)(Sub() params.Add(conflicting))

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that adding <see langword="Nothing"/> throws <see cref="ArgumentNullException"/>.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub Add_Nothing_ThrowsArgumentNullException()

        Dim params As New CommandlineParameterCollection()
        Dim param As CommandlineParameter = Nothing

        Dim ex As ArgumentNullException =
            Assert.ThrowsExactly(Of ArgumentNullException)(Sub() params.Add(param))

        Assert.AreEqual("param", ex.ParamName)

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that several parameters without short name can be added and found.
    ''' <para></para>
    ''' Regression test: before v1.2 this threw a <see cref="NullReferenceException"/>.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub Add_ParametersWithoutShortName_AreAddedAndFound()

        Dim params As New CommandlineParameterCollection()
        Dim first As CommandlineParameter = TestHelpers.CreateParameter("/Switch1", Nothing, "", isOptional:=False)
        Dim second As CommandlineParameter = TestHelpers.CreateParameter("/Switch2", Nothing, "", isOptional:=True)

        params.Add(first)
        params.Add(second)

        Assert.HasCount(2, params)
        Assert.AreSame(first, params.Find("/switch1"))
        Assert.AreSame(second, params.Find("/SWITCH2"))
        Assert.IsTrue(params.Contains("/Switch2"))
        Assert.AreEqual(1, params.IndexOf("/Switch2"))

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that an empty or <see langword="Nothing"/> key does not match parameters without short name.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub Contains_EmptyKey_DoesNotMatchParametersWithoutShortName()

        Dim params As New CommandlineParameterCollection()
        params.Add(TestHelpers.CreateParameter("/Switch1", Nothing, "", isOptional:=False))

        Dim nullName As String = Nothing

        Assert.IsFalse(params.Contains(nullName))
        Assert.IsFalse(params.Contains(String.Empty))
        Assert.IsNull(params.Find(nullName))
        Assert.AreEqual(-1, params.IndexOf(String.Empty))

    End Sub

#End Region

#Region " AddRange "

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that <see cref="CommandlineParameterCollection.AddRange"/> adds all the parameters.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub AddRange_DistinctParameters_AddsAll()

        Dim params As New CommandlineParameterCollection()
        Dim items As CommandlineParameter() = {
            TestHelpers.CreateParameter("/Input", "/I", "", isOptional:=False),
            TestHelpers.CreateParameter("/Expression", "/E", "", isOptional:=False),
            TestHelpers.CreateParameter("/Substitution", "/S", "", isOptional:=False)
        }

        params.AddRange(items)

        Assert.HasCount(3, params)
        Assert.AreSame(items(2), params(2))

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that <see cref="CommandlineParameterCollection.AddRange"/> rejects duplicated parameters.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub AddRange_DuplicatedParameters_ThrowsArgumentException()

        Dim params As New CommandlineParameterCollection()
        Dim items As CommandlineParameter() = {
            TestHelpers.CreateParameter("/Input", "/I", "", isOptional:=False),
            TestHelpers.CreateParameter("/Input", "/X", "", isOptional:=False)
        }

        Assert.ThrowsExactly(Of ArgumentException)(Sub() params.AddRange(items))
        Assert.HasCount(1, params)

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that <see cref="CommandlineParameterCollection.AddRange"/> rejects <see langword="Nothing"/>.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub AddRange_Nothing_ThrowsArgumentNullException()

        Dim params As New CommandlineParameterCollection()
        Dim items As CommandlineParameter() = Nothing

        Assert.ThrowsExactly(Of ArgumentNullException)(Sub() params.AddRange(items))

    End Sub

#End Region

#Region " Find / Contains / IndexOf / Indexer "

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that parameters are found by name and by short name, case-insensitive.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub Find_ByNameOrShortName_IsCaseInsensitive()

        Dim params As CommandlineParameterCollection = TestHelpers.CreateApplicationParameters()

        Assert.AreEqual("/Input", params.Find("/input").Name)
        Assert.AreEqual("/Input", params.Find("/i").Name)
        Assert.AreEqual("/Expression", params.Find("/E").Name)
        Assert.AreEqual("/MatchCase", params("/m").Name)

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that searching an unknown parameter returns <see langword="Nothing"/>,
    ''' <see langword="False"/> and <c>-1</c> respectively.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub Search_UnknownParameter_ReturnsNotFound()

        Dim params As CommandlineParameterCollection = TestHelpers.CreateApplicationParameters()

        Assert.IsNull(params.Find("/Unknown"))
        Assert.IsNull(params("/Unknown"))
        Assert.IsFalse(params.Contains("/Unknown"))
        Assert.AreEqual(-1, params.IndexOf("/Unknown"))

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that <see cref="CommandlineParameterCollection.IndexOf(String)"/> returns the zero-based index.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub IndexOf_ExistingParameter_ReturnsZeroBasedIndex()

        Dim params As CommandlineParameterCollection = TestHelpers.CreateApplicationParameters()

        Assert.AreEqual(0, params.IndexOf("/Input"))
        Assert.AreEqual(1, params.IndexOf("/e"))
        Assert.AreEqual(2, params.IndexOf("/Substitution"))
        Assert.AreEqual(3, params.IndexOf("/M"))

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that the string indexer setter replaces the matching parameter.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub Indexer_SetByName_ReplacesParameter()

        Dim params As CommandlineParameterCollection = TestHelpers.CreateApplicationParameters()
        Dim replacement As CommandlineParameter = TestHelpers.CreateParameter("/Input", "/I", "new", isOptional:=True)

        params("/I") = replacement

        Assert.AreSame(replacement, params("/Input"))
        Assert.HasCount(4, params)

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that the string indexer setter fails for an unknown parameter.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub Indexer_SetUnknownName_ThrowsArgumentOutOfRangeException()

        Dim params As CommandlineParameterCollection = TestHelpers.CreateApplicationParameters()
        Dim replacement As CommandlineParameter = TestHelpers.CreateParameter("/Other", "/O", "", isOptional:=True)

        Assert.ThrowsExactly(Of ArgumentOutOfRangeException)(Sub() params("/Unknown") = replacement)

    End Sub

#End Region

#Region " Remove "

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that a parameter can be removed by its name or short name.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub Remove_ByName_RemovesParameter()

        Dim params As CommandlineParameterCollection = TestHelpers.CreateApplicationParameters()

        params.Remove("/input")
        params.Remove("/E")

        Assert.HasCount(2, params)
        Assert.IsFalse(params.Contains("/Input"))
        Assert.IsFalse(params.Contains("/Expression"))

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that a parameter can be removed by its instance.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub Remove_ByInstance_RemovesParameter()

        Dim params As CommandlineParameterCollection = TestHelpers.CreateApplicationParameters()
        Dim param As CommandlineParameter = params("/MatchCase")

        params.Remove(param)

        Assert.HasCount(3, params)
        Assert.IsNull(params("/MatchCase"))

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Verifies that removing an unknown parameter throws <see cref="ArgumentException"/>.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestMethod>
    Public Sub Remove_UnknownParameter_ThrowsArgumentException()

        Dim params As CommandlineParameterCollection = TestHelpers.CreateApplicationParameters()
        Dim foreign As CommandlineParameter = TestHelpers.CreateParameter("/Input", "/I", "", isOptional:=False)

        Dim exByName As ArgumentException =
            Assert.ThrowsExactly(Of ArgumentException)(Sub() params.Remove("/Unknown"))
        Dim exByInstance As ArgumentException =
            Assert.ThrowsExactly(Of ArgumentException)(Sub() params.Remove(foreign))

        Assert.AreEqual("name", exByName.ParamName)
        Assert.AreEqual("param", exByInstance.ParamName)
        Assert.HasCount(4, params)

    End Sub

#End Region

End Class

#End Region
