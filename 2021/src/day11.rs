use std::collections::{HashSet, VecDeque};

use crate::parsing::{FromLines, InputParser};

pub struct Solution {
    field: Vec<Vec<u8>>,
}

#[derive(PartialEq, Eq, Clone, Copy, Hash)]
struct Point {
    x: usize,
    y: usize,
}

enum SimulationResult {
    TotalFlashes(i32),
    FirstSimultaneousStep(i32),
}

impl FromLines for Solution {
    fn new(lines: Vec<String>) -> Self {
        let field: Vec<Vec<u8>> = lines
            .into_iter()
            .filter(|string| !string.is_empty())
            .map(|line| {
                line.chars()
                    .map(|char| char.to_digit(10).unwrap() as u8)
                    .collect()
            })
            .collect();
        Solution { field }
    }
}

impl crate::Solution for Solution {
    const DAY: i32 = 11;

    fn create() -> Self {
        InputParser::from_lines()
    }

    fn solve_first_part(&self) -> String {
        const STEPS: i32 = 100;
        let result = self.simulate_flashes(Some(STEPS));
        match result {
            SimulationResult::FirstSimultaneousStep(_) => panic!("Did not expect this result"),
            SimulationResult::TotalFlashes(value) => value.to_string(),
        }
    }

    fn solve_second_part(&self) -> String {
        let result = self.simulate_flashes(None);
        match result {
            SimulationResult::FirstSimultaneousStep(value) => value.to_string(),
            SimulationResult::TotalFlashes(_) => panic!("Did not expect this result"),
        }
    }
}

impl Solution {
    fn simulate_flashes(&self, steps: Option<i32>) -> SimulationResult {
        let mut field = self.field.clone();
        let height = field.len();
        let width = field[0].len();
        let total_steps = steps.unwrap_or(i32::MAX);
        let mut total_flashes: i32 = 0;
        for step in 1..=total_steps {
            let mut flashes: VecDeque<Point> = VecDeque::new();
            let mut flashed: HashSet<Point> = HashSet::new();
            for row in 0..height {
                for col in 0..width {
                    let point = Point { x: col, y: row };
                    visit(&mut field, point, &mut flashes, &mut flashed);
                }
            }
            while flashes.len() > 0 {
                let flash = flashes.pop_front().unwrap();
                let neighbours = self.get_neighbours(&flash);
                let neighbours: Vec<Point> = neighbours
                    .into_iter()
                    .filter(|point| !flashed.contains(point))
                    .collect();
                for neighbour in neighbours {
                    visit(&mut field, neighbour, &mut flashes, &mut flashed);
                }
            }
            if flashed.len() == width * height {
                return SimulationResult::FirstSimultaneousStep(step);
            }
            total_flashes += flashed.len() as i32;
        }
        SimulationResult::TotalFlashes(total_flashes)
    }

    fn get_neighbours(&self, point: &Point) -> Vec<Point> {
        let height = self.field.len() as i32;
        let width = self.field[0].len() as i32;
        let result = (-1..=1)
            .flat_map(|x| (-1..=1).map(move |y| (x, y)))
            .filter(|&pair| pair.0 != 0 || pair.1 != 0)
            .map(|pair| (pair.0 + point.x as i32, pair.1 + point.y as i32))
            .filter(|&pair| pair.0 >= 0 && pair.1 >= 0 && pair.0 < width && pair.1 < height)
            .map(|pair| Point {
                x: pair.0 as usize,
                y: pair.1 as usize,
            })
            .collect();
        result
    }
}

fn visit(
    field: &mut Vec<Vec<u8>>,
    point: Point,
    flashes: &mut VecDeque<Point>,
    flashed: &mut HashSet<Point>,
) {
    field[point.y][point.x] += 1;
    if field[point.y][point.x] > 9 {
        field[point.y][point.x] = 0;
        flashes.push_back(point);
        flashed.insert(point);
    }
}
