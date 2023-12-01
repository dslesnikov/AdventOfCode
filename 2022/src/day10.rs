use std::str::FromStr;

use crate::FromLines;

enum Instruction {
    Nop,
    Add(i32),
}

impl FromStr for Instruction {
    type Err = ();

    fn from_str(s: &str) -> Result<Self, Self::Err> {
        if s == "noop" {
            return Ok(Instruction::Nop);
        }
        let value = s.split(' ').last().unwrap().parse().unwrap();
        Ok(Instruction::Add(value))
    }
}

pub(crate) struct Solution {
    instructions: Vec<Instruction>,
}

impl FromLines for Solution {
    fn from_lines<Iter: Iterator<Item = String>>(lines: Iter) -> Self {
        let instructions = lines.map(|line| line.parse().unwrap()).collect();
        Solution { instructions }
    }
}

impl crate::Solution for Solution {
    const DAY: u32 = 10;

    fn solve_first_part(&self) -> String {
        let mut x = 1;
        let cycles = 221;
        let mut iterator = self.instructions.iter();
        let mut current = iterator.next();
        let mut executed = false;
        let interesting_cycles = [20, 60, 100, 140, 180, 220];
        let mut total_strength = 0;
        for cycle_counter in 1..=cycles {
            if interesting_cycles.contains(&cycle_counter) {
                total_strength += cycle_counter * x;
            }
            match current {
                None => panic!(),
                Some(Instruction::Nop) => {
                    current = iterator.next();
                }
                Some(Instruction::Add(value)) => {
                    if executed {
                        x += value;
                        current = iterator.next();
                    }
                    executed = !executed;
                }
            }
        }
        total_strength.to_string()
    }

    fn solve_second_part(&self) -> String {
        let mut x = 1;
        let cycles = 240;
        let mut iterator = self.instructions.iter();
        let mut current = iterator.next();
        let mut executed = false;
        let mut screen = String::with_capacity(246);
        for cycle_counter in 1..=cycles {
            let drawn: i32 = cycle_counter % 40 - 1;
            if (x - drawn).abs() <= 1 {
                screen.push('#');
            } else {
                screen.push('.');
            }
            match current {
                None => panic!(),
                Some(Instruction::Nop) => {
                    current = iterator.next();
                }
                Some(Instruction::Add(value)) => {
                    if executed {
                        x += value;
                        current = iterator.next();
                    }
                    executed = !executed;
                }
            }
            if cycle_counter % 40 == 0 {
                screen.push('\n');
            }
        }
        screen
    }
}
