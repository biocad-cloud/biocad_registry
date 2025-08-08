Imports System.Security.Cryptography
Imports biocad_storage
Imports BioNovoGene.BioDeep.Chemistry.NCBI.PubChem.DataSources
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Microsoft.VisualBasic.Linq
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

    Sub imports_all_plantcyc()
        For Each dir As String In "D:\datapool\plants".ListDirectory
            For Each subdir As String In dir.ListDirectory
                Dim biocyc As New Workspace(subdir)

                If biocyc.checkValid Then
                    Call New MetaCycImports(registry, biocyc).ImportsCompounds(topic:="plant")
                End If
            Next
        Next
    End Sub

    Sub importsUniprot()


        Pause()
    End Sub

    Sub imports_drugdata()
        Dim sources = "U:\pubchem\drugs".EnumerateFiles("*.json").ToArray
        Dim annotations = AnnotationJSON.GetAnnotations(sources).ToArray

        For Each annotation As Annotation In TqdmWrapper.Wrap(annotations)
            If annotation.LinkedRecords IsNot Nothing Then
                For Each cid As String In annotation.LinkedRecords.CID.SafeQuery

                Next
            End If
        Next

        Pause()
    End Sub
End Module
