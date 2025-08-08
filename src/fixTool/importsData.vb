Imports biocad_storage
Imports BioNovoGene.BioDeep.Chemistry.NCBI.PubChem.DataSources
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Microsoft.VisualBasic.Linq
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
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
        Dim pubchem_key = registry.vocabulary_terms.pubchem_term
        Dim excludes As UInteger() = {"acetic acid metabolism",
"bacteria",
"banana",
"barley",
"chinese baijiu",
"cooking",
"flavonoid",
"fragrance",
"fruit",
"glutathione",
"lignin biosynthesis",
"natural products",
"perfume",
"phenylpropanoid",
"phytochemicals",
"phytohormone",
"phytosterols",
"plant",
"plant hormone",
"plant natural products",
"rice",
"rotten odor",
"stripe rust resistance",
"tea",
"wheat",
"wine"}.Select(Function(name) registry.getVocabulary(name, "Topic", [readonly]:=True)).ToArray

        Dim drug_id = registry.getVocabulary("drug", "Topic")
        Dim drug_link = registry.molecule_tags.open_transaction.ignore
        Dim bar As Tqdm.ProgressBar = Nothing

        For Each annotation As Annotation In TqdmWrapper.Wrap(annotations, bar:=bar)
            If annotation.LinkedRecords IsNot Nothing Then
                Call bar.SetLabel(annotation.Name)

                For Each cid As String In annotation.LinkedRecords.CID.SafeQuery
                    Dim mols = registry.db_xrefs.where(field("db_key") = pubchem_key, field("xref") = cid).select(Of biocad_registryModel.db_xrefs)

                    For Each mol In mols
                        For Each eid As UInteger In excludes
                            drug_link.add(registry.molecule_tags.where(field("molecule_id") = mol.obj_id, field("tag_id") = eid).delete_sql)
                        Next

                        drug_link.add(field("molecule_id") = mol.obj_id,
                                      field("tag_id") = drug_id,
                                      field("description") = annotation.Name)
                    Next
                Next
            End If
        Next

        Call drug_link.commit()

        Pause()
    End Sub
End Module
