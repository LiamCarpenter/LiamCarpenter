Imports System.IO
Imports System.Net
Imports System.Text
Imports EvoUtilities.ConfigUtils
Imports Microsoft.Win32

''' <summary>
''' Class WebRetrieve - Checks if today's file is ready for download, if so it does it. 
''' File can be up to 2MB
''' </summary>
''' <remarks>Created 14/03/2009 Nick Massarella</remarks>
Public Class WebRetrieve
    Public Shared Function Main() As String
        Try
            'first check to see if today's file has been downloaded yetNYS-igF6vk 

            'https://ssl.conferma.com/confermaReports/auto/NYS-igF6vk/20110721-booking.csv
            If Not IO.File.Exists(getConfig("downloadedfiles") & Format(Now, "dd-MM-yyyy") & ".csv") And _
                Not IO.File.Exists(getConfig("downloadedfilesOK") & Format(Now, "dd-MM-yyyy") & ".csv") And _
                Not IO.File.Exists(getConfig("downloadedfilesERROR") & "Error_" & Format(Now, "dd-MM-yy") & ".csv") Then
                'Dim wr As HttpWebRequest = CType(WebRequest.Create(getConfig("ConfermaFileLocation") &
                '                            Format(Now, "yyyyMMdd") & "-booking.csv"), HttpWebRequest)
                'Dim ws As HttpWebResponse = CType(wr.GetResponse(), HttpWebResponse)

                Dim wr As FileWebRequest = CType(FileWebRequest.Create(getConfig("ConfermaFileLocation") & Format(Now, "yyyyMMdd") & "-booking.csv"), FileWebRequest)
                Dim ws As FileWebResponse = CType(wr.GetResponse(), FileWebResponse)

                Dim str As Stream = ws.GetResponseStream()
                Dim inBuf(2000000) As Byte
                Dim bytesToRead As Integer = CInt(inBuf.Length)
                Dim bytesRead As Integer = 0
                While bytesToRead > 0
                    Dim n As Integer = str.Read(inBuf, bytesRead, bytesToRead)
                    If n = 0 Then
                        Exit While
                    End If
                    bytesRead += n
                    bytesToRead -= n
                End While

                Dim fstr As New FileStream(getConfig("downloadedfiles") & Format(Now, "dd-MM-yyyy") & _
                                           ".csv", FileMode.OpenOrCreate, FileAccess.Write)
                fstr.Write(inBuf, 0, bytesRead)
                str.Close()
                fstr.Close()
                Return ""
            Else
                Return "Today's file has already been downloaded"
            End If
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    Public Shared Function BookedData() As String
        Try
            'first check to see if today's file has been downloaded yet
            'https://ssl.conferma.com/confermaReports/auto/NYS-igF6vk/20090703-bookingsMadeToday.csv
            If Not IO.File.Exists(getConfig("downloadedfilesBooked") & Format(Now, "dd-MM-yyyy") & ".csv") And _
                Not IO.File.Exists(getConfig("downloadedfilesBookedOK") & Format(Now, "dd-MM-yyyy") & ".csv") And _
                Not IO.File.Exists(getConfig("downloadedfilesBookedERROR") & "Error_" & Format(Now, "dd-MM-yy") & ".csv") Then
                'Dim wr As HttpWebRequest = CType(WebRequest.Create(getConfig("ConfermaFileLocation") &
                '                            Format(Now, "yyyyMMdd") & "-bookingsMadeToday.csv"), HttpWebRequest)
                'Dim ws As HttpWebResponse = CType(wr.GetResponse(), HttpWebResponse)

                'test
                Dim wr As FileWebRequest = CType(FileWebRequest.Create(getConfig("ConfermaFileLocation") & Format(Now, "yyyyMMdd") & "-bookingsMadeToday.csv"), FileWebRequest)
                Dim ws As FileWebResponse = CType(wr.GetResponse(), FileWebResponse)

                Dim str As Stream = ws.GetResponseStream()
                Dim inBuf(2000000) As Byte
                Dim bytesToRead As Integer = CInt(inBuf.Length)
                Dim bytesRead As Integer = 0
                While bytesToRead > 0
                    Dim n As Integer = str.Read(inBuf, bytesRead, bytesToRead)
                    If n = 0 Then
                        Exit While
                    End If
                    bytesRead += n
                    bytesToRead -= n
                End While

                Dim fstr As New FileStream(getConfig("downloadedfilesBooked") & Format(Now, "dd-MM-yyyy") & _
                                           ".csv", FileMode.OpenOrCreate, FileAccess.Write)
                fstr.Write(inBuf, 0, bytesRead)
                str.Close()
                fstr.Close()
                Return ""
            Else
                Return "Today's file has already been downloaded"
            End If
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    'R2.21.7 CR
    Public Shared Function BookedAftData() As String
        Try
            'first check to see if today's file has been downloaded yet
            'https://ssl.conferma.com/confermaReports/auto/NYS-igF6vk/20130522-bookingsMadeToday%20between%207am%20and%204pm.csv
            If Not IO.File.Exists(getConfig("downloadedfilesBookedAft") & Format(Now, "dd-MM-yyyy") & ".csv") And
                Not IO.File.Exists(getConfig("downloadedfilesBookedAftOK") & Format(Now, "dd-MM-yyyy") & ".csv") And
                Not IO.File.Exists(getConfig("downloadedfilesBookedAftERROR") & "Error_" & Format(Now, "dd-MM-yy") & ".csv") Then

                'Dim wr As HttpWebRequest = CType(WebRequest.Create(getConfig("ConfermaFileLocation") &
                '                                Format(Now, "yyyyMMdd") & "-bookingsMadeToday%20between%2012am%20and%204pm.csv"), HttpWebRequest)
                'Dim ws As HttpWebResponse = CType(wr.GetResponse(), HttpWebResponse)

                ''test
                Dim wr As FileWebRequest = CType(FileWebRequest.Create(getConfig("ConfermaFileLocation") & Format(Now, "yyyyMMdd") & "-bookingsMadeToday%20between%2012am%20and%204pm.csv"), FileWebRequest)
                Dim ws As FileWebResponse = CType(wr.GetResponse(), FileWebResponse)

                Dim str As Stream = ws.GetResponseStream()
                Dim inBuf(2000000) As Byte
                Dim bytesToRead As Integer = CInt(inBuf.Length)
                Dim bytesRead As Integer = 0
                While bytesToRead > 0
                    Dim n As Integer = str.Read(inBuf, bytesRead, bytesToRead)
                    If n = 0 Then
                        Exit While
                    End If
                    bytesRead += n
                    bytesToRead -= n
                End While
                Dim fstr As New FileStream(getConfig("downloadedfilesBookedAft") & Format(Now, "dd-MM-yyyy") &
                                               ".csv", FileMode.OpenOrCreate, FileAccess.Write)
                fstr.Write(inBuf, 0, bytesRead)
                str.Close()
                fstr.Close()
                Return ""
            Else
                Return "Today's file has already been downloaded"
            End If
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    Public Shared Function TransData(ByVal pConFileDate As String, ByVal pOurFileDate As String) As String
        Try
            'first check to see if today's file has been downloaded yet
            'https://ssl.conferma.com/confermaReports/auto/NYS-igF6vk/
            'https://ssl.conferma.com/confermaReports/auto/NYS-igF6vk/2011-09-15-Hotel_DailyTransactions_Agency1438.csv
            If Not IO.File.Exists(getConfig("downloadedfilesTrans") & pOurFileDate & ".csv") And _
                Not IO.File.Exists(getConfig("downloadedfilesTransOK") & pOurFileDate & ".csv") And _
                Not IO.File.Exists(getConfig("downloadedfilesTransERROR") & "Error_" & pOurFileDate & ".csv") Then
                'Dim wr As HttpWebRequest = CType(WebRequest.Create(getConfig("ConfermaFileLocation") &
                '                            pConFileDate & "-Hotel_DailyTransactions_Agency1438.csv"), HttpWebRequest)
                'Dim ws As HttpWebResponse = CType(wr.GetResponse(), HttpWebResponse)

                'test
                Dim wr As FileWebRequest = CType(FileWebRequest.Create(getConfig("ConfermaFileLocation") & Format(Now, "yyyyMMdd") & "-Hotel_DailyTransactions_Agency1438.csv"), FileWebRequest)
                Dim ws As FileWebResponse = CType(wr.GetResponse(), FileWebResponse)

                Dim str As Stream = ws.GetResponseStream()
                Dim inBuf(2000000) As Byte
                Dim bytesToRead As Integer = CInt(inBuf.Length)
                Dim bytesRead As Integer = 0
                While bytesToRead > 0
                    Dim n As Integer = str.Read(inBuf, bytesRead, bytesToRead)
                    If n = 0 Then
                        Exit While
                    End If
                    bytesRead += n
                    bytesToRead -= n
                End While

                Dim fstr As New FileStream(getConfig("downloadedfilesTrans") & pOurFileDate & _
                                           ".csv", FileMode.OpenOrCreate, FileAccess.Write)
                fstr.Write(inBuf, 0, bytesRead)
                str.Close()
                fstr.Close()
                Return ""
            Else
                Return "Today's file has already been downloaded"
            End If
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function
End Class
