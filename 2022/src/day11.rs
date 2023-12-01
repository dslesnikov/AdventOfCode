use std::{collections::HashMap, str::FromStr};

use crate::FromContent;

#[derive(Clone)]
enum Token {
    Value,
    Constant(u64),
}

impl Token {
    fn calculate(&self, value: u64) -> u64 {
        match self {
            Token::Value => value,
            Token::Constant(constant) => *constant,
        }
    }
}

#[derive(Clone)]
enum Operation {
    Addition,
    Multiplication,
}

#[derive(Clone)]
struct Modifier {
    operation: Operation,
    left: Token,
    right: Token,
}

impl FromStr for Modifier {
    type Err = ();

    fn from_str(s: &str) -> Result<Self, Self::Err> {
        let operation = s.split('=').last().unwrap().trim();
        let tokens: Vec<&str> = operation.split(' ').collect();
        let left = match tokens[0] {
            "old" => Token::Value,
            value => Token::Constant(value.parse().unwrap()),
        };
        let right = match tokens[2] {
            "old" => Token::Value,
            value => Token::Constant(value.parse().unwrap()),
        };
        let operation = match tokens[1] {
            "*" => Operation::Multiplication,
            "+" => Operation::Addition,
            _ => panic!("Unknown operation"),
        };
        Ok(Self {
            operation,
            left,
            right,
        })
    }
}

impl Modifier {
    fn invoke(&self, value: u64) -> u64 {
        let left_value = self.left.calculate(value);
        let right_value = self.right.calculate(value);
        match self.operation {
            Operation::Addition => left_value + right_value,
            Operation::Multiplication => left_value * right_value,
        }
    }
}

#[derive(Clone)]
struct MonkeyBehaviour {
    modifier: Modifier,
    test_divider: u64,
    positive_monkey: usize,
    negative_monkey: usize,
}

#[derive(Clone)]
struct Monkey {
    behaviour: MonkeyBehaviour,
    items: Vec<u64>,
}

pub(crate) struct Solution {
    monkeys: Vec<Monkey>,
}

impl FromContent for Solution {
    fn from_content(content: String) -> Self {
        let monkeys: Vec<Monkey> = content
            .split("\n\n")
            .map(|chunk| {
                let lines: Vec<&str> = chunk.split('\n').collect();
                let starting_items = lines[1]
                    .split(':')
                    .last()
                    .unwrap()
                    .trim()
                    .split(", ")
                    .map(|digits| digits.parse().unwrap())
                    .collect();
                let operation: Modifier = lines[2].parse().unwrap();
                let test = lines[3].split(' ').last().unwrap().parse().unwrap();
                let positive = lines[4].chars().last().unwrap().to_digit(10).unwrap();
                let negative = lines[5].chars().last().unwrap().to_digit(10).unwrap();
                let behaviour = MonkeyBehaviour {
                    modifier: operation,
                    test_divider: test,
                    positive_monkey: positive as usize,
                    negative_monkey: negative as usize,
                };
                Monkey {
                    behaviour,
                    items: starting_items,
                }
            })
            .collect();
        Self { monkeys }
    }
}

impl crate::Solution for Solution {
    const DAY: u32 = 11;

    fn solve_first_part(&self) -> String {
        let monkey_business = self.simulate(20, 3);
        return monkey_business.to_string();
    }

    fn solve_second_part(&self) -> String {
        let monkey_business = self.simulate(10000, 1);
        return monkey_business.to_string();
    }
}

impl Solution {
    fn simulate(&self, rounds: u32, worry_management: u64) -> u64 {
        let mut monkeys = self.monkeys.clone();
        let mut inspection_counter: HashMap<usize, u64> = HashMap::new();
        let divider = monkeys
            .iter()
            .map(|m| m.behaviour.test_divider)
            .fold(1u64, |acc, item| acc * item);
        for _ in 0..rounds {
            for index in 0..monkeys.len() {
                let monkey = &mut monkeys[index];
                let mut new_values: Vec<(usize, u64)> = Vec::new();
                for item in monkey.items.iter() {
                    let new_value = monkey.behaviour.modifier.invoke(*item) / worry_management;
                    let target = if new_value % monkey.behaviour.test_divider == 0 {
                        monkey.behaviour.positive_monkey
                    } else {
                        monkey.behaviour.negative_monkey
                    };
                    new_values.push((target, new_value));
                }
                let length = monkey.items.len() as u64;
                inspection_counter
                    .entry(index)
                    .and_modify(|val| {
                        *val += length;
                    })
                    .or_insert(length);
                monkey.items.clear();
                for (target, value) in new_values {
                    monkeys[target].items.push(value % divider);
                }
            }
        }
        let mut values: Vec<u64> = inspection_counter.values().cloned().collect();
        values.sort();
        let result = values
            .iter()
            .rev()
            .take(2)
            .fold(1u64, |acc, item| acc * item);
        return result;
    }
}
