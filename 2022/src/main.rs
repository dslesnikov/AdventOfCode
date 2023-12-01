use std::io::{BufRead, Read};

mod day1;
mod day10;
mod day11;
mod day12;
mod day13;
mod day14;
mod day15;
mod day2;
mod day3;
mod day4;
mod day5;
mod day6;
mod day7;
mod day8;
mod day9;

fn main() {
    let solution = day15::Solution::new();
    let first = solution.solve_first_part();
    println!("{}", first);
    let second = solution.solve_second_part();
    println!("{}", second);
}

pub(crate) trait Solution: Sized {
    const DAY: u32;
    fn solve_first_part(&self) -> String;
    fn solve_second_part(&self) -> String;
}

trait LinesSolution: Solution + FromLines {
    fn new() -> Self;
}

impl<T: FromLines + Solution> LinesSolution for T {
    fn new() -> Self {
        InputParser::from_lines()
    }
}

trait ContentSolution: Solution + FromContent {
    fn new() -> Self;
}

impl<T: FromContent + Solution> ContentSolution for T {
    fn new() -> Self {
        InputParser::from_content()
    }
}

pub(crate) trait FromLines {
    fn from_lines<Iter: Iterator<Item = String>>(lines: Iter) -> Self;
}

pub(crate) trait FromContent {
    fn from_content(content: String) -> Self;
}

struct InputParser;

impl InputParser {
    fn from_lines<S: Solution + FromLines>() -> S {
        let filename = format!("input/{}.txt", S::DAY);
        let file = std::fs::File::open(filename).unwrap();
        let reader = std::io::BufReader::new(file);
        let lines = reader.lines().map(|line| line.unwrap());
        S::from_lines(lines)
    }

    fn from_content<S: Solution + FromContent>() -> S {
        let filename = format!("input/{}.txt", S::DAY);
        let mut file = std::fs::File::open(filename).unwrap();
        let mut result = String::new();
        let _ = file.read_to_string(&mut result).unwrap();
        S::from_content(result)
    }
}
