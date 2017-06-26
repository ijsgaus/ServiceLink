#r @"packages/tools/FAKE/tools/FakeLib.dll"

open System.IO
open Fake

Target "Build" (fun _ ->
    DotNetCli.Restore id
    DotNetCli.Build (fun p -> { p with Configuration = "Debug"})
)


RunTargetOrDefault "Build"