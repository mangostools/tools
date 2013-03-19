Imports System.Runtime.InteropServices

Namespace Blizzard
    <StructLayout(LayoutKind.Sequential)> _
    Structure PTCH
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=4)> _
        Public m_magic As Byte()
        Public m_patchSize As UInteger
        Public m_sizeBefore As UInteger
        Public m_sizeAfter As Integer
    End Structure

    <StructLayout(LayoutKind.Sequential)> _
    Structure MD5_
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=4)> _
        Public m_magic As Byte()
        Public m_md5BlockSize As UInteger
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=16)> _
        Public m_md5Before As Byte()
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=16)> _
        Public m_md5After As Byte()
    End Structure

    <StructLayout(LayoutKind.Sequential)> _
    Structure XFRM
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=4)> _
        Public m_magic As Byte()
        Public m_xfrmBlockSize As UInteger
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=4)> _
        Public m_type As Byte()
    End Structure

    <StructLayout(LayoutKind.Sequential)> _
    Structure BSDIFF40
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=8)> _
        Public m_magic As Byte()
        Public m_ctrlBlockSize As ULong
        Public m_diffBlockSize As ULong
        Public m_sizeAfter As ULong
    End Structure

    <StructLayout(LayoutKind.Sequential)> _
    Structure WDBC
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=5)> _
        Public m_magic As Byte()            ' "WDBC"
        Public m_recordCount As UInteger    ' The number of records
        Public m_fieldCount As UInteger     ' The number of fields
        Public m_recSize As UInteger        ' The size of a record
        Public m_stringSize As UInteger     ' The size of a string field
    End Structure

    <StructLayout(LayoutKind.Sequential)> _
    Structure map_fileheader
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=10)> _
        Public mapMagic As UInteger
        Public versionMagic As UInteger
        Public areaMapOffset As UInteger
        Public areaMapSize As UInteger
        Public heightMapOffset As UInteger
        Public heightMapSize As UInteger
        Public liquidMapOffset As UInteger
        Public liquidMapSize As UInteger
        Public holesOffset As UInteger
        Public holesSize As UInteger
    End Structure

    <StructLayout(LayoutKind.Sequential)> _
    Structure map_areaHeader
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=3)> _
        Public fourcc As UInteger
        Public flags As UInteger
        Public gridArea As UInteger
    End Structure

    <StructLayout(LayoutKind.Sequential)> _
    Structure map_heightHeader
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=4)> _
        Public fourcc As UInteger
        Public flags As UInteger
        Public gridHeight As Double
        Public gridMaxHeight As Double
    End Structure

    <StructLayout(LayoutKind.Sequential)> _
    Structure map_liquidHeader
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=8)> _
        Public fourcc As UInteger
        Public flags As UInt16
        Public liquidType As UInt16
        Public offsetX As SByte    'uint8
        Public offsetY As SByte
        Public width As SByte
        Public height As SByte
        Public liquidLevel As Double
    End Structure

    Enum MAP_LIQUID_TYPE As Integer
        NO_WATER = 0
        MAGMA = 1
        OCEAN = 2
        SLIME = 4
        WATER = 8
        DARK_WATER = 16 ' 0x10
        WMO_WATER = 32 '   0x20
    End Enum


    Enum MAP_HEIGHT As Integer
        NO_HEIGHT = 1
        AS_INT16 = 2
        AS_INT8 = 4
    End Enum





End Namespace