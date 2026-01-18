Imports System.Runtime.CompilerServices
Imports registry_data.biocad_registryModel
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder

Public Module RegisterSymbol

    <Extension>
    Public Function makeSymbol(name As String) As String
        Return Strings.Trim(name) _
            .Replace("(", "_") _
            .Replace(")", "_") _
            .Replace("""", "_") _
            .Replace("'", "_") _
            .Replace("\", "_") _
            .Replace("/", "_") _
            .StringReplace("\s", "_") _
            .StringReplace("[_-]{2,}", "_") _
            .Trim("-"c, "_"c)
    End Function

    ''' <summary>
    ''' get an existed register symbol for the target metabolite
    ''' </summary>
    ''' <param name="registry"></param>
    ''' <param name="meta_id"></param>
    ''' <returns>
    ''' this function will returns nothing if the symbol is not found
    ''' </returns>
    <Extension>
    Public Function GetMetaboliteModel(registry As biocad_registry, meta_id As UInteger) As registry_resolver
        Return registry.registry_resolver _
            .where(field("symbol_id") = meta_id,
                   field("type") = registry.biocad_vocabulary.metabolite_type) _
            .find(Of registry_resolver)
    End Function

    <Extension>
    Public Function MetaboliteScore(m As metabolites) As Double
        Dim score As Double = 0

        If m.exact_mass > 0 Then score += 1
        If m.pubchem_cid > 0 Then score += 1
        If m.chebi_id > 0 Then score += 1

        If Not m.kegg_id.StringEmpty Then score += 1
        If Not m.hmdb_id.StringEmpty Then score += 1
        If Not m.biocyc.StringEmpty Then score += 1
        If Not m.cas_id.StringEmpty Then score += 1
        If Not m.drugbank_id.StringEmpty Then score += 1
        If Not m.lipidmaps_id.StringEmpty Then score += 1
        If Not m.wikipedia.StringEmpty Then score += 1
        If Not m.mesh_id.StringEmpty Then score += 1

        Return score / m.id
    End Function

    ''' <summary>
    ''' make register of the metabolite symbol inside the biocad registry and then returns the new symbol
    ''' </summary>
    ''' <param name="registry"></param>
    ''' <param name="meta"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' this function will try to find the existsed symbol with the same name, if not found then will try to make a new one
    ''' </remarks>
    <Extension>
    Public Function SymbolRegister(registry As biocad_registry, meta As metabolites) As registry_resolver
        Dim metabolite_type As UInteger = registry.biocad_vocabulary.metabolite_type
        Dim symbol_name As String = meta.name.makeSymbol
        Dim check As registry_resolver = registry.registry_resolver _
            .where(field("register_name") = symbol_name,
                   field("type") = metabolite_type) _
            .find(Of registry_resolver)

        If check IsNot Nothing Then
            ' symbol is already existsed
            If check.symbol_id <> meta.id Then
                registry.registry_resolver.where(field("id") = check.id).save(field("symbol_id") = meta.id)
                Return GetMetaboliteModel(registry, meta.id)
            End If

            Return check
        End If

        ' make a new symbol register inside the registry system
        Call registry.registry_resolver.add(
            field("register_name") = symbol_name,
            field("type") = metabolite_type,
            field("symbol_id") = meta.id
        )

        Return GetMetaboliteModel(registry, meta.id)
    End Function
End Module
