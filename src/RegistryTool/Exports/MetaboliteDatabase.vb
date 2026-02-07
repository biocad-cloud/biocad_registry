Imports System.IO
Imports System.Runtime.CompilerServices
Imports BioNovoGene.BioDeep.Chemistry.MetaLib
Imports BioNovoGene.BioDeep.Chemistry.MetaLib.CrossReference
Imports Microsoft.VisualBasic.Data.Framework.IO
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Linq
Imports registry_data
Imports registry_exports
Imports RegistryTool.My
Imports SMRUCC.genomics.Assembly.NCBI.GenBank
Imports Metadata = BioNovoGene.BioDeep.Chemistry.MetaLib.Models.MetaLib


Module MetaboliteDatabase

    Public Function ExportLocal(println As Action(Of String), filename As String, subset$) As Boolean
        Dim registry As biocad_registry = MyApplication.biocad_registry
        Dim class_id As UInteger = registry.biocad_vocabulary.GetDatabaseResource("RefMet").id

        If filename.ExtensionSuffix("csv") Then
            Call registry.ExportTable(println, class_id, subset, filename)
        Else
            Call registry.ExportPack(println, class_id, subset, filename)
        End If

        Return True
    End Function

    <Extension>
    Private Sub ExportPack(registry As biocad_registry, println As Action(Of String), class_id As UInteger, subset$, filename As String)
        Dim repo As New RepositoryWriter(filename.Open(FileMode.OpenOrCreate, doClear:=True))
        Dim block As i32 = 1
        Dim i As Integer = 0

        Call println("Export metabolite annotation into local repository...")

        For Each mol As Metadata In ExportMetaboliteData.ExportMetabolites(MyApplication.biocad_registry,
                                                                           dbname:=Nothing,
                                                                           ontology_id:=class_id)
            If i > 2000 Then
                i = 0
                repo.CommitBlock()
                println($"Commit block data: {++block}")
            Else
                i += 1
                repo.Add(mol)
            End If
        Next

        Call repo.Dispose()
    End Sub

    <Extension>
    Private Sub ExportTable(registry As biocad_registry, println As Action(Of String), class_id As UInteger, subset$, filename As String)
        Dim csv As New StreamWriter(filename.Open(FileMode.OpenOrCreate, doClear:=True))
        Dim row As New RowObject From {"ID", "name", "formula", "exact_mass", "cas_id", "kegg_id", "hmdb_id", "chebi_id", "pubchem_cid", "lipidmaps_id", "smiles"}
        Dim db_xrefs As xref
        Dim i As Integer = 0
        Dim block As i32 = 1

        Call csv.WriteLine(row.AsLine)
        Call row.Clear()
        Call println("Export metabolite annotation into table sheet...")

        For Each mol As Metadata In ExportMetaboliteData.ExportMetabolites(MyApplication.biocad_registry, subset, ontology_id:=class_id)
            db_xrefs = mol.xref
            row.AddRange({mol.ID, mol.name, mol.formula, mol.exact_mass, db_xrefs.CAS.FirstOrDefault, db_xrefs.KEGG, db_xrefs.HMDB, db_xrefs.chebi, db_xrefs.pubchem, db_xrefs.lipidmaps, db_xrefs.SMILES})
            csv.WriteLine(row.AsLine)
            row.Clear()
            i += 1

            If i > 2000 Then
                i = 0
                println($"Commit block data: {++block}")
                csv.Flush()
            End If
        Next

        Call csv.Flush()
        Call csv.Close()
        Call csv.Dispose()
    End Sub

End Module
