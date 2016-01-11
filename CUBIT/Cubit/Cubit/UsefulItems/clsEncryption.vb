Imports System.Web.Configuration
Imports System.Web.Security
Imports System.Configuration
Imports System.Web.HttpServerUtility

Public Class clsEncryption
    
    Public Shared Sub EncryptConnStr(ByVal protectionProvider As String)
        '---open the web.config file

        Dim config As Configuration = _
           WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath)
        '---indicate the section to protect
        Dim section As ConfigurationSection = _
           config.Sections("connectionStrings")
        '---specify the protection provider
        If Not section.SectionInformation.IsProtected Then
            section.SectionInformation.ProtectSection(protectionProvider)
            '---Apple the protection and update
            config.Save()
        End If
    End Sub

    Public Shared Sub DecryptConnStr()
        Dim path As String = System.AppDomain.CurrentDomain.BaseDirectory.ToString
        Dim config As Configuration = _
           WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath)
        Dim section As ConfigurationSection = _
           config.Sections("connectionStrings")
        If section.SectionInformation.IsProtected Then
            section.SectionInformation.UnprotectSection()
            config.Save()
        End If
    End Sub

End Class
