Public Class item
    Inherits loot
    Public name As String
    Public cost As Integer
    Public keyword As String
    Public description As String
    Public special As String

    '   statType        Integer(0)          Integer(1)          Integer(2)
    '   0               Might (Flat)        Cunning (Flat)      Sorcery (Flat)
    '   1               Might (Att.)        Cunning (Att.)      Sorcery (Att.)
    '   2               Might (Def.)            "                   "
    '   3               Might (Skill)           "                   "
    Public statBonuses As New Dictionary(Of statType, Integer())


    Friend Sub New()
    End Sub
    Friend Sub New(item As item)
        name = item.name
        cost = item.cost
        keyword = item.keyword
        description = item.description
        statBonuses = item.statBonuses
    End Sub
    Friend Sub New(targetName As String, targetCost As Integer, targetKeyword As String, targetDescription As String, _
                   Optional flat() As Integer = Nothing, _
                   Optional attack() As Integer = Nothing, _
                   Optional defence() As Integer = Nothing, _
                   Optional skill() As Integer = Nothing, _
                   Optional targetSpecial As String = Nothing)
        name = targetName
        keyword = targetKeyword
        description = targetDescription
        special = targetspecial

        Dim control As New Queue(Of Integer())
        control.Enqueue(flat)
        control.Enqueue(attack)
        control.Enqueue(defence)
        control.Enqueue(skill)

        For n = 0 To 3
            Dim currentControl As Integer() = control.Dequeue
            If currentControl Is Nothing = True Then
                statBonuses.Add(n, {0, 0, 0})
            Else
                statBonuses.Add(n, currentControl)
            End If
        Next
    End Sub

    Friend Function consoleReport(Optional space As Integer = 0) As String
        Dim x As Integer = space + 1
        Dim sum As String = ""

        sum &= vbSpace(x) & name & " - " & keyword & vbCrLf
        For n As stat = 0 To 2
            For p As statType = 0 To 3
                Dim statBonus As Integer = statBonuses(p)(n)
                If statBonus <> 0 Then
                    sum &= vbSpace(x + 1) & parseStatBonus(p, n) & " " & withSign(statBonus) & vbCrLf
                End If
            Next
        Next
        sum &= vbCrLf

        Return sum
    End Function
    Private Function parseStatBonus(statType As statType, stat As stat) As String
        If statType <> God_Game.statType.Flat Then Return stat.ToString & " (" & statType.ToString & ")" Else Return stat.ToString
    End Function

    Friend Function specialBonus() As Integer
        If special = Nothing Then Return 0

        Dim spaceIndex As Integer = special.IndexOf(" ")
        Dim sign As String = special.Substring(spaceIndex + 1, 1)
        Dim valueStr As String = ""
        If sign = "+" Then
            valueStr = special.Substring(spaceIndex + 2, special.Length - spaceIndex - 2)
        ElseIf sign = "-" Then
            valueStr = special.Substring(spaceIndex + 1, special.Length - spaceIndex - 1)
        Else
            Return 0
        End If
        Return Convert.ToInt32(valueStr)
    End Function
End Class
