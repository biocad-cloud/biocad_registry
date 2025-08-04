Imports BioNovoGene.BioDeep.Chemistry.LOTUS
Imports BioNovoGene.BioDeep.Chemistry.MetaLib.Models

Public Class LotusNPImports

    ReadOnly registry As biocad_registry

    Sub New(registry As biocad_registry)
        Me.registry = registry
    End Sub

    Public Sub ImportsNP(np As IEnumerable(Of NaturalProduct))
        For Each page As NaturalProduct() In np.SplitIterator(10000)
            Call importsPage(page)
        Next
    End Sub

    Private Sub importsPage(page As NaturalProduct())
        Dim meta = page.Select(Function(a) DirectCast(a.CreateMetabolite, MetaInfo)).ToArray

        Call MetaboliteImports.RunDataCommit(registry, meta, uniref:=Function(m) m.ID, lazyMol:=False)
    End Sub

End Class
