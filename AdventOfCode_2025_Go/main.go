package main

import (
	"fmt"
	"time"

	"github.com/blfuentes/AdventOfCode_2025_Go/day01"
	"github.com/blfuentes/AdventOfCode_2025_Go/day02"
	"github.com/blfuentes/AdventOfCode_2025_Go/day03"
	"github.com/blfuentes/AdventOfCode_2025_Go/day04"
	"github.com/blfuentes/AdventOfCode_2025_Go/day05"
	"github.com/blfuentes/AdventOfCode_2025_Go/day06"
	"github.com/blfuentes/AdventOfCode_2025_Go/day07"
	"github.com/blfuentes/AdventOfCode_2025_Go/day08"
	"github.com/blfuentes/AdventOfCode_2025_Go/day09"
	"github.com/blfuentes/AdventOfCode_2025_Go/day10"
	"github.com/blfuentes/AdventOfCode_2025_Go/day11"
	"github.com/blfuentes/AdventOfCode_2025_Go/day12"
	"github.com/blfuentes/AdventOfCode_2025_Go/utilities"
)

func main() {
	var timer time.Time

	// Day 01
	utilities.RetrieveContent(2025, 1)
	timer = time.Now()
	fmt.Printf("Final result Day 01 part 1: %d", day01.Executepart1())
	fmt.Printf(" in %s\n", utilities.FormatDuration(time.Since(timer)))
	timer = time.Now()
	fmt.Printf("Final result Day 01 part 2: %d", day01.Executepart2())
	fmt.Printf(" in %s\n", utilities.FormatDuration(time.Since(timer)))

	// Day 02
	utilities.RetrieveContent(2025, 2)
	timer = time.Now()
	fmt.Printf("Final result Day 02 part 1: %d", day02.Executepart1())
	fmt.Printf(" in %s\n", utilities.FormatDuration(time.Since(timer)))
	timer = time.Now()
	fmt.Printf("Final result Day 02 part 2: %d", day02.Executepart2())
	fmt.Printf(" in %s\n", utilities.FormatDuration(time.Since(timer)))

	// Day 03
	utilities.RetrieveContent(2025, 3)
	timer = time.Now()
	fmt.Printf("Final result Day 03 part 1: %d", day03.Executepart1())
	fmt.Printf(" in %s\n", utilities.FormatDuration(time.Since(timer)))
	timer = time.Now()
	fmt.Printf("Final result Day 03 part 2: %d", day03.Executepart2())
	fmt.Printf(" in %s\n", utilities.FormatDuration(time.Since(timer)))

	// Day 04
	utilities.RetrieveContent(2025, 4)
	timer = time.Now()
	fmt.Printf("Final result Day 04 part 1: %d", day04.Executepart1())
	fmt.Printf(" in %s\n", utilities.FormatDuration(time.Since(timer)))
	timer = time.Now()
	fmt.Printf("Final result Day 04 part 2: %d", day04.Executepart2())
	fmt.Printf(" in %s\n", utilities.FormatDuration(time.Since(timer)))

	// Day 05
	utilities.RetrieveContent(2025, 5)
	timer = time.Now()
	fmt.Printf("Final result Day 05 part 1: %d", day05.Executepart1())
	fmt.Printf(" in %s\n", utilities.FormatDuration(time.Since(timer)))
	timer = time.Now()
	fmt.Printf("Final result Day 05 part 2: %d", day05.Executepart2())
	fmt.Printf(" in %s\n", utilities.FormatDuration(time.Since(timer)))

	// Day 06
	utilities.RetrieveContent(2025, 6)
	timer = time.Now()
	fmt.Printf("Final result Day 06 part 1: %d", day06.Executepart1())
	fmt.Printf(" in %s\n", utilities.FormatDuration(time.Since(timer)))
	timer = time.Now()
	fmt.Printf("Final result Day 06 part 2: %d", day06.Executepart2())
	fmt.Printf(" in %s\n", utilities.FormatDuration(time.Since(timer)))

	// Day 07
	utilities.RetrieveContent(2025, 7)
	timer = time.Now()
	fmt.Printf("Final result Day 07 part 1: %d", day07.Executepart1())
	fmt.Printf(" in %s\n", utilities.FormatDuration(time.Since(timer)))
	timer = time.Now()
	fmt.Printf("Final result Day 07 part 2: %d", day07.Executepart2())
	fmt.Printf(" in %s\n", utilities.FormatDuration(time.Since(timer)))

	// Day 08
	utilities.RetrieveContent(2025, 8)
	timer = time.Now()
	fmt.Printf("Final result Day 08 part 1: %d", day08.Executepart1())
	fmt.Printf(" in %s\n", utilities.FormatDuration(time.Since(timer)))
	timer = time.Now()
	fmt.Printf("Final result Day 08 part 2: %d", day08.Executepart2())
	fmt.Printf(" in %s\n", utilities.FormatDuration(time.Since(timer)))

	// Day 09
	utilities.RetrieveContent(2025, 9)
	timer = time.Now()
	fmt.Printf("Final result Day 09 part 1: %d", day09.Executepart1())
	fmt.Printf(" in %s\n", utilities.FormatDuration(time.Since(timer)))
	timer = time.Now()
	fmt.Printf("Final result Day 09 part 2: %d", day09.Executepart2())
	fmt.Printf(" in %s\n", utilities.FormatDuration(time.Since(timer)))

	// Day 10
	utilities.RetrieveContent(2025, 10)
	timer = time.Now()
	fmt.Printf("Final result Day 10 part 1: %d", day10.Executepart1())
	fmt.Printf(" in %s\n", utilities.FormatDuration(time.Since(timer)))
	timer = time.Now()
	fmt.Printf("Final result Day 10 part 2: %d", day10.Executepart2())
	fmt.Printf(" in %s\n", utilities.FormatDuration(time.Since(timer)))

	// Day 11
	utilities.RetrieveContent(2025, 11)
	timer = time.Now()
	fmt.Printf("Final result Day 11 part 1: %d", day11.Executepart1())
	fmt.Printf(" in %s\n", utilities.FormatDuration(time.Since(timer)))
	timer = time.Now()
	fmt.Printf("Final result Day 11 part 2: %d", day11.Executepart2())
	fmt.Printf(" in %s\n", utilities.FormatDuration(time.Since(timer)))

	// Day 12
	utilities.RetrieveContent(2025, 12)
	timer = time.Now()
	fmt.Printf("Final result Day 12 part 1: %d", day12.Executepart1())
	fmt.Printf(" in %s\n", utilities.FormatDuration(time.Since(timer)))
	timer = time.Now()
	fmt.Printf("Final result Day 12 part 2: %d", day12.Executepart2())
	fmt.Printf(" in %s\n", utilities.FormatDuration(time.Since(timer)))
}
