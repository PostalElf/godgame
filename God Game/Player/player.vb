Public Class player
    Public pawns As New List(Of pawn)
    Public pawnsUnused As Integer = 3
    Public powerMax As Integer = 1
    Public power As Integer = 1
    Public glory As Integer = 0

    Public Function spawnPawn() As pawn
        Dim pawn As New pawn
        pawn.player = Me
        pawns.Add(pawn)
        Return pawn
    End Function
End Class
