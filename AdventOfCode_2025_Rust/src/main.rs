use std::time::Instant;
use std::fmt::Display;

pub mod utils;

pub mod day01;
pub mod day02;
pub mod day03;
pub mod day04;
pub mod day05;
pub mod day06;
pub mod day07;
pub mod day08;
pub mod day09;
pub mod day10;
pub mod day11;
pub mod day12;

fn run_solution<F, T>(day: u8, part: u8, solve_fn: F)
where
    F: FnOnce() -> T,
    T: Display,
{
    let start = Instant::now();
    let result = solve_fn();
    let duration = start.elapsed();

    let total_micros = duration.as_micros();
    let minutes = total_micros / 60_000_000;
    let seconds = (total_micros % 60_000_000) / 1_000_000;
    let millis = (total_micros % 1_000_000) / 1_000;
    let micros = total_micros % 1_000;

    println!(
        "Final result Day {:02} part {}: {} in {:02}:{:02}:{:03}:{:03}",
        day, part, result, minutes, seconds, millis, micros
    );
}

fn main() {
    // Day 01
    run_solution(1, 1, day01::day01_part1::solve);
    run_solution(1, 2, day01::day01_part2::solve);
    // Day 02
    run_solution(2, 1, day02::day02_part1::solve);
    run_solution(2, 2, day02::day02_part2::solve);
    // Day 03
    run_solution(3, 1, day03::day03_part1::solve);
    run_solution(3, 2, day03::day03_part2::solve);
    // // Day 04
    // run_solution(4, 1, day04::day04_part1::solve);
    // run_solution(4, 2, day04::day04_part2::solve);
    // // Day 05
    // run_solution(5, 1, day05::day05_part1::solve);
    // run_solution(5, 2, day05::day05_part2::solve);
    // // Day 06
    // run_solution(6, 1, day06::day06_part1::solve);
    // run_solution(6, 2, day06::day06_part2::solve);
    // // Day 07
    // run_solution(7, 1, day07::day07_part1::solve);
    // run_solution(7, 2, day07::day07_part2::solve);
    // // Day 08
    // run_solution(8, 1, day08::day08_part1::solve);
    // run_solution(8, 2, day08::day08_part2::solve);
    // // Day 09
    // run_solution(9, 1, day09::day09_part1::solve);
    // run_solution(9, 2, day09::day09_part2::solve);
    // // Day 10
    // run_solution(10, 1, day10::day10_part1::solve);
    // run_solution(10, 2, day10::day10_part2::solve);
    // // Day 11
    // run_solution(11, 1, day11::day11_part1::solve);
    // run_solution(11, 2, day11::day11_part2::solve);
    // // Day 12
    // run_solution(12, 1, day12::day12_part1::solve);
    // run_solution(12, 2, day12::day12_part2::solve);
}
