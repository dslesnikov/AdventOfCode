#![allow(dead_code)]

#[allow(unused_imports)]
use crate::parsing::{CreateFromContent, CreateFromLines};

mod day1;
mod day2;
mod day3;
mod day4;
mod day5;
mod day6;
mod day7;
mod parsing;

trait Solution {
    const DAY: i32;

    fn solve_first_part(&self) -> String;

    fn solve_second_part(&self) -> String {
        String::from("Not yet")
    }
}

fn main() {
    let solution = day7::Solution::create();
    let first = solution.solve_first_part();
    println!("First part: {}", first);
    let second = solution.solve_second_part();
    println!("Second part: {}", second);
}
