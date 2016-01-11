Option Strict On
Option Explicit On

Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports MonoSoftware.Web.Dialogs
Imports System.Collections.Generic
Imports EvoUtilities.CollectionUtils
Imports EvoUtilities.Utils

Imports System.Globalization


Namespace NysMaintainScroll

    Public Class MaintainScrollPanel
        Inherits Panel
        Public Shared ReadOnly vbNewLine As String = Microsoft.VisualBasic.vbNewLine


        Private Function makeHiddenField(ByVal id As String) As HiddenField
            Dim ret As New HiddenField()
            ret.ID = ClientID & "_" & id
            Return ret
        End Function

        Private WithEvents hdnScrollTop As HiddenField
        Private WithEvents hdnScrollLeft As HiddenField

        Private Sub MaintainScrollPanel_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            'If Not Page.IsPostBack Then
            EnsureChildControls()
            'End If
        End Sub

        Protected Overrides Sub CreateChildControls()
            hdnScrollTop = makeHiddenField("hdnScrollTop")
            hdnScrollLeft = makeHiddenField("hdnScrollLeft")
            'hack: add these here to get postback data
            Controls.Add(hdnScrollTop)
            Controls.Add(hdnScrollLeft)
            MyBase.CreateChildControls()
        End Sub

        Private Function getJS() As String
            Return _
            "function saveScroll(id, fieldTopID, fieldLeftID)" & vbNewLine & _
            "{" & vbNewLine & _
            "    var elem = document.getElementById(id);" & vbNewLine & _
            "    var fieldTop = document.getElementById(fieldTopID);" & vbNewLine & _
            "    var fieldLeft = document.getElementById(fieldLeftID);" & vbNewLine & _
            "    fieldTop.value = elem.scrollTop;" & vbNewLine & _
            "    fieldLeft.value = elem.scrollLeft;" & vbNewLine & _
            "}" & vbNewLine & _
            vbNewLine & _
            "function restoreScroll(id, fieldTopID, fieldLeftID)" & vbNewLine & _
            "{" & vbNewLine & _
            "    var elem = document.getElementById(id);" & vbNewLine & _
            "    var fieldTop = document.getElementById(fieldTopID);" & vbNewLine & _
            "    var fieldLeft = document.getElementById(fieldLeftID);" & vbNewLine & _
            "    elem.scrollTop = fieldTop.value;" & vbNewLine & _
            "    elem.scrollLeft = fieldLeft.value;" & vbNewLine & _
            "}" & vbNewLine & _
            "restoreScroll('" & ClientID & _
                "', '" & hdnScrollTop.ClientID & "','" & _
                hdnScrollLeft.ClientID & "');" & vbNewLine

        End Function

        Private Sub MaintainScrollPanel_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            If Not Controls.Contains(hdnScrollTop) Then
                Controls.Add(hdnScrollTop)
            End If
            If Not Controls.Contains(hdnScrollTop) Then
                Controls.Add(hdnScrollTop)
            End If
            If Visible Then
                Page.ClientScript.RegisterStartupScript(Me.GetType, ID, getJS(), True)

                ' ScriptManager.RegisterStartupScript(Page, Me.GetType, ID, getJS(), True)
                Attributes("onscroll") = "saveScroll('" & ClientID & _
                    "', '" & hdnScrollTop.ClientID & "','" & hdnScrollLeft.ClientID & "')"
            End If
        End Sub

        'Protected Overrides Sub render(ByVal writer As HtmlTextWriter)
        '    hdnScrollTop.RenderControl(writer)
        '    hdnScrollLeft.RenderControl(writer)
        '    MyBase.Render(writer)
        'End Sub



        'Private Sub hdnScrollTop_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnScrollTop.ValueChanged
        '    System.Diagnostics.Debug.WriteLine("changed to " & hdnScrollTop.Value)
        'End Sub
    End Class

End Namespace
