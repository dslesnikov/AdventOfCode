use std::{
    fmt::{Display, Write},
    iter::repeat,
};

use crate::FromLines;

#[derive(Clone, Copy, PartialEq, Eq)]
enum Terrain {
    Air,
    Rock,
    Sand,
}

impl Display for Terrain {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        match self {
            Terrain::Air => f.write_char('.'),
            Terrain::Rock => f.write_char('#'),
            Terrain::Sand => f.write_char('O'),
        }
    }
}

#[derive(Clone)]
struct Map {
    map: Vec<Vec<Terrain>>,
}

impl Map {
    fn width(&self) -> usize {
        self.map[0].len()
    }

    fn height(&self) -> usize {
        self.map.len()
    }
}

pub(crate) struct Solution {
    map: Map,
    min_x: usize,
}

impl FromLines for Solution {
    fn from_lines<Iter: Iterator<Item = String>>(lines: Iter) -> Self {
        let chains: Vec<Vec<(usize, usize)>> = lines
            .map(|line| {
                line.split(" -> ")
                    .map(|pair| {
                        let split: Vec<&str> = pair.split(',').collect();
                        let x = split[0].parse().unwrap();
                        let y = split[1].parse().unwrap();
                        (x, y)
                    })
                    .collect()
            })
            .collect();
        let mut min_x = usize::MAX;
        let mut min_y = usize::MAX;
        let mut max_x = usize::MIN;
        let mut max_y = usize::MIN;
        for chain in chains.iter() {
            for pair in chain {
                min_x = std::cmp::min(min_x, pair.0);
                max_x = std::cmp::max(max_x, pair.0);
                min_y = std::cmp::min(min_y, pair.1);
                max_y = std::cmp::max(max_y, pair.1);
            }
        }
        let width = max_x - min_x + 1;
        let mut map: Vec<Vec<Terrain>> = Vec::new();
        for _ in 0..=max_y {
            let row = repeat(Terrain::Air).take(width as usize).collect();
            map.push(row);
        }
        for chain in chains {
            let length = chain.len();
            for i in 1..length {
                let previous = chain[i - 1];
                let current = chain[i];
                if previous.0 != current.0 {
                    let min = std::cmp::min(previous.0, current.0);
                    let max = std::cmp::max(previous.0, current.0);
                    for x in min..=max {
                        let x = x - min_x;
                        let y = current.1;
                        map[y][x] = Terrain::Rock;
                    }
                }
                if previous.1 != current.1 {
                    let min = std::cmp::min(previous.1, current.1);
                    let max = std::cmp::max(previous.1, current.1);
                    for y in min..=max {
                        let x = current.0 - min_x;
                        map[y][x] = Terrain::Rock;
                    }
                }
            }
        }
        let map = Map { map };
        Self { map, min_x }
    }
}

impl Display for Map {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        for row in self.map.iter() {
            for item in row {
                f.write_fmt(format_args!("{}", item)).unwrap();
            }
            f.write_str("\n").unwrap();
        }
        Ok(())
    }
}

enum SandFallingState {
    Down,
    Left,
    Right,
    IntoVoid,
    Stationary,
}

#[derive(Clone, Copy)]
struct Point {
    x: usize,
    y: usize,
}

impl Point {
    fn travel(&mut self, state: SandFallingState) {
        match state {
            SandFallingState::Down => self.y += 1,
            SandFallingState::Left => {
                self.y += 1;
                self.x -= 1;
            }
            SandFallingState::Right => {
                self.y += 1;
                self.x += 1;
            }
            _ => {}
        }
    }

    fn get_state(&self, map: &Map) -> SandFallingState {
        if self.y == map.height() - 1 {
            return SandFallingState::IntoVoid;
        }
        if map.map[self.y + 1][self.x] == Terrain::Air {
            return SandFallingState::Down;
        }
        if self.x == 0 {
            return SandFallingState::IntoVoid;
        }
        if map.map[self.y + 1][self.x - 1] == Terrain::Air {
            return SandFallingState::Left;
        }
        if self.x == map.width() - 1 {
            return SandFallingState::IntoVoid;
        }
        if map.map[self.y + 1][self.x + 1] == Terrain::Air {
            return SandFallingState::Right;
        }
        return SandFallingState::Stationary;
    }
}

impl crate::Solution for Solution {
    const DAY: u32 = 14;

    fn solve_first_part(&self) -> String {
        let mut map = self.map.clone();
        let mut sand_units = 0;
        'generating_sand: loop {
            sand_units += 1;
            let mut point = Point {
                x: 500 - self.min_x,
                y: 0,
            };
            loop {
                let state = point.get_state(&map);
                if let SandFallingState::IntoVoid = state {
                    break 'generating_sand;
                }
                if let SandFallingState::Stationary = state {
                    break;
                }
                point.travel(state);
            }
            map.map[point.y][point.x] = Terrain::Sand;
        }
        (sand_units - 1).to_string()
    }

    fn solve_second_part(&self) -> String {
        let mut map = self.map.clone();
        extend(&mut map);
        let mut sand_units = 0;
        'generating_sand: loop {
            sand_units += 1;
            let x = 500 - self.min_x + map.height();
            let mut point = Point { x: x, y: 0 };
            loop {
                let state = point.get_state(&map);
                if let SandFallingState::IntoVoid = state {
                    break 'generating_sand;
                }
                if let SandFallingState::Stationary = state {
                    if point.x == x && point.y == 0 {
                        break 'generating_sand;
                    }
                    break;
                }
                point.travel(state);
            }
            map.map[point.y][point.x] = Terrain::Sand;
        }
        sand_units.to_string()
    }
}

fn extend(map: &mut Map) {
    let new_height = map.height() + 2;
    for i in 0..map.map.len() {
        let row = &map.map[i];
        let appendix = repeat(Terrain::Air).take(new_height);
        let new_row: Vec<Terrain> = appendix
            .clone()
            .chain(row.iter().cloned())
            .chain(appendix)
            .collect();
        map.map[i] = new_row;
    }
    let new_width = 2 * new_height + map.width();
    let air_row: Vec<Terrain> = repeat(Terrain::Air).take(new_width).collect();
    let floor: Vec<Terrain> = repeat(Terrain::Rock).take(new_width).collect();
    map.map.push(air_row);
    map.map.push(floor);
}
