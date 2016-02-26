' ***********************************************************************
' Author   : Elektro
' Modified : 26-February-2016
' ***********************************************************************

#Region " Public Members Summary "

#Region " Constructors "

' New()

#End Region

#Region " Indexers "

' Item(String) As CommandLineParameter

#End Region

#Region " Functions "

' Contains(String) As Boolean
' Find(String) As IniSection
' IndexOf(String) As Integer

#End Region

#Region " Methods "

'  Add(CommandLineParameter)
'  Add(String, String, Opt: String)
'  AddRange(CommandLineParameter())
'  Remove(CommandLineParameter)
'  Remove(String)

#End Region

#End Region

#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System
Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Linq
Imports System.Xml.Serialization

#End Region

#Region " CommandlineParameter Collection "

Namespace Types

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Represents a strongly typed list of <see cref="CommandlineParameter"/> that can be accessed by an index.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <Serializable>
    <XmlRoot("Items")>
    <ImmutableObject(True)>
    Public NotInheritable Class CommandlineParameterCollection : Inherits Collection(Of CommandlineParameter)

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="CommandlineParameterCollection"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Public Sub New()
        End Sub

#End Region

#Region " Indexers "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the <see cref="CommandlineParameter"/> that matches the specified key name.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="paramName">
        ''' The parameter name.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' The <see cref="CommandLineParameter"/>.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        Default Public Overloads Property Item(ByVal paramName As String) As CommandlineParameter
            <DebuggerStepThrough>
            Get
                Return Me.Find(paramName)
            End Get
            <DebuggerStepThrough>
            Set(ByVal value As CommandlineParameter)
                Me(Me.IndexOf(paramName)) = value
            End Set
        End Property

#End Region

#Region " Public Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Adds a <see cref="CommandlineParameter"/> to the end of the <see cref="CommandlineParameterCollection"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="param">
        ''' The parameter to add.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <exception cref="ArgumentException">
        ''' Parameter already exists.;param
        ''' </exception>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Shadows Sub Add(ByRef param As CommandlineParameter)

            If Me.Contains(param.Name) OrElse Me.Contains(param.ShortName) Then
                Throw New ArgumentException(message:="Parameter already exists.", paramName:="param")

            Else
                MyBase.Add(param)

            End If

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Adds the specified parameters to the end of the <see cref="CommandlineParameterCollection"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="params">
        ''' The parameters to add.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <exception cref="ArgumentException">
        ''' Parameter already exists.;param
        ''' </exception>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Shadows Sub AddRange(ByRef params As CommandlineParameter())

            For Each param As CommandlineParameter In params
                Me.Add(param)
            Next param

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Removes a <see cref="CommandlineParameter"/> from the <see cref="CommandlineParameterCollection"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="param">
        ''' The parameter.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <exception cref="ArgumentException">
        ''' Parameter doesn't exists.;name
        ''' </exception>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Shadows Sub Remove(ByVal param As CommandlineParameter)

            Dim indexOf As Integer = Me.IndexOf(param)

            If (indexOf = -1) Then
                Throw New ArgumentException(message:="Parameter doesn't exists.", paramName:="param")

            Else
                MyBase.RemoveAt(indexOf)

            End If

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Removes a <see cref="CommandlineParameter"/> from the <see cref="CommandlineParameterCollection"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="name">
        ''' The name of the parameter.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <exception cref="ArgumentException">
        ''' Parameter doesn't exists.;name
        ''' </exception>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Shadows Sub Remove(ByVal name As String)

            Dim indexOf As Integer = Me.IndexOf(name)

            If (indexOf = -1) Then
                Throw New ArgumentException(message:="Parameter doesn't exists.", paramName:="name")

            Else
                MyBase.RemoveAt(indexOf)

            End If

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Determines whether the <see cref="CommandlineParameterCollection"/> contains a <see cref="CommandlineParameter"/> that 
        ''' matches the specified key name.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="name">
        ''' The name of the parameter.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' <see langword="True"/> if the <see cref="CommandlineParameterCollection"/> contains the <see cref="CommandlineParameter"/>, 
        ''' <see langword="False"/> otherwise.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Overloads Function Contains(ByVal name As String) As Boolean

            Return (From param As CommandlineParameter In MyBase.Items
                    Where param.Name.Equals(name, StringComparison.OrdinalIgnoreCase) OrElse
                          param.ShortName.Equals(name, StringComparison.OrdinalIgnoreCase)).Any

        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Searches for an <see cref="CommandlineParameter"/> that matches the specified parameter name, 
        ''' and returns the first occurrence within the entire <see cref="CommandlineParameterCollection"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="name">
        ''' The name of the parameter.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' <see cref="CommandlineParameter"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Overloads Function Find(ByVal name As String) As CommandlineParameter

            Return (From param As CommandlineParameter In MyBase.Items
                    Where param.Name.Equals(name, StringComparison.OrdinalIgnoreCase) OrElse
                          param.ShortName.Equals(name, StringComparison.OrdinalIgnoreCase)).
                    DefaultIfEmpty(Nothing).
                    SingleOrDefault

        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Searches for an <see cref="CommandlineParameter"/> that matches the specified key name and 
        ''' returns the zero-based index of the first occurrence within the entire <see cref="CommandlineParameterCollection"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="name">
        ''' The name of the parameter.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The zero-based index of the first occurrence of <see cref="CommandlineParameter"/> within the entire <see cref="CommandlineParameterCollection"/>, if found; 
        ''' otherwise, <c>–1</c>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Overloads Function IndexOf(ByVal name As String) As Integer

            Dim index As Integer = 0
            Dim found As Boolean = False

            For Each param As CommandlineParameter In MyBase.Items

                If param.Name.Equals(name, StringComparison.OrdinalIgnoreCase) OrElse
                   param.ShortName.Equals(name, StringComparison.OrdinalIgnoreCase) Then
                    found = True
                    Exit For
                End If

                index += 1

            Next param

            If (found) Then
                Return index

            Else
                Return -1

            End If

        End Function

#End Region

#Region " Hidden Base Members "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Serves as a hash function for a particular type.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <EditorBrowsable(EditorBrowsableState.Never)>
        <DebuggerNonUserCode>
        Public Shadows Function GetHashCode() As Integer
            Return MyBase.GetHashCode
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the <see cref="System.Type"/> of the current instance.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The exact runtime type of the current instance.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <EditorBrowsable(EditorBrowsableState.Never)>
        <DebuggerNonUserCode>
        Public Shadows Function [GetType]() As Type
            Return MyBase.GetType
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Returns a String that represents the current object.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' A string that represents the current object.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <EditorBrowsable(EditorBrowsableState.Never)>
        <DebuggerNonUserCode>
        Public Shadows Function ToString() As String
            Return MyBase.ToString
        End Function

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

    End Class

End Namespace

#End Region
