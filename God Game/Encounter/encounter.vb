﻿Public Class encounter
    Friend monsterBase As monster
    Friend tMonster As combatant
    Friend pawnBase As pawn
    Friend tPawn As combatant
    Friend tier As Integer
    Friend difficulty As Integer
    Friend dropRangeModifier As Integer
    Friend dropRollModifier As Integer
    Friend ReadOnly Property isOver As Boolean
        Get
            If tMonster.health <= 0 OrElse tPawn.health <= 0 Then Return True Else Return False
        End Get
    End Property

    Public Sub New(_monster As monster, _pawn As pawn, _difficulty As Integer)
        monsterBase = _monster
        tMonster = New combatant(monsterBase)
        pawnBase = _pawn
        tPawn = New combatant(pawnBase)
        difficulty = _difficulty
    End Sub
    Public Sub New(_pawn As pawn, _tier As tier, _difficulty As Integer, Optional _dropRangeModifier As Integer = 0, Optional _dropRollModifier As Integer = 0)
        monsterBase = monster.getRandomMonster(_tier, _difficulty)
        tMonster = New combatant(monsterBase)
        pawnBase = _pawn
        tPawn = New combatant(pawnBase)

        tier = _tier
        difficulty = _difficulty
        dropRangeModifier = _dropRangeModifier
        dropRollModifier = _dropRollModifier
    End Sub

    Friend Function chooseStats(_monsterAttack As stat, _pawnAttack As stat) As encounterReport
        tMonster.attackStat = _monsterAttack
        tPawn.defenceStat = _monsterAttack
        tPawn.attackStat = _pawnAttack
        tMonster.defenceStat = _pawnAttack

        Return New encounterReport(Me)
    End Function
    Friend Sub tick()
        If tMonster.defenceValue > tPawn.defenceValue Then
            performAttack(tMonster, tPawn)
            encounterReport.Dequeue()
            If isOver = False Then
                performAttack(tPawn, tMonster)
                encounterReport.Dequeue()
            End If
        Else
            performAttack(tPawn, tMonster)
            encounterReport.Dequeue()
            If isOver = False Then
                performAttack(tMonster, tPawn)
                encounterReport.Dequeue()
            End If
        End If
    End Sub
    Private Function performAttack(attacker As combatant, defender As combatant) As encounterReport
        If isOver = True Then Return Nothing

        'roll
        Dim attackDice(1) As Integer
        For n = 0 To 1
            attackDice(n) = rng.Next(1, 7)
        Next
        Dim attackTotal As Integer = attackDice(0) + attackDice(1) + attacker.attackValue
        Dim defenceTotal As Integer = 7 + defender.defenceValue
        Dim rawDamage As Integer = attackTotal - defenceTotal
        Dim damage As Integer = constrain(rawDamage, 1, attacker.attackValue)


        'check special for monster
        Dim originalAttackDice() As Integer = attackDice
        If attacker.parentMonster Is Nothing = False Then
            Dim special As KeyValuePair(Of monsterSpecial, Integer) = attacker.parentMonster.special
            If IsNothing(special) = False Then
                Select Case special.Key
                    Case monsterSpecial.Attack : attackTotal += special.Value
                    Case monsterSpecial.Damage : damage += special.Value
                    Case monsterSpecial.Vampiric : If damage > 0 Then attacker.health += constrain(damage, 0, special.Value)
                    Case monsterSpecial.Weaken : If rollResistance(special.Value) = True Then defender.parentPawn.getStatus(status.Weakened)
                    Case monsterSpecial.Slow : If rollResistance(special.Value) = True Then defender.parentPawn.getStatus(status.Slowed)
                    Case monsterSpecial.Poison : If rollResistance(special.Value) = True Then defender.parentPawn.getStatus(status.Poisoned)
                    Case monsterSpecial.Reroll
                        For n = 0 To 1
                            If attackDice(n) <= special.Value Then attackDice(n) = rng.Next(1, 7)
                        Next
                        attackTotal = attackDice(0) + attackDice(1) + attacker.attackValue
                End Select
            End If
        End If
        If defender.parentMonster Is Nothing = False Then
            Dim special As KeyValuePair(Of monsterSpecial, Integer) = defender.parentMonster.special
            If IsNothing(special) = False Then
                Select Case special.Key
                    Case monsterSpecial.Defence : defenceTotal += special.Value
                    Case monsterSpecial.Regenerate : If damage <= 0 Then defender.health = constrain(defender.health + special.Value, 0, defender.parentMonster.health)
                End Select
            End If
        End If


        'check if hit
        If damage > 0 Then
            defender.health = constrain(defender.health - damage, 0, 100)
            Return New encounterReport(Me, attacker, defender, attackDice, damage)
        Else
            Return New encounterReport(Me, attacker, defender, attackDice, 0)
        End If
    End Function
    Friend Function endEncounter() As loot
        If isOver = False Then Return Nothing

        If tPawn.health <= 0 Then
            'pawn lost
            pawnBase.player.pawns.Remove(pawnBase)
            pawnBase = Nothing
            tMonster = Nothing
            Return Nothing
        Else
            'pawn wins
            'loot roll is always modified by difficulty / 3; this should help mitigate bad loot drops from high difficulty mobs
            tPawn.health = pawnBase.health
            Dim loot As loot = loot.dropLoot(monsterBase, tier, dropRangeModifier, dropRollModifier + (difficulty / 3))
            Return loot
        End If
    End Function

    Private Function rollResistance(difficulty As Integer) As Boolean
        If rng.Next(1, 7) + rng.Next(1, 7) >= difficulty Then Return True Else Return False
    End Function
End Class

Public Class combatant
    Friend parentMonster As monster
    Friend parentPawn As pawn
    Friend name As String
    Friend stats(2) As Integer
    Friend health As Integer
    Friend attackStat As stat
    Friend ReadOnly Property attackValue As Integer
        Get
            If parentPawn Is Nothing = True Then
                Return stats(attackStat)
            Else
                Return stats(attackStat) + parentPawn.equipment.statBonuses(statType.Attack, attackStat)
            End If
        End Get
    End Property
    Friend defenceStat As stat
    Friend ReadOnly Property defenceValue As Integer
        Get
            If parentPawn Is Nothing = True Then
                Return stats(defenceStat)
            Else
                Return stats(defenceStat) + parentPawn.equipment.statBonuses(statType.Defence, defenceStat)
            End If
        End Get
    End Property

    Public Sub New(monster As monster)
        parentMonster = monster
        parentPawn = Nothing
        name = monster.name
        stats = monster.stats
        health = monster.health
    End Sub
    Public Sub New(pawn As pawn)
        parentMonster = Nothing
        parentPawn = pawn
        name = pawn.name
        For n As stat = 0 To 2
            stats(n) = pawn.moddedStats(n)
        Next
        health = pawn.health
    End Sub
End Class