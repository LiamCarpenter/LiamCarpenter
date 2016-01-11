Imports EvoUtilities.ConfigUtils
Imports System.Net.Mail
Imports NysDat
Imports System.Collections.Generic
Imports System.Globalization
Partial Public Class IAAdmin
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
                    populateBossRef()
                    PopulateDropDowns()
                    'R2.23 SA 
                    clearBoxes()
                    setControls(True)
                    btnlogout.Attributes.Add("OnMouseOver", "this.src='images/logout_over.gif';")
                    btnlogout.Attributes.Add("OnMouseOut", "this.src='images/logout_out.gif';")
                End If
            Catch ex As Exception
                handleexception(ex, "IAAdmin", Me.Page)
            End Try
        End Using
    End Sub

    Private Sub populateBossRef()

        Using Database As NYSDBEntities = New NYSDBEntities

            Dim BossRefList = (From x In Database.ad_StatementsParameters Select x.BOSS_code Order By BOSS_code Ascending).ToList

            ddBossRef.Items.Clear()
            ddBossRef.Items.Add(New ListItem("Please select", CStr(0)))

            For Each BossRef In BossRefList
                ddBossRef.Items.Add(BossRef)
            Next

        End Using

    End Sub

    'R2.23 SA - added 
    Private Sub clearBoxes()

        lblMessage.Text = ""
        ddBossRef.SelectedIndex = 0
        lblHiddenBossID.Text = 0
        txtBossRef.Text = ""
        txtEmailAddress.Text = ""

        ddReference.SelectedIndex = 0
        ddReference2.SelectedIndex = 0
        ddReference3.SelectedIndex = 0
        ddReference4.SelectedIndex = 0
        txtReference1.Text = ""
        txtReference2.Text = ""
        txtReference3.Text = ""
        txtReference4.Text = ""

        chkSendtoEmailAddress.Checked = False
        'chkonConinvoices.Checked = False

        'R?? SA - commented out for now! Might need again later! 
        'chkBranch00.Checked = False
        'chkBranch01.Checked = False
        'chkBranch08.Checked = False

    End Sub

    'R2.23 SA - recoded 
    Private Sub setControls(ByVal pValue As Boolean)

        ddBossRef.Enabled = pValue
        btnAdd.Visible = pValue
        btnChange.Visible = pValue

        txtEmailAddress.Enabled = Not (pValue)

        ddReference.Enabled = Not (pValue)
        ddReference2.Enabled = Not (pValue)
        ddReference3.Enabled = Not (pValue)
        ddReference4.Enabled = Not (pValue)

        txtReference1.Enabled = Not (pValue)
        txtReference2.Enabled = Not (pValue)
        txtReference3.Enabled = Not (pValue)
        txtReference4.Enabled = Not (pValue)

        chkSendtoEmailAddress.Enabled = Not (pValue)
        'chkonConinvoices.Enabled = Not (pValue)

        btnSave.Visible = Not (pValue)
        btnCancel.Visible = Not (pValue)
        btnOk.Visible = Not (pValue)
        btnClose.Visible = Not (pValue)

    End Sub

    Protected Sub ddBossRef_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddBossRef.SelectedIndexChanged

        Using Database As NYSDBEntities = New NYSDBEntities

            btnChange.Visible = True
            btnSave.Visible = False
            btnCancel.Visible = False

            If ddBossRef.SelectedIndex <> 0 Then
                Dim SelectedClientStatementDetails = (From x In Database.ad_StatementsParameters Where x.BOSS_code = ddBossRef.SelectedItem.Text).FirstOrDefault

                lblHiddenBossID.Text = SelectedClientStatementDetails.ID

                txtEmailAddress.Text = SelectedClientStatementDetails.Email

                ddReference.SelectedValue = SelectedClientStatementDetails.yourref_column
                ddReference2.SelectedValue = SelectedClientStatementDetails.yourref_column2
                ddReference3.SelectedValue = SelectedClientStatementDetails.yourref_column3
                ddReference4.SelectedValue = SelectedClientStatementDetails.yourref_column4

                txtReference1.Text = SelectedClientStatementDetails.yourref_column_header
                txtReference2.Text = SelectedClientStatementDetails.yourref_column2_header
                txtReference3.Text = SelectedClientStatementDetails.yourref_column3_header
                txtReference4.Text = SelectedClientStatementDetails.yourref_column4_header

                'We dont need this value anymore - As NYS Accounts App will automatically check if there are on a consolidated invoice
                'chkonConinvoices.Checked = SelectedClientStatementDetails.Active

                chkSendtoEmailAddress.Checked = SelectedClientStatementDetails.Active

            Else
                clearBoxes()
                setControls(True)
            End If

        End Using

    End Sub

    Protected Sub btnChange_Click(sender As Object, e As EventArgs) Handles btnChange.Click
        Using New clslogger(log, className, "btnChange_Click")
            Try
                If ddBossRef.SelectedIndex <> 0 Then

                    'R2.23 SA 
                    txtBossRef.Text = ""
                    txtBossRef.Visible = False
                    lblBossRefNew.Visible = False
                    setControls(False)
                Else
                    lblMessage.Text = "Please select a Boss Reference to continue!"
                    btnClose.Visible = True
                    btnClose.Enabled = True
                    lblMessage.Visible = True
                    pnlMessage.Style("TOP") = "200px"
                    pnlMessage.Style("LEFT") = "250px"
                    pnlMessage.Visible = True
                End If
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleexception(ex, "IAAdmin", Page)
                End If
            End Try
        End Using
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Using New clslogger(log, className, "btnSave_Click")
            Try
                Dim intID As Integer
                Dim strBossRef As String = ""
                Dim blnResult As Boolean

                'R2.20C CR - check validity of multiple email addresses
                If txtEmailAddress.Text.Contains(";") Then
                    'multiple addresses - check them all
                    For Each strSingleEmailAddress As String In txtEmailAddress.Text.Split(";")
                        'check each email address for validity
                        If strSingleEmailAddress <> "" Then
                            If Not Regex.IsMatch(strSingleEmailAddress.Trim, emailRegex) Then
                                Throw New EvoFriendlyException("Email address is incorrectly formatted, please check.", "Info")
                            End If
                        End If
                    Next
                Else
                    If Not Regex.IsMatch(txtEmailAddress.Text, emailRegex) Then
                        Throw New EvoFriendlyException("Email address is incorrectly formatted, please check.", "Info")
                    End If
                End If

                ''R?? - SA Bug Fix!
                'If txtBossRef.Visible = True AndAlso notString(txtBossRef.Text) <> "" Then
                '    intID = 0
                '    strBossRef = txtBossRef.Text
                'Else
                '    intID = clsClientStatementDetails.getID(ddBossRef.SelectedItem.Text)
                '    strBossRef = ddBossRef.SelectedItem.Text
                'End If

                Using Database As NYSDBEntities = New NYSDBEntities

                    If lblHiddenBossID.Text = "0" Then
                        'Create New
                        Dim NewClientStatementDetails As New ad_StatementsParameters

                        NewClientStatementDetails.BOSS_code = txtBossRef.Text
                        NewClientStatementDetails.Email = txtEmailAddress.Text

                        NewClientStatementDetails.yourref_column = ddReference.SelectedValue
                        NewClientStatementDetails.yourref_column2 = ddReference2.SelectedValue
                        NewClientStatementDetails.yourref_column3 = ddReference3.SelectedValue
                        NewClientStatementDetails.yourref_column4 = ddReference4.SelectedValue

                        NewClientStatementDetails.yourref_column_header = txtReference1.Text
                        NewClientStatementDetails.yourref_column2_header = txtReference2.Text
                        NewClientStatementDetails.yourref_column3_header = txtReference3.Text
                        NewClientStatementDetails.yourref_column4_header = txtReference4.Text

                        NewClientStatementDetails.Active = chkSendtoEmailAddress.Checked

                        Database.ad_StatementsParameters.Add(NewClientStatementDetails)
                        Database.SaveChanges()

                        blnResult = True
                    Else
                        'Save Current
                        Dim SelectedClientStatementDetails = (From x In Database.ad_StatementsParameters Where x.ID = lblHiddenBossID.Text).FirstOrDefault

                        'SelectedClientStatementDetails.BOSS_code = strBossRef
                        SelectedClientStatementDetails.Email = txtEmailAddress.Text

                        SelectedClientStatementDetails.yourref_column = ddReference.SelectedValue
                        SelectedClientStatementDetails.yourref_column2 = ddReference2.SelectedValue
                        SelectedClientStatementDetails.yourref_column3 = ddReference3.SelectedValue
                        SelectedClientStatementDetails.yourref_column4 = ddReference4.SelectedValue

                        SelectedClientStatementDetails.yourref_column_header = txtReference1.Text
                        SelectedClientStatementDetails.yourref_column2_header = txtReference2.Text
                        SelectedClientStatementDetails.yourref_column3_header = txtReference3.Text
                        SelectedClientStatementDetails.yourref_column4_header = txtReference4.Text

                        SelectedClientStatementDetails.Active = chkSendtoEmailAddress.Checked

                        Database.SaveChanges()

                        blnResult = True
                    End If

                End Using

                'R2.23 SA added: ddReference3.SelectedValue, ddReference4.SelectedValue, txtReference1.Text, _
                '  txtReference2.Text, txtReference3.Text, txtReference4.Text
                'blnResult = clsClientStatementDetails.update(intID, strBossRef, notString(txtEmailAddress.Text), ddReference.SelectedValue, chkSendtoEmailAddress.Checked, _
                '                                       ddReference2.SelectedValue, ddReference3.SelectedValue, ddReference4.SelectedValue, txtReference1.Text, _
                '                                       txtReference2.Text, txtReference3.Text, txtReference4.Text, False, False, False, chkonConinvoices.Checked)
                'R2.23 SA 
                txtBossRef.Text = ""
                txtBossRef.Visible = False
                lblBossRefNew.Visible = False

                If blnResult = True Then
                    lblMessage.Text = "Saved"
                    lblMessage.Visible = True
                    pnlMessage.Style("TOP") = "200px"
                    pnlMessage.Style("LEFT") = "250px"
                    pnlMessage.Visible = True
                    btnOk.Enabled = True
                    btnOk.Visible = True
                Else
                    lblMessage.Text = "Save failed!"
                    lblMessage.Visible = True
                    pnlMessage.Style("TOP") = "200px"
                    pnlMessage.Style("LEFT") = "250px"
                    pnlMessage.Visible = True
                    btnOk.Enabled = True
                    btnOk.Visible = True
                End If

            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleexception(ex, "IAAdmin", Page)
                End If
            End Try
        End Using

    End Sub

    Protected Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Using New clslogger(log, className, "btnAdd_Click")
            Try
                populateBossRef()
                'ddBossRef.Enabled = False
                'txtEmailAddress.Text = ""
                'txtEmailAddress.ReadOnly = False
                'ddReference.SelectedIndex = 0
                'chkSendtoEmailAddress.Checked = False
                'txtBossRef.Text = ""
                'txtBossRef.Visible = True
                'lblBossRefNew.Visible = True
                'btnSave.Visible = True
                'btnCancel.Visible = True
                ''R2.18 CR - enable some of the boxes
                'txtBossRef.Enabled = True
                'txtEmailAddress.Enabled = True
                'ddReference.Enabled = True
                'chkSendtoEmailAddress.Enabled = True

                'ddReference2.SelectedIndex = 0
                'ddReference2.Enabled = True
                ''R2.23 SA 
                'ddReference3.SelectedIndex = 0
                'ddReference3.Enabled = True
                'ddReference4.SelectedIndex = 0
                'ddReference4.Enabled = True
                'txtReference1.Enabled = True
                'txtReference2.Enabled = True
                'txtReference3.Enabled = True
                'txtReference4.Enabled = True

                clearBoxes()
                setControls(False)

                txtBossRef.Text = ""
                txtBossRef.Visible = True
                lblBossRefNew.Visible = True
                txtBossRef.Enabled = True

                ''R2.21.1 SA 
                'chksenddaily.Enabled = True
                'chksendtobooker.Enabled = True
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleexception(ex, "IAAdmin", Page)
                End If
            End Try
        End Using
    End Sub

    Protected Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        Using New clslogger(log, className, "btnOk_Click")
            Try
                lblMessage.Text = ""
                btnOk.Visible = False
                btnOk.Enabled = False
                pnlMessage.Visible = False
                clearBoxes()
                setControls(True)
                populateBossRef()
                'R2.23 SA 
                txtBossRef.Text = ""
                txtBossRef.Visible = False
                lblBossRefNew.Visible = False
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleexception(ex, "IAAdmin", Page)
                End If
            End Try
        End Using
    End Sub

    Protected Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Using New clslogger(log, className, "btnclose_Click")
            Try
                lblMessage.Text = ""
                btnClose.Visible = False
                btnClose.Enabled = False
                pnlMessage.Visible = False
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleexception(ex, "IAAdmin", Page)
                End If
            End Try
        End Using
    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Using New clslogger(log, className, "btnCancel_Click")
            Try
                'R2.23 SA 
                txtBossRef.Text = ""
                txtBossRef.Visible = False
                lblBossRefNew.Visible = False

                setControls(True)
                populateBossRef()
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleexception(ex, "IAAdmin", Page)
                End If
            End Try
        End Using
    End Sub

    Private Sub IAAdmin_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Using New clslogger(log, className, "IAAdmin_PreRender")
            Try
                Dim fp As ucReportMenu = DirectCast(LoadControl("UserControls/ucReportMenu.ascx"),  _
                    ucReportMenu)
                fp.pageName = "IAAdmin"
                phMenu.Controls.Add(fp)
            Catch ex As Exception
                handleexception(ex, "IAControl", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub btnlogout_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnlogout.Click
        Using New clslogger(log, className, "btnlogout_Click")
            Try
                Response.Redirect("IALogonAdmin.aspx")
            Catch ex As Exception
                handleexception(ex, "IAClientStatements", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub txtBossRef_TextChanged(sender As Object, e As EventArgs) Handles txtBossRef.TextChanged
        Using New clsLoggerDat(log, className, "txtBossRef_TextChanged")
            Try
                Dim blnCheck As Boolean = False
                For Each item As ListItem In ddBossRef.Items
                    If item.Text.ToLower.Trim = txtBossRef.Text.ToLower.Trim Then
                        blnCheck = True
                        lblMessage.Text = "Boss Reference already exsists! Please enter new reference instead:"
                        txtBossRef.Text = ""
                        btnClose.Enabled = True
                        btnClose.Visible = True
                        lblMessage.Visible = True
                        pnlMessage.Style("TOP") = "200px"
                        pnlMessage.Style("LEFT") = "250px"
                        pnlMessage.Visible = True
                    End If
                Next
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleexception(ex, "IAAdmin", Page)
                End If
            End Try
        End Using
    End Sub

    Protected Sub PopulateDropDowns()
        Using Database As NYSDBEntities = New NYSDBEntities

            Dim RefOptionsList = (From x In Database.ad_statements_refoptions Order By x.friendly_title Ascending).ToList

            For Each RefOption In RefOptionsList

                ddReference.Items.Add(New ListItem With {.Text = RefOption.friendly_title, .Value = RefOption.id})
                ddReference2.Items.Add(New ListItem With {.Text = RefOption.friendly_title, .Value = RefOption.id})
                ddReference3.Items.Add(New ListItem With {.Text = RefOption.friendly_title, .Value = RefOption.id})
                ddReference4.Items.Add(New ListItem With {.Text = RefOption.friendly_title, .Value = RefOption.id})

            Next

        End Using
    End Sub
End Class