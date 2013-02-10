
'
' * Copyright (C) 2012-2013 Arctium <http://arctium.org>
' * 
' * This program is free software: you can redistribute it and/or modify
' * it under the terms of the GNU General Public License as published by
' * the Free Software Foundation, either version 3 of the License, or
' * (at your option) any later version.
' *
' * This program is distributed in the hope that it will be useful,
' * but WITHOUT ANY WARRANTY; without even the implied warranty of
' * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' * GNU General Public License for more details.
' *
' * You should have received a copy of the GNU General Public License
' * along with this program.  If not, see <http://www.gnu.org/licenses/>.
' 


Imports System.Data

Namespace Framework.Database
    Public Class SQLResult
        Inherits DataTable
        Public Property Count() As Integer
            Get
                Return m_Count
            End Get
            Set(value As Integer)
                m_Count = Value
            End Set
        End Property
        Private m_Count As Integer

        Public Function Read(Of T)(row As Integer, columnName As String, Optional number As Integer = 0) As T
            Return DirectCast(Convert.ChangeType(Rows(row)(columnName & (If(number <> 0, (1 + number).ToString(), ""))), GetType(T)), T)
        End Function

        Public Function ReadAllValuesFromField(columnName As String) As Object()
            Dim obj As Object() = New Object(Count - 1) {}

            For i As Integer = 0 To Count - 1
                obj(i) = Rows(i)(columnName)
            Next

            Return obj
        End Function
    End Class
End Namespace