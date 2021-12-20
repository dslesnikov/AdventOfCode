mod day1;
mod day10;
mod day11;
mod day12;
mod day13;
mod day14;
mod day15;
mod day16;
mod day17;
mod day18;
//mod day19;
mod day20;
mod day2;
mod day3;
mod day4;
mod day5;
mod day6;
mod day7;
mod day8;
mod day9;
mod parsing;

trait Solution: Sized {
    const DAY: i32;
    fn create() -> Self;
    fn solve_first_part(&self) -> String;
    fn solve_second_part(&self) -> String {
        String::from("Not yet")
    }
}

fn main() {
    let solution = day20::Solution::create();
    let first = solution.solve_first_part();
    println!("First part: {}", first);
    let second = solution.solve_second_part();
    println!("Second part: {}", second);
}
