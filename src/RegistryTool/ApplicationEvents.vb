Imports Oracle.LinuxCompatibility.MySQL.Uri

Namespace My

    ' The following events are available for MyApplication:
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.

    ' **NEW** ApplyApplicationDefaults: Raised when the application queries default values to be set for the application.

    ' Example:
    ' Private Sub MyApplication_ApplyApplicationDefaults(sender As Object, e As ApplyApplicationDefaultsEventArgs) Handles Me.ApplyApplicationDefaults
    '
    '   ' Setting the application-wide default Font:
    '   e.Font = New Font(FontFamily.GenericSansSerif, 12, FontStyle.Regular)
    '
    '   ' Setting the HighDpiMode for the Application:
    '   e.HighDpiMode = HighDpiMode.PerMonitorV2
    '
    '   ' If a splash dialog is used, this sets the minimum display time:
    '   e.MinimumSplashScreenDisplayTime = 4000
    ' End Sub

    Partial Friend Class MyApplication

        Public Shared ReadOnly Property biocad_registry As biocad_registry.biocad_registry

        Public Shared Function Load() As Boolean
            Dim mysqli As New ConnectionUri With {
                .Database = "cad_registry",
                .IPAddress = "localhost",
                .Password = 123456,
                .Port = 3306,
                .User = "root"
            }

            _biocad_registry = New biocad_registry.biocad_registry(mysqli)

            Return biocad_registry.getDriver.Ping >= 0
        End Function

    End Class
End Namespace
