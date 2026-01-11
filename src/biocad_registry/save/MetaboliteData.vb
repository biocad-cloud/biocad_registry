Imports System.Runtime.CompilerServices
Imports BioNovoGene.BioDeep.Chemistry.MetaLib.Models
Imports BioNovoGene.BioDeep.Chemoinformatics.Formula
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Serialization.JSON
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes
Imports registry_data
Imports registry_data.biocad_registryModel

Module MetaboliteData

    <Extension>
    Public Sub SaveDbLinks(registry As biocad_registry,
                           vocabulary As biocad_vocabulary,
                           meta As MetaInfo,
                           m As metabolites,
                           db_source As UInteger)

        Dim metabolite_type As UInteger = vocabulary.metabolite_type
        Dim updates As New List(Of FieldAssert)
        Dim pubchem_cid As String = Strings.Trim(meta.xref.pubchem).int_id
        Dim chebi_id As String = Strings.Trim(meta.xref.chebi).int_id
        ' transaction of registry.db_xrefs
        Dim trans As CommitTransaction = registry.db_xrefs.open_transaction.ignore

        If m.pubchem_cid = 0 AndAlso Not pubchem_cid Is Nothing Then
            updates.Add(field("pubchem_cid") = pubchem_cid)
        End If
        If m.chebi_id = 0 AndAlso Not chebi_id Is Nothing Then
            updates.Add(field("chebi_id") = chebi_id)
        End If
        If m.hmdb_id.StringEmpty AndAlso Not meta.xref.HMDB.StringEmpty Then
            updates.Add(field("hmdb_id") = meta.xref.HMDB)
        End If
        If m.kegg_id.StringEmpty AndAlso Not meta.xref.KEGG.StringEmpty Then
            updates.Add(field("kegg_id") = meta.xref.KEGG)
        End If
        If m.lipidmaps_id.StringEmpty AndAlso Not meta.xref.lipidmaps.StringEmpty Then
            updates.Add(field("lipidmaps_id") = meta.xref.lipidmaps)
        End If
        If m.mesh_id.StringEmpty AndAlso Not meta.xref.MeSH.StringEmpty Then
            updates.Add(field("mesh_id") = meta.xref.MeSH)
        End If
        If m.wikipedia.StringEmpty AndAlso Not meta.xref.Wikipedia.StringEmpty Then
            updates.Add(field("wikipedia") = meta.xref.Wikipedia)
        End If
        If m.cas_id.StringEmpty AndAlso Not meta.xref.CAS.DefaultFirst.StringEmpty Then
            updates.Add(field("cas_id") = meta.xref.CAS.DefaultFirst)
        End If
        If m.biocyc.StringEmpty AndAlso Not meta.xref.MetaCyc.StringEmpty Then
            updates.Add(field("biocyc") = meta.xref.MetaCyc)
        End If
        If m.drugbank_id.StringEmpty AndAlso Not meta.xref.DrugBank.StringEmpty Then
            updates.Add(field("drugbank_id") = meta.xref.DrugBank)
        End If

        If updates.Any Then
            Call trans.add(registry.metabolites.where(field("id") = m.id).save_sql(updates.ToArray))
        End If

        If Not pubchem_cid.StringEmpty Then
            trans.ignore.add(field("db_source") = db_source, field("db_name") = vocabulary.db_pubchem, field("db_xref") = pubchem_cid, field("type") = metabolite_type, field("obj_id") = m.id)
        End If
        If Not chebi_id.StringEmpty Then
            chebi_id = $"ChEBI:{chebi_id}"
            trans.ignore.add(field("db_source") = db_source, field("db_name") = vocabulary.db_chebi, field("db_xref") = chebi_id, field("type") = metabolite_type, field("obj_id") = m.id)
        End If
        If Not meta.xref.HMDB.StringEmpty Then
            trans.ignore.add(field("db_source") = db_source, field("db_name") = vocabulary.db_hmdb, field("db_xref") = meta.xref.HMDB, field("type") = metabolite_type, field("obj_id") = m.id)
        End If
        If Not meta.xref.lipidmaps.StringEmpty Then
            trans.ignore.add(field("db_source") = db_source, field("db_name") = vocabulary.db_lipidmaps, field("db_xref") = meta.xref.lipidmaps, field("type") = metabolite_type, field("obj_id") = m.id)
        End If
        If Not meta.xref.KEGG.StringEmpty Then
            trans.ignore.add(field("db_source") = db_source, field("db_name") = vocabulary.db_kegg, field("db_xref") = meta.xref.KEGG, field("type") = metabolite_type, field("obj_id") = m.id)
        End If
        If Not meta.xref.MeSH.StringEmpty Then
            trans.ignore.add(field("db_source") = db_source, field("db_name") = vocabulary.db_mesh, field("db_xref") = meta.xref.MeSH, field("type") = metabolite_type, field("obj_id") = m.id)
        End If
        If Not meta.xref.Wikipedia.StringEmpty Then
            trans.ignore.add(field("db_source") = db_source, field("db_name") = vocabulary.db_wikipedia, field("db_xref") = meta.xref.Wikipedia, field("type") = metabolite_type, field("obj_id") = m.id)
        End If
        If Not meta.xref.MetaCyc.StringEmpty Then
            trans.ignore.add(field("db_source") = db_source, field("db_name") = vocabulary.db_biocyc, field("db_xref") = meta.xref.MetaCyc, field("type") = metabolite_type, field("obj_id") = m.id)
        End If
        If Not meta.xref.DrugBank.StringEmpty Then
            trans.ignore.add(field("db_source") = db_source, field("db_name") = vocabulary.db_drugbank, field("db_xref") = meta.xref.DrugBank, field("type") = metabolite_type, field("obj_id") = m.id)
        End If

        For Each id As String In meta.xref.CAS.SafeQuery
            If id.StringEmpty Then
                Continue For
            End If

            Call trans _
                .ignore _
                .add(field("db_source") = db_source,
                     field("db_name") = vocabulary.db_cas,
                     field("db_xref") = id,
                     field("type") = metabolite_type,
                     field("obj_id") = m.id)
        Next

        Call trans.commit()
    End Sub

    <Extension>
    Private Function int_id(id As String) As String
        If Not id.StringEmpty(, True) Then
            id = id.Match("\d+")
        End If
        If id = "" Then
            Return Nothing
        Else
            Return id
        End If
    End Function

    <Extension>
    Public Function FindMolecule(registry As biocad_registry, meta As MetaInfo, primaryKey As String, Optional nameSearch As Boolean = False) As metabolites
        Dim pubchem_cid As String = Strings.Trim(meta.xref.pubchem).int_id
        Dim chebi_id As String = Strings.Trim(meta.xref.chebi).int_id
        Dim name As String = Strings.Trim(meta.name)
        Dim hashcode As String = name.ToLower.MD5
        Dim exact_mass As Double = FormulaScanner.EvaluateExactMass(meta.formula)
        Dim m As metabolites = registry.metabolites _
            .where(field(primaryKey) = meta.ID) _
            .find(Of metabolites)

        If exact_mass < 0 Then
            exact_mass = 0
        End If

        If exact_mass > 1 Then
            If m Is Nothing AndAlso Not meta.xref.KEGG.StringEmpty Then
                m = registry.metabolites.where(field("kegg_id") = meta.xref.KEGG, field("exact_mass").between(exact_mass - 1, exact_mass + 1)).find(Of metabolites)
            End If
            If m Is Nothing AndAlso Not meta.xref.HMDB.StringEmpty Then
                m = registry.metabolites.where(field("hmdb_id") = meta.xref.HMDB, field("exact_mass").between(exact_mass - 1, exact_mass + 1)).find(Of metabolites)
            End If
            If m Is Nothing AndAlso Not meta.xref.lipidmaps.StringEmpty Then
                m = registry.metabolites.where(field("lipidmaps_id") = meta.xref.lipidmaps, field("exact_mass").between(exact_mass - 1, exact_mass + 1)).find(Of metabolites)
            End If

            If m Is Nothing AndAlso nameSearch Then
                m = registry.FindByName({meta.name, meta.IUPACName}.JoinIterates(meta.synonym), exact_mass)
            End If
        End If

        If m Is Nothing Then
            Dim hashset As String() = meta.synonym _
                .JoinIterates({meta.name, meta.IUPACName}) _
                .Where(Function(str) Not str.StringEmpty(, True)) _
                .Select(Function(str) str.ToLower.MD5) _
                .ToArray

            Call registry.metabolites.add(
                field("name") = name,
                field("hashcode") = hashcode,
                field("formula") = meta.formula,
                field("exact_mass") = exact_mass,
                field("cas_id") = meta.xref.CAS.DefaultFirst,
                field("pubchem_cid") = pubchem_cid,
                field("chebi_id") = chebi_id,
                field("hmdb_id") = meta.xref.HMDB,
                field("lipidmaps_id") = meta.xref.lipidmaps,
                field("kegg_id") = meta.xref.KEGG,
                field("biocyc") = meta.xref.MetaCyc,
                field("mesh_id") = meta.xref.MeSH,
                field("wikipedia") = meta.xref.Wikipedia,
                field("drugbank_id") = meta.xref.DrugBank,
                field("note") = meta.description
            )
            m = registry.metabolites.where(field(primaryKey) = meta.ID).order_by("id", desc:=True).find(Of metabolites)

            ' 20260106 fix for the non-primary key database
            ' exmple as refmet is missing from the master list
            ' so m is always nothing
            ' try to find by name at here
            If m Is Nothing Then
                m = registry.metabolites _
                    .where(field("exact_mass").between(exact_mass - 1, exact_mass + 1),
                           field("name") = name) _
                    .order_by("id", desc:=True) _
                    .find(Of metabolites)
            End If
        Else
            If m.note.StringEmpty(, True) Then
                registry.metabolites.where(field("id") = m.id).save(field("note") = meta.description)
            End If
            If m.name.StringEmpty(, True) Then
                registry.metabolites.where(field("id") = m.id).save(field("name") = meta.name)
            End If
        End If

        Return m
    End Function

    <Extension>
    Public Function FindByName(registry As biocad_registry, names As IEnumerable(Of String), exact_mass As Double) As metabolites
        Dim hits As New List(Of NameHit)
        Dim metabolite_type As UInteger = New biocad_vocabulary(registry).metabolite_type

        For Each name As String In names
            If name.StringEmpty(, True) Then
                Continue For
            Else
                name = name _
                    .Replace("-", " ") _
                    .Replace("+", " ") _
                    .Replace("""", " ") _
                    .Replace("'", " ") _
                    .Replace("*", " ") _
                    .Replace(">", " ") _
                    .Replace("<", " ") _
                    .Replace("~", " ") _
                    .Replace("(", " ") _
                    .Replace(")", " ") _
                    .Replace("`", " ") _
                    .Trim
            End If

            Dim fulltext As String = match("synonym").against(name, booleanMode:=True).ToString
            Dim top = registry.synonym _
                .left_join("metabolites").on((field("`metabolites`.id") = field("obj_id")) And (field("type") = metabolite_type)) _
                .where(field("exact_mass").between(exact_mass - 1, exact_mass + 1), expr(fulltext)) _
                .group_by("`metabolites`.id") _
                .order_by($"count(*) * SUM({fulltext})", desc:=True) _
                .find(Of NameHit)("metabolites.id", "COUNT(*) AS size", $"count(*) * SUM({fulltext}) as score")

            If top IsNot Nothing Then
                Call hits.Add(top)
            End If
        Next

        If hits.Any Then
            Dim top_id = hits _
                .GroupBy(Function(a) a.id) _
                .OrderByDescending(Function(a) Aggregate i In a Into Sum(i.size)) _
                .First

            Return registry.metabolites.where(field("id") = top_id.Key).find(Of metabolites)
        Else
            Return Nothing
        End If
    End Function

    Private Class NameHit

        <DatabaseField> Public Property id As UInteger
        <DatabaseField> Public Property size As Long
        <DatabaseField> Public Property score As Double

        Public Overrides Function ToString() As String
            Return Me.GetJson
        End Function

    End Class
End Module
