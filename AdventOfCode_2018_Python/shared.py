from pathlib import Path


def read_input_as_lines(path, day):
    path = f"./day{day:02}/{path}"
    with open(Path(__file__).parent / path, "r") as f:
        return f.readlines()


def read_input_as_line(path, day):
    path = f"./day{day:02}/{path}"
    with open(Path(__file__).parent / path, "r") as f:
        return f.read()
