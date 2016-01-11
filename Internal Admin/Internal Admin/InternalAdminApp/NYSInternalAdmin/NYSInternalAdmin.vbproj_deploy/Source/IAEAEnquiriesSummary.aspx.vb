Imports Microsoft.Reporting.WebForms
'R2.20E - creating new page for EA enquiries summary

Partial Public Class IAEAEnquiriesSummary
    Inherits clsNYS

    'Private Shared ReadOnly className As String
    'Shared Sub New()
    '    className = System.Reflection.MethodBase. _
    '    GetCurrentMethod().DeclaringType.FullName
    '    log = log4net.LogManager.GetLogger(className)
    'End Sub
    'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    '    Try
    '        If Not IsPostBack Then
    '            Dim strRet As String = setUser()
    '            If strRet.StartsWith("ERROR") Then
    '                Response.Redirect("IALogonAdmin.aspx?User=falseX")
    '            End If

    '            Dim oUser As NysDat.clsSystemNYSUser
    '            oUser = CType(Session.Item("loggedinuser"), NysDat.clsSystemNYSUser)
    '            If getConfig("CCPUsers").ToString.ToLower.Contains(oUser.Systemnysuserlastname.ToLower) Then
    '                btnlogout.Attributes.Add("OnMouseOver", "this.src='images/logout_over.gif';")
    '                btnlogout.Attributes.Add("OnMouseOut", "this.src='images/logout_out.gif';")
    '                btnrefresh.Attributes.Add("OnMouseOver", "this.src='images/refresh_over.gif';")
    '                btnrefresh.Attributes.Add("OnMouseOut", "this.src='images/refresh_out.gif';")
    '                btnrefresh.Attributes.Add("Onclick", "javascript:toggleDiv('pnTrans');")

    '                txtcompany.Text = 100039 'EA only, can be changed in the future 
    '            End If
    '        End If
    '    Catch ex As Exception
    '        handleexception(ex, "IAEAEnquiries", Me.Page)
    '    End Try
    'End Sub

    'Private Sub IAEAEnquiries_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
    '    Using New clslogger(log, className, "IAEAEnquiries_PreRender")
    '        Try
    '            Dim fp As ucReportMenu = DirectCast(LoadControl("UserControls/ucReportMenu.ascx"),  _
    '                ucReportMenu)
    '            fp.pageName = "IAEAEnquiries"
    '            phMenu.Controls.Add(fp)
    '        Catch ex As Exception
    '            handleexception(ex, "IAEAEnquiries", Me.Page)
    '        End Try
    '    End Using
    'End Sub

    'Protected Sub btnlogout_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnlogout.Click
    '    Try
    '        Response.Redirect("IALogonAdmin.aspx")
    '    Catch ex As Exception
    '        handleexception(ex, "IAEAEnquiries", Me.Page)
    '    End Try
    'End Sub

    'Protected Sub btnrefresh_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnrefresh.Click
    '    Try
    '        rvEAEnquiries.LocalReport.Refresh()
    '    Catch ex As Exception
    '        handleexception(ex, "IAEAEnquiries", Me.Page)
    '    End Try
    'End Sub
End Class