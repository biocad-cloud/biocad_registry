Imports BioNovoGene.BioDeep.Chemistry
Imports BioNovoGene.BioDeep.Chemistry.MetaLib.Models
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Microsoft.VisualBasic.Linq
Imports registry_data
Imports registry_data.biocad_registryModel
Imports registry_exports

Module MoNADatabase

    Private Function MakeCleanName(ByRef spectra As SpectraSection) As MetaInfo
        spectra.name = Strings.Trim(spectra.name).Trim(""""c, " "c, "'"c, "!"c, "_"c, "-"c)

        Dim clean_name As String = spectra.name

        If clean_name.IsPattern("((NCGC)|(MLS))\d+[-]\d+[!].+") Then
            clean_name = clean_name.GetTagValue("!").Value.ToLower
        End If

        If clean_name.StartsWith("(((Cl)|[CHONPS])\d*){3,}_", RegexICMul) Then
            clean_name = clean_name.GetTagValue("_").Value
        End If

        If clean_name <> "" Then
            spectra.name = clean_name
        End If

        Return DirectCast(spectra, MetaInfo)
    End Function

    Public Function FindBySpectraID(registry As biocad_registry, db_key As UInteger, spectra_id As String) As metabolites
        Dim metabolite_type As UInteger = registry.biocad_vocabulary.metabolite_type
        Dim sql As String = $"
SELECT 
    *
FROM
    cad_registry.metabolites
WHERE
    id = (SELECT 
            obj_id
        FROM
            db_xrefs
        WHERE
            type = {metabolite_type} AND db_name = {db_key}
                AND db_xref = '{spectra_id}'
                AND db_source = {db_key}
        LIMIT 1) LIMIT 1;
"
        Dim m As metabolites = registry.metabolites.getDriver.ExecuteScalar(Of biocad_registryModel.metabolites)(sql)
        Return m
    End Function

    Public Sub MakeImports(registry As biocad_registry, chunk As SpectraSection())
        Dim db_mona As UInteger = registry.biocad_vocabulary.GetDatabaseResource("MoNA").id
        Dim metabolite_type As UInteger = registry.biocad_vocabulary.metabolite_type

        For Each spectra As SpectraSection In TqdmWrapper.Wrap(chunk)
            Dim model As MetaInfo = MakeCleanName(spectra)
            ' check mona id reference
            Dim m As metabolites = FindBySpectraID(registry, db_mona, model.ID)

            If Not m Is Nothing Then
                ' skip of the existed metadata
            Else
                m = registry.FindMolecule(model, "kegg_id", nameSearch:=True)
            End If

            If m Is Nothing Then
                Continue For
            End If

            Call registry.SaveStructureData(m, model.xref.SMILES)
            Call registry.SaveSynonyms(m, model.synonym.JoinIterates({model.name, model.IUPACName}).Distinct, db_mona)
            Call registry.SaveDbLinks(model, m, db_mona, saveID:=True)
        Next
    End Sub

    Public Sub MakeImports(registry As biocad_registry, chunk As spectraverse())
        Dim db_spectraverse As UInteger = registry.biocad_vocabulary.GetDatabaseResource("spectraverse").id
        Dim metabolite_type As UInteger = registry.biocad_vocabulary.metabolite_type

        For Each meta As spectraverse In chunk
            Dim model As MetaInfo = meta.CreateMeta
            ' check mona id reference
            Dim m As metabolites = FindBySpectraID(registry, db_spectraverse, model.ID)

            If m Is Nothing Then
                m = registry.FindMolecule(model, "kegg_id", nameSearch:=True)
            End If

            If m Is Nothing Then
                Continue For
            End If

            Call registry.SaveDbLinks(model, m, db_spectraverse, saveID:=True)
            Call registry.SaveStructureData(m, model.xref.SMILES)
            Call registry.SaveSynonyms(m, model.synonym.JoinIterates({model.name, model.IUPACName}).Distinct, db_spectraverse)
        Next
    End Sub
End Module
