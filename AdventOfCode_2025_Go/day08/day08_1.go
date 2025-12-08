package day08

import (
	"sort"
	"strings"

	"github.com/blfuentes/AdventOfCode_2025_Go/utilities"
)

func Executepart1() int64 {
	var result int64 = 0

	// var fileName string = "./day08/test_input_08.txt"
	// var numOfPairs int = 10
	var fileName string = "./day08/day08.txt"
	var numOfPairs int = 1000
	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {
		boxes := make([]Box, len(fileContent))
		for i, row := range fileContent {
			parts := strings.Split(row, ",")
			boxes[i] = Box{
				Id: i,
				X:  utilities.StringToInt64(parts[0]),
				Y:  utilities.StringToInt64(parts[1]),
				Z:  utilities.StringToInt64(parts[2]),
			}
		}
		distancePairs := make([]DistancePair, 0)
		for i := 0; i < len(boxes)-1; i++ {
			boxA := boxes[i]
			for j := i + 1; j < len(boxes); j++ {
				boxB := boxes[j]
				distancePairs = append(distancePairs, DistancePair{
					BoxAId:   boxA.Id,
					BoxBId:   boxB.Id,
					Distance: EuclideanDistanceSquared(boxA, boxB),
				})
			}
		}

		sort.Slice(distancePairs, func(i, j int) bool {
			return distancePairs[i].Distance < distancePairs[j].Distance
		})

		circuits := make([]utilities.Set[int], 0)
		for i := range numOfPairs {
			BoxAId, BoxBId := distancePairs[i].BoxAId, distancePairs[i].BoxBId
			// fmt.Printf("Processing %d - %d with distance %d\n", BoxAId, BoxBId, distancePairs[i].Distance)
			circuitAIdx, circuitBIdx := -1, -1
			for cIdx, circuit := range circuits {
				for _, item := range circuit.Values() {
					if item == BoxAId {
						circuitAIdx = cIdx
					}
					if item == BoxBId {
						circuitBIdx = cIdx
					}
				}
			}
			// merge circuits if both boxes are found in different circuits
			if circuitAIdx != -1 && circuitBIdx != -1 && circuitAIdx != circuitBIdx {
				for _, item := range circuits[circuitBIdx].Values() {
					circuits[circuitAIdx].Add(item)
				}
				// remove circuitBIdx
				circuits = append(circuits[:circuitBIdx], circuits[circuitBIdx+1:]...)
				// fmt.Printf("Merged circuits %d and %d\n", circuitAIdx, circuitBIdx)
			} else if circuitAIdx == -1 && circuitBIdx == -1 {
				newCircuit := utilities.NewSet[int]()
				newCircuit.Add(BoxAId)
				newCircuit.Add(BoxBId)
				circuits = append(circuits, *newCircuit)
				// fmt.Printf("Created new circuit with boxes %d and %d\n", BoxAId, BoxBId)
			} else if circuitAIdx != -1 {
				circuits[circuitAIdx].Add(BoxBId)
				// fmt.Printf("Added boxes %d and %d to circuit %d\n", BoxAId, BoxBId, circuitAIdx)
				// fmt.Printf("Current boxes: %v\n", circuits[circuitAIdx].Values())
			} else if circuitBIdx != -1 && circuitAIdx != circuitBIdx {
				circuits[circuitBIdx].Add(BoxAId)
				// fmt.Printf("Added boxes %d and %d to circuit %d\n", BoxAId, BoxBId, circuitBIdx)
				// fmt.Printf("Current boxes: %v\n", circuits[circuitBIdx].Values())
			}
		}
		sort.Slice(circuits, func(i, j int) bool {
			return circuits[i].Size() > circuits[j].Size()
		})
		result = int64(circuits[0].Size() * circuits[1].Size() * circuits[2].Size())
	}
	return result
}
