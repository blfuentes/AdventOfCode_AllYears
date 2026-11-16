import shared


# Part 1
def solve_part1():
    result = 0
    for line in shared.read_input_as_lines("day_01.txt", 1):
        result += int(line)
    return result


# Part 2
def solve_part2():
    path = "day_01.txt"
    # path = "test_01.txt"
    frequencies = {}
    result = 0
    lines = shared.read_input_as_lines(path, 1)
    lineIdx = 0
    while True:
        result += int(lines[lineIdx % len(lines)])
        # print(f"Current frequency: {result}")
        if result in frequencies:
            return result
        frequencies[result] = True
        lineIdx += 1
