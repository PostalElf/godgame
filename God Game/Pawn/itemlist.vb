Public MustInherit Class itemList
    Friend MustOverride Property pawn As pawn
    Friend MustOverride Function Add(item As item) As item
    Friend MustOverride Function Remove(item As item) As item
    Protected total As New List(Of item)
    Friend Overridable Function consoleReport(Optional space As Integer = 0) As String
        Dim x As Integer = space + 1
        Dim sum As String = ""
        If total.Count = 0 Then
            sum &= vbSpace(x) & "None." & vbCrLf
        Else
            For Each item In total
                sum &= item.consoleReport(x)
            Next
        End If
        Return sum
    End Function
End Class


Public Class equipment
    Inherits itemList
    Friend Overrides Property pawn As pawn
    Friend Overrides Function Add(targetItem As item) As item
        Dim replacedItem As item = searchKeyword(targetItem.keyword)
        If replacedItem Is Nothing = True Then
            'no item needs to be replaced
            total.Add(targetItem)
            Return Nothing
        Else
            'return replaced item
            total.Remove(replacedItem)
            total.Add(targetItem)
            Return replacedItem
        End If
    End Function
    Friend Overrides Function Remove(targetItem As item) As item
        If total.Contains(targetItem) = False Then
            Return Nothing
        Else
            total.Remove(targetItem)
            Return targetItem
        End If
    End Function
    Friend Function searchKeyword(keyword As String) As item
        For Each item In total
            If item.keyword = keyword Then Return item
        Next
        Return Nothing
    End Function

    Friend Function statBonuses(statType As statType, stat As stat) As Integer
        Dim sum As Integer = 0
        For Each item In total
            sum += item.statBonuses(statType)(stat)
        Next
        Return sum
    End Function
    Friend Function specialBonus(specialName As String) As Integer
        Dim sum As Integer = 0
        For Each item In total
            If item.special Is Nothing = False Then
                If item.special.ToLower.Contains(specialName.ToLower) Then
                    sum += item.specialBonus()
                End If
            End If
        Next
        Return sum
    End Function

    Public Sub New(_pawn As pawn)
        pawn = _pawn
    End Sub
    Friend Overrides Function consoleReport(Optional space As Integer = 0) As String
        Dim x As Integer = space + 1
        Dim sum As String = vbSpace(x) & "Equipment:" & vbCrLf
        sum &= MyBase.consoleReport(x)
        Return sum
    End Function
End Class


Public Class inventory
    Inherits itemList
    Friend Overrides Property pawn As pawn
    Private Const defaultSlots As Integer = 2
    Public ReadOnly Property slots As Integer
        Get
            Return defaultSlots + pawn.equipment.specialBonus("Space")
        End Get
    End Property
    Friend ReadOnly Property hasSpace As Boolean
        Get
            If total.Count + 1 > slots Then Return False Else Return True
        End Get
    End Property
    Friend ReadOnly Property Count As Integer
        Get
            Return total.Count
        End Get
    End Property

    Friend Overrides Function Add(item As item) As item
        If total.Count + 1 <= slots Then
            total.Add(item)
            Return Nothing
        Else
            Return item
        End If
    End Function
    Friend Overrides Function Remove(item As item) As item
        If total.Contains(item) = False Then
            Return Nothing
        Else
            total.Remove(item)
            Return item
        End If
    End Function
    Friend Sub RemoveAt(index As Integer)
        total.RemoveAt(index)
    End Sub
    Friend Function Item(index As Integer)
        Return total.Item(index)
    End Function

    Public Sub New(_pawn As pawn)
        pawn = _pawn
    End Sub
    Friend Overrides Function consoleReport(Optional space As Integer = 0) As String
        Dim x As Integer = space + 1
        Dim sum As String = vbSpace(x) & "Inventory:" & vbTab & total.Count & "/" & slots & vbCrLf
        sum &= MyBase.consoleReport(x)
        Return sum
    End Function
    Friend Function consoleReportMenu() As String
        Dim sum As String = ""
        Dim n As Integer = 1
        For Each thing In total
            sum &= n & ". " & thing.consoleReport
            n += 1
        Next
        Return sum
    End Function
End Class