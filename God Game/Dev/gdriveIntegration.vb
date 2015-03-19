Imports System.Net

Public Class gdriveIntegration
    Public Class WebClientEx
        Inherits WebClient
        Private Shadows ReadOnly container As New CookieContainer()

        Public Sub New(container As CookieContainer)
            Me.container = container
        End Sub

        Protected Overrides Function GetWebRequest(address As Uri) As WebRequest
            Dim r As WebRequest = MyBase.GetWebRequest(address)
            Dim request = TryCast(r, HttpWebRequest)
            If request IsNot Nothing Then
                request.CookieContainer = Container
            End If
            Return r
        End Function

        Protected Overrides Function GetWebResponse(request As WebRequest, result As IAsyncResult) As WebResponse
            Dim response As WebResponse = MyBase.GetWebResponse(request, result)
            ReadCookies(response)
            Return response
        End Function

        Protected Overrides Function GetWebResponse(request As WebRequest) As WebResponse
            Dim response As WebResponse = MyBase.GetWebResponse(request)
            ReadCookies(response)
            Return response
        End Function

        Private Sub ReadCookies(r As WebResponse)
            Dim response = TryCast(r, HttpWebResponse)
            If response IsNot Nothing Then
                Dim cookies As CookieCollection = response.Cookies
                container.Add(cookies)
            End If
        End Sub
    End Class

    Public Function getCSV(url As String)
        'apphend "&output=csv" to link
        Const suffix As String = "&output=csv"
        If url.Length > suffix.Length AndAlso url.Substring(url.Length - suffix.Length) <> suffix Then url = url & suffix


        Dim wc As New WebClientEx(New CookieContainer())
        wc.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:22.0) Gecko/20100101 Firefox/22.0")
        wc.Headers.Add("DNT", "1")
        wc.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8")
        wc.Headers.Add("Accept-Encoding", "deflate")
        wc.Headers.Add("Accept-Language", "en-US,en;q=0.5")

        Dim outputCSVdata = wc.DownloadString(url)
        Return outputCSVdata
    End Function
End Class

