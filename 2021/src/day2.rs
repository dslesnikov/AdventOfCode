use create_macro_derive::CreateFromLines;

use crate::parsing::FromLines;

enum Command {
    Forward(i32),
    Up(i32),
    Down(i32),
}

#[derive(CreateFromLines)]
pub struct Solution {
    input: Vec<Command>,
}

impl FromLines for Solution {
    fn new(lines: Vec<String>) -> Self {
        let result: Vec<Command> = lines
            .into_iter()
            .filter(|string| !string.is_empty())
            .map(|string| {
                let split: Vec<&str> = string.split(' ').collect();
                let command = split[0];
                let value: i32 = split[1].parse().unwrap();
                match command {
                    "forward" => Command::Forward(value),
                    "down" => Command::Down(value),
                    "up" => Command::Up(value),
                    &_ => panic!("Failed to parse command"),
                }
            })
            .collect();
        Solution { input: result }
    }
}

impl crate::Solution for Solution {
    const DAY: i32 = 2;

    fn solve_first_part(&self) -> String {
        let mut depth = 0;
        let mut position = 0;
        for command in self.input.iter() {
            match &command {
                Command::Forward(value) => position += value,
                Command::Up(value) => depth -= value,
                Command::Down(value) => depth += value,
            }
        }
        (depth * position).to_string()
    }

    fn solve_second_part(&self) -> String {
        let mut depth = 0;
        let mut position = 0;
        let mut aim = 0;
        for command in self.input.iter() {
            match &command {
                Command::Forward(value) => {
                    position += value;
                    depth += aim * value;
                }
                Command::Up(value) => aim -= value,
                Command::Down(value) => aim += value,
            }
        }
        (depth * position).to_string()
    }
}
