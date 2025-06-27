Imports BioNovoGene.BioDeep.Chemistry.MetaLib.CrossReference
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Linq
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder

Public Class ExportMetabolites

    ReadOnly registry As biocad_registry

    ReadOnly mona As UInteger
    ReadOnly cas As UInteger
    ReadOnly kegg As UInteger
    ReadOnly kegg_drug As UInteger
    ReadOnly hmdb As UInteger
    ReadOnly lipidmaps As UInteger
    ReadOnly mesh As UInteger
    ReadOnly biocyc As UInteger
    ReadOnly chebi As UInteger
    ReadOnly chembl As UInteger
    ReadOnly ChemIDplus As UInteger
    ReadOnly chemspider As UInteger
    ReadOnly DrugBank As UInteger
    ReadOnly foodb As UInteger
    ReadOnly KNApSAcK As UInteger
    ReadOnly Wikipedia As UInteger
    ReadOnly PubChem As UInteger
    ReadOnly metlin As UInteger

    Sub New(registry As biocad_registry)
        Me.registry = registry

        Me.mona = registry.vocabulary_terms.GetDatabaseKey("MoNA")
        Me.cas = registry.vocabulary_terms.GetDatabaseKey("CAS")
        Me.kegg = registry.vocabulary_terms.GetDatabaseKey("KEGG")
        Me.kegg_drug = registry.vocabulary_terms.GetDatabaseKey("KEGGdrug")
        Me.hmdb = registry.vocabulary_terms.GetDatabaseKey("HMDB")
        Me.lipidmaps = registry.vocabulary_terms.GetDatabaseKey("lipidmaps")
        Me.mesh = registry.vocabulary_terms.GetDatabaseKey("MeSH")
        Me.biocyc = registry.vocabulary_terms.GetDatabaseKey("biocyc")
        Me.chebi = registry.vocabulary_terms.GetDatabaseKey("chebi")
        Me.chembl = registry.vocabulary_terms.GetDatabaseKey("ChEMBL")
        Me.ChemIDplus = registry.vocabulary_terms.GetDatabaseKey("ChemIDplus")
        Me.chemspider = registry.vocabulary_terms.GetDatabaseKey("chemspider")
        Me.DrugBank = registry.vocabulary_terms.GetDatabaseKey("DrugBank")
        Me.foodb = registry.vocabulary_terms.GetDatabaseKey("foodb")
        Me.KNApSAcK = registry.vocabulary_terms.GetDatabaseKey("KNApSAcK")
        Me.Wikipedia = registry.vocabulary_terms.GetDatabaseKey("Wikipedia")
        Me.PubChem = registry.vocabulary_terms.GetDatabaseKey("PubChem")
        Me.metlin = registry.vocabulary_terms.GetDatabaseKey("metlin")
    End Sub

    Public Function ExportAll(Optional page_size As Integer = 10000, Optional ByRef mona_libnames As Dictionary(Of String, String) = Nothing) As IEnumerable(Of BioNovoGene.BioDeep.Chemistry.MetaLib.Models.MetaInfo)
        Dim list As New List(Of BioNovoGene.BioDeep.Chemistry.MetaLib.Models.MetaInfo)

        mona_libnames = New Dictionary(Of String, String)

        For i As Integer = 0 To Integer.MaxValue
            Dim pagedata = registry.molecule _
                .where(field("type") = registry.vocabulary_terms.metabolite_term) _
                .limit(i * page_size, page_size) _
                .select(Of biocad_registryModel.molecule)

            If pagedata.IsNullOrEmpty Then
                Exit For
            Else
                Call list.AddRange(ExportByID(pagedata, mona_libnames, tqdm_wrap:=True))
            End If
        Next

        Return list
    End Function

    ''' <summary>
    ''' export by biocad registry id collection
    ''' </summary>
    ''' <param name="idset">the biocad registry id collection</param>
    ''' <param name="mona_libnames"></param>
    ''' <returns></returns>
    Public Function ExportByID(idset As String(),
                               Optional ByRef mona_libnames As Dictionary(Of String, String) = Nothing,
                               Optional wrap_tqdm As Boolean = True) As IEnumerable(Of BioNovoGene.BioDeep.Chemistry.MetaLib.Models.MetaInfo)

        Dim metadata As New List(Of biocad_registryModel.molecule)

        mona_libnames = New Dictionary(Of String, String)

        For Each page As String() In idset.SplitIterator(100)
            Dim mol_id As UInteger() = page _
                .Select(Function(s) UInteger.Parse(s.Match("\d+"))) _
                .ToArray

            If page.IsNullOrEmpty Then
                Continue For
            End If

            Dim pagedata As biocad_registryModel.molecule() = registry.molecule _
                .where(field("id").in(mol_id)) _
                .select(Of biocad_registryModel.molecule)

            Call metadata.AddRange(pagedata)
        Next

        Return ExportByID(metadata, mona_libnames, wrap_tqdm)
    End Function

    Shared ReadOnly cache As New Dictionary(Of String, BioNovoGene.BioDeep.Chemistry.MetaLib.Models.MetaInfo)

    Private Iterator Function ExportByID(pagedata As ICollection(Of biocad_registryModel.molecule), mapping As Dictionary(Of String, String), tqdm_wrap As Boolean) As IEnumerable(Of BioNovoGene.BioDeep.Chemistry.MetaLib.Models.MetaInfo)
        Dim bar As Tqdm.ProgressBar = Nothing

        For Each metabolite As biocad_registryModel.molecule In TqdmWrapper.Wrap(pagedata, bar:=bar, wrap_console:=tqdm_wrap)
            Dim cad_id As String = "BioCAD" & metabolite.id.ToString.PadLeft(11, "0"c)

            If cache.ContainsKey(cad_id) Then
                Yield cache(cad_id)
            End If

            Dim mona_xrefs As biocad_registryModel.db_xrefs() = registry.db_xrefs _
                .where(field("db_key") = mona,
                       field("obj_id") = metabolite.id) _
                .select(Of biocad_registryModel.db_xrefs)

            For Each id As biocad_registryModel.db_xrefs In mona_xrefs
                mapping(id.xref) = cad_id
            Next

            Call bar.SetLabel(cad_id & " -> " & metabolite.name)

            Dim xrefs = registry.db_xrefs _
                .where(field("db_key") <> mona,
                       field("obj_id") = metabolite.id) _
                .select(Of biocad_registryModel.db_xrefs)() _
                .GroupBy(Function(a) a.db_key) _
                .ToDictionary(Function(a) a.Key,
                              Function(a)
                                  Return a _
                                      .Select(Function(x) x.xref) _
                                      .Distinct _
                                      .ToArray
                              End Function)
            Dim metab As New BioNovoGene.BioDeep.Chemistry.MetaLib.Models.MetaInfo With {
                .ID = cad_id,
                .description = metabolite.note,
                .exact_mass = metabolite.mass,
                .formula = metabolite.formula,
                .IUPACName = metabolite.name,
                .name = metabolite.name,
                .synonym = registry.synonym.where(field("obj_id") = metabolite.id).project(Of String)("synonym"),
                .xref = New xref With {
                    .CAS = xrefs.TryGetValue(cas),
                    .chebi = xrefs.TryGetValue(chebi).DefaultFirst,
                    .ChEMBL = xrefs.TryGetValue(chembl).DefaultFirst,
                    .ChemIDplus = xrefs.TryGetValue(ChemIDplus).DefaultFirst,
                    .chemspider = xrefs.TryGetValue(chemspider).DefaultFirst,
                    .DrugBank = xrefs.TryGetValue(DrugBank).DefaultFirst,
                    .foodb = xrefs.TryGetValue(foodb).DefaultFirst,
                    .HMDB = xrefs.TryGetValue(hmdb).DefaultFirst,
                    .KEGG = xrefs.TryGetValue(kegg).DefaultFirst,
                    .KEGGdrug = xrefs.TryGetValue(kegg_drug).DefaultFirst,
                    .KNApSAcK = xrefs.TryGetValue(KNApSAcK).DefaultFirst,
                    .lipidmaps = xrefs.TryGetValue(lipidmaps).DefaultFirst,
                    .MeSH = xrefs.TryGetValue(mesh).DefaultFirst,
                    .Wikipedia = xrefs.TryGetValue(Wikipedia).DefaultFirst,
                    .pubchem = xrefs.TryGetValue(PubChem).DefaultFirst,
                    .metlin = xrefs.TryGetValue(metlin).DefaultFirst,
                    .MetaCyc = xrefs.TryGetValue(biocyc).DefaultFirst,
                    .SMILES = registry.sequence_graph _
                        .where(field("molecule_id") = metabolite.id) _
                        .find(Of biocad_registryModel.sequence_graph) _
                       ?.sequence
                }
            }

            cache(cad_id) = metab

            Yield metab
        Next
    End Function

End Class
