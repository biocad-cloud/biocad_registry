
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder

''' <summary>
''' A wrapper for the biocad registry vocabulary table
''' </summary>
Public Class BioCadVocabulary

#Region "molecule type vocabulary"
    Public ReadOnly Property gene_term As UInteger
    Public ReadOnly Property rna_term As UInteger
    Public ReadOnly Property protein_term As UInteger
    Public ReadOnly Property metabolite_term As UInteger
    Public ReadOnly Property complex_term As UInteger
    Public ReadOnly Property dna_motif_term As UInteger
    Public ReadOnly Property protein_motif_term As UInteger
#End Region

#Region "database source"
    Public ReadOnly Property genbank_term As UInteger
    Public ReadOnly Property kegg_term As UInteger
    Public ReadOnly Property uniprot_term As UInteger
    Public ReadOnly Property biocad_term As UInteger
    Public ReadOnly Property pubchem_term As UInteger
    Public ReadOnly Property ecnumber_term As UInteger
    Public ReadOnly Property chebi_term As UInteger
    Public ReadOnly Property hmdb_term As UInteger
    ''' <summary>
    ''' gene ontology
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property go_term As UInteger
#End Region

    Public Const CategoryDatabase = "External Database"
    Public Const CategoryMolecule = "Molecule Type"

    ReadOnly registry As biocad_registry

    Public ReadOnly Property molecule_entity As UInteger

    Sub New(registry As biocad_registry)
        Me.registry = registry

        gene_term = registry.getVocabulary("Nucleic Acid", CategoryMolecule)
        rna_term = registry.getVocabulary("RNA", CategoryMolecule)
        protein_term = registry.getVocabulary("Polypeptide", CategoryMolecule)
        metabolite_term = registry.getVocabulary("Metabolite", CategoryMolecule)
        complex_term = registry.getVocabulary("Complex", CategoryMolecule)
        dna_motif_term = registry.getVocabulary("Nucleotide Motif", CategoryMolecule)
        protein_motif_term = registry.getVocabulary("Protein Motif", CategoryMolecule)

        ' database source
        genbank_term = registry.getVocabulary("NCBI GenBank", CategoryDatabase)
        kegg_term = registry.getVocabulary("KEGG", CategoryDatabase)
        uniprot_term = registry.getVocabulary("UniProtKB/Swiss-Prot", CategoryDatabase)
        biocad_term = registry.getVocabulary("BioCAD Registry", CategoryDatabase)
        pubchem_term = registry.getVocabulary("PubChem", CategoryDatabase)
        ecnumber_term = registry.getVocabulary("EC", CategoryDatabase)
        chebi_term = registry.getVocabulary("chebi", CategoryDatabase)
        go_term = registry.getVocabulary("GO", CategoryDatabase)
        hmdb_term = registry.getVocabulary("HMDB", CategoryDatabase)

        molecule_entity = registry.getVocabulary("Molecule", "Entity Type")
    End Sub

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function GetDatabaseKey(key As String) As UInteger
        Static dbnameMap As New Dictionary(Of String, String) From {
            {"", ""}
        }

        Return registry.getVocabulary(If(dbnameMap.ContainsKey(key), dbnameMap(key), key), CategoryDatabase)
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function GetVocabularyTerm(key As String, category As String) As UInteger
        Return registry.getVocabulary(key, category)
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function GetUniProtKeyword(id As String, name As String) As UInteger
        Return registry.getVocabulary(name, "UniProt Keyword", id)
    End Function

    Public Function GetBioCadOntology(id As String, Optional name As String = Nothing) As biocad_registryModel.ontology
        Static cache As New Dictionary(Of String, biocad_registryModel.ontology)
        Return cache.ComputeIfAbsent(id,
            lazyValue:=Function(db_xref)
                           Dim find = registry.ontology.find_object(field("db_xref") = db_xref, field("db_source") = biocad_term)

                           If find Is Nothing Then
                               registry.ontology.add(
                                   field("db_xref") = db_xref,
                                   field("db_source") = biocad_term,
                                   field("name") = name
                               )
                               find = registry.ontology.find_object(
                                   field("db_xref") = db_xref,
                                   field("db_source") = biocad_term
                               )
                           End If

                           Return find
                       End Function)
    End Function

    Public Function GetOntologyTerm(id As String, rank As String, source_db As String, Optional name As String = Nothing) As biocad_registryModel.ontology
        If id Is Nothing Then
            Return Nothing
        End If

        Dim unikey As String = $"{id}-{rank}-{source_db}"

        Static cache As New Dictionary(Of String, biocad_registryModel.ontology)

        Return cache.ComputeIfAbsent(unikey,
             Function(null)
                 Dim rank_id As UInteger = GetVocabularyTerm(rank, $"{source_db} Rank")
                 Dim dbkey As UInteger = GetDatabaseKey(source_db)
                 Dim find = registry.ontology.find_object(field("db_xref") = id,
                                                          field("db_source") = dbkey,
                                                          field("rank") = rank_id)

                 If find Is Nothing Then
                     registry.ontology.add(
                        field("db_xref") = id,
                        field("db_source") = dbkey,
                        field("rank") = rank_id,
                        field("name") = name
                     )

                     find = registry.ontology.find_object(field("db_xref") = id,
                                                          field("db_source") = dbkey,
                                                          field("rank") = rank_id)
                 End If

                 Return find
             End Function)
    End Function
End Class
