Imports biocad_storage
Imports BioNovoGene.BioDeep.Chemistry.ChEBI
Imports BioNovoGene.BioDeep.Chemistry.LOTUS
Imports BioNovoGene.BioDeep.Chemistry.MetaLib.Models
Imports BioNovoGene.BioDeep.Chemistry.NCBI.PubChem
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports SMRUCC.genomics.Assembly.NCBI.GenBank
Imports SMRUCC.genomics.Assembly.NCBI.Taxonomy
Imports SMRUCC.genomics.Assembly.Uniprot.XML
Imports SMRUCC.genomics.foundation.OBO_Foundry.IO.Models
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Internal.[Object]
Imports SMRUCC.Rsharp.Runtime.Interop

<Package("data_imports")>
Module imports_api

    <ExportAPI("imports_genbank")>
    Public Function imports_genbank(registry As biocad_registry, genbank As GBFF.File) As Object
        Call New GenBankImports(registry, genbank).ImportsData()
        Return Nothing
    End Function

    <ExportAPI("genbank_repo")>
    Public Function genbank_repo(dir As String) As GenBankScanner
        Return New GenBankScanner(dir)
    End Function

    <ExportAPI("pubchem_repo")>
    Public Function pubchem_repo(dir As String) As PubChemScanner
        Return New PubChemScanner(dir)
    End Function

    <ExportAPI("imports_lotus_np")>
    Public Function imports_lotus(registry As biocad_registry, <RRawVectorArgument> lotus As Object, Optional env As Environment = Nothing) As Object
        Dim np As pipeline = pipeline.TryCreatePipeline(Of NaturalProduct)(lotus, env)

        If np.isError Then
            Return np.getError
        End If

        Call New LotusNPImports(registry).ImportsNP(np.populates(Of NaturalProduct)(env))

        Return Nothing
    End Function

    <ExportAPI("imports_pubchem_repo")>
    Public Sub imports_pubchem_repo(registry As biocad_registry, repo As PubChemScanner)
        For Each page As PugViewRecord() In repo.LoadPageData
            Call PubChemImports.RunDataCommit(registry, page)
        Next
    End Sub

    <ExportAPI("imports_chebi_repo")>
    Public Sub imports_chebi(registry As biocad_registry, chebi As OBOFile)
        For Each page As MetaInfo() In ChEBIObo.ImportsMetabolites(chebi).SplitIterator(3000)
            Call MetaboliteImports.RunDataCommit(registry, page, uniref:=Function(m) m.ID)
        Next
    End Sub

    <ExportAPI("imports_refmet")>
    Public Sub imports_refmet(registry As biocad_registry, file As String)
        Call New RefMetImports(file, registry).Imports()
    End Sub

    ''' <summary>
    ''' general function for make metabolite imports
    ''' </summary>
    ''' <param name="registry"></param>
    ''' <param name="metab"></param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("imports_metab_repo")>
    Public Function imports_metabolites(registry As biocad_registry, <RRawVectorArgument> metab As Object,
                                        Optional lazy_molecule_ctor As Boolean = False,
                                        Optional topic As String = Nothing,
                                        Optional exclude_topic As String = Nothing,
                                        Optional env As Environment = Nothing)

        Dim pull As pipeline = pipeline.TryCreatePipeline(Of MetaInfo)(metab, env)

        If pull.isError Then
            Return pull.getError
        End If

        For Each page As MetaInfo() In pull.populates(Of MetaInfo)(env).SplitIterator(3000)
            Call MetaboliteImports.RunDataCommit(registry, page, uniref:=Function(m) m.ID, lazy_molecule_ctor)

            If Not topic.StringEmpty(, True) Then
                Call MetaboliteCommit.CommitTags(registry, page, topic)
            End If
            If Not exclude_topic.StringEmpty(, True) Then
                Call MetaboliteCommit.RemoveTags(registry, page, exclude_topic)
            End If
        Next

        Return Nothing
    End Function

    ''' <summary>
    ''' make imports of the genomics sequence data into database
    ''' </summary>
    ''' <param name="registry"></param>
    ''' <param name="genbank"></param>
    ''' <returns></returns>
    <ExportAPI("imports_genomics")>
    Public Function imports_genomics(registry As biocad_registry, genbank As GenBankScanner) As Object
        For Each page As GBFF.File() In genbank.LoadPageData
            Call registry.importsGenomics(page)
        Next

        Return Nothing
    End Function

    <ExportAPI("imports_genes")>
    Public Function imports_genes(registry As biocad_registry, genbank As GenBankScanner) As Object
        For Each page As GBFF.File() In genbank.LoadPageData
            Call registry.importsGenes(page)
        Next

        Return Nothing
    End Function

    <ExportAPI("imports_genbank_proteins")>
    Public Function imports_genbank_proteins(registry As biocad_registry, genbank As GenBankScanner) As Object
        For Each page As GBFF.File() In genbank.LoadPageData
            Call registry.importsProteins(page)
        Next

        Return Nothing
    End Function

    <ExportAPI("imports_dbxrefs")>
    Public Function imports_dbxrefs(registry As biocad_registry, genbank As GenBankScanner) As Object
        For Each page As GBFF.File() In genbank.LoadPageData
            Call registry.importsFeatureXrefs(page)
        Next

        Return Nothing
    End Function

    <ExportAPI("imports_taxonomy")>
    Public Function imports_taxonomy(registry As biocad_registry, taxdump As NcbiTaxonomyTree) As Object
        Dim bar As ProgressBar = Nothing
        Dim blocks = taxdump.Taxonomy.Values _
            .SplitIterator(1000) _
            .ToArray

        For Each taxlist As TaxonomyNode() In TqdmWrapper.Wrap(blocks, bar:=bar)
            Dim tree = registry.taxonomy_tree.batch_insert(max_blocksize:=2048)
            Dim transaction = registry.ncbi_taxonomy.open_transaction

            For Each tax As TaxonomyNode In taxlist
                Call transaction.add(
                    field("id") = tax.taxid,
                    field("taxname") = tax.name,
                    field("nsize") = tax.nchilds,
                    field("parent_id") = CUInt(Val(tax.parent)),
                    field("rank") = registry.getVocabulary(If(tax.rank.StringEmpty(, True), "no rank", tax.rank), "Taxonomy Rank")
                )

                For Each id As Integer In tax.children.SafeQuery
                    Call tree.add(
                        field("tax_id") = tax.taxid,
                        field("child_tax") = id
                    )
                Next
            Next

            Call tree.commit(transaction)
            Call transaction.commit()

            Call bar.SetLabel(taxlist(0).name)
        Next

        Return Nothing
    End Function

    <ExportAPI("imports_uniprot")>
    Public Function imports_uniprot(registry As biocad_registry, uniprot As String)
        Dim source As New UniProtPageLoader(uniprot)

        For Each page As entry() In source.LoadPageData
            Call New UniProtImporter(registry).importsData(page)
        Next

        Return Nothing
    End Function

End Module
