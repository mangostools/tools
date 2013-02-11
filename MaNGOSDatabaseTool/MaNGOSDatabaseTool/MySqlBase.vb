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


Imports Microsoft.VisualBasic.Logging
Imports MySql.Data.MySqlClient
Imports System.Collections.Generic
Imports System.Data
Imports System.Globalization
Imports System.Text
Imports System.Threading

Namespace Framework.Database
    Public Class MySqlBase
        Private ConnectionString As String
        Public Property RowCount() As Integer
            Get
                Return m_RowCount
            End Get
            Set(value As Integer)
                m_RowCount = Value
            End Set
        End Property
        Private m_RowCount As Integer

        Public Property ErrorMsg() As String
            Get
                Return m_ErrMsg
            End Get
            Set(value As String)
                m_ErrMsg = value
            End Set
        End Property
        Private m_ErrMsg As Integer

        Public Function Init(host As String, user As String, password As String, database As String, port As Integer) As Boolean
            ConnectionString = "Server=" & host & ";User Id=" & user & ";Port=" & port & ";" & "Password=" & password & ";Database=" & database & ";Allow Zero Datetime=True;" & "Min Pool Size = 25;Max Pool Size=150"

            Using Connection = New MySqlConnection(ConnectionString)
                Try
                    Connection.Open()
                    Return True
                    '                    Log.Message(LogType.NORMAL, "Successfully tested connection to {0}:{1}:{2}", host, port, database)
                Catch ex As MySqlException
                    '                   Log.Message(LogType.[ERROR], "{0}", ex.Message)

                    ' Try auto reconnect on error (every 5 seconds)
                    'Log.Message(LogType.[ERROR], "Try reconnect in 5 seconds...")
                    Thread.Sleep(5000)

                    Init(host, user, password, database, port)
                    Return False
                End Try
            End Using
        End Function

        Public Function Execute(sql As String, ParamArray args As Object()) As Boolean
            Dim sqlString As New StringBuilder()
            ' Fix for floating point problems on some languages
            sqlString.AppendFormat(CultureInfo.GetCultureInfo("en-US").NumberFormat, sql)

            Using Connection = New MySqlConnection(ConnectionString)
                Connection.Open()

                Using sqlCommand As New MySqlCommand(sqlString.ToString(), Connection)
                    Try
                        Dim mParams As New List(Of MySqlParameter)(args.Length)
                        For Each a As object In args
                            mParams.Add(New MySqlParameter("", a))
                        Next

                        sqlCommand.Parameters.AddRange(mParams.ToArray())
                        sqlCommand.ExecuteNonQuery()

                        Return True
                    Catch ex As MySqlException
                        'Log.Message(LogType.[ERROR], "{0}", ex.Message)
                        Return False
                    End Try
                End Using
            End Using
        End Function

        Public Function [Select](sql As String, args As Dictionary(Of String, Int32)) As SQLResult
            Dim sqlString As New StringBuilder()
            ' Fix for floating point problems on some languages
            sqlString.AppendFormat(CultureInfo.GetCultureInfo("en-US").NumberFormat, sql)

            Using Connection = New MySqlConnection(ConnectionString)
                Connection.Open()

                Dim sqlCommand As New MySqlCommand(sqlString.ToString(), Connection)

                Try
                    Dim mParams As New List(Of MySqlParameter)(args.Count)

                    For Each a As Object In args
                        mParams.Add(New MySqlParameter(a.key.ToString(), a.value))
                    Next

                    sqlCommand.Parameters.AddRange(mParams.ToArray())

                    Using SqlData = sqlCommand.ExecuteReader(CommandBehavior.[Default])
                        Using retData = New SQLResult()
                            retData.Load(SqlData)
                            retData.Count = retData.Rows.Count

                            Return retData
                        End Using
                    End Using
                Catch ex As MySqlException
                    Dim retData = New SQLResult()
                    retData.ErrorMsg = ex.Message
                    Return retData
                End Try
            End Using

            Return Nothing
        End Function


        Public Function [Select](sql As String, ParamArray args As Object()) As SQLResult
            Dim sqlString As New StringBuilder()
            ' Fix for floating point problems on some languages
            sqlString.AppendFormat(CultureInfo.GetCultureInfo("en-US").NumberFormat, sql)

            Using Connection = New MySqlConnection(ConnectionString)
                Connection.Open()

                Dim sqlCommand As New MySqlCommand(sqlString.ToString(), Connection)

                Try
                    Dim mParams As New List(Of MySqlParameter)(args.Length)

                    For Each a As Object In args
                        mParams.Add(New MySqlParameter("", a))
                    Next

                    sqlCommand.Parameters.AddRange(mParams.ToArray())

                    Using SqlData = sqlCommand.ExecuteReader(CommandBehavior.[Default])
                        Using retData = New SQLResult()
                            retData.Load(SqlData)
                            retData.Count = retData.Rows.Count

                            Return retData
                        End Using
                    End Using
                Catch ex As MySqlException
                    Dim retData = New SQLResult()
                    retData.ErrorMsg = ex.Message
                    Return retData
                End Try
            End Using

            Return Nothing
        End Function

        Public Sub ExecuteBigQuery(table As String, fields As String, fieldCount As Integer, resultCount As Integer, values As Object(), Optional resultAsIndex As Boolean = True)
            If values.Length > 0 Then
                Dim sqlString As New StringBuilder()

                sqlString.AppendFormat("INSERT INTO {0} ({1}) VALUES ", table, fields)

                For i As Integer = 0 To resultCount - 1
                    sqlString.AppendFormat("(")

                    For j As Integer = 0 To fieldCount - 1
                        Dim index As Integer = If(resultAsIndex, i, j)

                        If j = fieldCount - 1 Then
                            sqlString.Append([String].Format(CultureInfo.GetCultureInfo("en-US").NumberFormat, "'{0}'", values(index)))
                        Else
                            sqlString.Append([String].Format(CultureInfo.GetCultureInfo("en-US").NumberFormat, "'{0}', ", values(index)))
                        End If
                    Next

                    If i = resultCount - 1 Then
                        sqlString.AppendFormat(");")
                    Else
                        sqlString.AppendFormat("),")
                    End If
                Next

                Using Connection = New MySqlConnection(ConnectionString)
                    Connection.Open()

                    Dim sqlCommand As New MySqlCommand(sqlString.ToString(), Connection)
                    sqlCommand.ExecuteNonQuery()
                End Using
            End If
        End Sub
    End Class
End Namespace
