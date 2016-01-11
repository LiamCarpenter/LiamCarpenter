Imports Microsoft.Reporting.WebForms

Partial Public Class IAEnquiryGovConfirmedVenueReport
    Inherits clsNYS
    Private Shared ReadOnly className As String

    Shared Sub New()
        className = System.Reflection.MethodBase. _
        GetCurrentMethod().DeclaringType.FullName
        log = log4net.LogManager.GetLogger(className)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                Dim strRet As String = setUser()
                If strRet.StartsWith("ERROR") Then
                    Response.Redirect("IALogonAdmin.aspx?User=falseX")
                End If
                btnlogout.Attributes.Add("OnMouseOver", "this.src='images/logout_over.gif';")
                btnlogout.Attributes.Add("OnMouseOut", "this.src='images/logout_out.gif';")
                txtcompany.Text = CStr(Session.Item("clientid"))
            End If
            Me.Title = "Management Information by NYS Corporate"
        Catch ex As Exception
            handleexception(ex, "IAEnquiryGovConfirmedVenueReport", Me.Page)
        End Try
    End Sub

    Protected Sub btnlogout_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnlogout.Click
        Response.Redirect("IALogonAdmin.aspx")
    End Sub

    Private Sub IAEnquiryGovConfirmedVenueReport_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Using New clslogger(log, className, "IAEnquiryGovConfirmedVenueReport_PreRender")
            Try
                Dim fp As ucReportMenu = DirectCast(LoadControl("UserControls/ucReportMenu.ascx"),  _
                    ucReportMenu)
                fp.pageName = "IAEnquiryGovConfirmedVenueReport"
                phMenu.Controls.Add(fp)
            Catch ex As Exception
                handleexception(ex, "IAEnquiryGovConfirmedVenueReport", Me.Page)
            End Try
        End Using
    End Sub

End Class