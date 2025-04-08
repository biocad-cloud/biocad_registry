Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData
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

    End Function

End Module
