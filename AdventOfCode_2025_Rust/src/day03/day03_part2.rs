use crate::utils::{
    read_day_input,
    split_nonempty_lines
};

pub fn solve() -> i64 {
    solve_with_input(None)
}

#[inline]
fn find_joltage(line: &str) -> i64 {
    let bytes = line.as_bytes();
    let len = bytes.len();
    
    let mut result = 0i64;
    let mut init = 0;
    
    for i in (0..12).rev() {
        let window_end = len - i;
        
        let mut max_val = 0;
        let mut max_idx = 0;
        
        for (idx, &byte) in bytes[init..window_end].iter().enumerate() {
            let digit = (byte - b'0') as i64;
            if digit > max_val {
                max_val = digit;
                max_idx = idx;
            }
        }
        
        init += max_idx + 1;
        result = result * 10 + max_val;
    }
    
    result
}

pub fn solve_with_input(file_path: Option<&str>) -> i64 {
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
    fn day03_part_2_matches() {
        let result = solve_with_input(Some("day03/test_input_03.txt"));
        assert_eq!(result, 3121910778619);
    }

    #[test]
    fn day03_part_2_real_input() {
        let result = solve_with_input(None);
        assert_eq!(result, 173577199527257);
    }
}