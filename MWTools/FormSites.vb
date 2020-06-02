Imports System.Xml

Public Class FormSites
    Public Const XMLHeader As String = "<?xml version='1.0' encoding='UTF-8'?>"

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim SitesFile As XDocument
        Dim Row As String

        Dim WikiSite As WikiSite
        Dim WikiSites As New List(Of WikiSite)

        'Dim Reader = New System.IO.StreamReader("d:\Users\luca\Documents\Dropbox\Docs\WikiTrek\Memory\DataTrek\masterSites.csv")
        'Dim Reader = New System.IO.StreamReader("d:\Users\luca\Documents\Dropbox\Docs\WikiTrek\DO2020\masterSites.csv")
        Dim Reader = New System.IO.StreamReader("H:\pCloud\Docs\WikiTrek\DO2020\masterSites.csv")

        Try
            While Not Reader.EndOfStream
                Row = Reader.ReadLine
                'Console.WriteLine(Row)

                WikiSite = New WikiSite(Row.Split("|"))
                'Console.WriteLine(WikiSite.Fields.Count)
                WikiSites.Add(WikiSite)
            End While

            Dim IsFirstRow As Boolean = True
            Dim TableSites As DataTable
            Dim RowSite As DataRow

            For Each CurrSite As WikiSite In WikiSites
                If IsFirstRow Then
                    TableSites = New DataTable

                    With TableSites
                        For Each Field As String In CurrSite.Fields
                            .Columns.Add(New DataColumn(Field))
                        Next
                    End With

                    IsFirstRow = False
                Else
                    RowSite = TableSites.NewRow
                    RowSite.ItemArray = CurrSite.Fields
                    TableSites.Rows.Add(RowSite)
                End If
            Next

            WikiSites.Remove(WikiSites.Item(0))

            With GridSource
                .AutoGenerateColumns = True
                .MultiSelect = False
                '.DataSource = WikiSites
                .DataSource = TableSites
                For Each Column As DataGridViewColumn In .Columns
                    .AutoResizeColumn(Column.Index)
                Next
            End With

            BoxQuery.Text = MakeQueries(WikiSites)
            BoxSites.Text = MakeSitesXML(WikiSites)
            BoxLUA.Text = MakeLUACode(WikiSites)
        Catch ex As Exception
        Finally
            Reader.Close()
        End Try

    End Sub

    Private Function MakeLUACode(Allsites As List(Of WikiSite)) As String
        '    local Titles = {
        '    wikitrek = 'WikiTrek',
        '    datatrek = 'DataTrek',
        '    enma = 'Memory Alpha (inlgese)',
        '    itma = 'Memory Alpha (italiano)',
        '    enmb = 'Memory Beta (inlgese)',
        '    sto = 'Star Trek Online wiki',
        '    enwiki = 'Wikipedia (inglese)',
        '    itwiki = 'Wikipedia (inglese)'
        '}

        Const OpenCode As String = "local VariableName = {"
        Const CloseCode As String = "}"

        Dim Members As New Text.StringBuilder

        Members.AppendLine(OpenCode)
        For Each CurrSite As WikiSite In Allsites
            With Members
                .Append(Convert.ToChar(Keys.Tab))
                .Append(CurrSite.Fields(0))
                .Append(" = '")
                .Append(CurrSite.Fields(6))
                .AppendLine("',")
            End With
        Next
        Members.AppendLine(CloseCode)

        Return Members.ToString
    End Function

    Private Function MakeQueries(AllSites As List(Of WikiSite)) As String
        'Esempio
        'INSERT INTO interwiki (iw_prefix, iw_url, iw_api, iw_wikiid, iw_local, iw_trans) VALUES ("sto", "https://sto.gamepedia.com/$1", "https://sto.gamepedia.com/api.php", '', 0, 0);
        'INSERT INTO interwiki (iw_prefix, iw_url, iw_api, iw_wikiid, iw_local, iw_trans) VALUES ('sto', 'https://sto.gamepedia.com/$1', 'https://sto.gamepedia.com/api.php', '', 0, 0);

        Const StatementStart As String = "INSERT INTO interwiki (iw_prefix, iw_url, iw_api, iw_wikiid, iw_local, iw_trans) VALUES ("
        Const StatementEnd As String = "'', 0, 0);"
        Const OpenString As String = "'"
        Const CloseStringComma As String = "', "

        Dim Statement As System.Text.StringBuilder
        Dim Statements As New System.Text.StringBuilder

        For Each CurrSite As WikiSite In AllSites
            Statement = New System.Text.StringBuilder(StatementStart)
            With Statement
                .Append(OpenString & CurrSite.Fields(0) & CloseStringComma)
                .Append(OpenString & CurrSite.Fields(2) & CloseStringComma)
                .Append(OpenString & CurrSite.Fields(3) & CloseStringComma)
                .Append(StatementEnd)
            End With
            Statements.AppendLine(Statement.ToString)
        Next

        Return Statements.ToString
    End Function
    Private Function MakeSitesXML(AllSites As List(Of WikiSite)) As String
        Dim SitesXML As New XmlDocument
        Dim SitesDeclaration As XmlDeclaration
        Dim SitesDocument As New XDocument
        Dim SiteElement As XmlElement
        Dim ChildElement As XmlElement


        Try
            SitesDeclaration = SitesXML.CreateXmlDeclaration("1.0", Nothing, Nothing)

            SiteElement = SitesXML.CreateElement("sites")
            With SiteElement
                .SetAttribute("xmlns", "http://www.mediawiki.org/xml/sitelist-1.0/")
                .SetAttribute("version", "1.0")
            End With
            SitesXML.AppendChild(SiteElement)

        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

        For Each CurrSite As WikiSite In AllSites
            SiteElement = SitesXML.CreateElement("site")
            With SiteElement
                .SetAttribute("type", "mediawiki")
                Try
                    For i = 0 To CurrSite.Fields.Length - 1
                        Select Case i
                            Case 0
                                ChildElement = SitesXML.CreateElement("globalid")
                            Case 1
                                ChildElement = SitesXML.CreateElement("group")
                            Case 5
                                ChildElement = SitesXML.CreateElement("languagecode")
                            Case 4
                                ChildElement = SitesXML.CreateElement("path")
                                ChildElement.SetAttribute("type", "file_path")
                            Case 2
                                ChildElement = SitesXML.CreateElement("path")
                                ChildElement.SetAttribute("type", "page_path")
                            Case Else
                                Continue For
                        End Select
                        ChildElement.InnerText = CurrSite.Fields(i)
                        .AppendChild(ChildElement)
                    Next
                Catch ex As Exception
                    MessageBox.Show(ex.ToString)
                End Try
            End With
            SitesXML.FirstChild.AppendChild(SiteElement)
        Next
        SitesXML.InsertBefore(SitesDeclaration, SitesXML.DocumentElement)

        SitesDocument = XDocument.Parse(SitesXML.InnerXml)
        Return SitesDeclaration.OuterXml & Environment.NewLine & SitesDocument.ToString

    End Function

    Private Sub Boxes_DoubleClick(sender As Object, e As EventArgs) Handles BoxSites.DoubleClick, BoxLUA.DoubleClick, BoxQuery.DoubleClick
        Dim Box As TextBox = CType(sender, TextBox)

        Box.SelectAll()
        Clipboard.SetText(Box.SelectedText)

        MessageBox.Show("Text copied from " & Box.Name)
    End Sub

    Private Sub BTNTabler_Click(sender As Object, e As EventArgs) Handles BTNTabler.Click
        Dim Tabler As CSVTabler.CSVTabler

        Tabler = New CSVTabler.CSVTabler(False, "|")
        If Tabler.LoadFile("H:\pCloud\Docs\WikiTrek\DO2020\masterSitesTest.csv") AndAlso Tabler.ProcessCSV Then
            With GridSource
                .AutoGenerateColumns = True
                .MultiSelect = False
                .DataSource = Tabler.ValuesTable
                For Each Column As DataGridViewColumn In .Columns
                    .AutoResizeColumn(Column.Index)
                Next
            End With
        Else
            MessageBox.Show(Tabler.ErrorMessage)
        End If


    End Sub
End Class