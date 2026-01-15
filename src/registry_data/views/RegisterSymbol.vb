Imports System.Runtime.CompilerServices
Imports registry_data.biocad_registryModel
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder

Public Module RegisterSymbol

    <Extension>
    Public Function makeSymbol(name As String) As String
        Return Strings.Trim(name).Replace("(", "_").Replace(")", "_").Replace("""", "_").Replace("'", "_").Replace("\", "_").Replace("/", "_").StringReplace("\s", "_").StringReplace("[_-]{2,}", "_").Trim("-"c, "_"c)
    End Function

    <Extension>
    Public Function GetMetaboliteModel(registry As biocad_registry, meta_id As UInteger) As registry_resolver
        Return registry.registry_resolver _
            .where(field("symbol_id") = meta_id,
                   field("type") = registry.biocad_vocabulary.metabolite_type) _
            .find(Of registry_resolver)
    End Function
End Module
