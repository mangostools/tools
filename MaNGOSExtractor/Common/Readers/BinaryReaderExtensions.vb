Imports System.Globalization
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Text

Public Module BinaryReaderExtensions
#Region "Coords3"
    ''' <summary>
    '''  Represents a coordinates of WoW object without orientation.
    ''' </summary>
    <StructLayout(LayoutKind.Sequential)> _
    Structure Coords3
        Public X As Single, Y As Single, Z As Single

        ''' <summary>
        '''  Converts the numeric values of this instance to its equivalent string representations, separator is space.
        ''' </summary>
        Public Function GetCoords() As String
            Dim coords As String = [String].Empty

            coords += X.ToString(CultureInfo.InvariantCulture)
            coords += " "
            coords += Y.ToString(CultureInfo.InvariantCulture)
            coords += " "
            coords += Z.ToString(CultureInfo.InvariantCulture)

            Return coords
        End Function
    End Structure
#End Region

#Region "Coords4"
    ''' <summary>
    '''  Represents a coordinates of WoW object with specified orientation.
    ''' </summary>
    <StructLayout(LayoutKind.Sequential)> _
    Structure Coords4
        Public X As Single, Y As Single, Z As Single, O As Single

        ''' <summary>
        '''  Converts the numeric values of this instance to its equivalent string representations, separator is space.
        ''' </summary>
        Public Function GetCoordsAsString() As String
            Dim coords As String = [String].Empty

            coords += X.ToString(CultureInfo.InvariantCulture)
            coords += " "
            coords += Y.ToString(CultureInfo.InvariantCulture)
            coords += " "
            coords += Z.ToString(CultureInfo.InvariantCulture)
            coords += " "
            coords += O.ToString(CultureInfo.InvariantCulture)

            Return coords
        End Function
    End Structure
#End Region

    Sub New()
    End Sub
    Public Function FromFile(fileName As String) As BinaryReader
        Return New BinaryReader(New FileStream(fileName, FileMode.Open), Encoding.UTF8)
    End Function

#Region "ReadPackedGuid"
    ''' <summary>
    '''  Reads the packed guid from the current stream and advances the current position of the stream by packed guid size.
    ''' </summary>
    <System.Runtime.CompilerServices.Extension> _
    Public Function ReadPackedGuid(reader As BinaryReader) As ULong
        Dim res As ULong = 0
        Dim mask As Byte = reader.ReadByte()

        If mask = 0 Then
            Return res
        End If

        Dim i As Integer = 0

        While i < 9
            If (mask And 1 << i) <> 0 Then
                res += CULng(reader.ReadByte()) << (i * 8)
            End If
            i += 1
        End While
        Return res
    End Function
#End Region

#Region "ReadStringNumber"
    ''' <summary>
    '''  Reads the string with known length from the current stream and advances the current position of the stream by string length.
    ''' <seealso cref="GenericReader.ReadStringNull"/>
    ''' </summary>
    <System.Runtime.CompilerServices.Extension> _
    Public Function ReadStringNumber(reader As BinaryReader) As String
        Dim text As String = [String].Empty
        Dim num As UInteger = reader.ReadUInt32()
        ' string length
        For i As UInteger = 0 To num - 1
            text += ChrW(reader.ReadByte())
        Next
        Return text
    End Function
#End Region

#Region "ReadStringNull"
    ''' <summary>
    '''  Reads the NULL terminated string from the current stream and advances the current position of the stream by string length + 1.
    ''' <seealso cref="GenericReader.ReadStringNumber"/>
    ''' </summary>
    <System.Runtime.CompilerServices.Extension> _
    Public Function ReadStringNull(reader As BinaryReader) As String
        Dim num As Byte
        Dim text As String = [String].Empty
        Dim temp As New System.Collections.Generic.List(Of Byte)()

        While (InlineAssignHelper(num, reader.ReadByte())) <> 0
            temp.Add(num)
        End While

        text = Encoding.UTF8.GetString(temp.ToArray())

        Return text
    End Function
#End Region

#Region "ReadCoords3"
    ''' <summary>
    '''  Reads the object coordinates from the current stream and advances the current position of the stream by 12 bytes.
    ''' </summary>
    <System.Runtime.CompilerServices.Extension> _
    Public Function ReadCoords3(reader As BinaryReader) As Coords3
        Dim v As Coords3

        v.X = reader.ReadSingle()
        v.Y = reader.ReadSingle()
        v.Z = reader.ReadSingle()

        Return v
    End Function
#End Region

#Region "ReadCoords4"
    ''' <summary>
    '''  Reads the object coordinates and orientation from the current stream and advances the current position of the stream by 16 bytes.
    ''' </summary>
    <System.Runtime.CompilerServices.Extension> _
    Public Function ReadCoords4(reader As BinaryReader) As Coords4
        Dim v As Coords4

        v.X = reader.ReadSingle()
        v.Y = reader.ReadSingle()
        v.Z = reader.ReadSingle()
        v.O = reader.ReadSingle()

        Return v
    End Function
#End Region

#Region "ReadStruct"
    ''' <summary>
    ''' Reads struct from the current stream and advances the current position if the stream by SizeOf(T) bytes.
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="reader"></param>
    ''' <returns></returns>
    <System.Runtime.CompilerServices.Extension> _
    Public Function ReadStruct(Of T As Structure)(reader As BinaryReader) As T
        Dim rawData As Byte() = reader.ReadBytes(Marshal.SizeOf(GetType(T)))
        Dim handle As GCHandle = GCHandle.Alloc(rawData, GCHandleType.Pinned)
        Dim returnObject As T = DirectCast(Marshal.PtrToStructure(handle.AddrOfPinnedObject(), GetType(T)), T)
        handle.Free()
        Return returnObject
    End Function
    Private Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
        target = value
        Return value
    End Function
#End Region
End Module
