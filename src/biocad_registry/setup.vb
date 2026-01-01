
Imports BioNovoGene.BioDeep.Chemistry.MetaLib
Imports BioNovoGene.BioDeep.Chemoinformatics.Formula
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports registry_data
Imports registry_data.biocad_registryModel
Imports SMRUCC.genomics.Assembly.KEGG.DBGET.bGetObject
Imports SMRUCC.genomics.ComponentModel.DBLinkBuilder
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Internal.[Object]
Imports SMRUCC.Rsharp.Runtime.Interop

''' <summary>
''' The Initial setup of the database
''' </summary>
''' 
<Package("setup")>
Public Module setup

    ''' <summary>
    ''' use kegg+refmet as template for make setup of the metabolites table
    ''' </summary>
    ''' <param name="kegg"></param>
    ''' <param name="refmet"></param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("setup_metabolites")>
    Public Function setup_metabolites(registry As biocad_registry, <RRawVectorArgument> kegg As Object, <RRawVectorArgument> refmet As Object, Optional env As Environment = Nothing) As Object
        Dim keggLib As pipeline = pipeline.TryCreatePipeline(Of Compound)(kegg, env)
        Dim refmetLib As pipeline = pipeline.TryCreatePipeline(Of RefMet)(refmet, env)

        If keggLib.isError Then
            Return keggLib.getError
        ElseIf refmetLib.isError Then
            Return refmetLib.getError
        End If

        For Each cpd As Compound In keggLib.populates(Of Compound)(env)
            Dim m As metabolites = registry.metabolites _
                .where(field("kegg_id") = cpd.entry) _
                .find(Of metabolites)

            If m Is Nothing Then
                Dim exact_mass As Double = FormulaScanner.EvaluateExactMass(cpd.formula)
                Dim name As String = cpd.commonNames.DefaultFirst([default]:=cpd.entry)
                Dim db_xrefs As DBLink() = cpd.DbLinks

                registry.metabolites.add(
                    field("name") = name,
                    field("hashcode") = name.ToLower.MD5,
                    field("formula") = cpd.formula,
                    field("exact_mass") = If(exact_mass < 0, 0, exact_mass),
                    field("kegg_id") = cpd.entry
                )
            End If
        Next

        For Each met As RefMet In refmetLib.populates(Of RefMet)(env)

        Next

        Return Nothing
    End Function

End Module
