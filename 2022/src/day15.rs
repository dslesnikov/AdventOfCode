use std::collections::HashSet;

use once_cell::unsync::Lazy;
use regex::Regex;

use crate::FromLines;

#[derive(Clone, Copy, PartialEq, Eq, Hash)]
struct Point {
    x: i32,
    y: i32,
}

impl Point {
    fn get_distance(&self, another: &Point) -> i32 {
        return (self.x - another.x).abs() + (self.y - another.y).abs();
    }
}

struct SensorPair {
    sensor: Point,
    beacon: Point,
}

#[derive(PartialEq, Eq, Clone, Copy)]
struct Range {
    start: i32,
    end: i32,
}

impl Range {
    const EMPTY: Range = Range {
        start: i32::MAX,
        end: i32::MIN,
    };

    fn length(&self) -> i32 {
        if *self == Self::EMPTY {
            return 0;
        }
        self.end - self.start + 1
    }

    fn intersect(&self, another: &Range) -> Range {
        if self.end < another.start || another.end < self.start {
            return Range::EMPTY;
        }
        let start = std::cmp::max(self.start, another.start);
        let end = std::cmp::min(self.end, another.end);
        return Range { start, end };
    }
}

pub(crate) struct Solution {
    input: Vec<SensorPair>,
}

const INPUT_REGEX: Lazy<Regex> = Lazy::new(|| {
    Regex::new("Sensor at x=(-?\\d+), y=(-?\\d+): closest beacon is at x=(-?\\d+), y=(-?\\d+)")
        .unwrap()
});

impl FromLines for Solution {
    fn from_lines<Iter: Iterator<Item = String>>(lines: Iter) -> Self {
        let pairs: Vec<SensorPair> = lines
            .map(|line| {
                let captures = INPUT_REGEX.captures(&line).unwrap();
                let sensor_x: i32 = captures.get(1).unwrap().as_str().parse().unwrap();
                let sensor_y: i32 = captures.get(2).unwrap().as_str().parse().unwrap();
                let beacon_x: i32 = captures.get(3).unwrap().as_str().parse().unwrap();
                let beacon_y: i32 = captures.get(4).unwrap().as_str().parse().unwrap();
                SensorPair {
                    sensor: Point {
                        x: sensor_x,
                        y: sensor_y,
                    },
                    beacon: Point {
                        x: beacon_x,
                        y: beacon_y,
                    },
                }
            })
            .collect();
        Self { input: pairs }
    }
}

impl crate::Solution for Solution {
    const DAY: u32 = 15;

    fn solve_first_part(&self) -> String {
        const TARGET_ROW: i32 = 2000000;
        let ranges = self.get_covered_ranges(TARGET_ROW);
        let mut result = ranges.iter().fold(0, |acc, range| acc + range.length());
        let mut visited: HashSet<Point> = HashSet::new();
        for pair in self.input.iter() {
            if pair.beacon.y == TARGET_ROW && visited.insert(pair.beacon) {
                result -= 1;
            }
        }
        result.to_string()
    }

    fn solve_second_part(&self) -> String {
        let target = Range {
            start: 0,
            end: 4000000,
        };
        for y in 0..=4000000 {
            let covered = self.get_covered_ranges(y);
            let covered_length = covered
                .iter()
                .fold(0, |acc, range| acc + range.intersect(&target).length());
            if covered_length < target.length() {
                for i in 1..covered.len() {
                    if covered[i].start > covered[i - 1].end
                        && covered[i].start > 0
                        && covered[i].start <= 4000000
                    {
                        let x = (covered[i].start - 1) as u64;
                        let y = y as u64;
                        let frequency = x * 4000000u64 + y;
                        return frequency.to_string();
                    }
                }
            }
        }
        panic!("Incorrect input")
    }
}

impl Solution {
    fn get_covered_ranges(&self, target_row: i32) -> Vec<Range> {
        let mut covered_ranges: Vec<Range> = Vec::new();
        for sensor_pair in self.input.iter() {
            let scanned_radius = sensor_pair.beacon.get_distance(&sensor_pair.sensor);
            let distance_to_target_row = (sensor_pair.sensor.y - target_row).abs();
            if scanned_radius <= distance_to_target_row {
                continue;
            }
            let start = sensor_pair.sensor.x - (scanned_radius - distance_to_target_row);
            let end = sensor_pair.sensor.x + (scanned_radius - distance_to_target_row);
            covered_ranges.push(Range { start, end });
        }
        covered_ranges.sort_by(|left, right| left.start.cmp(&right.start));
        let mut discrete_ranges = Vec::new();
        discrete_ranges.push(covered_ranges[0]);
        let mut discrete_ranges_index = 0;
        for i in 1..covered_ranges.len() {
            let current = discrete_ranges.get_mut(discrete_ranges_index).unwrap();
            let next = covered_ranges[i];
            if next.end <= current.end {
                continue;
            }
            if next.end > current.end && next.start <= current.end {
                current.end = next.end;
                continue;
            }
            discrete_ranges.push(next);
            discrete_ranges_index += 1;
        }
        discrete_ranges
    }
}
