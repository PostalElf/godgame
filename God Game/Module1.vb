Module Module1

    Sub Main()
        Console.SetWindowSize(100, 50)
        Console.ForegroundColor = ConsoleColor.DarkGray

        Dim str As String = "Badvalue 2"
        Dim kvp As KeyValuePair(Of monsterSpecial, Integer) = commons.parseSpecialField(str)
        Console.WriteLine(kvp.Key.ToString)
        Console.WriteLine(kvp.Value)
        Console.ReadKey()

        'Dim tPlayer As New player
        'world.players.Add(tPlayer)
        'Dim tPawn As pawn = tPlayer.spawnPawn()

        ''cheats
        'tPawn.inventory.Add(New item("Junk", 100, "Junk", ""))
        'tPawn.equipment.Add(New item("Blazing Sword", 100, "Weapon", "A blazing sword", Nothing, {10, 0, 0}, Nothing, Nothing))

        'For n = 1 To 20
        '    testCombat(tPawn)
        '    Console.ReadKey()

        '    Console.Clear()
        '    Console.WriteLine(tPawn.consoleReport)
        '    Console.ReadKey()
        'Next
    End Sub
    Function getMenuKey(min As Integer, max As Integer) As Integer
        While True
            Dim input As ConsoleKeyInfo = Console.ReadKey
            Dim inputChar As String = input.KeyChar
            If IsNumeric(inputChar) = True Then
                Dim inputNum As Integer = Convert.ToInt32(inputChar)
                If inputNum >= min AndAlso inputNum <= max Then Return inputNum
            End If
        End While
        Return Nothing
    End Function
    Function getMenuKeyBoolean() As Boolean
        Dim input As ConsoleKeyInfo = Console.ReadKey
        Dim inputChar As String = input.KeyChar.ToString.ToLower
        If inputChar = "y" Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub testCombat(tPawn As pawn)
        Dim difficulty As Integer = 20
        Dim encounter As New encounter(tPawn, tier.Easy, difficulty)
        Dim tMonster As monster = encounter.monsterBase
        Console.Clear()
        Console.WriteLine(tPawn.name & " is fighting " & tMonster.name & "!")
        Console.WriteLine("Choose a stat:")
        Console.WriteLine(vbSpace() & "1. Might   (" & tPawn.statRange(stat.Might) & ")")
        Console.WriteLine(vbSpace() & "2. Cunning (" & tPawn.statRange(stat.Cunning) & ")")
        Console.WriteLine(vbSpace() & "3. Sorcery (" & tPawn.statRange(stat.Sorcery) & ")")
        Dim chosenAttack As stat = getMenuKey(1, 3) - 1
        Console.WriteLine()
        encounter.chooseStats(tMonster.highestStat, chosenAttack)
        encounterReport.Dequeue()

        While encounter.isOver = False
            encounter.tick()
            consolePause(1000)
        End While

        Dim loot As loot = encounter.endEncounter
        Dim foundItem As KeyValuePair(Of loot, String) = tPawn.getLoot(loot)
        Console.Write(foundItem.Value)
        If foundItem.Key Is Nothing = False Then
            Dim foundItemCast As item = CType(foundItem.Key, item)
            If tPawn.equipment.searchKeyword(foundItemCast.keyword) Is Nothing = False Then
                'has existing equipment
                Console.ForegroundColor = ConsoleColor.White
                Console.WriteLine(tPawn.name & " already has a " & foundItemCast.keyword.ToString & " equipped.")
                Console.WriteLine(tPawn.equipment.searchKeyword(foundItemCast.keyword).consoleReport())
                Console.Write("Do you want to replace it? ")
                If getMenuKeyBoolean() = True Then
                    'replace equipment
                    Console.WriteLine()
                    Console.WriteLine(tPawn.name & " equips " & foundItemCast.name & ".")
                    managePawnInventory(tPawn, tPawn.equipment.Add(foundItemCast))
                Else
                    'do not replace equipment; add to bags
                    Console.WriteLine()
                    managePawnInventory(tPawn, foundItemCast)
                End If
            Else
                'does not have existing equipment; autoequip
                Console.WriteLine()
                Console.WriteLine(tPawn.name & " equips the " & foundItemCast.name & ".")
                tPawn.equipment.Add(foundItemCast)
            End If
            Console.ForegroundColor = ConsoleColor.DarkGray
        End If
    End Sub
    Private Sub managePawnInventory(ByRef pawn As pawn, ByVal newItem As item)
        While newItem Is Nothing = False
            If pawn.inventory.hasSpace = True Then
                'has space
                Console.WriteLine(pawn.name & " adds " & newItem.name & " to his bags.")
                pawn.inventory.Add(New item(newItem))
                newItem = Nothing
            Else
                'no space
                Console.WriteLine()
                Console.WriteLine(pawn.name & " does not have enough space in his bags for " & newItem.name)
                Console.WriteLine("Pick an item to discard:")
                Console.Write(pawn.inventory.consoleReportMenu)
                Console.WriteLine(pawn.inventory.Count + 1 & ". " & newItem.consoleReport)
                Dim input As Integer = getMenuKey(1, pawn.inventory.Count + 1) - 1
                If input = pawn.inventory.Count Then
                    'discard newItem
                    Console.WriteLine()
                    Console.WriteLine(newItem.name & " has been discarded.")
                    newItem = Nothing
                Else
                    Console.WriteLine()
                    Console.WriteLine(pawn.inventory.Item(input).name & " has been discarded.")
                    pawn.inventory.RemoveAt(input)
                    pawn.inventory.Add(New item(newItem))
                    newItem = Nothing
                End If
            End If
        End While
    End Sub
End Module
