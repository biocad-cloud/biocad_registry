Imports biocad_storage
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.MIME.application.json
Imports Microsoft.VisualBasic.MIME.application.json.Javascript
Imports Microsoft.VisualBasic.Scripting.Runtime
Imports Microsoft.VisualBasic.Serialization.JSON
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes

Module exportwebJSONDb

    Const db_cache As String = "G:\BlueprintCAD\App\net8.0-windows\data\local"

    Dim term As biocad_registryModel.vocabulary
    Dim reaction_term As biocad_registryModel.vocabulary
    Dim metab_term As biocad_registryModel.vocabulary

    Dim left_term As biocad_registryModel.vocabulary
    Dim right_term As biocad_registryModel.vocabulary

    Sub runlocalDbCache()
        ' Call exportOperonDb()
        ' Call exportReactions()
        Call exportMolecules()

        Pause()
    End Sub

    Sub exportMolecules()
        Dim reactions = JsonContract.LoadJsonFile(Of Dictionary(Of String, WebJSON.Reaction()))($"{db_cache}/enzyme_reactions.json")
        Dim all As String() = reactions.Values _
            .IteratesALL _
            .Select(Function(r)
                        Return r.left _
                            .JoinIterates(r.right) _
                            .Select(Function(a) a.molecule_id.ToString) _
                            .JoinIterates(r.law.SafeQuery.Select(Iterator Function(a) As IEnumerable(Of String)
                                                                     Yield a.metabolite_id

                                                                     For Each val As String In a.params.Values
                                                                         Yield val
                                                                     Next
                                                                 End Function).IteratesALL)
                    End Function) _
            .IteratesALL _
            .Where(Function(id) id.IsPattern("\d+") OrElse id.IsPattern("BioCAD\d+")) _
            .ToArray
        Dim metabolites As New List(Of WebJSON.Molecule)

        For Each id As UInteger In TqdmWrapper.Wrap(all.Select(Function(str)
                                                                   Dim a As UInteger

                                                                   If UInteger.TryParse(str.Match("\d+"), a) Then
                                                                       Return a
                                                                   Else
                                                                       Return 0
                                                                   End If
                                                               End Function).Distinct.ToArray)
            Dim mol = registry.molecule.where(field("id") = id).find(Of biocad_registryModel.molecule)

            If Not mol Is Nothing Then
                Dim moldata As New WebJSON.Molecule With {.id = mol.id, .formula = mol.formula, .name = mol.name}
                moldata.db_xrefs = registry.db_xrefs _
                    .left_join("vocabulary") _
                    .on(field("`vocabulary`.id") = field("`db_xrefs`.db_key")) _
                    .where(field("obj_id") = id) _
                    .distinct _
                    .order_by("dbname") _
                    .select(Of localcacheViews.xref_query)("term AS dbname", "xref AS xref_id") _
                    .SafeQuery _
                    .Select(Function(a) New WebJSON.DBXref With {.dbname = a.dbname, .xref_id = a.xref_id}) _
                    .ToArray

                metabolites.Add(moldata)
            End If
        Next

        Call JsonContract.GetJson(metabolites.ToArray).SaveTo($"{db_cache}/molecules.json")
    End Sub

    Sub exportReactions()
        Dim all_ec_number As String() = registry.regulation_graph.where(field("role") = 292).distinct.project(Of String)("term")

        term = registry.vocabulary.where(field("category") = "Regulation Type", field("term") = "Enzymatic Catalysis").find(Of biocad_registryModel.vocabulary)
        reaction_term = registry.vocabulary.where(field("category") = "Entity Type", field("term") = "Reaction").find(Of biocad_registryModel.vocabulary)
        metab_term = registry.vocabulary.where(field("category") = "Molecule Type", field("term") = "Metabolite").find(Of biocad_registryModel.vocabulary)

        left_term = registry.vocabulary.where(field("category") = "Compound Role", field("term") = "substrate").find(Of biocad_registryModel.vocabulary)
        right_term = registry.vocabulary.where(field("category") = "Compound Role", field("term") = "product").find(Of biocad_registryModel.vocabulary)

        Dim reactions As New Dictionary(Of String, WebJSON.Reaction())

        For Each ec_num As String In TqdmWrapper.Wrap(all_ec_number)
            Call reactions.Add(ec_num, export_reactionByID(ec_num))
        Next

        Call JsonContract.GetJson(reactions).SaveTo($"{db_cache}/enzyme_reactions.json")
    End Sub

    Public Function export_reactionByID(ec_number As String) As WebJSON.Reaction()
        Dim cats = registry.regulation_graph.where(field("term") = ec_number, field("role") = term.id).project(Of UInteger)("reaction_id")

        If cats.IsNullOrEmpty Then
            Return {}
        End If

        Dim unique_hash = registry.hashcode _
            .where(field("type_id") = reaction_term.id, field("obj_id").in(cats), field("hashcode") <> "") _
            .group_by("hashcode") _
            .select(Of localcacheViews.reaction_group)("hashcode", "GROUP_CONCAT(DISTINCT obj_id) AS reactions")
        Dim list As New Dictionary(Of String, WebJSON.Reaction)

        For Each hash As localcacheViews.reaction_group In unique_hash
            Dim first_id = hash.reactions.Split(","c).FirstOrDefault

            If first_id.StringEmpty Then
                Continue For
            End If

            Dim rxn = registry.reaction.where(field("id") = first_id).find(Of biocad_registryModel.reaction)
            Dim left = registry.reaction_graph.where(field("reaction") = first_id, field("role") = left_term.id).select(Of biocad_registryModel.reaction_graph)
            Dim right = registry.reaction_graph.where(field("reaction") = first_id, field("role") = right_term.id).select(Of biocad_registryModel.reaction_graph)

            If left.IsNullOrEmpty OrElse right.IsNullOrEmpty Then
                Continue For
            End If
            If left.JoinIterates(right).Any(Function(a) a.molecule_id = 0) Then
                Continue For
            End If

            Dim mol_list = left.JoinIterates(right).Select(Function(a) a.molecule_id).ToArray
            Dim args = registry.kinetic_law _
                .left_join("kinetic_substrate") _
                .on(field("`kinetic_law`.id") = field("`kinetic_substrate`.kinetic_id")) _
                .where(field("ec_number") = ec_number,
                       field("metabolite_id").in(mol_list),
                       field("temperature").between(25, 40)) _
                .select(Of localcacheViews.kinetics_args)("params", "lambda", "metabolite_id", "json_str")

            For i As Integer = 0 To args.Length - 1
                Dim pars = args(i).params.LoadJSON(Of Dictionary(Of String, String))
                Dim pack_json As JsonObject = JsonParser.Parse(args(i).json_str)
                Dim json As JsonObject = pack_json("xref")
                Dim missing As Boolean = False
                Dim parsData As New Dictionary(Of String, String)

                For Each tuple In pars
                    Dim name = tuple.Key
                    Dim val = tuple.Value

                    If json.HasObjectKey(val) Then
                        Dim idset = json(val).As(Of JsonArray).AsStringVector(False)

                        If idset.IsNullOrEmpty Then
                            missing = True
                        Else
                            idset = registry.db_xrefs _
                                .where(field("type") = metab_term.id, field("xref").in(idset)) _
                                .distinct _
                                .project(Of UInteger)("obj_id") _
                                .AsCharacter _
                                .ToArray

                            idset = idset.Intersect(mol_list.AsCharacter).ToArray

                            If idset.IsNullOrEmpty Then
                                missing = True
                            Else
                                val = "BioCAD" & idset.First.PadLeft(11, "0"c)
                            End If
                        End If
                    End If

                    parsData(name) = val
                Next

                If Not missing Then
                    args(i).params = parsData.GetJson(enumToStr:=True)
                Else
                    args(i) = Nothing
                End If
            Next

            list(hash.hashcode) = New WebJSON.Reaction With {
                .guid = hash.hashcode,
                .name = rxn.name,
                .reaction = rxn.equation,
                .left = left.Select(Function(a) New WebJSON.Substrate With {.factor = a.factor, .molecule_id = a.molecule_id}).ToArray,
                .right = right.Select(Function(a) New WebJSON.Substrate With {.factor = a.factor, .molecule_id = a.molecule_id}).ToArray,
                .law = args.Where(Function(a) Not a Is Nothing).Select(Function(a) New WebJSON.LawData With {
                .lambda = a.lambda,
                .metabolite_id = a.metabolite_id,
                .params = a.params.LoadJSON(Of Dictionary(Of String, String))
            }).ToArray
            }
        Next

        Return list.Values.ToArray
    End Function

    Sub exportOperonDb()
        Dim list = registry.conserved_cluster _
            .left_join("cluster_link") _
            .on(field("`cluster_link`.`cluster_id`") = field("`conserved_cluster`.`id`")) _
            .where("NOT cluster_id IS NULL") _
            .group_by("cluster_id") _
            .select(Of localcacheViews.operonData)("cluster_id", "MIN(name) AS name", "GROUP_CONCAT(DISTINCT gene_id) AS members")
        Dim operons = list _
            .Select(Function(a)
                        Return New WebJSON.Operon With {
                            .cluster_id = a.cluster_id,
                            .name = a.name,
                            .members = a.members.Split(",")
                        }
                    End Function) _
            .ToArray

        Call operons.GetJson(enumToStr:=True).SaveTo($"{db_cache}/all_operons.json")
    End Sub
End Module

Namespace localcacheViews

    Public Class xref_query
        <DatabaseField> Public Property dbname As String
        <DatabaseField> Public Property xref_id As String
    End Class

    Public Class kinetics_args

        <DatabaseField> Public Property params As String
        <DatabaseField> Public Property lambda As String
        <DatabaseField> Public Property metabolite_id As UInteger
        <DatabaseField> Public Property json_str As String

    End Class

    Public Class reaction_group

        <DatabaseField> Public Property hashcode As String
        <DatabaseField> Public Property reactions As String

    End Class

    Public Class operonData

        <DatabaseField> Public Property cluster_id As UInteger
        <DatabaseField> Public Property name As String
        <DatabaseField> Public Property members As String

    End Class

End Namespace