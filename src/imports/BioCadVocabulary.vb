
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

    Sub New(registry As biocad_registry)
        gene_term = registry.getVocabulary("Nucleic Acid", "Molecule Type")
        rna_term = registry.getVocabulary("RNA", "Molecule Type")
        protein_term = registry.getVocabulary("Polypeptide", "Molecule Type")
        metabolite_term = registry.getVocabulary("Metabolite", "Molecule Type")
        complex_term = registry.getVocabulary("Complex", "Molecule Type")
        dna_motif_term = registry.getVocabulary("Nucleotide Motif", "Molecule Type")
        protein_motif_term = registry.getVocabulary("Protein Motif", "Molecule Type")
    End Sub
End Class
