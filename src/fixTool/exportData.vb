Imports biocad_storage
Imports BioNovoGene.BioDeep.Chemoinformatics.Formula
Imports BioNovoGene.BioDeep.MSEngine
Imports Microsoft.VisualBasic.Data.Framework
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
            "Synthetic steroids", "antibiotic", "carcinogen", "industrial pollutants", "Drug metabolites", "drug"
        }

        Dim checks_atom = {"Br", "Li", "Ge", "Ba", "Cu", "F", "Se", "Si", "Na", "As", "I", "Pb", "B", "Hg", "Ag", "Sn", "Zn", "Co", "Rh", "Al", "Cd", "Mn", "Ti", "Pt", "Ni"}

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
            .Where(Function(a) Not a.First.id.name.IsPattern("CHEMBL\d+")) _
            .Where(Function(a) a.First.id.name.Length < 64) _
            .Where(Function(a)
                       Dim formula = FormulaScanner.ScanFormula(a.First.id.formula)

                       If formula Is Nothing Then
                           Return False
                       End If

                       For Each atom In checks_atom
                           If formula(atom) > 0 Then
                               Return False
                           End If
                       Next

                       Return True
                   End Function) _
            .Where(Function(a) Not (LCase(a.First.id.name).StartsWith("zinc ") OrElse LCase(a.First.id.name).StartsWith("silver "))) _
            .Where(Function(a) LCase(a.First.id.name).InStrAny("potassium", "sodium", "calcium", "example") <= 0) _
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

        Dim smiles = Program.registry.ExportSmiles(all.Select(Function(a) a.id)).IteratesALL.ToArray

        Call smiles.SaveTo("./plant_smiles.csv")

        Pause()
    End Sub


End Module
