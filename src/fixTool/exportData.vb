Imports biocad_storage
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

        Dim plant = topics_plant.Select(Function(tag)
                                            Return Program.registry.ExportTagData(tag).Select(Function(id) (id, tag, "plant"))
                                        End Function).IteratesALL.Where(Function(a) a.id.exact_mass > 50 AndAlso a.id.exact_mass < 1200).ToArray
        Dim all = plant.GroupBy(Function(r) r.id.id) _
            .Select(Function(r)
                        Dim name = r.First.id.name
                        Dim id = r.First.id.id
                        Dim plants = r.Where(Function(a) a.Item3 = "plant").Select(Function(a) a.tag).Distinct.ToArray
                        Dim not_plants = r.Where(Function(a) a.Item3 <> "plant").Select(Function(a) a.tag).Distinct.ToArray

                        Return (id, name, plants, not_plants)
                    End Function) _
            .OrderByDescending(Function(a)
                                   Return a.plants.TryCount
                               End Function) _
            .Take(20000) _
            .ToArray

        Pause()
    End Sub


End Module
