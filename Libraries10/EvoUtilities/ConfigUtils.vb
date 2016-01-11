Option Strict On
Option Explicit On

Imports System.Configuration.ConfigurationSettings

Public NotInheritable Class ConfigUtils

    Private Shared configurationAppSettings As New System.Configuration.AppSettingsReader

    Private Sub New()
    End Sub

    Public Shared Function getConnection() As String
        Return DirectCast(System.Web.Configuration.WebConfigurationManager.ConnectionStrings _
            ("SqlServerConnStr").ConnectionString, String)
    End Function

    'CR - FileUploadApp, uses this new one for getting the encryption key
    Public Shared Function getEncStr() As String
        Return DirectCast(System.Web.Configuration.WebConfigurationManager.ConnectionStrings _
            ("EncStr").ConnectionString, String)
    End Function

    'SA - CreditCardApp, used to get the first 8 digits of card number 
    Public Shared Function getEncNumber() As String
        Return DirectCast(System.Web.Configuration.WebConfigurationManager.ConnectionStrings _
            ("EncNumber").ConnectionString, String)
    End Function

    Public Shared Function getCubitConnection() As String
        Return DirectCast(System.Web.Configuration.WebConfigurationManager.ConnectionStrings _
            ("SqlServerConnStrCubit").ConnectionString, String)
    End Function

    Public Shared Function getMevisConnection() As String
        Return DirectCast(System.Web.Configuration.WebConfigurationManager.ConnectionStrings _
            ("SqlServerMevisConnStr").ConnectionString, String)
    End Function

    Public Shared Function getGIDSConnection() As String
        Return DirectCast(System.Web.Configuration.WebConfigurationManager.ConnectionStrings _
            ("SqlServerConnStrGIDS").ConnectionString, String)
    End Function

    Public Shared Function getSSOConnection() As String
        Return DirectCast(System.Web.Configuration.WebConfigurationManager.ConnectionStrings _
            ("SqlServerConnStrSSO").ConnectionString, String)
    End Function

    Public Shared Function getChipConnection() As String
        Return DirectCast(System.Web.Configuration.WebConfigurationManager.ConnectionStrings _
            ("SqlServerConnStrCHIP").ConnectionString, String)
    End Function

    Public Shared Function getConfig(ByVal key As String) As String
        Return DirectCast(configurationAppSettings.GetValue(key, GetType(System.String)), String)
    End Function

    Public Shared Function getOptionalConfig(ByVal key As String) As String
        Return DirectCast(System.Web.Configuration.WebConfigurationManager.GetSection("appSettings"),  _
            System.Collections.Specialized.NameValueCollection).Item(key)
    End Function

End Class
