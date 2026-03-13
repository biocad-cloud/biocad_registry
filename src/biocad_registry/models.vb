
Imports BioNovoGene.BioDeep.Chemistry.NCBI.PubChem.ExtensionModels
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports Microsoft.VisualBasic.Serialization.JSON
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports registry_data
Imports registry_data.biocad_registryModel
Imports SMRUCC.genomics.ComponentModel.Annotation
Imports SMRUCC.Rsharp.Runtime
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
        Dim term_location As UInteger = registry.biocad_vocabulary.GetRegistryEntity(biocad_vocabulary.EntitySubcellularLocation).id

        For i As Integer = 0 To locations.nrows - 1
            Dim loc As compartment_location = registry.compartment_location.where(field("name") = name(i)).find(Of compartment_location)

            If Not loc Is Nothing Then
                Dim term As String = loc.name.Replace(" ", "_")

                Call updates.add(registry.compartment_location _
                            .where(field("id") = loc.id) _
                            .save_sql(field("zh_name") = zh_names(i),
                                      field("membrane") = membrane(i)))

                If registry.registry_resolver _
                    .where(field("symbol_id") = loc.id,
                           field("type") = term_location,
                           field("register_name") = term) _
                    .find(Of registry_resolver) Is Nothing Then

                    Call updates.add(registry.registry_resolver.add_sql(
                        field("symbol_id") = loc.id,
                        field("type") = term_location,
                        field("register_name") = term
                    ))
                End If
            End If
        Next

        Call updates.commit()
    End Sub

    <ExportAPI("update_metabolic_network")>
    Public Sub update_metabolic_network(registry As biocad_registry)
        Call registry.UpdateMetabolicNetwork
    End Sub

    <ExportAPI("register_metabolic_symbols")>
    Public Sub register_metabolic_symbols(registry As biocad_registry)
        Call registry.RegisterMetabolicSymbols
    End Sub

    <ExportAPI("resolve_metabolite_duplicates")>
    Public Sub update_metabolite_models(registry As biocad_registry)
        Call registry.MetaboliteLinks
    End Sub

    <ExportAPI("build_plantnp_library")>
    Public Sub build_plantnp(registry As biocad_registry)
        Call TopicViews.PlantNP(registry)
    End Sub

    <ExportAPI("build_microbial_nps")>
    Public Sub build_fermertation_np(registry As biocad_registry)
        Call TopicViews.MicrobialNP(registry)
    End Sub

    <ExportAPI("imports_pathways")>
    Public Function imports_pathways(registry As biocad_registry, <RRawVectorArgument> pathways As Object, Optional env As Environment = Nothing) As Object
        Dim pull As pipeline = pipeline.TryCreatePipeline(Of PathwayGraph)(pathways, env)

        If pull.isError Then
            Return pull.getError
        End If

        Dim vocabulary As biocad_vocabulary = registry.biocad_vocabulary
        Dim metab_class = vocabulary.metabolite_type
        Dim enzyme_class = vocabulary.db_ECNumber
        Dim prot_class = vocabulary.protein_data
        Dim db_uniprot = vocabulary.db_uniprot

        For Each pwy As PathwayGraph In TqdmWrapper.Wrap(pull.populates(Of PathwayGraph)(env).ToArray)
            Dim source_db As UInteger = vocabulary.GetDatabaseResource(pwy.source).id
            Dim check = registry.pathway _
                .where(field("db_source") = source_db,
                       field("accession_id") = pwy.pwacc) _
                .find(Of biocad_registryModel.pathway)
            Dim links As CommitTransaction = registry.pathway_network.ignore.open_transaction

            If check Is Nothing Then
                Call registry.pathway.add(
                    field("db_source") = source_db, field("accession_id") = pwy.pwacc,
                    field("name") = If(pwy.name, pwy.pwacc),
                    field("type") = pwy.pwtype,
                    field("taxid") = CInt(Val(pwy.taxid)),
                    field("dois") = pwy.dois.SafeQuery.Where(Function(did) Not did.StringEmpty(, True)).GetJson,
                    field("note") = pwy.citations.SafeQuery.Where(Function(ct) Not ct.StringEmpty(, True)).JoinBy("; ")
                )
                check = registry.pathway _
                    .where(field("db_source") = source_db,
                           field("accession_id") = pwy.pwacc) _
                    .order_by("id", desc:=True) _
                    .find(Of biocad_registryModel.pathway)
            End If

            If check Is Nothing Then
                Throw New InvalidOperationException("create new pathway model error: " & registry.pathway.GetLastErrorMessage)
            End If

            For Each cid As String In pwy.cids.SafeQuery
                If cid.StringEmpty(, True) Then
                    Continue For
                End If

                Dim check_link = registry.pathway_network _
                    .where(field("pathway_id") = check.id,
                           field("symbol_id") = cid,
                           field("class_id") = metab_class) _
                    .find(Of pathway_network)

                If check_link Is Nothing Then
                    Dim metab = registry.metabolites.where(field("pubchem_cid") = cid).find(Of metabolites)

                    Call links.add(
                        field("pathway_id") = check.id,
                        field("symbol_id") = cid,
                        field("class_id") = metab_class,
                        field("note") = pwy.name & $" [{If(metab Is Nothing, $"CID:{cid}", metab.name)}]",
                        field("model_id") = If(metab Is Nothing, 0, metab.id)
                    )
                End If
            Next

            For Each ec_id As String In pwy.ecs.SafeQuery
                If ec_id.StringEmpty(, True) Then
                    Continue For
                End If

                Dim check_link = registry.pathway_network _
                    .where(field("pathway_id") = check.id,
                           field("symbol_id") = ec_id,
                           field("class_id") = enzyme_class) _
                    .find(Of pathway_network)

                If check_link Is Nothing Then
                    Dim ecnum As ECNumber = ECNumber.ValueParser(ec_id)
                    Dim enz = registry.enzyme _
                        .where(field("enzyme_class") = CInt(ecnum.type),
                               field("sub_class") = ecnum.subType,
                               field("sub_category") = ecnum.subCategory,
                               field("enzyme_number") = ecnum.serialNumber) _
                        .find(Of biocad_registryModel.enzyme)

                    Call links.add(
                        field("pathway_id") = check.id,
                        field("symbol_id") = ec_id,
                        field("class_id") = enzyme_class,
                        field("note") = pwy.name & $" [{ec_id}]",
                        field("model_id") = If(enz Is Nothing, 0, enz.id)
                    )
                End If
            Next

            For Each prot_id As String In pwy.protacxns.SafeQuery
                If prot_id.StringEmpty(, True) Then
                    Continue For
                End If

                Dim check_link = registry.pathway_network _
                    .where(field("pathway_id") = check.id,
                           field("symbol_id") = prot_id,
                           field("class_id") = prot_class) _
                    .find(Of pathway_network)

                If check_link Is Nothing Then
                    Dim uniprot = registry.db_xrefs _
                        .where(field("type") = prot_class,
                               field("db_name") = db_uniprot,
                               field("db_xref") = prot_id) _
                        .find(Of biocad_registryModel.db_xrefs)

                    Call links.add(
                        field("pathway_id") = check.id,
                        field("symbol_id") = prot_id,
                        field("class_id") = prot_class,
                        field("note") = pwy.name & $" [{prot_id}]",
                        field("model_id") = If(uniprot Is Nothing, 0, uniprot.obj_id)
                    )
                End If
            Next

            Call links.commit()
        Next

        Return Nothing
    End Function
End Module
