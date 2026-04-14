Imports Microsoft.VisualBasic.Language.UnixBash
Imports Microsoft.VisualBasic.Linq
Imports biopax = SMRUCC.genomics.Model.Biopax.Level3.File

Module fixRhea

    Sub Main2()
        For Each filepath As String In ls - l - r - "*.owl" <= "D:\datapool\reactions\chunks"
            Dim xml As biopax = biopax.LoadDoc(filepath)
            Dim loader As SMRUCC.genomics.Model.Biopax.Level3.ResourceReader = SMRUCC.genomics.Model.Biopax.Level3.ResourceReader.LoadResource(file:=xml)
            Dim reactions = loader.GetAllReactions(entity_refs:=True).ToArray
            Dim compounds = loader.GetAllCompounds.ToArray

            Pause()
        Next
    End Sub
End Module
