Public Class pawn
    Public player As player
    Public name As String
    Public epithet As String
    Public ReadOnly Property fullname As String
        Get
            Return name & " " & epithet
        End Get
    End Property
    Public age As Integer
    Public health As Integer
    Public healthMax As Integer
    Public stats(2) As Integer
    Public ReadOnly Property statBonus(statType As statType, stat As stat) As Integer
        Get
            Return equipment.statBonuses(statType, stat)
        End Get
    End Property
    Public ReadOnly Property moddedStats(stat As stat) As Integer
        Get
            Dim total As Integer = stats(stat) + statBonus(statType.Flat, stat)
            Return constrain(total, 0, 100)
        End Get
    End Property
    Public ReadOnly Property statRange(stat As stat) As String
        Get
            Return stats(stat) + equipment.statBonuses(statType.Flat, stat) & "/" & _
                stats(stat) + equipment.statBonuses(statType.Attack, stat) & "/" & _
                stats(stat) + equipment.statBonuses(statType.Defence, stat)
        End Get
    End Property
    Public constellation As constellation

    Public towncards As New townCards
    Public oppCards As New List(Of oppCard)
    Public gold As Integer = 0
    Public equipment As New equipment(Me)
    Public inventory As New inventory(Me)
    Public glory As Integer = 0


#Region "Character Generation"
    Friend Sub New()
        name = generateName()
        epithet = "the Novice"
        constellation = world.constellation
        age = 14 + rng.Next(3)
        healthMax = 20
        health = healthMax
        stats = generateStats(constellation)
        towncards.generateDefaultTownCards()
    End Sub

    Private firstNameList As New List(Of String) From {"Taz", "Far", "Mul", "Kul", "Pyr"}
    Private lastNameList As New List(Of String) From {"gar", "tar", "jar", "mam", "yul"}
    Private Function generateName() As String
        Dim firstNameSize As Integer = firstNameList.Count - 1
        Dim lastNameSize As Integer = lastNameList.Count - 1
        Dim firstName As String = firstNameList.Item(rng.Next(firstNameSize))
        Dim lastName As String = lastNameList.Item(rng.Next(lastNameSize))
        Return firstName & lastName
    End Function
    Private Function generateStats(targetConstellation As constellation) As Integer()
        Select Case targetConstellation
            Case God_Game.constellation.Hammer : Return {2, 1, 0}
            Case God_Game.constellation.Anvil : Return {2, 0, 1}
            Case God_Game.constellation.Cloak : Return {0, 2, 1}
            Case God_Game.constellation.Dagger : Return {1, 2, 0}
            Case God_Game.constellation.Stallion : Return {1, 0, 2}
            Case God_Game.constellation.Mare : Return {0, 1, 2}
            Case Else : Return {0, 0, 0}
        End Select
    End Function
#End Region

    Public Function getLoot(loot As loot) As KeyValuePair(Of loot, String)
        If TypeOf (loot) Is treasure Then
            Dim treasure As treasure = CType(loot, treasure)
            Dim gainGold As Integer = treasure.range.roll
            gold += gainGold
            Return New KeyValuePair(Of loot, String)(Nothing, name & " gains " & gainGold & " gold.")
        ElseIf TypeOf (loot) Is oppCardLoot Then
            Dim oppCardLoot As oppCardLoot = CType(loot, oppCardLoot)
            oppCards.Add(oppCardLoot.oppCard)
            Return New KeyValuePair(Of loot, String)(Nothing, name & " gains an Opportunity Card: " & oppCardLoot.name)
        ElseIf TypeOf (loot) Is item Then
            Dim item As item = CType(loot, item)
            Return New KeyValuePair(Of loot, String)(item, name & " finds " & item.name & "." & vbCrLf & item.consoleReport())
        Else
            Return Nothing
        End If
    End Function

    Public Function consoleReport(Optional space As Integer = 0) As String
        Dim x As Integer = space + 1
        Dim total As String = vbSpace(x - 1) & fullname & vbCrLf
        total &= vbSpace(x) & "Age:" & vbTab & vbTab & age & vbCrLf
        total &= vbSpace(x) & "Health:" & vbTab & health & "/" & healthMax & vbCrLf
        total &= vbSpace(x) & "Sign:" & vbTab & vbTab & constellation.ToString & vbCrLf
        total &= vbCrLf

        For n As stat = 0 To 2
            total &= vbSpace(x) & n.ToString & ":" & vbTab & moddedStats(n) & vbCrLf
            total &= vbSpace(x + 1) & "Attack:" & vbTab & withSign(statBonus(statType.Attack, n)) & vbCrLf
            total &= vbSpace(x + 1) & "Defence:" & vbTab & withSign(statBonus(statType.Defence, n)) & vbCrLf
            total &= vbSpace(x + 1) & "Skill:" & vbTab & withSign(statBonus(statType.Skill, n)) & vbCrLf
        Next
        total &= vbCrLf

        total &= vbSpace(x) & "Gold:" & vbTab & vbTab & gold & vbCrLf
        total &= equipment.consoleReport(space) & vbCrLf & vbCrLf
        total &= inventory.consoleReport(space) & vbCrLf & vbCrLf
        total &= vbSpace(x) & "Town Deck:" & vbTab & towncards.Count & vbCrLf
        total &= vbSpace(x) & "Opp. Hand:" & vbTab & oppCards.Count & vbCrLf
        Return total
    End Function
End Class