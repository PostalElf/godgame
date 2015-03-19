Imports Microsoft.VisualBasic.FileIO

Public Class csvFile
    Public rows As New List(Of String())

    Public Sub New(filename As String)
        Using parser As New TextFieldParser(filename)
            parser.CommentTokens = {"'"}
            parser.SetDelimiters({","})

            'skip header
            parser.ReadLine()

            While parser.EndOfData = False
                Dim currentRow As String() = parser.ReadFields
                rows.Add(currentRow)
            End While
        End Using
    End Sub
End Class
