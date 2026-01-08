Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Microsoft.VisualBasic.Linq
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports SMRUCC.genomics.Assembly.KEGG.DBGET

Public Module ProteinModels

    ''' <summary>
    ''' Use KEGG orthology as protein reference model
    ''' </summary>
    ''' <param name="registry"></param>
    ''' <param name="ko"></param>
    <Extension>
    Public Sub SetupKOModels(registry As biocad_registry, ko As IEnumerable(Of KOrthology))
        Dim vocabulary As New biocad_vocabulary(registry)
        Dim kegg_db As UInteger = vocabulary.db_kegg
        Dim prot_type As UInteger = vocabulary.protein_type
        Dim ec_number As UInteger = vocabulary.db_ECNumber
        Dim xrefs As CommitTransaction = registry.db_xrefs.ignore.open_transaction

        For Each term As KOrthology In TqdmWrapper.Wrap(ko.OrderBy(Function(k) k.function).ToArray)
            Dim name As String = term.geneNames.JoinBy(", ")
            Dim prot As biocad_registryModel.protein = registry.protein _
                .where(field("name") = name) _
                .find(Of biocad_registryModel.protein)

            If prot Is Nothing Then
                Call registry.protein.add(
                    field("name") = name,
                    field("template") = 0,
                    field("pdb_data") = 0,
                    field("function") = term.function,
                    field("note") = term.ToString
                )

                prot = registry.protein _
                    .where(field("name") = name) _
                    .order_by("id", desc:=True) _
                    .find(Of biocad_registryModel.protein)
            End If

            Call xrefs.ignore.add(field("obj_id") = prot.id, field("type") = prot_type, field("db_name") = kegg_db, field("db_xref") = term.KO_id, field("db_source") = kegg_db)

            For Each ec As String In term.EC_number.SafeQuery
                Call xrefs.ignore.add(field("obj_id") = prot.id, field("type") = prot_type, field("db_name") = ec_number, field("db_xref") = ec, field("db_source") = kegg_db)
            Next
        Next

        Call xrefs.commit()
    End Sub
End Module
