use std::{
    fs::File,
    io::{self, BufRead},
};

enum Command {
    Forward(i32),
    Up(i32),
    Down(i32),
}

pub struct Solution {
    input: Vec<Command>,
}

impl Solution {
    pub fn new() -> Solution {
        let input_file = File::open("input/2.txt").expect("Failed to read input file");
        let file_lines = io::BufReader::new(input_file).lines();
        let result: Vec<Command> = file_lines
            .map(|line| line.expect("Failed to read a line"))
            .filter(|line| !line.is_empty())
            .map(|string| {
                let split: Vec<&str> = string.split(' ').collect();
                let command = split[0];
                let value: i32 = split[1].parse().expect("Failed to parse command value");
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
    type Output = String;

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
                    depth += aim * value
                }
                Command::Up(value) => aim -= value,
                Command::Down(value) => aim += value,
            }
        }
        (depth * position).to_string()
    }
}
