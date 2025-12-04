use crate::utils::{read_day_input, split_nonempty_lines};

pub fn solve() -> i32 {
    solve_with_input(None)
}

#[inline]
fn find_joltage(line: &str) -> i32 {
    let bytes = line.as_bytes();
    let len = bytes.len();
    
    // Find first max (max value, leftmost if tie)
    let mut first_max = 0;
    let mut first_idx = 0;
    
    for i in 0..len-1 {
        let digit = (bytes[i] - b'0') as i32;
        if digit > first_max {
            first_max = digit;
            first_idx = i;
        }
    }
    
    // Find second max after first_idx
    let mut second_max = 0;
    for i in (first_idx + 1)..len {
        let digit = (bytes[i] - b'0') as i32;
        if digit > second_max {
            second_max = digit;
        }
    }

    first_max * 10 + second_max
}

pub fn solve_with_input(file_path: Option<&str>) -> i32 {
    let default_path = "day03/day03_input.txt";
    let file_path = file_path.unwrap_or(default_path);
    let input = read_day_input(file_path).expect("failed to read input");
    let lines = split_nonempty_lines(&input);
    
    lines
        .iter()
        .map(|line| find_joltage(line))
        .sum()
}

#[cfg(test)]
mod tests {
    use super::*;
    #[test]
    fn day03_part_1_matches() {
        let result = solve_with_input(Some("day03/test_input_03.txt"));
        assert_eq!(result, 357);
    }

    #[test]
    fn day03_part_1_real_input() {
        let result = solve_with_input(None);
        assert_eq!(result, 17535);
    }
}