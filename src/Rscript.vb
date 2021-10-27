Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Data.IO.MessagePack
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports SMRUCC.Rsharp.Runtime.Interop
Imports REnv = SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp
Imports System.IO
Imports SMRUCC.Rsharp.Runtime.Components

<Package("MessagePack")>
Public Module Rscript

    <ExportAPI("pack")>
    Public Function pack(<RRawVectorArgument> x As Object,
                         <RListObjectArgument>
                         Optional schema As list = Nothing,
                         Optional env As Environment = Nothing) As Object

        If TypeOf x Is vector Then
            Dim vec As vector = DirectCast(x, vector)
            Dim array As Array = REnv.TryCastGenericArray(vec.data, env)
            Dim type As RType

            If vec.elementType Is Nothing Then
                type = RType.GetRSharpType(array.GetType.GetElementType)
            Else
                type = vec.elementType
            End If

            If type.mode.IsPrimitive(False) Then
                Return MsgPackSerializer.SerializeObject(array)
            Else
                Throw New NotImplementedException
            End If
        Else
            Throw New NotImplementedException
        End If
    End Function

    <ExportAPI("unpack")>
    Public Function unpack(<RRawVectorArgument> data As Object, Optional env As Environment = Nothing) As Object
        Dim buffer = GetFileStream(data, FileAccess.ReadWrite, env)

        If buffer Like GetType(Message) Then
            Return buffer.TryCast(Of Message)
        End If

        Return MsgPackSerializer.Deserialize(buffer.TryCast(Of Stream))
    End Function
End Module
