use crate::utils::{read_day_input, split_nonempty_lines};

pub fn solve() -> i32 {
    solve_with_input(None)
}

pub fn solve_with_input(file_path: Option<&str>) -> i32 {
    let default_path = "day11/day11_input";
    let file_path = file_path.unwrap_or(default_path);
    let input = read_day_input(file_path).expect("failed to read input");
    let lines = split_nonempty_lines(&input);
    lines.len() as i32
}

#[cfg(test)]
mod tests {
    use super::*;
    #[test]
    fn dat11_part_2_matches() {
        let result = solve_with_input(Some("day11/test_input_02"));
        assert_eq!(result, 0);
    }
}