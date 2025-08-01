Imports System.Runtime.CompilerServices
Imports biocad_storage
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports SMRUCC.genomics.Metagenomics
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Internal.[Object]
Imports SMRUCC.Rsharp.Runtime.Interop

''' <summary>
''' query of the biocad_registry
''' </summary>
''' 
<Package("registry")>
Module registry

    Sub New()
        Call Converts.makeDataframe.addHandler(GetType(taxonomyInfo()), AddressOf taxinfoTable)
    End Sub

    <RGenericOverloads("as.data.frame")>
    Private Function taxinfoTable(list As taxonomyInfo(), args As list, env As Environment) As dataframe
        Dim table As New dataframe With {.columns = New Dictionary(Of String, Array)}

        Call table.add("ncbi_taxid", From tax As taxonomyInfo In list Select tax.ncbi_taxid)
        Call table.add("taxname", From tax As taxonomyInfo In list Select tax.taxname)
        Call table.add("rank", From tax As taxonomyInfo In list Select tax.rank)
        Call table.add("parent_id", From tax As taxonomyInfo In list Select tax.parent_id)
        Call table.add("description", From tax As taxonomyInfo In list Select tax.description)

        Return table
    End Function

    ''' <summary>
    ''' get taxonomy node information via the taxonomy name or the taxonomy id
    ''' </summary>
    ''' <param name="registry"></param>
    ''' <param name="tax">the taxonomy name or the ncbi taxonomy id</param>
    ''' <returns></returns>
    <ExportAPI("get_taxinfo")>
    <Extension>
    Public Function get_taxinfo(registry As biocad_registry, tax As String) As taxonomyInfo
        Dim q As FieldAssert

        If tax.IsPattern("\d+") Then
            ' is tax id
            q = field("`ncbi_taxonomy`.id") = tax
        Else
            ' is tax name
            q = field("`ncbi_taxonomy`.taxname") = tax
        End If

        Return registry.ncbi_taxonomy _
            .left_join("vocabulary") _
            .on(field("`vocabulary`.id") = field("`ncbi_taxonomy`.`rank`")) _
            .where(q) _
            .find(Of taxonomyInfo)("ncbi_taxonomy.id AS ncbi_taxid",
    "taxname",
    "term AS `rank`",
    "parent_id",
    "description")
    End Function

    <ExportAPI("find_taxinfo")>
    Public Function find_taxinfo(registry As biocad_registry, tax As String) As taxonomyInfo()
        Return registry.ncbi_taxonomy _
            .left_join("vocabulary") _
            .on(field("`vocabulary`.id") = field("`ncbi_taxonomy`.`rank`")) _
            .where(match("taxname", "description").against(tax, booleanMode:=True)) _
            .select(Of taxonomyInfo)("ncbi_taxonomy.id AS ncbi_taxid",
    "taxname",
    "term AS `rank`",
    "parent_id",
    "description")
    End Function

    <ExportAPI("taxonomy_lineage")>
    Public Function taxonomy_lineage(registry As biocad_registry, tax_id As String) As Taxonomy
        Dim taxnode As taxonomyInfo = registry.get_taxinfo(tax:=tax_id)
        Dim lineage As New Dictionary(Of String, String) From {
            {taxnode.rank, taxnode.taxname}
        }
        Dim list As New List(Of taxonomyInfo) From {taxnode}

        Do While True
            Dim parent = registry.taxonomy_tree _
                .where(field("child_tax") = taxnode.ncbi_taxid) _
                .find(Of biocad_registryModel.taxonomy_tree)

            If parent Is Nothing Then
                Exit Do
            End If

            taxnode = registry.get_taxinfo(tax:=parent.tax_id)
            list.Add(taxnode)
            lineage(taxnode.rank) = taxnode.taxname
        Loop

        Return New Taxonomy(lineage) With {
            .ncbi_taxid = tax_id
        }
    End Function

    ''' <summary>
    ''' get child taxonomy id list
    ''' </summary>
    ''' <param name="registry"></param>
    ''' <param name="tax_id"></param>
    ''' <returns></returns>
    <ExportAPI("child_list")>
    Public Function child_list(registry As biocad_registry,
                               tax_id As String,
                               Optional direct_list As Boolean = True) As Object

        Dim list As List(Of biocad_registryModel.taxonomy_tree) = registry.taxonomy_tree _
            .where(field("tax_id") = tax_id) _
            .select(Of biocad_registryModel.taxonomy_tree) _
            .AsList

        If Not direct_list Then
            Dim childs As biocad_registryModel.taxonomy_tree() = list.ToArray

            Do While True
                childs = registry.taxonomy_tree _
                    .where(field("tax_id").in(childs.Select(Function(t) t.child_tax).Distinct)) _
                    .select(Of biocad_registryModel.taxonomy_tree)

                Call list.AddRange(childs)

                If childs.IsNullOrEmpty Then
                    Exit Do
                End If
            Loop
        End If

        Return list.Select(Function(t) t.child_tax).Distinct.ToArray
    End Function

    ''' <summary>
    ''' get molecule information by its reference id
    ''' </summary>
    ''' <param name="registry"></param>
    ''' <param name="id">default is the biocad id, this parameter could also be other xref in external database if the <paramref name="dbname"/> has been specific</param>
    ''' <param name="dbname"></param>
    ''' <returns></returns>
    <ExportAPI("get_by_xref")>
    Public Function get_by_xref(registry As biocad_registry, id As String, Optional dbname As String = Nothing) As Object
        If Not dbname Is Nothing Then
            Dim dbkey = registry.getVocabulary(dbname, category:="External Database", [readonly]:=True)
            Dim db_xrefs = registry.db_xrefs _
                .where(field("db_key") = dbkey,
                       field("xref") = id) _
                .select(Of biocad_registryModel.db_xrefs)
            Dim uniq As UInteger() = db_xrefs _
                .Select(Function(r) r.obj_id) _
                .Distinct _
                .ToArray

            If uniq.IsNullOrEmpty Then
                Return Nothing
            ElseIf uniq.Length = 1 Then
                Return New ExportMetabolites(registry).GetByBioCADId(db_xrefs(0).obj_id)
            Else
                Dim result As list = list.empty
                Dim export As New ExportMetabolites(registry)
                Dim mol As BioNovoGene.BioDeep.Chemistry.MetaLib.Models.MetaInfo

                For Each uid As UInteger In uniq
                    mol = export.GetByBioCADId(uid)
                    result.slots(mol.ID) = mol
                Next

                Return result
            End If
        Else
            Return New ExportMetabolites(registry).GetByBioCADId(UInteger.Parse(id.Match("\d+")))
        End If
    End Function
End Module

