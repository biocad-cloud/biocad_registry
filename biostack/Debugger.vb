Imports SMRUCC.WebCloud.d3js.Network
Imports SMRUCC.WebCloud.HTTPInternal
Imports NetGraph = Microsoft.VisualBasic.Data.visualize.Network.FileStream.Network
Imports Microsoft.VisualBasic.Serialization.JSON

Module Debugger

    Sub Main()
        Call App.JoinVariable("database", "SMRUCC-cloud")
        Call App.JoinVariable("host", "localhost")
        Call App.JoinVariable("port", 3306)
        Call App.JoinVariable("user", "root")
        Call App.JoinVariable("password", "1234")

        Call D3NetworkTest()
        Call DebuggerAPI.Start(wwwroot:="../wwwroot", threads:=0)
    End Sub

    Sub D3NetworkTest()
        Call NetGraph _
            .Load("C:\Users\xieguigang\OneDrive\4.7\sqq\drugbank-drug-pathway.knowledge_network") _
            .FromNetwork(True) _
            .SaveTo("./net.json")
    End Sub
End Module
