Public Class loot
    'integer holds the rarity of the drop
    'items with a higher rarity are rarer
    'lootTable(0) will refer to easy, (1) to moderate etc.
    Friend Shared lootTables As List(Of Dictionary(Of loot, Integer))
    Friend Shared Function generateLootTables() As List(Of Dictionary(Of loot, Integer))
        Dim pLootTables As New List(Of Dictionary(Of loot, Integer))

        For n = 0 To 3
            Dim pDictionary As New Dictionary(Of loot, Integer)

            Dim filename As String = "lootTable" & n & ".csv"
            Dim lootCSV As New csvFile(filename)
            For Each row In lootCSV.rows
                Dim keyword As String = row(3)
                If keyword.ToLower = "gold" Then
                    'format: 30-50
                    Dim str As String = row(1)
                    Dim dropRange As Integer = Convert.ToInt32(row(2))

                    Dim dashIndex As Integer = str.IndexOf("-")
                    Dim minGold As Integer = Convert.ToInt32(str.Substring(0, dashIndex))
                    Dim maxGold As Integer = Convert.ToInt32(str.Substring(dashIndex + 1, str.Length - dashIndex - 1))

                    Dim treasure As New treasure(minGold, maxGold)
                    pDictionary.Add(treasure, dropRange)
                ElseIf keyword.ToLower = "opportunity" Then
                    Dim name As String = row(0)
                    Dim skill As stat = Nothing
                    Select Case row(1).ToLower
                        Case "might" : skill = God_Game.stat.Might
                        Case "cunning" : skill = God_Game.stat.Cunning
                        Case "sorcery" : skill = God_Game.stat.Sorcery
                        Case Else : skill = rng.Next(0, 4)
                    End Select
                    Dim dropRange As Integer = commons.parseStatField(row(2))
                    Dim description As String = row(16)

                    Dim oppCardLoot As New oppCardLoot(name, skill, description)
                    pDictionary.Add(oppCardLoot, dropRange)
                Else
                    Dim name As String = row(0)
                    Dim cost As Integer = commons.parseStatField(row(1))
                    Dim dropRange As Integer = commons.parseStatField(row(2))
                    Dim fMight As Integer = commons.parseStatField(row(4))
                    Dim fCunning As Integer = commons.parseStatField(row(5))
                    Dim fSorcery As Integer = commons.parseStatField(row(6))
                    Dim aMight As Integer = commons.parseStatField(row(7))
                    Dim aCunning As Integer = commons.parseStatField(row(8))
                    Dim aSorcery As Integer = commons.parseStatField(row(9))
                    Dim dMight As Integer = commons.parseStatField(row(10))
                    Dim dCunning As Integer = commons.parseStatField(row(11))
                    Dim dSorcery As Integer = commons.parseStatField(row(12))
                    Dim sMight As Integer = commons.parseStatField(row(13))
                    Dim sCunning As Integer = commons.parseStatField(row(14))
                    Dim sSorcery As Integer = commons.parseStatField(row(15))
                    Dim description As String = row(16)
                    Dim special As String = row(17)

                    Dim item As New item(name, cost, keyword, description, _
                                         {fMight, fCunning, fSorcery}, _
                                         {aMight, aCunning, aSorcery}, _
                                         {dMight, dCunning, dSorcery}, _
                                         {sMight, sCunning, sSorcery})
                    pDictionary.Add(item, dropRange)
                End If
            Next

            pLootTables.Add(pDictionary)
        Next

        Return pLootTables
    End Function

    Friend Shared Function dropLoot(monster As monster, tier As tier, dropRangeModifier As Integer, dropRollModifier As Integer) As loot
        If lootTables Is Nothing = True OrElse lootTables.Count = 0 Then lootTables = generateLootTables()
        Dim lootTable As Dictionary(Of loot, Integer) = lootTables.Item(tier)

        Dim dropRange As Integer = monster.dropRange + dropRangeModifier
        Dim drops As New List(Of loot)
        For Each kvp As KeyValuePair(Of loot, Integer) In lootTable
            If kvp.Value <= dropRange Then
                drops.Add(kvp.Key)
            Else
                Exit For
            End If
        Next

        'higher rolls are better as rarer items are added to the list later
        'every item has an equal chance of dropping
        Dim dropRoll As Integer = rng.Next(drops.Count) + dropRollModifier
        dropRoll = constrain(dropRoll, 0, drops.Count - 1)
        Dim drop As loot = drops.Item(dropRoll)
        dropLoot = drop

        'remove item from lootTable
        'comment out if loot should be repeatable
        If TypeOf (drop) Is item Then lootTable.Remove(drops.Item(dropRoll))
    End Function
End Class