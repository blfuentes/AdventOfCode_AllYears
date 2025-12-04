use crate::utils::read_day_input;

pub fn solve() -> i64 {
    solve_with_input(None)
}

fn is_not_valid(num: i64) -> bool {
    let digits = num.to_string();
    if digits.len() % 2 != 0 {
        return false;
    }
    let half_len = digits.len() / 2;
    digits[..half_len] == digits[half_len..]
}

pub fn solve_with_input(file_path: Option<&str>) -> i64 {
    let input = read_day_input(file_path.unwrap_or("day02/day02_input"))
        .expect("failed to read input");
    
    input
        .split(',')
        .filter_map(|part| {
            let mut range = part.split('-');
            let start = range.next()?.trim().parse::<i64>().ok()?;
            let finish = range.next()?.trim().parse::<i64>().ok()?;
            Some((start, finish))
        })
        .flat_map(|(start, finish)| start..=finish)
        .filter(|&num| is_not_valid(num))
        .sum()
}

#[cfg(test)]
mod tests {
    use super::*;
    
    #[test]
    fn day02_part_1_matches() {
        let result = solve_with_input(Some("day02/test_input_02"));
        assert_eq!(result, 1227775554);
    }

    #[test]
    fn day02_part_1_real_input() {
        let result = solve_with_input(None);
        assert_eq!(result, 38437576669);
    }
}