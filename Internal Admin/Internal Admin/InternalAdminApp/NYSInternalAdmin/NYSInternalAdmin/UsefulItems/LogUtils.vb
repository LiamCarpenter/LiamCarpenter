Option Strict On
Option Explicit On

Namespace NSLogUtils
    Public Module LogUtils

        Private log As log4net.ILog = _
           log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        Public Function reloadConfig() As String
            'Try
            '    Dim results As New System.Text.StringBuilder
            Dim configurationAppSettings As New System.Configuration.AppSettingsReader
            Dim filename As String = CType(configurationAppSettings.GetValue("log4netconfig", _
                                                                GetType(System.String)), String)

            log4net.Config.XmlConfigurator.ConfigureAndWatch(New System.IO.FileInfo(filename))
            log.Info("Config refresh")

            Return ""
        End Function

    End Module
End Namespace