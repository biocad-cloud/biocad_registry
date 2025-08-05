Imports biocad_storage
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Microsoft.VisualBasic.Data.Framework
Imports Microsoft.VisualBasic.DataMining.KMeans
Imports Microsoft.VisualBasic.MachineLearning.Bootstrapping
Imports Microsoft.VisualBasic.Net.Http
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes
Imports SMRUCC.genomics.Model.MotifGraph.ProteinStructure
Imports SMRUCC.genomics.Model.MotifGraph.ProteinStructure.Kmer

Module fingerprintBuilder

    Sub run()
        Call Embedding.RunEnzymeFingerprintBuilder(Program.registry)
    End Sub
End Module
