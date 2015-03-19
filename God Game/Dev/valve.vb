﻿Imports System.IO

Module valve
    Public rng As New Random

    Public Function vbSpace(Optional times As Integer = 1) As String
        Dim total As String = Nothing
        For n = 1 To times
            total &= "  "
        Next
        Return total
    End Function

    Public Function constrain(value As Integer, Optional minValue As Integer = 1, Optional maxValue As Integer = 100) As Integer
        Dim total As Integer = value
        If total < minValue Then total = minValue
        If total > maxValue Then total = maxValue
        Return total
    End Function
    Public Function circular(value As Integer, Optional minValue As Integer = 1, Optional maxValue As Integer = 4) As Integer
        Dim total As Integer = value
        While total < minValue OrElse total > maxValue
            If total < minValue Then total += maxValue
            If total > maxValue Then total -= maxValue
        End While
        Return total
    End Function

    Public Function sign(value As Decimal) As String
        If value < 0 Then Return "" Else Return "+"
    End Function
    Public Function withSign(value As Decimal) As String
        Return sign(value) & value
    End Function
    Public Function withCommas(inputList As List(Of String)) As String
        Dim total As String = ""
        For n = 0 To inputList.Count - 1
            total &= inputList(n)
            If n < inputList.Count - 1 Then total &= ", "
        Next
        Return total
    End Function
    Public Sub consolePause(milliseconds As Integer)
        System.Threading.Thread.Sleep(milliseconds)
    End Sub

    Public Function percentRoll(probability As Integer) As Boolean
        Dim roll As Integer = rng.Next(1, 101)
        If roll <= probability Then Return True Else Return False
    End Function
    Public Function coinFlip() As Boolean
        Randomize()
        If Int(Rnd() * 2) + 1 = 1 Then Return True Else Return False
    End Function
    Public Function pythogoras(xy1 As xy, xy2 As xy) As Integer
        Dim x As Integer = Math.Abs(xy1.x - xy2.x)
        Dim y As Integer = Math.Abs(xy1.y - xy2.y)
        Return Int(Math.Sqrt(x * x + y * y))
    End Function

    Public Function getLiteral(value As Integer, Optional sigDigits As Integer = 2) As String
        Dim str As String = ""
        For n = 1 To sigDigits
            str = str & "0"
        Next
        str = str & value

        Dim characters() As Char = StrReverse(str)
        str = Nothing
        For n = 0 To sigDigits - 1
            str = str & characters(n)
        Next

        Return StrReverse(str)
    End Function
    Public Function joinString(stringQueue As Queue(Of String)) As String
        Dim total As String = ""
        While stringQueue.Count > 0
            total &= stringQueue.Dequeue
        End While
        Return total
    End Function
    Public Function writeDash(n As Integer) As String
        Dim str As String = ""
        For count = 0 To n
            str &= ("-")
        Next
        Return str
    End Function

    Public Function fileget(pathname As String) As List(Of String)
        Dim templist As New List(Of String)

        Try
            Dim sr As New StreamReader(pathname)
            Do While sr.Peek <> -1
                Dim line As String = sr.ReadLine
                templist.Add(line)
            Loop
        Catch ex As Exception
            MsgBox("Invalid pathname")
            For count As Integer = 1 To 20
                templist.Add(count)
            Next
        End Try

        Return templist
    End Function
End Module


Public Class range
    Public Property min As Integer
    Public Property max As Integer
    Public ReadOnly Property isEmpty As Boolean
        Get
            If min = 0 AndAlso max = 0 Then Return True Else Return False
        End Get
    End Property

    Public Sub New(_min As Integer, _max As Integer)
        min = _min
        max = _max
    End Sub
    Public Overrides Function ToString() As String
        If min = max Then Return min Else Return min & "-" & max
    End Function

    Public Function roll() As Integer
        Return rng.Next(min, max + 1)
    End Function
    Public Function isWithin(value As Integer) As Boolean
        If value >= min AndAlso value <= max Then Return True Else Return False
    End Function
End Class

Public Class xy
    Public Property x As Integer
    Public Property y As Integer

    Public Sub New()
    End Sub
    Public Sub New(_x As Integer, _y As Integer)
        x = _x
        y = _y
    End Sub
End Class
