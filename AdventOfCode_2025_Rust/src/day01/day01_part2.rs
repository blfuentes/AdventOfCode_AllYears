use crate::{
    day01::Rotation, 
    utils::{
        read_day_input,
        floor_div,
        split_nonempty_lines}
};

pub fn solve() -> i32 {
    solve_with_input(None)
}

pub fn parse_content(lines: Vec<&str>) -> Vec<Rotation> {
    lines.iter().map(|line| {
        let (dir, num_str) = line.split_at(1);
        let num: i32 = num_str.parse().expect("failed to parse number");
        match dir {
            "L" => Rotation::Left(num),
            "R" => Rotation::Right(num),
            _ => panic!("invalid direction")
        }
    }).collect()
}

pub fn zero_crossings(start: i32, end: i32) -> i32 {
    if start <= end {
        floor_div(end, 100) - floor_div(start, 100)
    } else {
        zero_crossings(end - 1, start - 1)
    }
}

pub fn count_zeroes(rotations: &Vec<Rotation>) -> i32 {
    let f = |acc: (i32,i32), rotation: &Rotation| {
        let (sum, count) = acc;
        let new_sum =
            match rotation {
                Rotation::Left(val) => sum - val,
                Rotation::Right(val) => sum + val,
            };
        let crossings = zero_crossings(sum, new_sum);
        (new_sum, count + crossings)
    };
    rotations.iter().fold((50, 0), f).1
}

pub fn solve_with_input(file_path: Option<&str>) -> i32 {
    let default_path = "day01/day01_input";
    let file_path = file_path.unwrap_or(default_path);
    let input = read_day_input(file_path).expect("failed to read input");
    let lines = split_nonempty_lines(&input);
    let rotations = parse_content(lines);
    let result = count_zeroes(&rotations);
    result
}

#[cfg(test)]
mod tests {
    use super::*;
    #[test]
    fn day01_part_2_matches() {
        let result = solve_with_input(Some("day01/test_input_01"));
        assert_eq!(result, 6);
    }

    #[test]
    fn day01_part_2_real_input() {
        let result = solve_with_input(None);
        assert_eq!(result, 5923);
    }
}