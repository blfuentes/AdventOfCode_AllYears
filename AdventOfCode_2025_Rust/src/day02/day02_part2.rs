use crate::utils::read_day_input;

pub fn solve() -> i64 {
    solve_with_input(None)
}

fn is_not_valid(num: i64) -> bool {
    let digits = num.to_string();
    let length = digits.len();
    let bytes = digits.as_bytes();

    for pattern_len in 1..=(length / 2) {
        if length % pattern_len != 0 {
            continue;
        }
        
        let is_pattern = (pattern_len..length)
            .step_by(pattern_len)
            .all(|offset| {
                (0..pattern_len).all(|i| bytes[i] == bytes[offset + i])
            });
        
        if is_pattern {
            return true;
        }
    }
    false
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
    fn day02_part_2_matches() {
        let result = solve_with_input(Some("day02/test_input_02"));
        assert_eq!(result, 4174379265);
    }

    #[test]
    fn day02_part_2_real_input() {
        let result = solve_with_input(None);
        assert_eq!(result, 49046150754);
    }
}