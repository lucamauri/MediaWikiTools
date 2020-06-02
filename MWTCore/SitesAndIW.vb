Imports System.Data
Imports System.Xml
Public Class SitesAndIW
    Public ReadOnly Property AllSitesTable As DataTable
    Public ReadOnly Property LastError As String
    Public Property LUACode As String
    Public Property Queries As String
    Public Property SitesXML As String

    Sub New(AllSites As DataTable)
        _AllSitesTable = AllSites
    End Sub
    Function BuildTexts() As Boolean
        Return MakeLUACode() And MakeQueries() And MakeSitesXML()
    End Function
    Private Function MakeLUACode() As Boolean
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

        Try
            Const OpenCode As String = "local VariableName = {"
            Const CloseCode As String = "}"

            Dim Members As New Text.StringBuilder
            Dim IsFirstLine As Boolean

            IsFirstLine = True
            Members.AppendLine(OpenCode)
            For Each CurrSite As DataRow In _AllSitesTable.Rows
                With Members
                    If Not IsFirstLine Then
                        .AppendLine(",")
                    End If
                    .Append("    ")
                    .Append(CurrSite("globalid / iw_prefix").ToString)
                    .Append(" = '")
                    .Append(CurrSite("Descrizione").ToString)
                    .Append("'")
                End With
                IsFirstLine = False
            Next
            Members.AppendLine()
            Members.AppendLine(CloseCode)

            _LUACode = Members.ToString

            Return True
        Catch ex As Exception
            _LastError = ex.ToString
            Return False
        End Try

    End Function

    Private Function MakeQueries() As Boolean
        'Esempio
        'INSERT INTO interwiki (iw_prefix, iw_url, iw_api, iw_wikiid, iw_local, iw_trans) VALUES ("sto", "https://sto.gamepedia.com/$1", "https://sto.gamepedia.com/api.php", '', 0, 0);
        'INSERT INTO interwiki (iw_prefix, iw_url, iw_api, iw_wikiid, iw_local, iw_trans) VALUES ('sto', 'https://sto.gamepedia.com/$1', 'https://sto.gamepedia.com/api.php', '', 0, 0);
        Try
            Const StatementStart As String = "INSERT INTO interwiki (iw_prefix, iw_url, iw_api, iw_wikiid, iw_local, iw_trans) VALUES ("
            Const StatementEnd As String = "'', 0, 0);"
            Const OpenString As String = "'"
            Const CloseStringComma As String = "', "

            Dim Statement As System.Text.StringBuilder
            Dim Statements As New System.Text.StringBuilder

            For Each CurrSite As DataRow In _AllSitesTable.Rows
                Statement = New System.Text.StringBuilder(StatementStart)
                With Statement
                    .Append(OpenString & CurrSite("globalid / iw_prefix").ToString & CloseStringComma)
                    .Append(OpenString & CurrSite("page_path / iw_url").ToString & CloseStringComma)
                    .Append(OpenString & CurrSite("iw_api").ToString & CloseStringComma)
                    .Append(StatementEnd)
                End With
                Statements.AppendLine(Statement.ToString)
            Next

            _Queries = Statements.ToString
            Return True
        Catch ex As Exception
            _LastError = ex.ToString
            Return False
        End Try
    End Function
    Private Function MakeSitesXML() As String
        Dim SitesXML As New XmlDocument
        Dim SitesDeclaration As XmlDeclaration
        Dim SitesDocument As New XDocument
        Dim SiteElement As XmlElement
        Dim ChildElement As XmlElement
        'Dim ColumnToTag As New Dictionary(Of String, String)

        'With ColumnToTag
        '    .Add("globalid / iw_prefix", "globalid")
        '    .Add("group", "group")
        '    .Add("languagecode", "languagecode")
        '    .Add("file_path", "file_path")
        '    .Add("page_path / iw_url", "page_path")
        'End With

        Try
            SitesDeclaration = SitesXML.CreateXmlDeclaration("1.0", Nothing, Nothing)

            SiteElement = SitesXML.CreateElement("sites")
            With SiteElement
                .SetAttribute("xmlns", "http://www.mediawiki.org/xml/sitelist-1.0/")
                .SetAttribute("version", "1.0")
            End With
            SitesXML.AppendChild(SiteElement)

        Catch ex As Exception
            Return False
            _LastError = ex.ToString
        End Try

        For Each CurrSite As DataRow In _AllSitesTable.Rows
            SiteElement = SitesXML.CreateElement("site")
            With SiteElement
                .SetAttribute("type", "mediawiki")
                Try
                    For Each CurrColumn As DataColumn In _AllSitesTable.Columns
                        Select Case CurrColumn.ColumnName
                            Case "globalid / iw_prefix"
                                ChildElement = SitesXML.CreateElement("globalid")
                            Case "group"
                                ChildElement = SitesXML.CreateElement("group")
                            Case "languagecode"
                                ChildElement = SitesXML.CreateElement("languagecode")
                            Case "file_path"
                                ChildElement = SitesXML.CreateElement("path")
                                ChildElement.SetAttribute("type", "file_path")
                            Case "page_path / iw_url"
                                ChildElement = SitesXML.CreateElement("path")
                                ChildElement.SetAttribute("type", "page_path")
                            Case Else
                                Continue For
                        End Select
                        ChildElement.InnerText = CurrSite(CurrColumn.ColumnName)
                        .AppendChild(ChildElement)
                    Next
                Catch ex As Exception
                    Return False
                    _LastError = ex.ToString
                End Try
            End With
            SitesXML.FirstChild.AppendChild(SiteElement)
        Next
        SitesXML.InsertBefore(SitesDeclaration, SitesXML.DocumentElement)

        SitesDocument = XDocument.Parse(SitesXML.InnerXml)
        _SitesXML = SitesDeclaration.OuterXml & Environment.NewLine & SitesDocument.ToString

        Return True

    End Function
End Class
