Imports System.Runtime.CompilerServices
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder

Module Translations

    <Extension>
    Public Sub TranslateOntology(registry As biocad_registry, ontology As String)
        Dim ontology_id As UInteger = registry.biocad_vocabulary.GetDatabaseResource(ontology).id
        Dim terms = registry.ontology _
            .where(field("ontology_id") = ontology_id,
                   field("term_zh").char_length = 0) _
            .select(Of biocad_registryModel.ontology)

        For Each term As biocad_registryModel.ontology In terms

        Next
    End Sub

End Module
