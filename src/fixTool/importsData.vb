Imports biocad_storage

Module importsData

    Sub ImportsChebi()
        Dim workflow As New ChEBIOntologyImports(Program.registry, "G:\biocad_registry\data\chebi_lite.obo")
        workflow.RunImports()


        Pause()
    End Sub
End Module
