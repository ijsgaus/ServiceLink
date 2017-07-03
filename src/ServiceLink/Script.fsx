// Learn more about F# at http://fsharp.org. See the 'F# Tutorial' project
// for more guidance on F# programming.


#r @"bin\Debug\ServiceLink.Markers.Full.dll"
#r @"bin\Debug\ServiceLink.Full.dll"
open ServiceLink
open FSharp.Quotations

type ITestService =
    abstract Event : int
    
let cfg = ServiceLinkConfiguration
let srv = ServiceLink.create<ITestService> cfg
let expr : Expr<ITestService -> int> = <@ fun p -> p.Event @> 

