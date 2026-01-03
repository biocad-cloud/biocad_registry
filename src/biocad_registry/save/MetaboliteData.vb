Imports System.Runtime.CompilerServices
Imports BioNovoGene.BioDeep.Chemistry.MetaLib.Models
Imports Microsoft.VisualBasic.Linq
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports registry_data
Imports registry_data.biocad_registryModel

Module MetaboliteData

    <Extension>
    Public Sub SaveDbLinks(registry As biocad_registry,
                           vocabulary As biocad_vocabulary,
                           meta As MetaLib,
                           m As metabolites,
                           db_source As UInteger,
                           pubchem_cid As String,
                           chebi_id As String)

        Dim metabolite_type As UInteger = vocabulary.metabolite_type
        Dim updates As New List(Of FieldAssert)

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

        If updates.Any Then
            Call registry.metabolites.where(field("id") = m.id).save(updates.ToArray)
        End If

        If Not pubchem_cid.StringEmpty Then
            registry.db_xrefs.ignore.add(field("db_source") = db_source, field("db_name") = vocabulary.db_pubchem, field("db_xref") = pubchem_cid, field("type") = metabolite_type, field("obj_id") = m.id)
        End If
        If Not chebi_id.StringEmpty Then
            chebi_id = $"ChEBI:{chebi_id}"
            registry.db_xrefs.ignore.add(field("db_source") = db_source, field("db_name") = vocabulary.db_chebi, field("db_xref") = chebi_id, field("type") = metabolite_type, field("obj_id") = m.id)
        End If
        If Not meta.xref.HMDB.StringEmpty Then
            registry.db_xrefs.ignore.add(field("db_source") = db_source, field("db_name") = vocabulary.db_hmdb, field("db_xref") = meta.xref.HMDB, field("type") = metabolite_type, field("obj_id") = m.id)
        End If
        If Not meta.xref.lipidmaps.StringEmpty Then
            registry.db_xrefs.ignore.add(field("db_source") = db_source, field("db_name") = vocabulary.db_lipidmaps, field("db_xref") = meta.xref.lipidmaps, field("type") = metabolite_type, field("obj_id") = m.id)
        End If
        If Not meta.xref.KEGG.StringEmpty Then
            registry.db_xrefs.ignore.add(field("db_source") = db_source, field("db_name") = vocabulary.db_kegg, field("db_xref") = meta.xref.KEGG, field("type") = metabolite_type, field("obj_id") = m.id)
        End If
        If Not meta.xref.MeSH.StringEmpty Then
            registry.db_xrefs.ignore.add(field("db_source") = db_source, field("db_name") = vocabulary.db_mesh, field("db_xref") = meta.xref.MeSH, field("type") = metabolite_type, field("obj_id") = m.id)
        End If
        If Not meta.xref.Wikipedia.StringEmpty Then
            registry.db_xrefs.ignore.add(field("db_source") = db_source, field("db_name") = vocabulary.db_wikipedia, field("db_xref") = meta.xref.Wikipedia, field("type") = metabolite_type, field("obj_id") = m.id)
        End If
        If Not meta.xref.MetaCyc.StringEmpty Then
            registry.db_xrefs.ignore.add(field("db_source") = db_source, field("db_name") = vocabulary.db_biocyc, field("db_xref") = meta.xref.MetaCyc, field("type") = metabolite_type, field("obj_id") = m.id)
        End If
        If Not meta.xref.DrugBank.StringEmpty Then
            registry.db_xrefs.ignore.add(field("db_source") = db_source, field("db_name") = vocabulary.db_drugbank, field("db_xref") = meta.xref.DrugBank, field("type") = metabolite_type, field("obj_id") = m.id)
        End If

        For Each id As String In meta.xref.CAS.SafeQuery
            If id.StringEmpty Then
                Continue For
            End If

            Call registry.db_xrefs _
                .ignore _
                .add(field("db_source") = db_source,
                     field("db_name") = vocabulary.db_cas,
                     field("db_xref") = id,
                     field("type") = metabolite_type,
                     field("obj_id") = m.id)
        Next
    End Sub
End Module
