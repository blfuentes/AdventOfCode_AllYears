import shared


# Part 1
def solve_part1():
    # path = "test_02.txt"
    path = "day_02.txt"
    numOfTwos = 0
    numOfThrees = 0
    for line in shared.read_input_as_lines(path, 2):
        twos = False
        threes = False
        scores = {}
        for c in line:
            if c in scores:
                scores[c] += 1
            else:
                scores[c] = 1
        for count in scores.values():
            if count == 2 and not twos:
                twos = True
                numOfTwos += 1
            elif count == 3 and not threes:
                threes = True
                numOfThrees += 1

    result = numOfTwos * numOfThrees
    return result


# Part 2
def solve_part2():
    # path = "test_02.txt"
    path = "day_02.txt"
    lines = shared.read_input_as_lines(path, 2)
    for i in range(len(lines)):
        for j in range(i + 1, len(lines)):
            diff_count = 0
            for c1, c2 in zip(lines[i], lines[j]):
                if c1 != c2:
                    diff_count += 1
            if diff_count == 1:
                result = "".join(c1 for c1, c2 in zip(lines[i], lines[j]) if c1 == c2)
                return result
    return ""
