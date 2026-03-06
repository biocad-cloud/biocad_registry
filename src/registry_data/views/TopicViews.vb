Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports registry_data.biocad_registryModel

Public Module TopicViews

    <Extension>
    Public Sub PlantNP(registry As biocad_registry)
        Dim np_topic As UInteger = registry.biocad_vocabulary.GetTopic("Plant Natural Products").id
        ' Viridiplantae
        Dim plant_tax As UInteger = 33090

        Call registry.NaturalProductLib(np_topic, plant_tax)
    End Sub

    <Extension>
    Public Sub MicrobialNP(registry As biocad_registry)
        Dim np_topic As UInteger = registry.biocad_vocabulary.GetTopic("Microbial Natural Products").id
        ' bacterials
        Dim bacterial_tax As UInteger = 2
        ' fungi
        Dim fungi As UInteger = 4751
        ' archaea
        Dim archaea As UInteger = 2157

        Call registry.NaturalProductLib(np_topic, bacterial_tax)
    End Sub

    <Extension>
    Private Sub NaturalProductLib(registry As biocad_registry, np_topic As UInteger, root_tax As UInteger)
        Dim page_size As Integer = 5000
        Dim metabolite_type As UInteger = registry.biocad_vocabulary.metabolite_type

        For page As Integer = 1 To Integer.MaxValue
            Dim page_data = registry.organism_source _
                .where(field("organism_id") > 0) _
                .limit((page - 1) * page_size, page_size) _
                .select(Of biocad_registryModel.organism_source)

            If page_data.IsNullOrEmpty Then
                Exit For
            End If

            For Each link As biocad_registryModel.organism_source In TqdmWrapper.Wrap(page_data)
                If registry.CheckLineage(link.organism_id, root_tax) Then
                    Dim m As metabolites = registry.metabolites _
                        .where(field("id") = link.metabolite_id) _
                        .find(Of metabolites)

                    If m Is Nothing Then
                        Continue For
                    End If

                    Dim meta As registry_resolver = registry.SymbolRegister(m)

                    If meta Is Nothing Then
                        ' name is too long, insert error
                        Continue For
                    End If

                    Dim check_topic = registry.topic _
                        .where(field("topic_id") = np_topic,
                               field("model_id") = meta.id) _
                        .find(Of biocad_registryModel.topic)

                    If check_topic Is Nothing Then
                        Call registry.topic.add(
                            field("topic_id") = np_topic,
                            field("model_id") = meta.id,
                            field("type") = 0,
                            field("note") = link.note
                        )
                    End If
                End If
            Next
        Next
    End Sub
End Module
