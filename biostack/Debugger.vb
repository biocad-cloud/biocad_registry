Imports SMRUCC.WebCloud.d3js.Network
Imports SMRUCC.WebCloud.HTTPInternal
Imports NetGraph = Microsoft.VisualBasic.Data.visualize.Network.FileStream.Network

Module Debugger

    Sub Main()
        Call D3NetworkTest
        Call DebuggerAPI.Start(wwwroot:="../wwwroot", threads:=0)
    End Sub

    Sub D3NetworkTest
        Call NetGraph _
            .Load("C:\Users\xieguigang\OneDrive\4.7\sqq\drugbank-drug-pathway.knowledge_network") _
            .FromNetwork _
            .SaveTo("./net.json")
    End Sub
End Module
