use std::cmp::Reverse;

use crate::utils::{read_day_input, split_nonempty_lines};

pub fn solve() -> i32 {
    solve_with_input(None)
}

fn find_joltage(bank: &[i32]) -> i32 {
    let (first_idx, first_max) = bank[..bank.len()-1]
        .iter()
        .enumerate()
        .max_by_key(|&(i, &val)| (val, Reverse(i)))
        .map(|(idx, &val)| (idx, val))
        .unwrap();
    
    let second_max = bank[first_idx+1..]
        .iter()
        .max()
        .unwrap();

    first_max * 10 + second_max
}

pub fn solve_with_input(file_path: Option<&str>) -> i32 {
    let default_path = "day03/day03_input";
    let file_path = file_path.unwrap_or(default_path);
    let input = read_day_input(file_path).expect("failed to read input");
    let lines = split_nonempty_lines(&input);
    
    lines
        .iter()
        .map(|line| {
            let bank: Vec<i32> = line
                .chars()
                .filter_map(|c| c.to_digit(10))
                .map(|d| d as i32)
                .collect();
            find_joltage(&bank)
        })
        .sum()
}

#[cfg(test)]
mod tests {
    use super::*;
    #[test]
    fn day03_part_1_matches() {
        let result = solve_with_input(Some("day03/test_input_03"));
        assert_eq!(result, 357);
    }

    #[test]
    fn day03_part_1_real_input() {
        let result = solve_with_input(None);
        assert_eq!(result, 17535);
    }
}