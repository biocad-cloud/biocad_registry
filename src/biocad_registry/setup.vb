
Imports BioNovoGene.BioDeep.Chemistry.MetaLib
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports registry_data
Imports SMRUCC.genomics.Assembly.KEGG.DBGET.bGetObject
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

        Next

        Return Nothing
    End Function

End Module
