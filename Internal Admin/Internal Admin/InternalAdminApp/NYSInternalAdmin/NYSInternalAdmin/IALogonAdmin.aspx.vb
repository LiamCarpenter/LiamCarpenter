Imports System.Net.Mail
Imports MonoSoftware.Web.Dialogs
Imports NysDat

Partial Public Class IALogonAdmin
    Inherits clsNYS

    Private Shared ReadOnly className As String

    Shared Sub New()
        className = System.Reflection.MethodBase. _
        GetCurrentMethod().DeclaringType.FullName
        log = log4net.LogManager.GetLogger(className)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Using New clslogger(log, className, "Page_Load")
            Try
                If Not Me.IsPostBack Then
                    Session.Remove("UserGroup")
                    Session.Remove("loggedinuser")
                    Dim strUser As String = notString(Request.QueryString("User"))
                    Dim blnRet As Boolean = True
                    lblversion.Text = "IA " & getConfig("Version")

                    If strUser = "false" Then
                        lbluser.Text = Request.ServerVariables("LOGON_USER") & " is not a registered user of this system, please contact the system administrator."
                        lblNoAccess.Visible = True
                        pnlogin.Visible = False

                    ElseIf strUser = "falseX" Then
                        lbluser.Text = Request.ServerVariables("LOGON_USER") & " is not a registered user of this system, please contact the system administrator."
                        lblNoAccess.Visible = True
                        pnlogin.Visible = False
                    Else
                        'let's check if using company intranet first
                        If Mid(Request.ServerVariables("LOGON_USER"), InStr(Request.ServerVariables("LOGON_USER"), "\") + 1) = "" Then
                            Dim strToken As String = notString(Request.QueryString("Token"))
                            log.Error("TOKEN:" & strToken)
                            blnRet = checkLoginstatus(strToken, True)
                        Else
                            log.Error("USER:" & Mid(Request.ServerVariables("LOGON_USER"), InStr(Request.ServerVariables("LOGON_USER"), "\") + 1))
                            blnRet = checkLoginstatus(Mid(Request.ServerVariables("LOGON_USER"), InStr(Request.ServerVariables("LOGON_USER"), "\") + 1), False)
                        End If
                        If blnRet Then
                            Session.Item("LoginType") = "ADMIN"
                            populategroups()
                            pnlogin.Visible = True
                            lblNoAccess.Visible = False
                            btnlogin.Attributes.Add("OnMouseOver", "this.src='images/continue_over.gif';")
                            btnlogin.Attributes.Add("OnMouseOut", "this.src='images/continue_out.gif';")
                        Else
                            pnlogin.Visible = False
                            lblNoAccess.Visible = True
                        End If
                    End If
                End If
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleexception(ex, "IALogonAdmin", Page)
                End If
            End Try
        End Using
    End Sub

    Private Function checkLoginstatus(ByVal pstrUserName As String, ByVal pblnCheckHash As Boolean) As Boolean
        Using New clslogger(log, className, "checkLoginstatus")

            Dim oUsers As List(Of clsSystemNYSUser)
            oUsers = clsSystemNYSUser.Populate(0, "")

            Dim strUserName As String = ""
            Dim ret As Integer = -1
            Dim strfirstname As String = ""
            Dim strlastname As String = ""

            If Session.Item("loggedinuser") Is Nothing Then
                If Not pblnCheckHash Then
                    oUsers = clsSystemNYSUser.Populate(0, pstrUserName)
                    log.Error("11")
                End If
                For Each oUser As clsSystemNYSUser In oUsers
                    strUserName = oUser.Systemnysuserloginname
                    If pblnCheckHash Then
                        Dim hasher As New NSMd5Hasher.Md5Hasher()
                        Dim strToken As String = hasher.Hash(strUserName)
                        If pstrUserName = strToken Then
                            strfirstname = oUser.Systemnysuserfirstname
                            strlastname = oUser.Systemnysuserlastname
                            ret = setUserX(oUser.Systemnysuserfirstname, oUser.Systemnysuserlastname, _
                                    CBool(oUser.SystemnysuserInactive), oUser.Systemnysusergroup)
                            Session.Item("loggedinuser") = oUser
                            Session.Item("UserGroup") = oUser.Systemnysusergroup
                            log.Error("22")
                            Exit For
                        End If
                    Else
                        If strUserName.ToUpper = pstrUserName.ToUpper Then
                            strfirstname = oUser.Systemnysuserfirstname
                            strlastname = oUser.Systemnysuserlastname
                            ret = setUserX(oUser.Systemnysuserfirstname, oUser.Systemnysuserlastname, _
                                    CBool(oUser.SystemnysuserInactive), oUser.Systemnysusergroup)
                            Session.Item("loggedinuser") = oUser
                            Session.Item("UserGroup") = oUser.Systemnysusergroup
                            log.Error("33")
                            Exit For
                        End If
                    End If

                Next
            Else
                Dim oUser As NysDat.clsSystemNYSUser
                oUser = CType(Session.Item("loggedinuser"), NysDat.clsSystemNYSUser)
                If oUser IsNot Nothing Then
                    strfirstname = oUser.Systemnysuserfirstname
                    strlastname = oUser.Systemnysuserlastname
                    ret = setUserX(oUser.Systemnysuserfirstname, oUser.Systemnysuserlastname, _
                            CBool(oUser.SystemnysuserInactive), oUser.Systemnysusergroup)
                    log.Error("44")
                End If
            End If

            If ret = -1 Then 'not registered
                lbluser.Text = Request.ServerVariables("LOGON_USER") & "3 is not a registered user of this system, please contact the system administrator."
                Return False
            ElseIf ret = 1 Then 'not admin user
                lbluser.Text = Request.ServerVariables("LOGON_USER") & "4 is not an Admin user of this system, please contact the system administrator."
                Return False
            ElseIf ret = 2 Then 'user inactive
                lbluser.Text = Request.ServerVariables("LOGON_USER") & "5 user account is inactive, please contact the system administrator."
                Return False
            Else 'all is well
                lbluser.Text = "Current user: " & strfirstname & " " & strlastname
                Return True
            End If
        End Using
    End Function

    Private Function setUserX(ByVal pfirstname As String, ByVal plastname As String, _
                                ByVal pInactive As Boolean, ByVal pgroup As String) As Integer
        Using New clslogger(log, className, "setUser")

            Dim intlockdown As Integer = 0

            If CBool(pInactive) = False Then
                If pgroup.ToUpper <> "ADMIN" And pgroup.ToUpper <> "ACCOUNTS" Then
                    If pgroup.ToUpper = "BOOKER" Or pgroup.ToUpper = "CONTRACTS" Or pgroup.ToUpper = "SUPERVISOR" Then
                        intlockdown = 3
                    Else
                        intlockdown = 1
                    End If
                End If
            Else
                intlockdown = 2
            End If
            Return intlockdown

        End Using
    End Function

    Private Sub populategroups()
        Using New clslogger(log, className, "populategroups")

            Dim oGroups As List(Of clsGroup)

            oGroups = clsGroup.listnone()
            ddgroups.Items.Clear()
            ddgroups.Items.Add(New ListItem("Please Select", CStr(0)))
            ddgroups.Items.Add(New ListItem("All Clients", CStr(1)))


            For Each gr As clsGroup In oGroups
                If gr.Groupname.ToLower <> "default templates" And _
                    gr.Groupname.ToLower <> "xtraining" And _
                    gr.Groupname.ToLower <> "zzz testing" And _
                    gr.Groupname.ToLower <> "one stop shop" And _
                    gr.Groupname.ToLower <> "event management" Then
                    Dim s As String = gr.Groupname

                    'R2.21.2 CR - rename NPSA to NHS Litigation Authority
                    'R2.17 CR - rename NPSA to NCAS
                    If gr.Groupname.ToLower = "npsa" Then
                        's = "ncas"
                        s = "NHS Litigation Authority"
                    End If

                    s = StrConv(LCase$(s), vbProperCase)
                    ddgroups.Items.Add(New ListItem(s, CStr(gr.Groupid)))
                End If
            Next

        End Using
    End Sub

    Protected Sub btnlogin_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnlogin.Click
        Using New clslogger(log, className, "btnlogin_Click")
            Try
                If ddgroups.SelectedIndex < 1 Then
                    Throw New EvoFriendlyException("Please select a client from the list to continue.", "Info")
                End If

                Dim dtstart As New Date(Now.Year, Now.Month, 1)
                dtstart = dtstart.AddMonths(-1)
                Dim dtend As Date = dtstart.AddMonths(1).AddDays(-1)
                Session.Item("startdate") = Format(dtstart, "dd/MM/yyyy")
                Session.Item("enddate") = Format(dtend, "dd/MM/yyyy")
                Session.Item("TTEnabled") = "true"
                Session.Item("AllEnabled") = "true"
                Dim dtStartYTD As Date
                Dim dtEndYTD As Date
                If Now.Month = 1 Then 'show last year
                    dtStartYTD = New Date(Now.Year - 1, 1, 1)
                    dtEndYTD = dtStartYTD.AddYears(1).AddDays(-1)
                Else
                    dtStartYTD = New Date(Now.Year, 1, 1)
                    dtEndYTD = New Date(Now.Year, Now.Month, 1)
                    dtEndYTD = dtEndYTD.AddDays(-1)
                End If

                Session.Item("startYTD") = Format(dtStartYTD, "dd/MM/yyyy")
                Session.Item("endYTD") = Format(dtEndYTD, "dd/MM/yyyy")


                Session.Item("clientid") = ddgroups.SelectedItem.Value
                Session.Item("clientname") = ddgroups.SelectedItem.Text '.Replace("=", "")
                Dim retDetails As New getBossIDs
                retDetails = checkClientName(ddgroups.SelectedItem.Text)

                Session.Item("clientnameshort") = retDetails.strBossID1 'checkClientName(ddgroups.SelectedItem.Text)
                Session.Item("clientnameshort2") = retDetails.strBossID2
                Session.Item("companyid") = retDetails.intCompanyID
                'set up Feeder file session
                Session.Item("FeederFileClient") = "none"
                If ddgroups.SelectedIndex > 0 Then
                    Session.Item("FeederFileClient") = ddgroups.SelectedItem.Text
                End If
                Response.Redirect("IAControl.aspx")

            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleexception(ex, "IALogonAdmin", Page)
                End If
            End Try
        End Using
    End Sub
End Class