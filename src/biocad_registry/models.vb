
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports registry_data
Imports registry_data.biocad_registryModel
Imports SMRUCC.Rsharp.Runtime.Internal.[Object]
Imports SMRUCC.Rsharp.Runtime.Interop
Imports SMRUCC.Rsharp.Runtime.Vectorization

<Package("models")>
<RTypeExport("cad_registry", GetType(biocad_registry))>
Public Module registry_models

    Friend Sub Main()
    End Sub

    ''' <summary>
    ''' make updates of the compartment location metadata
    ''' </summary>
    ''' <param name="registry"></param>
    ''' <param name="locations"></param>
    <ExportAPI("update_location")>
    Public Sub update_location(registry As biocad_registry, locations As dataframe)
        Dim name As String() = CLRVector.asCharacter(locations!name)
        Dim zh_names As String() = CLRVector.asCharacter(locations!zh_name)
        Dim membrane As Integer() = CLRVector.asInteger(locations!membrane)
        Dim updates As CommitTransaction = registry.compartment_location.ignore.open_transaction

        For i As Integer = 0 To locations.nrows - 1
            Dim loc As compartment_location = registry.compartment_location.where(field("name") = name(i)).find(Of compartment_location)

            If Not loc Is Nothing Then
                Call updates.add(registry.compartment_location _
                            .where(field("id") = loc.id) _
                            .save_sql(field("zh_name") = zh_names(i),
                                      field("membrane") = membrane(i)))
            End If
        Next

        Call updates.commit()
    End Sub
End Module
