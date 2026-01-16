Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports registry_data
Imports registry_data.biocad_registryModel
Imports registry_data.Exports
Imports registry_exports
Imports SMRUCC.Rsharp.Runtime.Internal.[Object]

<Package("exports")>
Module exports

    <ExportAPI("metabolite_table")>
    Public Function export_metabolite_table(registry As biocad_registry, Optional dbname As String = Nothing) As Object
        Dim table As metabolites()

        If dbname.StringEmpty Then
            table = registry.metabolites.where(field("exact_mass") > 0).select(Of metabolites)
        Else
            table = registry.metabolites.where(Not field(dbname).is_nothing, field(dbname) <> "").select(Of metabolites)
        End If

        Dim df As New dataframe With {
            .rownames = table _
                .Select(Function(a) "BioCAD" & a.id.ToString.PadLeft(11, "0"c)) _
                .ToArray,
            .columns = New Dictionary(Of String, Array)
        }

        Call df.add("id", df.rownames)
        Call df.add("name", From m As metabolites In table Select Strings.Trim(m.name).Replace("\", ""))
        Call df.add("formula", From m As metabolites In table Select m.formula)
        Call df.add("exact_mass", From m As metabolites In table Select m.exact_mass)
        Call df.add("cas_id", From m As metabolites In table Select m.cas_id.default)
        Call df.add("pubchem_id", From m As metabolites In table Select m.pubchem_cid.default(Nothing))
        Call df.add("chebi_id", From m As metabolites In table Select m.chebi_id.default("ChEBI:"))
        Call df.add("hmdb_id", From m As metabolites In table Select m.hmdb_id.default)
        Call df.add("lipidmaps_id", From m As metabolites In table Select m.lipidmaps_id.default)
        Call df.add("kegg_id", From m As metabolites In table Select m.kegg_id.default)
        Call df.add("drugbank_id", From m As metabolites In table Select m.drugbank_id.default)
        Call df.add("biocyc", From m As metabolites In table Select m.biocyc.default)
        Call df.add("mesh_id", From m As metabolites In table Select m.mesh_id.default)
        Call df.add("wikipedia", From m As metabolites In table Select m.wikipedia.default)

        Return df
    End Function

    <Extension>
    Private Function [default](id As UInteger, prefix As String) As String
        If id <= 0 Then
            Return "-"
        ElseIf prefix Is Nothing Then
            Return id.ToString
        Else
            Return prefix & id.ToString
        End If
    End Function

    <Extension>
    Private Function [default](s As String) As String
        If s.StringEmpty(, True) Then
            Return "-"
        Else
            Return s
        End If
    End Function

    <ExportAPI("export_smiles_data")>
    Public Function export_smiles_data(registry As biocad_registry, dbname As String) As Object
        Return registry.exportSMILES(dbname).ToArray
    End Function

    <ExportAPI("virtualcell_componentdb")>
    Public Function export_virtualCell_components(registry As biocad_registry, repo As String) As Object
        Dim dump As New ExportVirtualCellModels(registry, repo)

        Call dump.ExportLocations()
        Call dump.ExportEnzymeDb()
        Call dump.ExportSubcellularLocationDb()
        Call dump.ExportReactionPool()
        Call dump.ExportMoleculeData()

        Return Nothing
    End Function

End Module
