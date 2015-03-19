Public Class world
    Private Const constellationMin As Integer = 1
    Private Const constellationMax As Integer = 6

    Public Shared constellation As constellation = rng.Next(constellationMin, constellationMax + 1)
    Public Shared players As New List(Of player)

    Public Shared Sub tick()
        constellation = circular(constellation + 1, constellationMin, constellationMax)
        For Each player In players
            player.powerMax = constrain(player.powerMax + 1, 1, 20)
            For Each pawn In player.pawns
                pawn.age += 1
            Next
        Next
    End Sub
End Class
