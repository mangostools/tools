Imports System.IO

Public Class Export2Map
    '
    ' Adt file convertor function and data
    '

    ' Map file format data
    Public MAP_MAGIC As String = "MAPS"
    Public MAP_VERSION_MAGIC As String = "z1.3"
    Public MAP_AREA_MAGIC As String = "AREA"
    Public MAP_HEIGHT_MAGIC As String = "MHGT"
    Public MAP_LIQUID_MAGIC As String = "MLIQ"

    Structure map_fileheader
        Private mapMagic As UInt32
        Private versionMagic As UInt32
        Private areaMapOffset As UInt32
        Private areaMapSize As UInt32
        Private heightMapOffset As UInt32
        Private heightMapSize As UInt32
        Private liquidMapOffset As UInt32
        Private liquidMapSize As UInt32
        Private holesOffset As UInt32
        Private holesSize As UInt32
    End Structure

    Const MAP_AREA_NO_AREA As Integer = &H1

    Private Structure map_areaHeader
        Dim fourcc As UInt32
        Dim flags As UInt16
        Dim gridArea As UInt16
    End Structure

    Const MAP_HEIGHT_NO_HEIGHT As Integer = &H1
    Const MAP_HEIGHT_AS_INT16 As Integer = &H2
    Const MAP_HEIGHT_AS_INT8 As Integer = &H4

    Private Structure map_heightHeader
        Dim fourcc As UInt32
        Dim flags As UInt32
        Dim gridHeight As Double
        Dim gridMaxHeight As Double
    End Structure

    Const MAP_LIQUID_TYPE_NO_WATER As Integer = &H0
    Const MAP_LIQUID_TYPE_MAGMA As Integer = &H1
    Const MAP_LIQUID_TYPE_OCEAN As Integer = &H2
    Const MAP_LIQUID_TYPE_SLIME As Integer = &H4
    Const MAP_LIQUID_TYPE_WATER As Integer = &H8

    Const MAP_LIQUID_TYPE_DARK_WATER As Integer = &H10
    Const MAP_LIQUID_TYPE_WMO_WATER As Integer = &H20


    Const MAP_LIQUID_NO_TYPE As Integer = &H1
    Const MAP_LIQUID_NO_HEIGHT As Integer = &H2

    Structure map_liquidHeader
        Private fourcc As UInt32
        Private flags As UInt16
        Private liquidType As UInt16
        Private offsetX As SByte
        Private offsetY As SByte
        Private width As SByte
        Private height As SByte
        Private liquidLevel As Double
    End Structure

    Private Shared Function selectUInt8StepStore(maxDiff As Single) As Single
        Return 255 / maxDiff
    End Function

    Private Shared Function selectUInt16StepStore(maxDiff As Single) As Single
        Return 65535 / maxDiff
    End Function

    Const ADT_CELLS_PER_GRID As Integer = 16
    Const ADT_CELL_SIZE As Integer = 8
    Const ADT_GRID_SIZE As Integer = (ADT_CELLS_PER_GRID * ADT_CELL_SIZE)

    ' Temporary grid data store
    Public area_flags(ADT_CELLS_PER_GRID, ADT_CELLS_PER_GRID) As UInt16

    Public V8(ADT_GRID_SIZE, ADT_GRID_SIZE) As Double
    Public V9(ADT_GRID_SIZE + 1, ADT_GRID_SIZE + 1) As Double
    Public uint16_V8(ADT_GRID_SIZE, ADT_GRID_SIZE) As UInt16
    Public uint16_V9(ADT_GRID_SIZE + 1, ADT_GRID_SIZE + 1) As UInt16
    Public uint8_V8(ADT_GRID_SIZE, ADT_GRID_SIZE) As SByte
    Public uint8_V9(ADT_GRID_SIZE + 1, ADT_GRID_SIZE + 1) As SByte

    Public liquid_entry(ADT_CELLS_PER_GRID, ADT_CELLS_PER_GRID) As UInt16
    Public liquid_flags(ADT_CELLS_PER_GRID, ADT_CELLS_PER_GRID) As SByte
    Public liquid_show(ADT_GRID_SIZE, ADT_GRID_SIZE) As Boolean
    Public liquid_height(ADT_GRID_SIZE + 1, ADT_GRID_SIZE + 1) As Double

    ' This option allow use float to int conversion
    Public CONF_allow_float_to_int As Boolean = True
    Public CONF_float_to_int8_limit As Double = 2.0F  '    // Max accuracy = val/256
    Public CONF_float_to_int16_limit As Double = 2048.0F '   // Max accuracy = val/65536
    Public CONF_flat_height_delta_limit As Double = 0.005F ' // If max - min less this value - surface is flat
    Public CONF_flat_liquid_delta_limit As Double = 0.001F ' // If max - min less this value - liquid surface is flat

    Public Sub ConvertADT(adtfilename As String, ByRef outputFilename As String, mapx As Integer, mapy As Integer, dictMaps As Dictionary(Of Integer, String), dictAreaTable As Dictionary(Of Integer, String), dictLiquidType As Dictionary(Of Integer, Integer))
        'these need to be options switches
        Dim CONF_allow_height_limit As Boolean = True
        Dim CONF_use_minHeight As Double = -500.0F

        Dim filename = adtfilename
        Console.WriteLine(filename)
        Dim f = New FileStream(filename, FileMode.Open, FileAccess.Read)
        Dim map = New MapParser(f)

        Dim maxV = [Single].MinValue
        Dim minV = [Single].MaxValue

        Dim area_flags(ADT_CELLS_PER_GRID, ADT_CELLS_PER_GRID) As UInt16

        Dim V8(ADT_GRID_SIZE, ADT_GRID_SIZE) As Double
        Dim V9(ADT_GRID_SIZE + 1, ADT_GRID_SIZE + 1) As Double
        Dim uint16_V8(ADT_GRID_SIZE, ADT_GRID_SIZE) As UInt16
        Dim uint16_V9(ADT_GRID_SIZE + 1, ADT_GRID_SIZE + 1) As UInt16
        Dim uint8_V8(ADT_GRID_SIZE, ADT_GRID_SIZE) As SByte
        Dim uint8_V9(ADT_GRID_SIZE + 1, ADT_GRID_SIZE + 1) As SByte

        Dim liquid_entry(ADT_CELLS_PER_GRID, ADT_CELLS_PER_GRID) As UInt16
        Dim liquid_flags(ADT_CELLS_PER_GRID, ADT_CELLS_PER_GRID) As SByte
        Dim liquid_show(ADT_GRID_SIZE, ADT_GRID_SIZE) As Boolean
        Dim liquid_height(ADT_GRID_SIZE + 1, ADT_GRID_SIZE + 1) As Double

        If map.MaxV > maxV Then
            maxV = map.MaxV
        End If
        If map.MinV < minV Then
            minV = map.MinV
        End If


        ' Get area flags data
        Dim thisMCNK As MCNK = map.MCNK

        For i As Integer = 0 To ADT_CELLS_PER_GRID - 1
            For j As Integer = 0 To ADT_CELLS_PER_GRID - 1
                thisMCNK.IndexX = i
                thisMCNK.IndexY = j
                If thisMCNK.areaid > 0 Then
                    If dictAreaTable(thisMCNK.areaid) <> &HFFFF Then
                        area_flags(i, j) = dictAreaTable(thisMCNK.areaid)
                        Continue For
                    Else
                        Core.Alert("File: " & filename & " Can't find area flag for areaid " & thisMCNK.areaid & " [" & i & "," & j & "].", Core.AlertNewLine.AddCRLF)
                    End If
                End If

                area_flags(i, j) = &HFFFF
            Next
        Next


        '============================================
        ' Try pack area data
        '============================================
        Dim fullAreaData As Boolean = False
        Dim areaflag As UInt32 = area_flags(0, 0)
        For y As Integer = 0 To ADT_CELLS_PER_GRID - 1
            For x As Integer = 0 To ADT_CELLS_PER_GRID - 1
                If area_flags(y, x) <> areaflag Then
                    fullAreaData = True
                    Exit For
                End If
            Next
        Next

        '          map.areaMapOffset = sizeof(map);
        'map.areaMapSize   = sizeof(map_areaHeader);

        Dim areaHeader As map_areaHeader
        areaHeader.fourcc = 1095910721 'AREA 
        areaHeader.flags = 0
        If (fullAreaData) Then

            areaHeader.gridArea = 0
            'map.areaMapSize += sizeof(area_flags)

        Else

            areaHeader.flags = MAP_AREA_NO_AREA
            areaHeader.gridArea = CShort(thisMCNK.flags)
        End If



        '
        ' Get Height map from grid
        '
        For i As Integer = 0 To ADT_CELLS_PER_GRID - 1
            For j As Integer = 0 To ADT_CELLS_PER_GRID - 1
                thisMCNK.IndexX = i
                thisMCNK.IndexY = j

                'Dim cell As Pointer(Of adt_MCNK) = cells.getMCNK(i, j)
                'If Not cell Then
                '    Continue For
                'End If
                ' Height values for triangles stored in order:
                ' 1     2     3     4     5     6     7     8     9
                '    10    11    12    13    14    15    16    17
                ' 18    19    20    21    22    23    24    25    26
                '    27    28    29    30    31    32    33    34
                ' . . . . . . . .
                ' For better get height values merge it to V9 and V8 map
                ' V9 height map:
                ' 1     2     3     4     5     6     7     8     9
                ' 18    19    20    21    22    23    24    25    26
                ' . . . . . . . .
                ' V8 height map:
                '    10    11    12    13    14    15    16    17
                '    27    28    29    30    31    32    33    34
                ' . . . . . . . .

                ' Set map height as grid height
                For y As Integer = 0 To ADT_CELL_SIZE
                    Dim cy As Integer = i * ADT_CELL_SIZE + y
                    For x As Integer = 0 To ADT_CELL_SIZE
                        Dim cx As Integer = j * ADT_CELL_SIZE + x
                        V9(cy, cx) = thisMCNK.position.Y ' cell.ypos
                    Next
                Next
                For y As Integer = 0 To ADT_CELL_SIZE - 1
                    Dim cy As Integer = i * ADT_CELL_SIZE + y
                    For x As Integer = 0 To ADT_CELL_SIZE - 1
                        Dim cx As Integer = j * ADT_CELL_SIZE + x
                        V8(cy, cx) = thisMCNK.position.Y ' cell.ypos
                    Next
                Next
                ' Get custom height
                Dim thisMCVT As MCVT = map.MCVT


                'Dim v As Double = thisMCVT.height '  thisMCNK.ofsHeight ' cell.getMCVT()
                If Not IsNothing(thisMCVT) Then
                    Continue For
                End If
                ' get V9 height map
                For y As Integer = 0 To ADT_CELL_SIZE
                    Dim cy As Integer = i * ADT_CELL_SIZE + y
                    For x As Integer = 0 To ADT_CELL_SIZE
                        Dim cx As Integer = j * ADT_CELL_SIZE + x
                        V9(cy, cx) += thisMCVT.height '.height_map(y * (ADT_CELL_SIZE * 2 + 1) + x)
                    Next
                Next
                ' get V8 height map
                For y As Integer = 0 To ADT_CELL_SIZE - 1
                    Dim cy As Integer = i * ADT_CELL_SIZE + y
                    For x As Integer = 0 To ADT_CELL_SIZE - 1
                        Dim cx As Integer = j * ADT_CELL_SIZE + x
                        V8(cy, cx) += thisMCVT.height 'v.height_map(y * (ADT_CELL_SIZE * 2 + 1) + ADT_CELL_SIZE + 1 + x)
                    Next
                Next
            Next
        Next

        '============================================
        ' Try pack height data
        '============================================
        Dim maxHeight As Single = -20000
        Dim minHeight As Single = 20000
        For y As Integer = 0 To ADT_GRID_SIZE - 1
            For x As Integer = 0 To ADT_GRID_SIZE - 1
                Dim h As Single = V8(y, x)
                If maxHeight < h Then
                    maxHeight = h
                End If
                If minHeight > h Then
                    minHeight = h
                End If
            Next
        Next
        For y As Integer = 0 To ADT_GRID_SIZE
            For x As Integer = 0 To ADT_GRID_SIZE
                Dim h As Single = V9(y, x)
                If maxHeight < h Then
                    maxHeight = h
                End If
                If minHeight > h Then
                    minHeight = h
                End If
            Next
        Next

        ' Check for allow limit minimum height (not store height in deep ochean - allow save some memory)
        If CONF_allow_height_limit = True AndAlso minHeight < CONF_use_minHeight Then
            For y As Integer = 0 To ADT_GRID_SIZE - 1
                For x As Integer = 0 To ADT_GRID_SIZE - 1
                    If V8(y, x) < CONF_use_minHeight Then
                        V8(y, x) = CONF_use_minHeight
                    End If
                Next
            Next
            For y As Integer = 0 To ADT_GRID_SIZE
                For x As Integer = 0 To ADT_GRID_SIZE
                    If V9(y, x) < CONF_use_minHeight Then
                        V9(y, x) = CONF_use_minHeight
                    End If
                Next
            Next
            If minHeight < CONF_use_minHeight Then
                minHeight = CONF_use_minHeight
            End If
            If maxHeight < CONF_use_minHeight Then
                maxHeight = CONF_use_minHeight
            End If
        End If


        '        map.heightMapOffset = map.areaMapOffset + map.areaMapSize;
        'map.heightMapSize = sizeof(map_heightHeader);

        Dim heightHeader As map_heightHeader
        heightHeader.fourcc = 1296582484 'MGHT
        heightHeader.flags = 0
        heightHeader.gridHeight = minHeight
        heightHeader.gridMaxHeight = maxHeight

        If (maxHeight = minHeight) Then
            heightHeader.flags = MAP_HEIGHT_NO_HEIGHT
        End If

        ' Not need store if flat surface
        If (CONF_allow_float_to_int And (maxHeight - minHeight) < CONF_flat_height_delta_limit) Then
            heightHeader.flags = MAP_HEIGHT_NO_HEIGHT
        End If

        ' Try store as packed in uint16 or uint8 values
        If Not (heightHeader.flags And MAP_HEIGHT_NO_HEIGHT) Then
            Dim [step] As Single
            ' Try Store as uint values
            If CONF_allow_float_to_int Then
                Dim diff As Single = maxHeight - minHeight
                If diff < CONF_float_to_int8_limit Then
                    ' As uint8 (max accuracy = CONF_float_to_int8_limit/256)
                    heightHeader.flags = heightHeader.flags Or MAP_HEIGHT_AS_INT8
                    [step] = selectUInt8StepStore(diff)
                ElseIf diff < CONF_float_to_int16_limit Then
                    ' As uint16 (max accuracy = CONF_float_to_int16_limit/65536)
                    heightHeader.flags = heightHeader.flags Or MAP_HEIGHT_AS_INT16
                    [step] = selectUInt16StepStore(diff)
                End If
            End If

            ' Pack it to int values if need
            If heightHeader.flags And MAP_HEIGHT_AS_INT8 Then
                For y As Integer = 0 To ADT_GRID_SIZE - 1
                    For x As Integer = 0 To ADT_GRID_SIZE - 1
                        uint8_V8(y, x) = CSByte((V8(y, x) - minHeight) * [step] + 0.5F)
                    Next
                Next
                For y As Integer = 0 To ADT_GRID_SIZE
                    For x As Integer = 0 To ADT_GRID_SIZE
                        uint8_V9(y, x) = CSByte((V9(y, x) - minHeight) * [step] + 0.5F)
                    Next
                Next
                'map.heightMapSize += sizeof(uint8_V9) + sizeof(uint8_V8)
            ElseIf heightHeader.flags And MAP_HEIGHT_AS_INT16 Then
                For y As Integer = 0 To ADT_GRID_SIZE - 1
                    For x As Integer = 0 To ADT_GRID_SIZE - 1
                        uint16_V8(y, x) = CUInt((V8(y, x) - minHeight) * [step] + 0.5F)
                    Next
                Next
                For y As Integer = 0 To ADT_GRID_SIZE
                    For x As Integer = 0 To ADT_GRID_SIZE
                        uint16_V9(y, x) = CUInt((V9(y, x) - minHeight) * [step] + 0.5F)
                    Next
                Next
                'map.heightMapSize += sizeof(uint16_V9) + sizeof(uint16_V8)
            Else
                'map.heightMapSize += sizeof(V9) + sizeof(V8)
            End If
        End If

        ' Get from MCLQ chunk (old)
        For i As Integer = 0 To ADT_CELLS_PER_GRID - 1
            For j As Integer = 0 To ADT_CELLS_PER_GRID - 1
                'Dim cell As Pointer(Of adt_MCNK) = cells.getMCNK(i, j)
                'If Not cell Then
                '    Continue For
                'End If
                Dim thisMCLQ As MCLQ = map.MCLQ

                Dim liquid(,) As liquid_data = thisMCLQ.liquid ' Double = thisMCLQ.liquid(i, j).height
                Dim count As Integer = 0

                'If Not liquid OrElse cell.sizeMCLQ <= 8 Then
                '    Continue For
                'End If

                For y As Integer = 0 To ADT_CELL_SIZE - 1
                    Dim cy As Integer = i * ADT_CELL_SIZE + y
                    For x As Integer = 0 To ADT_CELL_SIZE - 1
                        Dim cx As Integer = j * ADT_CELL_SIZE + x
                        If thisMCLQ.flags(y, x) <> &HF Then
                            liquid_show(cy, cx) = True
                            If thisMCLQ.flags(y, x) And (1 << 7) Then
                                liquid_flags(i, j) = liquid_flags(i, j) Or MAP_LIQUID_TYPE_DARK_WATER
                            End If
                            count += 1
                        End If
                    Next
                Next

                Dim c_flag As UInt32 = thisMCLQ.flags(i, j)
                If c_flag And (1 << 2) Then
                    liquid_entry(i, j) = 1
                    ' water
                    liquid_flags(i, j) = liquid_flags(i, j) Or MAP_LIQUID_TYPE_WATER
                End If
                If c_flag And (1 << 3) Then
                    liquid_entry(i, j) = 2
                    ' ocean
                    liquid_flags(i, j) = liquid_flags(i, j) Or MAP_LIQUID_TYPE_OCEAN
                End If
                If c_flag And (1 << 4) Then
                    liquid_entry(i, j) = 3
                    ' magma/slime
                    liquid_flags(i, j) = liquid_flags(i, j) Or MAP_LIQUID_TYPE_MAGMA
                End If

                If Not count AndAlso liquid_flags(i, j) Then
                    Core.Alert("Wrong liquid detect in MCLQ chunk", Core.AlertNewLine.AddCRLF)
                End If

                For y As Integer = 0 To ADT_CELL_SIZE
                    Dim cy As Integer = i * ADT_CELL_SIZE + y
                    For x As Integer = 0 To ADT_CELL_SIZE
                        Dim cx As Integer = j * ADT_CELL_SIZE + x
                        liquid_height(cy, cx) = thisMCLQ.liquid(y, x).height
                    Next
                Next
            Next
        Next



    End Sub




End Class
