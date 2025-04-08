Imports Oracle.LinuxCompatibility.MySQL
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Uri

Namespace biocad_registryModel

Public MustInherit Class db_mysql : Inherits IDatabase
Protected ReadOnly m_complex As Model
Protected ReadOnly m_db_xrefs As Model
Protected ReadOnly m_genomics As Model
Protected ReadOnly m_kinetic_law As Model
Protected ReadOnly m_molecule As Model
Protected ReadOnly m_molecule_function As Model
Protected ReadOnly m_molecule_ontology As Model
Protected ReadOnly m_molecule_tags As Model
Protected ReadOnly m_ncbi_taxonomy As Model
Protected ReadOnly m_odor As Model
Protected ReadOnly m_ontology As Model
Protected ReadOnly m_ontology_tree As Model
Protected ReadOnly m_pathway As Model
Protected ReadOnly m_pathway_graph As Model
Protected ReadOnly m_reaction As Model
Protected ReadOnly m_reaction_graph As Model
Protected ReadOnly m_regulation_graph As Model
Protected ReadOnly m_sequence_graph As Model
Protected ReadOnly m_subcellular_compartments As Model
Protected ReadOnly m_subcellular_location As Model
Protected ReadOnly m_synonym As Model
Protected ReadOnly m_taxonomy_tree As Model
Protected ReadOnly m_vocabulary As Model
Protected Sub New(mysqli As ConnectionUri)
Call MyBase.New(mysqli)

Me.m_complex = model(Of complex)()
Me.m_db_xrefs = model(Of db_xrefs)()
Me.m_genomics = model(Of genomics)()
Me.m_kinetic_law = model(Of kinetic_law)()
Me.m_molecule = model(Of molecule)()
Me.m_molecule_function = model(Of molecule_function)()
Me.m_molecule_ontology = model(Of molecule_ontology)()
Me.m_molecule_tags = model(Of molecule_tags)()
Me.m_ncbi_taxonomy = model(Of ncbi_taxonomy)()
Me.m_odor = model(Of odor)()
Me.m_ontology = model(Of ontology)()
Me.m_ontology_tree = model(Of ontology_tree)()
Me.m_pathway = model(Of pathway)()
Me.m_pathway_graph = model(Of pathway_graph)()
Me.m_reaction = model(Of reaction)()
Me.m_reaction_graph = model(Of reaction_graph)()
Me.m_regulation_graph = model(Of regulation_graph)()
Me.m_sequence_graph = model(Of sequence_graph)()
Me.m_subcellular_compartments = model(Of subcellular_compartments)()
Me.m_subcellular_location = model(Of subcellular_location)()
Me.m_synonym = model(Of synonym)()
Me.m_taxonomy_tree = model(Of taxonomy_tree)()
Me.m_vocabulary = model(Of vocabulary)()
End Sub
End Class

End Namespace
