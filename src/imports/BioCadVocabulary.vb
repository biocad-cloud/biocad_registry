
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
#End Region

    Public Const CategoryDatabase = "Database"
    Public Const CategoryMolecule = "Molecule Type"

    Sub New(registry As biocad_registry)
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
        uniprot_term = registry.getVocabulary("UniProt", CategoryDatabase)
    End Sub
End Class
