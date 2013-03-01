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
End Namespace