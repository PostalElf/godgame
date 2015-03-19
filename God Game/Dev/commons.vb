Public Class commons
    Friend Shared Function parseStatField(str As String) As Integer
        If str = "" Then
            Return 0
        Else
            If IsNumeric(str) = True Then Return Convert.ToInt32(str) Else Return 0
        End If
    End Function
End Class
