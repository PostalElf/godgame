Public Class oppCardLoot
    Inherits loot
    Friend Property name As String
    Friend Property skill As stat
    Friend Property description As String
    Friend Property oppCard As oppCard

    Public Sub New()
    End Sub
    Public Sub New(targetName As String, targetSkill As stat, targetDescription As String)
        name = targetName
        skill = targetSkill
        description = targetDescription
    End Sub
End Class
