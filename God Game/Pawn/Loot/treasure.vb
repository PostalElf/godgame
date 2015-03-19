Public Class treasure
    Inherits loot
    Friend range As range

    Friend Sub New(_range As range)
        range = _range
    End Sub
    Friend Sub New(min As Integer, max As Integer)
        range = New range(min, max)
    End Sub
End Class