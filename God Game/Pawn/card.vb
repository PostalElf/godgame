Public MustInherit Class card
    Public pawn As pawn
    Public name As String

    Public buyItems As Boolean = False
    Public saleItems As Integer = 0
    Public saleItemCost As Double = 1
    Public transmute As Boolean = False
    Public gainPower As New range(0, 0)
    Public gainGold As New range(0, 0)
    Public restHealthBonus As New range(0, 0)
End Class


Public Class townCard
    Inherits card

    Public Function consoleReport() As String
        Dim total As String = ""

        If buyItems = True Then
            If saleItemCost = 1 Then
                total &= "Pursue a selection of " & saleItems & " goods." & vbCrLf
            Else
                total &= "Pursue a selection of " & saleItems & " goods at " & (saleItemCost * 100) & "% cost." & vbCrLf
            End If
        End If
        If transmute = True Then total &= "Transmute a single item into 100% gold." & vbCrLf
        If gainPower.isEmpty = False Then total &= determineGainLose(gainPower, "Power") & vbCrLf
        If gainGold.isEmpty = False Then total &= determineGainLose(gainGold, "Gold") & vbCrLf
        If restHealthBonus.isEmpty = False Then total &= determineGainLose(restHealthBonus, "Health") & vbCrLf

        Return total
    End Function
    Private Function determineGainLose(range As range, unit As String) As String
        If range.min > 0 Then
            Return "Gain " & range.min & "-" & range.max & " " & unit & "."
        Else
            Return "Lose " & Math.Abs(range.max) & "-" & Math.Abs(range.min) & " " & unit & "."
        End If
    End Function
End Class


Public Class oppCard
    Inherits card
End Class