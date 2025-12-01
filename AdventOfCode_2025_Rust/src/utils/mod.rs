use std::fs;
use std::path::PathBuf;

/// Build absolute path anchored at project root (CARGO_MANIFEST_DIR).
/// Example: file_path = "day01/test_input_01" -> <root>/day01/test_input_01.txt
pub fn input_path(file_path: &str) -> PathBuf {
    let root = PathBuf::from(env!("CARGO_MANIFEST_DIR"));
    root.join(format!("src/{file_path}.txt"))
}

pub fn read_day_input(file_path: &str) -> std::io::Result<String> {
    let file_path = input_path(file_path);
    fs::read_to_string(file_path)
}

pub fn split_nonempty_lines(s: &str) -> Vec<&str> {
    s.lines().filter(|l| !l.trim().is_empty()).collect()
}

pub fn parse_i32s_whitespace(s: &str) -> Vec<i32> {
    s.split_whitespace().filter_map(|t| t.parse::<i32>().ok()).collect()
}

pub fn parse_i32s_symbol(s: &str, sep: char) -> Vec<i32> {
    s.split(sep).filter_map(|t| t.parse::<i32>().ok()).collect()
}

pub fn parse_i64s_whitespace(s: &str) -> Vec<i64> {
    s.split_whitespace().filter_map(|t| t.parse::<i64>().ok()).collect()
}

pub fn parse_i64s_symbol(s: &str, sep: char) -> Vec<i64> {
    s.split(sep).filter_map(|t| t.parse::<i64>().ok()).collect()
}

// Helper function for modulo that handles negative numbers correctly
pub fn safe_mod(a: i32, b: i32) -> i32 {
    ((a % b) + b) % b
}

// Helper function for floodDiv
pub fn floor_div(a: i32, b: i32) -> i32 {
    (a as f64 / b as f64).floor() as i32
}

#[cfg(test)]
mod tests {
    use super::*;
    #[test]
    fn test_input_path() {
        let p = input_path("day01/test_input_01");
        assert!(p.ends_with("day01/test_input_01.txt"));
    }
    #[test]
    fn test_split_nonempty_lines() {
        let v = split_nonempty_lines("a\n\nb\n");
        assert_eq!(v, vec!["a", "b"]);
    }

    #[test]
    fn test_safe_mod() {
        assert_eq!(safe_mod(5, 3), 2);
        assert_eq!(safe_mod(-1, 3), 2);
        assert_eq!(safe_mod(-4, 3), 2);
    }

    #[test]
    fn test_floor_div() {
        assert_eq!(floor_div(5, 3), 1);
        assert_eq!(floor_div(-1, 3), -1);
        assert_eq!(floor_div(-4, 3), -2);
    }
}