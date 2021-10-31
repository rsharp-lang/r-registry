#Region "Microsoft.VisualBasic::d95fbe43fdd560e1873fca930b58dc1e, Rmsgpack\src\Rscript.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xie (genetics@smrucc.org)
    '       xieguigang (xie.guigang@live.com)
    ' 
    ' Copyright (c) 2018 GPL3 Licensed
    ' 
    ' 
    ' GNU GENERAL PUBLIC LICENSE (GPL3)
    ' 
    ' 
    ' This program is free software: you can redistribute it and/or modify
    ' it under the terms of the GNU General Public License as published by
    ' the Free Software Foundation, either version 3 of the License, or
    ' (at your option) any later version.
    ' 
    ' This program is distributed in the hope that it will be useful,
    ' but WITHOUT ANY WARRANTY; without even the implied warranty of
    ' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    ' GNU General Public License for more details.
    ' 
    ' You should have received a copy of the GNU General Public License
    ' along with this program. If not, see <http://www.gnu.org/licenses/>.



    ' /********************************************************************************/

    ' Summaries:

    ' Module Rscript
    ' 
    '     Function: pack, unpack
    ' 
    ' /********************************************************************************/

#End Region

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
Imports Microsoft.VisualBasic.Data.IO.MessagePack.Constants
Imports Microsoft.VisualBasic.Serialization.JSON

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

        Dim ms As Stream = buffer.TryCast(Of Stream)
        Dim scan0 As Long = ms.Position
        Dim bytVal As Byte = ms.ReadByte
        Dim bytType As Type

        Call ms.Seek(scan0, SeekOrigin.Begin)

        Select Case bytVal
            Case MsgPackFormats.UINT_8, 147 : bytType = GetType(Byte())
            Case Else
                Dim formats = MsgPackFormats.GetEnums
                Dim err As New NotImplementedException(formats.GetJson)

                Return Internal.debug.stop(err, env)
        End Select

        Return MsgPackSerializer.Deserialize(bytType, ms)
    End Function
End Module

