Imports EvoUtilities.ConfigUtils
Imports System.Net.Mail
Imports NysDat
Imports System.Collections.Generic
Imports System.Globalization

Partial Public Class IAAdminCCP
    Inherits clsNYS

    'Private Shared ReadOnly className As String
    'Shared Sub New()
    '    className = System.Reflection.MethodBase. _
    '    GetCurrentMethod().DeclaringType.FullName
    '    log = log4net.LogManager.GetLogger(className)
    'End Sub

    'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    '    Using New clslogger(log, className, "Page_Load")
    '        Try
    '            If Not Me.IsPostBack Then
    '                Dim strRet As String = setUser()
    '                If strRet.StartsWith("ERROR") Then
    '                    Response.Redirect("IALogonAdmin.aspx?User=falseX")
    '                End If

    '                Dim oUser As NysDat.clsSystemNYSUser
    '                oUser = CType(Session.Item("loggedinuser"), NysDat.clsSystemNYSUser)
    '                If getConfig("CCPUsers").ToString.ToLower.Contains(oUser.Systemnysuserlastname.ToLower) Then
    '                    btnsave.Visible = True
    '                    btndelete.Visible = True
    '                    btnpopulate.Visible = True
    '                    btnClearboxes.Visible = True
    '                    'check in db for records to be deleted
    '                    Dim intCount As Integer
    '                    intCount = NysDat.clsCCPDat.checkToDelete()
    '                    If intCount > 0 Then
    '                        lbldelete.ForeColor = Drawing.Color.Red
    '                        If intCount = 1 Then
    '                            lbldelete.Text = "There is 1 credit card record in database needs deleting!"
    '                        Else
    '                            lbldelete.Text = "There are " & intCount & " credit card records in database needs deleting!"
    '                        End If
    '                        btndelete.Enabled = True
    '                    Else
    '                        lbldelete.ForeColor = Drawing.Color.Black
    '                        lbldelete.Text = "No credit card records to delete!"
    '                        btndelete.Enabled = False
    '                    End If
    '                Else
    '                    btnsave.Visible = False
    '                    btnpopulate.Visible = False
    '                    btndelete.Visible = False
    '                    btnClearboxes.Visible = False
    '                    Throw New EvoFriendlyException("User not authorised to login to this section, please log out!", "Info")
    '                End If
    '            End If
    '        Catch ex As Exception
    '            handleexception(ex, "IAAdminCCP", Me.Page)
    '        End Try
    '    End Using
    'End Sub

    'Private Sub IAAdminCCP_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
    '    Using New clslogger(log, className, "IAAdminCCP_PreRender")
    '        Try
    '            Dim fp As ucReportMenu = DirectCast(LoadControl("UserControls/ucReportMenu.ascx"),  _
    '                ucReportMenu)
    '            fp.pageName = "IAAdminCCP"
    '            phMenu.Controls.Add(fp)
    '        Catch ex As Exception
    '            handleexception(ex, "IAControlCCP", Me.Page)
    '        End Try
    '    End Using
    'End Sub

    'Private Sub clearBoxes()
    '    Using New clslogger(log, className, "clearBoxes")
    '        txtname.Text = ""
    '        txtcardnumber1.Text = ""
    '        txtcardnumber2.Text = ""
    '        txtcvv.Text = ""
    '        ddexdateMM.SelectedIndex = 0
    '        ddexdateYYYY.SelectedIndex = 0
    '        txtemail.Text = ""
    '        txtpostalline1.Text = ""
    '        txtpostalline2.Text = ""
    '        txtpostalcity.Text = ""
    '        txtpostalpostcode.Text = ""
    '        txtaddressline1.Text = ""
    '        txtaddressLine2.Text = ""
    '        txtaddresscity.Text = ""
    '        txtaddresspostcode.Text = ""
    '        txtnysref.Text = ""
    '        txtprocessedDate.Text = ""
    '        chkProcessed.Checked = False
    '    End Using
    'End Sub

    'Private Sub changeDropDowns(ByVal dd As System.Web.UI.WebControls.DropDownList, _
    '                           ByVal pstrvalue As String, ByVal pblnclear As Boolean, _
    '                           ByVal pblntext As Boolean)
    '    Using New clslogger(log, className, "changeDropDowns")
    '        If pblnclear = True Then
    '            For intcount As Integer = 0 To dd.Items.Count - 1
    '                If dd.Items(intcount).Selected = True Then
    '                    dd.Items(intcount).Selected = False
    '                End If
    '            Next
    '            dd.SelectedIndex = 0
    '        Else
    '            If pblntext = True Then
    '                For intcount As Integer = 0 To dd.Items.Count - 1
    '                    If dd.Items(intcount).Text.ToUpper = pstrvalue.ToUpper Then
    '                        dd.SelectedIndex = intcount
    '                        Exit For
    '                    End If
    '                Next
    '            Else
    '                For intcount As Integer = 0 To dd.Items.Count - 1
    '                    If dd.Items(intcount).Value = pstrvalue Then
    '                        dd.SelectedIndex = intcount
    '                        Exit For
    '                    End If
    '                Next
    '            End If
    '        End If
    '    End Using
    'End Sub

    ''Private Sub populate(ByVal pstrReference As String)
    ''    Using New clslogger(log, className, "populate")
    ''        'poulate details 
    ''        Dim oDetails As New NysDat.clsCCPDat
    ''        oDetails = NysDat.clsCCPDat.populate(pstrReference)
    ''        txtname.Text = oDetails.CardHolderName
    ''        txtcardnumber1.Text = oDetails.CardNumber.Substring(0, 8)
    ''        txtcardnumber2.Text = oDetails.CardNumber.Substring(8)
    ''        txtcvv.Text = oDetails.CVV
    ''        txtemail.Text = oDetails.Email
    ''        txtpostalline1.Text = oDetails.PostalAddressLine1
    ''        txtpostalline2.Text = oDetails.PostalAddressLine2
    ''        txtpostalcity.Text = oDetails.PostalCity
    ''        txtpostalpostcode.Text = oDetails.PostalPostCode
    ''        txtaddressline1.Text = oDetails.RegisteredAddressLine1
    ''        txtaddressLine2.Text = oDetails.RegisteredAddressLine2
    ''        txtaddresscity.Text = oDetails.RegisteredCity
    ''        txtaddresspostcode.Text = oDetails.RegisteredPostCode
    ''        txtprocessedDate.Text = oDetails.ProcessedDate
    ''        chkProcessed.Checked = oDetails.Processed
    ''        changeDropDowns(ddexdateMM, oDetails.ExpiryDate.Substring(0, 2), False, False)
    ''        changeDropDowns(ddexdateYYYY, oDetails.ExpiryDate.Substring(3), False, False)
    ''    End Using
    ''End Sub

    'Private Function validateForm() As String
    '    Using New clslogger(log, className, "checkSave")
    '        Dim strMessage As String = ""

    '        If txtnysref.Text = "" Then
    '            Throw New EvoFriendlyException("Please entre Enquiry NYS Reference to continue!", "Info")
    '            strMessage = "Please entre Enquiry NYS Reference to continue!"
    '        End If

    '        If txtname.Text = "" Then
    '            Throw New EvoFriendlyException("Please provide card holder name to continue!", "Info")
    '            strMessage = "Please provide card holder name to continue!"
    '        End If

    '        If txtcardnumber1.Text = "" Then
    '            Throw New EvoFriendlyException("Please provide the card number to continue!", "Info")
    '            strMessage = "Please provide the card number to continue!"
    '        Else
    '            Dim mRegExp As New Regex(getConfig("Number"))
    '            If Not mRegExp.IsMatch(txtcardnumber1.Text) Then
    '                Throw New EvoFriendlyException("Credit Card number must be 16 digits only!. please amend!", "Info")
    '                strMessage = "Credit Card number must be 16 digits only! please amend!"
    '            End If
    '        End If


    '        If txtcardnumber2.Text = "" Then
    '            Throw New EvoFriendlyException("Please provide the card number to continue!", "Info")
    '            strMessage = "Please provide the card number to continue!"
    '        Else
    '            Dim mRegExp As New Regex(getConfig("Number"))
    '            If Not mRegExp.IsMatch(txtcardnumber2.Text) Then
    '                Throw New EvoFriendlyException("Credit Card number must be 16 digits only!. please amend!", "Info")
    '                strMessage = "Credit Card number must be 16 digits only! please amend!"
    '            End If
    '        End If

    '        If txtcvv.Text = "" Then
    '            Throw New EvoFriendlyException("Please provide CVV number to continue!", "Info")
    '            strMessage = "Please provide CVV number to continue!"
    '        Else
    '            Dim mRegExp As New Regex(getConfig("CVV"))
    '            If Not mRegExp.IsMatch(txtcvv.Text) Then
    '                Throw New EvoFriendlyException("CVV number must be 3 digits only!. please amend!", "Info")
    '                strMessage = "CVV number must be 3 digits only! please amend!"
    '            End If
    '        End If

    '        If ddexdateMM.SelectedIndex < 1 Then
    '            Throw New EvoFriendlyException("Please choose card expiry month from the list to continue", "Info")
    '            strMessage = "Please choose card expiry month from the list to continue"
    '        End If

    '        If ddexdateYYYY.SelectedIndex < 1 Then
    '            Throw New EvoFriendlyException("Please choose card expiry year from the list to continue!", "Info")
    '            strMessage = "Please choose card expiry year from the list to continue!"
    '        End If

    '        Dim strExDate As String = ddexdateMM.SelectedItem.Text & "/" & ddexdateYYYY.SelectedItem.Text
    '        Dim dToday As Date
    '        dToday = Date.Now()
    '        If CDate(strExDate) <= dToday Then
    '            Throw New EvoFriendlyException("You cannot select an expiry date earlier than this month! please amend!", "Info")
    '        End If

    '        If txtemail.Text = "" Then
    '            Throw New EvoFriendlyException("Please provide Email address for receipt to continue!", "Info")
    '            strMessage = "Please provide Email address for receipt to continue!"
    '        Else
    '            If Not Regex.IsMatch(txtemail.Text, emailRegex) Then
    '                Throw New EvoFriendlyException("Email address not valid, please amend!", "Check Details")
    '                strMessage = "Email address not valid, please amend!"
    '            End If
    '        End If

    '        If txtpostalline1.Text = "" Then
    '            Throw New EvoFriendlyException("Please provide first line of postal address to continue!", "Info")
    '            strMessage = "Please provide first line of postal address to continue!"
    '        End If

    '        If txtpostalcity.Text = "" Then
    '            Throw New EvoFriendlyException("Please provide City/Town name to continue!", "Info")
    '            strMessage = "Please provide City/Town name to continue!"
    '        End If

    '        If txtpostalpostcode.Text = "" Then
    '            Throw New EvoFriendlyException("Please provide post code to continue!", "Info")
    '            strMessage = "Please provide post code to continue!"
    '        End If

    '        If chkProcessed.Checked AndAlso txtprocessedDate.Text = "" Then
    '            Throw New EvoFriendlyException("Please enter card procedded date to continue!", "Info")
    '            strMessage = "Please enter card procedded date to continue!"
    '        End If

    '        If chkProcessed.Checked = False Then
    '            txtprocessedDate.Text = ""
    '        End If

    '        If txtprocessedDate.Text <> "" Then
    '            If CDate(txtprocessedDate.Text) <= dToday Then
    '                Throw New EvoFriendlyException("You cannot select an expiry date earlier than this month! please amend!", "Info")
    '            End If
    '        End If

    '        Return strMessage
    '    End Using
    'End Function

    'Protected Sub btnpopulate_Click(sender As Object, e As EventArgs) Handles btnpopulate.Click
    '    Using New clslogger(log, className, "btnpopulate_Click")
    '        Try
    '            If txtnysref.Text = "" Then
    '                Throw New EvoFriendlyException("Please enter Enquiry NYS refernce to continue!", "Info")
    '            Else
    '                'if payment recieved populate details - else throw ex
    '                If NysDat.clsCCPDat.checkReference(txtnysref.Text) > 0 Then
    '                    populate(txtnysref.Text)
    '                Else
    '                    Throw New EvoFriendlyException("No payment details for this booking were found in the database!", "Info")
    '                End If
    '            End If
    '        Catch ex As Exception
    '            handleexception(ex, "IAAdminCCP", Me.Page)
    '        End Try
    '    End Using
    'End Sub

    'Protected Sub btnClearboxes_Click(sender As Object, e As EventArgs) Handles btnClearboxes.Click
    '    Using New clslogger(log, className, "btnClearboxes_Click")
    '        Try
    '            clearBoxes()
    '        Catch ex As Exception
    '            handleexception(ex, "IAAdminCCP", Me.Page)
    '        End Try
    '    End Using
    'End Sub

    'Protected Sub btnsave_Click(sender As Object, e As EventArgs) Handles btnsave.Click
    '    Using New clslogger(log, className, "btnsave_Click")
    '        Try
    '            Dim strCheck As String = validateForm()
    '            If strCheck = "" Then
    '                Dim strExDate As String = ddexdateMM.SelectedItem.Text & "/" & ddexdateYYYY.SelectedItem.Text
    '                Dim intID As Integer = NysDat.clsCCPDat.checkReference(txtnysref.Text)
    '                Dim oDetails As New NysDat.clsCCPDat(intID, txtname.Text, txtcardnumber2.Text, txtcvv.Text, strExDate, txtemail.Text, txtpostalline1.Text, _
    '                                    txtpostalline2.Text, txtpostalcity.Text, txtpostalpostcode.Text, txtaddressline1.Text, txtaddressLine2.Text, txtaddresscity.Text, _
    '                                    txtaddresspostcode.Text, txtnysref.Text, chkProcessed.Checked, txtprocessedDate.Text)
    '                If oDetails.save > 0 Then
    '                    clearBoxes()
    '                    Throw New EvoFriendlyException("Details saved!", "Info")
    '                Else
    '                    Throw New EvoFriendlyException("An error occured while saving, please check details then try again later!", "Info")
    '                End If
    '            Else
    '                Throw New EvoFriendlyException(strCheck, "Info")
    '            End If
    '        Catch ex As Exception
    '            handleexception(ex, "IAAdminCCP", Me.Page)
    '        End Try
    '    End Using
    'End Sub

    'Protected Sub btndelete_Click(sender As Object, e As EventArgs) Handles btndelete.Click
    '    Using New clslogger(log, className, "btndelete_Click")
    '        Try
    '            Dim intResult As Integer
    '            intResult = NysDat.clsCCPDat.delete()

    '            If intResult > 0 Then
    '                lbldelete.ForeColor = Drawing.Color.Black
    '                lbldelete.Text = "Rercords sucessfully deleted!"
    '            Else
    '                lbldelete.ForeColor = Drawing.Color.Red
    '                lbldelete.Text = "Delete failed!"
    '            End If
    '        Catch ex As Exception
    '            handleexception(ex, "IAAdminCCP", Me.Page)
    '        End Try
    '    End Using
    'End Sub

    'Protected Sub btnlogout_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnlogout.Click
    '    Using New clslogger(log, className, "")
    '        Try
    '            Response.Redirect("IALogonAdmin.aspx")
    '        Catch ex As Exception
    '            handleexception(ex, "IAAdminCCP", Me.Page)
    '        End Try
    '    End Using
    'End Sub
End Class