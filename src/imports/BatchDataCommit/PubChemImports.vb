Imports System.Runtime.CompilerServices
Imports BioNovoGene.BioDeep.Chemistry.MetaLib.Models
Imports BioNovoGene.BioDeep.Chemistry.NCBI.PubChem
Imports BioNovoGene.BioDeep.Chemoinformatics.Formula
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder

Public Module PubChemImports

    <Extension>
    Public Sub RunDataCommit(registry As biocad_registry, pagedata As PugViewRecord())
        Dim trans As CommitTransaction = registry.molecule.open_transaction.ignore
        Dim terms = registry.vocabulary_terms
        Dim metadata As MetaLib() = pagedata _
            .Select(Function(m)
                        Try
                            Return m.GetMetaInfo
                        Catch ex As Exception
                            Call App.LogException(ex)
                        End Try

                        Return Nothing
                    End Function) _
            .Where(Function(m) Not m Is Nothing) _
            .ToArray

        For Each meta As MetaLib In metadata
            Dim mol As biocad_registryModel.molecule = registry.findMolecule(meta)

            If mol Is Nothing Then
                Dim mass As Double = FormulaScanner.EvaluateExactMass(meta.formula)

                ' add new 
                Call trans.add(
                    field("xref_id") = $"PubChem:{meta.ID}",
                    field("name") = meta.name,
                    field("mass") = mass,
                    field("type") = terms.metabolite_term,
                    field("formula") = meta.formula,
                    field("parent") = 0,
                    field("tax_id") = 0,
                    field("note") = meta.description
                )
            End If
        Next

        Call trans.commit()
    End Sub

    <Extension>
    Private Function findMolecule(registry As biocad_registry, meta As MetaLib) As biocad_registryModel.molecule
        Dim cid As String = $"PubChem:{meta.ID}"

    End Function

End Module
