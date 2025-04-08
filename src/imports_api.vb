Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports SMRUCC.genomics.Assembly.NCBI.GenBank
Imports SMRUCC.genomics.Assembly.NCBI.Taxonomy

<Package("data_imports")>
Module imports_api

    <ExportAPI("imports_genbank")>
    Public Function imports_genbank(registry As biocad_registry, genbank As GBFF.File) As Object
        Call GenBankImports.ImportsData(registry, genbank)
        Return Nothing
    End Function

    <ExportAPI("imports_taxonomy")>
    Public Function imports_taxonomy(registry As biocad_registry, taxdump As NcbiTaxonomyTree) As Object
        Dim bar As ProgressBar = Nothing

        For Each tax As TaxonomyNode In TqdmWrapper.Wrap(taxdump.Taxonomy.Values, bar:=bar)
            Dim tree = registry.taxonomy_tree.batch_insert

            Call registry.ncbi_taxonomy.add(
                field("id") = tax.taxid,
                field("taxname") = tax.name,
                field("nsize") = tax.nchilds,
                field("parent_id") = CUInt(Val(tax.parent)),
                field("rank") = registry.getVocabulary(If(tax.rank.StringEmpty(, True), "norank", tax.rank), "Taxonomy Rank")
            )

            For Each id As Integer In tax.children.SafeQuery
                Call tree.add(
                    field("tax_id") = tax.taxid,
                    field("child_tax") = id
                )
            Next

            Call tree.commit()
            Call bar.SetLabel(tax.name)
        Next

        Return Nothing
    End Function

End Module
