Imports Oracle.LinuxCompatibility.MySQL
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Uri

Namespace biocad_registryModel

Public MustInherit Class db_mysql : Inherits IDatabase
Protected ReadOnly complex As Model
Protected ReadOnly dblinks As Model
Protected ReadOnly family As Model
Protected ReadOnly gene_ontology As Model
Protected ReadOnly genomes As Model
Protected ReadOnly go_dag As Model
Protected ReadOnly model_graph As Model
Protected ReadOnly molecules As Model
Protected ReadOnly motif As Model
Protected ReadOnly motif_sites As Model
Protected ReadOnly ncbi_taxonomy_tree As Model
Protected ReadOnly operon_graph As Model
Protected ReadOnly operon_group As Model
Protected ReadOnly pathway As Model
Protected ReadOnly reaction_graph As Model
Protected ReadOnly reaction_node As Model
Protected ReadOnly regulator As Model
Protected ReadOnly seq_archive As Model
Protected ReadOnly seq_graph As Model
Protected ReadOnly subcellular_compartments As Model
Protected ReadOnly subcellular_locations As Model
Protected ReadOnly synonym As Model
Protected ReadOnly taxonomic As Model
Protected ReadOnly vocabulary As Model
Protected Sub New(mysqli As ConnectionUri)
Call MyBase.New(mysqli)

Me.complex = model(Of complex)()
Me.dblinks = model(Of dblinks)()
Me.family = model(Of family)()
Me.gene_ontology = model(Of gene_ontology)()
Me.genomes = model(Of genomes)()
Me.go_dag = model(Of go_dag)()
Me.model_graph = model(Of model_graph)()
Me.molecules = model(Of molecules)()
Me.motif = model(Of motif)()
Me.motif_sites = model(Of motif_sites)()
Me.ncbi_taxonomy_tree = model(Of ncbi_taxonomy_tree)()
Me.operon_graph = model(Of operon_graph)()
Me.operon_group = model(Of operon_group)()
Me.pathway = model(Of pathway)()
Me.reaction_graph = model(Of reaction_graph)()
Me.reaction_node = model(Of reaction_node)()
Me.regulator = model(Of regulator)()
Me.seq_archive = model(Of seq_archive)()
Me.seq_graph = model(Of seq_graph)()
Me.subcellular_compartments = model(Of subcellular_compartments)()
Me.subcellular_locations = model(Of subcellular_locations)()
Me.synonym = model(Of synonym)()
Me.taxonomic = model(Of taxonomic)()
Me.vocabulary = model(Of vocabulary)()
End Sub
End Class

End Namespace
