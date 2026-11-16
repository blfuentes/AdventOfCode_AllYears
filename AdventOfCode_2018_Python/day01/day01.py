from pathlib import Path


# Part 1
def solve_part1():
    result = 0
    with open(Path(__file__).parent / "day_01.txt", "r") as f:
        for line in f:
            result += int(line)
    return result


# Part 2
def solve_part2():
    path = Path(__file__).parent / "day_01.txt"
    # path = Path(__file__).parent / "test_01.txt"
    frequencies = {}
    result = 0
    with open(path, "r") as f:
        lines = f.readlines()
        lineIdx = 0
        while True:
            result += int(lines[lineIdx % len(lines)])
            # print(f"Current frequency: {result}")
            if result in frequencies:
                return result
            frequencies[result] = True
            lineIdx += 1
