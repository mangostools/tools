Imports System.Globalization

Class FilterOptions
	Public Property Column() As String
		Get
			Return m_Column
		End Get
		Private Set
			m_Column = Value
		End Set
	End Property
	Private m_Column As String
	Public Property Value() As String
		Get
			Return m_Value
		End Get
		Set
			m_Value = Value
		End Set
	End Property
	Private m_Value As String
	Public Property CompType() As ComparisonType
		Get
			Return m_CompType
		End Get
		Private Set
			m_CompType = Value
		End Set
	End Property
	Private m_CompType As ComparisonType

	Public Sub New(col As String, ct As ComparisonType, val As String)
		Column = col
		Value = val
		CompType = ct
	End Sub

	Public Overrides Function ToString() As String
		Return [String].Format(CultureInfo.InvariantCulture, "{0} {1} {2}", Column, CompType, Value)
	End Function
End Class
