Imports System.Globalization

Public Class BinaryFormatter
	Implements IFormatProvider
	Implements ICustomFormatter
	' IFormatProvider.GetFormat implementation.
	Public Function GetFormat(formatType As Type) As Object Implements IFormatProvider.GetFormat
		' Determine whether custom formatting object is requested.
		If formatType Is GetType(ICustomFormatter) Then
			Return Me
		Else
			Return Nothing
		End If
	End Function

	' Format number in binary (B), octal (O), or hexadecimal (H).
	Public Function Format(format__1 As String, arg As Object, formatProvider As IFormatProvider) As String Implements ICustomFormatter.Format
		' Handle format string.
		Dim baseNumber As Integer
		' Handle null or empty format string, string with precision specifier.
		Dim thisFmt As String = [String].Empty
		' Extract first character of format string (precision specifiers
		' are not supported).
		If Not [String].IsNullOrEmpty(format__1) Then
			thisFmt = If(format__1.Length > 1, format__1.Substring(0, 1), format__1)
		End If

		' Get a byte array representing the numeric value.
		Dim bytes As Byte()
		If TypeOf arg Is SByte Then
			Dim byteString As String = CSByte(arg).ToString("X2", CultureInfo.InvariantCulture)
			bytes = New Byte(0) {[Byte].Parse(byteString, NumberStyles.HexNumber, CultureInfo.InvariantCulture)}
		ElseIf TypeOf arg Is Byte Then
			bytes = New Byte(0) {CByte(arg)}
		ElseIf TypeOf arg Is Short Then
			bytes = BitConverter.GetBytes(CShort(arg))
		ElseIf TypeOf arg Is Integer Then
			bytes = BitConverter.GetBytes(CInt(arg))
		ElseIf TypeOf arg Is Long Then
			bytes = BitConverter.GetBytes(CLng(arg))
		ElseIf TypeOf arg Is UShort Then
			bytes = BitConverter.GetBytes(CUShort(arg))
		ElseIf TypeOf arg Is UInteger Then
			bytes = BitConverter.GetBytes(CUInt(arg))
		ElseIf TypeOf arg Is ULong Then
			bytes = BitConverter.GetBytes(CULng(arg))
		ElseIf TypeOf arg Is Single Then
			bytes = BitConverter.GetBytes(CSng(arg))
		Else
			Try
				Return HandleOtherFormats(format__1, arg)
			Catch e As FormatException
				Throw New FormatException([String].Format(CultureInfo.InvariantCulture, "The format of '{0}' is invalid.", format__1), e)
			End Try
		End If

		Select Case thisFmt.ToUpper(CultureInfo.InvariantCulture)
			' Binary formatting.
			Case "B"
				baseNumber = 2
				Exit Select
			Case "O"
				baseNumber = 8
				Exit Select
			Case "H"
				baseNumber = 16
				Exit Select
			Case Else
				' Handle unsupported format strings.
				Try
					Return HandleOtherFormats(format__1, arg)
				Catch e As FormatException
					Throw New FormatException([String].Format(CultureInfo.InvariantCulture, "The format of '{0}' is invalid.", format__1), e)
				End Try
		End Select

		' Return a formatted string.
		Dim numericString As String = [String].Empty
		For ctr As Integer = bytes.GetUpperBound(0) To bytes.GetLowerBound(0) Step -1
			Dim byteString As String = Convert.ToString(bytes(ctr), baseNumber)
			If baseNumber = 2 Then
				byteString = New [String]("0"C, 8 - byteString.Length) & byteString
			ElseIf baseNumber = 8 Then
				byteString = New [String]("0"C, 4 - byteString.Length) & byteString
			Else
				' Base is 16.
				byteString = New [String]("0"C, 2 - byteString.Length) & byteString
			End If

			numericString += byteString & " "
		Next
		Return numericString.Trim()
	End Function

	Private Shared Function HandleOtherFormats(format As String, arg As Object) As String
		Dim iFmt = TryCast(arg, IFormattable)
		If iFmt IsNot Nothing Then
			Return iFmt.ToString(format, CultureInfo.CurrentCulture)
		ElseIf arg IsNot Nothing Then
			Return arg.ToString()
		Else
			Return [String].Empty
		End If
	End Function
End Class
