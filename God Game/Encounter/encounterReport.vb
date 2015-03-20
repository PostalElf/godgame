Friend Class encounterReport
    Private Shared reportlist As New Queue(Of encounterReport)
    Friend encounter As encounter
    Friend encounterReportType As encounterReportType

    Friend Sub report()
        Select Case encounterReportType
            Case God_Game.encounterReportType.ChooseStats
                Dim pawn As combatant = encounter.tPawn
                Dim monster As combatant = encounter.tMonster
                Console.WriteLine(pawn.name & " is attacking with " & pawn.attackStat.ToString & " & defending with " & monster.attackStat.ToString & ".")
                Console.WriteLine(monster.name & " is attacking with " & monster.attackStat.ToString & " & defending with " & pawn.attackStat.ToString & ".")
                consolePause(1000)
                Console.WriteLine()


            Case God_Game.encounterReportType.Attack
                Dim attackTotal As Integer = attackDice(0) + attackDice(1) + attacker.attackValue
                Dim defenceTotal As Integer = 7 + defender.defenceValue
                Console.ForegroundColor = ConsoleColor.DarkGray
                Console.Write(attacker.name & " rolled " & attackDice(0) & " & " & attackDice(1) & "!  ")
                Console.WriteLine("Total: " & attackDice(0) + attackDice(1) & " + " & attacker.attackValue & " = " & attackTotal)
                Console.WriteLine(defender.name & " has a defence of " & defenceTotal & ".")
                Console.WriteLine()
                If damage > 0 Then
                    'hit
                    Console.ForegroundColor = ConsoleColor.DarkRed
                    Console.WriteLine(vbSpace() & attacker.name & " hits " & defender.name & " for " & damage & " damage!")
                    Console.WriteLine(vbSpace() & "Remaining Health: " & defender.health)
                    Console.WriteLine()
                Else
                    'miss
                    Console.WriteLine(vbSpace() & attacker.name & " misses!" & vbCrLf)
                    Console.WriteLine()
                End If
                If defender.health <= 0 Then
                    Console.WriteLine(vbSpace() & defender.name & " has been defeated!")
                    Console.WriteLine()
                    Console.WriteLine()
                End If
                Console.ForegroundColor = ConsoleColor.DarkGray
        End Select
    End Sub
    Friend Shared Sub Dequeue()
        If reportlist.Peek Is Nothing = False Then reportlist.Dequeue.report()
    End Sub

#Region "Choose Stats"
    Public Sub New(_encounter As encounter)
        encounter = _encounter
        encounterReportType = God_Game.encounterReportType.ChooseStats

        reportlist.Enqueue(Me)
    End Sub
#End Region
#Region "Attack"
    Friend attackDice() As Integer
    Friend damage As Integer
    Friend attacker As combatant
    Friend defender As combatant
    Public Sub New(ByRef _encounter As encounter, ByRef _attacker As combatant, ByRef _defender As combatant, _attackDice() As Integer, _damage As Integer)
        encounter = _encounter
        encounterReportType = God_Game.encounterReportType.Attack

        attacker = _attacker
        defender = _defender
        attackDice = _attackDice
        damage = _damage

        reportlist.Enqueue(Me)
    End Sub
#End Region
#Region "End Encounter"
    Friend loot As loot
    Public Sub New(_encounter As encounter, _loot As loot)
        encounter = _encounter
        encounterReportType = God_Game.encounterReportType.EndCombat

        loot = _loot
    End Sub
#End Region

End Class
