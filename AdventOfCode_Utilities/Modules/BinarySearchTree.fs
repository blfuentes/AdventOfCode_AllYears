namespace AdventOfCode_Utilities.Modules.BinarySearthTree

module BinarySearthTree = 
    type Tree<'a> = 
      | Empty
      | Node of value: 'a * left: Tree<'a> * right: Tree<'a>

    let isBST (tree : Tree<'a>) =
      let rec verify lo hi tree =
        match tree with
        | Empty -> true
        | Node (value, left, right) ->
          match lo, hi with
          | Some lo, _ when value < lo -> false
          | _, Some hi when value > hi -> false
          | _ ->
            let hi' = defaultArg hi value |> min value |> Some
            let lo' = defaultArg lo value |> max value |> Some
            verify lo hi' left && verify lo' hi right
    
      verify None None tree

    let rec insert newValue (tree : Tree<'a>) =
      match tree with
      | Empty -> Node (newValue, Empty, Empty)
      | Node (value, left, right) when newValue < value ->
        let left' = insert newValue left
        Node (value, left', right)
      | Node (value, left, right) when newValue > value ->
        let right' = insert newValue right
        Node (value, left, right')
      | _ -> tree

    let rec findInOrderPredecessor (tree : Tree<'a>) =
      match tree with
      | Empty -> Empty
      | Node (_, _, Empty) -> tree
      | Node (_, _, right) -> findInOrderPredecessor right 
    
    let rec delete value (tree : Tree<'a>) =
      match tree with
      | Empty -> Empty
      | Node (value', left, right) when value < value' ->
        let left' = delete value left
        Node (value', left', right)
      | Node (value', left, right) when value > value' ->
        let right' = delete value right
        Node (value', left, right')
      | Node (_, Empty, Empty) ->
        Empty
      | Node (_, left, Empty) -> 
        left
      | Node (_, Empty, right) ->
        right
      | Node (_, left, right) ->
        let (Node(value', _, _)) = findInOrderPredecessor left
        let left' = delete value' left
        Node (value', left', right)

    // Process the tree in order: 1 2 3 4 5 6 - Breadth-First Traversal
    let BFtraverse (tree : Tree<'a>) =
      let rec loop (trees : seq<Tree<'a>>) = seq {
        let values = 
          trees
          |> Seq.choose (function
            | Empty -> None
            | Node (value, _, _) -> Some value)
    
        yield! values
    
        let subtrees = 
          trees 
          |> Seq.collect (function 
            | Empty -> Seq.empty
            | Node (value, left, right) -> seq { yield left; yield right })
          |> Seq.toArray
    
        if subtrees.Length > 0 then
          yield! loop subtrees
      }
    
      loop <| seq { yield tree }

    // Process the tree in order: 1 2 4 5 3 6 - Depth-First Traversal
    let DFtraverse (tree : Tree<'a>) =
      let rec loop (tree : Tree<'a>) = seq {
        match tree with
        | Empty -> ()
        | Node (value, left, right) ->
          yield value
    
          yield! loop left
          yield! loop right
      }
      
      loop tree