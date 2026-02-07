Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Linq
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes
Imports registry_data.biocad_registryModel

Namespace Exports

    Public Module MetaboliteData

        <Extension>
        Public Iterator Function ExportSMILES(registry As biocad_registry, dbname As String) As IEnumerable(Of SMILESData)
            Dim page_size As Integer = 2000
            Dim offset As UInteger
            Dim page_data As SMILESData()

            For i As Integer = 1 To Integer.MaxValue
                offset = (i - 1) * page_size
                page_data = registry.metabolites _
                    .left_join("struct_data") _
                    .on(field("`struct_data`.metabolite_id") = field("`metabolites`.id")) _
                    .where(Not field(dbname).is_nothing, field(dbname) <> "") _
                    .limit(offset, page_size) _
                    .select(Of SMILESData)($"{dbname} AS xref_id",
                                           "smiles",
                                           "CONCAT('BioCAD', LPAD(metabolite_id, 11, '0')) AS id",
                                           "name",
                                           "formula",
                                           "exact_mass")
                If page_data.IsNullOrEmpty Then
                    Exit For
                Else
                    For Each data As SMILESData In page_data
                        Yield data
                    Next
                End If
            Next
        End Function

        <Extension>
        Public Function ExportIDMapping(registry As biocad_registry, db_field As String) As Dictionary(Of String, String())
            Dim id As String = "CONCAT('BioCAD', LPAD(id, 11, '0')) AS id"
            Dim db_xref As String = $"`{db_field}` AS db_xref"
            Dim mapping As ExportIDMapping() = registry.metabolites.where(field(db_field).char_length > 0).select(Of ExportIDMapping)(id, db_xref)

            Return mapping.SafeQuery _
                .GroupBy(Function(a) a.db_xref) _
                .ToDictionary(Function(a) a.Key,
                              Function(a)
                                  Return a _
                                      .Select(Function(map) map.id) _
                                      .Distinct _
                                      .ToArray
                              End Function)
        End Function

        <Extension>
        Public Function ExportTagList(registry As biocad_registry, topic As String) As IEnumerable(Of String)
            Return registry.ExportTagIDSet(topic).IteratesALL.Distinct.Select(Function(id) "BioCAD" & id.ToString.PadLeft(11, "0"c))
        End Function

        <Extension>
        Private Function ExportTagIDSet(registry As biocad_registry, topic As String) As IEnumerable(Of UInteger())

        End Function

        <Extension>
        Public Iterator Function ExportTagData(registry As biocad_registry, topic As String) As IEnumerable(Of MetaboliteTable)
            Dim class_id As UInteger = registry.biocad_vocabulary.GetDatabaseResource("RefMet").id

            For Each id As UInteger In registry.ExportTagIDSet(topic).IteratesALL.Distinct
                Dim meta As MetaboliteTable = registry.metabolites _
                    .left_join("struct_data") _
                    .on(field("`metabolites`.id") = field("`struct_data`.metabolite_id")) _
                    .where(field("`metabolites`.id") = id) _
                    .find(Of MetaboliteTable)("name",
                                              "formula",
                                              "exact_mass",
                                              "pubchem_cid",
                                              "CAST(chebi_id AS CHAR) AS chebi",
                                              "cas_id",
                                              "hmdb_id",
                                              "kegg_id",
                                              "lipidmaps_id",
                                              "drugbank_id",
                                              "biocyc",
                                              "mesh_id",
                                              "wikipedia",
                                              "smiles")
                Dim lineage As ontology() = registry.GetClassLineage(id, class_id)

                meta.id = "BioCAD" & id.ToString.PadLeft(11, "0"c)

                If Not lineage Is Nothing Then
                    meta.super_class = lineage.ElementAtOrDefault(0)?.term
                    meta.class = lineage.ElementAtOrDefault(1)?.term
                    meta.sub_class = lineage.ElementAtOrDefault(2)?.term
                End If

                Yield meta
            Next
        End Function

        <Extension>
        Public Function GetClassLineage(registry As biocad_registry, metabolite_id As UInteger, ontology_id As UInteger) As ontology()
            Dim term As biocad_registryModel.ontology = registry.metabolite_class _
                .left_join("ontology") _
                .on(field("`ontology`.id") = field("class_id")) _
                .where(field("ontology_id") = ontology_id,
                       field("metabolite_id") = metabolite_id) _
                .find(Of biocad_registryModel.ontology)("`ontology`.*")

            If term Is Nothing Then
                Return Nothing
            End If

            Dim lineage As New List(Of ontology) From {term}

            For i As Integer = 0 To Integer.MaxValue
                Dim is_a = registry.ontology_relation.where(field("term_id") = term.id).find(Of ontology_relation)

                If is_a IsNot Nothing Then
                    term = registry.ontology.where(field("id") = is_a.is_a).find(Of ontology)

                    If term IsNot Nothing Then
                        Call lineage.Add(term)
                    Else
                        Exit For
                    End If
                Else
                    Exit For
                End If
            Next

            Return lineage.AsEnumerable.Reverse.ToArray
        End Function
    End Module

    Friend Class ExportIDMapping

        <DatabaseField> Public Property id As String
        <DatabaseField> Public Property db_xref As String

    End Class

    Public Class MetaboliteTable

        Public Property id As String

        <DatabaseField> Public Property name As String
        <DatabaseField> Public Property formula As String
        <DatabaseField> Public Property exact_mass As Double
        <DatabaseField> Public Property pubchem_cid As UInteger
        <DatabaseField> Public Property chebi As String
        <DatabaseField> Public Property cas_id As String
        <DatabaseField> Public Property hmdb_id As String
        <DatabaseField> Public Property kegg_id As String
        <DatabaseField> Public Property lipidmaps_id As String
        <DatabaseField> Public Property drugbank_id As String
        <DatabaseField> Public Property biocyc As String
        <DatabaseField> Public Property mesh_id As String
        <DatabaseField> Public Property wikipedia As String
        <DatabaseField> Public Property smiles As String

        Public Property super_class As String
        Public Property [class] As String
        Public Property sub_class As String

    End Class
End Namespace