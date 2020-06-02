Imports System.Data
Imports System.IO

Public Class CSVTabler
    'TODO Handle " char as qualifier
    'TODO Handle rows with different length

    Private ReadOnly Ellipsis As Char = Convert.ToChar(8230) '…
    Private ReadOnly EmDash As Char = Convert.ToChar(8212) '—

    Public ReadOnly Property CSVText As String
    Public ReadOnly Property ErrorMessage As String
    Public ReadOnly Property HasHeaders As Boolean
    Public ReadOnly Property Separator As Char
    Public ReadOnly Property SourceFile As FileInfo
    Public ReadOnly Property ValuesTable As DataTable
    Public ReadOnly Property ValuesTableText As String

    Sub New(HasHeaders As Boolean, Optional CSVSeparator As Char = ",")
        _Separator = CSVSeparator
        _HasHeaders = HasHeaders
        _CSVText = ""
    End Sub
    Function LoadFile(CSVInput As String) As Boolean
        Try
            _SourceFile = New FileInfo(CSVInput)
        Catch ex As Exception
            _ErrorMessage = ex.ToString
            Return False
        End Try

        Return True
    End Function
    Function LoadText(CSVInput As String) As Boolean
        Try
            If CSVInput.Contains(_Separator) Then
                _CSVText = CSVInput
                Return True
            Else
                _ErrorMessage = "Input does not contain any instance of separator"
                Return False
            End If
        Catch ex As Exception
            _ErrorMessage = ex.ToString
            Return False
        End Try
    End Function
    Function ProcessCSV() As Boolean
        Dim Reader As StreamReader
        Dim CurrRow As String
        Dim CurrDataRow As DataRow
        Dim IsFirstRow As Boolean
        Dim Fields As String()
        Try
            _ValuesTable = New DataTable

            If _CSVText = "" Then
                Reader = New StreamReader(_SourceFile.FullName)
            Else
                Reader = New StreamReader(_CSVText)
            End If

            IsFirstRow = True
            While Not Reader.EndOfStream
                CurrRow = Reader.ReadLine
                Fields = CurrRow.Split(Separator)

                Dim CurrDataColumn As DataColumn

                If IsFirstRow Then
                    IsFirstRow = False
                    If _HasHeaders Then
                        For Each Header As String In CurrRow.Split(_Separator)
                            CurrDataColumn = _ValuesTable.Columns.Add(Header)
                            CurrDataColumn.ExtendedProperties.Add("TextTableWidth", Header.Length)
                        Next
                        Continue While
                    Else
                        For i = 0 To Fields.Count - 1
                            CurrDataColumn = _ValuesTable.Columns.Add("Columns " & i.ToString)
                            CurrDataColumn.ExtendedProperties.Add("TextTableWidth", ("Columns " & i.ToString).Length)
                        Next
                    End If
                End If

                CurrDataRow = _ValuesTable.NewRow
                For i = 0 To Fields.Count - 1
                    CurrDataRow(i) = Fields(i)
                    _ValuesTable.Columns(i).ExtendedProperties("TextTableWidth") = (Math.Max(Convert.ToInt16(_ValuesTable.Columns(i).ExtendedProperties("TextTableWidth")), Fields(i).Length)).ToString
                Next
                _ValuesTable.Rows.Add(CurrDataRow)
            End While

            Return True
        Catch ex As Exception
            _ErrorMessage = ex.ToString
            Return False
        End Try
    End Function
    Function TableToText(Optional MaxColumnWidth As Integer = -1) As Boolean
        Const ColumnSeparator As String = " | "

        'Const Omissis As String = "[…]"

        Dim RowSeparator As String
        Dim TextTable As Text.StringBuilder

        Dim ColumnLength As Dictionary(Of String, Integer)
        Dim CurrColumnLength As Integer

        'Dim TableCopy As DataTable

        Try
            If _ValuesTable Is Nothing Then
                _ErrorMessage = "Table is empty: call 'ProcessCSV' function before"
                Return False
            End If

            'TableCopy = _ValuesTable.Copy

            Dim ColumnLabel As String
            TextTable = New Text.StringBuilder
            RowSeparator = ""

            If -1 < MaxColumnWidth AndAlso MaxColumnWidth < 4 Then
                MaxColumnWidth = 4
            End If

            ColumnLength = New Dictionary(Of String, Integer)

            For Each CurrColumn As DataColumn In _ValuesTable.Columns
                ColumnLabel = CurrColumn.ColumnName

                CurrColumnLength = Convert.ToInt16(CurrColumn.ExtendedProperties("TextTableWidth"))
                ColumnLabel = TrimField(ColumnLabel, Math.Min(MaxColumnWidth, CurrColumnLength))
                RowSeparator &= "+".PadLeft(ColumnLabel.Length + ColumnSeparator.Length, EmDash)
                TextTable.Append(ColumnLabel & ColumnSeparator)
            Next

            With TextTable
                .AppendLine()
                .Append(RowSeparator.Substring(1))
                .AppendLine()

                Dim RowBuilder As Text.StringBuilder
                For Each CurrRow As DataRow In _ValuesTable.Rows
                    RowBuilder = New Text.StringBuilder

                    For Each CurrColumn As DataColumn In _ValuesTable.Columns
                        CurrColumnLength = Convert.ToInt16(CurrColumn.ExtendedProperties("TextTableWidth"))
                        With RowBuilder
                            .Append(TrimField(CurrRow.Item(CurrColumn).ToString, Math.Min(MaxColumnWidth, CurrColumnLength)))
                            .Append(ColumnSeparator)
                        End With
                    Next
                    .AppendLine(RowBuilder.ToString)
                Next
            End With

            _ValuesTableText = TextTable.ToString

            Return True
        Catch ex As Exception
            _ErrorMessage = ex.ToString
            Return False
        End Try
    End Function
    Function TrimField(FieldText As String, FieldLength As Integer) As String
        Dim Omissis As String = "[" & Ellipsis & "]"

        If FieldLength < (Omissis.Length + 1) Then
            FieldLength = Omissis.Length + 1
        End If

        If FieldText.Length > FieldLength Then
            Return FieldText.Substring(0, FieldLength - Omissis.Length) & Omissis
        Else
            Return FieldText.PadRight(FieldLength, " ")
        End If
    End Function
End Class
