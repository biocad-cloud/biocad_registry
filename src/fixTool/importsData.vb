Imports biocad_storage

Module importsData

    Sub ImportsChebi()
        ' Dim workflow As New GeneOntologyImports(Program.registry, "G:\biocad_registry\data\go.obo")
        ' Dim workflow As New ChEBIOntologyImports(Program.registry, "G:\biocad_registry\data\chebi_lite.obo")
        '  workflow.RunImports()
        ' workflow.ImportsOntology()
        Dim hmdb As New HMDBImports(Program.registry, "U:\hmdb\hmdb_metabolites.xml")
        hmdb.Imports()

        Pause()
    End Sub
End Module
