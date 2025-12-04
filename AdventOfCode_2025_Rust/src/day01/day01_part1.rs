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

pub fn parse_content(lines: Vec<&str>) -> impl Iterator<Item = Rotation> + '_ {
    lines.into_iter().map(|line| {
        let (dir, num_str) = line.split_at(1);
        let num: i32 = num_str.parse().expect("failed to parse number");
        match dir {
            "L" => Rotation::Left(num),
            "R" => Rotation::Right(num),
            _ => panic!("invalid direction")
        }
    })
}

pub fn count_zeroes(rotations: impl Iterator<Item = Rotation>) -> i32 {
    let mut pos = 50;
    let mut count = 0;
    
    for rotation in rotations {
        pos = match rotation {
            Rotation::Left(val) => safe_mod(pos - val, 100),
            Rotation::Right(val) => safe_mod(pos + val, 100),
        };
        if pos == 0 {
            count += 1;
        }
    }
    
    count
}

pub fn solve_with_input(file_path: Option<&str>) -> i32 {
    let default_path = "day01/day01_input.txt";
    let file_path = file_path.unwrap_or(default_path);
    let input = read_day_input(file_path).expect("failed to read input");
    let lines = split_nonempty_lines(&input);
    count_zeroes(parse_content(lines))
}

#[cfg(test)]
mod tests {
    use super::*;
    #[test]
    fn day01_part_1_matches() {
        let result = solve_with_input(Some("day01/test_input_01.txt"));
        assert_eq!(result, 3);
    }

    #[test]
    fn day01_part_1_real_input() {
        let result = solve_with_input(None);
        assert_eq!(result, 1026);
    }
}