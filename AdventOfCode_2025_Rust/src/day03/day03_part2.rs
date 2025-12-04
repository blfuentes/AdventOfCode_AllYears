use crate::utils::{
    read_day_input,
    split_nonempty_lines
};

pub fn solve() -> i64 {
    solve_with_input(None)
}

fn find_joltage(bank: &[i64]) -> i64 {
    fn find_max_values(bank: &[i64], i: i32, init: usize, mut acc: Vec<i64>) -> Vec<i64> {
        if i < 0 {
            return acc;
        }
        
        let window_end = bank.len() - i as usize;
        let slice = &bank[init..window_end];
        let max_val = *slice
            .iter()
            .max()
            .unwrap();
        let idx = bank[init..]
            .iter()
            .position(|&x| x == max_val)
            .unwrap();
        let new_init = init + idx + 1;
        
        acc.insert(0, max_val);
        
        find_max_values(bank, i - 1, new_init, acc)
    }
    
    let values = find_max_values(bank, 11, 0, Vec::new());
    let values: Vec<i64> = values
        .into_iter()
        .rev()
        .collect();
    
    values.iter().fold(0i64, |acc, &digit| acc * 10 + digit)
}

pub fn solve_with_input(file_path: Option<&str>) -> i64 {
    let default_path = "day03/day03_input";
    let file_path = file_path.unwrap_or(default_path);
    let input = read_day_input(file_path).expect("failed to read input");
    let lines = split_nonempty_lines(&input);
    
    lines
        .iter()
        .map(|line| {
            let bank: Vec<i64> = line
                .chars()
                .filter_map(|c| c.to_digit(10))
                .map(|d| d as i64)
                .collect();
            find_joltage(&bank)
        })
        .sum()
}

#[cfg(test)]
mod tests {
    use super::*;
    #[test]
    fn day03_part_2_matches() {
        let result = solve_with_input(Some("day03/test_input_03"));
        assert_eq!(result, 3121910778619);
    }

    #[test]
    fn day03_part_2_real_input() {
        let result = solve_with_input(None);
        assert_eq!(result, 173577199527257);
    }
}