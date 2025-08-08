Imports biocad_storage
Imports Microsoft.VisualBasic.Linq

Module exportData

    Sub export_plantid()
        Dim topics_plant = {"wheat",
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
                                            Return Program.registry.ExportTagList(tag).Select(Function(id) (id, tag, "plant"))
                                        End Function).IteratesALL.ToArray
        Dim excludeList = excludes.Select(Function(tag)
                                              Return Program.registry.ExportTagList(tag).Select(Function(id) (id, tag, "not plant"))
                                          End Function).IteratesALL.ToArray
        Dim all = plant.JoinIterates(excludeList).GroupBy(Function(r) r.id) _
            .Select(Function(r)
                        Return (r.Key, r.GroupBy(Function(a) a.Item3).ToDictionary(Function(a) a.Key, Function(a) a.Select(Function(i) i.tag).Distinct.ToArray))
                    End Function) _
            .OrderByDescending(Function(a)
                                   Return a.Item2.TryGetValue("not plant").TryCount
                               End Function) _
            .ToArray

        Pause()
    End Sub


End Module
