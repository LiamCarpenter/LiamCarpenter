Imports NysDat

Public Class AMTemplate
    Inherits clsNYS

    Private Shared ReadOnly className As String

    Shared Sub New()
        className = System.Reflection.MethodBase. _
        GetCurrentMethod().DeclaringType.FullName
        log = log4net.LogManager.GetLogger(className)
    End Sub
    Protected Sub btnlogout_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnlogout.Click
        Using New clslogger(log, className, "btnlogout_Click")
            Try
                Response.Redirect("IALogonAdmin.aspx")
            Catch ex As Exception
                handleexception(ex, "AMTemplate", Me.Page)
            End Try
        End Using
    End Sub
    Private Sub IAClientStatements_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Using New clslogger(log, className, "IAClientStatements_PreRender")
            Try
                Dim fp As ucReportMenu = DirectCast(LoadControl("UserControls/ucReportMenu.ascx"),  _
                    ucReportMenu)
                fp.pageName = "AMTemplate"
                phMenu.Controls.Add(fp)
            Catch ex As Exception
                handleexception(ex, "AMTemplate", Me.Page)
            End Try
        End Using
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Using New clslogger(log, className, "Page_Load")
            Try
                If Not IsPostBack Then
                    'Dim strRet As String = setUser()
                    'If strRet.StartsWith("ERROR") Then
                    '    Response.Redirect("IALogonAdmin.aspx?User=falseX")
                    'End If

                    btnlogout.Attributes.Add("OnMouseOver", "this.src='images/logout_over.gif';")
                    btnlogout.Attributes.Add("OnMouseOut", "this.src='images/logout_out.gif';")
                    but.Attributes.Add("OnMouseOver", "this.src='images/run_over.gif';")
                    but.Attributes.Add("OnMouseOut", "this.src='images/run_out.gif';")
                    but.Attributes.Add("Onclick", "javascript:toggleDiv('pnTrans');")

                End If
                Me.Title = CStr(Session.Item("clientname")) & " Management Information by NYS Corporate"
            Catch ex As Exception
                handleexception(ex, "IAClientStatements", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub but_Click(sender As Object, e As EventArgs) Handles but.Click

        Dim oEnqs As New List(Of clsEnquiryDat)
        oEnqs = clsEnquiryDat.checkETPenquiryAM(0)

    End Sub
End Class