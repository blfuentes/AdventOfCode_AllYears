module CustomDataTypes

// Day 12
type CaveType = BIG | SMALL | START | END
type Cave = { Name:string; Size: CaveType; Connections: Cave list }
