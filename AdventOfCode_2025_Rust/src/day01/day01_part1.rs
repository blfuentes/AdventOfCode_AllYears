use crate::{
    day01::Rotation, 
    utils::{
        read_day_input, 
        safe_mod,
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

pub fn count_zeroes(rotations: &Vec<Rotation>) -> i32 {
    let mut count = 0;
    let f = |acc: (i32,i32), rotation: &Rotation| {
        let (pos, _) = acc;
        let new_pos =
            match rotation {
                Rotation::Left(val) => safe_mod(pos - val, 100),
                Rotation::Right(val) => safe_mod(pos + val, 100),
            };
        if new_pos == 0 {
            count += 1;
        }
        (new_pos, count)
    };
    rotations.iter().fold((50, 0), f);
    count
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
    fn day01_part_1_matches() {
        let result = solve_with_input(Some("day01/test_input_01"));
        assert_eq!(result, 3);
    }

    #[test]
    fn day01_part_1_real_input() {
        let result = solve_with_input(None);
        assert_eq!(result, 1026);
    }
}