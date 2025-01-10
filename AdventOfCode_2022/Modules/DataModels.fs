namespace AoC_2022.Modules

[<AutoOpen>]
module DataModels = 
    type NodeEnum =
        | File = 0
        | Directory = 1

    type FileSystemItem = {
        Name: string;
        NodeType: NodeEnum;
        Size: int;
        Parent: string;
        Path: string
    }

    type Monkey = {
        Idx: int;
        Items: bigint list;
        Op: string;
        OpValue1: string;
        OpValue2: string;
        TestValue: bigint;
        TrueThrow: int;
        FalseThrow: int;
        NumberOfInspects: bigint;
    }