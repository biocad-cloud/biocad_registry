Imports biocad_storage
Imports SMRUCC.genomics.Data.BioCyc

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

    Sub importsMetaCyc()
        Dim biocyc As Workspace = Workspace.Open("F:\ecoli\29.0")

        ' Call New MetaCycImports(registry, biocyc).ImportsTranscriptUnits()
        Call New MetaCycImports(registry, biocyc).ImportsCompounds(topic:="bacteria")
    End Sub

    Sub importsUniprot()


        Pause()
    End Sub
End Module
