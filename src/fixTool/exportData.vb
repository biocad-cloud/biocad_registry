Imports biocad_storage
Imports BioNovoGene.BioDeep.MSEngine
Imports Microsoft.VisualBasic.Linq

Module exportData

    Sub export_plantid()
        Dim topics_plant = {"wheat", "barley", "rice",
"flavonoid",
"glutathione",
"lignin biosynthesis",
"phenylpropanoid",
"phytohormone",
"phytosterols",
"stripe rust resistance"}
        Dim excludes = {
            "Synthetic steroids", "antibiotic", "carcinogen", "industrial pollutants", "Drug metabolites"
        }

        Dim plant = topics_plant _
            .Select(Function(tag)
                        Return Program.registry.ExportTagData(tag).Select(Function(id) (id, tag, "plant"))
                    End Function) _
            .IteratesALL _
            .Where(Function(a) a.id.exact_mass > 50 AndAlso a.id.exact_mass < 1200) _
            .ToArray
        Dim notplant = excludes _
            .Select(Function(tag)
                        Return Program.registry.ExportTagData(tag).Select(Function(id) (id, tag, "not plant"))
                    End Function) _
            .IteratesALL _
            .Where(Function(a) a.id.exact_mass > 50 AndAlso a.id.exact_mass < 1200) _
            .ToArray
        Dim all = plant.JoinIterates(notplant) _
            .GroupBy(Function(r) r.id.id) _
            .Where(Function(a)
                       Return (Not MetalIons.IsMetalIon(a.First.id.formula)) AndAlso MetalIons.IsOrganic(a.First.id.formula)
                   End Function) _
            .Where(Function(a) Not a.First.id.name.IsPattern("CID \d+")) _
            .Select(Function(r)
                        Dim name = r.First.id.name
                        Dim id = r.First.id.id
                        Dim plants = r.Where(Function(a) a.Item3 = "plant").Select(Function(a) a.tag).Distinct.ToArray
                        Dim not_plants = r.Where(Function(a) a.Item3 <> "plant").Select(Function(a) a.tag).Distinct.ToArray

                        Return (id, name, score:=plants.Length / (not_plants.Length + 1), plants, not_plants)
                    End Function) _
            .OrderByDescending(Function(a)
                                   Return a.score
                               End Function) _
            .Take(20000) _
            .ToArray

        Pause()
    End Sub


End Module
