use core::fmt;
use std::collections::{HashSet, VecDeque};

use crate::FromLines;

#[derive(Clone, Copy, Hash, PartialEq, Eq)]
struct Coordinates {
    x: usize,
    y: usize,
}

impl fmt::Display for Coordinates {
    fn fmt(&self, f: &mut fmt::Formatter<'_>) -> fmt::Result {
        f.write_str(&(format!("({}, {})", self.x, self.y)))
    }
}

pub(crate) struct Solution {
    map: Vec<Vec<u8>>,
    start: Coordinates,
    end: Coordinates,
}

impl FromLines for Solution {
    fn from_lines<Iter: Iterator<Item = String>>(lines: Iter) -> Self {
        let mut start: Option<Coordinates> = None;
        let mut end: Option<Coordinates> = None;
        let map: Vec<Vec<u8>> = lines
            .enumerate()
            .map(|(row, line)| {
                line.chars()
                    .enumerate()
                    .map(|(col, ch)| {
                        if ch == 'S' {
                            start = Some(Coordinates { x: col, y: row });
                            return 0;
                        }
                        if ch == 'E' {
                            end = Some(Coordinates { x: col, y: row });
                            return 25;
                        }
                        return ch as u8 - 'a' as u8;
                    })
                    .collect()
            })
            .collect();
        Self {
            map,
            start: start.unwrap(),
            end: end.unwrap(),
        }
    }
}

impl crate::Solution for Solution {
    const DAY: u32 = 12;

    fn solve_first_part(&self) -> String {
        let result = solve_sssp(&self.map, self.start, self.end);
        if let Some(length) = result {
            return length.to_string();
        }
        panic!("Unable to find route!")
    }

    fn solve_second_part(&self) -> String {
        let mut min_length = u32::MAX;
        for row in 0..self.map.len() {
            for col in 0..self.map[row].len() {
                if self.map[row][col] != 0 {
                    continue;
                }
                let start = Coordinates { x: col, y: row };
                let result = solve_sssp(&self.map, start, self.end);
                if let Some(length) = result {
                    min_length = std::cmp::min(min_length, length);
                }
            }
        }
        min_length.to_string()
    }
}

fn solve_sssp(map: &Vec<Vec<u8>>, start: Coordinates, end: Coordinates) -> Option<u32> {
    let height = map.len();
    let width = map[0].len();
    let mut queue: VecDeque<(Coordinates, u32)> = VecDeque::new();
    let mut visited: HashSet<Coordinates> = HashSet::new();
    visited.insert(start);
    queue.push_back((start, 0));
    let mut result = None;
    while !queue.is_empty() {
        let current = queue.pop_front().unwrap();
        if current.0 == end {
            result = Some(current.1);
            break;
        }
        let prev_value = map[current.0.y][current.0.x];
        let neighbours = get_neighbours(&current.0, height, width);
        for neighbour in neighbours {
            let map_value = map[neighbour.y][neighbour.x];
            if !visited.contains(&neighbour) && map_value <= prev_value + 1 {
                let new_length = current.1 + 1;
                queue.push_back((neighbour, new_length));
                visited.insert(neighbour);
            }
        }
    }
    result
}

fn get_neighbours(position: &Coordinates, height: usize, width: usize) -> Vec<Coordinates> {
    let mut neighbours = Vec::with_capacity(4);
    if position.x > 0 {
        neighbours.push(Coordinates {
            x: position.x - 1,
            y: position.y,
        });
    }
    if position.y > 0 {
        neighbours.push(Coordinates {
            x: position.x,
            y: position.y - 1,
        });
    }
    if position.x < width - 1 {
        neighbours.push(Coordinates {
            x: position.x + 1,
            y: position.y,
        });
    }
    if position.y < height - 1 {
        neighbours.push(Coordinates {
            x: position.x,
            y: position.y + 1,
        });
    }
    neighbours
}
