Public Class commons
    Friend Shared Function parseStatField(str As String) As Integer
        If str = "" Then
            Return 0
        Else
            If IsNumeric(str) = True Then Return Convert.ToInt32(str) Else Return 0
        End If
    End Function
    Friend Shared Function parseSpecialField(str As String) As KeyValuePair(Of monsterSpecial, Integer)
        If str = "" Then Return Nothing
        Select Case str
            Case "Immune:Might" : Return New KeyValuePair(Of monsterSpecial, Integer)(monsterSpecial.Immune, stat.Might)
            Case "Immune:Cunning" : Return New KeyValuePair(Of monsterSpecial, Integer)(monsterSpecial.Immune, stat.Cunning)
            Case "Immune:Sorcery" : Return New KeyValuePair(Of monsterSpecial, Integer)(monsterSpecial.Immune, stat.Sorcery)
            Case Else
                If str.Contains(" ") = False Then Return Nothing
                Dim index As Integer = str.IndexOf(" ")
                Dim firstHalf As String = str.Substring(0, index)
                Dim secondHalf As String = str.Substring(index + 1, str.Length - index - 1)
                Try
                    Dim key As monsterSpecial = CInt([Enum].Parse(GetType(monsterSpecial), firstHalf))
                    Dim value As Integer = Convert.ToInt32(secondHalf)
                    If key = 0 Then Return Nothing Else Return New KeyValuePair(Of monsterSpecial, Integer)(key, value)
                Catch ex As Exception
                    Return Nothing
                End Try
        End Select
    End Function
End Class
