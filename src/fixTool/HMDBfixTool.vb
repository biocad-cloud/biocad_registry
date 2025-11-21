Imports biocad_storage
Imports BioNovoGene.BioDeep.Chemistry.TMIC.HMDB
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder

Module HMDBfixTool

    Sub RunFix()
        Dim hmdb As IEnumerable(Of metabolite) = metabolite.Load("K:\hmdb_metabolites.xml", tqdm:=True)
        Dim hmdb_term As String = registry.vocabulary_terms.hmdb_term

        For Each metabolite As metabolite In hmdb
            Dim q = registry.db_xrefs.left_join("molecule") _
                .on(field("obj_id") = field("molecule.id")) _
                .where(field("db_key") = hmdb_term,
                       field("xref") = metabolite.accession,
                       field("formula") = metabolite.chemical_formula) _
                .order_by("`molecule`.id") _
                .find(Of biocad_registryModel.molecule)("`molecule`.*")

            If q Is Nothing Then
                ' add new entry
            End If



        Next
    End Sub
End Module
