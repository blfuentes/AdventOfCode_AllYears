namespace AdventOfCode_Utilities.Modules

module GridGraph =
    type Node = { Row: int; Col: int; Value: int; }
    
    type Edge = { Start: Node; End: Node; Weight: int }

    let generateGraph (grid: int[,]) =
        let nodes =
            seq {
                for i in 0..(grid.GetUpperBound(0)) do
                for j in 0..(grid.GetUpperBound(1)) do
                    yield { Row = i; Col = j; Value = grid.[i,j] }     
            }
        let edges =
            seq {
                for i in 0..(grid.GetUpperBound(0)) do
                    for j in 0..(grid.GetUpperBound(1)) do
                        let currentNode = { Row = i; Col = j; Value = grid.[i,j] }
                        let neighbors =
                            seq {
                                for k in -1..1 do
                                    for l in -1..1 do
                                        if k <> 0 || l <> 0 then
                                            let neighborRow = i + k
                                            let neighborCol = j + l
                                            if neighborRow >= 0 && neighborRow <= grid.GetUpperBound(0) && 
                                                neighborCol >= 0 && neighborCol <= grid.GetUpperBound(1) then
                                                yield { Row = neighborRow; Col = neighborCol; Value = grid.[neighborRow, neighborCol] }
                            }
                        for neighbor in neighbors do
                            yield { Start = currentNode; End = neighbor; Weight = 1 }
            }
        nodes, edges