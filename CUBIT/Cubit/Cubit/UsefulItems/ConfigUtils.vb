Option Strict On
Option Explicit On

Namespace NSConfigUtils
    Public Module ConfigUtils

        Private configurationAppSettings As New System.Configuration.AppSettingsReader

        Public Function getConfig(ByVal key As String) As String
            Return CType(configurationAppSettings.GetValue(key, GetType(System.String)), String)
        End Function

    End Module
End Namespace