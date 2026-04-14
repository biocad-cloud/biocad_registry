Imports Microsoft.VisualBasic.Linq
Imports biopax = SMRUCC.genomics.Model.Biopax.Level3.File

Module fixRhea

    Sub Main2()
        Dim xml As biopax = biopax.LoadDoc("D:\datapool\rhea-biopax.owl")
        Dim loader As SMRUCC.genomics.Model.Biopax.Level3.ResourceReader = SMRUCC.genomics.Model.Biopax.Level3.ResourceReader.LoadResource(file:=xml)
        Dim reactions = loader.GetAllReactions.ToArray

        Pause()
    End Sub
End Module
