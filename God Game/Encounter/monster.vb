Public Class monster
    Friend name As String
    Friend dropRange As Integer
    Friend health As Integer
    Friend stats(2) As Integer
    Friend ReadOnly Property highestStat As stat
        Get
            Dim highest As Integer = 0
            Dim highestIndex As Integer = 0
            For n = 0 To 2
                If stats(n) >= highest Then
                    highest = stats(n)
                    highestIndex = n
                End If
            Next
            Return highestIndex
        End Get
    End Property
    Friend special As String
    Friend description As String


    Friend Sub New()
    End Sub
    Friend Sub New(ByRef monster As monster)
        name = monster.name
        dropRange = monster.dropRange
        health = monster.health
        For n As stat = 0 To 2
            stats(n) = monster.stats(n)
        Next
        special = monster.special
        description = monster.description
    End Sub

    Friend Shared monsters As New List(Of List(Of monster))
    Friend Shared Function generateMonsters() As List(Of List(Of monster))
        Dim monsterLists As New List(Of List(Of monster))
        For n As tier = 0 To 3
            Dim filename As String = "monsterTable" & n & ".csv"
            Dim csvFile As New csvFile(filename)

            Dim currentTierMonsters As New List(Of monster)
            For Each row As String() In csvFile.rows
                Dim monster As New monster
                With monster
                    .name = row(0)
                    .dropRange = commons.parseStatField(row(1))
                    .health = commons.parseStatField(row(2))
                    .stats(stat.Might) = commons.parseStatField(row(3))
                    .stats(stat.Cunning) = commons.parseStatField(row(4))
                    .stats(stat.Sorcery) = commons.parseStatField(row(5))
                    .special = row(6)
                    .description = row(7)
                End With
                currentTierMonsters.Add(monster)
            Next
            monsterLists.Add(currentTierMonsters)
        Next
        Return monsterLists
    End Function
    Friend Shared Function getRandomMonster(tier As tier, difficulty As Integer) As monster
        If monsters Is Nothing = True OrElse monsters.Count = 0 Then monsters = generateMonsters()
        Dim currentMonsters As List(Of monster) = monsters.Item(tier)
        Dim prunedMonsters As New List(Of monster)
        For Each monster In currentMonsters
            If monster.dropRange <= difficulty Then
                prunedMonsters.Add(monster)
            Else
                Exit For
            End If
        Next

        Dim roll As Integer = rng.Next(prunedMonsters.Count)
        Return prunedMonsters.Item(roll)
    End Function
End Class
