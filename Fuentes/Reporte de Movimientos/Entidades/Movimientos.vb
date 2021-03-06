﻿Imports System.Data.SqlClient

Public Class Movimientos

    Private idAlmacen As Integer
    Private idFamilia As Integer
    Private idSubFamilia As Integer
    Private idArticulo As Integer
    Private fecha As Date
    Private fecha2 As Date

    Public Property EIdAlmacen() As Integer
        Get
            Return Me.idAlmacen
        End Get
        Set(value As Integer)
            Me.idAlmacen = value
        End Set
    End Property
    Public Property EIdFamilia() As Integer
        Get
            Return Me.idFamilia
        End Get
        Set(value As Integer)
            Me.idFamilia = value
        End Set
    End Property
    Public Property EIdSubFamilia() As Integer
        Get
            Return Me.idSubFamilia
        End Get
        Set(value As Integer)
            Me.idSubFamilia = value
        End Set
    End Property
    Public Property EIdArticulo() As Integer
        Get
            Return Me.idArticulo
        End Get
        Set(value As Integer)
            Me.idArticulo = value
        End Set
    End Property
    Public Property EFecha() As String
        Get
            Return Me.fecha
        End Get
        Set(value As String)
            Me.fecha = value
        End Set
    End Property
    Public Property EFecha2() As String
        Get
            Return Me.fecha2
        End Get
        Set(value As String)
            Me.fecha2 = value
        End Set
    End Property

    Public Function ObtenerListadoReporte(ByVal opcionMovimiento As Integer, ByVal aplicaFecha As Boolean) As DataTable

        Dim datos As New DataTable
        Try
            Dim comando As New SqlCommand()
            comando.Connection = BaseDatos.conexionAlmacen
            Dim condicion As String = String.Empty
            Dim condicionFechaRango As String = String.Empty
            Dim tabla As String = String.Empty
            Dim campo As String = String.Empty
            If (Me.EIdAlmacen > 0) Then
                condicion &= " AND IdAlmacen=@idAlmacen "
            End If
            If (Me.EIdFamilia > 0) Then
                condicion &= " AND IdFamilia=@idFamilia "
            End If
            If (Me.EIdSubFamilia > 0) Then
                condicion &= " AND IdSubFamilia=@idSubFamilia "
            End If
            If (Me.EIdArticulo > 0) Then
                condicion &= " AND IdArticulo=@idArticulo "
            End If
            If (aplicaFecha) Then
                condicionFechaRango &= " AND Fecha BETWEEN @fecha AND @fecha2 " 
            End If
            If (opcionMovimiento = 0) Then
                tabla = "Entradas"
                campo = "Entrada"
            Else
                tabla = "Salidas"
                campo = "Salida"
            End If 
            comando.CommandText = String.Format("SELECT O.Nombre, M.Id, M.Fecha, C.*, M.IdTipo{2}, TM.Nombre, M.Cantidad, M.Precio, M.Costo " & _
            " FROM " & _
            " ( " & _
                " SELECT IdOrigen, IdAlmacen, IdFamilia, IdSubFamilia, IdArticulo, Fecha, Id, IdTipo{2}, NULL AS NombreTipo, ISNULL(SUM(Cantidad), 0) AS Cantidad, ISNULL(SUM(Precio), 0) AS Precio, ISNULL(SUM(Total), 0) AS Costo " & _
                " FROM {1} WHERE 0=0 {3} GROUP BY  IdOrigen, IdAlmacen, IdFamilia, IdSubFamilia, IdArticulo, Fecha, Id, IdTipo{2} " & _
            " ) AS M " & _
            " LEFT JOIN " & _
            " ( " & _
                " SELECT A.Id AS IdAlmacen, A.Abreviatura AS Abreviatura, A.Nombre AS NombreAlmacen, F.Id AS IdFamilia, F.Nombre AS NombreFamilia, SF.Id AS IdSubFamilia, SF.Nombre AS NombreSubFamilia, A2.Id AS IdArticulo, A2.Nombre AS NombreArticulo " & _
                " FROM {0}Almacenes AS A LEFT JOIN {0}Familias AS F ON A.Id = F.IdAlmacen LEFT JOIN {0}SubFamilias AS SF ON A.Id = SF.IdAlmacen AND F.Id = SF.IdFamilia LEFT JOIN {0}Articulos AS A2 ON A.Id = A2.IdAlmacen AND F.Id = A2.IdFamilia AND SF.Id = A2.IdSubFamilia " & _
            " ) AS C " & _
            " ON M.IdAlmacen = C.IdAlmacen AND M.IdFamilia = C.IdFamilia AND M.IdSubFamilia = C.IdSubFamilia AND M.IdArticulo = C.IdArticulo " & _
            " LEFT JOIN " & _
            " ( " & _
                " SELECT Id, Nombre FROM {0}Origenes " & _
            " ) AS O ON M.IdOrigen = O.Id " & _
            " LEFT JOIN " & _
            " ( " & _
                " SELECT Id, Nombre FROM {0}Tipos{1} " & _
            " ) AS TM ON M.IdTipo{2} = TM.Id " & _
            " GROUP BY O.Nombre, M.Id, M.Fecha, M.IdTipo{2}, TM.Nombre, M.Cantidad, M.Precio, M.Costo, C.IdAlmacen, C.Abreviatura, C.NombreAlmacen, C.IdFamilia, C.NombreFamilia, C.IdSubFamilia, C.NombreSubFamilia, C.IdArticulo, C.NombreArticulo ORDER BY M.Id ASC, C.IdAlmacen ASC", ALMLogicaReporteMovimientos.Programas.bdCatalogo & ".dbo." & ALMLogicaReporteMovimientos.Programas.prefijoBaseDatosAlmacen, tabla, campo, condicionFechaRango & condicion)
            comando.Parameters.AddWithValue("@idAlmacen", Me.EIdAlmacen)
            comando.Parameters.AddWithValue("@idFamilia", Me.EIdFamilia)
            comando.Parameters.AddWithValue("@idSubFamilia", Me.EIdSubFamilia)
            comando.Parameters.AddWithValue("@idArticulo", Me.EIdArticulo)
            comando.Parameters.AddWithValue("@fecha", ALMLogicaReporteMovimientos.Funciones.ValidarFechaAEstandar(Me.EFecha))
            comando.Parameters.AddWithValue("@fecha2", ALMLogicaReporteMovimientos.Funciones.ValidarFechaAEstandar(Me.EFecha2))
            BaseDatos.conexionAlmacen.Open()
            Dim dataReader As SqlDataReader
            dataReader = comando.ExecuteReader()
            datos.Load(dataReader)
            BaseDatos.conexionAlmacen.Close()
            Return datos
        Catch ex As Exception
            Throw ex
        Finally
            BaseDatos.conexionAlmacen.Close()
        End Try

    End Function

End Class
