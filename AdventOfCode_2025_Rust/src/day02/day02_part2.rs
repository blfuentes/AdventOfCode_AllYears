use crate::utils::read_day_input;

pub fn solve() -> i64 {
    solve_with_input(None)
}

#[inline]
fn get_digit_at(num: i64, pos: usize, total_digits: u32) -> i64 {
    (num / 10_i64.pow(total_digits - pos as u32 - 1)) % 10
}

#[inline]
fn count_digits(mut num: i64) -> u32 {
    let mut count = 0;
    while num > 0 {
        num /= 10;
        count += 1;
    }
    count
}

fn is_not_valid(num: i64) -> bool {
    let digit_count = count_digits(num);
    
    for pattern_len in 1..=(digit_count / 2) {
        if digit_count % pattern_len != 0 {
            continue;
        }
        
        let mut is_pattern = true;
        'outer: for offset in (pattern_len..digit_count).step_by(pattern_len as usize) {
            for i in 0..pattern_len {
                if get_digit_at(num, i as usize, digit_count) != get_digit_at(num, (offset + i) as usize, digit_count) {
                    is_pattern = false;
                    break 'outer;
                }
            }
        }
        
        if is_pattern {
            return true;
        }
    }
    false
}

pub fn solve_with_input(file_path: Option<&str>) -> i64 {
    let input = read_day_input(file_path.unwrap_or("day02/day02_input.txt"))
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
        let result = solve_with_input(Some("day02/test_input_02.txt"));
        assert_eq!(result, 4174379265);
    }

    #[test]
    fn day02_part_2_real_input() {
        let result = solve_with_input(None);
        assert_eq!(result, 49046150754);
    }
}