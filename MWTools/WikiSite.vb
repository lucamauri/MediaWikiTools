Friend Class WikiSite
    Public ReadOnly Property Fields As String()
    Public ReadOnly Property GlobalID As String
    Public ReadOnly Property SiteFiedls As SiteField()
    Sub New(TextFields As String())
        _Fields = TextFields
        _GlobalID = TextFields(0)
    End Sub
End Class
Friend Class SiteField
    Friend Enum FieldTypeEnum
        SitesOnly = 0
        InterWikiOnly = 1
        SitesAndInterWiki = 2
    End Enum

    Public Property Type As FieldTypeEnum
    Public ReadOnly Property SitesName As String
    Public ReadOnly Property InterWikiName As String
    Public ReadOnly Property Value As String

    Sub New(Fieldtype As FieldTypeEnum, FieldName As String, FieldValue As String)
        _Type = Fieldtype
        Select Case Fieldtype
            Case FieldTypeEnum.SitesOnly
                _SitesName = FieldName
            Case FieldTypeEnum.InterWikiOnly
                _InterWikiName = FieldName
            Case Else
                Dim BothNames As String()

                BothNames = FieldName.Split("/")
                _SitesName = BothNames(0).Trim
                _InterWikiName = BothNames(1).Trim
        End Select

        _Value = FieldValue
    End Sub
End Class
