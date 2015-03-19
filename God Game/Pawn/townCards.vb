Public Class townCards
    Private total As New List(Of townCard)
    Public ReadOnly Property Count As Integer
        Get
            Return total.Count
        End Get
    End Property
    Public Sub Add(towncard As card)
        total.Add(towncard)
    End Sub
    Public Sub Remove(towncard As card)
        total.Remove(towncard)
    End Sub
    Public Sub generateDefaultTownCards()
        total.Clear()

        For n = 1 To 3
            Dim card As New townCard
            With card
                .name = "Merchant"
                .buyItems = True
                .saleItems = 3
                .saleItemCost = 1
            End With
            total.Add(card)
        Next

        For n = 1 To 2
            Dim card As New townCard
            With card
                .name = "Faithful Servant"
                .gainPower = New range(1, 6)
            End With
            total.Add(card)
        Next

        For n = 1 To 2
            Dim card As New townCard
            With card
                .name = "Bargain Hunting"
                .buyItems = True
                .saleItems = 1
                .saleItemCost = 0.75
            End With
            total.Add(card)
        Next

        For n = 1 To 1
            Dim card As New townCard
            With card
                .name = "A Restful Night's Sleep"
                .restHealthBonus = New range(1, 6)
            End With
            total.Add(card)
        Next

        For n = 1 To 1
            Dim card As New townCard
            With card
                .name = "Thief, Thief!"
                .gainGold = New range(-6, -1)
            End With
            total.Add(card)
        Next

        For n = 1 To 1
            Dim card As New townCard
            With card
                .name = "Bar Brawl"
            End With
            total.Add(card)
        Next
    End Sub
    Public Function consoleReport(Optional space As Integer = 0) As String
        Dim x As Integer = space + 1
        Dim sum As String = ""
        Dim tallysheet As New Dictionary(Of townCard, Integer)
        For Each card As townCard In total
            Dim searchedCard As townCard = dictionarySearch(card, tallysheet)
            If searchedCard Is Nothing = True Then
                tallysheet.Add(card, 1)
            Else
                tallysheet.Item(searchedCard) += 1
            End If
        Next
        For Each kpv As KeyValuePair(Of townCard, Integer) In tallysheet
            Dim card As townCard = kpv.Key
            Dim count As Integer = kpv.Value
            sum &= vbSpace(x - 1) & "(x" & count & ") " & card.name & vbCrLf
            sum &= vbSpace(x) & card.consoleReport() & vbCrLf
        Next
        Return sum
    End Function
    Private Function dictionarySearch(ByVal targetCard As townCard, ByRef dictionary As Dictionary(Of townCard, Integer)) As townCard
        For Each kpv As KeyValuePair(Of townCard, Integer) In dictionary
            If kpv.Key.name = targetCard.name Then Return kpv.Key
        Next
        Return Nothing
    End Function
End Class