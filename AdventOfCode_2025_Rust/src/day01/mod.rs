pub mod day01_part1;
pub mod day01_part2;

pub fn solve_part1() -> i32 {
    day01_part1::solve()
}

pub fn solve_part2() -> i32 {
    day01_part2::solve()
}

pub enum Rotation {
    Left(i32),
    Right(i32)
}