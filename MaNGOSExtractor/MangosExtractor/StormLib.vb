Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Runtime.InteropServices
Imports Microsoft.Win32

Namespace StormLib
    ' Flags for SFileOpenArchive	
    <Flags> _
    Public Enum OpenArchiveFlags As UInteger
        NO_LISTFILE = &H10
        ' Don't load the internal listfile	
        NO_ATTRIBUTES = &H20
        ' Don't open the attributes	
        MFORCE_MPQ_V1 = &H40
        ' Always open the archive as MPQ v 1.00, ignore the "wFormatVersion" variable in the header	
        MCHECK_SECTOR_CRC = &H80
        ' On files with MPQ_FILE_SECTOR_CRC, the CRC will be checked when reading file	
        READ_ONLY = &H100
        ' Open the archive for read-only access	
        ENCRYPTED = &H200
        ' Opens an encrypted MPQ archive (Example: Starcraft II installation)	
    End Enum

    ' Values for SFileExtractFile	
    Public Enum OpenFile As UInteger
        FROM_MPQ = &H0
        ' Open the file from the MPQ archive	
        PATCHED_FILE = &H1
        ' Open the file from the MPQ archive	
        BY_INDEX = &H2
        ' The 'szFileName' parameter is actually the file index	
        ANY_LOCALE = &HFFFFFFFEUI
        ' Reserved for StormLib internal use	
        LOCAL_FILE = &HFFFFFFFFUI
        ' Open the file from the MPQ archive	
    End Enum

    Public Class StormLib
        <DllImport("StormLib.dll")> _
        Public Shared Function SFileOpenArchive(<MarshalAs(UnmanagedType.LPStr)> szMpqName As String, dwPriority As UInteger, <MarshalAs(UnmanagedType.U4)> dwFlags As OpenArchiveFlags, ByRef phMpq As IntPtr) As Boolean
        End Function


        <DllImport("StormLib.dll")> _
        Public Shared Function SFileCloseArchive(hMpq As IntPtr) As Boolean
        End Function


        <DllImport("StormLib.dll")> _
        Public Shared Function SFileExtractFile(hMpq As IntPtr, <MarshalAs(UnmanagedType.LPStr)> szToExtract As String, <MarshalAs(UnmanagedType.LPStr)> szExtracted As String, <MarshalAs(UnmanagedType.U4)> dwSearchScope As OpenFile) As Boolean
        End Function


        <DllImport("StormLib.dll")> _
        Public Shared Function SFileOpenPatchArchive(hMpq As IntPtr, <MarshalAs(UnmanagedType.LPStr)> szMpqName As String, <MarshalAs(UnmanagedType.LPStr)> szPatchPathPrefix As String, dwFlags As UInteger) As Boolean
        End Function

    End Class

    Public Class MpqArchiveSet
        Implements IDisposable
        Private archives As New List(Of MpqArchive)()
        Private GameDir As String = ".\"

        Public Sub SetGameDir(dir As String)
            GameDir = dir
        End Sub

        Public Sub Dispose() Implements System.IDisposable.Dispose
            Close()
        End Sub

        Public Shared Function GetGameDirFromReg() As String
            Dim key As RegistryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Blizzard Entertainment\World of Warcraft")
            If key Is Nothing Then
                Return Nothing
            End If
            Dim val As [Object] = key.GetValue("InstallPath")
            If val Is Nothing Then
                Return Nothing
            End If
            Return val.ToString()
        End Function


        Public Function AddArchive(file As String) As Boolean
            Console.WriteLine("Adding archive: {0}", file)

            Dim a As New MpqArchive(GameDir + file, 0, OpenArchiveFlags.READ_ONLY)
            If a.IsOpen Then
                archives.Add(a)
                Console.WriteLine("Added archive: {0}", file)
                Return True
            End If
            Console.WriteLine("Failed to add archive: {0}", file)
            Return False
        End Function


        Public Function AddArchives(files As String()) As Integer
            Dim n As Integer = 0
            For Each s As String In files
                If AddArchive(s) Then
                    System.Math.Max(System.Threading.Interlocked.Increment(n), n - 1)
                End If
            Next
            Return n
        End Function


        Public Function ExtractFile(from As String, [to] As String, dwSearchScope As OpenFile) As Boolean
            For Each a As MpqArchive In archives
                Dim r As Object = a.ExtractFile(from, [to], dwSearchScope)
                If r Then
                    Return True
                End If
            Next
            Return False
        End Function





        Public Sub Close()
            For Each a As MpqArchive In archives
                a.Close()
            Next
            archives.Clear()
        End Sub

    End Class

    Public Class MpqLocale
        Public Shared ReadOnly Locales As String() = New String() {"enUS", "koKR", "frFR", "deDE", "zhTW", "esES", _
            "esMX", "ruRU", "enGB", "enTW", "base"}

        Public Shared Function GetPrefix(file As String) As String
            For Each loc As Object In Locales
                If file.Contains(loc) Then
                    Return loc
                End If
            Next

            Return "base"
        End Function


        Public Shared Function GetPrefixForPatch(file As String) As String
            Dim dir As Object = Path.GetDirectoryName(file)

            For Each loc As Object In Locales
                If file.Contains(loc) Then
                    Return [String].Empty
                End If
            Next

            Return "locale"
        End Function

    End Class

    Public Class MpqArchive
        Implements IDisposable
        Private handle As IntPtr = IntPtr.Zero

        Public Sub New(file As String, Prio As UInteger, Flags As OpenArchiveFlags)
            Dim r As Boolean = Open(file, Prio, Flags)
        End Sub


        Public ReadOnly Property IsOpen() As Boolean
            Get
                Return handle <> IntPtr.Zero
            End Get
        End Property

        Private Function Open(file As String, Prio As UInteger, Flags As OpenArchiveFlags) As Boolean
            Dim r As Boolean = StormLib.SFileOpenArchive(file, Prio, Flags, handle)
            If r Then
                OpenPatch(file)
            End If
            Return r
        End Function


        Private Sub OpenPatch(file As String)
            Dim gamedir As Object = MpqArchiveSet.GetGameDirFromReg()

            Dim patches As Object = Directory.GetFiles(gamedir, "Data\wow-update-*.mpq").ToList()

            Dim prefix As Object = MpqLocale.GetPrefix(file)

            If prefix <> "base" Then
                patches.RemoveAll(Function(s) s.Contains("base"))

                Dim localePatches As Object = Directory.GetFiles(gamedir, [String].Format("Data\{0}\wow-update-*.mpq", prefix))

                patches.AddRange(localePatches)
            End If

            For Each patch As Object In patches
                prefix = MpqLocale.GetPrefix(file)
                Dim pref As Object = MpqLocale.GetPrefixForPatch(patch)

                If pref <> "locale" Then
                    prefix = [String].Empty
                End If

                Console.WriteLine("Adding patch: {0} with prefix {1}", Path.GetFileName(patch), If(prefix <> [String].Empty, """" + prefix + """", """"""))
                Dim r As Boolean = StormLib.SFileOpenPatchArchive(handle, patch, prefix, 0)
                If Not r Then
                    Console.WriteLine("Failed to add patch: {0}", Path.GetFileName(patch))
                Else
                    Console.WriteLine("Added patch: {0}", Path.GetFileName(patch))
                End If
            Next
        End Sub


        Public Sub Dispose() Implements System.IDisposable.Dispose
            Close()
        End Sub


        Public Function Close() As Boolean
            Dim r As Boolean = StormLib.SFileCloseArchive(handle)
            If r Then
                handle = IntPtr.Zero
            End If
            Return r
        End Function


        Public Function ExtractFile(from As String, [to] As String, dwSearchScope As OpenFile) As Boolean
            Dim dir As Object = Path.GetDirectoryName([to])

            If Not Directory.Exists(dir) AndAlso Not [String].IsNullOrEmpty(dir) Then
                Directory.CreateDirectory(dir)
            End If

            Return StormLib.SFileExtractFile(handle, from, [to], dwSearchScope)
        End Function

    End Class
End Namespace