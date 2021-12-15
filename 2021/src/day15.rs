use std::collections::{BinaryHeap, HashMap, HashSet};

use crate::parsing::{FromLines, InputParser};

#[derive(PartialEq, Eq, Hash, Clone, Copy)]
struct Point {
    row: usize,
    col: usize,
}

#[derive(PartialEq, Eq)]
struct Distance {
    point: Point,
    distance: u32,
}

impl Ord for Distance {
    fn cmp(&self, other: &Self) -> std::cmp::Ordering {
        other.distance.cmp(&self.distance)
    }
}

impl PartialOrd for Distance {
    fn partial_cmp(&self, other: &Self) -> Option<std::cmp::Ordering> {
        other.distance.partial_cmp(&self.distance)
    }
}

pub struct Solution {
    field: Vec<Vec<u8>>,
}

impl FromLines for Solution {
    fn new(lines: Vec<String>) -> Self {
        let field = lines
            .iter()
            .filter(|&line| !line.is_empty())
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
    const DAY: i32 = 15;

    fn create() -> Self {
        InputParser::from_lines()
    }

    fn solve_first_part(&self) -> String {
        let distance = Self::get_distance(&self.field);
        distance.to_string()
    }

    fn solve_second_part(&self) -> String {
        let original_height = self.field.len();
        let original_width = self.field[0].len();
        let mut actual_field: Vec<Vec<u8>> = Vec::new();
        for row_increase in 0..5 {
            for row_index in 0..original_height {
                actual_field.push(Vec::new());
                for col_increase in 0..5usize {
                    let actual_row_index = row_index + row_increase * original_height;
                    for col_index in 0..original_width {
                        let value = self.field[row_index][col_index] as usize;
                        let mut new_value = value + row_increase + col_increase;
                        if new_value > 9 {
                            new_value = new_value % 9;
                        }
                        actual_field[actual_row_index].push(new_value as u8);
                    }
                }
            }
        }
        let distance = Self::get_distance(&actual_field);
        distance.to_string()
    }
}

impl Solution {
    fn get_distance(field: &Vec<Vec<u8>>) -> u32 {
        let mut heap: BinaryHeap<Distance> = BinaryHeap::new();
        let mut distance_map: HashMap<Point, u32> = HashMap::new();
        let starting_point = Point { col: 0, row: 0 };
        let target = Point {
            row: field.len() - 1,
            col: field[0].len() - 1,
        };
        heap.push(Distance {
            point: starting_point,
            distance: 0,
        });
        let mut visited = HashSet::new();
        while let Some(current) = heap.pop() {
            visited.insert(current.point);
            let current_distance = distance_map.entry(current.point).or_insert(u32::MAX);
            if *current_distance > current.distance {
                *current_distance = current.distance;
            }
            let current_distance = *current_distance;
            if current.point == target {
                break;
            }
            let neighbours = Self::get_neighbours(&field, current.point, &visited);
            for neighbour in neighbours {
                let current_neighbour_distance = distance_map.entry(neighbour).or_insert(u32::MAX);
                let neighbour_distance =
                    current_distance + field[neighbour.row][neighbour.col] as u32;
                if *current_neighbour_distance > neighbour_distance {
                    *current_neighbour_distance = neighbour_distance;
                    heap.push(Distance {
                        point: neighbour,
                        distance: *current_neighbour_distance,
                    });
                }
            }
        }
        distance_map[&target]
    }

    fn get_neighbours(field: &Vec<Vec<u8>>, point: Point, visited: &HashSet<Point>) -> Vec<Point> {
        let mut result = Vec::new();
        if point.col > 0 {
            let mut candidate = point;
            candidate.col -= 1;
            if !visited.contains(&candidate) {
                result.push(candidate);
            }
        }
        if point.row > 0 {
            let mut candidate = point;
            candidate.row -= 1;
            if !visited.contains(&candidate) {
                result.push(candidate);
            }
        }
        if point.col < field[0].len() - 1 {
            let mut candidate = point;
            candidate.col += 1;
            if !visited.contains(&candidate) {
                result.push(candidate);
            }
        }
        if point.row < field.len() - 1 {
            let mut candidate = point;
            candidate.row += 1;
            if !visited.contains(&candidate) {
                result.push(candidate);
            }
        }
        result
    }
}
