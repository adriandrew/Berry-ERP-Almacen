﻿Imports System.Data.SqlClient

Public Class TiposCambios

    Private idMoneda As Integer
    Private fecha As Date
    Private valor As Double

    Public Property EIdMoneda() As Integer
        Get
            Return idMoneda
        End Get
        Set(value As Integer)
            idMoneda = value
        End Set
    End Property
    Public Property EFecha() As Date
        Get
            Return fecha
        End Get
        Set(value As Date)
            fecha = value
        End Set
    End Property
    Public Property EValor() As Double
        Get
            Return valor
        End Get
        Set(value As Double)
            valor = value
        End Set
    End Property

    Public Function ObtenerListado() As DataTable

        Try
            Dim datos As New DataTable
            Dim comando As New SqlCommand()
            comando.Connection = BaseDatos.conexionCatalogo
            Dim condicion As String = String.Empty
            If (Me.EIdMoneda > 0) Then
                condicion &= " AND IdMoneda=@idMoneda AND Fecha=@fecha"
            End If
            comando.CommandText = String.Format("SELECT IdMoneda, Fecha, Valor FROM {0}TiposCambios WHERE 0=0 {1} ORDER BY Fecha, IdMoneda DESC", ALMLogicaSalidas.Programas.prefijoBaseDatosAlmacen, condicion)
            comando.Parameters.AddWithValue("@idMoneda", Me.EIdMoneda)
            comando.Parameters.AddWithValue("@fecha", ALMLogicaSalidas.Funciones.ValidarFechaAEstandar(Me.EFecha))
            BaseDatos.conexionCatalogo.Open()
            Dim lectorDatos As SqlDataReader
            lectorDatos = comando.ExecuteReader()
            datos.Load(lectorDatos)
            BaseDatos.conexionCatalogo.Close()
            Return datos
        Catch ex As Exception
            Throw ex
        Finally
            BaseDatos.conexionCatalogo.Close()
        End Try

    End Function
     
End Class
