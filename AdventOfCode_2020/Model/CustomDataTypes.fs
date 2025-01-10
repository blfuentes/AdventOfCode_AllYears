module CustomDataTypes

// Day 02
type PasswordPolicy = {min: int; max: int; element: string; code: string }
type PasswordPolicyOption = PasswordPolicy option

// Day 04
type HeightType = { height: int; unittype: string }
type HeightTypeOption = HeightType option

// Day 07
type ChristmasBag =  { Name: string; Size: int; Content: ChristmasBag list }
type ChristmasBagOption = ChristmasBag option

// Day 08
type HandheldOpType = ACC | JMP | NOP | MISSING
type HandledOperation = { Op: HandheldOpType; Offset: int; }

// Day 11
type SeatStatus = FLOOR | EMPTY | OCCUPIED
type SeatInfo = { Seat: SeatStatus; Location: (int * int); Content: char }

// Day 12
type MovementType = NORTH | SOUTH | EAST | WEST | LEFT | RIGHT | FORWARD
type MovementOperation = { Mov: MovementType; Offset: int }