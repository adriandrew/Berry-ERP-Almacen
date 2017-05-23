﻿Imports System.Data.SqlClient

Public Class UnidadesMedidas

    Private id As Integer
    Private nombre As String

    Public Property EId() As Integer
        Get
            Return id
        End Get
        Set(value As Integer)
            id = value
        End Set
    End Property
    Public Property ENombre() As String
        Get
            Return nombre
        End Get
        Set(value As String)
            nombre = value
        End Set
    End Property

    Public Sub Guardar()

        Try
            Dim comando As New SqlCommand()
            comando.Connection = BaseDatos.conexionCatalogo
            comando.CommandText = "INSERT INTO " & LogicaEntradas.Programas.prefijoBaseDatosAlmacen & "UnidadesMedidas (Id, Nombre) VALUES (@id, @nombre)"
            comando.Parameters.AddWithValue("@id", Me.EId)
            comando.Parameters.AddWithValue("@nombre", Me.ENombre)
            BaseDatos.conexionCatalogo.Open()
            comando.ExecuteNonQuery()
            BaseDatos.conexionCatalogo.Close()
        Catch ex As Exception
            Throw ex
        Finally
            BaseDatos.conexionCatalogo.Close()
        End Try

    End Sub

    Public Sub Eliminar()

        Try
            Dim comando As New SqlCommand()
            comando.Connection = BaseDatos.conexionCatalogo
            Dim condicion As String = String.Empty
            If (Me.EId > 0) Then
                condicion &= " WHERE Id=@id"
            End If
            comando.CommandText = "DELETE FROM " & LogicaEntradas.Programas.prefijoBaseDatosAlmacen & "UnidadesMedidas " & condicion
            comando.Parameters.AddWithValue("@id", Me.id)
            BaseDatos.conexionCatalogo.Open()
            comando.ExecuteNonQuery()
            BaseDatos.conexionCatalogo.Close()
        Catch ex As Exception
            Throw ex
        Finally
            BaseDatos.conexionCatalogo.Close()
        End Try

    End Sub

    Public Function ObtenerListado() As List(Of UnidadesMedidas)

        Try
            Dim lista As New List(Of UnidadesMedidas)
            Dim comando As New SqlCommand()
            comando.Connection = BaseDatos.conexionCatalogo
            Dim condicion As String = String.Empty
            If (Me.EId > 0) Then
                condicion &= " WHERE Id=@id"
            End If
            comando.CommandText = "SELECT Id, Nombre FROM " & LogicaEntradas.Programas.prefijoBaseDatosAlmacen & "UnidadesMedidas " & condicion & " ORDER BY Id ASC"
            comando.Parameters.AddWithValue("@id", Me.id)
            BaseDatos.conexionCatalogo.Open()
            Dim lectorDatos As SqlDataReader
            lectorDatos = comando.ExecuteReader()
            Dim unidadesMedidas As UnidadesMedidas
            While lectorDatos.Read()
                unidadesMedidas = New UnidadesMedidas()
                unidadesMedidas.id = Convert.ToInt32(lectorDatos("Id").ToString())
                unidadesMedidas.nombre = lectorDatos("Nombre").ToString()
                lista.Add(unidadesMedidas)
            End While
            BaseDatos.conexionCatalogo.Close()
            Return lista
        Catch ex As Exception
            Throw ex
        Finally
            BaseDatos.conexionCatalogo.Close()
        End Try

    End Function

End Class