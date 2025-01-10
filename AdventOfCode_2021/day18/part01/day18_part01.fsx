open System
open System.IO
open System.Text.RegularExpressions
open System.Collections.Generic
open System.Globalization

//let path = "day18_input.txt"
let path = "test_input_00.txt"  

let snailfishpaircollection = 
    File.ReadLines(__SOURCE_DIRECTORY__ + @"../../" + path)

let addition(first, second) =
    (first, second)