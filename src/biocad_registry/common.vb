Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.Imaging.Driver
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Serialization.BinaryDumping
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports registry_data
Imports registry_data.biocad_registryModel
Imports SMRUCC.genomics.Analysis.SequenceTools.SequencePatterns.Motif
Imports SMRUCC.genomics.Analysis.SequenceTools.SequencePatterns.SequenceLogo

Module common

    ReadOnly nethost As New NetworkByteOrderBuffer

    <Extension>
    Public Sub UpdateLogo(registry As biocad_registry, model As motif, motif As MotifPWM)
        Dim pwm As Double()() = motif
        Dim matrix As String = nethost.Base64String(pwm.IteratesALL, gzip:=False)
        Dim w As Integer = pwm.Length
        Dim logo = DrawingDevice.DrawFrequency(motif, title:=model.name, logoPadding:="padding:30% 5% 20% 8%;", driver:=Drivers.SVG)
        Dim logoUri As String = logo.GetDataURI.ToString

        ' 在这里分为两个步骤做这条记录的更新
        ' 避免可能出现的sql语句过长，数据量过大导致的问题
        Call registry.motif.where(field("id") = model.id).save(field("pwm") = matrix, field("width") = w)
        Call registry.motif.where(field("id") = model.id).save(field("logo") = logoUri)
    End Sub
End Module
