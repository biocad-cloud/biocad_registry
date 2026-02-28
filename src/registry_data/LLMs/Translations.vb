Imports System.Runtime.CompilerServices
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder

Public Module Translations

    <Extension>
    Public Sub TranslateOntology(registry As biocad_registry, ontology As String)
        Dim ontology_id As UInteger = registry.biocad_vocabulary.GetDatabaseResource(ontology).id
        Dim terms = registry.ontology _
            .where(field("ontology_id") = ontology_id,
                   field("term_zh").char_length = 0) _
            .select(Of biocad_registryModel.ontology)

        For Each term As biocad_registryModel.ontology In terms
            Dim prompt As String = $"将下面的这个化合物分类词条名称翻译为中文：'{term.term}'，如果没有正式的翻译，请进行音译。使用下面的json格式返回结果给我以方便我进行数据解析：{{""zh_name"": ""translated_name""}}"
            Dim zh_name As String = TranslatedName.DecodeLLMTranslateOutput(LLMs.LLMsTalk(prompt))

            If Not zh_name.StringEmpty(, True) Then
                Call registry.ontology _
                    .where(field("id") = term.id) _
                    .save(field("term_zh") = zh_name)
            End If
        Next
    End Sub

End Module
