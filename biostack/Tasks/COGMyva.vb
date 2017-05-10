Imports SMRUCC.WebCloud.HTTPInternal.Platform

Public Class COGMyva : Inherits TaskModel

    Protected Overrides Function contents() As String()
         Return {
            "Blast+ myva database search", 
            "Export blastp table", 
            "COG annotation", 
            "Data Plot"
        }
    End Function
End Class
